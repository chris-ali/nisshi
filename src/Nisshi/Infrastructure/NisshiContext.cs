using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Nisshi.Infrastructure.Security;
using Nisshi.Models;
using System;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Nisshi.Infrastructure
{
    /// <summary>
    /// EFCore implmentation for the Nisshi database - Contains definitions 
    /// for table relations and seeding for an in-memory database and 
    /// transactional capability
    /// </summary>
    public class NisshiContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<LogbookEntry> LogbookEntries { get; set; }
        public DbSet<Aircraft> Aircraft { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<Model> Models { get; set; }
        public DbSet<CategoryClass> CategoryClasses { get; set; }
        public DbSet<Airport> Airports { get; set; }

        private IDbContextTransaction currentTransaction;

        public NisshiContext(DbContextOptions<NisshiContext> options) : base(options)
        {
        }

        /// <summary>
        /// Defines table relations and in-memory seeding
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        {
            modelBuilder.Entity<User>(b => 
            {
                b.HasKey(x => x.Id);
                b.Property(x => x.Username).IsRequired();
                b.Property(x => x.Email).IsRequired();
                b.Property(x => x.Hash).IsRequired();
                b.Property(x => x.Salt).IsRequired();
                b.HasMany(x => x.Aircraft)
                 .WithOne(x => x.Owner)
                 .HasForeignKey(x => x.IdUser);
                b.HasMany(x => x.LogbookEntries)
                 .WithOne(x => x.Owner)
                 .HasForeignKey(x => x.IdUser)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<LogbookEntry>(b =>
            {
                b.HasKey(x => x.Id);
                b.HasOne(x => x.Owner)
                 .WithMany(x => x.LogbookEntries)
                 .HasForeignKey(x => x.IdUser);
                b.HasOne(x => x.Aircraft)
                 .WithMany(x => x.LogbookEntries)
                 .HasForeignKey(x => x.IdAircraft);
            });

            modelBuilder.Entity<Aircraft>(b =>
            {
                b.HasKey(x => x.Id);
                b.HasOne(x => x.Model)
                 .WithMany(x => x.Aircraft)
                 .HasForeignKey(x => x.IdModel);
                b.HasMany(x => x.LogbookEntries)
                 .WithOne(x => x.Aircraft)
                 .HasForeignKey(x => x.IdAircraft);
            });

            modelBuilder.Entity<Model>(b =>
            {
                b.HasKey(x => x.Id);
                b.HasOne(x => x.Manufacturer)
                 .WithMany(x => x.Models)
                 .HasForeignKey(x => x.IdManufacturer);
                b.HasMany(x => x.Aircraft)
                 .WithOne(x => x.Model)
                 .HasForeignKey(x => x.IdModel);
                b.HasOne(x => x.CategoryClass)
                 .WithMany(x => x.Models)
                 .HasForeignKey(x => x.IdCategoryClass)
                 .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<Manufacturer>(b =>
            {
                b.HasKey(x => x.Id);
                b.HasMany(x => x.Models)
                 .WithOne(x => x.Manufacturer)
                 .HasForeignKey(x => x.IdManufacturer);
            });

            modelBuilder.Entity<Airport>(b =>
            {
                b.HasKey(x => x.AirportCode);
            });

            modelBuilder.Entity<CategoryClass>(b =>
            {
                b.HasKey(x => x.Id);
            });

            #region In Memory Seeding
            var hasher = new PasswordHasher();
            var salt1 = Guid.NewGuid().ToByteArray();
            var salt2 = Guid.NewGuid().ToByteArray();
            var users = new User[] 
            {
                new User { Id = 1, Username = "chris", FirstName = "Chris", LastName = "Ali", Email = "chris@ali.com", 
                    Hash = hasher.Hash("test123", salt1), Salt = salt1 },
                new User { Id = 2, Username = "somebodyElse", FirstName = "Somebody", LastName = "Else", Email = "somebody@else.com", 
                    Hash = hasher.Hash("test456", salt2), Salt = salt2 },
            };
            modelBuilder.Entity<User>().HasData(users);

            var categoryClasses = new CategoryClass[] 
            {
                new CategoryClass { Id = 1, Category = "Airplane", Class = "Single Engine Land", CatClass = "ASEL" },
                new CategoryClass { Id = 2, Category = "Airplane", Class = "Multi Engine Land", CatClass = "AMEL" },
            };
            modelBuilder.Entity<CategoryClass>().HasData(categoryClasses);

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
                new Model { Id = 1, Family = "172", ModelName = "172N", IdManufacturer = manufacturers[0].Id, IdCategoryClass = categoryClasses[0].Id },
                new Model { Id = 2, Family = "182", ModelName = "182Q", IdManufacturer = manufacturers[0].Id, IdCategoryClass = categoryClasses[0].Id, IsSimOnly = true },
                new Model { Id = 3, Family = "182", ModelName = "182S", IdManufacturer = manufacturers[0].Id, IdCategoryClass = categoryClasses[0].Id },
                new Model { Id = 4, Family = "PA-28", ModelName = "PA-28-160", IdManufacturer = manufacturers[1].Id, IdCategoryClass = categoryClasses[0].Id },
                new Model { Id = 5, Family = "PA-28", ModelName = "PA-28-200", IdManufacturer = manufacturers[1].Id, IdCategoryClass = categoryClasses[1].Id },
                new Model { Id = 6, Family = "PA-44", ModelName = "PA-44-200", IdManufacturer = manufacturers[1].Id, IdCategoryClass = categoryClasses[1].Id },
                new Model { Id = 7, Family = "Bonanza", ModelName = "A36", IdManufacturer = manufacturers[2].Id, IdCategoryClass = categoryClasses[0].Id },
                new Model { Id = 8, Family = "Bonanza", ModelName = "V35", IdManufacturer = manufacturers[2].Id, IdCategoryClass = categoryClasses[0].Id, IsSimOnly = true },
                new Model { Id = 9, Family = "Baron", ModelName = "B58", IdManufacturer = manufacturers[2].Id, IdCategoryClass = categoryClasses[0].Id },
                new Model { Id = 10, Family = "M20", ModelName = "M20J", IdManufacturer = manufacturers[3].Id, IdCategoryClass = categoryClasses[0].Id },
                new Model { Id = 11, Family = "M20", ModelName = "M20F", IdManufacturer = manufacturers[3].Id, IdCategoryClass = categoryClasses[0].Id },
            };
            modelBuilder.Entity<Model>().HasData(models);

            var aircraft = new Aircraft[] 
            {
                new Aircraft { Id = 1, IdModel = models[1].Id, IdUser = users[0].Id, TailNumber = "N8445D" },
                new Aircraft { Id = 2, IdModel = models[4].Id, IdUser = users[0].Id, TailNumber = "N5427D" },
                new Aircraft { Id = 3, IdModel = models[0].Id, IdUser = users[1].Id, TailNumber = "N9440D" },
                new Aircraft { Id = 4, IdModel = models[0].Id, IdUser = users[1].Id, TailNumber = "N9441D" },
                new Aircraft { Id = 5, IdModel = models[0].Id, IdUser = users[1].Id, TailNumber = "N9442D" },
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
                    NumInstrumentApproaches = 1, Route = $"{airports[0].AirportCode} {airports[1].AirportCode}", IdAircraft = aircraft[0].Id, IdUser = users[0].Id },
                new LogbookEntry { Id = 2, Comments = "Test2", FlightDate = DateTime.Now.AddDays(-2), PIC = 2.7m, CrossCountry = 2.7m, NumLandings = 2, 
                    NumInstrumentApproaches = 1, Route = $"{airports[1].AirportCode} {airports[2].AirportCode}", IdAircraft = aircraft[1].Id, IdUser = users[0].Id },
                new LogbookEntry { Id = 3, Comments = "Test3", FlightDate = DateTime.Now.AddDays(-3), PIC = 3.0m, CrossCountry = 3.0m, NumLandings = 3, 
                    NumInstrumentApproaches = 4, Route = $"{airports[0].AirportCode} {airports[2].AirportCode}", IdAircraft = aircraft[1].Id, IdUser = users[0].Id },
                new LogbookEntry { Id = 4, Comments = "Test4", FlightDate = DateTime.Now.AddDays(-4), PIC = 1.2m, CrossCountry = 1.2m, NumLandings = 1, 
                    NumInstrumentApproaches = 1, Route = $"{airports[2].AirportCode} {airports[0].AirportCode}", IdAircraft = aircraft[0].Id, IdUser = users[0].Id },
                new LogbookEntry { Id = 5, Comments = "Test5", FlightDate = DateTime.Now.AddDays(-5), PIC = 2.0m, CrossCountry = 2.0m, NumLandings = 1, 
                    NumInstrumentApproaches = 3, Route = $"{airports[1].AirportCode} {airports[1].AirportCode}", IdAircraft = aircraft[2].Id, IdUser = users[1].Id },
                new LogbookEntry { Id = 6, Comments = "Test6", FlightDate = DateTime.Now.AddDays(-6), PIC = 1.3m, CrossCountry = 1.3m, Night = 1.3m, NumLandings = 4, 
                    NumInstrumentApproaches = 2, Route = $"{airports[0].AirportCode} {airports[1].AirportCode}", IdAircraft = aircraft[4].Id, IdUser = users[1].Id },
                new LogbookEntry { Id = 7, Comments = "Test7", FlightDate = DateTime.Now.AddDays(-7), PIC = 2.0m, NumLandings = 1, 
                    NumInstrumentApproaches = 1, Route = $"{airports[1].AirportCode} {airports[1].AirportCode}", IdAircraft = aircraft[4].Id, IdUser = users[1].Id },
                new LogbookEntry { Id = 8, Comments = "Test8", FlightDate = DateTime.Now.AddDays(-8), PIC = 4.2m, CrossCountry = 4.2m, NumLandings = 2, 
                    NumInstrumentApproaches = 2, Route = $"{airports[1].AirportCode} {airports[2].AirportCode}", IdAircraft = aircraft[0].Id, IdUser = users[0].Id },
                new LogbookEntry { Id = 9, Comments = "Test9", FlightDate = DateTime.Now.AddDays(-9), PIC = 1.1m, Night = 2.0m, NumLandings = 1, 
                    NumInstrumentApproaches = 1, Route = $"{airports[2].AirportCode} {airports[1].AirportCode}", IdAircraft = aircraft[0].Id, IdUser = users[0].Id },
            };
            modelBuilder.Entity<LogbookEntry>().HasData(logbookEntries);
            #endregion
        }      
        
        public void BeginTransaction()
        {
            if (currentTransaction != null)
                return;

            if (!Database.IsInMemory())
                currentTransaction = Database.BeginTransaction(IsolationLevel.ReadCommitted);
        }

        public async Task CommitTransaction()
        {
            try
            {
                if (currentTransaction != null)
                    await currentTransaction.CommitAsync();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
            finally
            {
                if (currentTransaction != null)
                {
                    currentTransaction.Dispose();
                    currentTransaction = null;
                }
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                currentTransaction?.Rollback();     
            }
            finally
            {
                if (currentTransaction != null)
                {
                    currentTransaction.Dispose();
                    currentTransaction = null;
                }
            }
        }
    }
}