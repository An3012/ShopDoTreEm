using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Web2.AppData;
using Web2.Models;
using System.Configuration;
using Microsoft.Identity.Client;

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
        public ActionResult fEditProduct(int IdProduct)
        {
            var sanpham = DB.SanPham.FirstOrDefault(sp => sp.Id == IdProduct);
            ViewBag.IdProduct = IdProduct;
            return View("~/Areas/Admin/Views/Shared/Edit_Product.cshtml");
        }

        public ActionResult DetailProductEdit(int IdProduct)
        {
            var sanpham = DB.SanPham.FirstOrDefault(sp => sp.Id == IdProduct);
            ViewBag.IdProduct = IdProduct;
            return View("~/Areas/Admin/Views/SanPham/SanPham.cshtml");
        }

        public JsonResult DeleteProduct(int IdProduct)
        {
            var sanpham = DB.SanPham.FirstOrDefault(sp => sp.Id == IdProduct);
            var SizeSanPhams = DB.SpBySize.Where(sp => sp.idsp == IdProduct);
            foreach (var item in SizeSanPhams)
            {
                DB.SpBySize.Remove(item);
            }
            DB.SanPham.Remove(sanpham);
            DB.SaveChanges();
            return Json("Xóa sản phẩm thành công!" );
        }

        public ActionResult EditProduct(string EditSanPham, string ListEditDDNB)
        {
            List<ListEditProDuct> products = JsonConvert.DeserializeObject<List<ListEditProDuct>>(EditSanPham);
            List<ListEditDacDiemNB> listEditDDNB = JsonConvert.DeserializeObject<List<ListEditDacDiemNB>>(ListEditDDNB);
            foreach(var item in listEditDDNB)
            {
                var DDNBEdit = DB.DacDiemSP.FirstOrDefault(x => x.id == item.iddd);
                if(DDNBEdit != null)
                {
                    DDNBEdit.DacDiemSanPham = item.DDNB;
                    DDNBEdit.NameDacDiem = item.NameDD;
                }    
                DB.SaveChanges();
            }    
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

                    var SanPham_SizeUpdate = DB.SpBySize.FirstOrDefault(x => x.id == item.IdSize);
                    if (SanPham_SizeUpdate != null)
                    {
                        var sql = "UPDATE SpBySize SET SoLuong = @soluong WHERE id = @id";
                        DB.Database.ExecuteSqlCommand(sql, new SqlParameter("@soluong", item.SoLuongSanPham), new SqlParameter("@id", item.IdSize));
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

        public class ImgEdit
        {
            public HttpPostedFileBase fileImage { get; set; }
            public int IdSp { get; set; }

        }

        public JsonResult EditImage(ImgEdit formdata)
        {
            var uploadResults = new List<string>();
            try
            {
                int idSP = formdata.IdSp;
                HttpPostedFileBase file = formdata.fileImage;
                if (file == null || file.ContentLength == 0)
                {
                    return Json("Lỗi: Không có tập tin được tải lên");
                }
                var sanphamupdate = DB.SanPham.FirstOrDefault(sp => sp.Id == idSP);
                var fileName = Path.GetFileName(file.FileName);
                var urlfile = "/AppData/Image/Product/";
                var urlServer = Server.MapPath(urlfile);
                var savePath = urlfile + fileName;
                string urlFile = urlServer + fileName;

                if (System.IO.File.Exists(urlFile) == true)
                {
                    savePath = urlfile + Path.GetFileName(urlFile);
                    sanphamupdate.AnhSp = savePath;
                    DB.SaveChanges();
                }
                else
                {
                    file.SaveAs(urlFile);
                    sanphamupdate.AnhSp = savePath;
                    DB.SaveChanges();
                }
                uploadResults.Add($"Cập nhật hình ảnh thành công: {fileName}");
            }
            catch (Exception ex)
            {
                uploadResults.Add($"Lỗi cập nhật: {ex.Message}");
            }

            return Json(uploadResults);
        }
        public JsonResult EditImageDetail(ImgEdit formdata)
        {
            var uploadResults = new List<string>();
            try
            {
                int idSP = formdata.IdSp;
                HttpPostedFileBase file = formdata.fileImage;
                if (file == null || file.ContentLength == 0)
                {
                    return Json("Lỗi: Không có tập tin được tải lên");
                }
                var sanphamupdate = DB.HinhAnhChiTietSp.FirstOrDefault(sp => sp.Id == idSP);
                var fileName = Path.GetFileName(file.FileName);
                var urlfile = "/AppData/Image/Product/";
                var urlServer = Server.MapPath(urlfile);
                var savePath = urlfile + fileName;
                string urlFile = urlServer + fileName;

                if (System.IO.File.Exists(urlFile) == true)
                {
                    savePath = urlfile + Path.GetFileName(urlFile);
                    sanphamupdate.FileAnhChiTiet = savePath;
                    DB.SaveChanges();
                }
                else
                {
                    file.SaveAs(urlFile);
                    sanphamupdate.FileAnhChiTiet = savePath;
                    DB.SaveChanges();
                }
                uploadResults.Add($"Cập nhật hình ảnh thành công: {fileName}");
            }
            catch (Exception ex)
            {
                uploadResults.Add($"Lỗi cập nhật: {ex.Message}");
            }
            return Json(uploadResults);
        }

        public JsonResult DeleteImg(int id)
        {
            var uploadResults = new List<string>();
            try
            {
                int idSP = id;
                var sanpham = DB.SanPham.Find(id);
                sanpham.AnhSp = "";
                DB.SaveChanges();
                uploadResults.Add($"Xóa hình ảnh thành công!");
            }
            catch (Exception ex)
            {
                uploadResults.Add($"Đã xảy ra lỗi: {ex.Message}");
            }
            return Json(uploadResults);
        }

        public JsonResult DeleteImgDetail(int id)
        {
            var uploadResults = new List<string>();
            try
            {
                int idSP = id;
                var hinhAnhCT = DB.HinhAnhChiTietSp.Find(id);

                DB.HinhAnhChiTietSp.Remove(hinhAnhCT);
                DB.SaveChanges();
                uploadResults.Add($"Xóa hình ảnh thành công!");
            }
            catch (Exception ex)
            {
                uploadResults.Add($"Đã xảy ra lỗi: {ex.Message}");
            }
            return Json(uploadResults);
        }

        public JsonResult AddImgDetail(ImgEdit formdata)
        {
            var uploadResults = new List<string>();
            try
            {
                int idSP = formdata.IdSp;
                HttpPostedFileBase file = formdata.fileImage;
                if (file == null || file.ContentLength == 0)
                {
                    return Json("Lỗi: Không có tập tin được tải lên");
                }
                var fileName = Path.GetFileName(file.FileName);
                var urlfile = "/AppData/Image/Product/";
                var urlServer = Server.MapPath(urlfile);
                var savePath = urlfile + fileName;
                string urlFile = urlServer + fileName;

                if (System.IO.File.Exists(urlFile) == true)
                {
                    savePath = urlfile + Path.GetFileName(urlFile);
                    var AddImgSanPham = new HinhAnhChiTietSp
                    {
                        IdSanPham = idSP,
                        FileAnhChiTiet = savePath
                    };
                    DB.HinhAnhChiTietSp.Add(AddImgSanPham);
                }
                else
                {
                    file.SaveAs(urlFile);
                    var AddImgSanPham = new HinhAnhChiTietSp
                    {
                        IdSanPham = idSP,
                        FileAnhChiTiet = savePath
                    };
                    DB.HinhAnhChiTietSp.Add(AddImgSanPham);
                }
                DB.SaveChanges();
                uploadResults.Add($"Thêm hình ảnh chi tiết thành công!");
            }
            catch (Exception ex)
            {
                uploadResults.Add($"Đã xảy ra lỗi: {ex.Message}");
            }
            return Json(uploadResults);
        }

        public JsonResult AddDDNoiBat(string TagDD, string DDNB, int IdSp)
        {
            var uploadResults = new List<string>();
            try
            {
                DacDiemSP dacDiemSP = new DacDiemSP();
                dacDiemSP.MaSp = DB.SanPham.Find(IdSp).Masp;
                dacDiemSP.NameDacDiem = TagDD;
                dacDiemSP.DacDiemSanPham = DDNB;
                DB.DacDiemSP.Add(dacDiemSP);
                DB.SaveChanges();
                uploadResults.Add($"Thêm đặc điểm nổi bật thành công!");
            }
            catch (Exception ex)
            {
                uploadResults.Add($"Đã xảy ra lỗi: {ex.Message}");
            }
            return Json(uploadResults);
        }
        public JsonResult DeleteDDNB(int Id)
        {
            var uploadResults = new List<string>();
            try
            {
                DacDiemSP dacDiemSP = new DacDiemSP();
                dacDiemSP = DB.DacDiemSP.Find(Id);
                DB.DacDiemSP.Remove(dacDiemSP);
                DB.SaveChanges();
                uploadResults.Add($"Xóa đặc điểm nổi bật thành công!");
            }
            catch (Exception ex)
            {
                uploadResults.Add($"Đã xảy ra lỗi: {ex.Message}");
            }
            return Json(uploadResults);
        }
        public JsonResult AddSizeBySanPham(int Id, int IdS, int SoLuongSP)
        {
            var uploadResults = new List<string>();
            try
            {
                var SizeBySp = DB.SpBySize.FirstOrDefault(x => x.idsp == Id && x.idsize == IdS);
                if (SizeBySp != null)
                {
                    SizeBySp.soluong += SoLuongSP;
                    uploadResults.Add("Cập nhật số lượng size cho sản phẩm thành công!");
                }
                else
                {
                    SizeBySp = new SpBySize
                    {
                        idsp = Id,
                        idsize = IdS,
                        soluong = SoLuongSP
                    };
                    DB.SpBySize.Add(SizeBySp);
                    uploadResults.Add("Thêm size cho sản phẩm thành công!");
                }
                DB.SaveChanges();
            }
            catch (Exception ex)
            {
                uploadResults.Add($"Đã xảy ra lỗi: {ex.Message}");
                return Json(uploadResults);
            }
            return Json(uploadResults);
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

        public class ListEditDacDiemNB
        {
            public int iddd { get; set; }
            public string NameDD { get; set; }
            public string DDNB { get; set; }
        }
    }
    
}