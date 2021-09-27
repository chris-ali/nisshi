using System;
using Nisshi.Models;
using Microsoft.EntityFrameworkCore;

namespace Nisshi.Infrastructure
{
    public class NisshiContext : DbContext
    {
        public NisshiContext(DbContextOptions<NisshiContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        {
            var users = new User[] 
            {
                new User { Id = 1, Username = "chris", FirstName = "Chris", LastName = "Ali", Email = "chris@ali.com"},
                new User { Id = 2, Username = "somebodyElse", FirstName = "Somebody", LastName = "Else", Email = "somebody@else.com"},
            };
            modelBuilder.Entity<User>().HasData(users);

            var heroes = new Hero[] 
            {
                new Hero { Id = 11, Name = "Dr Nice", DateCreated = DateTime.Now },
                new Hero { Id = 12, Name = "Narco", DateCreated = DateTime.Now },
                new Hero { Id = 13, Name = "Bombasto", DateCreated = DateTime.Now },
                new Hero { Id = 14, Name = "Celeritas", DateCreated = DateTime.Now },
                new Hero { Id = 15, Name = "Magneta", DateCreated = DateTime.Now },
                new Hero { Id = 16, Name = "RubberMan", DateCreated = DateTime.Now },
                new Hero { Id = 17, Name = "Dynama", DateCreated = DateTime.Now },
                new Hero { Id = 18, Name = "Dr IQ", DateCreated = DateTime.Now },
                new Hero { Id = 19, Name = "Magma", DateCreated = DateTime.Now },
                new Hero { Id = 20, Name = "Tornado", DateCreated = DateTime.Now },
            };
            modelBuilder.Entity<Hero>().HasData(heroes);

            var messages = new LogMessage[] 
            {
                new LogMessage { Id = 5, Contents = "Test Log Message", DateCreated = DateTime.Now, UserIdFk = 1 },
                new LogMessage { Id = 6, Contents = "Another Test Log Message", DateCreated = DateTime.Now.AddDays(-1), UserIdFk = 1 },
                new LogMessage { Id = 7, Contents = "Someone Else's Test Log Message", DateCreated = DateTime.Now.AddMonths(-1), UserIdFk = 2 }
            };
            modelBuilder.Entity<LogMessage>().HasData(messages);

            var heroUsers = new HeroUser[] 
            {
                new HeroUser{ HeroIdFk = 11, UserIdFk = 1 },
                new HeroUser{ HeroIdFk = 13, UserIdFk = 2 },
                new HeroUser{ HeroIdFk = 11, UserIdFk = 2 },
                new HeroUser{ HeroIdFk = 15, UserIdFk = 1 },
                new HeroUser{ HeroIdFk = 12, UserIdFk = 1 },
            };
            modelBuilder.Entity<HeroUser>().HasData(heroUsers);

            modelBuilder.Entity<User>(b => 
            {
                b.HasKey(x => x.Id)
                 .HasName("IDUser");
                b.Property(x => x.Username).IsRequired();
                b.Property(x => x.Email).IsRequired();
                b.Property(x => x.Password).IsRequired();
                b.HasMany(x => x.Aircraft)
                 .WithOne(x => x.Owner)
                 .HasForeignKey("IDUser");
                b.HasMany(x => x.LogbookEntries)
                 .WithOne(x => x.Owner)
                 .HasForeignKey("IDUser");
            });

            modelBuilder.Entity<Aircraft>(b =>
            {
                b.HasKey(x => x.Id)
                 .HasName("IDAircraft");
                b.HasOne(x => x.Model)
                 .WithMany(x => x.Aircraft)
                 .HasForeignKey("IDAircraft");
                b.HasMany(x => x.LogbookEntries)
                 .WithOne(x => x.Aircraft)
                 .HasForeignKey("IDAircraft");
            });

            modelBuilder.Entity<Model>(b =>
            {
                b.HasKey(x => x.Id)
                 .HasName("IDModel");
                b.HasOne(x => x.Manufacturer)
                 .WithMany(x => x.Models)
                 .HasForeignKey("IDModel");
                b.HasMany(x => x.Aircraft)
                 .WithOne(x => x.Model)
                 .HasForeignKey("IDModel");
            });

            modelBuilder.Entity<Manufacturer>(b =>
            {
                b.HasKey(x => x.Id)
                 .HasName("IDManufacturer");
                b.HasMany(x => x.Models)
                 .WithOne(x => x.Manufacturer)
                 .HasForeignKey("IDManufacturer");
            });

            modelBuilder.Entity<Airport>(b =>
            {
                b.HasKey(x => x.Id)
                 .HasName("IDAirport");
            });

            modelBuilder.Entity<CategoryClass>(b =>
            {
                b.HasKey(x => x.Id)
                 .HasName("IDCategoryClass");
            });
        }      

        public DbSet<Hero> Heroes { get; set; }
        public DbSet<LogMessage> Messages { get; set; }
        public DbSet<HeroUser> HeroUsers { get; set; }
        
        public DbSet<User> Users { get; set; }
        public DbSet<Hero> LogbookEntries { get; set; }
        public DbSet<Hero> Aircraft { get; set; }
        public DbSet<Hero> Currency { get; set; }
        public DbSet<Hero> Manufacturers { get; set; }
        public DbSet<Hero> Models { get; set; }
        public DbSet<Hero> CategoryClass { get; set; }
        public DbSet<Hero> Airports { get; set; }

    }
}