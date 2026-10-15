using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tecnomatix.Engineering;

namespace GmRdsDocsGenerator.Statics
{
    internal static class SnapshotStatics
    {
        public static void LoadDisplayData(this TxSnapshot snapshot)
        {
            snapshot.Apply(new TxApplySnapshotParams() { ObjectsColor = true, ObjectsVisibility = true, ObjectsViewingMode = true });
        }
    }
}
