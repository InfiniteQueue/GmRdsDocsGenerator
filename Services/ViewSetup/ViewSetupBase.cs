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
        }
        protected static void BlankAll(IEnumerable<ITxObject> objects)
        {
            foreach (var dispObject in objects.OfType<ITxDisplayableObject>()) dispObject.Blank();
        }



        //Generics are used here to allow the static function NewSetup to return a correctly typed instance of the overriding subclass
        public static T NewSetup<T>(ViewStateManager viewStateRegistry) where T : ViewSetupBase, new()
        {
            var newObject = new T();

            newObject.viewStateRegistry = viewStateRegistry;

            newObject.PerformSetup();

            return newObject;
        }
    }

}
