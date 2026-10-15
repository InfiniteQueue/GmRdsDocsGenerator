using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tecnomatix.Engineering;

namespace GmRdsDocsGenerator.Services.ViewSetup
{
    internal class ViewSetupDcsIso : ViewSetupDCS
    {
        protected override void PerformSetup()
        {
            var viewer = TxApplication.ViewersManager.GraphicViewer;
            var robotEquip = viewStateRegistry.Categories[Models.CategoryRegistry.ObjCategories.MainRobotEquipments].FirstOrDefault();
            var robot = ((ITxObjectCollection)robotEquip).GetAllDescendants(new TxTypeFilter(typeof(ITxRobot))).FirstOrDefault();
            var robotOrientation = new TxTransformation(((ITxLocatableObject)robot).AbsoluteLocation);
            var rootThird = Math.Sqrt(1.0 / 3.0);
            var forwardVector = robotOrientation.Transform(new TxVector(-rootThird, rootThird, -rootThird));
            var upVector = robotOrientation.Transform(new TxVector(-rootThird, rootThird, rootThird));
            viewer.CurrentCamera = new TxCamera(
                robotOrientation.Translation, //Reference point
                robotOrientation.Translation - forwardVector * 3000, //Position
                upVector); //Up vector
            base.PerformSetup();
        }
    }
}
