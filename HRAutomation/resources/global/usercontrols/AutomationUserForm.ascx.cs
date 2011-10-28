using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.IO;
using System.Text;
using System.Configuration;

using _HRAutomation.resources.classes;
using _HRAutomation.resources.classes.utility;


namespace _HRAutomation.resources.global.usercontrols
{
    public partial class AutomationUserForm : System.Web.UI.UserControl
    {
        public XDocument vendorConfigFile;
        public string[] arrOfVendors;
        List<VendorLite> lstVendors;
        private UserHandler CurrentUser { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            SetUIItems();
        }



        #region /// <page events> ...

            protected void btnRunHRAutomation_Click(object sender, EventArgs e)
            {
                bool IsTest = cboIsTest.Checked;

                try
                {
                    HandleFileImports(IsTest);
                    pnlResponse.Visible = true;
                    StringBuilder sb = new StringBuilder();
                    sb.Append("<h2>Congratulations!</h2>");
                    sb.Append("<p>Your files were encrypted and transmitted without error or issue.</p>");
                    this.ltlResultText.Text = sb.ToString();
                }
                catch (Exception ex)
                {
                    pnlResponse.Visible = true;
                    StringBuilder sb = new StringBuilder();
                    sb.Append("<h2 style=\"color:#ff0000;\">OH NO THERE WAS AN ERROR!</h2>");
                    sb.Append("<p style=\"font-weight:bold;border-bottom:solid 1px #000;\">Please stop your work here and email the correct admin with the message below!</p>");
                    sb.Append(String.Format("<p>{0}</p>", ex.ToString()));
                    this.ltlResultText.Text = sb.ToString();
                }
            }

        #endregion 

        #region /// <private events> ...

            /// <summary>
            /// The last date that a file(s) was submitted...
            /// </summary>
            private void SetUIItems()
            {
                DateTime lastDate = new DateTime();
                lastDate = DBItems.ReturnLastActionDate();
                this.ltlLastRunDate.Text = lastDate.ToShortDateString();
            }



            /// <summary>
            /// The entry method for calling and handling the 
            /// files for encryption and transmission.
            /// </summary>
            private void HandleFileImports(bool isTest)
            {
                //build up the vendor items...
                this.vendorConfigFile = XDocument.Load(Server.MapPath(appvars.PathToXML));
                this.arrOfVendors = Enum.GetNames(typeof(VendorType));
                lstVendors = new List<VendorLite>();
                var hI = new helperImpersonate();
                var domain = ConfigurationManager.AppSettings["ADAccountDomain"].ToString();
                var un = ConfigurationManager.AppSettings["ADAccountUN"].ToString();
                var pw = ConfigurationManager.AppSettings["ADAccountPW"].ToString();
                int _count = 1;


                //get the config information for all venfors...
                lstVendors = (from c in vendorConfigFile.Descendants("configSet")
                              select new VendorLite
                              {
                                  VendorName = c.Element("vendorName").Value,
                                  filePath = c.Element("hrFilePath").Value,
                                  workingPath = c.Element("workingPath").Value,
                                  publicKeyPath = c.Element("encryptionKeyPath").Value,
                                  scriptCommands = Server.MapPath(c.Element("scriptCommands").Value),
                                  testCommands = Server.MapPath(c.Element("testCommands").Value)
                              }).ToList();

                if(hI.impersonateValidUser(un, domain, pw))
                {
                    foreach (VendorLite vl in this.lstVendors)
                    {
                        vl.WasTest = isTest;
                        HandleMovedVendorFile(vl, _count);
                        _count++;
                    }
                    
                    hI.undoImpersonation(); //put it bakc the way it was!
                }//end if impersonation succeeds...
            }

            /// <summary>
            /// Handles the actions for the current working file for encryption and transmission
            /// </summary>
            /// <param name="workingFile"></param>
            private void HandleMovedVendorFile(VendorLite vl, int _count)
            {

                    var source = vl.filePath;
                    var file = Path.GetFileName(source);
                    var dest = String.Format(@"{0}\{1}", Server.MapPath(vl.workingPath), file);
                    var PublicKeyFileName = Server.MapPath(vl.publicKeyPath);
                    var PrivateKeyFileName = Server.MapPath(appvars.SignPrivateKey);
                    var vt = (VendorType)Enum.Parse(typeof(VendorType), vl.VendorName, true);
                    var EncryptionFile = String.Format("{0}{1}", dest.Replace(Path.GetFileName(dest), ""), appvars.ReturnNewFileName(vt));
                    
                    //Get the user info from their session var...
                    SetUserInformation();
                    vl.UserID = this.CurrentUser.UserObject.LoginID.ToLower();
                        
                    if (!File.Exists(dest))
                    {
                        FileStream fs = File.Create(dest);
                        fs.Close();
                    }

                    File.Copy(source, dest, true);

                    PgpEncryptionKeys encryptionKeys = new PgpEncryptionKeys(PublicKeyFileName, PrivateKeyFileName, appvars.SignPassWord);
                    PgpEncrypt encrypter = new PgpEncrypt(encryptionKeys);

                    using (Stream outputStream = File.Create(EncryptionFile))
                    {
                        //build out the encypted file...
                        encrypter.EncryptAndSign(outputStream, new FileInfo(dest));

                        //Our file transfer class requires a fully qualified clean-up set... 
                            CleanUpFiles cuf = new CleanUpFiles();
                            cuf.EncryptedFilePath = EncryptionFile;
                            cuf.TxtWorkingFilePath = dest;
                            // we'll put in the script path later.... cuf.ClientScriptFilePath = "";
                        //set the vL item's cleanupItems for the next routine..     
                        vl.cleanUpItems = cuf;
                    }//end using statement

                    //construct the object and run it...     
                        FileTransfer fT = new FileTransfer(vl);
                        fT.RunUpload();

                    //File the report and store the files before clean-up...                    
                        FileReport fr = new FileReport(vl);
                        fr.SubmitReport();
                
                
                
                
                    //clean up...
                    FileCleanUp.CleanUpFileTransfer(vl.cleanUpItems);    

                
            }//handle the HandleMovedVendorFile

            /// <summary>
            /// sets the user if it is not set...
            /// </summary>
            public void SetUserInformation()
            {

                if (!UserHandler.IsUserSet())
                {
                    UserHandler uH = new UserHandler();
                    uH.CreateUserSession();
                    this.CurrentUser = UserHandler.ReturnCurrentStoredUser();
                }
                else
                {
                    this.CurrentUser = UserHandler.ReturnCurrentStoredUser();
                }


            }
            

        #endregion
    }
}