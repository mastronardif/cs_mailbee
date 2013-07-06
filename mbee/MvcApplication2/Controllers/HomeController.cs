using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcApplication2.Models;
using System.Drawing;
using log4net;
using MvcApplication2.Business;

namespace MvcApplication2.Controllers
{
    
    public class HomeController : Controller
    {
        static readonly ILog _log = LogManager.GetLogger(typeof(HomeController));
        //private static readonly log4net.ILog _logger
        //    = log4net.LogManager.GetLogger(
        //            System.Reflection.MethodBase.GetCurrentMethod()
        //             .DeclaringType);

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult publish22(FormCollection collection)
        {
            string op = collection["Operation"];
            string msg = collection["message"];
            string debug = string.Empty;

            _log.Debug("op = " + op);
            _log.Debug("op = " + msg);

            string sw = op.ToLower();
            switch (sw)
            {
                case "publish":
                    debug = busMail.publish(msg);
                    break;
                case "send to my_gmail":
                    string tagValue = collection["message"];
                    debug = busMail.sendToGmail(tagValue);
                    _log.Debug("debug = " + debug);
                    break;
                case "rabbit vcap_services":

                    break;
                default:
                    // do nothing
                    break;
            }            
            // I don't want a million ___ views.  I will use the current view.
            return RedirectToAction("Index");
            //return View("Index");
        }

        public ActionResult Index(int? id)
        {

            if (id == 411)
            {
                string xml = "<SP_HELP lastmodified= \"4/16/13 10:26 AM\"> </SP_HELP>"; //Semantics.Help;
                return this.Content(xml, "text/xml");
            }

            return View();
        }

    }
}
