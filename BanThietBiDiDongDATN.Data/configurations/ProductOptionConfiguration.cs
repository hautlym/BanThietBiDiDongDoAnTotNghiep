using BanThietBiDiDongDATN.Data.entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanThietBiDiDongDATN.Data.configurations
{
    public class ProductOptionConfiguration:IEntityTypeConfiguration<ProductOption>
    {
        public void Configure(EntityTypeBuilder<ProductOption> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasOne(x=>x.product).WithMany(x=>x.productOptions).HasForeignKey(x=>x.ProductId);
        }
    }
}
