﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CFWDatamigrationApp
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class mydbEntities : DbContext
    {
        public mydbEntities()
            : base("name=mydbEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Claim> Claims { get; set; }
        public virtual DbSet<VolvoProgram> VolvoPrograms { get; set; }
        public virtual DbSet<Claim2> Claim2 { get; set; }
        public virtual DbSet<Claim3> Claim3 { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
    }
}
