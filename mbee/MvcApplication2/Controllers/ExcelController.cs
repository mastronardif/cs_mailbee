using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MvcApplication2.Models;

namespace MvcApplication2.Controllers
{
    public class ExcelController : ApiController
    {

        public MyRequest Post(MyRequest req)
        {
            //req.Right = "results for " + req.Left;

            req.Right = PerformRequest(req.Left);

            return req;
        }

        public string PerformRequest(string req)
        {
            string retval = string.Empty;

            retval = Semantics22.doRequest(req);

            // check results.
            return retval;
        
        }
    }
}
