using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _HRAutomation.resources.classes
{
    public class VendorLite
    {
        public string VendorName { get; set; }
        public string filePath { get; set; }
        public string workingPath { get; set; }
        public string publicKeyPath { get; set; }
        public string scriptCommands { get; set; }
        public string testCommands { get; set; }
        public CleanUpFiles cleanUpItems { get; set; }
        public string TransmissionReport { get; set; }
        public string UserID { get; set; }
        public bool WasTest { get; set; }
    }

    public class CleanUpFiles
    {
        public string TxtWorkingFilePath { get; set; }
        public string EncryptedFilePath { get; set; }
        public string ClientScriptFilePath { get; set; }
        public string ClientBatFilePath { get; set; }
    }

}
