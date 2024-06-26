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
            ViewBag.listsp = listsp;
            return View("~/Areas/Admin/Views/SanPham/SanPham.cshtml");
        }
        
        public ActionResult DetailProduct(int IdProduct)
        {
            var sanpham = DB.SanPham.FirstOrDefault(sp => sp.Id == IdProduct);
            ViewBag.IdProduct = IdProduct;
            return View("~/Areas/Admin/Views/Shared/Product.cshtml");
        }
    }
}