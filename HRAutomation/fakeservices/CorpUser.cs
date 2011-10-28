using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _HRAutomation.fakeservices
{  
    public class CorpUser
    {

        #region
            public string FullName{get;set;}
        
            public string LastName{get;set;}
        
            public string FirstName{get;set;}
        
            public string MiddleName{get;set;}
        
            public string Office{get;set;}
        
            public string OfficeCode{get;set;}
        
            public string DepartmentCode{get;set;}
        
            public string Department{get;set;}
        
            public string Extension{get;set;}
        
            public string DirectDial{get;set;}
        
            public string LoginID{get;set;}
        
            public string Position{get;set;}
        
            public string MapLocation{get;set;}
        
            public string Email{get;set;}

            public DateTime AnniversaryStartDate {get;set;}
        
            public Guid PersonGUID{get;set;}
        #endregion


        /// <summary>
        /// This is a fake CorpUser object to emulate previous behavior when this 
        /// application resided on a network leverageing AD user objects for authentication
        /// </summary>
        public CorpUser()
        {}

    }
}