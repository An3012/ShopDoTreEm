
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
    
public partial class Order
{

    public int Id { get; set; }

    public Nullable<int> IdKhachHang { get; set; }

    public string TinhTrang { get; set; }

    public string IdSanPham { get; set; }

    public string SoLuongSP { get; set; }

    public string GiaSp { get; set; }

    public Nullable<System.DateTime> ThoigianTao { get; set; }

}

}