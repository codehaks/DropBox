using DropBox.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DropBox.Data
{
    public class DocDbContext:DbContext
    {
        public DocDbContext(DbContextOptions<DocDbContext> options)
              : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public DbSet<Doc> Docs { get; set; }

    }
}
