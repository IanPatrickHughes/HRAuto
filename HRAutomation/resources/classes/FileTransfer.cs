using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;
using System.Xml;
using System.IO;
using System.Xml.XPath;


namespace _HRAutomation.resources.classes
{
    public class FileTransfer
    {
        /// <summary>
        /// The given vendor object for FileUpload
        /// </summary>
        public VendorLite VendorObject { get; private set; }
        public string TransferReport { get; private set; }

        /// <summary>
        /// String value to be consumed by the class or routine that instanced this class
        /// </summary>
        public string ReportOfUpload { get; private set; }

        public FileTransfer(VendorLite vl)
        {
            this.VendorObject = vl;
        }


        /// <summary>
        /// Executes the file upload for the given vendor object (VendorLite)
        /// </summary>
        public void RunUpload()
        {


           //#1 We need to get the array of commands from the the given 
           //   client's template command template file.
                //#1.A get the commands from the template
                string[] arrOfCommands;

                if (this.VendorObject.WasTest)
                {
                    arrOfCommands = File.ReadAllLines(this.VendorObject.testCommands); //TEST FILE!!
                }
                else
                {
                    arrOfCommands = File.ReadAllLines(this.VendorObject.scriptCommands); //REAL FILE!!
                }
            
                for (int i = 0; i < arrOfCommands.Length; i++)
                {
                  arrOfCommands[i] = arrOfCommands[i].Replace(appvars.ReplaceValueForFilePut, this.VendorObject.cleanUpItems.EncryptedFilePath);
                }
                //#1.B get the commands from the bat template
                string[] arrOfBatCommands = File.ReadAllLines(HttpContext.Current.Server.MapPath(appvars.PathToWinSCpBatTemplate));


           //#2 Create the new dynamic file that will be fired against the dynamic WinSCP bat...
                //#2.A format the dynamic file path and then set it into our dyn bat file command
                string DynFile = String.Format("{0}commandfile_{1}.ini", HttpContext.Current.Server.MapPath(appvars.PathToExecutableDirectory), this.VendorObject.VendorName);
                for (int x = 0; x < arrOfBatCommands.Length; x++)
                {
                    arrOfBatCommands[x] = arrOfBatCommands[x].Replace(appvars.ReplaceValueForFileBatArguments, DynFile);
                }

                //#2.B Create the ini file to execute against the bat file...
                TextWriter tw = new StreamWriter(DynFile);
                this.VendorObject.cleanUpItems.ClientScriptFilePath = DynFile;
                foreach (string _s in arrOfCommands)
                {
                    tw.WriteLine(_s);
                }
                tw.Close();

                //#2.C Create out our dyanmic bat file for execution below and add it to the clean-up list...    
                string DynBatFile = String.Format("{0}WinScP_{1}.bat", HttpContext.Current.Server.MapPath(appvars.PathToExecutableDirectory), this.VendorObject.VendorName);
                this.VendorObject.cleanUpItems.ClientBatFilePath = DynBatFile;
                TextWriter tw2 = new StreamWriter(DynBatFile);
                foreach (string _s2 in arrOfBatCommands)
                {
                    tw2.WriteLine(_s2);
                }
                tw2.Close();



           //#3 Fire the Bat file with the newly created txt command file...
            // Get the full file path
                    string strFilePath = DynBatFile;

                    // Create the ProcessInfo object
                    System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo("cmd.exe");
                    psi.UseShellExecute = false;
                    psi.RedirectStandardOutput = true;
                    psi.RedirectStandardInput = true;
                    psi.RedirectStandardError = true;
                    psi.WindowStyle = ProcessWindowStyle.Hidden;
                    psi.CreateNoWindow = true;
                    psi.WorkingDirectory = HttpContext.Current.Server.MapPath(appvars.PathToExecutableDirectory);

                    // Start the process
                    System.Diagnostics.Process proc = System.Diagnostics.Process.Start(psi);

                    // Open the batch file for reading
                    System.IO.StreamReader strm = System.IO.File.OpenText(strFilePath);

                    // Attach the output for reading
                    System.IO.StreamReader sOut = proc.StandardOutput;

                    // Attach the in for writing
                    System.IO.StreamWriter sIn = proc.StandardInput;

                    // Write each line of the batch file to standard input
                    while(strm.Peek() != -1)
                    {
                        string currentLine = strm.ReadLine();
                        if (currentLine.Contains("%1"))
                        {
                            currentLine = currentLine.Replace("%1", DynFile);
                            sIn.WriteLine(currentLine);
                        }
                        else
                        {
                            sIn.WriteLine(currentLine);
                        }
                    }

                    strm.Close();

                    // Exit CMD.EXE
                    string stEchoFmt = "# {0} run successfully. Exiting";

                    sIn.WriteLine(String.Format(stEchoFmt, strFilePath));
                    sIn.WriteLine("EXIT");

                    // Close the process
                    proc.Close();

                    // Read the sOut to a string.
                    string results = sOut.ReadToEnd().Trim();

                    // Close the io Streams;
                    sIn.Close();
                    sOut.Close();

                    // Write out the results.
                    string fmtStdOut = "<font face=courier size=0>{0}</font>";
                    this.VendorObject.TransmissionReport = String.Format(fmtStdOut, results.Replace(System.Environment.NewLine, "<br>"));
                    

           //#4 Set the report string for this FileTransfer Class
                
                
           

        }//end public void RunTest


        #region /// <helper methods> ...




        #endregion


    }//end class FileTransfer
}
