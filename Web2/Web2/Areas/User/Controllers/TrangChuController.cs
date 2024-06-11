using Microsoft.Ajax.Utilities;
using PagedList;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Web2.AppData;
using Web2.Models;

namespace Web2.Areas.User.Controllers
{
    public class TrangChuController : Controller
    {
        // GET: User/TrangChu
        WebDoTreEmEntities2 db = new WebDoTreEmEntities2();


        public ActionResult TrangChu(int? page)
        {
            int pageNumber = page ?? 1;
            int pageSize = HangSo.PageSize;
            List<SanPham> lstSanPham = db.SanPham.OrderBy(sp => sp.Id).Skip(pageSize * (pageNumber - 1)).Take(pageSize).ToList();
            float pages = db.SanPham.Count() / (float)pageSize;
            int numberRecords = (int)Math.Ceiling(pages);

            ViewBag.PageNumber = pageNumber;
            ViewBag.PageSize = pageSize;
            ViewBag.NumberRecords = numberRecords;

            return View(lstSanPham);
        }

        public ActionResult TrangSPTheoDM(int id, int? page)
        {
            int pageNumber = page ?? 1;
            int pageSize = HangSo.PageSize;
            List<SanPham> lstSanPham = db.SanPham.Where(sp => sp.IdDanhMuc == id).ToList();
            float pages = lstSanPham.Count() / (float)pageSize;
            int numberRecords = (int)Math.Ceiling(pages);

            ViewBag.PageNumber = pageNumber;
            ViewBag.PageSize = pageSize;
            ViewBag.NumberRecords = numberRecords;
            ViewBag.id = id;
            List<SanPham> lstSP = lstSanPham.Skip(pageSize * (pageNumber - 1)).Take(pageSize).ToList();

            ViewBag.IdDanhMuc = id;
            ViewBag.LstSanPham = lstSP;
            return View();
        }

        public ActionResult TrangSPTheoPLoai(int id, int? page)

        {
            int pageNumber = page ?? 1;
            int pageSize = HangSo.PageSize;
            List<SanPham> lstSanPham = db.SanPham.Where(sp => sp.IdLoai == id).ToList();
            float pages = lstSanPham.Count() / (float)pageSize;
            int numberRecords = (int)Math.Ceiling(pages);

            ViewBag.PageNumber = pageNumber;
            ViewBag.PageSize = pageSize;
            ViewBag.NumberRecords = numberRecords;

            List<SanPham> lstSP = lstSanPham.Skip(pageSize * (pageNumber - 1)).Take(pageSize).ToList();

            ViewBag.IdDanhMuc = db.LoaiSp.FirstOrDefault(x => x.id == id).IdDM;
            ViewBag.IdLoaiSP = id;

            ViewBag.LstSanPham = lstSP;

            return View();
        }

        public ActionResult PhanLoaiSp(int idDM, int idPloai, int? page)
        {
            int pageNumber = page ?? 1;
            int pageSize = HangSo.PageSize;
            int IdPLoai = idPloai;
            int IdDM = idDM;

            int numberRecords;
            var lstSP = new List<SanPham>();


            if (IdPLoai != 0)
            {
                List<SanPham> lstSanPham = db.SanPham.Where(sp => sp.IdLoai == IdPLoai).ToList();
                float pages = lstSanPham.Count() / (float)pageSize;

                numberRecords = (int)Math.Ceiling(pages);

                 lstSP = lstSanPham.Skip(pageSize * (pageNumber - 1)).Take(pageSize).ToList();
            }
            else
            {
                List<SanPham> lstSanPham = db.SanPham.Where(sp => sp.IdDanhMuc == IdDM).ToList();
                float pages = lstSanPham.Count() / (float)pageSize;

                numberRecords = (int)Math.Ceiling(pages);

                lstSP = lstSanPham.Skip(pageSize * (pageNumber - 1)).Take(pageSize).ToList();
                IdPLoai = 0;
            }
            
            ViewBag.PageNumber = pageNumber;
            ViewBag.PageSize = pageSize;
            ViewBag.NumberRecords = numberRecords;
            ViewBag.LstSanPham = lstSP;
            return View("~/Areas/User/Views/SearchSP/FilterSp.cshtml");
        }

        public ActionResult ALLSP(int? page)
        {
            int pageNumber = page ?? 1;
            int pageSize = HangSo.PageSize;
            List<SanPham> lstSanPham = db.SanPham.OrderBy(sp => sp.Id).Skip(pageSize * (pageNumber - 1)).Take(pageSize).ToList();
            float pages = db.SanPham.Count() / (float)pageSize;
            int numberRecords = (int)Math.Ceiling(pages);

            ViewBag.PageNumber = pageNumber;
            ViewBag.PageSize = pageSize;
            ViewBag.NumberRecords = numberRecords;

            ViewBag.LstSanPham = lstSanPham;
            return View("~/Areas/User/Views/SearchSP/AllSp.cshtml");
        }

    }

}