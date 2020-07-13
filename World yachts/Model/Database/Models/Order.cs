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
    
    public partial class Order
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Order()
        {
            this.Contract = new HashSet<Contract>();
            this.OrderDetails = new HashSet<OrderDetails>();
        }
    
        public int IdOrder { get; set; }
        public System.DateTime Date { get; set; }
        public int IdSalesPerson { get; set; }
        public int IdCustomer { get; set; }
        public int IdBoat { get; set; }
        public string DeliveryAddress { get; set; }
        public string City { get; set; }
    
        public virtual Boat Boat { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Contract> Contract { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual SalesPerson SalesPerson { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderDetails> OrderDetails { get; set; }
    }
}
