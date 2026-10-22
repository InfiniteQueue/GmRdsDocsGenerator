using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using GmRdsDocsGenerator.Constants;
using Tecnomatix.Engineering;
using static GmRdsDocsGenerator.Statics.SnapshotStatics;

namespace GmRdsDocsGenerator.Models
{
    internal class CategoryRegistry
    {
        public CategoryRegistry(){
            foreach(var item in CategoryColors)
            {
                InitNewCategoryCollection(item);
            }
        }

        public IEnumerable<ITxObject> this[ObjCategories category] => categoryLists.TryGetValue(category, out var value) ? value : Enumerable.Empty<ITxObject>();

        public void AssignItem(ITxDisplayableObject obj, ObjCategories category)
        {
            foreach (var list in categoryLists) list.Value.Remove(obj);
            categoryLists[category].Add(obj);
        }
        public void AssignItems(IEnumerable<ITxDisplayableObject> objects, ObjCategories category)
        {
            foreach (var obj in objects) AssignItem(obj, category);
        }

        /// <summary>
        /// The original color of the object cannot be assumed to be blank, so restore the snapshot and recolor all items when an item is removed from a list
        /// </summary>
        public void UnassignItem(ViewStateManager viewStateRegistry, ITxDisplayableObject obj)
        {
            var matchingCategories = categoryLists.Where(x => x.Value.Contains(obj));
            if (matchingCategories.Count() == 0) return;

            foreach (var category in matchingCategories) category.Value.Remove(obj);
            viewStateRegistry.OriginalViewSnapshot?.LoadDisplayData();
            ColorAllItems();
        }

        private Dictionary<ObjCategories, ObservableCollection<ITxDisplayableObject>> categoryLists = new Dictionary<ObjCategories, ObservableCollection<ITxDisplayableObject>>();

        public enum ObjCategories
        {
            MainRobotContainers,
            MainTxRobot,
            MainDcs,
            OtherRobotsContainers,
            OtherTxRobots,
            EOAT,
            Risers,
            MainRobotToolChanger,
            MainEOATToolChangers,
            Fencing,
            MainProduct,
            OtherRobotProduct,
            CPCSharedZones,
            CPCSharedZonesCSC,
            CPCRestrictedZones,
            CPCRestrictedZonesCSC,
            RestrictedVolume,
            Tooling,
        }

        /// <summary> Links categories to their color for screenshots. Value may be null if color should not be overwritten </summary>
        public static Dictionary<ObjCategories, TxColor> CategoryColors => new Dictionary<ObjCategories, TxColor>()
        {
            {ObjCategories.MainRobotContainers, GMColours.Blue },
            {ObjCategories.MainTxRobot, null },
            {ObjCategories.MainDcs, GMColours.Blue },
            {ObjCategories.OtherRobotsContainers, null },
            {ObjCategories.OtherTxRobots, GMColours.Gray1 },
            {ObjCategories.EOAT, GMColours.Red },
            {ObjCategories.Risers, GMColours.Blue },
            {ObjCategories.MainRobotToolChanger, GMColours.Red },
            {ObjCategories.MainEOATToolChangers, GMColours.Red },
            {ObjCategories.Fencing, GMColours.Yellow },
            {ObjCategories.MainProduct, GMColours.Blue },
            {ObjCategories.OtherRobotProduct, GMColours.Blue },
            {ObjCategories.CPCSharedZones, GMColours.Cyan },
            {ObjCategories.CPCSharedZonesCSC, GMColours.Teal },
            {ObjCategories.CPCRestrictedZones, GMColours.Magenta },
            {ObjCategories.CPCRestrictedZonesCSC, GMColours.DarkMagenta },
            {ObjCategories.RestrictedVolume, null },
            {ObjCategories.Tooling, GMColours.Black1 },
        };
        void InitNewCategoryCollection(KeyValuePair<ObjCategories, TxColor> item)
        {
            var collection = new ObservableCollection<ITxDisplayableObject>();
            categoryLists[item.Key] = collection;
            collection.CollectionChanged += (s, e) =>
            {
                ColorAddedItems(e.NewItems, item.Value);
                CollectionChanged?.Invoke(this, e);
            };

        }

        /// <summary> Subscribes to CollectionChanged to run automatically when an object is added to a list /// </summary>
        void ColorAddedItems(IList newItems, TxColor newColor)
        {
            if (newColor == null || newItems == null || newItems.Count == 0) return;
            foreach (var item in newItems.OfType<ITxDisplayableObject>()) item.Color = newColor;
        }

        void ColorAllItems()
        {
            foreach(var pair in categoryLists)
            {
                foreach(var  item in pair.Value)
                {
                    if (CategoryColors[pair.Key] is TxColor foundColor)
                        item.Color = foundColor;
                }
            }
        }

        public void ApplyCategoryColor(ObjCategories category)
        {
            if (CategoryColors[category] is TxColor foundColor)
            {
                foreach (var item in categoryLists[category]) item.Color = foundColor;
            }
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;
    }
}
