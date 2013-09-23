using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using mymail;
using myrabbitmq;
using RabbitMQ.Models;
using System.Configuration;
using System.Net.Mail;


namespace RabbitMQ.Business
{

    public class busMail
    {
        public const string joemailweb = "joemailweb";
        public const string joeping    = "joeping";

        static class myRabbitMQVitals
        {
            public static string _emailRoot = ConfigurationManager.AppSettings["EmailRoot"];
            public static string _tagLine = ConfigurationManager.AppSettings["TagLine"];
        }

        static public string sendToGmail(string tag)
        {
            string retval = MyMail.prepForGmail(tag);
            return publish(retval);
        }

        static private bool CheckWhiteList(MailMessage hdr)
        {
            return true;
        }

        static private bool CheckVitals(MailMessage hdr)
        {
            return true;
        }


        static public string Reply(string msg)
        {
            string retval = string.Empty;
            string sendReplys = string.Empty;
            string resp;

            try
            {
                MailMessage hdr = MyMail.GetVitals(msg);
                // Does it pass miniaml values test(s).
                if (!CheckVitals(hdr)) { return "Failed minimal values!"; }
                if (!CheckWhiteList(hdr)) { return "Failed whitelist test!"; }

                // vitals
                string from = hdr.From.Address; //myRabbitMQVitals._emailRoot;
                string to = hdr.To[0].Address; //hdr.To.ToList().ToString(); // make the comma sep list, and remove whitelist
                //string cc, bcc = hdr.To.ToList().ToString(); // make the comma sep list, and remove whitelist
                string subject = hdr.Subject.ToString();
                //string body = hdr.Body;

                // Switch on action(Subject) joemailweb, or joeping every thing else.            
                if (subject.IndexOf(busMail.joemailweb, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    // get tag urls
                    //List<string> tags = MyBrowser.getUniqueTagUrls(hdr.Body);
                    Dictionary<string, string> tags = MyBrowser.getUniqueTagUrls(hdr.Body);
                    retval += "NO TAGS FOUND!\n" + hdr.Body;

                    // for each Tag/url do _____________, mail it
                    
                    foreach (var tag in tags)
                    {
                        string theUrl = tag.Key;
                        // get value
                        // get attribs
                        Uri url = new System.Uri(theUrl);
                        resp = MyBrowser.GetResponse(url);

                        // remove style shits. iPhone can not handle all the styles.
                        resp = MyBrowser.RemoveStyleAttributes(resp);

                        // Noramlize the a href to email, and reomve images if requested.
                        resp = MyBrowser.NormalizeHttpToEmail(url.AbsoluteUri, resp, from, false); //myRabbitMQVitals._emailRoot);
                        //Program._log.Debug(retval);

                        string tagLine = "<br/>"+myRabbitMQVitals._tagLine;
                        tagLine += "<hr></hr>";
                        tagLine += url; //encode_entities($tag);
                        //_tagLine

                        sendReplys += mymail.MyMail.SendByMG22(to, from, resp + tagLine);
                        sendReplys += "\n";
                    }
                    if (!string.IsNullOrWhiteSpace(sendReplys) )
                    {
                        retval = sendReplys;
                    }
                }
                else
                {
                    // joeping 
                    Uri urlDetermine = new System.Uri("http://yahoo.com");  // FM fix this

                    resp = MyBrowser.NormalizeHttpToEmail(urlDetermine.AbsoluteUri, hdr.Body, from, false); //myRabbitMQVitals._emailRoot);

                    //Program._log.Debug(retval);

                    retval = mymail.MyMail.SendByMG22(to, from, resp);
                    retval += "\n";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return retval;
        }

        static public string popOne()
        {
            // pop a message from the queue.
            string retval = MyRabbotMQ.Pop();
            
            return Reply(retval);

            //// Parse the data for vitals.
            ////retval = MvcApplication2.Models.TestDataClass1.strMail;
            //  //retval = MvcApplication2.Models.TestDataClass1.readTestFile("Testjoemailweb.txt");
            ////retval = MvcApplication2.Models.TestDataClass1.readTestFile("Testjoeping.txt");


            //MailMessage hdr = MyMail.GetVitals(retval);
            //// Does it pass miniaml values test(s).
            //if (!   CheckVitals(hdr)) { return "Failed minimal values!"; }
            //if (!CheckWhiteList(hdr)) { return "Failed whitelist test!"; }

            //// vitals
            //string from = myRabbitMQVitals._emailRoot; 
            //string to = hdr.To[0].Address; //hdr.To.ToList().ToString(); // make the comma sep list, and remove whitelist
            ////string cc, bcc = hdr.To.ToList().ToString(); // make the comma sep list, and remove whitelist
            //string subject = hdr.Subject.ToString();
            ////string body = hdr.Body;

            //// Switch on action(Subject) joemailweb, or joeping every thing else.            
            //if (subject.IndexOf(busMail.joemailweb, StringComparison.OrdinalIgnoreCase) >= 0)
            //{
            //    // get tag urls
            //    //List<string> tags = MyBrowser.getUniqueTagUrls(hdr.Body);
            //    Dictionary<string, string>  tags = MyBrowser.getUniqueTagUrls(hdr.Body);

            //    // for each Tag/url do _____________, mail it
            //    foreach (var tag in tags)
            //    {
            //        string theUrl = tag.Key;
            //        // get value
            //        // get attribs
            //        Uri url = new System.Uri(theUrl);
            //        string resp = MyBrowser.GetResponse(url);

            //        // Noramlise the a href to email, and reomve images if requested.
            //        resp = MyBrowser.NormalizeHttpToEmail(url.AbsoluteUri, resp, myRabbitMQVitals._emailRoot);
            //        //Program._log.Debug(retval);

            //        retval += mymail.MyMail.SendByMG22(to, from, resp);
            //        retval += "\n";
            //    }
            //}
            //else
            //{
            //    // joeping 
            //    Uri urlDetermine = new System.Uri("http://yahoo.com");  // FM fix this
                
            //    MyBrowser.NormalizeHttpToEmail(urlDetermine.AbsoluteUri, hdr.Body, myRabbitMQVitals._emailRoot);
                
            //    //Program._log.Debug(retval);

            //    retval += mymail.MyMail.SendByMG22(to, from, retval);
            //    retval += "\n";
            //}

            //return retval;
        }


        static public string publish(string tag)
        {
            string retval = MyRabbotMQ.Publish(tag);
            return retval;
        }
    }
}