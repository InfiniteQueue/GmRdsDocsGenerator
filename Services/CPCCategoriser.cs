using System.Windows.Documents;
using Tecnomatix.Engineering;
using System.Collections.Generic;
using System.Linq;

namespace GmRdsDocsGenerator.Services
{
    public class CPCCategoriser
    {
        public CPCCategoriser(TxObjectList<TxGroup> cpcObjects, ITxRobot robot)
        {
            this.CPCObjects = cpcObjects;
            this.robot = robot;
        }

        public void Process()
        {
            var cscIndexes = CSCIndexes(robot);

            foreach (var item in CPCObjects)
            {
                var cpcObject = new CpcObject() { component = item };
                cpcObjects.Add(cpcObject);
                cpcObject.index = GetIndex(cpcObject);
                cpcObject.isPLC = cpcObject.index <= 7;
                cpcObject.isCSC = cscIndexes.Contains(cpcObject.index);
            }

            plc = cpcObjects.Where(x => x.isPLC && !x.isCSC).Select(y => y.component).ToList();
            plcCSC = cpcObjects.Where(x => x.isPLC && x.isCSC).Select(y => y.component).ToList();
            rob = cpcObjects.Where(x => !x.isPLC && !x.isCSC).Select(y => y.component).ToList();
            robCSC = cpcObjects.Where(x => !x.isPLC && x.isCSC).Select(y => y.component).ToList();
        }

        int[] CSCIndexes(ITxRobot rob)
        {
            var indexList = new List<int>();

            foreach(var parm in robot.Attributes)
            {
                if (parm is TxStringAttribute strAttribute && strAttribute.Name.Contains("RJ_DCS_SPEED"))
                {
                    indexList.Add(int.Parse(strAttribute.Value.Split().LastOrDefault(x => int.TryParse(x, out _))));
                }
            }

            return indexList.ToArray();
        }

        int GetIndex(CpcObject item)
        {
            return int.Parse(item.component.Name.Split('_').FirstOrDefault(x => int.TryParse(x, out _)));
        }

        TxObjectList<TxGroup> CPCObjects;
        ITxRobot robot;

        public List<TxGroup> plc;
        public List<TxGroup> plcCSC;
        public List<TxGroup> rob;
        public List<TxGroup> robCSC;

        private List<CpcObject> cpcObjects = new List<CpcObject>();

        private class CpcObject
        {
            public TxGroup component;
            public int index;
            public bool isPLC;
            public bool isCSC;
        }

    }
}
