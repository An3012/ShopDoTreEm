//using Microsoft.Ajax.Utilities;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;
//using System.Web.UI.WebControls;
//using Web2.AppData;
//using Web2.Models;

//namespace Web2.Areas.User.Controllers
//{
//    public class PhanLoaiSpController : Controller
//    {
//        // GET: User/PhanLoaiSp

//        WebDoTreEmEntities2 db = new WebDoTreEmEntities2();

//        public ActionResult PhanLoaiSp(int id)
//        {
//            var lstSanPham = db.SanPham.Where(sp => sp.IdLoai == id).ToList();
//            ViewBag.LstSanPham = lstSanPham;
//            return View("~/Areas/User/Views/Ao/AoBeTrai.cshtml");
//        }

//        public ActionResult FilterProduct(List<int> lstSize,int? idDM,int? idPloai,int? page) 
//        {
//            int pageNumber = page ?? 1;
//            int pageSize = HangSo.PageSize;
//            int IdPLoai = idPloai ?? 0;
//            int IdDM = idDM ?? 0;
//            int numberRecords;
//            var lstSP = new List<SanPham>();
//            var lstSanPham = new List<SanPham>();
//            lstSize = lstSize ?? new List<int>();
//            var LstSPhamFilter = new List<SanPham>();

//            if (IdPLoai != 0)
//            {
//                lstSanPham = db.SanPham.Where(sp => sp.IdLoai == IdPLoai).ToList();
//                if (lstSize != null)
//                {
//                    LstSPhamFilter = lstSanPham.Where(sp => lstSize.Contains(sp.Size.GetValueOrDefault(0))).ToList();
//                }
//                else
//                {
//                    LstSPhamFilter = lstSanPham;
//                }
//                float pages = lstSanPham.Count() / (float)pageSize;
//                numberRecords = (int)Math.Ceiling(pages);
//                lstSP = LstSPhamFilter.Skip(pageSize * (pageNumber - 1)).Take(pageSize).ToList();
//            }
//            else if (IdDM != 0)
//            {
//                lstSanPham = db.SanPham.Where(sp => sp.IdDanhMuc == IdDM).ToList();
//                if (lstSize != null)
//                {
//                    LstSPhamFilter = lstSanPham.Where(sp => lstSize.Contains(sp.Size.GetValueOrDefault(0))).ToList();
//                }
//                else
//                {
//                    LstSPhamFilter = lstSanPham;
//                }
//                float pages = lstSanPham.Count() / (float)pageSize;
//                numberRecords = (int)Math.Ceiling(pages);
//                lstSP = LstSPhamFilter.Skip(pageSize * (pageNumber - 1)).Take(pageSize).ToList();
//            }
//            else
//            {
//                lstSanPham = db.SanPham.ToList();
//                if (lstSize != null)
//                {
//                    LstSPhamFilter = lstSanPham.Where(sp => lstSize.Contains(sp.Size.GetValueOrDefault(0))).ToList();
//                }
//                else
//                {
//                    LstSPhamFilter = lstSanPham;
//                }
//                float pages = lstSanPham.Count() / (float)pageSize;
//                numberRecords = (int)Math.Ceiling(pages);
//                lstSP = LstSPhamFilter.Skip(pageSize * (pageNumber - 1)).Take(pageSize).ToList();
//            }


//            ViewBag.PageNumber = pageNumber;
//            ViewBag.PageSize = pageSize;
//            ViewBag.NumberRecords = numberRecords;
//            ViewBag.LstSanPham = lstSP;
//            ViewBag.IdDM = IdDM;
//            ViewBag.IdPLoai = IdPLoai;
            
//            ViewBag.LstSanPhamFilter = lstSP.GroupBy(x => x.TenSanPham).Select(g => g.First()).ToList();
//            return View("~/Areas/User/Views/PhanLoaiSp/FilterProduct.cshtml"); 
//        }
//    }
//}