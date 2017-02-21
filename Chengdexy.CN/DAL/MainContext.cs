using Chengdexy.CN.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace Chengdexy.CN.DAL
{
    public class MainContext : DbContext
    {
        public MainContext() : base("MainContext")
        {

        }
        public DbSet<Program> Programs { get; set; }
        public DbSet<ProgramEdition> ProgramEditions { get; set; }
        public DbSet<AboutItem> AboutItems { get; set; }
        public DbSet<BlogPage> BlogPages { get; set; }
        public DbSet<Jumbotron> Jumbotrons { get; set; }
        public DbSet<NavbarItem> NavbarItems { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}