using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web2.Models;

namespace Web2.Areas.User.Controllers
{
    public class PhanLoaiSpController : Controller
    {
        // GET: User/PhanLoaiSp

        WebDoTreEmEntities2 db = new WebDoTreEmEntities2();

        public ActionResult PhanLoaiSp(int id)
        {
            var lstSanPham = db.SanPham.Where(sp => sp.IdLoai == id).ToList();
            ViewBag.LstSanPham = lstSanPham;
            return View("~/Areas/User/Views/Ao/AoBeTrai.cshtml");
        }
    }
}