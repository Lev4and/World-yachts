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
    
    public partial class v_contract
    {
        public int IdContract { get; set; }
        public System.DateTime Date { get; set; }
        public int DepositPayed { get; set; }
        public int IdOrder { get; set; }
        public int ContractTotalPrice { get; set; }
        public int ContractTotalPriceInclVAT { get; set; }
        public string ProductionProcess { get; set; }
    }
}
