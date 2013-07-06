using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication2.Models
{
    public class Semantics
    {
        static public string Help = @"" +
        "<SP_HELP lastmodified= \"4/16/13 10:26 AM\">" +

        "<Action> SP_HELP </Action>" +
        "<description> Help about this Excel file</description>" +

        "<Action> ABOUT </Action>" +
        "<description> File version and seal information.</description>" +

        "<Action> run_calculation </Action>" +
        "<description> run the calculation</description>" +

        "<Action> PING </Action>" +
        "<description>Return what you send me</description>" +

"<Input_Example>" +
"<!-- Input test data for worksheet x -->" +
"<Action> run_calculation </Action>" +
"<root_wb_pf>" +
" <CELL>  <Cell>$F8</Cell>   <Value> 14      </Value> </CELL>" +
" <CELL>  <Cell>$F9</Cell>   <Value> 125     </Value> </CELL>" +
" <CELL>  <Cell>$F10</Cell>  <Value> 1100000 </Value> </CELL>" +
" <name>Vaccine model</name>" +
"</root_wb_pf>" +
"</Input_Example>" +
"</SP_HELP>";


        static public string UnknowRoot_wb_pf = "" +
            "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n" +
            "<Status>" +
            "<StatusCode> 001 </StatusCode>" +
            "<StatusDesc> Bad root_wb_pf</StatusDesc>" +
            "</Status>";

        static public string UnknownRequest = "" +
            "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n" +
            "<data>" +
            "<StatusCode> 1 </StatusCode>" +
            "<description>Unknown Request.</description>\n" +
            "</data>"+
            "";
    }
}