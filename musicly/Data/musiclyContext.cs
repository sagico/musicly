using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using musicly.Models;

namespace musicly.Data
{
    public class musiclyContext : DbContext
    {
        public musiclyContext (DbContextOptions<musiclyContext> options)
            : base(options)
        {
        }

        public DbSet<musicly.Models.User> User { get; set; }

        public DbSet<musicly.Models.InstrumentType> InstrumentType { get; set; }

        public DbSet<musicly.Models.Instrument> Instrument { get; set; }

        public DbSet<musicly.Models.InstrumentOrder> InstrumentOrder { get; set; }

        public DbSet<musicly.Models.Order> Orders { get; set; }

        public DbSet<musicly.Models.Store> Store { get; set; }
    }
}
