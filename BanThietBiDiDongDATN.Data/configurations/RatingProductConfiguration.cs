using BanThietBiDiDongDATN.Data.entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BanThietBiDiDongDATN.Data.configurations
{
    public class RatingProductConfiguration : IEntityTypeConfiguration<RatingProduct>
    {
        public void Configure(EntityTypeBuilder<RatingProduct> builder)
        {
            builder.HasOne(x => x.product).WithMany(x => x.ratingProducts).HasForeignKey(x => x.productId);
        }
    }
}
