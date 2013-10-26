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

namespace mybrowser
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
            //List<string> tags = getTags(src);
            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(src);
                 
            var links = doc.DocumentNode.SelectNodes("//tags");
            if (links == null)
                return dictionary;

            foreach (var ttt in links)
            {
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

//        static public string NormalizeHttpToEmail(string url, string src, string emailRoot, bool bRemoveimgs)
//        {
//            Program._log.Debug(src);
//            string retval = string.Empty;
//            var webGet = new HtmlWeb();
//            HtmlDocument document = new HtmlDocument();
//            const int kMax = 42;

//            document.LoadHtml(src);

//            var urlRoot = new Uri(url);
//            if (!urlRoot.IsAbsoluteUri)
//                urlRoot = new Uri(new Uri(url), urlRoot);

//            var links = document.DocumentNode.SelectNodes("//a[@href] | //img/@src");
//            if (links != null)
//            {
//                foreach (var link in links)
//                {
//                    if (bRemoveimgs && link.Attributes["src"] != null)
//                    {
//                        link.Remove();
//                        continue;
//                    }

//                    HtmlAttribute att = link.Attributes["href"];
//                    if (att == null) continue;

//                    string href = att.Value;

//                    //fm 7/27/13 Uri urlNext = new Uri(href, UriKind.RelativeOrAbsolute);
//                    Uri urlNext = null;

//                    if (Uri.TryCreate(href, UriKind.RelativeOrAbsolute, out urlNext))
//                    {
//                        // Make it absolute if it's relative
//                        if (!urlNext.IsAbsoluteUri)
//                        {
//                           // urlNext = new Uri(urlRoot, urlNext);
//                            if (Uri.TryCreate(urlRoot, urlNext, out urlNext))
//                            {
//                                ;
//                            }
//                        }
//                    }

//                    // Make it absolute if it's relative
//                    //fm 7/27/13  if (!urlNext.IsAbsoluteUri)
//                    //fm 7/27/13 {
////fm 7/27/13                         urlNext = new Uri(urlRoot, urlNext);
//                    //fm 7/27/13                     }

//                    //if (link.Attributes["href"].Value.ToString().Contains("http"))
//                    //FM 7/26/13 if (urlNext.ToString().Contains("http"))
//                    if (urlNext != null && urlNext.ToString().Contains("http"))
//                    {
//                        //Console.WriteLine(link.Attributes["href"].Value);

//                        int iMax = (link.InnerText.Length > kMax) ? kMax : link.InnerText.Length;
//                        string elisp = (link.InnerText.Length > kMax) ? "..." : "";

//                        string subject = link.InnerText.ToString().Substring(0, iMax) + elisp + " joemailweb";
//                        string replacement = @"mailto:" + emailRoot + "?subject=" + subject +
//                                             @"&body=%26lttags img=keep%26gt" +
//                                             @"<tags img=keep>" + urlNext +
//                                             //@"<tags img=keep>" + link.Attributes["href"].Value +
//                                             @"</tags> %26lt%3B%2Ftags%26gt";
//                        link.Attributes["href"].Value = replacement;
//                    }
//                }
//            }

//            using (StringWriter writer = new StringWriter())
//            {
//                document.Save(writer);
//                retval = writer.ToString();
//            }
//            Program._log.Debug(retval);
//            return retval;
//        }


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
