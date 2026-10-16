using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tecnomatix.Engineering;
using Tecnomatix.Planning;

namespace GmRdsDocsGenerator.Services
{
    public static class SimObjectRetrieval
    {
        /// <summary> All ItxDevice attachments</summary>
        public static IEnumerable<ITxDevice> GetGrippers(ITxObjectCollection sourceRobot)
        {
            var allAttachments = GetRobotAttachments(sourceRobot);

            var robotGroup = sourceRobot.Collection.Collection;

            IEnumerable<ITxDevice> devices = allAttachments.OfType<ITxDevice>();

            return devices.Where(x => StepUpParents(x).Any(y => y == robotGroup));
        }


        /// <summary>
        /// Components that aren't ItxDevice, are under a collection named 'PRODUCT', and are attached to the robot's toolframe or k7
        /// either directly or through other attachments (e.g. held by the EOAT)
        /// </summary>
        public static ITxComponent[] GetPanels(ITxObjectCollection sourceRobot)
        {
            var directAttachments = GetRobotAttachments(sourceRobot).ToList();
            var allAttachments = directAttachments.Concat(directAttachments.OfType<ITxLocatableObject>().SelectMany(x => GetAllAttachmentDescendants(x)));

            IEnumerable<ITxComponent> ItxComponents = allAttachments.OfType<ITxComponent>();

            var filteredItems = ItxComponents.Where(x => !(x is ITxDevice) && StepUpParents(x).Any(y => y.Name.ToLower() == "product"));

            return filteredItems.Distinct().ToArray();
        }

        static IEnumerable<ITxObjectCollection> StepUpParents(ITxObject child, bool includeSelf = false)
        {
            if (includeSelf && child is ITxObjectCollection) yield return child as ITxObjectCollection;

            var parent = child.Collection;
            while (parent != null && parent.Collection != parent)
            {
                //if (parent is TxPhysicalRoot || parent is TxPhysicalRoot || parent is TxLogicalRoot || parent is TxMfgRoot) break;
                yield return parent;
                parent = parent.Collection as ITxObjectCollection;
            }
        }

        public static IEnumerable<ITxObject> GetDCSObjects(ITxObjectCollection sourceRobot)
        {
            return GetRobotAttachments(sourceRobot).Where(x => StepUpParents(x).Any(y => y.Name.ToLower() == "dcs"));
        }

        /// <summary> Returns attachments, and attachments of those objects or any of their children recursively </summary>
        public static IEnumerable<ITxObject> GetAllAttachmentDescendants(ITxLocatableObject rootObject)
        {
            var attachments = rootObject.GetDirectAttachmentDescendants(new TxNoTypeFilter());

            if (rootObject is ITxObjectCollection)
            {

                foreach (ITxLocatableObject subItem in ((rootObject as ITxObjectCollection)?.GetAllDescendants(new TxNoTypeFilter()).OfType<ITxLocatableObject>()))
                {
                    foreach (var subChild in subItem.GetDirectAttachmentDescendants(new TxNoTypeFilter()))
                    {
                        yield return subChild;
                    }
                }
            }

            foreach (var attachment in attachments)
            {
                yield return attachment;

                if (attachment is ITxLocatableObject locatableAttachment)
                {
                    foreach (var descendant in GetAllAttachmentDescendants(locatableAttachment))
                    {
                        yield return descendant;
                    }
                }
                if (attachment is ITxObjectCollection subCollection)
                {
                    foreach(var subChild in subCollection.GetAllDescendants(new TxNoTypeFilter()).OfType<ITxLocatableObject>()) //Non-locatable children would pass null to the recursive call
                    {
                        foreach (var item in GetAllAttachmentDescendants(subChild)) yield return item;
                    }
                }
            }
        }

        public static IEnumerable<ITxObject> GetRobotAttachments(ITxObjectCollection sourceRobot)
        {
            var kinematicLink = sourceRobot.GetAllDescendants(new TxTypeFilter(typeof(TxKinematicLink))).FirstOrDefault(x => x.Name.ToLower() == "k7") as TxKinematicLink;
            var toolFrame = sourceRobot.GetAllDescendants(new TxTypeFilter(typeof(TxFrame))).FirstOrDefault(x => x.Name.ToLower() == "toolframe") as TxFrame;

            var allAttachments = toolFrame.GetDirectAttachmentDescendants(new TxNoTypeFilter()).Concat(kinematicLink.GetDirectAttachmentDescendants(new TxNoTypeFilter()));

            return allAttachments;
        }
        public static IEnumerable<ITxComponent> GetInbuiltRobotDcs(ITxRobot x)
        {
            var container = GetRobotContainer(x);
            if (container == null) return new ITxComponent[0];
            var kinematicLinks = (x as ITxObjectCollection).GetAllDescendants(new TxTypeFilter(typeof(TxKinematicLink))).OfType<TxKinematicLink>().Where(y => y.Name.ToLower() != "k7");
            var kinematicLinksDescendants = kinematicLinks.SelectMany(z => GetAllAttachmentDescendants(z));

            //Links are attached to each other (k6 -> k7), so the search above also reaches k7 and the EOAT on its toolframe. Remove anything mounted there
            var k7 = (x as ITxObjectCollection).GetAllDescendants(new TxTypeFilter(typeof(TxKinematicLink))).FirstOrDefault(y => y.Name.ToLower() == "k7");
            var toolFrame = (x as ITxObjectCollection).GetAllDescendants(new TxTypeFilter(typeof(TxFrame))).FirstOrDefault(y => y.Name.ToLower() == "toolframe");

            return kinematicLinksDescendants
                .Where(obj => StepUpParents(obj).Any(parent => parent == container) == false)
                .Where(obj => StepUpAttachmentParents(obj).Any(parent => Equals(parent, k7) || Equals(parent, toolFrame)) == false)
                .OfType<ITxComponent>();
        }

        /// <summary> Steps up through each object's AttachmentParent, or its Collection when it has no attachment parent </summary>
        static IEnumerable<ITxObject> StepUpAttachmentParents(ITxObject child)
        {
            var current = child;
            for (int depth = 0; depth < 1000; depth++) //Depth limit guards against an attachment loop
            {
                var next = (current as ITxLocatableObject)?.AttachmentParent ?? (ITxObject)current.Collection;
                if (next == null || Equals(next, current)) yield break; //The root's Collection is itself
                yield return next;
                current = next;
            }
        }
        /// <summary>
        /// All TxGroups under DCS / [robot's collection] / SafeZones, where the robot's collection is a direct child of a collection named "DCS"
        /// whose first two underscore separated name parts match the robot container's, e.g. "QA185_R01" for "QA185_R01_B01"
        /// </summary>
        public static IEnumerable<TxGroup> GetCpcZones(ITxRobot robot)
        {
            var container = GetRobotContainer(robot);
            if (container == null) return Enumerable.Empty<TxGroup>();
            var containerPrefix = FirstTwoNameParts(container.Name);

            var dcsCollections = TxApplication.ActiveDocument.PhysicalRoot.GetAllDescendants(new TxTypeFilter(typeof(ITxObjectCollection)))
                .OfType<ITxObjectCollection>()
                .Where(x => x.Name.ToLower() == "dcs");

            var robotDcsCollections = dcsCollections
                .SelectMany(dcs => dcs.GetDirectDescendants(new TxTypeFilter(typeof(ITxObjectCollection))).OfType<ITxObjectCollection>())
                .Where(x => string.Equals(FirstTwoNameParts(x.Name), containerPrefix, StringComparison.OrdinalIgnoreCase));

            var safeZoneCollections = robotDcsCollections
                .SelectMany(x => x.GetAllDescendants(new TxTypeFilter(typeof(ITxObjectCollection))).OfType<ITxObjectCollection>())
                .Where(x => x.Name.ToLower() == "safezones");

            return safeZoneCollections.SelectMany(x => x.GetAllDescendants(new TxTypeFilter(typeof(TxGroup))).OfType<TxGroup>());
        }

        static string FirstTwoNameParts(string name)
        {
            return string.Join("_", name.Split('_').Take(2));
        }

        /// <summary>
        /// The topmost collection above the robot whose name still contains the first two underscore separated parts of the robot's name,
        /// e.g. "QA185_R01" for "QA185_R01_B01". Stepping up stops at the first collection whose name doesn't contain them
        /// </summary>
        public static ITxObjectCollection GetRobotContainer(ITxRobot rob)
        {
            var robotPrefix = FirstTwoNameParts(rob.Name);
            return StepUpParents(rob).TakeWhile(x => x.Name.IndexOf(robotPrefix, StringComparison.OrdinalIgnoreCase) >= 0).LastOrDefault();
        }
    }
}
