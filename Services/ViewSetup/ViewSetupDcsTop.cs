using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tecnomatix.Engineering;

namespace GmRdsDocsGenerator.Services.ViewSetup
{
    internal class ViewSetupDcsTop : ViewSetupDCS
    {
        protected override void PerformSetup()
        {
            var viewer = TxApplication.ViewersManager.GraphicViewer;
            var robotContainer = viewStateRegistry.Categories[Models.CategoryRegistry.ObjCategories.MainRobotContainers].FirstOrDefault();
            var robot = ((ITxObjectCollection)robotContainer).GetAllDescendants(new TxTypeFilter(typeof(ITxRobot))).FirstOrDefault();
            var robotOrientation = new TxTransformation(((ITxLocatableObject)robot).AbsoluteLocation);
            var origin = robotOrientation.Translation;
            viewer.CurrentCamera = new TxCamera(
                origin,                                                         //Reference point
                origin + robotOrientation.RotateAndScale(new TxVector(0, 0, 3000)), //Position: above, along robot Z
                new TxVector(1, 0, 0));       //Up vector: robot front at bottom of image
            BaseDcsSetup();
        }
    }
}
