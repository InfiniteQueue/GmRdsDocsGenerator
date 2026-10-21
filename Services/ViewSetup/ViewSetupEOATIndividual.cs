using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GmRdsDocsGenerator.Statics;
using Tecnomatix.Engineering;
using static GmRdsDocsGenerator.Models.CategoryRegistry;

namespace GmRdsDocsGenerator.Services.ViewSetup
{
    internal class ViewSetupEOATIndividual : ViewSetupBase
    {
        public ViewSetupEOATIndividual()
        {
        }
        public int EOATIndex { get; set;  }
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

            var robotEquipment = viewStateRegistry.Categories[Models.CategoryRegistry.ObjCategories.MainRobotContainers].FirstOrDefault();

            var eoat = SimObjectRetrieval.GetEOAT((ITxObjectCollection)robotEquipment).ToArray()[EOATIndex];
            ((ITxDisplayableObject)eoat).Display();

            BlankAll(TxApplication.ActiveDocument.PhysicalRoot.GetAllDescendants(new TxTypeFilter(typeof(TxFrame))));
            HideCables();
            viewer.ZoomToFit();


            var disp = (ITxDisplayableObject)robotEquipment;
            disp.Display();
            disp.RestoreColor();


            BlankAll(TxApplication.ActiveDocument.PhysicalRoot.GetAllDescendants(new TxTypeFilter(typeof(TxFrame))));
            HideCables();

            var manager = TxApplication.CommandsManager;
            manager.ExecuteCommand("GraphicViewer.FLShadedView");

            RecolorCategory(ObjCategories.Risers);
            RecolorCategory(ObjCategories.EOAT);

        }

        private static void OrientCameraIso(TxGraphicViewer viewer, ITxObject robot)
        {
            var robotOrientation = new TxTransformation(((ITxLocatableObject)robot).AbsoluteLocation);
            var origin = robotOrientation.Translation;
            var forwardVector = robotOrientation.RotateAndScale(new TxVector(-1, 1, -1) / Math.Sqrt(3));
            var upVector = robotOrientation.RotateAndScale(new TxVector(-1, 1, 2) / Math.Sqrt(6)); // robot Z projected perpendicular to forward
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
    }
}
