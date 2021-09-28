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
                new User { ID = 1, Username = "chris", FirstName = "Chris", LastName = "Ali", Email = "chris@ali.com"},
                new User { ID = 2, Username = "somebodyElse", FirstName = "Somebody", LastName = "Else", Email = "somebody@else.com"},
            };
            modelBuilder.Entity<User>().HasData(users);

            var heroes = new Hero[] 
            {
                new Hero { ID = 11, Name = "Dr Nice", DateCreated = DateTime.Now },
                new Hero { ID = 12, Name = "Narco", DateCreated = DateTime.Now },
                new Hero { ID = 13, Name = "Bombasto", DateCreated = DateTime.Now },
                new Hero { ID = 14, Name = "Celeritas", DateCreated = DateTime.Now },
                new Hero { ID = 15, Name = "Magneta", DateCreated = DateTime.Now },
                new Hero { ID = 16, Name = "RubberMan", DateCreated = DateTime.Now },
                new Hero { ID = 17, Name = "Dynama", DateCreated = DateTime.Now },
                new Hero { ID = 18, Name = "Dr IQ", DateCreated = DateTime.Now },
                new Hero { ID = 19, Name = "Magma", DateCreated = DateTime.Now },
                new Hero { ID = 20, Name = "Tornado", DateCreated = DateTime.Now },
            };
            modelBuilder.Entity<Hero>().HasData(heroes);

            var messages = new LogMessage[] 
            {
                new LogMessage { ID = 5, Contents = "Test Log Message", DateCreated = DateTime.Now, UserIdFk = 1 },
                new LogMessage { ID = 6, Contents = "Another Test Log Message", DateCreated = DateTime.Now.AddDays(-1), UserIdFk = 1 },
                new LogMessage { ID = 7, Contents = "Someone Else's Test Log Message", DateCreated = DateTime.Now.AddMonths(-1), UserIdFk = 2 }
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
                b.HasKey(x => x.ID);
                b.Property(x => x.Username).IsRequired();
                b.Property(x => x.Email).IsRequired();
                b.Property(x => x.Password).IsRequired();
                b.HasMany(x => x.Aircraft)
                 .WithOne(x => x.Owner)
                 .HasForeignKey(x => x.IDUser);
                b.HasMany(x => x.LogbookEntries)
                 .WithOne(x => x.Owner)
                 .HasForeignKey(x => x.IDUser)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<LogbookEntry>(b =>
            {
                b.HasKey(x => x.ID);
                b.HasOne(x => x.Owner)
                 .WithMany(x => x.LogbookEntries)
                 .HasForeignKey(x => x.IDUser);
                b.HasOne(x => x.Aircraft)
                 .WithMany(x => x.LogbookEntries)
                 .HasForeignKey(x => x.IDAircraft);
            });

            modelBuilder.Entity<Aircraft>(b =>
            {
                b.HasKey(x => x.ID);
                b.HasOne(x => x.Model)
                 .WithMany(x => x.Aircraft)
                 .HasForeignKey(x => x.IDModel);
                b.HasMany(x => x.LogbookEntries)
                 .WithOne(x => x.Aircraft)
                 .HasForeignKey(x => x.IDAircraft);
            });

            modelBuilder.Entity<Model>(b =>
            {
                b.HasKey(x => x.ID);
                b.HasOne(x => x.Manufacturer)
                 .WithMany(x => x.Models)
                 .HasForeignKey(x => x.IDManufacturer);
                b.HasMany(x => x.Aircraft)
                 .WithOne(x => x.Model)
                 .HasForeignKey(x => x.IDModel);
                b.HasOne(x => x.CategoryClass)
                 .WithMany(x => x.Models)
                 .HasForeignKey(x => x.IDCategoryClass)
                 .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<Manufacturer>(b =>
            {
                b.HasKey(x => x.ID);
                b.HasMany(x => x.Models)
                 .WithOne(x => x.Manufacturer)
                 .HasForeignKey(x => x.IDManufacturer);
            });

            modelBuilder.Entity<Airport>(b =>
            {
                b.HasKey(x => x.ID);
            });

            modelBuilder.Entity<CategoryClass>(b =>
            {
                b.HasKey(x => x.ID);
            });
        }      

        public DbSet<Hero> Heroes { get; set; }
        public DbSet<LogMessage> Messages { get; set; }
        public DbSet<HeroUser> HeroUsers { get; set; }
        
        public DbSet<User> Users { get; set; }
        public DbSet<LogbookEntry> LogbookEntries { get; set; }
        public DbSet<Aircraft> Aircraft { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<Model> Models { get; set; }
        public DbSet<CategoryClass> CategoryClass { get; set; }
        public DbSet<Airport> Airports { get; set; }

    }
}