﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace kinnemed05.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Objects;
    using System.Data.Objects.DataClasses;
    using System.Linq;
    
    public partial class bd_kinnemed02Entities : DbContext
    {
        public bd_kinnemed02Entities()
            : base("name=bd_kinnemed02Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<canton> canton { get; set; }
        public DbSet<empresa> empresa { get; set; }
        public DbSet<especialidad> especialidad { get; set; }
        public DbSet<pais> pais { get; set; }
        public DbSet<profesion> profesion { get; set; }
        public DbSet<provincia> provincia { get; set; }
        public DbSet<valores> valores { get; set; }
        public DbSet<area> area { get; set; }
        public DbSet<control> control { get; set; }
        public DbSet<perfil> perfil { get; set; }
        public DbSet<fisico> fisico { get; set; }
        public DbSet<revision> revision { get; set; }
        public DbSet<cie10> cie10 { get; set; }
        public DbSet<diagnostico> diagnostico { get; set; }
        public DbSet<sub_cie10> sub_cie10 { get; set; }
        public DbSet<plan> plan { get; set; }
        public DbSet<familiar> familiar { get; set; }
        public DbSet<personal> personal { get; set; }
        public DbSet<signos> signos { get; set; }
        public DbSet<subsecuente> subsecuente { get; set; }
        public DbSet<inmunizacion> inmunizacion { get; set; }
        public DbSet<riesgo> riesgo { get; set; }
        public DbSet<vacuna> vacuna { get; set; }
        public DbSet<ocupacional> ocupacional { get; set; }
        public DbSet<laboral> laboral { get; set; }
        public DbSet<examen> examen { get; set; }
        public DbSet<registro> registro { get; set; }
        public DbSet<concepto> concepto { get; set; }
        public DbSet<ginecologico> ginecologico { get; set; }
        public DbSet<trabajador> trabajador { get; set; }
        public DbSet<laboratorista> laboratorista { get; set; }
        public DbSet<medico> medico { get; set; }
        public DbSet<codigo> codigo { get; set; }
        public DbSet<orden> orden { get; set; }
        public DbSet<prueba> prueba { get; set; }
        public DbSet<historia> historia { get; set; }
        public DbSet<reposo> reposo { get; set; }
        public DbSet<paciente> paciente { get; set; }
        public DbSet<oftalmologia> oftalmologia { get; set; }
        public DbSet<rayos> rayos { get; set; }
        public DbSet<audiometria> audiometria { get; set; }
        public DbSet<espirometria> espirometria { get; set; }
    
        public virtual ObjectResult<getReporte01_Result> getReporte01(string fecha_ini, string fecha_fin, Nullable<int> empresa)
        {
            var fecha_iniParameter = fecha_ini != null ?
                new ObjectParameter("fecha_ini", fecha_ini) :
                new ObjectParameter("fecha_ini", typeof(string));
    
            var fecha_finParameter = fecha_fin != null ?
                new ObjectParameter("fecha_fin", fecha_fin) :
                new ObjectParameter("fecha_fin", typeof(string));
    
            var empresaParameter = empresa.HasValue ?
                new ObjectParameter("empresa", empresa) :
                new ObjectParameter("empresa", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<getReporte01_Result>("getReporte01", fecha_iniParameter, fecha_finParameter, empresaParameter);
        }
    
        public virtual ObjectResult<Lista01> getLista01(string fecha_ini, string fecha_fin, Nullable<int> empresa)
        {
            var fecha_iniParameter = fecha_ini != null ?
                new ObjectParameter("fecha_ini", fecha_ini) :
                new ObjectParameter("fecha_ini", typeof(string));
    
            var fecha_finParameter = fecha_fin != null ?
                new ObjectParameter("fecha_fin", fecha_fin) :
                new ObjectParameter("fecha_fin", typeof(string));
    
            var empresaParameter = empresa.HasValue ?
                new ObjectParameter("empresa", empresa) :
                new ObjectParameter("empresa", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Lista01>("getLista01", fecha_iniParameter, fecha_finParameter, empresaParameter);
        }
    
        public virtual ObjectResult<getReporte02_Result> getReporte02(string fecha_ini, string fecha_fin, Nullable<int> empresa)
        {
            var fecha_iniParameter = fecha_ini != null ?
                new ObjectParameter("fecha_ini", fecha_ini) :
                new ObjectParameter("fecha_ini", typeof(string));
    
            var fecha_finParameter = fecha_fin != null ?
                new ObjectParameter("fecha_fin", fecha_fin) :
                new ObjectParameter("fecha_fin", typeof(string));
    
            var empresaParameter = empresa.HasValue ?
                new ObjectParameter("empresa", empresa) :
                new ObjectParameter("empresa", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<getReporte02_Result>("getReporte02", fecha_iniParameter, fecha_finParameter, empresaParameter);
        }
    
        public virtual ObjectResult<getReporte03_Result> getReporte03(string fecha_ini, string fecha_fin, Nullable<int> empresa)
        {
            var fecha_iniParameter = fecha_ini != null ?
                new ObjectParameter("fecha_ini", fecha_ini) :
                new ObjectParameter("fecha_ini", typeof(string));
    
            var fecha_finParameter = fecha_fin != null ?
                new ObjectParameter("fecha_fin", fecha_fin) :
                new ObjectParameter("fecha_fin", typeof(string));
    
            var empresaParameter = empresa.HasValue ?
                new ObjectParameter("empresa", empresa) :
                new ObjectParameter("empresa", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<getReporte03_Result>("getReporte03", fecha_iniParameter, fecha_finParameter, empresaParameter);
        }
    
        public virtual ObjectResult<Nullable<int>> getReporte04(string fecha_ini, string fecha_fin, Nullable<int> empresa)
        {
            var fecha_iniParameter = fecha_ini != null ?
                new ObjectParameter("fecha_ini", fecha_ini) :
                new ObjectParameter("fecha_ini", typeof(string));
    
            var fecha_finParameter = fecha_fin != null ?
                new ObjectParameter("fecha_fin", fecha_fin) :
                new ObjectParameter("fecha_fin", typeof(string));
    
            var empresaParameter = empresa.HasValue ?
                new ObjectParameter("empresa", empresa) :
                new ObjectParameter("empresa", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("getReporte04", fecha_iniParameter, fecha_finParameter, empresaParameter);
        }
    
        public virtual ObjectResult<Nullable<int>> getReporte05(string fecha_ini, string fecha_fin, Nullable<int> empresa)
        {
            var fecha_iniParameter = fecha_ini != null ?
                new ObjectParameter("fecha_ini", fecha_ini) :
                new ObjectParameter("fecha_ini", typeof(string));
    
            var fecha_finParameter = fecha_fin != null ?
                new ObjectParameter("fecha_fin", fecha_fin) :
                new ObjectParameter("fecha_fin", typeof(string));
    
            var empresaParameter = empresa.HasValue ?
                new ObjectParameter("empresa", empresa) :
                new ObjectParameter("empresa", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("getReporte05", fecha_iniParameter, fecha_finParameter, empresaParameter);
        }
    
        public virtual ObjectResult<Nullable<int>> getReporte06(string fecha_ini, string fecha_fin, Nullable<int> empresa)
        {
            var fecha_iniParameter = fecha_ini != null ?
                new ObjectParameter("fecha_ini", fecha_ini) :
                new ObjectParameter("fecha_ini", typeof(string));
    
            var fecha_finParameter = fecha_fin != null ?
                new ObjectParameter("fecha_fin", fecha_fin) :
                new ObjectParameter("fecha_fin", typeof(string));
    
            var empresaParameter = empresa.HasValue ?
                new ObjectParameter("empresa", empresa) :
                new ObjectParameter("empresa", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("getReporte06", fecha_iniParameter, fecha_finParameter, empresaParameter);
        }
    
        public virtual ObjectResult<Nullable<int>> getReporte07(string fecha_ini, string fecha_fin, Nullable<int> empresa)
        {
            var fecha_iniParameter = fecha_ini != null ?
                new ObjectParameter("fecha_ini", fecha_ini) :
                new ObjectParameter("fecha_ini", typeof(string));
    
            var fecha_finParameter = fecha_fin != null ?
                new ObjectParameter("fecha_fin", fecha_fin) :
                new ObjectParameter("fecha_fin", typeof(string));
    
            var empresaParameter = empresa.HasValue ?
                new ObjectParameter("empresa", empresa) :
                new ObjectParameter("empresa", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("getReporte07", fecha_iniParameter, fecha_finParameter, empresaParameter);
        }
    
        public virtual ObjectResult<Lista02> getLista02(string fecha_ini, string fecha_fin, Nullable<int> empresa)
        {
            var fecha_iniParameter = fecha_ini != null ?
                new ObjectParameter("fecha_ini", fecha_ini) :
                new ObjectParameter("fecha_ini", typeof(string));
    
            var fecha_finParameter = fecha_fin != null ?
                new ObjectParameter("fecha_fin", fecha_fin) :
                new ObjectParameter("fecha_fin", typeof(string));
    
            var empresaParameter = empresa.HasValue ?
                new ObjectParameter("empresa", empresa) :
                new ObjectParameter("empresa", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Lista02>("getLista02", fecha_iniParameter, fecha_finParameter, empresaParameter);
        }
    
        public virtual ObjectResult<Lista03> getLista03(string fecha_ini, string fecha_fin, Nullable<int> empresa)
        {
            var fecha_iniParameter = fecha_ini != null ?
                new ObjectParameter("fecha_ini", fecha_ini) :
                new ObjectParameter("fecha_ini", typeof(string));
    
            var fecha_finParameter = fecha_fin != null ?
                new ObjectParameter("fecha_fin", fecha_fin) :
                new ObjectParameter("fecha_fin", typeof(string));
    
            var empresaParameter = empresa.HasValue ?
                new ObjectParameter("empresa", empresa) :
                new ObjectParameter("empresa", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Lista03>("getLista03", fecha_iniParameter, fecha_finParameter, empresaParameter);
        }
    
        public virtual ObjectResult<Lista04> getLista04(string fecha_ini, string fecha_fin, Nullable<int> empresa)
        {
            var fecha_iniParameter = fecha_ini != null ?
                new ObjectParameter("fecha_ini", fecha_ini) :
                new ObjectParameter("fecha_ini", typeof(string));
    
            var fecha_finParameter = fecha_fin != null ?
                new ObjectParameter("fecha_fin", fecha_fin) :
                new ObjectParameter("fecha_fin", typeof(string));
    
            var empresaParameter = empresa.HasValue ?
                new ObjectParameter("empresa", empresa) :
                new ObjectParameter("empresa", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Lista04>("getLista04", fecha_iniParameter, fecha_finParameter, empresaParameter);
        }
    
        public virtual ObjectResult<Lista05> getLista05(string fecha_ini, string fecha_fin, Nullable<int> empresa)
        {
            var fecha_iniParameter = fecha_ini != null ?
                new ObjectParameter("fecha_ini", fecha_ini) :
                new ObjectParameter("fecha_ini", typeof(string));
    
            var fecha_finParameter = fecha_fin != null ?
                new ObjectParameter("fecha_fin", fecha_fin) :
                new ObjectParameter("fecha_fin", typeof(string));
    
            var empresaParameter = empresa.HasValue ?
                new ObjectParameter("empresa", empresa) :
                new ObjectParameter("empresa", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Lista05>("getLista05", fecha_iniParameter, fecha_finParameter, empresaParameter);
        }
    
        public virtual ObjectResult<Lista06> getLista06(string fecha_ini, string fecha_fin, Nullable<int> empresa)
        {
            var fecha_iniParameter = fecha_ini != null ?
                new ObjectParameter("fecha_ini", fecha_ini) :
                new ObjectParameter("fecha_ini", typeof(string));
    
            var fecha_finParameter = fecha_fin != null ?
                new ObjectParameter("fecha_fin", fecha_fin) :
                new ObjectParameter("fecha_fin", typeof(string));
    
            var empresaParameter = empresa.HasValue ?
                new ObjectParameter("empresa", empresa) :
                new ObjectParameter("empresa", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Lista06>("getLista06", fecha_iniParameter, fecha_finParameter, empresaParameter);
        }
    
        public virtual ObjectResult<Lista07> getLista07(string fecha_ini, string fecha_fin, Nullable<int> empresa)
        {
            var fecha_iniParameter = fecha_ini != null ?
                new ObjectParameter("fecha_ini", fecha_ini) :
                new ObjectParameter("fecha_ini", typeof(string));
    
            var fecha_finParameter = fecha_fin != null ?
                new ObjectParameter("fecha_fin", fecha_fin) :
                new ObjectParameter("fecha_fin", typeof(string));
    
            var empresaParameter = empresa.HasValue ?
                new ObjectParameter("empresa", empresa) :
                new ObjectParameter("empresa", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Lista07>("getLista07", fecha_iniParameter, fecha_finParameter, empresaParameter);
        }
    }
}
