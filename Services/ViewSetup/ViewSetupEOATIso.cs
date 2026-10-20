using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tecnomatix.Engineering;

namespace GmRdsDocsGenerator.Services.ViewSetup
{
    internal class ViewSetupEOATIso : ViewSetupBase
    {
        protected override void PerformSetup()
        {
            var viewer = TxApplication.ViewersManager.GraphicViewer;
            var robotContainer = viewStateRegistry.Categories[Models.CategoryRegistry.ObjCategories.MainRobotContainers].FirstOrDefault();
            var robot = ((ITxObjectCollection)robotContainer).GetAllDescendants(new TxTypeFilter(typeof(ITxRobot))).FirstOrDefault();
            OrientCameraIso(viewer, robot);

            HideAll();

            DisplayAll( viewStateRegistry.Categories[Models.CategoryRegistry.ObjCategories.MainRobotContainers]);
            var robotEquipment = viewStateRegistry.Categories[Models.CategoryRegistry.ObjCategories.MainRobotContainers].FirstOrDefault();

            var gripper = SimObjectRetrieval.GetEOAT((ITxObjectCollection)robotEquipment);
            ((ITxDisplayableObject)gripper).Display();
            
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
        }
    }
}
