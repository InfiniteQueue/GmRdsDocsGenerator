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
            GetRobotContainers(newScan);
            GetRisers(newScan);
            GetGrippers(newScan);
            GetPanels(newScan);
            GetDcsObjects(newScan);
            GetFencing(newScan, root);
            GetRobotInbuiltDcsObjects(newScan);
            GetToolingGroups(newScan);
            return newScan;
        }

        private static void GetDcsObjects(SimObjectScanLists newScan)
        {
            var attachedDcsObjects = new TxObjectList<ITxObject>(newScan.AllRobots.SelectMany(x => SimObjectRetrieval.GetDCSObjects((ITxObjectCollection)x)));
            Debug.WriteLine($"{attachedDcsObjects.Count} Attached Dcs objects found");
            foreach (var item in attachedDcsObjects) Debug.WriteLine(item.Name);
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
            var EOATObjects = new TxObjectList<ITxDevice>(newScan.AllRobots.SelectMany(x => SimObjectRetrieval.GetEOAT((ITxObjectCollection)x)));
            Debug.WriteLine($"{EOATObjects.Count} EOAT objects found");
            foreach (var item in EOATObjects) Debug.WriteLine(item.Name);
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
        static void GetRisers(SimObjectScanLists newScan)
        {
            var risers = newScan.AllRobots.Select(x => SimObjectRetrieval.GetRiser(x)).Where(x => x != null).Distinct();
            Debug.WriteLine($"{risers.Count()} risers found");
            newScan.AllRisers = new TxObjectList<ITxComponent>(risers);
        }
        static void GetRobotContainers(SimObjectScanLists newScan)
        {
            var containers = newScan.AllRobots.Select(x => Services.SimObjectRetrieval.GetRobotContainer(x)).OfType<ITxObjectCollection>();
            Debug.WriteLine($"{containers.Count()} robot containers found");
            newScan.AllRobotContainers = new TxObjectList<ITxObjectCollection>(containers);
        }
        static void GetToolingGroups(SimObjectScanLists newScan)
        {
            var tooling = TxApplication.ActiveDocument.PhysicalRoot.GetAllDescendants(new TxTypeFilter(typeof(TxCompoundResource))).Where(x => x.Name == "TOOLING").OfType<TxCompoundResource>();
            newScan.AllTooling = new TxObjectList<TxCompoundResource> (tooling );
        }
    }
}
