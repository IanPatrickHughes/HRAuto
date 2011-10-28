using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using System.IO;
using System.Text;

using _HRAutomation.resources.classes;
using _HRAutomation.resources.classes.utility;


namespace _HRAutomation.resources.global.usercontrols
{
    public partial class SingleFileUploadForm : System.Web.UI.UserControl
    {
        public XDocument vendorConfigFile;
        public string[] arrOfVendors;
        List<VendorLite> lstVendors;
        private UserHandler CurrentUser { get; set; }
        private string currentSelectedVendorTitle = String.Empty;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SetUIItems();
            }


            this.btnRunHRAutomation.Click += new EventHandler(btnRunHRAutomation_Click);
            
        }
               
     
        protected void btnRunHRAutomation_Click(object sender, EventArgs e)
        {
            currentSelectedVendorTitle = this.ddlVendorList.SelectedValue.ToString();

            if (this.currentSelectedVendorTitle.ToLower().Equals("none"))
            {
                string _js = String.Format("alert('Please select a vendor to process!');");
                ScriptManager.RegisterClientScriptBlock(this.Page, typeof(System.Web.UI.Page), "curren_selected_value", _js, true);
            }
            else
            {
                bool _isTest = this.cboIsTest.Checked;

                try
                {
                    this.HandleFileImports(_isTest, this.currentSelectedVendorTitle);
                    pnlResponse.Visible = true;
                    StringBuilder sb = new StringBuilder();
                    sb.Append("<h2>Congratulations!</h2>");
                    sb.Append("<p>Your single-vendor file was encrypted and transmitted without error or issue.</p>");
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
        }


        #region /// <private events> ...

            /// <summary>
            /// The last date that a file(s) was submitted...
            /// </summary>
            private void SetUIItems()
            {
                this.ddlVendorList.Items.Clear(); //always clear, no matter what...

                //build up the vendor items...
                this.vendorConfigFile = XDocument.Load(Server.MapPath(appvars.PathToXML));
                this.arrOfVendors = Enum.GetNames(typeof(VendorType));
                lstVendors = new List<VendorLite>();
               

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

                //First add our default "none selected" value...
                ListItem li = new ListItem("Select a Vendor to Process...", "none");
                this.ddlVendorList.Items.Add(li);

                foreach (var item in lstVendors)
                {
                    ListItem liNext = new ListItem();
                    liNext.Value = item.VendorName;
                    liNext.Text = item.VendorName;

                    this.ddlVendorList.Items.Add(liNext);
                }
            }

            /// <summary>
            /// The entry method for calling and handling the 
            /// files for encryption and transmission.
            /// </summary>
            private void HandleFileImports(bool isTest, string vendorTitle)
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
                              where c.Element("vendorName").Value.Equals(vendorTitle)
                              select new VendorLite
                              {
                                  VendorName = c.Element("vendorName").Value,
                                  filePath = c.Element("hrFilePath").Value,
                                  workingPath = c.Element("workingPath").Value,
                                  publicKeyPath = c.Element("encryptionKeyPath").Value,
                                  scriptCommands = Server.MapPath(c.Element("scriptCommands").Value),
                                  testCommands = Server.MapPath(c.Element("testCommands").Value)
                              }).ToList();

                if (hI.impersonateValidUser("ttcApplication", "ttc", "ttcapp"))
                {
                    //should always just be one...
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

                string source = vl.filePath;
                string file = Path.GetFileName(source);
                string dest = String.Format(@"{0}\{1}", Server.MapPath(vl.workingPath), file);
                string PublicKeyFileName = Server.MapPath(vl.publicKeyPath);
                string PrivateKeyFileName = Server.MapPath(appvars.SignPrivateKey);
                VendorType vt = (VendorType)Enum.Parse(typeof(VendorType), vl.VendorName, true);
                string EncryptionFile = String.Format("{0}{1}", dest.Replace(Path.GetFileName(dest), ""), appvars.ReturnNewFileName(vt));

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