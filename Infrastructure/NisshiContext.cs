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
                new User { Id = 1, Username = "chris", FirstName = "Chris", LastName = "Ali", Email = "chris@ali.com" },
                new User { Id = 2, Username = "somebodyElse", FirstName = "Somebody", LastName = "Else", Email = "somebody@else.com" },
            };
            modelBuilder.Entity<User>().HasData(users);

            var categoryClasses = new CategoryClass[] 
            {
                new CategoryClass { Id = 1, Category = "Airplane", Class = "Single Engine Land", CatClass = "ASEL" },
                new CategoryClass { Id = 2, Category = "Airplane", Class = "Multi Engine Land", CatClass = "AMEL" },
            };
            modelBuilder.Entity<User>().HasData(users);

            var manufacturers = new Manufacturer[] 
            {
                new Manufacturer { Id = 1, ManufacturerName = "Cessna" },
                new Manufacturer { Id = 2, ManufacturerName = "Piper" },
                new Manufacturer { Id = 3, ManufacturerName = "Beechcraft" },
                new Manufacturer { Id = 4, ManufacturerName = "Mooney" },
            };
            modelBuilder.Entity<Manufacturer>().HasData(manufacturers);

            var models = new Model[]
            {
                new Model { Id = 1, Family = "172", ModelName = "172N", IDManufacturer = manufacturers[0].Id, IDCategoryClass = categoryClasses[0].Id },
                new Model { Id = 2, Family = "182", ModelName = "182Q", IDManufacturer = manufacturers[0].Id, IDCategoryClass = categoryClasses[0].Id, IsSimOnly = true },
                new Model { Id = 3, Family = "182", ModelName = "182S", IDManufacturer = manufacturers[0].Id, IDCategoryClass = categoryClasses[0].Id },
                new Model { Id = 4, Family = "PA-28", ModelName = "PA-28-160", IDManufacturer = manufacturers[1].Id, IDCategoryClass = categoryClasses[0].Id },
                new Model { Id = 5, Family = "PA-28", ModelName = "PA-28-200", IDManufacturer = manufacturers[1].Id, IDCategoryClass = categoryClasses[1].Id },
                new Model { Id = 6, Family = "PA-44", ModelName = "PA-44-200", IDManufacturer = manufacturers[1].Id, IDCategoryClass = categoryClasses[1].Id },
                new Model { Id = 7, Family = "Bonanza", ModelName = "A36", IDManufacturer = manufacturers[2].Id, IDCategoryClass = categoryClasses[0].Id },
                new Model { Id = 8, Family = "Bonanza", ModelName = "V35", IDManufacturer = manufacturers[2].Id, IDCategoryClass = categoryClasses[0].Id, IsSimOnly = true },
                new Model { Id = 9, Family = "Baron", ModelName = "B58", IDManufacturer = manufacturers[2].Id, IDCategoryClass = categoryClasses[0].Id },
                new Model { Id = 10, Family = "M20", ModelName = "M20J", IDManufacturer = manufacturers[3].Id, IDCategoryClass = categoryClasses[0].Id },
                new Model { Id = 10, Family = "M20", ModelName = "M20F", IDManufacturer = manufacturers[3].Id, IDCategoryClass = categoryClasses[0].Id },
            };
            modelBuilder.Entity<Model>().HasData(models);

            var aircraft = new Aircraft[] 
            {
                new Aircraft { Id = 1, IDModel = models[1].Id, IDUser = users[0].Id, TailNumber = "N8445D" },
                new Aircraft { Id = 2, IDModel = models[4].Id, IDUser = users[0].Id, TailNumber = "N5427D" },
                new Aircraft { Id = 3, IDModel = models[0].Id, IDUser = users[1].Id, TailNumber = "N9440D" },
                new Aircraft { Id = 4, IDModel = models[0].Id, IDUser = users[1].Id, TailNumber = "N9441D" },
                new Aircraft { Id = 5, IDModel = models[0].Id, IDUser = users[1].Id, TailNumber = "N9442D" },
            };
            modelBuilder.Entity<Aircraft>().HasData(aircraft);

            var airports = new Airport[]
            {
                new Airport { AirportCode = "KTTN", FacilityName = "Trenton Mercer Airport", Latitude = 41, Longitude = -74, Preferred = true },
                new Airport { AirportCode = "KRDG", FacilityName = "Reading Regional Airport", Latitude = 40, Longitude = -76, Preferred = true },
                new Airport { AirportCode = "KACY", FacilityName = "Atlantic City International Airport", Latitude = 40, Longitude = -74, Preferred = true },
            };
            modelBuilder.Entity<Airport>().HasData(airports);

            var logbookEntries = new LogbookEntry[]
            {
                new LogbookEntry { Id = 1, Comments = "Test1", FlightDate = DateTime.Now.AddDays(-1), PIC = 2.0m, Night = 2.0m, NumLandings = 1, 
                    NumInstrumentApproaches = 1, Route = $"{airports[0].AirportCode} {airports[1].AirportCode}", IDAircraft = aircraft[0].Id, IDUser = users[0].Id },
                new LogbookEntry { Id = 2, Comments = "Test2", FlightDate = DateTime.Now.AddDays(-2), PIC = 2.7m, CrossCountry = 2.7m, NumLandings = 2, 
                    NumInstrumentApproaches = 1, Route = $"{airports[1].AirportCode} {airports[2].AirportCode}", IDAircraft = aircraft[1].Id, IDUser = users[0].Id },
                new LogbookEntry { Id = 3, Comments = "Test3", FlightDate = DateTime.Now.AddDays(-3), PIC = 3.0m, CrossCountry = 3.0m, NumLandings = 3, 
                    NumInstrumentApproaches = 4, Route = $"{airports[0].AirportCode} {airports[2].AirportCode}", IDAircraft = aircraft[1].Id, IDUser = users[0].Id },
                new LogbookEntry { Id = 4, Comments = "Test4", FlightDate = DateTime.Now.AddDays(-4), PIC = 1.2m, CrossCountry = 1.2m, NumLandings = 1, 
                    NumInstrumentApproaches = 1, Route = $"{airports[3].AirportCode} {airports[0].AirportCode}", IDAircraft = aircraft[0].Id, IDUser = users[0].Id },
                new LogbookEntry { Id = 5, Comments = "Test5", FlightDate = DateTime.Now.AddDays(-5), PIC = 2.0m, CrossCountry = 2.0m, NumLandings = 1, 
                    NumInstrumentApproaches = 3, Route = $"{airports[1].AirportCode} {airports[1].AirportCode}", IDAircraft = aircraft[2].Id, IDUser = users[1].Id },
                new LogbookEntry { Id = 6, Comments = "Test6", FlightDate = DateTime.Now.AddDays(-6), PIC = 1.3m, CrossCountry = 1.3m, Night = 1.3m, NumLandings = 4, 
                    NumInstrumentApproaches = 2, Route = $"{airports[0].AirportCode} {airports[1].AirportCode}", IDAircraft = aircraft[4].Id, IDUser = users[1].Id },
                new LogbookEntry { Id = 7, Comments = "Test7", FlightDate = DateTime.Now.AddDays(-7), PIC = 2.0m, NumLandings = 1, 
                    NumInstrumentApproaches = 1, Route = $"{airports[1].AirportCode} {airports[1].AirportCode}", IDAircraft = aircraft[4].Id, IDUser = users[1].Id },
                new LogbookEntry { Id = 8, Comments = "Test8", FlightDate = DateTime.Now.AddDays(-8), PIC = 4.2m, CrossCountry = 4.2m, NumLandings = 2, 
                    NumInstrumentApproaches = 2, Route = $"{airports[1].AirportCode} {airports[2].AirportCode}", IDAircraft = aircraft[0].Id, IDUser = users[0].Id },
                new LogbookEntry { Id = 9, Comments = "Test9", FlightDate = DateTime.Now.AddDays(-9), PIC = 1.1m, Night = 2.0m, NumLandings = 1, 
                    NumInstrumentApproaches = 1, Route = $"{airports[2].AirportCode} {airports[1].AirportCode}", IDAircraft = aircraft[0].Id, IDUser = users[0].Id },
            };
            modelBuilder.Entity<Airport>().HasData(airports);

            modelBuilder.Entity<User>(b => 
            {
                b.HasKey(x => x.Id);
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
                b.HasKey(x => x.Id);
                b.HasOne(x => x.Owner)
                 .WithMany(x => x.LogbookEntries)
                 .HasForeignKey(x => x.IDUser);
                b.HasOne(x => x.Aircraft)
                 .WithMany(x => x.LogbookEntries)
                 .HasForeignKey(x => x.IDAircraft);
            });

            modelBuilder.Entity<Aircraft>(b =>
            {
                b.HasKey(x => x.Id);
                b.HasOne(x => x.Model)
                 .WithMany(x => x.Aircraft)
                 .HasForeignKey(x => x.IDModel);
                b.HasMany(x => x.LogbookEntries)
                 .WithOne(x => x.Aircraft)
                 .HasForeignKey(x => x.IDAircraft);
            });

            modelBuilder.Entity<Model>(b =>
            {
                b.HasKey(x => x.Id);
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
                b.HasKey(x => x.Id);
                b.HasMany(x => x.Models)
                 .WithOne(x => x.Manufacturer)
                 .HasForeignKey(x => x.IDManufacturer);
            });

            modelBuilder.Entity<Airport>(b =>
            {
                b.HasKey(x => x.Id);
            });

            modelBuilder.Entity<CategoryClass>(b =>
            {
                b.HasKey(x => x.Id);
            });
        }      
        
        public DbSet<User> Users { get; set; }
        public DbSet<LogbookEntry> LogbookEntries { get; set; }
        public DbSet<Aircraft> Aircraft { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<Model> Models { get; set; }
        public DbSet<CategoryClass> CategoryClass { get; set; }
        public DbSet<Airport> Airports { get; set; }

    }
}