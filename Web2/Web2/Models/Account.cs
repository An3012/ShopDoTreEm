//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Web2.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Account
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Account()
        {
            this.ThongTinGiaoHang = new HashSet<ThongTinGiaoHang>();
        }
    
        public int id { get; set; }
        public string TenDangNhap { get; set; }
        public string HoVaTen { get; set; }
        public string MatKhau { get; set; }
        public string EmailDangNhap { get; set; }
        public string SoDienThoai { get; set; }
        public Nullable<int> idRole { get; set; }
        public Nullable<int> idTinhThanh { get; set; }
        public string DiaChi { get; set; }
        public Nullable<int> idQuanHuyen { get; set; }
        public Nullable<int> idPhuongXa { get; set; }
        public Nullable<bool> HoatDong { get; set; }
    
        public virtual PhuongXa PhuongXa { get; set; }
        public virtual QuanHuyen QuanHuyen { get; set; }
        public virtual RoleAccount RoleAccount { get; set; }
        public virtual TinhThanh TinhThanh { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ThongTinGiaoHang> ThongTinGiaoHang { get; set; }
    }
}