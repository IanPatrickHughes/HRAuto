using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _HRAutomation
{
    public partial class sorry : System.Web.UI.Page
    {
        protected string CurrentUserName { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.ShowLeftPanel = false;
            this.Master.SetUserInformation();
            this.CurrentUserName = this.Master.CurrentUserName.ToString();
        }
    }
}
