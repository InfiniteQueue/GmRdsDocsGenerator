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
            MainRobot,
            MainDcs,
            OtherRobots,
            EOAT,
            Fencing,
            Product,
            ToolsSurroundingRobot,
            CPCSharedZones,
            CPCSharedZonesCSC,
            CPCRestrictedZones,
            CPCRestrictedZonesCSC,
        }

        public static Dictionary<ObjCategories, TxColor> CategoryColors => new Dictionary<ObjCategories, TxColor>()
        {
            {ObjCategories.MainRobot, GMColours.Blue },
            {ObjCategories.MainDcs, GMColours.Blue },
            {ObjCategories.Product, GMColours.Blue },
            {ObjCategories.OtherRobots, GMColours.Gray1 },
            {ObjCategories.EOAT, GMColours.Red },
            {ObjCategories.Fencing, GMColours.Yellow },
            {ObjCategories.ToolsSurroundingRobot, GMColours.Black1 },
            {ObjCategories.CPCSharedZones, GMColours.Cyan },
            {ObjCategories.CPCSharedZonesCSC, GMColours.Teal },
            {ObjCategories.CPCRestrictedZones, GMColours.Magenta },
            {ObjCategories.CPCRestrictedZonesCSC, GMColours.DarkMagenta },
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
            if (newItems == null || newItems.Count == 0) return;
            foreach (var item in newItems.OfType<ITxDisplayableObject>()) item.Color = newColor;
        }

        void ColorAllItems()
        {
            foreach(var pair in categoryLists)
            {
                foreach(var  item in pair.Value)
                {
                    item.Color = CategoryColors[pair.Key];
                }
            }
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;
    }
}
