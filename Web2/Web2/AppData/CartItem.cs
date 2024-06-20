using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web2.Models;

namespace Web2.AppData
{
    [Serializable]
    public class CartItem
    {
        public SanPham SanPham { get; set; }
        public int SoLuong { get; set; }
        public int DonGia { get; set; }
    }
}