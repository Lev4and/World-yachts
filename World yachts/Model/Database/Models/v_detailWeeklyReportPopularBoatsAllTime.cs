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
    
    public partial class v_detailWeeklyReportPopularBoatsAllTime
    {
        public string Date { get; set; }
        public Nullable<int> Year { get; set; }
        public Nullable<int> Week { get; set; }
        public int IdBoat { get; set; }
        public Nullable<int> Count { get; set; }
        public Nullable<int> Rate { get; set; }
        public string Model { get; set; }
    }
}
