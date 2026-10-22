using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GmRdsDocsGenerator.Constants;
using GmRdsDocsGenerator.Models;
using GmRdsDocsGenerator.Statics;
using Tecnomatix.Engineering;
using static GmRdsDocsGenerator.Models.CategoryRegistry;

namespace GmRdsDocsGenerator.Services.ViewSetup
{
    internal class ViewSetupCPCPlan : ViewSetupBase
    {
        protected override void PerformSetup()
        {
            viewStateRegistry.DocumentViewSnapshot.LoadDisplayData();


            HideAll();
            var viewer = TxApplication.ViewersManager.GraphicViewer;
            var robotContainer = viewStateRegistry.Categories[Models.CategoryRegistry.ObjCategories.MainRobotContainers].FirstOrDefault();
            var robot = ((ITxObjectCollection)robotContainer).GetAllDescendants(new TxTypeFilter(typeof(ITxRobot))).FirstOrDefault() as TxRobot;

            var homePose = (TxPose)robot.PoseList.FirstOrDefault(x => x.Name.ToLower() == "home");
            robot.CurrentPose = homePose.PoseData;

            OrientCameraTop(viewer, robot);

            DisplayAll(viewStateRegistry.Categories[ObjCategories.RestrictedVolume]);

            viewer.ZoomToFit();

            DisplayAll(viewStateRegistry.Categories[ObjCategories.Tooling]);
            DisplayAll(viewStateRegistry.Categories[ObjCategories.OtherRobotsContainers]);
            DisplayAll(viewStateRegistry.Categories[ObjCategories.MainRobotContainers]);
            DisplayAll(viewStateRegistry.Categories[ObjCategories.Fencing]);

            foreach (var item in viewStateRegistry.Categories[ObjCategories.MainRobotContainers]) ((ITxDisplayableObject)item).RestoreColor();
            viewStateRegistry.Categories.ApplyCategoryColor(ObjCategories.EOAT);
            viewStateRegistry.Categories.ApplyCategoryColor(ObjCategories.Risers);

            ColorOtherRobotsGrey();
            HideCables();

        }

        private void OrientCameraTop(TxGraphicViewer viewer, ITxObject robot)
        {
            var robotContainer = viewStateRegistry.Categories[Models.CategoryRegistry.ObjCategories.MainRobotContainers].FirstOrDefault();
            var robotOrientation = new TxTransformation(((ITxLocatableObject)robot).AbsoluteLocation);
            var origin = robotOrientation.Translation;
            viewer.CurrentCamera = new TxCamera(
                origin,                                                         //Reference point
                origin + robotOrientation.RotateAndScale(new TxVector(0, 0, 3000)), //Position: above, along robot Z
                robotOrientation.RotateAndScale(new TxVector(-1, 0, 0)));       //Up vector: robot front at bottom of image
        }

        protected override void Cleanup()
        {
            var manager = TxApplication.CommandsManager;
            manager.ExecuteCommand("GraphicViewer.ShadedMode");
        }
        void ColorOtherRobotsGrey()
        {
            foreach(var item in viewStateRegistry.Categories[ObjCategories.OtherRobotsContainers])
            {
                var robot = ((ITxObjectCollection)item).GetAllDescendants(new TxTypeFilter(typeof(TxRobot))).FirstOrDefault();
                var robotEquipment = SimObjectRetrieval.StepUpParents(robot).FirstOrDefault(x => x is ITxComponent component && component.IsEquipment);

                if (robotEquipment is ITxDisplayableObject displayable) displayable.Color = GMColours.Gray1;
            }
        }
    }
}
