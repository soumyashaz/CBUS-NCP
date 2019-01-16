using CBUSA.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBUSA.Repository
{
   public class CBUSADbContext : DbContext
    {
        public CBUSADbContext()
            : base("name=CBUSA")
        {
            //Database.SetInitializer<CBUSADbContext>(null);
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        public DbSet<Contract> DbContracts { get; set; }
        public DbSet<UploadResource> UploadResources { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategorys { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<Market> Markets { get; set; }
        public DbSet<Builder> Builders { get; set; }
        public DbSet<Survey> Surveys { get; set; }

    }
}
