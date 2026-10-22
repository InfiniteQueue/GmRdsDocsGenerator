using System;
using System.Collections.Generic;
using System.IO;
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
    internal class ViewSetupCPCAngledIso : ViewSetupBase
    {
        public int ClockwiseTurns { get; set; }

        protected override void PerformSetup()
        {
            viewStateRegistry.DocumentViewSnapshot.LoadDisplayData();


            HideAll();
            var viewer = TxApplication.ViewersManager.GraphicViewer;
            var robotContainer = viewStateRegistry.Categories[Models.CategoryRegistry.ObjCategories.MainRobotContainers].FirstOrDefault();
            var robot = ((ITxObjectCollection)robotContainer).GetAllDescendants(new TxTypeFilter(typeof(ITxRobot))).FirstOrDefault() as TxRobot;

            var homePose = (TxPose)robot.PoseList.FirstOrDefault(x => x.Name.ToLower() == "home");
            robot.CurrentPose = homePose.PoseData;

            OrientCameraIso(viewer, robot);

            var allCPC = viewStateRegistry.Categories[ObjCategories.CPCRestrictedZones]
                .Concat(viewStateRegistry.Categories[ObjCategories.CPCRestrictedZonesCSC])
                .Concat(viewStateRegistry.Categories[ObjCategories.CPCSharedZones])
                .Concat(viewStateRegistry.Categories[ObjCategories.CPCSharedZonesCSC]);

            DisplayAll(allCPC);
            DisplayAll(viewStateRegistry.Categories[ObjCategories.MainRobotContainers]);
            DisplayAll(viewStateRegistry.Categories[ObjCategories.MainDcs]);

            //Todo: Examine why the cpc zones get converted to wireframes
            //foreach (var item in allCPC) ((ITxDisplayableObject)item).DisplayMode = TxDisplayMode.Shaded;

            viewer.ZoomToFit();

            DisplayAll(viewStateRegistry.Categories[ObjCategories.Tooling]);
            DisplayAll(viewStateRegistry.Categories[ObjCategories.OtherRobotsContainers]);
            DisplayAll(viewStateRegistry.Categories[ObjCategories.Fencing]);

            //foreach (var item in viewStateRegistry.Categories[ObjCategories.MainRobotContainers]) ((ITxDisplayableObject)item).RestoreColor();
            viewStateRegistry.Categories.ApplyCategoryColor(ObjCategories.EOAT);
            viewStateRegistry.Categories.ApplyCategoryColor(ObjCategories.Risers);

            ColorOtherRobotsGrey();
            HideCables();

        }

        private void OrientCameraIso(TxGraphicViewer viewer, ITxObject robot)
        {
            //++, +-, --, -+, looping for X and Y respectively
            var xMult = Math.Pow(-1, (ClockwiseTurns + 1)/2); //Integer division
            var yMult = Math.Pow(-1, ClockwiseTurns / 2);

            var robotContainer = viewStateRegistry.Categories[Models.CategoryRegistry.ObjCategories.MainRobotContainers].FirstOrDefault();
            var robotOrientation = new TxTransformation(((ITxLocatableObject)robot).AbsoluteLocation);
            var origin = robotOrientation.Translation;
            var forwardVector = robotOrientation.RotateAndScale(new TxVector(-1 * xMult, -1 * yMult, -1) / Math.Sqrt(3));
            var upVector = robotOrientation.RotateAndScale(new TxVector(-1 * xMult, -1 * yMult, 2) / Math.Sqrt(6)); // robot Z projected perpendicular to forward
            viewer.CurrentCamera = new TxCamera(
                origin,                          //Reference point
                origin - forwardVector * 3000,   //Position
                upVector);                       //Up vector
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
