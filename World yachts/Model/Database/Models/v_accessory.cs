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
    
    public partial class v_accessory
    {
        public int IdAccessory { get; set; }
        public string AccName { get; set; }
        public string DescriptionOfAccessory { get; set; }
        public int Price { get; set; }
        public double VAT { get; set; }
        public int Inventory { get; set; }
        public int OrderLevel { get; set; }
        public int OrderBatch { get; set; }
        public string PartnerName { get; set; }
        public string ListCompatibleModelBoats { get; set; }
    }
}
