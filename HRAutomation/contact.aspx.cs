using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _HRAutomation
{
    public partial class contact : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.ShowLeftPanel = false;
            this.Master.SetUserInformation();
            this.Master.divMainContent.Style.Add("float", "left");
            this.Master.divMainContent.Style.Add("text-align", "left");
        }
    }
}
