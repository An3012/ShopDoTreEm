using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web2.Models;

namespace Web2.Areas.User.Controllers
{
    public class ChiTietSanPhamController : Controller
    {
        // GET: User/ChiTietSanPham
        WebDoTreEmEntities2 Db = new WebDoTreEmEntities2();
        public ActionResult ChiTietSanPham(int IdSp)
        {
            var SanPham = Db.SanPham.FirstOrDefault(x => x.Id == IdSp);
            var SpTuongTu = Db.SanPham.Where(x => x.TenSanPham == SanPham.TenSanPham).ToList();
            ViewBag.IdSp = IdSp;
            ViewBag.SpTuongTu = SpTuongTu;
            return View();
        }
        public ActionResult SPChiTietTheoMau(int IdSp)
        {
            var SanPham = Db.SanPham.FirstOrDefault(x => x.Id == IdSp);
            var SpTuongTu = Db.SanPham.Where(x => x.TenSanPham == SanPham.TenSanPham).ToList();
            ViewBag.IdSp = IdSp;
            ViewBag.SpTuongTu = SpTuongTu;
            return View("~/Areas/User/Views/ChiTietSanPham/SPChiTietTheoMau.cshtml");
        }
    }
}