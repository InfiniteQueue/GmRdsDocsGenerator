using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Tecnomatix.Engineering;

namespace GmRdsDocsGenerator.Services
{
    static class SimDataRetrieval
    {
        public static string GetFileName(ITxStorable storable)
        {
            try
            {
                if (storable.StorageObject is TxLibraryStorage libStor)
                {
                    return Regex.Match(libStor.FullPath, @".*\\(.*).cojt").Groups[1].Value;
                }
                if (storable.StorageObject is TxLocalStorage localStorage)
                {
                    return Regex.Match(localStorage.ToString(), @".*\\(.*).cojt").Groups[1].Value;
                }
            }
            catch { }
            return string.Empty;
        }
    }
}
