using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GmRdsDocsGenerator.Constants;
using GmRdsDocsGenerator.Models;
using Microsoft.Win32;
using Tecnomatix.Engineering;

namespace GmRdsDocsGenerator.Services
{
    internal static class ObjectCategoryColouring
    {
        public static void DoColouring(SimObjectsModel mainObjects, SimObjectScanLists registry)
        {
            foreach(var item in TxApplication.ActiveDocument.PhysicalRoot.GetDirectDescendants(new TxNoTypeFilter()))
            {
                if (item is ITxDisplayableObject dispObj) dispObj.Blank();
            }

            (mainObjects.Robot as ITxDisplayableObject).Display();

            Debug.WriteLine($"Colouring {registry.AllRobots.Count} robots");
            ColourAll(registry.AllRobots.Where(x => x != mainObjects.Robot), GMColours.Gray1);

            Debug.WriteLine($"Colouring {registry.AllEOAT.Count} grippers");
            ColourAll(registry.AllEOAT, GMColours.Red);

            Debug.WriteLine($"Colouring {registry.AllProduct.Count} panels");
            ColourAll(registry.AllProduct, GMColours.Blue);

            Debug.WriteLine($"Colouring {registry.Fencing.Count} fencing objects");
            ColourAll(registry.Fencing, GMColours.Yellow);

            Debug.WriteLine($"Colouring {registry.AllDCSObjects.Count} dcs objects");
            ColourAll(registry.AllDCSObjects, GMColours.DcsModel);
        }

        static void ColourAll(IEnumerable<ITxObject> objects, TxColor color)
        {
            foreach (var item in objects)
            {
                var displayable = item as ITxDisplayableObject;
                displayable.Display();
                displayable.Color = color;
            }
        }
    }
}
