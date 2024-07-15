using Microsoft.Ajax.Utilities;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static Web2.Models.ModelDAO;

namespace Web2.Models
{
    public class ModelDAO
    {
        WebDoTreEmEntities2 Db = new WebDoTreEmEntities2();
        public IEnumerable<SanPham> ListSPAllPaging(int page, int pageSize)
        {
            return Db.SanPham.ToPagedList(page, pageSize);
        }
        public Account account(string Username)
        {
            try
            {
                var model = Db.Account.SingleOrDefault(acc => acc.TenDangNhap == Username.ToLower() || acc.EmailDangNhap == Username.ToLower());
                return model;
            }
            catch
            {
                return null;
            }
        }
        public class ImgEdit
        {
            public HttpPostedFileBase fileImage { get; set; }
            public int IdSp { get; set; }

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

        public class ImgDetail
        {
            public int Id { get; set; }
            public string fileImge { get; set; }
        }
        public class DDNB
        {
            public int Id { get; set; }
            public string TagDDNB { get; set; }
            public string NDDDNB { get; set; }
        }
        public class SoLuongSPBySize
        {
            public int Id { get; set; }
            public int IdSize { get; set; }
            public int Soluong { get; set; }
        }
        public class ListEditDacDiemNB
        {
            public int iddd { get; set; }
            public string NameDD { get; set; }
            public string DDNB { get; set; }
        }
        public class SanPhamByOrder
        {
            public int idSPbySize { get; set; }
            public int IdOrder { get; set; }
            public int SoLuong { get; set; }
            public int GiaSanpham { get; set; }
        }
        public class TinhTrangsanPham
        {
            public string Value { get; set; }
            public string title { get; set; }

            public static List<TinhTrangsanPham> TinhTrangList = new List<TinhTrangsanPham>();

            static TinhTrangsanPham()
            {
                TinhTrangList.Add(new TinhTrangsanPham { Value = "AlsoProduct", title = "Còn Hàng" });
                TinhTrangList.Add(new TinhTrangsanPham { Value = "New", title = "Mới" });
                TinhTrangList.Add(new TinhTrangsanPham { Value = "Sale", title = "Sale" });
                TinhTrangList.Add(new TinhTrangsanPham { Value = "HotSearch", title = "Hot Search" });
            }
        }
    }
}