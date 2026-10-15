using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tecnomatix.Engineering;

namespace GmRdsDocsGenerator.DebugClasses
{
    internal static class DebugMethods
    {
        public static void DebugMain()
        {

            if (TxApplication.ActiveSelection.GetAllItems().FirstOrDefault() is ITxObject itxObj)
            {
                //if (itxObj is ITxDisplayableObject dispObj) dispObj.RestoreColor();

                ((ITxDisplayableObject)itxObj).DisplayMode = TxDisplayMode.Wireframe;
                if (itxObj is ITxRobot selRobot)
                {
                    var grippers = SimObjectRetrieval.GetGrippers(selRobot as ITxObjectCollection);
                    ;
                }
            }
        }
    }
}
