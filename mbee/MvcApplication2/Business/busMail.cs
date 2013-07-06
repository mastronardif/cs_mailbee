using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using mymail;
using myrabbitmq;


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
    }
}