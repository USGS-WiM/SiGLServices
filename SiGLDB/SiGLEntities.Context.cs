﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SiGLDB
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class SiGLEntities : DbContext
    {
        public SiGLEntities()
            : base("name=SiGLEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<contact> contacts { get; set; }
        public virtual DbSet<data_host> data_host { get; set; }
        public virtual DbSet<data_manager> data_manager { get; set; }
        public virtual DbSet<division> divisions { get; set; }
        public virtual DbSet<frequency_type> frequency_type { get; set; }
        public virtual DbSet<keyword> keywords { get; set; }
        public virtual DbSet<lake_type> lake_type { get; set; }
        public virtual DbSet<media_type> media_type { get; set; }
        public virtual DbSet<objective_type> objective_type { get; set; }
        public virtual DbSet<organization> organizations { get; set; }
        public virtual DbSet<organization_system> organization_system { get; set; }
        public virtual DbSet<parameter_type> parameter_type { get; set; }
        public virtual DbSet<proj_duration> proj_duration { get; set; }
        public virtual DbSet<proj_status> proj_status { get; set; }
        public virtual DbSet<project> projects { get; set; }
        public virtual DbSet<project_contacts> project_contacts { get; set; }
        public virtual DbSet<project_cooperators> project_cooperators { get; set; }
        public virtual DbSet<project_keywords> project_keywords { get; set; }
        public virtual DbSet<project_objectives> project_objectives { get; set; }
        public virtual DbSet<project_publication> project_publication { get; set; }
        public virtual DbSet<publication> publications { get; set; }
        public virtual DbSet<resource_type> resource_type { get; set; }
        public virtual DbSet<role> roles { get; set; }
        public virtual DbSet<section> sections { get; set; }
        public virtual DbSet<site> sites { get; set; }
        public virtual DbSet<site_frequency> site_frequency { get; set; }
        public virtual DbSet<site_media> site_media { get; set; }
        public virtual DbSet<site_parameters> site_parameters { get; set; }
        public virtual DbSet<site_resource> site_resource { get; set; }
        public virtual DbSet<status_type> status_type { get; set; }
        public virtual DbSet<monitoring_coordination> monitoring_coordination { get; set; }
        public virtual DbSet<project_monitor_coord> project_monitor_coord { get; set; }
    }
}
