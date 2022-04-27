using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Nisshi.Infrastructure.Enums;
using Nisshi.Infrastructure.Security;
using Nisshi.Models;
using Nisshi.Models.Users;

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
        public DbSet<MaintenanceEntry> MaintenanceEntries { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }

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
                b.HasMany(x => x.Vehicles)
                 .WithOne(x => x.Owner)
                 .HasForeignKey(x => x.IdUser);
                b.HasMany(x => x.MaintenanceEntries)
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

            modelBuilder.Entity<MaintenanceEntry>(b =>
            {
                b.HasKey(x => x.Id);
                b.HasOne(x => x.Owner)
                 .WithMany(x => x.MaintenanceEntries)
                 .HasForeignKey(x => x.IdUser);
                b.HasOne(x => x.Vehicle)
                 .WithMany(x => x.MaintenanceEntries)
                 .HasForeignKey(x => x.IdVehicle);
            });

            modelBuilder.Entity<Vehicle>(b =>
            {
                b.HasKey(x => x.Id);
                b.HasMany(x => x.MaintenanceEntries)
                 .WithOne(x => x.Vehicle)
                 .HasForeignKey(x => x.IdVehicle);
            });

            // Set each DB item's name to lower case to match mysql's lower casing
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                entity.SetTableName(entity.GetTableName().ToLower(CultureInfo.InvariantCulture));

                foreach (var property in entity.GetProperties())
                    property.SetColumnName(property.Name.ToLower(CultureInfo.InvariantCulture));

                foreach (var key in entity.GetKeys())
                    key.SetName(key.GetName().ToLower(CultureInfo.InvariantCulture));

                foreach (var fk in entity.GetForeignKeys())
                    fk.SetConstraintName(fk.GetConstraintName().ToLower(CultureInfo.InvariantCulture));

                foreach (var index in entity.GetIndexes())
                    index.SetDatabaseName(index.GetDatabaseName().ToLower(CultureInfo.InvariantCulture));
            }

            if (Database.IsInMemory())
                SeedMe(modelBuilder);
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

        /// <summary>
        /// Seeds in memory database for unit tests, demos, etc.
        /// </summary>
        /// <param name="modelBuilder"></param>
        private void SeedMe(ModelBuilder modelBuilder)
        {
            var hasher = new PasswordHasher();
            var salt1 = Guid.NewGuid().ToByteArray();
            var salt2 = Guid.NewGuid().ToByteArray();
            var users = new User[]
            {
                new User { Id = 1, Username = "chris", FirstName = "Chris", LastName = "Ali", Email = "chris@ali.com",
                    Hash = hasher.Hash("tesT123!", salt1), Salt = salt1, UserType = UserType.Administrator },
                new User { Id = 2, Username = "somebodyElse", FirstName = "Somebody", LastName = "Else", Email = "somebody@else.com",
                    Hash = hasher.Hash("tesT456!", salt2), Salt = salt2, UserType = UserType.User },
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
                new Model { Id = 1, Family = "172", TypeName = "C172", ModelName = "172N", IdManufacturer = manufacturers[0].Id, IdCategoryClass = categoryClasses[0].Id },
                new Model { Id = 2, Family = "182", TypeName = "C182", ModelName = "182Q", IdManufacturer = manufacturers[0].Id, IdCategoryClass = categoryClasses[0].Id, IsSimOnly = true },
                new Model { Id = 3, Family = "182", TypeName = "C182", ModelName = "182S", IdManufacturer = manufacturers[0].Id, IdCategoryClass = categoryClasses[0].Id },
                new Model { Id = 4, Family = "PA-28", TypeName = "PA28", ModelName = "PA-28-160", IdManufacturer = manufacturers[1].Id, IdCategoryClass = categoryClasses[0].Id },
                new Model { Id = 5, Family = "PA-28", TypeName = "PA28", ModelName = "PA-28-200", IdManufacturer = manufacturers[1].Id, IdCategoryClass = categoryClasses[1].Id },
                new Model { Id = 6, Family = "PA-44", TypeName = "PA44", ModelName = "PA-44-200", IdManufacturer = manufacturers[1].Id, IdCategoryClass = categoryClasses[1].Id },
                new Model { Id = 7, Family = "Bonanza", TypeName = "BE36", ModelName = "A36", IdManufacturer = manufacturers[2].Id, IdCategoryClass = categoryClasses[0].Id },
                new Model { Id = 8, Family = "Bonanza", TypeName = "BE35", ModelName = "V35", IdManufacturer = manufacturers[2].Id, IdCategoryClass = categoryClasses[0].Id, IsSimOnly = true },
                new Model { Id = 9, Family = "Baron", TypeName = "BE58", ModelName = "B58", IdManufacturer = manufacturers[2].Id, IdCategoryClass = categoryClasses[0].Id },
                new Model { Id = 10, Family = "M20", TypeName = "M20", ModelName = "M20J", IdManufacturer = manufacturers[3].Id, IdCategoryClass = categoryClasses[0].Id },
                new Model { Id = 11, Family = "M20", TypeName = "M20", ModelName = "M20F", IdManufacturer = manufacturers[3].Id, IdCategoryClass = categoryClasses[0].Id },
            };
            modelBuilder.Entity<Model>().HasData(models);

            var aircraft = new Aircraft[]
            {
                new Aircraft { Id = 1, IdModel = models[1].Id, IdUser = users[0].Id, TailNumber = "N8445D", InstanceType = InstanceType.Real, Last100Hobbs = 235, LastAnnual = DateTime.Now.AddMonths(-5),
                    Notes = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Iaculis eu non diam phasellus vestibulum lorem." },
                new Aircraft { Id = 2, IdModel = models[4].Id, IdUser = users[0].Id, TailNumber = "N5427D", InstanceType = InstanceType.Simulation,
                    Notes = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Iaculis eu non diam phasellus vestibulum lorem." },
                new Aircraft { Id = 3, IdModel = models[0].Id, IdUser = users[1].Id, TailNumber = "N9440D", InstanceType = InstanceType.Real, LastOilHobbs = 1330, LastTransponder = DateTime.Now.AddMonths(-4),
                    Notes = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Iaculis eu non diam phasellus vestibulum lorem." },
                new Aircraft { Id = 4, IdModel = models[0].Id, IdUser = users[1].Id, TailNumber = "N9441D", InstanceType = InstanceType.Simulation,
                    Notes = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Iaculis eu non diam phasellus vestibulum lorem." },
                new Aircraft { Id = 5, IdModel = models[0].Id, IdUser = users[1].Id, TailNumber = "N9442D", InstanceType = InstanceType.Real, LastEngineHobbs = 3242, LastPitotStatic = DateTime.Now.AddMonths(-4),
                    Notes = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Iaculis eu non diam phasellus vestibulum lorem." },
                new Aircraft { Id = 6, IdModel = models[2].Id, IdUser = users[0].Id, TailNumber = "N9443D", InstanceType = InstanceType.Real, LastOilHobbs = 1330, LastTransponder = DateTime.Now.AddMonths(-4),
                    Notes = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Iaculis eu non diam phasellus vestibulum lorem." },
                new Aircraft { Id = 7, IdModel = models[4].Id, IdUser = users[0].Id, TailNumber = "N9444D", InstanceType = InstanceType.Real, LastOilHobbs = 1330, LastTransponder = DateTime.Now.AddMonths(-4),
                    Notes = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Iaculis eu non diam phasellus vestibulum lorem." },
                new Aircraft { Id = 8, IdModel = models[7].Id, IdUser = users[0].Id, TailNumber = "N9445D", InstanceType = InstanceType.Simulation, LastOilHobbs = 1330, LastTransponder = DateTime.Now.AddMonths(-4),
                    Notes = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Iaculis eu non diam phasellus vestibulum lorem." },
                new Aircraft { Id = 9, IdModel = models[9].Id, IdUser = users[0].Id, TailNumber = "N9446D", InstanceType = InstanceType.Real, LastOilHobbs = 1330, LastTransponder = DateTime.Now.AddMonths(-4),
                    Notes = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Iaculis eu non diam phasellus vestibulum lorem." },
            };
            modelBuilder.Entity<Aircraft>().HasData(aircraft);

            var airports = new Airport[]
            {
                new Airport { AirportCode = "KTTN", FacilityName = "Trenton Mercer Airport", Latitude = 41, Longitude = -74, Preferred = true },
                new Airport { AirportCode = "KRDG", FacilityName = "Reading Regional Airport", Latitude = 40, Longitude = -76, Preferred = true },
                new Airport { AirportCode = "KACY", FacilityName = "Atlantic City International Airport", Latitude = 40, Longitude = -74, Preferred = true },
            };
            modelBuilder.Entity<Airport>().HasData(airports);

            var logbookEntries = new List<LogbookEntry>();

            for (int i = 1; i < 200; i++)
            {
                logbookEntries.Add(CreateTestLogbookEntry(i, users[i % 1], aircraft, airports));
            }

            modelBuilder.Entity<LogbookEntry>().HasData(logbookEntries);
        }

        /// <summary>
        /// Creates a dummy logbook entry for seeding the database
        /// </summary>
        /// <param name="user">User persisted in the database</param>
        /// <returns>Dummy logbook entry</returns>
        private LogbookEntry CreateTestLogbookEntry(int id, User user, Aircraft[] airs, Airport[] airports)
        {
            var aircraft = airs.Where(x => x.IdUser == user.Id).ToArray();
            var rand = new Random();

            return new LogbookEntry
            {
                Id = id,
                IdAircraft = aircraft[rand.Next(0, aircraft.Length - 1)].Id,
                IdUser = user.Id,
                Comments = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Iaculis eu non diam phasellus vestibulum lorem.",
                CrossCountry = RandomDuration(rand),
                DualGiven = RandomDuration(rand),
                GroundSim = RandomDuration(rand),
                IMC = RandomDuration(rand),
                MultiEngine = RandomDuration(rand),
                Night = RandomDuration(rand),
                PIC = RandomDuration(rand),
                SIC = RandomDuration(rand),
                Turbine = RandomDuration(rand),
                TotalFlightTime = RandomDuration(rand),
                SimulatedInstrument = RandomDuration(rand),
                NumFullStopLandings = rand.Next(0, 10),
                NumInstrumentApproaches = rand.Next(0, 10),
                NumLandings = rand.Next(0, 10),
                NumNightLandings = rand.Next(0, 10),
                FlightDate = DateTime.Today.AddDays(-rand.Next(0, 720)),
                Route = $"{airports[rand.Next(0, 2)].AirportCode} - {airports[rand.Next(0, 2)].AirportCode}"
            };
        }

        /// <summary>
        /// Returns a random decimal between 0 and 10
        /// </summary>
        /// <param name="rand"></param>
        /// <returns></returns>
        private decimal RandomDuration(Random rand)
        {
            return (decimal)(rand.NextDouble() * rand.Next(0, 5));
        }
    }
}
