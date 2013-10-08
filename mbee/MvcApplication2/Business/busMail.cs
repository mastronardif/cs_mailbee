using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using mymail;
using myrabbitmq;
using System.Configuration;
using mytwitter;


namespace MvcApplication2.Business
{
    public class busMail
    {
        static public string sendToGmail(string tag)
        {
            string retval = MyMail.prepForGmail(tag);
            return publish(retval);
        }

        static public string publish(string tag)
        {
            string retval = MyRabbotMQ.Publish(tag);
            return retval;
        }

        static public string getTweetPage(string tag)
        {
            string retval = string.Format("tag{0}", tag);
            TwitterHelper twh = new TwitterHelper(OauthConsumerKey, OauthConsumerSecret,
                                      OauthAccessToken, OauthAccessTokenSecret);

            string tweetHash = tag ?? "Government%20Shutdown";

            var resp = twh.GetTweets(tweetHash);
            var strxml = myjson.MyJson.testJsonToXML(resp);

            //myhelpers.MyHelpers.hlp_WriteToFile(".\\wtf.xml", strxml);


            string strXslt = string.Empty;
            //myhelpers.MyHelpers.ReadFromApp_Data("myTable.xsl", ref strXslt);
            myhelpers.MyHelpers.ReadFromApp_Data("myselectedcols.xsl", ref strXslt);

            string strResults = string.Empty;
            myxslxml.MyXml.my_XslXmlTransformToString(strxml, strXslt, ref strResults);

            //Console.WriteLine(strResults);
            return strResults;
        }

        #region ConsumerKey & ConsumerSecret
        private static string OauthConsumerSecret
        {
            get { return ConfigurationManager.AppSettings["OauthConsumerSecret"]; }
        }

        private static string OauthConsumerKey
        {
            get { return ConfigurationManager.AppSettings["OauthConsumerKey"]; }
        }

        private static string OauthAccessToken
        {
            get { return ConfigurationManager.AppSettings["OauthAccessToken"]; }
        }

        private static string OauthAccessTokenSecret
        {
            get { return ConfigurationManager.AppSettings["OauthAccessTokenSecret"]; }
        }



        #endregion


    }
}