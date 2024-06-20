using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web2.AppData;
using Web2.Models;

namespace Web2.Areas.User.Controllers
{
    public class GioHangController : Controller
    {
        // GET: User/GioHang
        private const string CartSession = "CartSession";
        WebDoTreEmEntities2 db = new WebDoTreEmEntities2();
        public ActionResult AddDonHang(int Idsp, int SoluongProduct)
        {
            var Cart = Session[CartSession];
            if (Cart != null)
            {

                var list = (List<CartItem>)Cart;
                if (list.Exists(x => x.SanPham.Id == Idsp))
                {
                    foreach (var item in list)
                    {
                        if (item.SanPham.Id == Idsp)
                        {
                            item.SoLuong += SoluongProduct;
                            int gianew;
                            if (item.SanPham.GiamGia != 0)
                            {
                                gianew = int.Parse(item.SanPham.Gia.ToString()) * int.Parse(item.SanPham.GiamGia.ToString()) / 100;
                            }
                            else
                            {
                                gianew = int.Parse(item.SanPham.Gia.ToString());
                            }
                            item.DonGia = gianew;
                        }
                    }
                }
                else
                {
                    var item = new CartItem();
                    item.SanPham = db.SanPham.FirstOrDefault(sp => sp.Id == Idsp);
                    item.SoLuong = SoluongProduct;
                    int gianew;
                    if (item.SanPham.GiamGia != 0)
                    {
                        gianew = int.Parse(item.SanPham.Gia.ToString()) * int.Parse(item.SanPham.GiamGia.ToString()) / 100;
                    }
                    else
                    {
                        gianew = int.Parse(item.SanPham.Gia.ToString());
                    }
                    item.DonGia = gianew;
                    list.Add(item);

                    Session[CartSession] = list;
                }

            }
            else
            {
                var item = new CartItem();
                item.SanPham = db.SanPham.FirstOrDefault(sp => sp.Id == Idsp);
                item.SoLuong = SoluongProduct;
                int gianew;
                if (item.SanPham.GiamGia != 0)
                {
                    gianew = int.Parse(item.SanPham.Gia.ToString()) * int.Parse(item.SanPham.GiamGia.ToString()) / 100;
                }
                else
                {
                    gianew = int.Parse(item.SanPham.Gia.ToString());
                }
                item.DonGia = gianew;
                var list = new List<CartItem>();
                list.Add(item);
                Session[CartSession] = list;
            }
            var sanpham = db.SanPham.Find(Idsp);
            return Json(new { success = true, message = "Thêm vào giỏ hàng thành công!" });
        }
        public ActionResult GioHang()
        {
            var Cart = Session[CartSession];
            var list = new List<CartItem>();
            if (Cart != null)
            {
                list = (List<CartItem>)Cart;
            }
            return View(list);
        }

        public ActionResult UpdateGioHang(List<CartItem> CartList)
        {
            var Cart = Session[CartSession];
            if (Cart != null)
            {
                var list = (List<CartItem>)Cart;
                foreach (var itemUpdate in CartList)
                {
                    foreach (var itemAfter in list)
                    {
                        if (itemAfter.SanPham.Id == itemUpdate.SanPham.Id)
                        {
                            itemAfter.SoLuong = itemUpdate.SoLuong;
                        }
                    }
                }
                Session[CartSession] = list;
            }
            else
            {
                return Json(new { success = false, message = "Giỏ hàng của bạn chưa có sản phẩm!" });
            }

            return Json(new { success = true, message = "Cập nhật thành công!" });
        }
    }
}
