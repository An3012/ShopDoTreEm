using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web2.Models;

namespace Web2.Areas.Admin.Controllers
{
    public class DonHangController : Controller
    {
        // GET: Admin/GioHang
        WebDoTreEmEntities2 DB = new WebDoTreEmEntities2();
        ModelDAO ModelDAO = new ModelDAO();

        public ActionResult Index()
        {
            List<ModelDAO.SanPhamByOrder> danhSachSanPham = new List<ModelDAO.SanPhamByOrder>();

            var ListOrder = DB.Order.ToList();
            if (DB.Order.ToList() != null && DB.Order.ToList().Count() > 0)
            {
                foreach (var item in DB.Order.ToList())
                {
                    string[] idSPArray = item.IdSanPham.Split(';');
                    string[] soLuongArray = item.SoLuongSP.Split(';');
                    string[] giaSPArray = item.GiaSp.Split(';');
                    for (int j = 0; j < idSPArray.Length; j++)
                    {
                        danhSachSanPham.Add(new ModelDAO.SanPhamByOrder
                        {
                            IdOrder = item.Id,
                            idSPbySize = int.Parse(idSPArray[j]),
                            SoLuong = int.Parse(soLuongArray[j]),
                            GiaSanpham = int.Parse(giaSPArray[j])
                        });
                    }
                }
            }
            ViewBag.ListOrder = ListOrder;
            ViewBag.ListSanPhamByOrder = danhSachSanPham;
            return View();
        }

        public ActionResult Detail(int Id)
        {
            List<ModelDAO.SanPhamByOrder> danhSachSanPhamByIdOrDer = new List<ModelDAO.SanPhamByOrder>();
            if (DB.Order.ToList() != null && DB.Order.ToList().Count() > 0)
            {
                foreach (var item in DB.Order.Where(x=>x.Id == Id).ToList())
                {
                    string[] idSPArray = item.IdSanPham.Split(';');
                    string[] soLuongArray = item.SoLuongSP.Split(';');
                    string[] giaSPArray = item.GiaSp.Split(';');
                    for (int j = 0; j < idSPArray.Length; j++)
                    {
                        danhSachSanPhamByIdOrDer.Add(new ModelDAO.SanPhamByOrder
                        {
                            IdOrder = item.Id,
                            idSPbySize = int.Parse(idSPArray[j]),
                            SoLuong = int.Parse(soLuongArray[j]),
                            GiaSanpham = int.Parse(giaSPArray[j])
                        });
                    }
                }
            }
            ViewBag.danhSachSanPhamByIdOrDer = danhSachSanPhamByIdOrDer;
            ViewBag.DonHang = DB.Order.Find(Id);
            return View("/Areas/Admin/Views/DonHang/ThongTinDonHang.cshtml");
        }
        public ActionResult DuyetDonHang(int idOrder)
        {
            List<ModelDAO.SanPhamByOrder> danhSachSanPham = new List<ModelDAO.SanPhamByOrder>();
            var ListOrder = DB.Order.ToList();
            var DonHang = DB.Order.Find(idOrder);
            if (DB.Order.ToList() != null && DB.Order.ToList().Count() > 0)
            {
                foreach (var item in DB.Order.ToList())
                {
                    string[] idSPArray = item.IdSanPham.Split(';');
                    string[] soLuongArray = item.SoLuongSP.Split(';');
                    string[] giaSPArray = item.GiaSp.Split(';');
                    for (int j = 0; j < idSPArray.Length; j++)
                    {
                        danhSachSanPham.Add(new ModelDAO.SanPhamByOrder
                        {
                            IdOrder = item.Id,
                            idSPbySize = int.Parse(idSPArray[j]),
                            SoLuong = int.Parse(soLuongArray[j]),
                            GiaSanpham = int.Parse(giaSPArray[j])
                        });
                    }
                }
            }
            try
            {
                if (DonHang.TinhTrang == DB.TinhTrangDonHang.Find(1).TinhTrang || DonHang.TinhTrang == DB.TinhTrangDonHang.Find(2).TinhTrang)
                {
                    DonHang.TinhTrang = DB.TinhTrangDonHang.FirstOrDefault(x => x.id == 2).TinhTrang;
                    DB.SaveChanges();
                };
                ViewBag.ListOrder = ListOrder;
                ViewBag.ListSanPhamByOrder = danhSachSanPham;
                return View("/Areas/Admin/Views/DonHang/TableOrder.cshtml");
            }
            catch (Exception ex)
            {
                return Json(new { html = "", message = "Lỗi xảy ra khi duyệt đơn hàng"+ ex.Message });
            }
        }
    }
}