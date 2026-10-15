using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tecnomatix.Engineering;

namespace GmRdsDocsGenerator.Services.ViewSetup
{
    internal class ViewSetupDcsSide : ViewSetupDCS
    {
        protected override void PerformSetup()
        {
            var viewer = TxApplication.ViewersManager.GraphicViewer;
            var robotEquip = viewStateRegistry.Categories[Models.CategoryRegistry.ObjCategories.MainRobotEquipments].FirstOrDefault();
            var robot = ((ITxObjectCollection)robotEquip).GetAllDescendants(new TxTypeFilter(typeof(ITxRobot))).FirstOrDefault();
            var robotOrientation = new TxTransformation(((ITxLocatableObject)robot).AbsoluteLocation);
            viewer.CurrentCamera = new TxCamera(
                robotOrientation.Translation, //Reference point
                robotOrientation.Translation + robotOrientation.Transform(new TxVector(0, -3000, 0)), //Position
                new TxVector(0, 0, 1)); //Up vector
            base.PerformSetup();
        }
    }
}
