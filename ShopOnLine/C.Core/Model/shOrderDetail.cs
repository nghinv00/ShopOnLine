//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace C.Core.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class shOrderDetail
    {
        public string OrderDetailGuid { get; set; }
        public int OrderDetailId { get; set; }
        public string OrderDetailName { get; set; }
        public string OrderGuid { get; set; }
        public string MemberGuiId { get; set; }
        public string ProductGuid { get; set; }
        public string ProductName { get; set; }
        public string SectionGuid { get; set; }
        public string SizeGuid { get; set; }
        public Nullable<int> Number { get; set; }
        public Nullable<decimal> Prices { get; set; }
        public Nullable<decimal> Total { get; set; }
        public Nullable<int> PayType { get; set; }
        public Nullable<int> SortOrder { get; set; }
        public Nullable<bool> Status { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> NgayDat { get; set; }
        public Nullable<int> UserId { get; set; }
    
        public virtual shProduct shProduct { get; set; }
        public virtual shSetSize shSetSize { get; set; }
        public virtual shOrder shOrder { get; set; }
    }
}