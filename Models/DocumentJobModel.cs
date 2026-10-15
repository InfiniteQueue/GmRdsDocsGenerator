using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GmRdsDocsGenerator.Services;
using Tecnomatix.Engineering;

namespace GmRdsDocsGenerator.Models
{
    /// <summary>
    /// Groups all relevant information for generating a single RDS powerpoint document for one robot
    /// </summary>
    internal class DocumentJobModel
    {
        public DocumentJobModel()
        {
            SimObjects.PropertyChanged += this.SimObjects_PropertyChanged;
        }

        private void SimObjects_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SimObjectsModel.Robot))
            {
                if (SimObjects.Robot is ITxRobot robot)
                {
                    MainData.RobotName = robot.Name;
                    if (SimDataRetrieval.GetFileName(robot as ITxStorable) is string modelName && !string.IsNullOrWhiteSpace(modelName)) MainData.RobotModel = modelName;
                    ;

                        if (SimObjectRetrieval.GetGrippers((ITxObjectCollection)robot).FirstOrDefault() is ITxDevice foundGripper) SimObjects.Gripper = foundGripper;

                    SimObjects.DcsObjects = new TxObjectList(); ;
                    foreach (var item in SimObjectRetrieval.GetDCSObjects((ITxObjectCollection)robot)) SimObjects.DcsObjects.Add(item);
                }
            }
        }

        public MainDataModel MainData { get; set; } = new MainDataModel();
        public SimObjectsModel SimObjects { get; set; } = new SimObjectsModel();
        public ViewStateRegistry ViewStates { get; set; } = new ViewStateRegistry();
    }
}
