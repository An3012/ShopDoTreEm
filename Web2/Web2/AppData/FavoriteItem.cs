using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web2.Models;

namespace Web2.AppData
{
    [Serializable]
    public class FavoriteItem
    {
        public SanPham SanPham { get; set; }
    }
}