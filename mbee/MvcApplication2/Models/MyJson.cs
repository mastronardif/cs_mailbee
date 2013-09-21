using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Xsl;
using Newtonsoft.Json;
using System.IO;


namespace MvcApplication2.Models
{
    public class Product
    {
        public string Cell { get; set; }
        public string Row { get; set; }
        public string Value { get; set; }
    }

    public class CELL
    {
        public string Cell { get; set; }
        public string Row { get; set; }
        public string Value { get; set; }
    }

    public class RootWbPf
    {
        public string name { get; set; }
        public List<CELL> CELL { get; set; }
    }

    public class RootObject
    {
        public RootWbPf root_wb_pf { get; set; }
    }



    class MyJson
    {
        static public string test()
        {
            Product product = new Product();
            string json = JsonConvert.SerializeObject(product);

            return json;

        }

        static public string testXMLToJson(string xml)
        {
            TextReader tr = new StreamReader("c:\\Sample22.xml");
            xml = tr.ReadToEnd();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            string jsonText = JsonConvert.SerializeXmlNode(doc);
            return jsonText;
        }

        static public string testJsonToXML(string json)
        { 
            string strRetval = string.Empty;
            try
            {
                XmlDocument xmlDoc = (XmlDocument)Newtonsoft.Json.JsonConvert.DeserializeXmlNode(json);
//                XmlTextWriter writer = new XmlTextWriter("json.xml", null);
                StringWriter sw = new StringWriter();  
                XmlTextWriter xw = new XmlTextWriter(sw);  
                xw.Formatting = System.Xml.Formatting.Indented;
                xmlDoc.WriteTo(xw);

                strRetval = sw.ToString();

            }
            catch (Exception eee)
            {
                Console.WriteLine(eee.ToString());
            }

            return strRetval;

        }

        static public string testObjToJson(Object obj)
        {
            string strRetval = string.Empty;
            try
            { 

                strRetval = JsonConvert.SerializeObject(obj);
            //JavaScriptSerializer serializer = new JavaScriptSerializer()
            //return serializer.Serialize(YOURLIST);  
            }
            catch (Exception eee)
            {
                Console.WriteLine(eee.ToString());
            }

            return strRetval;
        }

        static public string testJsonToObj(string json)
        {
            //var obj2 = serializer.Deserialize<Product>(output);
            //JsonConvert.DeserializeObject<T>(json_data)
            string strTest = TestDataClass1.strTestJson;

            var json_data = string.Empty;
            try 
            {
                json_data = strTest; // json;
            }
            catch (Exception eee) 
            {
                Console.WriteLine(eee.ToString());
            }

    // if string with JSON data is not empty, deserialize it to class and return its instance 
    //return !string.IsNullOrEmpty(json_data) ? JsonConvert.DeserializeObject<T>(json_data) : new T();


              //Product pp = JsonConvert.DeserializeObject<Product>(json_data);
            //List<Product> pps = JsonConvert.DeserializeObject<List<Product>>(json_data);

            //RootWbPf pps2 = JsonConvert.DeserializeObject<RootWbPf>(json_data);

            RootObject pps3 = JsonConvert.DeserializeObject<RootObject>(json_data);

            pps3 = JsonConvert.DeserializeObject<RootObject>(json);


            testObjToJson(pps3);
            return json_data;

        }

        static public string testExcel()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("\n[\n ");
            for (int iRow = 1; iRow < 4; iRow++)
            {
                
                for (int iCell = 1; iCell < 4; iCell++)
                {
                    sb.Append("{");
                    string item = string.Format("\"Cell\":\"{0}\",\"Row\":\"{1}\",\"Value\":\"{2}\"", iCell, iRow, 123.123);
                    sb.Append(item);
                    sb.Append("}");
                    sb.Append("\n,");
                }

                // chop
                for (int iii = "\n,".Length; iii > 0; iii--) { sb.Length--; }

                sb.Append("\n");

                sb.Append(",");
            }
            // chop
            for (int iii = ",".Length; iii > 0; iii--) { sb.Length--; }

            sb.Append("]");

            string str = sb.ToString();
            return str;
        }
                            /*********
    [
{ "Cell":1,"Row":1,"Value":null}
,{"Cell":2,"Row":1,"Value":null}
,{"Cell":3,"Row":1,"Value":null}
]
                             *                        ****/
    
     

    }
}


/****************
 * Notes:
 * http://json2csharp.com/
 * http://james.newtonking.com/pages/json-net.aspx
 * http://www.freeformatter.com/json-to-xml-converter.html#ad-output
 * http://www.utilities-online.info/xmltojson/
 * http://www.softlion.com/webTools/XmlPrettyPrint/default.aspx
 * http://www.w3schools.com/xml/tryxslt.asp?xmlfile=simple&xsltfile=simple
 * 
 * ﻿http://support.appharbor.com/kb/getting-started/deploying-your-first-application-using-git
[
 {
  "Cell": "ann"
  ,"Row": "ann"
  ,"Value": "ann"
 }
]
 * 
public class RootObject
{
    public string Cell { get; set; }
    public string Row { get; set; }
    public string Value { get; set; }
}
 * 
<root>
   <Cell null="true" />
   <Row null="true" />
   <Value null="true" />
</root>

*********************/

/*****
 
Fiddler Notes!
Fiddler post, request, body text

http://localhost:57987/Home/publish22

User-Agent: Fiddler
Content-Type: application/x-www-form-urlencoded
Host: localhost:57987
Content-Length: 592


message-headers=[["Received",
"by 184.173.173.18 with SMTP mgrt -1057111050; Tue, 17 Jan 2012 18:56:20 +0000"],
["Subject",
"curl test27 joemailweb"],
["From",
"Frank Mastronardi <mastronardif@gmail.com>"],
["X-Mailer",
"iPod Mail (9A405)"],
["Message-Id",
"<8950389B-9325-4430-83FB-343C91A319D9@gmail.com>"],
["Date",
"Tue, 17 Jan 2012 13:56:17 -0500"],
["To",
"\"joemail@joeschedule.mailgun.org\" <joemail@joeschedule.mailgun.org>"],
["X-Mailgun-Spf",
"Neutral"]]
&sender=mastronardif@gmail.com 
&body-plain=<tags>http://www.joeschedule.com</tags>
&recipient=test@joeschedule.mailgun.org

 * 
 Curl notes!
 curl http://postbin.ryanbigg.com/96cfee41 \
curl http://csmailbee.apphb.com/ \
curl http://csmailbee.apphb.com/Home/publish22 \
curl http://localhost:57987/Home/publish22 \


curl http://localhost:57987/Home/publish22 \
-F message-headers='[["Received",
"by 184.173.173.18 with SMTP mgrt -1057111050; Tue, 17 Jan 2012 18:56:20 +0000"],
["Subject",
"curl test27 joemailweb"],
["From",
"Frank Mastronardi <mastronardif@gmail.com>"],
["X-Mailer",
"iPod Mail (9A405)"],
["Message-Id",
"<8950389B-9325-4430-83FB-343C91A319D9@gmail.com>"],
["Date",
"Tue, 17 Jan 2012 13:56:17 -0500"],
["To",
"\"joemail@joeschedule.mailgun.org\" <joemail@joeschedule.mailgun.org>"],
["X-Mailgun-Spf",
"Neutral"]]' \
-F sender=mastronardif\@gmail.com \
-F body-plain=' <tags>http://www.joeschedule.com</tags>' \
-F recipient=test\@joeschedule.mailgun.org \

******/