//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace World_yachts.Model.Database.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class v_detailWeeklyEconomicReportAllTime
    {
        public string Date { get; set; }
        public Nullable<int> Year { get; set; }
        public Nullable<int> Week { get; set; }
        public Nullable<int> Count { get; set; }
        public Nullable<decimal> DepositPayed { get; set; }
        public Nullable<decimal> ContractTotalPrice { get; set; }
        public Nullable<decimal> ContractTotalPriceInclVAT { get; set; }
        public Nullable<int> TotalCount { get; set; }
        public Nullable<decimal> TotalDepositPayed { get; set; }
        public Nullable<decimal> TotalContractTotalPrice { get; set; }
        public Nullable<decimal> TotalContractTotalPriceInclVAT { get; set; }
        public long Rank { get; set; }
    }
}
