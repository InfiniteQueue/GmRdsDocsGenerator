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

                    SimObjects.Riser = SimObjectRetrieval.GetRiser(robot);

                    //Fill lists before assigning them, so bound controls receive the complete list when PropertyChanged fires
                    var eoat = new TxObjectList();
                    foreach (var gripper in SimObjectRetrieval.GetGrippers((ITxObjectCollection)robot)) eoat.Add(gripper);
                    SimObjects.EOAT = eoat;

                    var dcsObjects = new TxObjectList();
                    foreach (var item in SimObjectRetrieval.GetDCSObjects((ITxObjectCollection)robot)) dcsObjects.Add(item);
                    SimObjects.DcsObjects = dcsObjects;

                    var panels = new TxObjectList();
                    foreach (var panel in SimObjectRetrieval.GetPanels((ITxObjectCollection)robot)) panels.Add(panel);
                    SimObjects.Panels = panels;

                    var cpcZones = new TxObjectList();
                    foreach (var zone in SimObjectRetrieval.GetCpcZones(robot)) cpcZones.Add(zone);
                    SimObjects.CpcZones = cpcZones;
                }
            }
        }

        public MainDataModel MainData { get; set; } = new MainDataModel();
        public SimObjectsModel SimObjects { get; set; } = new SimObjectsModel();
        public ViewStateManager ViewStates { get; set; } = new ViewStateManager();
    }
}
