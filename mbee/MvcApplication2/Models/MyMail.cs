using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Xsl; 

namespace mymail
{
    class MyMail
    {
        static public string prepForGmail(string tagValue)
        {
            string retval = MvcApplication2.Models.TestDataClass1.strMail;
            retval = retval.Replace("$tagValue", tagValue);

            return retval;
        }

        static public void my_XslXmlOut(string fnXml, string fnXsl, string fnOut)
        {
            XslCompiledTransform myXslTransform;
            myXslTransform = new XslCompiledTransform();
            myXslTransform.Load(fnXsl);
            myXslTransform.Transform(fnXml, fnOut); 
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
    }
}
