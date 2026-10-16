using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GmRdsDocsGenerator.Services;
using Tecnomatix.Engineering;

namespace GmRdsDocsGenerator.Models
{
    /// <summary>  </summary>
    public class SimObjectScanLists
    {
        protected SimObjectScanLists() { }

        public static SimObjectScanLists _GetNewBlank() => new SimObjectScanLists();

        public static SimObjectScanLists GenerateNew()
        {
            return SimObjectCategoryScan.GetNewScan();
        }

        public TxObjectList<ITxRobot> AllRobots { get; set; } = new TxObjectList<ITxRobot>();
        public TxObjectList<ITxDevice> AllEOAT { get; set; } = new TxObjectList<ITxDevice>();
        public TxObjectList<ITxComponent> AllProduct { get; set; } = new TxObjectList<ITxComponent>();
        public TxObjectList<ITxObject> AllDCSObjects { get; set; } = new TxObjectList<ITxObject>();
        public TxObjectList<ITxObject> Fencing { get; set; } = new TxObjectList<ITxObject>();
        public TxObjectList<ITxComponent> InbuiltRobotDCS { get; set; } = new TxObjectList<ITxComponent>();
        public TxObjectList<ITxComponent> AllRisers { get; set; } = new TxObjectList<ITxComponent>();
        public TxObjectList<ITxObjectCollection> AllRobotContainers { get; set; } = new TxObjectList<ITxObjectCollection>();
    }
}
