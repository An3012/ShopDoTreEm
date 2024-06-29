using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Web2.AppData;
using Web2.Models;

namespace Web2.Areas.Admin.Controllers
{
    public class SanPhamController : Controller
    {
        // GET: Admin/SanPham
        WebDoTreEmEntities2 DB = new WebDoTreEmEntities2();
        public JsonResult GetPhanLoaiByDanhMuc(int danhMucId)
        {
            try
            {
                var phanLoaiList = DB.LoaiSp
                    .Where(x => x.IdDM == danhMucId)
                    .Select(x => new { x.id, x.ChiTiet })
                    .ToList();
                return Json(phanLoaiList);
            }
            catch (Exception ex)
            {
                        // Log the exception
                        Console.WriteLine("Error: " + ex.Message);
                        // Return an appropriate error response
                        return Json(new { success = false, message = ex.Message });
                    }
        }
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
            return View("~/Areas/Admin/Views/Shared/ProductDetail.cshtml");
        }

        public ActionResult DeleteProduct(int IdProduct)
        {
            var sanpham = DB.SanPham.FirstOrDefault(sp => sp.Id == IdProduct);
            DB.SanPham.Remove(sanpham);
            DB.SaveChanges();
            return RedirectToAction("SanPham");
        }

        public class ListEditProDuct
        {
            public int Id { get; set; }
            public int IdDM { get; set; }
            public int IdLoai { get; set; }
            public string TenSanPham { get; set; }
            public string MotaSp { get; set; }
            public int IdSize { get; set; }
            public int SoLuongSanPham { get; set; }
            public int Gia { get; set; }
            public int GiamGia { get; set; }
            public string TinhTrang { get; set; }
            public string Brand { get; set; }

        }
        public ActionResult EditProduct(string EditSanPham)
        {
            List<ListEditProDuct> products = JsonConvert.DeserializeObject<List<ListEditProDuct>>(EditSanPham);
            foreach (var item in products)
            {
                var SanPhamUpdate = DB.SanPham.FirstOrDefault(x => x.Id == item.Id);
                if (SanPhamUpdate != null)
                {
                    SanPhamUpdate.IdDanhMuc = item.IdDM;
                    SanPhamUpdate.IdLoai = item.IdLoai;
                    SanPhamUpdate.TenSanPham = item.TenSanPham;
                    SanPhamUpdate.MotaSp = item.MotaSp;
                    SanPhamUpdate.Gia = item.Gia;
                    SanPhamUpdate.GiamGia = item.GiamGia;
                    SanPhamUpdate.TinhTrang = item.TinhTrang;
                    SanPhamUpdate.Brand = item.Brand;

                    var SanPham_SizeUpdate = DB.SanPham_Size.FirstOrDefault(x => x.Id == item.IdSize);
                    if (SanPham_SizeUpdate != null)
                    {
                        var sql = "UPDATE SanPham_Size SET SoLuong = @SoLuong WHERE Id = @Id";
                        DB.Database.ExecuteSqlCommand(sql, new SqlParameter("@SoLuong", item.SoLuongSanPham), new SqlParameter("@Id", item.IdSize));
                    }
                    else
                    {
                        return Json(new { success = false, message = "Cập nhật thất bại" });
                    }
                }
                else
                {
                    return Json(new { success = false, message = "Cập nhật thất bại" });
                }
            }
            DB.SaveChanges();
            return Json(new { success = true, message = "Cập nhật thành công!" });
        }
    }
}