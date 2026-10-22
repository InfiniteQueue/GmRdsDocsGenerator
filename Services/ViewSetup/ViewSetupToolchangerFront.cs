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
    internal class ViewSetupToolchangerFront : ViewSetupBase
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

            OrientCameraFront(viewer, robot);


            var toolchanger = viewStateRegistry.Categories[ObjCategories.MainRobotToolChanger].FirstOrDefault();
            ((ITxDisplayableObject)toolchanger).Display();
            ((ITxDisplayableObject)toolchanger).RestoreColor();

            viewer.ZoomToFit();


            var disp = (ITxDisplayableObject)robotContainer;
            disp.Display();
            disp.RestoreColor();
            foreach (var eoat in viewStateRegistry.Categories[ObjCategories.EOAT]) ((ITxDisplayableObject)eoat).Blank();

            BlankAll(TxApplication.ActiveDocument.PhysicalRoot.GetAllDescendants(new TxTypeFilter(typeof(TxFrame))));
            BlankAll(viewStateRegistry.Categories[ObjCategories.MainEOATToolChangers]);
            HideCables();


            var manager = TxApplication.CommandsManager;
            manager.ExecuteCommand("GraphicViewer.FLShadedView");

            RecolorCategory(ObjCategories.Risers);

        }

        private static void OrientCameraFront(TxGraphicViewer viewer, ITxObject robot)
        {
            var robotOrientation = new TxTransformation(((ITxLocatableObject)robot).AbsoluteLocation);
            var origin = robotOrientation.Translation;
            var forwardVector = robotOrientation.RotateAndScale(new TxVector(-1, 0,0));
            var upVector = new TxVector(0,0,1); // robot Z projected perpendicular to forward
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
