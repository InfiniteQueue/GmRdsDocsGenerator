using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GmRdsDocsGenerator.Services
{
    internal class BackgroundSetup : IDisposable
    {
        bool disposed = false;
        public BackgroundSetup() 
        {
            RecordBackgroundColor();
            ColorBackgroundWhite();
        }
        public void Dispose()
        {
            if (disposed) return;
            RestoreBackgroundColor();
            disposed = true;
        }

        void ColorBackgroundWhite()
        {
            //throw new NotImplementedException();
        }
        void RecordBackgroundColor()
        {
            //throw new NotImplementedException();
        }
        void RestoreBackgroundColor()
        {
            //throw new NotImplementedException();
        }
    }
}
