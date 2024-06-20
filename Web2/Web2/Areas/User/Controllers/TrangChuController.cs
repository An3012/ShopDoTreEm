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

        public ActionResult TrangChu()
        {
            List<SanPham> lstSanPham = db.SanPham.ToList().GroupBy(x => x.TenSanPham).Select(g => g.First()).ToList();
            return View(lstSanPham);
        }

        public ActionResult TrangSPTheoDM(int? idDanhMuc, int? page)
        {
            int IdDM = idDanhMuc ?? 0;
            int pageNumber = page ?? 1;
            int pageSize = HangSo.PageSize;
            List<SanPham> lstSanPham = db.SanPham.ToList().GroupBy(x => x.TenSanPham).Select(g => g.First()).ToList();
            if (IdDM != 0)
            {
                lstSanPham = db.SanPham.Where(x => x.IdDanhMuc == IdDM).ToList().GroupBy(x => x.TenSanPham).Select(g => g.First()).ToList();
            }
            else
            {
                lstSanPham = db.SanPham.ToList().GroupBy(x => x.TenSanPham).Select(g => g.First()).ToList();
            }

            float pages = lstSanPham.Count() / (float)pageSize;
            int numberRecords = (int)Math.Ceiling(pages);

            ViewBag.PageNumber = pageNumber;
            ViewBag.PageSize = pageSize;
            ViewBag.NumberRecords = numberRecords;
            ViewBag.id = IdDM;
            List<SanPham> lstSP = lstSanPham.Skip(pageSize * (pageNumber - 1)).Take(pageSize).ToList();
            ViewBag.LstSanPham = lstSP;
            ViewBag.IdDanhMuc = IdDM;
            return View();
        }

        public ActionResult TrangSPTheoPLoai(int? idPhanLoai, int? page)
        {
            int IDPPL = idPhanLoai ?? 0;
            int pageNumber = page ?? 1;
            int pageSize = HangSo.PageSize;
            List<SanPham> lstSanPham = db.SanPham.ToList().GroupBy(x => x.TenSanPham).Select(g => g.First()).ToList();
            if (IDPPL != 0)
            {
                lstSanPham = db.SanPham.Where(x => x.IdLoai == IDPPL).ToList();
                ViewBag.IdDanhMuc = db.LoaiSp.FirstOrDefault(x => x.id == idPhanLoai).IdDM;
            }
            else
            {
                ViewBag.IdDanhMuc = 0;
                lstSanPham = db.SanPham.ToList();
            }

            float pages = lstSanPham.Count() / (float)pageSize;
            int numberRecords = (int)Math.Ceiling(pages);

            ViewBag.PageNumber = pageNumber;
            ViewBag.PageSize = pageSize;
            ViewBag.NumberRecords = numberRecords;

            List<SanPham> lstSP = lstSanPham.Skip(pageSize * (pageNumber - 1)).Take(pageSize).ToList();

            ViewBag.IdLoaiSP = IDPPL;

            ViewBag.LstSanPhamFilter = lstSP.GroupBy(x => x.TenSanPham).Select(g => g.First()).ToList();

            return View();
        }

        public ActionResult PhanLoaiSp(int? idDM, int? idPloai, int? page)
        {
            int pageNumber = page ?? 1;
            int pageSize = HangSo.PageSize;
            int IdPLoai = idPloai ?? 0;
            int IdDM = idDM ?? 0;

            IdDM_IdPL.IdDMSP = IdDM;
            IdDM_IdPL.IdPlSP = IdPLoai;

            int numberRecords;
            var lstSP = new List<SanPham>();
            if (IdPLoai != 0)
            {
                List<SanPham> lstSanPham = db.SanPham.Where(sp => sp.IdLoai == IdPLoai).ToList().GroupBy(x => x.TenSanPham).Select(g => g.First()).ToList();
                float pages = lstSanPham.Count() / (float)pageSize;

                numberRecords = (int)Math.Ceiling(pages);

                 lstSP = lstSanPham.Skip(pageSize * (pageNumber - 1)).Take(pageSize).ToList();
            }
            else if(IdDM != 0)
            {
                List<SanPham> lstSanPham = db.SanPham.Where(sp => sp.IdDanhMuc == IdDM).ToList().GroupBy(x => x.TenSanPham).Select(g => g.First()).ToList();
                float pages = lstSanPham.Count() / (float)pageSize;

                numberRecords = (int)Math.Ceiling(pages);

                lstSP = lstSanPham.Skip(pageSize * (pageNumber - 1)).Take(pageSize).ToList();
            }
            else
            {
                List<SanPham> lstSanPham = db.SanPham.ToList().GroupBy(x => x.TenSanPham).Select(g => g.First()).ToList();
                float pages = lstSanPham.Count() / (float)pageSize;

                numberRecords = (int)Math.Ceiling(pages);

                lstSP = lstSanPham.Skip(pageSize * (pageNumber - 1)).Take(pageSize).ToList();
            }


            ViewBag.PageNumber = pageNumber;
            ViewBag.PageSize = pageSize;
            ViewBag.NumberRecords = numberRecords;
            ViewBag.LstSanPham = lstSP.GroupBy(x => x.TenSanPham).Select(g => g.First()).ToList();
            ViewBag.IdDM = IdDM;
            ViewBag.IdPLoai = IdPLoai;
            return View("~/Areas/User/Views/SearchSP/FilterSp.cshtml");
        }

        public ActionResult ALLSP(int? page)
        {
            int pageNumber = page ?? 1;
            int pageSize = HangSo.PageSize;
            List<SanPham> lstSanPham = db.SanPham.OrderBy(sp => sp.Id).Skip(pageSize * (pageNumber - 1)).Take(pageSize).ToList().GroupBy(x => x.TenSanPham).Select(g => g.First()).ToList();
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