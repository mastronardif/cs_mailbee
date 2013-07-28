using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Xsl;
using Newtonsoft.Json;
using System.IO;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;


namespace mymailgun
{
    class MyMailGun
    {
        static public string makeReplyFromMailGun(FormCollection collection)
        {
            string retval = string.Empty;
            string recp = collection["recipient"];            
            string top = string.Empty;
            string bot = string.Empty;
            string jsonHeader = collection["message-headers"];

            if (jsonHeader == null)
                return string.Empty;

//            string jsonHeader = @"[
//   {
//     'Name': 'Product 1',
//     'ExpiryDate': '2000-12-29T00:00Z',
//     'Price': 99.95,
//     'Sizes': null
//   },
//   {
//     'Name': 'Product 2',
//    'ExpiryDate': '2009-07-31T00:00Z',
//    'Price': 12.50,
//    'Sizes': null
//  }
//]";
            retval     = "\n<MYMAIL>\n";

            //JObject rss = JObject.Parse(jsonHeader);
            var results = JsonConvert.DeserializeObject<dynamic>(jsonHeader);

            JArray jArr = (JArray)JsonConvert.DeserializeObject(jsonHeader);

            for (int iii = 0; iii < jArr.Count; iii += 2)
            {
                string str = jArr[iii].ToList().ToArray()[1].ToString();
            
            }

            foreach (JArray lr in jArr)
            {
                string str22 = lr[1].ToString();

                string left = lr[0].ToString();
                //#Make the reply - To: is set to original sernder, From: is set to orignal joemail host.

                if (string.Compare(left, "Message-Id", true) == 0)
                {
                    top += string.Format("{0}: FM {1} \n", left, lr[1].ToString());
                    continue;
                }

                if (string.Compare(left, "From", true) == 0)
                {
                    top += string.Format("{0}: {1} \n", left, recp);
                    continue;
                }

                if (string.Compare(left, "To", true) == 0)
                {
                    string sender =  collection["sender"];
                    top += string.Format("{0}: {1} \n", left, sender);
                    continue;
                }

//{
//      my $recp = $query->{'recipient'}; # "joemail\@joeschedule.com"; #$query->param('r
//      $retval .=  "$FFF[0][0]:  $recp \n";
//      next;
//}

//if ($FFF[0][0] =~ m/^To$/i)
//{
//      my $sender =  $query->{'sender'};
//      $retval .=  "$FFF[0][0]:  $sender\n";
//      next;
//}
                //mailheader .= "$FFF[0][0]: $FFF[0][1]\n";
                bot += lr[0] + ": " + lr[1] + "\n";           
            }

            string mailheader = "<HEADER>\n";
            mailheader += top;
            mailheader += bot; 

            //#my $Reply
            //# I don't know for some reason mailgun does not set this filed.
            mailheader += "Content-Type: text/html; charset=\"UTF-8\"";

            mailheader +=  "\n</HEADER>\n";

            retval +=  mailheader + "\n";

            string body = collection["body-html"];
            if (body == null)
            {
                body = collection["body-plain"];
            }           
            retval +=  body;

            retval +=  "\n</MYMAIL>\n";

            return retval;
        }
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