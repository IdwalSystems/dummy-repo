using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MSNKTest.Models;

    public class MSNKDBContext : DbContext
    {
        public MSNKDBContext (DbContextOptions<MSNKDBContext> options)
            : base(options)
        {
        }

        public DbSet<MSNKTest.Models.KW> KW { get; set; }

        public DbSet<MSNKTest.Models.CaraBayar> CaraBayar { get; set; }

        public DbSet<MSNKTest.Models.Modul> Modul { get; set; }

        public DbSet<MSNKTest.Models.Bank> Bank { get; set; }
    }
