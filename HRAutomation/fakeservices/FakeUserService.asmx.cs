using System;
using System.Web.Services;

namespace _HRAutomation.fakeservices
{
    /// <summary>
    /// This is a fake service I just created so that this sample project could actually operate.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    public class FakeUserService : System.Web.Services.WebService
    {
        public  FakeUserService()
        {
        }


        /// <summary>
        /// Fake replacement for log in call that returned the network AD object
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [WebMethod]
        public CorpUser GetUserByLogin(string userId)
        {
           //This is useless logic...just for the demo.
            var usr = new CorpUser();
            if (userId == "adminLevelUser")
            {
                usr.DepartmentCode = "hr";
                usr.FullName = "Admin Sample Person";
                usr.FirstName = "Admin";
                usr.MiddleName = "Sample";
                usr.LastName = "Person";
                usr.LoginID = "adminLevelUser";
                usr.Email = "adminLevelUser@yoursite.com";
                usr.Extension = "666";
                usr.DirectDial = "857-5309";
                usr.PersonGUID = Guid.NewGuid();
                usr.Department = "Human Resources";
                usr.Office = "San Francisco";
                usr.OfficeCode = "SF";
                usr.Position = "HR Admin Staff";
                usr.MapLocation = "x1,y1,x2,y2";
                usr.AnniversaryStartDate = Convert.ToDateTime("1/1/1901");
            }


           

            return usr;
        }


    }
}
