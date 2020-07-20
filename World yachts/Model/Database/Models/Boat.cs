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
    
    public partial class Boat
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Boat()
        {
            this.AccToBoats = new HashSet<AccToBoats>();
            this.Order = new HashSet<Order>();
        }
    
        public int IdBoat { get; set; }
        public string Model { get; set; }
        public int IdBoatType { get; set; }
        public int NumberOfRowers { get; set; }
        public bool Mast { get; set; }
        public int IdColour { get; set; }
        public int IdWood { get; set; }
        public int BasePrice { get; set; }
        public double VAT { get; set; }
        public System.DateTime ProductionStartDate { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AccToBoats> AccToBoats { get; set; }
        public virtual BoatType BoatType { get; set; }
        public virtual Colour Colour { get; set; }
        public virtual Wood Wood { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order> Order { get; set; }
    }
}
