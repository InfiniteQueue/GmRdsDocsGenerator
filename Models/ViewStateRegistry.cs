using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GmRdsDocsGenerator.Services;
using Tecnomatix.Engineering;
using ObjCategories = GmRdsDocsGenerator.Models.CategoryRegistry.ObjCategories;

namespace GmRdsDocsGenerator.Models
{
    internal class ViewStateRegistry
    {
        public TxSnapshot OriginalViewState { get; set; }
        public TxSnapshot DocumentViewState { get; set; }

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

        private void _categories_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OverwriteDocumentViewStateWithCurrentView();
        }

        public ViewStateRegistry() { }

        public void GenerateDefaultDocumentViewState(SimObjectsModel mainObjects)
        {
            EnsureOriginalViewStateSet();

            Debug.WriteLine("Generating document view state");

            var objectLists = SimObjectScanLists.GenerateNew();

            Categories = new CategoryRegistry();

            Categories.AssignItems(mainObjects.DcsObjects.OfType<ITxDisplayableObject>(), ObjCategories.MainDcs);
            Categories.AssignItem((ITxDisplayableObject)mainObjects.Robot, ObjCategories.MainRobot);
            Categories.AssignItems(objectLists.AllRobots.OfType<ITxDisplayableObject>().Where(x => x != mainObjects.Robot), ObjCategories.OtherRobots);
            Categories.AssignItems(objectLists.AllEOAT.OfType<ITxDisplayableObject>(), ObjCategories.EOAT);
            Categories.AssignItems(objectLists.Fencing.OfType<ITxDisplayableObject>(), ObjCategories.Fencing);
            Categories.AssignItems(objectLists.AllProduct.OfType<ITxDisplayableObject>(), ObjCategories.Product);

            foreach (var item in objectLists.InbuiltRobotDCS.OfType<ITxDisplayableObject>()) item.Blank();

            DocumentViewState = TxApplication.ActiveDocument.PhysicalRoot.CreateSnapshot(new TxSnapshotCreationData("DocumentView"));

        }
        public void EnsureOriginalViewStateSet()
        {
            if (OriginalViewState == null)
            {
                Debug.WriteLine("Recording original view state");
                OriginalViewState = TxApplication.ActiveDocument.PhysicalRoot.CreateSnapshot(new TxSnapshotCreationData("OriginalView"));
            }
        }

        public void OverwriteDocumentViewStateWithCurrentView()
        {
            Debug.WriteLine("Saving document view snapshot");
            DocumentViewState = TxApplication.ActiveDocument.PhysicalRoot.CreateSnapshot(new TxSnapshotCreationData("DocumentView"));
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
