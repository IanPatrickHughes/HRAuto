using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace _HRAutomation.resources.classes
{
    public static class FileCleanUp
    {
        /// <summary>
        /// Cleans up all files in the member class...
        /// </summary>
        /// <param name="cuf"></param>
        public static void CleanUpFileTransfer(CleanUpFiles cuf)
        {
            if (File.Exists(cuf.ClientScriptFilePath))
            {
                File.Delete(cuf.ClientScriptFilePath);
            }
            if (File.Exists(cuf.EncryptedFilePath))
            {
                File.Delete(cuf.EncryptedFilePath);
            }
            if (File.Exists(cuf.TxtWorkingFilePath))
            {
                File.Delete(cuf.TxtWorkingFilePath);
            }
            if (File.Exists(cuf.ClientBatFilePath))
            {
                File.Delete(cuf.ClientBatFilePath);
            }

        }
    }
}
