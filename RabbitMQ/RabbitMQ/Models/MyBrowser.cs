using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RabbitMQ.Models
{
    class MyBrowser
    {

        static private List<string> getTags(string src)
        {
            string retval = string.Empty;
            List<string> tags = new List<string>();

            Regex regex = new Regex("(<tags.*?>.*?</tags>)", RegexOptions.Singleline);
            var v = regex.Matches(src);
            int cc = v.Count;
            if (cc > 0)
            {
                foreach (var link in v)
                {
                    string tag = link.ToString().Replace("\n", "");
                    retval += tag;
                    retval += "\n";

                    tags.Add(tag);
                }
            }

            return tags;
        }
       
           
        static public Dictionary<string, string> getUniqueTagUrls(string src)
        {
            Console.WriteLine("\t *** FM getUniqueTagUrls =  " + src);
            //List<string> tags = getTags(src);
            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(src);
                 
            var links = doc.DocumentNode.SelectNodes("//tags");
            if (links == null)
            {
                return dictionary;
            }
            Console.WriteLine("\t *** FM line 54 ");
            foreach (var ttt in links)
            {
                Console.WriteLine("\t *** FM line 59 ");
                string tag = ttt.OuterHtml;
                tag = tag.ToString().Replace("\n", "");

                string url = ttt.InnerText.ToString();
                url = url.Replace("\n", "");
                url = url.Replace("'", "");
                url = url.Replace("\"", "");
                    
                        //if (ttt.Attributes != null)
                        //{
                        //    Console.WriteLine(ttt);
                        //    Console.WriteLine(ttt.InnerText);
                        //    string sss = ttt.InnerText;
                        //    if (ttt.Attributes.Count > 0)
                        //    {
                        //        sss = ttt.Attributes[0].Value;
                        //        sss = ttt.Attributes[0].Name;
                        //    }
                        //}

                // FM 1/21/16
                Console.WriteLine("\t *** FM url =  " + url);
                if (IsBase64(url))
                {
                    // Decode
                    var base64EncodedBytes = System.Convert.FromBase64String(url);
                    url = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
                    Console.WriteLine("\t ***** FM Decoded = " + tag);
                }
                // FM 1/21/16

                        if (!dictionary.ContainsKey(url))
                        {
                            dictionary.Add(url, tag);
                        }
            }

            return dictionary;
        }


        static public List<string> getUniqueTagUrlsOldWay(string src)
        {
            List<string> tags = new List<string>();

            tags.Add("wtf");

            return tags;

        }

        static public string NormalizeHttpToEmail(string url, string src, string emailRoot, bool bRemoveimgs)
        {
            Program._log.Debug(src);
            string retval = string.Empty;
            string strEncoded;
            string urlEmailSafe = string.Empty;
            var webGet = new HtmlWeb();
            HtmlDocument document = new HtmlDocument();
            const int kMax = 42;

            document.LoadHtml(src);

            var urlRoot = new Uri(url);
            if (!urlRoot.IsAbsoluteUri)
                urlRoot = new Uri(new Uri(url), urlRoot);

            var links = document.DocumentNode.SelectNodes("//a[@href] | //img/@src");
            if (links != null)
            {
                foreach (var link in links)
                {
                    if (bRemoveimgs && link.Attributes["src"] != null)
                    {
                        link.Remove();
                        continue;
                    }

                    HtmlAttribute att = link.Attributes["href"];
                    if (att == null) continue;

                    string href = att.Value;

                    //fm 7/27/13 Uri urlNext = new Uri(href, UriKind.RelativeOrAbsolute);
                    Uri urlNext = null;

                    if (Uri.TryCreate(href, UriKind.RelativeOrAbsolute, out urlNext))
                    {
                        // Make it absolute if it's relative
                        if (!urlNext.IsAbsoluteUri)
                        {
                           // urlNext = new Uri(urlRoot, urlNext);
                            if (Uri.TryCreate(urlRoot, urlNext, out urlNext))
                            {
                                ;
                            }
                        }
                    }

                    // Make it absolute if it's relative
                    //fm 7/27/13  if (!urlNext.IsAbsoluteUri)
                    //fm 7/27/13 {
//fm 7/27/13                         urlNext = new Uri(urlRoot, urlNext);
                    //fm 7/27/13                     }

                    //if (link.Attributes["href"].Value.ToString().Contains("http"))
                    //FM 7/26/13 if (urlNext.ToString().Contains("http"))
                    if (urlNext != null && urlNext.ToString().Contains("http"))
                    {
                        //Console.WriteLine(link.Attributes["href"].Value);
                        urlEmailSafe = urlNext.ToString();

                        int iMax = (link.InnerText.Length > kMax) ? kMax : link.InnerText.Length;
                        string elisp = (link.InnerText.Length > kMax) ? "..." : "";

                        // FM 1/21/16 begin
                        // if url GET has args i.e. bbb=3&ccc=5
                        {
                            if (urlNext.Query.Length > 0)
                            {
                                Console.WriteLine("\t ***** FM urlNext BEGIN = " + urlNext.Query);

                                
                                strEncoded = urlNext.ToString();

                                {   // test for exception.
                                    // Decode
                                    if (IsBase64(strEncoded)) 
                                    {

                                    var base64EncodedBytes22 = System.Convert.FromBase64String(strEncoded);
                                    strEncoded = System.Text.Encoding.UTF8.GetString(base64EncodedBytes22);
                                    Console.WriteLine("\t ***** FM Decode = " + strEncoded);         
                                    }
                                }

                                // Encode
                                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(strEncoded);
                                strEncoded = System.Convert.ToBase64String(plainTextBytes);
                                urlEmailSafe = strEncoded;
                                Console.WriteLine("\t ***** FM Encode = " + strEncoded);


                                // Decode
                                var base64EncodedBytes = System.Convert.FromBase64String(strEncoded);
                                strEncoded = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
                                Console.WriteLine("\t ***** FM Decode = " + strEncoded);

                                Console.WriteLine("\t ***** FM urlNext " + "END");



                            }
                            Console.WriteLine("\t ***** FM urlNext = " + urlNext.Query);
                        }


                        // if encoded decode call str = fixString();
                        Console.WriteLine("urlNext = " + urlNext);
                        Program._log.Debug("FM line 79, theUrl = " + urlNext);
                        // 64bitencode urlNext becuase when  <a href="mailto:ah@joeschedule.mailgun.org ?subject &body=___
                        // extra & args get ignored in the email uri and the if the http url has them the get request will not see them and not work.
                        // FM 1/21/16 begin


                        string subject = link.InnerText.ToString().Substring(0, iMax) + elisp + " joemailweb";
                        string replacement = @"mailto:" + emailRoot + "?subject=" + subject +
                                             @"&body=%26lttags img=keep%26gt" +
                                             @"<tags img=keep>" + urlEmailSafe +
//                                             @"<tags img=keep>" + urlNext +
                            //@"<tags img=keep>" + link.Attributes["href"].Value +
                                             @"</tags> %26lt%3B%2Ftags%26gt";
                        link.Attributes["href"].Value = replacement;
                    }
                }
            }

            using (StringWriter writer = new StringWriter())
            {
                document.Save(writer);
                retval = writer.ToString();
            }
            Program._log.Debug(retval);
            return retval;
        }



        static private string NormalizeHttpToEmail33_REMOVE_ME(string url, string src, string emailRoot)
        {
            Program._log.Debug(src);
            string retval = string.Empty;
            var webGet = new HtmlWeb();
            HtmlDocument document = new HtmlDocument();
            const int kMax = 42;

            document.LoadHtml(src);

            //string urlRoot = url.
            var urlRoot = new Uri(url);
            if (!urlRoot.IsAbsoluteUri)
                urlRoot = new Uri(new Uri(url), urlRoot);

            var linksThatDoNotOpenInNewWindow = document.DocumentNode.SelectNodes("//a[@href]");
            if (linksThatDoNotOpenInNewWindow != null)
            {
                foreach (var link in linksThatDoNotOpenInNewWindow)
                {
                    //if (link.Attributes["onclick"] != null)
                    //{
                    //    //link.Attributes["onclick"];
                    //}
                    HtmlAttribute att = link.Attributes["href"];
                    if (att == null) continue;

                    string href = att.Value;

                    Uri urlNext = new Uri(href, UriKind.RelativeOrAbsolute);
                    // Make it absolute if it's relative
                    if (!urlNext.IsAbsoluteUri)
                    {
                        urlNext = new Uri(urlRoot, urlNext);
                    }

                    //if (link.Attributes["href"].Value.ToString().Contains("http"))
                    if (urlNext.ToString().Contains("http"))
                    {
                        //Console.WriteLine(link.Attributes["href"].Value);

                        int iMax = (link.InnerText.Length > kMax) ? kMax : link.InnerText.Length;
                        string elisp = (link.InnerText.Length > kMax) ? "..." : "";

                        string subject = link.InnerText.ToString().Substring(0, iMax) + elisp + " joemailweb";
                        string replacement = @"mailto:" + emailRoot + "?subject=" + subject +
                                             @"&body=%26lttags img=keep%26gt" +
                                             @"<tags img=keep>" + urlNext +
                                             //@"<tags img=keep>" + link.Attributes["href"].Value +
                                             @"</tags> %26lt%3B%2Ftags%26gt";
                        link.Attributes["href"].Value = replacement;
                    }
                }
            }

            using (StringWriter writer = new StringWriter())
            {
                document.Save(writer);
                retval = writer.ToString();
            }
            Program._log.Debug(retval);
            return retval;
        }




        static private string NormalizeHttpToEmail22_REMOVE_ME(string src, string emailRoot)
        {
            Program._log.Debug(src);
            string retval = string.Empty;
            var webGet = new HtmlWeb();
            HtmlDocument document = new HtmlDocument();
            const int kMax = 42;

            document.LoadHtml(src);

            var linksThatDoNotOpenInNewWindow = document.DocumentNode.SelectNodes("//a[@href]");
            if (linksThatDoNotOpenInNewWindow != null)
            {
                foreach (var link in linksThatDoNotOpenInNewWindow)
                {
                    if (link.Attributes["onclick"] != null)
                    {
                        //link.Attributes["onclick"];
                    }

                    if (link.Attributes["href"].Value.ToString().Contains("http"))
                    {
                        //Console.WriteLine (link.Attributes["href"].Value);

                        int iMax = (link.InnerText.Length > kMax) ? kMax : link.InnerText.Length;
                        string elisp = (link.InnerText.Length > kMax) ? "..." : "";

                        string subject = link.InnerText.ToString().Substring(0, iMax) + elisp + " joemailweb";
                        string replacement = @"mailto:" + emailRoot + "?subject=" + subject +
                                             @"&body=%26lttags img=keep%26gt" +
                                             @"<tags img=keep>" + link.Attributes["href"].Value +
                                             @"</tags> %26lt%3B%2Ftags%26gt";
                        link.Attributes["href"].Value = replacement;
                    }
                    //if (link.Attributes["target"] == null)
                    //    link.Attributes.Add("target", "_blank");
                    //else
                    //    link.Attributes["target"].Value = "_blank";
                }
            }

            //document.Save(
            //var linksOnPage = from lnks in document.DocumentNode.Descendants()
            //      where lnks.Name == "a" && 
            //           lnks.Attributes["href"] != null && 
            //           lnks.InnerText.Trim().Length > 0
            //      select new
            //      {
            //         Url = lnks.Attributes["href"].Value,
            //         Text = lnks.InnerText
            //      };

            using (StringWriter writer = new StringWriter())
            {
                document.Save(writer);
                retval = writer.ToString();
            }
            Program._log.Debug(retval);
            return retval;
        
        }

        public static string RemoveStyleAttributes(string strHtml)
        {
            string retval = string.Empty;
            HtmlAgilityPack.HtmlDocument html = new HtmlAgilityPack.HtmlDocument();

            html.LoadHtml(strHtml);

            //var elementsWithStyleAttribute = html.DocumentNode.SelectNodes("//@style");
            var elementsWithStyleAttribute = html.DocumentNode.SelectNodes("//link[@href] | //style/@type");

            if (elementsWithStyleAttribute != null)
            {
                foreach (var element in elementsWithStyleAttribute)
                {
                    //element.Attributes["style"].Remove();
                    element.Remove();
                }
            }

            using (StringWriter writer = new StringWriter())
            {
                html.Save(writer);
                retval = writer.ToString();
            }

            //Program._log.Debug(retval);

            return retval;

            //html.ToString();
        }

        public static bool IsBase64(string base64String)
        {
            // Credit: oybek http://stackoverflow.com/users/794764/oybek
            if (base64String == null || base64String.Length == 0 || base64String.Length % 4 != 0
               || base64String.Contains(" ") || base64String.Contains("\t") || base64String.Contains("\r") || base64String.Contains("\n"))
                ; // wtf  return false;

            try
            {
                Convert.FromBase64String(base64String);
                return true;
            }
            catch (Exception exception)
            {
                // Handle the exception
            }
            return false;
        }

        static private string NormalizeHttpToEmail_REMOVE_ME(string url, string src, string emailRoot)
        {
            return NormalizeHttpToEmail(url, src, emailRoot, false);
            //return NormalizeHttpToEmail33(url, src, emailRoot);
            //return NormalizeHttpToEmail22(src, emailRoot);

            Program._log.Debug("src");
            Program._log.Debug(src);
            Program._log.Debug("/src");
            string retval = string.Empty;
            //string email = "jimmy@joeschedule.mailgun.org";
            string Action = "ae=a";

//$data =~ s/(a.*?href\s*=\s*")(\s*http:.*?)"/"$1mailto:$from\?subject=joemailweb\&body=\%26lt\%3Btags$ae\%26gt\%3B".uri_escape("<tags$ae>".$2."<\/tags>")."\%26lt\%3B\%2Ftags\%26gt\%3B\""/iegs;

            //  string pattern     = @"(a.*?href\s*=\s*"")(\s*http:.*?)""";
                    string pattern = @"(a.*?href.*?=\s*"")(\s*http:.*?)""";
            //string replacement = @"$1mailto:FMJones@bee.com?subject=test joemailweb&body=%26lttags img=keep%26gt" + 
            string replacement = @"$1mailto:" + emailRoot + "?subject=test joemailweb&body=%26lttags img=keep%26gt" + 
                                 @"<tags img=keep>$2</tags> %26lt%3B%2Ftags%26gt""";

            //string options = "iegs";
            Regex rgx = new Regex(pattern, RegexOptions.IgnoreCase|RegexOptions.Multiline);
            retval = rgx.Replace(src, replacement);
            
            Program._log.Debug(retval);
            return retval;
        }

        static public string GetResponse(Uri url)
        {
            string retval = string.Empty;

            retval = get(url);

            return retval;
        }

        static private string get(Uri url)
        {
            string retval = string.Empty;
            using (var wb = new WebClient())
            {
                var response = wb.DownloadString(url);
                retval = response;
            }

            return retval;
        }

        static private string post(Uri url)
        {
            string retval = string.Empty;

            using (var wb = new WebClient())
            {
                var data = new NameValueCollection();
                data["username"] = "myUser";
                data["password"] = "myPassword";
                var response = wb.UploadValues(url, "POST", data);
            }
            return retval;
        }
    }
}
