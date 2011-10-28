using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;


namespace _HRAutomation.resources.classes.utility
{
    /// <summary>
    /// Silly little utility class that performs many functions used by various
    /// areas, which remains static to avoid instancing or inheriting
    /// </summary>
    public static class helperCentral
    {

        /// <summary>
        /// Returns the QString value
        /// </summary>
        /// <param name="qStringKey"></param>
        /// <returns></returns>
        public static string ReturnQueryStringValue(string qStringKey)
        {
            string qstring = "";
            if (System.Web.HttpContext.Current != null)
            {
                if (System.Web.HttpContext.Current.Request[qStringKey] != null)
                {
                   
                        qstring = System.Web.HttpContext.Current.Request[qStringKey].ToString();
                   
                }
            }
            return qstring;
        }


        /// <summary>
        /// Returns the name of the page the user is currently at 
        /// </summary>
        /// <returns></returns>
        public static string ReturnPageName()
        {
            return System.Web.HttpContext.Current.Request.Path.Substring(System.Web.HttpContext.Current.Request.Path.LastIndexOf("/") + 1);

        }


        /// <summary>
        /// Translates the tilde path into the app path, word.
        /// </summary>
        /// <param name="path"></param>
        public static string ExpandTildePath(string path)
        {
            string reference = path;

            //If the string passed-in contains a tilde then we can run
            //this 
            if (reference.Substring(0, 2) == "~/")
            {
                string appPath = "";
                HttpContext _Context = HttpContext.Current;

                //I developing local then I want to build the correct URL here
                if (_Context.Request.Url.Host.ToLower().Contains("localhost"))
                {
                    appPath = String.Format("http://{0}:{1}/"
                                            , _Context.Request.Url.Host.ToLower()
                                            , _Context.Request.Url.Port.ToString()
                                            );
                }
                else
                {
                    appPath = String.Format("http://{0}/"
                                            , _Context.Request.Url.Host.ToLower()
                                            );
                }

                return appPath + reference.Substring(2);
            }

            return path;
        }


        /// <summary>
        /// Get a supa-dupa data table back from a Generic List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static DataTable convertToTable<T>(List<T> list)
        {

            DataTable table = new DataTable();

            if (list.Count > 0)
            {

                PropertyInfo[] properties = list[0].GetType().GetProperties();
                List<string> columns = new List<string>();


                foreach (PropertyInfo pi in properties)
                {
                    table.Columns.Add(pi.Name);
                    columns.Add(pi.Name);
                }



                foreach (T item in list)
                {
                    object[] cells = getValues(columns, item);
                    table.Rows.Add(cells);
                }

            }

            return table;

        }

        private static object[] getValues(List<string> columns, object instance)
        {

            object[] ret = new object[columns.Count];

            for (int n = 0; n < ret.Length; n++)
            {

                PropertyInfo pi = instance.GetType().GetProperty(columns[n]);
                object value = pi.GetValue(instance, null);
                ret[n] = value;
            }


            return ret;

        }


        /// <summary>
        /// Takes and object and returns a DataTable
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static DataTable ConvertToDataTable(Object o)
        {
            PropertyInfo[] properties = o.GetType().GetProperties();
            DataTable dt = CreateDataTable(properties);
            FillData(properties, dt, o);
            return dt;
        }

        /// <summary>
        /// Takes an object array and returns a DataTable
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static DataTable ConvertToDataTable(Object[] array)
        {
            PropertyInfo[] properties = array.GetType().GetElementType().GetProperties();
            DataTable dt = CreateDataTable(properties);


            if (array.Length != 0)
            {
                foreach (object o in array)
                {
                    FillData(properties, dt, o);
                }
            }

            return dt;

        }

        /// <summary>
        /// Helper method for other converters
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        private static DataTable CreateDataTable(PropertyInfo[] properties)
        {
            DataTable dt = new DataTable();
            DataColumn dc = null;

            foreach (PropertyInfo pi in properties)
            {

                dc = new DataColumn();
                dc.ColumnName = pi.Name;
                dc.DataType = pi.PropertyType;

                dt.Columns.Add(dc);
            }

            return dt;

        }

        /// <summary>
        /// Helper for other methods for converting 
        /// </summary>
        /// <param name="properties"></param>
        /// <param name="dt"></param>
        /// <param name="o"></param>
        private static void FillData(PropertyInfo[] properties, DataTable dt, Object o)
        {

            DataRow dr = dt.NewRow();
            foreach (PropertyInfo pi in properties)

                dr[pi.Name] = pi.GetValue(o, null);

            dt.Rows.Add(dr);

        }


        public static System.Web.UI.WebControls.Label ReturnErrorLabel(string message)
        {
            System.Web.UI.WebControls.Label label = new System.Web.UI.WebControls.Label();
            label.Text = message;
            label.ForeColor = System.Drawing.Color.Red;
            label.ID = "lblErrorMessage";

            return label;
        }


        public static string StripAllHTMLTags(string inputString)
        {
            string HTML_TAG_PATTERN = "<.*?>";

            return Regex.Replace
               (inputString, HTML_TAG_PATTERN, string.Empty);
        }

        public static string WikiContentClean(string inputContent)
        {
            string tmpClean = inputContent;
            string HTML_TAG_PATTERN = "<.*?>";

            //Reg-Ex handler...
            tmpClean = Regex.Replace(tmpClean, HTML_TAG_PATTERN, string.Empty); //remove all HTML
            //Remove RTF
            Regex regexObject = new Regex(@"({\\)(.+?)(})|(\\)(.+?)(\b)");
            tmpClean = regexObject.Replace(tmpClean, "");
            tmpClean = Regex.Replace(tmpClean, "<script.*?</script>", string.Empty); //remove all JavaScript


            //remove special...
            tmpClean = tmpClean.Replace("&nbsp;", " "); //remove special
            tmpClean = tmpClean.Replace("&quot;", " ");
            tmpClean = tmpClean.Replace("&ndash;", " ");
            tmpClean = tmpClean.Replace("&amp;", " ");
            tmpClean = tmpClean.Replace("\r", " ");
            tmpClean = tmpClean.Replace("\n", " ");

            //Send it home...
            return tmpClean;
        }

        /// <summary>
        /// Finds a Control recursively. Note finds the first match and exists
        /// </summary>
        /// <param name="ContainerCtl"></param>
        /// <param name="IdToFind"></param>
        /// <returns></returns>
        public static Control FindControlRecursive(Control Root, string Id)
        {
            if (Root.ID == Id)
            {
                return Root;
            }

            foreach (Control Ctl in Root.Controls)
            {
                Control FoundCtl = FindControlRecursive(Ctl, Id);
                if (FoundCtl != null)
                {
                    return FoundCtl;
                }
            }

            return null;

        }

        /// <summary>
        /// Returns a string substring and handles for lengths that are longer than string length
        /// </summary>
        /// <param name="input"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static string ReturnSubString(string input, int len)
        {
            try
            {
                if (input.Length < len)
                {
                    len = input.Length;
                }

                return input.Substring(0, len);
            }
            catch
            {
                return "";
            }

        }

        public static string ReturnFormattedShortDate(string _date)
        {

            System.Globalization.DateTimeFormatInfo dtf = new System.Globalization.DateTimeFormatInfo();
            dtf.TimeSeparator = ".";
            DateTime dt = DateTime.Parse(_date, dtf);

            return dt.ToShortDateString();
        }



    }//END public static class helperCentral
}
