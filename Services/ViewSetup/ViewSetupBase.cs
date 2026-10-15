using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GmRdsDocsGenerator.Statics.SnapshotStatics;
using GmRdsDocsGenerator.Models;
using Tecnomatix.Engineering;

namespace GmRdsDocsGenerator.Services.ViewSetup
{
    //Generics are used here to allow the static function NewSetup to return a correctly typed instance of the overriding subclass
    /// <summary> Base class for view setups, used to prepare the document for different types of screenshot </summary>
    internal abstract class ViewSetupBase<T> : IDisposable where T : ViewSetupBase<T>, new()
    {
        protected ViewStateRegistry viewStateRegistry;
        protected ViewSetupBase() { }

        public static T NewSetup(ViewStateRegistry viewStateRegistry)
        {
            var newObject = new T();

            newObject.viewStateRegistry = viewStateRegistry;

            newObject.PerformSetup();

            return newObject;
        }

        protected static void HideAll()
        {
            foreach(var item in TxApplication.ActiveDocument.PhysicalRoot.OfType<ITxDisplayableObject>())
            {
                item.Blank();
            }
        }
        protected abstract void PerformSetup();
        public void Dispose()
        {
            viewStateRegistry?.DocumentViewState?.LoadDisplayData();
        }

        protected static void DisplayAll(IEnumerable<ITxObject> objects)
        {
            foreach (var dispObject in objects.OfType<ITxDisplayableObject>()) dispObject.Display();
        }
        protected static void BlankAll(IEnumerable<ITxObject> objects)
        {
            foreach (var dispObject in objects.OfType<ITxDisplayableObject>()) dispObject.Blank();
        }
    }
}
