using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Xsl; 


namespace myxslxml

{
    class MyXml
    {
        static public void my_XslXmlOut(string fnXml, string fnXsl, string fnOut)
        {
            XslCompiledTransform myXslTransform;
            myXslTransform = new XslCompiledTransform();
            myXslTransform.Load(fnXsl);
            myXslTransform.Transform(fnXml, fnOut); 
        }

        static public void my_XslXmlTransformToString(string strXml, string strXslt, ref string results)
        {
            XslCompiledTransform myXslTransform;
            myXslTransform = new XslCompiledTransform();
            //myXslTransform.Load(fnXsl);

            myXslTransform.Load(new XmlTextReader(new StringReader(strXslt)));

            using (var sw = new StringWriter())
            {
                using (var xw = XmlWriter.Create(sw))
                {
                    // Build Xml with xw.
                    //myXslTransform.Transform(fnXml, xw);           
                    using (var xwi = XmlReader.Create(new StringReader(strXml)))
                    {
                        myXslTransform.Transform(xwi, xw);
                    }

                    results = sw.ToString();
                }
            }
        }


        static public T Deserialize<T>(string xml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (var stringReader = new StringReader(xml))
            {
                T retval = (T)serializer.Deserialize(stringReader);

                return retval;
            }
        }


        //static public void xmlToObj()
        //{
        //    //XmlSerializer serializer = new XmlSerializer(typeof(CG.MyNamespace.root_wb_pfCELL));
        //    XmlSerializer serializer = new XmlSerializer(typeof(CG.MyNamespace.root_wb_pf));

        //    using (XmlReader reader = XmlReader.Create(@"C:\FxM\Dev\myxslxml\myxslxml\App_Data\Sample1.xml"))
        //    {
        //        CG.MyNamespace.root_wb_pf myXmlClass = (CG.MyNamespace.root_wb_pf)serializer.Deserialize(reader);

        //    }
        //}
    }
}
