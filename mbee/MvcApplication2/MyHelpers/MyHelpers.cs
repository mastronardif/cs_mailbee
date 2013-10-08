using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Xsl; 


namespace myhelpers
{
    class MyHelpers
    {
        static string getAppDir()
        {
            var dir = AppDomain.CurrentDomain.BaseDirectory;
            return dir + "App_Data";
        }

        static public void ReadFromApp_Data(string fn, ref string results)
        {
            //string uriXML = System.Web.HttpContext.Current.Server.MapPath("~/App_Data/testdata.xml");
            //string uriXML = System.Web.HttpContext.Current.Server.MapPath(fn);

            string uriXML = getAppDir();
            uriXML  = System.IO.Path.Combine(uriXML, fn);
            
            using (System.IO.StreamReader myFile = new System.IO.StreamReader(uriXML))
            {
                results = myFile.ReadToEnd();
            }
        }

        public static void hlp_WriteToFile(string fn, string data)
        {
            // Write the string to a file.
            System.IO.StreamWriter file = new System.IO.StreamWriter(fn);
            file.WriteLine(data);
            file.Close();
        }

    }
}
