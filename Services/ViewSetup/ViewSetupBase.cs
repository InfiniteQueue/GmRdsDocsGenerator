using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GmRdsDocsGenerator.Statics.SnapshotStatics;
using GmRdsDocsGenerator.Models;
using Tecnomatix.Engineering;
using System.Diagnostics;

namespace GmRdsDocsGenerator.Services.ViewSetup
{
    internal abstract class ViewSetupBase : IDisposable
    {
        protected ViewStateManager viewStateRegistry;
        protected ViewSetupBase() { }


        protected static void HideAll()
        {
            foreach(var item in TxApplication.ActiveDocument.PhysicalRoot.GetDirectDescendants(new TxNoTypeFilter()).OfType<ITxDisplayableObject>()
                .Concat(TxApplication.ActiveDocument.OperationRoot.GetDirectDescendants(new TxNoTypeFilter()).OfType<ITxDisplayableObject>()))
            {
                //Debug.WriteLine(item.Name);
                item.Blank();
            }
        }
        protected abstract void PerformSetup();
        protected abstract void Cleanup();
        public void Dispose()
        {
            viewStateRegistry?.DocumentViewSnapshot?.LoadDisplayData();
        }

        protected static void DisplayAll(IEnumerable<ITxObject> objects)
        {
            foreach (var dispObject in objects.OfType<ITxDisplayableObject>()) dispObject.Display();
            BlankAll(TxApplication.ActiveDocument.PhysicalRoot.GetAllDescendants(new TxTypeFilter(typeof(TxFrame))));
        }
        protected static void BlankAll(IEnumerable<ITxObject> objects)
        {
            foreach (var dispObject in objects.OfType<ITxDisplayableObject>()) dispObject.Blank();
        }



        /// <summary> Applies this view to the document. Called by ViewStateManager.ActiveView after the previous view is disposed </summary>
        public void Setup()
        {
            PerformSetup();
        }

        //Generics are used here to allow the static function NewSetup to return a correctly typed instance of the overriding subclass
        /// <summary> Creates the view without applying it; call Setup, or assign it to ViewStateManager.ActiveView, to apply it </summary>
        public static T NewSetup<T>(ViewStateManager viewStateRegistry) where T : ViewSetupBase, new()
        {
            var newObject = new T();

            newObject.viewStateRegistry = viewStateRegistry;

            return newObject;
        }
        protected void RecolorCategory(CategoryRegistry.ObjCategories category)
        {
            viewStateRegistry.Categories.ApplyCategoryColor(category);
        }
    }

}
