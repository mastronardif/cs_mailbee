using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication2.Models
{
    public class Class1
    {
    }

    public class MyClass
    {
        public string id { get; set; }

        static public string testData_json()
        {
            string json11 = ""+
 "{                       "+  
 "  cDataSet:[      {     "+
 "        Customer:Acme,  "+
 "        Contact:john,   "+
 "        Total:100,      "+
 "        Country:US      "+
 "     },                 "+
 "     {                  "+
 "        Customer:Smiths,"+
 "        Contact:fred,   "+
 "        Total:460,      "+
 "        Country:UK      "+  
 "     },                 "+
 "     {                  "+
 "        Customer:Jones, "+
 "        Contact:joe,    "+
 "        Total:24,       "+
 "        Country:US      "+
 "     },                 "+
 "     {                   "+
 "        Customer:Renault,"+
 "        Contact:Marie,   "+
 "        Total:536,       "+
 "        Country:FRANCE    "+
 "     },                   "+
 "     {                    "+
 "        Customer:Schneider,"+
 "        Contact:Hans,      "+
 "        Total:1334,        "+
 "        Country:Germany    "+
 "     }                     "+
 "   ]                       "+
 "}";

            //////////////////////////
string json = ""+
"{               "+   
"record: [      "+
"{               "+
"ROW:1,       "+
"CELL:A,      "+
"Value:123.87 "+
"},              "+
"{               "+
"ROW:1,       "+
"CELL:B,      "+
"Value:23.87  "+
"}               "+
"]               "+   
"}";




            return json;
        
        }
    }

    
}

/*************
 * 
  <?xml version="1.0" encoding="UTF-8" ?>
	<record>
		<ROW>1</ROW>
		<CELL>A</CELL>
		<Value>123.87</Value>
	</record>
	<record>
		<ROW>1</ROW>
		<CELL>B</CELL>
		<Value>23.87</Value>
	</record>
	

******************/