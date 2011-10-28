using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using _HRAutomation.resources.classes.utility;

namespace _HRAutomation.resources.global.master
{
    public partial class HrAuto : System.Web.UI.MasterPage
    {
        public UserHandler CurrentUser { get; set; }
        public string CurrentUserName { get; set; }
        public bool ShowLeftPanel = true;
        public HtmlGenericControl divMainContent
        {
            get { return this.divContent; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            SetUserInformation();
            CheckPanelOrNot();
        }


        /// <summary>
        /// sets the user if it is not set...
        /// </summary>
        public void SetUserInformation()
        {

            if (!UserHandler.IsUserSet())
            {
                var uH = new UserHandler();
                uH.CreateUserSession();
                this.CurrentUser = UserHandler.ReturnCurrentStoredUser();
            }
            else
            {
                this.CurrentUser = UserHandler.ReturnCurrentStoredUser();
            }

            this.CurrentUserName = this.CurrentUser.UserObject.FullName.ToString();
            CheckForSecurity();
           
        }

        /// <summary>
        /// check whether to display the side panel or not
        /// </summary>
        private void CheckPanelOrNot()
        {
            if (!this.ShowLeftPanel)
            {
                this.pnlLeftArea.Visible = false;
            }
        }

        /// <summary>
        /// Completes a simple check
        /// </summary>
        private void CheckForSecurity()
        {
            var deptCode = (this.CurrentUser.UserObject.DepartmentCode).ToLower();
            var currentPageName = this.GetCurrentPageName().ToLower().Replace(".aspx", "");

            switch (deptCode)
            {
                case "is":
                case "hr":
                        //[removed localized security calls]
                    break;
                default:
                    if (currentPageName != "sorry")
                    {
                        Response.Redirect("~/sorry.aspx");
                    }
                    break;
                    
            }
            
        }

        private string GetCurrentPageName()
        {
            var sPath = System.Web.HttpContext.Current.Request.Url.AbsolutePath;
            var oInfo = new System.IO.FileInfo(sPath);
            var sRet = oInfo.Name;
            return sRet;
        } 

    }
}
