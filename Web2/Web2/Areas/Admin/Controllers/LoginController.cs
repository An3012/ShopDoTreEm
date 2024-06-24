using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web2.Models;

namespace Web2.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        // GET: Admin/Login
        WebDoTreEmEntities2 DB = new WebDoTreEmEntities2();
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            var Role = DB.RoleAccount.FirstOrDefault(role => role.VaiTro == "Admin").id;
            if (string.IsNullOrEmpty(username) == true || string.IsNullOrEmpty(password) == true)
            {
                ViewBag.ErrorAccount = "Sai tài khoản hoặc mật khẩu!";
                return View();
            }
            var taikhoan = new ModelDAO().account(username);
            if (taikhoan == null)
            {
                ViewBag.ErrorAccount = "Sai tài khoản hoặc mật khẩu!";
                return View();
            }
            if (taikhoan.MatKhau != password)
            {
                ViewBag.ErrorAccount = "Sai tài khoản hoặc mật khẩu!";
                return View();
            }
            if (taikhoan.idRole != Role)
            {
                ViewBag.ErrorAccount = "Sai tài khoản hoặc mật khẩu!";
                return View();
            }
            Session["Admin"] = taikhoan;

            return RedirectToAction("TrangChu", "TrangChu", "");
        }
    }
}