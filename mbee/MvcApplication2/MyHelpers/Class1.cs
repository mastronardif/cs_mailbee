using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace myhelpers
{
    public class Class1
    {
        static public string makeXML_FromRequestData(string root, HttpRequestBase  Request)
        {
            string retval = string.Empty;

            foreach (string key in Request.Form.Keys)
            {
                string val = Request.Form[key];
                string pair = string.Format("<{0}>{1}</{0}>", key, Request.Form[key]);
                retval += pair;
                retval += "\n";
            }

            retval = string.Format("<{0}>\n{1}\n </{0}>\n", root, retval);

            return retval;

        }
    }
}