﻿namespace Saritasa.BoringWarehouse.Web.Controllers
{
    using System.Web.Mvc;

    /// <summary>
    /// Index.
    /// </summary>
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }
    }
}