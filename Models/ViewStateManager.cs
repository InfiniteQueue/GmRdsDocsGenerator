using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GmRdsDocsGenerator.Services;
using GmRdsDocsGenerator.Services.ViewSetup;
using Tecnomatix.Engineering;
using ObjCategories = GmRdsDocsGenerator.Models.CategoryRegistry.ObjCategories;

namespace GmRdsDocsGenerator.Models
{
    internal class ViewStateManager
    {
        public TxSnapshot OriginalViewSnapshot { get; set; }
        public TxSnapshot DocumentViewSnapshot { get; set; }

        CategoryRegistry _categories;
        public CategoryRegistry Categories
        {
            get => _categories;
            private set
            {
                if (_categories != null) _categories.CollectionChanged -= this._categories_CollectionChanged;
                _categories = value;
                if (_categories != null) _categories.CollectionChanged += this._categories_CollectionChanged;

            }
        }

        ViewSetupBase activeView;
        /// <summary> Disposes the previous view, then sets up the assigned one. Assign null to return to the document view </summary>
        public ViewSetupBase ActiveView
        {
            get => activeView;
            set
            {
                activeView?.Dispose();
                activeView = value;
                activeView?.Setup();
            }
        }

        private void _categories_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OverwriteDocumentViewStateWithCurrentView();
        }

        public ViewStateManager() { }

        public void GenerateDefaultDocumentViewState(SimObjectsModel mainObjects)
        {
            EnsureOriginalViewStateSet();

            Debug.WriteLine("Generating document view state");

            var objectLists = SimObjectScanLists.GenerateNew();

            Categories = new CategoryRegistry();

            var mainRobotContainer = SimObjectRetrieval.GetRobotContainer(mainObjects.Robot);
            Categories.AssignItems(mainObjects.DcsObjects.OfType<ITxDisplayableObject>(), ObjCategories.MainDcs);
            Categories.AssignItem((ITxDisplayableObject)mainRobotContainer, ObjCategories.MainRobotContainers);
            Categories.AssignItem((ITxDisplayableObject)mainObjects.Robot, ObjCategories.MainTxRobot);
            Categories.AssignItems(objectLists.AllTxRobots.Where(x => x != mainObjects.Robot).OfType<ITxDisplayableObject>(), ObjCategories.OtherTxRobots);
            Categories.AssignItems(objectLists.AllRobotContainers.OfType<ITxDisplayableObject>().Where(x => x != mainRobotContainer), ObjCategories.OtherRobotsContainers);
            Categories.AssignItems(objectLists.AllEOAT.OfType<ITxDisplayableObject>(), ObjCategories.EOAT);
            //Risers sit inside robot containers, so they're assigned after them to override the container colour
            Categories.AssignItems(objectLists.AllRisers.OfType<ITxDisplayableObject>(), ObjCategories.Risers);
            if (mainObjects.Riser is ITxDisplayableObject mainRiser) Categories.AssignItem(mainRiser, ObjCategories.Risers);
            //The tool changer also sits inside the robot container, so it's assigned after it
            if (mainObjects.ToolChanger is ITxDisplayableObject mainToolChanger) Categories.AssignItem(mainToolChanger, ObjCategories.MainRobotToolChanger);
            Categories.AssignItems(mainObjects.EoatToolChangers.OfType<ITxDisplayableObject>(), ObjCategories.MainEOATToolChangers);
            Categories.AssignItems(objectLists.Fencing.OfType<ITxDisplayableObject>(), ObjCategories.Fencing);
            Categories.AssignItems(objectLists.AllProduct.OfType<ITxDisplayableObject>(), ObjCategories.Product);
            Categories.AssignItems(objectLists.AllTooling.OfType<ITxDisplayableObject>(), ObjCategories.Tooling);

            Categories.AssignItems(mainObjects.CPCRestrictedZones.OfType<ITxDisplayableObject>(), ObjCategories.CPCRestrictedZones);
            Categories.AssignItems(mainObjects.CPCRestrictedZonesCSC.OfType<ITxDisplayableObject>(), ObjCategories.CPCRestrictedZonesCSC);
            Categories.AssignItems(mainObjects.CPCSharedZones.OfType<ITxDisplayableObject>(), ObjCategories.CPCSharedZones);
            Categories.AssignItems(mainObjects.CPCSharedZonesCSC.OfType<ITxDisplayableObject>(), ObjCategories.CPCSharedZonesCSC);

            foreach (var item in TxApplication.ActiveDocument.OperationRoot.GetDirectDescendants(new TxTypeFilter(typeof(ITxDisplayableObject))).OfType<ITxDisplayableObject>()) item.Blank();

            foreach (var item in TxApplication.ActiveDocument.PhysicalRoot.GetAllDescendants(new TxTypeFilter(typeof(TxFrame))))
            {
                ((ITxDisplayableObject)item).Blank();
            }

            Debug.WriteLine("Blanking inbuilt dcs Items:");
            foreach (var item in objectLists.InbuiltRobotDCS.OfType<ITxDisplayableObject>())
            {
                Debug.WriteLine(item.Name);
                item.Blank();
            }

            DocumentViewSnapshot = TxApplication.ActiveDocument.PhysicalRoot.CreateSnapshot(new TxSnapshotCreationData("DocumentView"));

        }
        public void EnsureOriginalViewStateSet()
        {
            if (OriginalViewSnapshot == null)
            {
                Debug.WriteLine("Recording original view state");
                OriginalViewSnapshot = TxApplication.ActiveDocument.PhysicalRoot.CreateSnapshot(new TxSnapshotCreationData("OriginalView"));
            }
        }

        public void OverwriteDocumentViewStateWithCurrentView()
        {
            Debug.WriteLine("Saving document view snapshot");
            DocumentViewSnapshot = TxApplication.ActiveDocument.PhysicalRoot.CreateSnapshot(new TxSnapshotCreationData("DocumentView"));
        }

        public void AssignObjectCategory(ITxDisplayableObject dispObj, ObjCategories? category)
        {
            if (category != null)
            {
                Categories.AssignItem(dispObj, category.Value);
            }
            else
            {
                Categories.UnassignItem(this, dispObj);
            }
        }
    }
}
