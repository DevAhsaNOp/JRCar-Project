﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace JRCar.BOL
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class jrcarEntities : DbContext
    {
        public jrcarEntities()
            : base("name=jrcarEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<C__MigrationHistory> C__MigrationHistory { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<tblAddress> tblAddresses { get; set; }
        public virtual DbSet<tblAdmin> tblAdmins { get; set; }
        public virtual DbSet<tblBlog> tblBlogs { get; set; }
        public virtual DbSet<tblCar> tblCars { get; set; }
        public virtual DbSet<tblCategory> tblCategories { get; set; }
        public virtual DbSet<tblCity> tblCities { get; set; }
        public virtual DbSet<tblClient> tblClients { get; set; }
        public virtual DbSet<tblContactU> tblContactUs { get; set; }
        public virtual DbSet<tblCountry> tblCountries { get; set; }
        public virtual DbSet<tblManfacturerCarModel> tblManfacturerCarModels { get; set; }
        public virtual DbSet<tblManufacturer> tblManufacturers { get; set; }
        public virtual DbSet<tblNotification> tblNotifications { get; set; }
        public virtual DbSet<tblNotification1> tblNotifications1 { get; set; }
        public virtual DbSet<tblRole> tblRoles { get; set; }
        public virtual DbSet<tblSearchHist> tblSearchHists { get; set; }
        public virtual DbSet<tblState> tblStates { get; set; }
        public virtual DbSet<tblSubCategory> tblSubCategories { get; set; }
        public virtual DbSet<tblToken> tblTokens { get; set; }
        public virtual DbSet<tblUnion> tblUnions { get; set; }
        public virtual DbSet<tblUserAddImage> tblUserAddImages { get; set; }
        public virtual DbSet<tblUser> tblUsers { get; set; }
        public virtual DbSet<tblZone> tblZones { get; set; }
        public virtual DbSet<tblCarModel> tblCarModels { get; set; }
        public virtual DbSet<tblCarImage> tblCarImages { get; set; }
        public virtual DbSet<tblCarFeature> tblCarFeatures { get; set; }
        public virtual DbSet<tblFavAdd> tblFavAdds { get; set; }
        public virtual DbSet<tblAnnouncement> tblAnnouncements { get; set; }
        public virtual DbSet<ANNOVIEW> ANNOVIEWs { get; set; }
        public virtual DbSet<tblAppointmentDetail> tblAppointmentDetails { get; set; }
        public virtual DbSet<tblAppointment> tblAppointments { get; set; }
        public virtual DbSet<tblQueryDetail> tblQueryDetails { get; set; }
        public virtual DbSet<tblPayment> tblPayments { get; set; }
        public virtual DbSet<tblQuery> tblQueries { get; set; }
        public virtual DbSet<tblUserAdd> tblUserAdds { get; set; }
        public virtual DbSet<tblShowroom> tblShowrooms { get; set; }
        public virtual DbSet<tblRolePermission> tblRolePermissions { get; set; }
    }
}
