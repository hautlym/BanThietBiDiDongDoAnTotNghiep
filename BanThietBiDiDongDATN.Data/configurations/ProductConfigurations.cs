using BanThietBiDiDongDATN.Data.entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BanThietBiDiDongDATN.Data.configurations
{
    public class ProductConfigurations : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasOne(x => x.brand).WithMany(x => x.products).HasForeignKey(x => x.BrandId);
            builder.HasOne(x => x.category).WithMany(x => x.Products).HasForeignKey(x => x.CategoryId);
        }
    }
}
