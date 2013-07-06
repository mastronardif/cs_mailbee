using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Xml;
using MvcApplication2.Models;
using Newtonsoft.Json;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


namespace MvcApplication2.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            string xml = "<Test><Name>Test class</Name><X>100</X><Y>200</Y></Test>";

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            string json = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);
            
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            string xml = "<Test><Name>Test class</Name><X>100</X><Y>200</Y></Test>";

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            string json = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);
            //return json;
            return "{\"Name\":\"asslass\",\"X\":\"100\"}";

            //return "value";
        }

        // POST api/values
        //public void Post(string value)
        //public void Post([FromBody]string value)
        //{
        //    string sss;
        //    if (1 == 1)
        //    {
        //        sss = "asdfasdf";
        //    }
        //}

        //public string Post(MyClass m)
        public string Post([FromBody]MyClass mmm) // works
        {
            string retval = "wtf";
            string action = string.Empty;
            //return "value1, value2";
            try
            {
                string xml = "<Test><Name>Test class</Name><X>100</X><Y>200</Y></Test>";
                xml = mmm.id.Trim();

                XmlDocument doc = new XmlDocument();
                doc.PreserveWhitespace = false;
                
                doc.LoadXml(xml);
                //doc.GetElementsByTagName("Action");
                XmlNodeList elemList = doc.GetElementsByTagName("Action");
                
                for (int i = 0; i < elemList.Count; i++)
                {
                    // version 2 will have actoins
                    action = elemList[i].InnerXml;
                }

                // sw/ on action
                if (!string.IsNullOrWhiteSpace(action))
                {
                    action = action.Trim();
                    if (string.Compare("PING", action, true) == 0)
                    {
                        {
                            MemoryStream memStream = new MemoryStream();
                            XmlTextWriter xmlTextWriter = new XmlTextWriter(memStream, System.Text.UTF8Encoding.ASCII);

                            xmlTextWriter.Formatting = System.Xml.Formatting.Indented;
                            xmlTextWriter.Indentation = 4;
                            xmlTextWriter.QuoteChar = '\'';

                           // Write the XML into a formatting XmlTextWriter
                            doc.WriteContentTo(xmlTextWriter);

                            xmlTextWriter.Flush();
                            memStream.Flush();


                            // Have to rewind the MemoryStream in order to read
                            // its contents.
                            memStream.Position = 0;

                            StreamReader streamReader = new StreamReader(memStream);

                            // Extract the text from the StreamReader.
                            String sFormattedXML = streamReader.ReadToEnd();
                            return sFormattedXML;
                        }
                    }

                    if (string.Compare("SP_HELP", action, true) == 0)
                    {
                        return "<SP_HELP lastmodified= \"4/16/13 10:26 AM\"> </SP_HELP>";
                    }

 

                    if (string.Compare("ABOUT", action, true) == 0)
                    {
                        retval = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n" +
                                 "<data>" +
                                 "<file>bobysfishouse.xsl</file>\n" +
                                 "<version>1.01</version>" +
                                 "</data>";
                        return retval;
                    }
                }

                // Unknown Action
                return "<SP_HELP lastmodified= \"4/16/13 10:26 AM\"> </SP_HELP>";
            }
            catch (Exception eee)
            {
                retval = "wtf\n" + eee.Message;
                return retval;
            }

        }


        //public IEnumerable<string> Post([FromBody]string value)
        //{
        //    string sss;
        //    if (1 == 1)
        //    {
        //        sss = "asdfasdf";
        //    }
        //    return new string[] { "value1", "value2" };

        //}

       

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}

/******
 * Notes: Fidler
Content-Type: application/json
Host: localhost:61295
Content-Length: 18


request Body:

{id:"102 pllolok"}

 * 
POST http://mvcapplication2-2.apphb.com/api/values/id HTTP/1.1
User-Agent: Fiddler
Content-Type: application/json
Host: mvcapplication2-2.apphb.com
Content-Length: 31

{id:"<Action>sp_help</Action>"}
 * 
 * 
 * http://localhost:61295/api/values
 * http://localhost:61295/api/values/id
 * 
 * POST
 * 
 * requestBody:
 * {id:"<Action>sp_help</Action>"}
 * {id:"<Action>run_calculation</Action>"}
*************/