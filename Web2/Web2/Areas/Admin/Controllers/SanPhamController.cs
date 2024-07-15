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
        ModelDAO ModelDAO = new ModelDAO();
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
                Console.WriteLine("Error: " + ex.Message);
                return Json(new { success = false, message = ex.Message });
            }
        }
        public ActionResult SanPham()
        {
            var listsp = DB.SanPham.ToList();
            ViewBag.listsp = listsp;
            return View();
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
            List<ModelDAO.ImgDetail> imgDetails = JsonConvert.DeserializeObject<List<ModelDAO.ImgDetail>>(sanpham.AnhChiTiet ?? "");
            List<ModelDAO.SoLuongSPBySize> SoLuongSPBySize = JsonConvert.DeserializeObject<List<ModelDAO.SoLuongSPBySize>>(sanpham.Sizesp_soLuong ?? "");
            List<ModelDAO.DDNB> DDNB = JsonConvert.DeserializeObject<List<ModelDAO.DDNB>>(sanpham.DacDiemNB ?? "");
            ViewBag.IdProduct = IdProduct;
            ViewBag.LstimgDetails = imgDetails;
            ViewBag.SoLuongSPBySize = SoLuongSPBySize;
            ViewBag.LstDDNBbySP = DDNB;

            return View("~/Areas/Admin/Views/Shared/Edit_Product.cshtml");
        }

        [HttpPost]
        public ActionResult EditProduct(int Idsp, HttpPostedFileBase fileImage, IEnumerable<HttpPostedFileBase> fileImgProductdetail, string val_TenSanPham, string val_MaSp, int val_DanhMuc, int? val_PhanLoaiSanPham, string val_Mota, IEnumerable<int> statesSize, string val_Soluong, string val_MauSp, int? val_Gia, int? val_GiamGia, string val_TinhTrang, string val_Brand, IEnumerable<string> val_TagDDNB, IEnumerable<string> val_DDNB)
        {
            try
            {
                var sanpham = DB.SanPham.FirstOrDefault(sp => sp.Id == Idsp);
                if (sanpham == null)
                {
                    return HttpNotFound(); 
                }

                sanpham.TenSanPham = val_TenSanPham;
                sanpham.IdDanhMuc = val_DanhMuc;
                sanpham.IdLoai = val_PhanLoaiSanPham ?? 0;
                sanpham.MotaSp = val_Mota;
                sanpham.MauSP = val_MauSp;
                sanpham.Gia = val_Gia ?? 0;
                sanpham.GiamGia = val_GiamGia ?? 0;
                sanpham.TinhTrang = val_TinhTrang;
                sanpham.Brand = val_Brand;

                if (fileImage != null && fileImage.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(fileImage.FileName);
                    var savePath = "/AppData/Image/Product/" + fileName;
                    var physicalPath = Server.MapPath(savePath);

                    if (!System.IO.File.Exists(physicalPath))
                    {
                        fileImage.SaveAs(physicalPath);
                    }
                    sanpham.AnhSp = savePath;
                }

                List<ModelDAO.ImgDetail> LstEditimgDetails = new List<ModelDAO.ImgDetail>();
                if (fileImgProductdetail != null && fileImgProductdetail.Any())
                {
                    int nextId = 1;
                    if (sanpham.AnhChiTiet != null)
                    {
                        LstEditimgDetails = JsonConvert.DeserializeObject<List<ModelDAO.ImgDetail>>(sanpham.AnhChiTiet ?? "");

                        if (LstEditimgDetails.Any())
                        {
                            nextId = LstEditimgDetails.Max(x => x.Id) + 1;
                        }
                        foreach (var item in fileImgProductdetail)
                        {
                            if (item != null && item.ContentLength > 0)
                            {
                                var fileName = Path.GetFileName(item.FileName);
                                var savePath = "/AppData/Image/Product/" + fileName;
                                var physicalPath = Server.MapPath(savePath);

                                if (!System.IO.File.Exists(physicalPath))
                                {
                                    item.SaveAs(physicalPath);
                                }
                                LstEditimgDetails.Add(new ModelDAO.ImgDetail { Id = nextId++, fileImge = fileName });
                            }
                        }
                    }
                    else
                    {
                        foreach (var item in fileImgProductdetail)
                        {
                            if (item != null && item.ContentLength > 0)
                            {
                                var fileName = Path.GetFileName(item.FileName);
                                var savePath = "/AppData/Image/Product/" + fileName;
                                var physicalPath = Server.MapPath(savePath);

                                if (!System.IO.File.Exists(physicalPath))
                                {
                                    item.SaveAs(physicalPath);
                                }
                                LstEditimgDetails.Add(new ModelDAO.ImgDetail { Id = nextId++, fileImge = fileName });
                            }
                        }
                    }
                    sanpham.AnhChiTiet = JsonConvert.SerializeObject(LstEditimgDetails);
                }

                List<ModelDAO.DDNB> DDNBs = new List<ModelDAO.DDNB>();
                if (val_TagDDNB != null && val_DDNB != null)
                {
                    var tagDDNBs = val_TagDDNB.ToList();
                    var ndDDNBs = val_DDNB.ToList();

                    for (int i = 0; i < Math.Min(tagDDNBs.Count, ndDDNBs.Count); i++)
                    {
                        DDNBs.Add(new ModelDAO.DDNB { Id = i, TagDDNB = tagDDNBs[i], NDDDNB = ndDDNBs[i] });
                    }

                    sanpham.DacDiemNB = JsonConvert.SerializeObject(DDNBs);
                }
                List<ModelDAO.SoLuongSPBySize> SanphamNewySizes = new List<ModelDAO.SoLuongSPBySize>();
                if (statesSize != null && statesSize.Any())
                {
                    var sizes = statesSize.ToList();
                    var quantities = val_Soluong.ToList();

                    for (int i = 0; i < Math.Min(sizes.Count, quantities.Count); i++)
                    {
                        SanphamNewySizes.Add(new ModelDAO.SoLuongSPBySize { Id = i, IdSize = sizes[i], Soluong = quantities[i] });
                    }

                    sanpham.Sizesp_soLuong = JsonConvert.SerializeObject(SanphamNewySizes);
                }

                DB.SaveChanges();
                var listsp = DB.SanPham.ToList();
                ViewBag.listsp = listsp;
                return View("~/Areas/Admin/Views/SanPham/TableSanPham.cshtml");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return View("~/Areas/Admin/Views/SanPham/Edit_Product.cshtml");
            }
        }

        public ActionResult DeleteProduct(int IdProduct)
        {
            var sanpham = DB.SanPham.FirstOrDefault(sp => sp.Id == IdProduct);
            var SizeSanPhams = DB.SpBySize.Where(sp => sp.idsp == IdProduct);
            foreach (var item in SizeSanPhams)
            {
                DB.SpBySize.Remove(item);
            }
            DB.SanPham.Remove(sanpham);
            DB.SaveChanges();
            var listsp = DB.SanPham.ToList();
            ViewBag.listsp = listsp;
            return View("~/Areas/Admin/Views/SanPham/TableSanPham.cshtml");
        }

        public ActionResult SanPhamMoi() { return View(); }

        [HttpPost]
        public ActionResult SanPhamMoi(HttpPostedFileBase fileImage, IEnumerable<HttpPostedFileBase> fileImgProductdetail, string val_TenSanPham, string val_MaSp, int val_DanhMuc, int? val_PhanLoaiSanPham, string val_Mota, IEnumerable<int> statesSize, string val_Soluong, string val_MauSp, int? val_Gia, int? val_GiamGia, string val_TinhTrang, string val_Brand, IEnumerable<string> val_TagDDNB, IEnumerable<string> val_DDNB)
        {
            ViewBag.error = "";
            SanPham SPNew = new SanPham();
            SPNew.TenSanPham = val_TenSanPham;
            SPNew.Masp = val_MaSp;
            SPNew.IdDanhMuc = val_DanhMuc;
            DB.SanPham.Add(SPNew);
            SPNew.IdLoai = val_PhanLoaiSanPham ?? 0;
            SPNew.MotaSp = val_Mota;
            SPNew.MauSP = val_MauSp;
            SPNew.Gia = val_Gia ?? 0;
            SPNew.GiamGia = val_GiamGia ?? 0;
            SPNew.TinhTrang = val_TinhTrang;
            SPNew.Brand = val_Brand;

            var urlfile = "/AppData/Image/Product/";
            var urlServer = Server.MapPath(urlfile);

            if (fileImage != null && fileImage.ContentLength > 0)
            {
                var fileName = Path.GetFileName(fileImage.FileName);
                var savePath = urlfile + fileName;
                string urlFile = urlServer + fileName;

                if (System.IO.File.Exists(urlFile) == true)
                {
                    savePath = urlfile + Path.GetFileName(urlFile);
                    SPNew.AnhSp = savePath;
                }
                else
                {
                    fileImage.SaveAs(urlFile);
                    SPNew.AnhSp = savePath;
                }
            }
            List<ModelDAO.ImgDetail> LstEditimgDetails = new List<ModelDAO.ImgDetail>();
            if (fileImgProductdetail != null && fileImgProductdetail.Count() > 0)
            {
                int i = 0;
                foreach (var item in fileImgProductdetail)
                {
                    ModelDAO.ImgDetail imgDetail = new ModelDAO.ImgDetail();
                    imgDetail.Id = i;
                    var savePathImgDetail = item.FileName;
                    if (item.ContentLength == 0)
                    {
                        ViewBag.error = "Tập tin không có nội dung";
                    }
                    else
                    {
                        string urlFileImgDetail = urlServer + item.FileName;
                        if (System.IO.File.Exists(urlFileImgDetail) == true)
                        {
                            savePathImgDetail = Path.GetFileName(urlFileImgDetail);
                            imgDetail.fileImge = savePathImgDetail;
                        }
                        else
                        {
                            item.SaveAs(urlFileImgDetail);
                            imgDetail.fileImge = savePathImgDetail;
                        }
                    }
                    LstEditimgDetails.Add(imgDetail);
                    i++;
                }
                string Anhchitiet = "";
                if (LstEditimgDetails != null && LstEditimgDetails.Count() > 0)
                {
                    Anhchitiet = JsonConvert.SerializeObject(LstEditimgDetails);
                }
                SPNew.AnhChiTiet = Anhchitiet;
            }

            List<ModelDAO.DDNB> DDNBs = new List<ModelDAO.DDNB>();
            if (val_TagDDNB != null && val_TagDDNB.Count() > 0)
            {
                for (int x = 0; x < val_DDNB.Count(); x++)
                {
                    var TagDDNBs = val_TagDDNB.ToList();
                    var NDDDNBs = val_DDNB.ToList();
                    ModelDAO.DDNB ddnb = new ModelDAO.DDNB();
                    ddnb.Id = x;
                    ddnb.TagDDNB = TagDDNBs[x];
                    ddnb.NDDDNB = NDDDNBs[x];
                    DDNBs.Add(ddnb);
                }
                string dacdiemnoibat = "";
                if (DDNBs != null && DDNBs.Count() > 0)
                {
                    dacdiemnoibat = JsonConvert.SerializeObject(DDNBs);
                }
                SPNew.DacDiemNB = dacdiemnoibat;
            }

            List<ModelDAO.SoLuongSPBySize> SanphamNewySizes = new List<ModelDAO.SoLuongSPBySize>();
            if (statesSize != null && statesSize.Count() > 0)
            {
                for (int x = 0; x < statesSize.Count(); x++)
                {
                    var sizes = statesSize.ToList();
                    var solg = val_Soluong.ToList();
                    ModelDAO.SoLuongSPBySize spbys = new ModelDAO.SoLuongSPBySize();
                    spbys.Id = x;
                    spbys.IdSize = sizes[x];
                    spbys.Soluong = solg[x];
                    SanphamNewySizes.Add(spbys);
                }
                string Spbysize = "";
                if (SanphamNewySizes != null && SanphamNewySizes.Count() > 0)
                {
                    Spbysize = JsonConvert.SerializeObject(SanphamNewySizes);
                }
                SPNew.Sizesp_soLuong = Spbysize;
            }

            DB.SaveChanges();
            return View("~/Areas/Admin/Views/SanPham/SanPham.cshtml");
        }
    }

}