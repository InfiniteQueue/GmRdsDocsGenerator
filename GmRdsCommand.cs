using System;
using GmRdsDocsGenerator;
using Tecnomatix.Engineering;

namespace GmRdsDocsGenerator
{
    public class GmRdsCommand : TxButtonCommand
    {
        public override String Name { get { return "GM RDS Docs Generator"; } }
        public override string Tooltip { get { return "Generate a new GM RDS report"; } }
        public override string Category { get { return "Utility"; } }

        //public override string Bitmap => "ImageSmall.bmp";

        //public override string LargeBitmap => "ImagLarge.bmp";

        //public override string MediumBitmap => "ImagMedium.bmp";

        public override bool Expose => true;

        [STAThread]
        public override void Execute(object cmdParams)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }
    }
}
