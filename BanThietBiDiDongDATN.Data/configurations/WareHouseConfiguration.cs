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
    internal class WareHouseConfiguration : IEntityTypeConfiguration<WareHouse>
    {
        public void Configure(EntityTypeBuilder<WareHouse> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasOne(x=>x.ProductOption).WithMany(x=>x.wareHouses).HasForeignKey(x=>x.ProductOptionId);
        }
    }
}
