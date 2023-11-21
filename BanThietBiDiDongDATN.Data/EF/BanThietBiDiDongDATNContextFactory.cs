using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanThietBiDiDongDATN.Data.EF
{
    public class BanThietBiDiDongDATNContextFactory : IDesignTimeDbContextFactory<BanThietBiDiDongDATNDbContext>
    {
        public BanThietBiDiDongDATNDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("datasetting.json")
                .Build();
            var optionBuilder = new DbContextOptionsBuilder<BanThietBiDiDongDATNDbContext>();
            var ConnectionString = configuration.GetConnectionString("DATN_Database");
            optionBuilder.UseSqlServer(ConnectionString);
            return new BanThietBiDiDongDATNDbContext(optionBuilder.Options);
        }
    }
}
