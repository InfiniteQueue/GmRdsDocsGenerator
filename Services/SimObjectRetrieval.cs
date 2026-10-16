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


        /// <summary> All attachments that aren't ItxDevice and aren't under a collection named 'dcs' </summary>
        public static ITxComponent[] GetPanels(ITxObjectCollection sourceRobot)
        {
            var allAttachments = GetRobotAttachments(sourceRobot);

            IEnumerable<ITxComponent> ItxComponents = allAttachments.OfType<ITxComponent>();

            var filteredItems = ItxComponents.Where(x => !(x is ITxDevice) && StepUpParents(x).Any(y => y.Name.ToLower() == "dcs") == false);

            return filteredItems.ToArray();
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
                    foreach(var subChild in subCollection.GetAllDescendants(new TxNoTypeFilter()))
                    {
                        foreach (var item in GetAllAttachmentDescendants(subChild as ITxLocatableObject)) yield return item;
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
            var equipment = GetRobotEquipment(x);
            if (equipment == null) return new ITxComponent[0];
            var kinematicLinks = (x as ITxObjectCollection).GetAllDescendants(new TxTypeFilter(typeof(TxKinematicLink))).OfType<TxKinematicLink>().Where(y => y.Name.ToLower() != "k7");
            var kinematicLinksDescendants = kinematicLinks.SelectMany(z => GetAllAttachmentDescendants(z));

            return kinematicLinksDescendants.Where(obj => StepUpParents(obj).Any(parent => parent == equipment) == false).OfType<ITxComponent>();
        }
        public static ITxProcessModelObject GetRobotEquipment(ITxRobot rob)
        {
            return StepUpParents(rob).OfType<ITxProcessModelObject>().FirstOrDefault(x => x.PlanningRepresentation.PlanningType == "PmEquipmentInstance");
        }
    }
}
