using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace MvcApplication2.Models
{
   [XmlRoot("Root"), XmlType("Root")]
    public class Root
    {
    }


    [System.Xml.Serialization.XmlRootAttribute(Namespace = "",  IsNullable = false)]
    public class CELL22
    {
        [System.Xml.Serialization.XmlElement("Cell", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Cell { get; set; }
        [System.Xml.Serialization.XmlElement("Row", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Row { get; set; }
        [System.Xml.Serialization.XmlElement("Address", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Address { get; set; }
    }

    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public class RootWbPf22
    {
        [System.Xml.Serialization.XmlElement("name", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string name { get; set; }
        [System.Xml.Serialization.XmlElement("CELL", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public List<CELL> CELL { get; set; }
    }

    [Serializable, XmlRoot("RootObject"), XmlType("rootobject")]
    public class RootObject22
    {
        [System.Xml.Serialization.XmlElement("root_wb_pf", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public RootWbPf root_wb_pf { get; set; }
    }

}