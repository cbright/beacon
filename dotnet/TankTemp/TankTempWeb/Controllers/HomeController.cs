﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TankTempWeb.Data;

namespace TankTempWeb.Controllers
{
    public class HomeController : Controller
    {
        private static log4net.ILog _log = log4net.LogManager.GetLogger(typeof(HomeController));

        [UnitOfWork]
        public ActionResult Index()
        {
            _log.Info("Testing");
            return View();
        }

    }
}
