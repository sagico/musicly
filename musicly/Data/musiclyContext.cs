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
    }
}
