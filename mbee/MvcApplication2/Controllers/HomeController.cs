using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcApplication2.Models;
using System.Drawing;
using log4net;
using MvcApplication2.Business;
using mymailgun;

namespace MvcApplication2.Controllers
{
    
    public class HomeController : Controller
    {
        static readonly ILog _log = LogManager.GetLogger(typeof(HomeController));
        //private static readonly log4net.ILog _logger
        //    = log4net.LogManager.GetLogger(
        //            System.Reflection.MethodBase.GetCurrentMethod()
        //             .DeclaringType);

        public ActionResult wtf(FormCollection collection)
        {
            int iCnt = 230;

            // count msgs on queue


            string str = string.Format("what the fuck how many <b>{0}</b>", iCnt);
            return Content(string.Format("<hr>{0}</hr>", str));
            //return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult publish22(FormCollection collection)
        {
            string op = collection["Operation"];
            //string op= (collection["Operation"] == null) ? string.Empty : collection["Operation"];


            string msg = collection["message"];
            string debug = string.Empty;
            try
            {
                // July 15th 2013
                //$msg = jm002ra::reply(\%allparams);
                //makeReplyFromMailGun
                string msgMG = MyMailGun.makeReplyFromMailGun(collection);
                if ((msgMG.IndexOf("<mymail>", StringComparison.OrdinalIgnoreCase) != -1) &&
                    (msgMG.IndexOf("<mymail>", StringComparison.OrdinalIgnoreCase) != -1))
                {
                    msg = msgMG;
                    op = "publish";
                }

                // July 15th 2013


                _log.Debug("op  = " + op);
                _log.Debug("msg = " + msg);

                string sw = op.ToLower();
                switch (sw)
                {
                    case "publish":
                        // Fm 9/19/13 removed something went wrong!!!! 
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
                //return RedirectToAction("Index");
                string str = string.Format("You in the hive <b>{0}</b>", Request.UserHostAddress);

                //filterContext.RequestContext.HttpContext.Request.UrlReferrer.AbsolutePath
                return Content(string.Format("<hr>{0}</hr>", str));
            }
            catch (Exception ex)
            { 
                //return a 403
                //return new HttpStatusCodeResult(404, "wtf! " + ex.Message);
                return Content(string.Format("<html>wtf: {0}</html>", ex.Message) );
                //return new HttpStatusCodeResult(500, "wtf! " + ex.Message);
                //return Json(new { Success = false, Message = ex.Message });
            }
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
