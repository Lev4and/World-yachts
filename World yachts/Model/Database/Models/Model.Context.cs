﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class WorldYachtsContext : DbContext
    {
        public WorldYachtsContext()
            : base("name=WorldYachtsContext")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Accessory> Accessory { get; set; }
        public virtual DbSet<AccToBoats> AccToBoats { get; set; }
        public virtual DbSet<Boat> Boat { get; set; }
        public virtual DbSet<BoatType> BoatType { get; set; }
        public virtual DbSet<Colour> Colour { get; set; }
        public virtual DbSet<Contract> Contract { get; set; }
        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<Invoice> Invoice { get; set; }
        public virtual DbSet<Order> Order { get; set; }
        public virtual DbSet<OrderDetails> OrderDetails { get; set; }
        public virtual DbSet<Partner> Partner { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<SalesPerson> SalesPerson { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Wood> Wood { get; set; }
        public virtual DbSet<v_accessory> v_accessory { get; set; }
        public virtual DbSet<v_boat> v_boat { get; set; }
        public virtual DbSet<v_user> v_user { get; set; }
        public virtual DbSet<v_boatSimplifiedInformation> v_boatSimplifiedInformation { get; set; }
        public virtual DbSet<v_cityCustomer> v_cityCustomer { get; set; }
        public virtual DbSet<v_customer> v_customer { get; set; }
        public virtual DbSet<v_organisationNameCustomer> v_organisationNameCustomer { get; set; }
        public virtual DbSet<v_cityPartner> v_cityPartner { get; set; }
        public virtual DbSet<v_order> v_order { get; set; }
        public virtual DbSet<v_salesPerson> v_salesPerson { get; set; }
        public virtual DbSet<v_cityOrder> v_cityOrder { get; set; }
        public virtual DbSet<v_deliveryAddressOrder> v_deliveryAddressOrder { get; set; }
        public virtual DbSet<v_accessorySimplifiedInformation> v_accessorySimplifiedInformation { get; set; }
        public virtual DbSet<v_briefReportBestSalesPersonAllTime> v_briefReportBestSalesPersonAllTime { get; set; }
        public virtual DbSet<v_briefReportPopularBoatsAllTime> v_briefReportPopularBoatsAllTime { get; set; }
        public virtual DbSet<v_detailReportBestSalesPersonAllTime> v_detailReportBestSalesPersonAllTime { get; set; }
        public virtual DbSet<v_detailReportPopularBoatsAllTime> v_detailReportPopularBoatsAllTime { get; set; }
        public virtual DbSet<v_contract> v_contract { get; set; }
        public virtual DbSet<v_contract2> v_contract2 { get; set; }
        public virtual DbSet<v_partner> v_partner { get; set; }
    
        public virtual int sp_toBlockUser()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_toBlockUser");
        }
    }
}
