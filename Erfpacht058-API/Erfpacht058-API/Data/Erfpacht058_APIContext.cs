﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Erfpacht058_API.Models;
using BCrypt.Net;
using Erfpacht058_API.Models.Eigendom;
using Erfpacht058_API.Models.OvereenkomstNS;

namespace Erfpacht058_API.Data
{
    public class Erfpacht058_APIContext : DbContext
    {
        public Erfpacht058_APIContext (DbContextOptions<Erfpacht058_APIContext> options)
            : base(options)
        {
        }

        public DbSet<Erfpacht058_API.Models.Gebruiker> Gebruiker { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Gebruiker>().HasData(
                    new Gebruiker
                    {
                        Id = 1,
                        Naam = "Gebruiker",
                        Voornamen = "Eerste",
                        Emailadres = "test@gebruiker.nl",
                        Role = Rol.Beheerder,
                        Actief = true,
                        Wachtwoord = BCrypt.Net.BCrypt.HashPassword("TEST123")
                    }
                );

            // Relaties definieren voor Eigendom
            modelBuilder.Entity<Eigendom>()
                .HasOne(e => e.Adres)
                .WithOne(a => a.Eigendom)
                .HasForeignKey<Adres>(a => a.EigendomId);

        }
        public DbSet<Erfpacht058_API.Models.Eigendom.Eigendom> Eigendom { get; set; } = default!;
        public DbSet<Erfpacht058_API.Models.Eigendom.Adres> Adres { get; set; } = default!;
        public DbSet<Erfpacht058_API.Models.Eigendom.Eigenaar> Eigenaar { get; set; } = default!;
        public DbSet<Erfpacht058_API.Models.Eigendom.Herziening> Herziening { get; set; } = default!;
        public DbSet<Erfpacht058_API.Models.Eigendom.Kadaster> Kadaster { get; set; } = default!;
        public DbSet<Erfpacht058_API.Models.Bestand> Bestand { get; set; } = default!;
        public DbSet<Erfpacht058_API.Models.OvereenkomstNS.Overeenkomst> Overeenkomst { get; set; } = default!;
        public DbSet<Erfpacht058_API.Models.OvereenkomstNS.Financien> Financien { get; set; } = default!;
    }
}
