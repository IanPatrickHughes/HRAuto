using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Data;
using System.Data.Linq;

using HRAutomation.DAL;

namespace _HRAutomation.resources.classes
{
    public static class DBItems
    {

        public static DateTime ReturnLastActionDate()
        {
            DateTime dt = new DateTime();

            using (var db = DatabaseHelper.GetRequestData())
            {
                dt = (from ir in db.InterfaceRecords
                      where ir.WasTest.Equals(false)
                      orderby ir.DateOfAction descending
                      select ir.DateOfAction).FirstOrDefault();
            }


            return dt;

        }

    }
}
