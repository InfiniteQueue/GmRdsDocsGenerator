using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GmRdsDocsGenerator.Models;
using Tecnomatix.Engineering;

namespace GmRdsDocsGenerator.Services
{
    internal static class SimObjectCategoryScan
    {
        public static SimObjectScanLists GetNewScan()
        {
            var newScan = SimObjectScanLists._GetNewBlank();
            var doc = TxApplication.ActiveDocument;
            var root = doc.PhysicalRoot;
            GetRobots(newScan, root);
            GetGrippers(newScan);
            GetPanels(newScan);
            GetDcsObjects(newScan);
            GetFencing(newScan, root);
            GetRobotInbuiltDcsObjects(newScan);
            return newScan;
        }

        private static void GetDcsObjects(SimObjectScanLists newScan)
        {
            var attachedDcsObjects = new TxObjectList<ITxObject>(newScan.AllRobots.SelectMany(x => SimObjectRetrieval.GetDCSObjects((ITxObjectCollection)x)));
            Debug.WriteLine($"{attachedDcsObjects.Count} Attached Dcs objects found");
            newScan.AllDCSObjects = attachedDcsObjects;
        }

        private static void GetPanels(SimObjectScanLists newScan)
        {
            var panelObjects = new TxObjectList<ITxComponent>(newScan.AllRobots.SelectMany(x => SimObjectRetrieval.GetPanels((ITxObjectCollection)x)));
            Debug.WriteLine($"{panelObjects.Count} product objects found");
            newScan.AllProduct = panelObjects;
        }

        private static void GetGrippers(SimObjectScanLists newScan)
        {
            var EOATObjects = new TxObjectList<ITxDevice>(newScan.AllRobots.SelectMany(x => SimObjectRetrieval.GetGrippers((ITxObjectCollection)x)));
            Debug.WriteLine($"{EOATObjects.Count} EOAT objects found");
            newScan.AllEOAT = EOATObjects;
        }

        private static void GetRobots(SimObjectScanLists newScan, TxPhysicalRoot root)
        {
            var robotObjects = new TxObjectList<ITxRobot>(root.GetAllDescendants(new TxTypeFilter(typeof(ITxRobot))).OfType<ITxRobot>());
            Debug.WriteLine($"{robotObjects.Count} robot objects found");
            newScan.AllRobots = robotObjects;
        }
        static void GetFencing(SimObjectScanLists newScan, TxPhysicalRoot root)
        {
            var fencingObjects = new TxObjectList<ITxObject>(root.GetAllDescendants(new TxNoTypeFilter()).Where(x => { var name = x.Name.ToLower(); return name.Contains("fence") || name.Contains("fencing"); }));
            Debug.WriteLine($"{fencingObjects.Count} fencing objects found");
            newScan.Fencing = fencingObjects;
        }
        static void GetRobotInbuiltDcsObjects(SimObjectScanLists newScan)
        {
            var allInbuilt = newScan.AllRobots.SelectMany(x => SimObjectRetrieval.GetInbuiltRobotDcs(x));
            Debug.WriteLine($"{allInbuilt.Count()} inbuilt dcs objects found");
            newScan.InbuiltRobotDCS = new TxObjectList<ITxComponent>(allInbuilt);
        }
        static void GetRobotEquipments(SimObjectScanLists newScan)
        {
            var equipments = newScan.AllRobots.Select(x => Services.SimObjectRetrieval.GetRobotEquipment(x)).OfType<ITxComponent>();
            Debug.WriteLine($"{equipments.Count()} robot equipments found");
            newScan.AllRobotEquipments = equipments;
        }
    }
}
