using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Erfpacht058_API.Models;
using BCrypt.Net;

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
        }
    }
}
