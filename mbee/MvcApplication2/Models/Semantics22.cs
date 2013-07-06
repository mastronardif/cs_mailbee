using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;

namespace MvcApplication2.Models
{
    static public class Semantics22
    {
        static public string doRequest(string req)
        { 

            string retval = "wtf";
            string action = string.Empty;

            try
            {
                string xml = "<Test><Name>Test class</Name><X>100</X><Y>200</Y></Test>";
                xml = req; //mmm.id.Trim();

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
                        return Semantics.Help;
                    }

                    //<Action>run_calculation</Action>
                    if (string.Compare("run_calculation", action, true) == 0)
                    {
                        // cheesy validatation until......
                        // we only do one workbook at a time.  we only do [0]. in the future we can do more.
                        xml = req; //mmm.id.Trim();

                        if (doc.GetElementsByTagName("root_wb_pf").Count != 0 &&
                            doc.GetElementsByTagName("root_wb_pf")[0].OuterXml.Trim().Length > 1)
                        {
                            xml = doc.GetElementsByTagName("root_wb_pf")[0].OuterXml.Trim();
                            retval = MySGExcel.runCalculation(xml);
                        }
                        else 
                        {
                            return Semantics.UnknowRoot_wb_pf;
                        }

                        return retval;
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
                return Semantics.UnknownRequest;
            }
            catch (Exception eee)
            {
                retval = "wtf\n" + eee.Message;
                return retval;
            }

        }
        
    }
}