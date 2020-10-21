using Microsoft.EntityFrameworkCore;

namespace MunicipalityTaxes.DataAccess.Model
{
    public class MunicipalityContext : DbContext
    {
        public MunicipalityContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Municipality> Municipality { get; set; }

        public DbSet<MunicipalityTax> MunicipalityTax { get; set; }

        public DbSet<MunicipalityTaxType> MunicipalityTaxType { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MunicipalityTaxType>()
                .Property(x => x.Id)
                .ValueGeneratedNever();

            modelBuilder.Entity<MunicipalityTaxType>()
                .HasData(
                    new MunicipalityTaxType
                    {
                        Id = MunicipalityTaxTypeEnum.Daily,
                        Name = "Daily",
                    },
                    new MunicipalityTaxType
                    {
                        Id = MunicipalityTaxTypeEnum.Weekly,
                        Name = "Weekly",
                    },
                    new MunicipalityTaxType
                    {
                        Id = MunicipalityTaxTypeEnum.Monthly,
                        Name = "Monthly",
                    },
                    new MunicipalityTaxType
                    {
                        Id = MunicipalityTaxTypeEnum.Yearly,
                        Name = "Yearly",
                    });
        }
    }
}
