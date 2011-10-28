using System;
using System.Web;
using _HRAutomation.fakeservices;


namespace _HRAutomation.resources.classes.utility
{
    /// <summary>
    /// Open class for handling the session compression and variables for the user object when a person hits this site.
    /// </summary>
    public class UserHandler
    {
        private string ID { get; set; }
        public CorpUser UserObject { private set; get; }

        public UserHandler() { }


        #region /// <static methods> ...
        /// <summary>
        /// Returns if the user object was set yet for the Word Processing request form
        /// </summary>
        /// <returns></returns>
        public static bool IsUserSet()
        {
            
            bool isSet = false;

            if (HttpContext.Current.Session["currentUser"] != null)
            {
                isSet = true;
            }

            return isSet;
        }

        /// <summary>
        /// Gets the current session state user...
        /// </summary>
        /// <returns></returns>
        public static UserHandler ReturnCurrentStoredUser()
        {
            var uH = new UserHandler();

            if (UserHandler.IsUserSet())
            {
                uH = (UserHandler)HttpContext.Current.Session["currentUser"];
            }


            return uH;
        }

        /// <summary>
        /// Returns the logged in person...
        /// </summary>
        /// <returns></returns>
        public static string ReturnLogInID()
        {
            //NOTE:
            //Previously when this was in place in the corporate network
            //I basically returned a logged in WindowsIdentity (WindowsIdentity wi = WindowsIdentity.GetCurrent();) since
            //I  was using AD and checking for authenticated users.
            //In this case I'm just returning a dummy ID to be used by
            //service higher up in the application....
            //Obviously you would want to structure how you check and authenticate
            //users to match whatever you would use this for...which should prolly be nothing, honestly. :)
            string userName = String.Empty;
            
            
            try
            {
                userName = "adminLevelUser";
            }
            catch
            {
                userName = "";
            }

            return userName;

        }
        #endregion


        /// <summary>
        /// Gets the user's information by login ID after object is constructed
        /// </summary>
        public void CreateUserSession()
        {
            //So, again, like throughout this
            //trash heap, basically Im trying to quickly 
            //emulate the same behavior that I had implemented
            //previously in the corp AD environment. 
            //Because the whole thing lived inside of the corp intranet, 
            //basically we had the luxury of just setting needed items
            //into a session to be used through the application and not
            //worry too too much about security restrictions...so thats whats here

            this.ID = UserHandler.ReturnLogInID();
            var fsu = new FakeUserService();

            var objUsr = fsu.GetUserByLogin(ID);
            this.UserObject = objUsr;

            //Finally set it into a session state...
            HttpContext.Current.Session["currentUser"] = this;

        }
    }
}
