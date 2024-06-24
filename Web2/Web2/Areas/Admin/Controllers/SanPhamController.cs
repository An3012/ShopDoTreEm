using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web2.Models;

namespace Web2.Areas.Admin.Controllers
{
    public class SanPhamController : Controller
    {
        // GET: Admin/SanPham
        WebDoTreEmEntities2 DB = new WebDoTreEmEntities2();
        public ActionResult SanPham()
        {
            var listsp = DB.SanPham.ToList();
            return View(listsp);
        }
    }
}