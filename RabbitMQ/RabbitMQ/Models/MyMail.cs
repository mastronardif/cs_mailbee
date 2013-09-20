using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Xsl;
using RestSharp;
using System.Text.RegularExpressions;


using CustomExtensions;
using OpenPop.Mime;
using System.Net.Mail;

namespace mymail
{
    static class myyMailVitals
    {
        public static string _MGApiKey = ConfigurationManager.AppSettings["MGApiKey"];
        public static string _MGurl = "https://api.mailgun.net/v2/joeschedule.mailgun.org/messages";
    }

    class MyMail
    {
        static public MailMessage GetVitals(string msg)
        {
            // check for the format <mymail></mymail>
            // if not then its assumed a mime format
            if (msg.IndexOf("</mymail>", StringComparison.OrdinalIgnoreCase) != -1 ||
                msg.IndexOf("<mymail>", StringComparison.OrdinalIgnoreCase)  != -1)
            {
                // get just the mail message
                msg = msg.Replace("<MYMAIL>", "");
                msg = msg.Replace("</MYMAIL>", "");
                msg = msg.Replace("<HEADER>", "");
                msg = msg.Replace("</HEADER>", "");
            }

            msg = msg.Trim();

            byte[] array = Encoding.ASCII.GetBytes(msg);
            var mmm = new Message(array);

            using (MailMessage message = mmm.ToMailMessage())
            {
                return mmm.ToMailMessage();
            }
        }

        static public string SendByMG22(string to, string from, string body)
        {
            string retval = string.Empty;

            var client = new RestClient();
            client.BaseUrl = myyMailVitals._MGurl;
            client.Authenticator = new HttpBasicAuthenticator("api", myyMailVitals._MGApiKey);

            RestRequest request = new RestRequest();
            request.AddParameter("from", from);
            request.AddParameter("to", to);

            string subject = "joemailweb";
            request.AddParameter("subject", subject);

            request.AddParameter("html", body);

            request.Method = Method.POST;
            IRestResponse resp = client.Execute(request);

            retval = resp.Content.ToString();

            return retval;
        }

        //static public string SendByMG(string tagValue)
        //{
        //    string retval = string.Empty;

        //    var client = new RestClient();
        //    client.BaseUrl       = myyMailVitals._MGurl;
        //    client.Authenticator = new HttpBasicAuthenticator("api", myyMailVitals._MGApiKey);

        //    RestRequest request = new RestRequest();
        //    request.AddParameter("from", "mastronardif@gmail.com"); //"jimmy@joeschedule.mailgun.org");
        //    request.AddParameter("to", "jimmy@joeschedule.mailgun.org"); //""jimmy@joeschedule.mailgun.org"

        //    // mailgun test
        //    request.AddParameter("sender", "mastronardif@gmail.com"); //"jimmy@joeschedule.mailgun.org");
            
        //    string subject = "joemailweb";                           
        //    request.AddParameter("subject", subject);

        //    //request.AddParameter("message-headers", "[[\"Received\",+\"by+184.173.173.18+with+SMTP+mgrt+-1057111050%3B+Tue,+17+Jan+2012+18:56:20++0000\"],+[\"X-Envelope-From\",+\"<mastronardif@gmail.com>\"],+[\"Received\",+\"from+mail-vx0-f182.google.com+(mail-vx0-f182.google.com+[209.85.220.182])\tby+mxa.mailgun.org+(Postfix)+with+ESMTPS+id+E1BDDF0209F\tfor+<joemail@joeschedule.mailgun.org>%3B+Tue,+17+Jan+2012+18:56:19++0000+(UTC)\"],+[\"Received\",+\"by+vcbfl17+with+SMTP+id+fl17so3214754vcb.13++++++++for+<joemail@joeschedule.mailgun.org>%3B+Tue,+17+Jan+2012+10:56:19+-0800+(PST)\"],+[\"Dkim-Signature\",+\"v=1%3B+a=rsa-sha256%3B+c=relaxed/relaxed%3B++++++++d=gmail.com%3B+s=gamma%3B++++++++h=subject:from:content-type:x-mailer:message-id:date:to+++++++++:content-transfer-encoding:mime-version%3B++++++++bh=ZYFs+jTX+yDm+CU7V4aOn0xfMazZHenergPOiHqdz0I=%3B++++++++b=iGgQ3eNkFyTUVXBPJ6VTnZ3y7cYE0koa4adSKZ4XuRx03hRqo2AkpfWMRvr4DUllAb+++++++++zpIvo4BFS3fP9+BsbprJlmB0tzzJKZWYIxFyTsHEonC5yPBnOaFEplutOtO5WsRW8JEF+++++++++HuOEz88KqELQ8m2lXvv6fYNu4Jr2Fq/w58nCk=\"],+[\"Received\",+\"by+10.220.149.68+with+SMTP+id+s4mr11262277vcv.43.1326826579740%3B++++++++Tue,+17+Jan+2012+10:56:19+-0800+(PST)\"],+[\"Received\",+\"from+[192.168.1.101]+(static-64-115-239-70.isp.broadviewnet.net.+[64.115.239.70])++++++++by+mx.google.com+with+ESMTPS+id+fe5sm19231559vdc.8.2012.01.17.10.56.18++++++++(version=TLSv1/SSLv3+cipher=OTHER)%3B++++++++Tue,+17+Jan+2012+10:56:19+-0800+(PST)\"],+[\"Subject\",+\"oeping+joemailweb\"],+[\"From\",+\"Frank+Mastronardi+<mastronardif@gmail.com>"],+["X-Mailer",+"iPod+Mail+(9A405)"],+["Message-Id",+"<8950389B-9325-4430-83FB-343C91A319D9@gmail.com>"],+["Date",+"Tue,+17+Jan+2012+13:56:17+-0500"],+["To",+"\"joemail@joeschedule.mailgun.org\"+<joemail@joeschedule.mailgun.org>\"],+[\"X-Mailgun-Spf\",+\"Neutral\"]]");

        //    string body = "&lt;tags img = keep&gt;<tags img = keep>http://drudgereport.com</tags>&lt;/tags&gt;";
        //    //body = System.Web.HttpUtility.HtmlEncode(body);
        //    request.AddParameter("text", body);
        //    //request.AddParameter("html", "<html>HTML version of the body <TAGS>http://drudgereport.com</TAGS> </html>");

        //    request.Method = Method.POST;
        //    IRestResponse resp = client.Execute(request);

        //    retval = resp.Content.ToString();


        //    return retval;
        //}


        static public string prepForGmail(string tagValue)
        {
            string retval = MvcApplication2.Models.TestDataClass1.strMail;
            retval = retval.Replace("$tagValue", tagValue);

            return retval;
        }

        static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
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
