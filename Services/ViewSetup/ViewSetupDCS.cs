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

            var robot = ((ITxObjectCollection)viewStateRegistry.Categories[ObjCategories.MainRobotContainers].FirstOrDefault()).GetAllDescendants(new TxTypeFilter(typeof(TxRobot))).OfType<TxRobot>().FirstOrDefault();
            var homePose = (TxPose)robot.PoseList.FirstOrDefault(x => x.Name.ToLower() == "home");
            robot.CurrentPose = homePose.PoseData;
            ;

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

            var robotContainer = viewStateRegistry.Categories[ObjCategories.MainRobotContainers].FirstOrDefault();

            ((ITxDisplayableObject)robotContainer).RestoreColor();

            foreach (var item in viewStateRegistry.Categories[ObjCategories.MainDcs])
            {
                ((ITxDisplayableObject)item).Color = GMColours.DcsModel;
            }

            DisplayAll(viewStateRegistry.Categories[ObjCategories.MainRobotContainers]);

            //RestoreColor on the robot container above also reverts its riser and tool changer, so reapply their category colours
            //Only the main robot's riser is displayed, as part of its container; other robots' risers stay hidden
            foreach (var riser in viewStateRegistry.Categories[ObjCategories.Risers].OfType<ITxDisplayableObject>())
            {
                riser.Color = Models.CategoryRegistry.CategoryColors[ObjCategories.Risers];
            }
            foreach (var toolChanger in viewStateRegistry.Categories[ObjCategories.MainToolChanger].OfType<ITxDisplayableObject>())
            {
                toolChanger.Color = Models.CategoryRegistry.CategoryColors[ObjCategories.MainToolChanger];
            }

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

            BlankAll(TxApplication.ActiveDocument.PhysicalRoot.GetAllDescendants(new TxTypeFilter(typeof(TxFrame))));
            HideCables();

        }

        protected override void Cleanup()
        {
            var graphicViewer = TxApplication.ViewersManager.GraphicViewer;
            graphicViewer.ShowContours = false;

            var manager = TxApplication.CommandsManager;
            manager.ExecuteCommand("GraphicViewer.ShadedMode");

        }

        protected override void PerformSetup()
        {
            BaseDcsSetup();
        }

        private void SetupDCSDisplay()
        {
            var dcsObjects = viewStateRegistry.Categories[ObjCategories.MainDcs];
            var manager = TxApplication.CommandsManager;

            manager.ExecuteCommand("GraphicViewer.FLShadedView");

            foreach (var item in dcsObjects.OfType<ITxDisplayableObject>())
            {
                item.DisplayMode = TxDisplayMode.Wireframe;
                item.DisplayMode = TxDisplayMode.Shaded;
            }
        }
    }
}
