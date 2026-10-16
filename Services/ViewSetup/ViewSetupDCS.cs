using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GmRdsDocsGenerator.Constants;
using GmRdsDocsGenerator.Statics;
using Tecnomatix.Engineering;
using ObjCategories = GmRdsDocsGenerator.Models.CategoryRegistry.ObjCategories;

namespace GmRdsDocsGenerator.Services.ViewSetup
{
    internal class ViewSetupDCS : ViewSetupBase
    {

        protected void BaseDcsSetup()
        {

            var graphicViewer = TxApplication.ViewersManager.GraphicViewer;
            viewStateRegistry.DocumentViewSnapshot.LoadDisplayData();
            HideAll();

            //Reframe on DCS objects

            var mainDcsObjects = viewStateRegistry.Categories[ObjCategories.MainDcs];

            Debug.WriteLine($"{mainDcsObjects.Count()} attached dcs objects found");
            foreach(var item in mainDcsObjects)
            {
                Debug.WriteLine(item.Name);
            }

            DisplayAll(viewStateRegistry.Categories[ObjCategories.MainDcs]);
            TxApplication.RefreshDisplay();
            graphicViewer.ZoomToFit();
            TxApplication.RefreshDisplay();

            graphicViewer.ShowContours = true;

            var robot = viewStateRegistry.Categories[ObjCategories.MainRobotEquipments].FirstOrDefault();

            ((ITxDisplayableObject)robot).RestoreColor();

            foreach (var item in viewStateRegistry.Categories[ObjCategories.MainDcs])
            {
                ((ITxDisplayableObject)item).Color = GMColours.DcsModel;
            }

            DisplayAll(viewStateRegistry.Categories[ObjCategories.MainRobotEquipments]);

            var allRobotEOAT = Services.SimObjectRetrieval.GetRobotAttachments((ITxObjectCollection)robot);

            Debug.WriteLine("Attached robot EOAT found:");
            foreach(var item in allRobotEOAT)
            {
                Debug.WriteLine(item.Name);
            }

            foreach (var item in viewStateRegistry.Categories[ObjCategories.EOAT])
            {
                if (allRobotEOAT.Contains(item)) ((ITxDisplayableObject)item).Display();
            }

            SetupDCSDisplay();
        }

        protected override void Cleanup()
        {
            var graphicViewer = TxApplication.ViewersManager.GraphicViewer;
            graphicViewer.ShowContours = false;

            var manager = TxApplication.CommandsManager;
            //manager.ExecuteCommand("Shaded") ;

        }

        protected override void PerformSetup()
        {
            BaseDcsSetup();
        }

        private void SetupDCSDisplay()
        {
            var dcsObjects = viewStateRegistry.Categories[ObjCategories.MainDcs];
            var manager = TxApplication.CommandsManager;

            //manager.ExecuteCommand("Feature Lines Over Solid");

            foreach (var item in dcsObjects.OfType<ITxDisplayableObject>())
            {
                item.DisplayMode = TxDisplayMode.Wireframe;
                item.DisplayMode = TxDisplayMode.Shaded;
            }
        }
    }
}
