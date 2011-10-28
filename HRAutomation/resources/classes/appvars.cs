using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _HRAutomation.resources.classes
{
    public static class appvars
    {
        /// <summary>
        /// Path to the configuration file for the HR automation
        /// </summary>
        public static string PathToXML = "~/resources/global/configFiles/configSettings.xml";

        /// <summary>
        /// Path to the secret key ring with our private signature.
        /// </summary>
        public static string SignPrivateKey = "~/resources/global/keys/private/secring.skr";

        /// <summary>
        /// Password to the Site PGP Key used for encrypting files for transmission
        /// </summary>
        public static string SignPassWord = "testpassword";

        /// <summary>
        /// Returns the correct file name for a given vendor...
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string ReturnNewFileName(VendorType type)
        {
            string NewTitle = String.Empty;
            string _year = DateTime.Now.Year.ToString();
            string _month = DateTime.Now.Month.ToString();
            string _day = DateTime.Now.Day.ToString().PadLeft(2, '0');
            string _time = String.Format("{0}{1}", DateTime.Now.TimeOfDay.Hours, DateTime.Now.TimeOfDay.Minutes.ToString().PadLeft(2, '0'));

                switch (type)
                {
                    case VendorType.FakeProvider:
                            NewTitle = String.Format("FakeProvider_test_file_{0}{1}{2}.txt.pgp", _year, _month, _day);    
                        break;
                    case VendorType.SampleProvider:
                            NewTitle = String.Format("SampleProvider_test_file_{0}{1}{2}.txt.pgp", _year, _month, _day);    
                        break;
                    case VendorType.StillFakeProvider:
                            NewTitle = String.Format("StillFakeProvider_test_file_{0}{1}{2}{3}.pgp", _year, _month, _day, _time);    
                        break;
                    default:
                        break;
                }

            return NewTitle;

        }

        public static string PathToWinSCPComFile = @"C:\Program Files\WinSCP\WinSCP.com";

        public static string ReplaceValueForFilePut = @"[FILE_UPLOAD_PATH]";
        public static string ReplaceValueForFileBatArguments = @"[SCRIPT_PATH]";

        public static string PathToExecutableDirectory = "~/resources/global/executables/";

        public static string PathToWinSCpBatTemplate = "~/resources/global/templates/WinScpBat.txt";


    }
}
