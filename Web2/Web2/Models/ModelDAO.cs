using Microsoft.Ajax.Utilities;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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

    }
}