using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MarketingCORE.Models;

namespace CORE
{
    public class MarketingCOREContext : DbContext
    {
        public MarketingCOREContext(DbContextOptions<MarketingCOREContext> options)
            : base(options)
        {
        }

        public DbSet<MarketingCORE.Models.Usuario> Usuario { get; set; } = default!;

        public DbSet<MarketingCORE.Models.KPIXCampana> KPIXCampana { get; set; }

        public DbSet<MarketingCORE.Models.KPI> KPI { get; set; }

        public DbSet<MarketingCORE.Models.Campana> Campana { get; set; }

        public DbSet<MarketingCORE.Models.Buyer> Buyer { get; set; }

    }
}
