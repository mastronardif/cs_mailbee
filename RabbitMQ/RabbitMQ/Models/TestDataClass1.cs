using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace MvcApplication2.Models
{
    class TestDataClass1
    {
        static public string strMail = ""+
"\n<MYMAIL>" +
"\n<HEADER>" +
"\nFrom: jimmy@joeschedule.mailgun.org" +
"\nTo: Frank Mastronardi <mastronardif@gmail.com>" +
"\nSubject: c# bee joemailweb" +
"\nIn-Reply-To: <CAAAKxgKEqWkQ_v3kPRhY+3ATgM1ePYcCLtv+-1qtT3T=s=AYsAmail.gmail.com>" +
"\nReferences: <CAAAKxgKEqWkQ_v3kPRhY+3ATgM1ePYcCLtv+-1qtT3T=s=AYsAmail.gmail.com>" +
"\nMessage-Id: <FU uddy mailbox-19950-1311902078-753076ww3.pairlite.com>" +
"\nDate: Thu, 13 Oct 2011 21:14:38 -0400" +
"\nMIME-Version: 1.0" +
"Content-Type: text/html; charset=\"UTF-8\"" +
"\n" +
"\n</HEADER>" +
"\n" +
"\n<tags>" +
"\n http://news.google.com/nwshp?hl=en&tab=wn" +
"\n</tags>" +
"\n</MYMAIL>" +
"\n"; 

        static public string strTestJson = ""+
"{                            "+
"  \"root_wb_pf\": {          " +  
"    \"name\": \"bobo\",      "+
"    \"CELL\": [              "+  
"      {                      "+
"        \"Cell\": \"1\",     "+  
"        \"Row\": \"1\",      "+
"        \"Value\": \"123.123\""+
"      },                      "+
"      {                       "+
"        \"Cell\": \"2\",      "+
"        \"Row\": \"1\",       "+
"        \"Value\": \"123.123\""+
"      },                      "+
"      {                       "+
"        \"Cell\": \"3\",       "+
"        \"Row\": \"1\",        "+
"        \"Value\": \"123.123\" "+
"      },                       "+
"      {                        "+
"        \"Cell\": \"1\",       "+
"        \"Row\": \"2\",        "+
"        \"Value\": \"123.123\" "+
"      },                       "+
"      {                        "+
"        \"Cell\": \"2\",       "+
"        \"Row\": \"2\",        "+
"        \"Value\": \"123.123\" "+
"      },                       "+
"      {                        "+
"        \"Cell\": \"3\",       "+
"        \"Row\": \"2\",        "+
"        \"Value\": \"123.123\" "+
"      },                       "+
"      {                        "+
"        \"Cell\": \"1\",       "+
"        \"Row\": \"3\",        "+
"        \"Value\": \"123.123\" "+
"      },                       "+
"      {                        "+
"        \"Cell\": \"2\",       "+
"        \"Row\": \"3\",        "+
"        \"Value\": \"123.123\" "+
"      },                       "+
"      {                        "+
"        \"Cell\": \"3\",       "+
"        \"Row\": \"3\",        "+
"        \"Value\": \"13.123\"  "+
"      }                        "+
"    ]                          "+
"  }                            "+
"}";


        static public string readTestFile(string test)
        {
            string filePath = AppDomain.CurrentDomain.BaseDirectory;
            filePath = filePath +  @"Models\" + test;

            StreamReader streamReader = new StreamReader(filePath, Encoding.UTF8);
            string text = streamReader.ReadToEnd();
            streamReader.Close();

            return text;

            ////Read between the tags.
            ////var element = XElement.Parse(text);
            //HtmlDocument doc = new HtmlDocument();
            //doc.LoadHtml(text);

            //var links = doc.DocumentNode.SelectNodes("//test");

            //if (links == null) 
            //    return "";

            //foreach (var ttt in links)
            //{
            //    if (ttt.Attributes["n"] != null)
            //    {
            //        string sss = ttt.Attributes["n"].Value.ToString();
            //        if (sss.IndexOf(test, StringComparison.OrdinalIgnoreCase) != -1)
            //        {
            //            return ttt.InnerHtml;
            //        }
            //    }
            
            //}
            //return "";
        }
      
    }                           
}                               
