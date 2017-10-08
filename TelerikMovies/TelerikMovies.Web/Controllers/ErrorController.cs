﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CoBuilder.GoBim.Web.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult Index()
        {
            Response.ContentType = "text/html";
            return View("Index");
        }

        public ActionResult NotFound()
        {
            Response.ContentType = "text/html";
            return View();
        }
    }
}