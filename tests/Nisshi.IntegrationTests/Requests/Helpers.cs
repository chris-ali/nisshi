using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Nisshi.Infrastructure.Enums;
using Nisshi.Models;
using Nisshi.Models.Users;
using Nisshi.Requests.Users;

namespace Nisshi.IntegrationTests.Requests
{
    /// <summary>
    /// Contans static methods to easily generate test objects
    /// </summary>
    public static class Helpers
    {
        public static readonly string TestUserName = "testUser";
        public static readonly string TestEmailAddress = "test@test.com";

        /// <summary>
        /// Registers a test user for tests needing a user object relation
        /// </summary>
        /// <param name="fixture">Testing slice fixture</param>
        /// <returns>A newly registered test user</returns>
        public static async Task<User> RegisterAndGetTestUser(SliceFixture fixture)
        {
            var command = new Register.Command(new Registration 
            {
                Email = TestEmailAddress,
                Username = TestUserName,
                Password = "test123!"
            });

            var user = await fixture.GetNisshiContext().Users
                .SingleOrDefaultAsync(x => x.Username == TestUserName);
            
            if (user == null)
                await fixture.SendAsync(command);

            return await fixture.GetNisshiContext().Users
                .SingleOrDefaultAsync(x => x.Username == TestUserName);
        }
        
        /// <summary>
        /// Creates a test aircraft for testing purposes
        /// </summary>
        /// <param name="fixture">Testing slice fixture</param>
        /// <returns>Test aircraft</returns>
        public static async Task<Aircraft> CreateTestAircraft(SliceFixture fixture, User user) 
        {
            var model = await fixture.GetNisshiContext().Models.FirstOrDefaultAsync();

            return new Aircraft 
            {
                IdModel = model.Id,
                IdUser = user?.Id ?? 0,
                TailNumber = "N31SD",
                InstanceType = InstanceType.Real,
                Last100Hobbs = 10,
                LastOilHobbs = 20,
                LastEngineHobbs = 30,
                LastAltimeter = DateTime.Today.AddDays(-1),
                LastAnnual = DateTime.Today.AddDays(-2),
                LastELT = DateTime.Today.AddDays(-3),
                LastPitotStatic = DateTime.Today.AddDays(-4),
                LastTransponder = DateTime.Today.AddDays(-5),
                LastVOR = DateTime.Today.AddDays(-6),
                RegistrationDue = DateTime.Today.AddDays(-7),
                Notes = "Test notes.",
            };
        }

        /// <summary>
        /// Creates a test manufacturer for testing purposes
        /// </summary>
        /// <param name="fixture">Testing slice fixture</param>
        /// <returns>Test manufacturer</returns>
        public static Manufacturer CreateTestManufacturer()
        {
            return new Manufacturer
            {
                ManufacturerName = "Test Manufacturer"
            };
        }

        /// <summary>
        /// Creates a test model for testing purposes
        /// </summary>
        /// <param name="fixture">Testing slice fixture</param>
        /// <returns>Test model</returns>
        public static async Task<Model> CreateTestModel(SliceFixture fixture)
        {
            var manufacturer = await fixture.GetNisshiContext().Manufacturers
                .FirstOrDefaultAsync();

            var catClass = await fixture.GetNisshiContext().CategoryClasses
                .FirstOrDefaultAsync();

            return new Model
            {
                IdManufacturer = manufacturer.Id,
                IdCategoryClass = catClass.Id,
                Family = "Test Family",
                HasConstantPropeller = true,
                HasFlaps = true,
                IsCertifiedSinglePilot = true,
                IsComplex = true,
                IsHelicopter = true,
                IsHighPerformance = true,
                IsMotorGlider = true,
                IsMultiEngine = true,
                IsSimOnly = true,
                IsTailwheel = true,
                IsTurbine = true,
                ModelName = "Test Model",
                TypeName = "Test Type",
            };
        }

        /// <summary>
        /// Creates a test logbook entry for testing purposes
        /// </summary>
        /// <param name="fixture">Testing slice fixture</param>
        /// <param name="user">User persisted in the database</param>
        /// <returns>Test logbook entry</returns>
        public static async Task<LogbookEntry> CreateTestLogbookEntry(SliceFixture fixture, User user)
        {
            var aircraft = await fixture.GetNisshiContext().Aircraft.FirstOrDefaultAsync();
            var airports = await fixture.GetNisshiContext().Airports.ToArrayAsync();
            var rand = new Random();

            return new LogbookEntry
            {
                IdAircraft = aircraft.Id,
                IdUser = user.Id,
                Comments = "Test comments.",
                CrossCountry = RandomDuration(rand),
                DualGiven = RandomDuration(rand),
                GroundSim = RandomDuration(rand),
                IMC = RandomDuration(rand),
                MultiEngine = RandomDuration(rand),
                Night = RandomDuration(rand),
                PIC = RandomDuration(rand),
                SIC = RandomDuration(rand),
                TotalFlightTime = RandomDuration(rand),
                SimulatedInstrument = RandomDuration(rand),
                NumFullStopLandings = rand.Next(0, 10),
                NumInstrumentApproaches = rand.Next(0, 10),
                NumLandings = rand.Next(0, 10),
                NumNightLandings = rand.Next(0, 10),
                FlightDate = DateTime.Today.AddDays(-rand.Next(0, 180)),
                Route = $"{airports[rand.Next(0,2)].AirportCode} - {airports[rand.Next(0,2)].AirportCode}"
            };
        }

        /// <summary>
        /// Returns a random decimal between 0 and 10
        /// </summary>
        /// <param name="rand"></param>
        /// <returns></returns>
        private static decimal RandomDuration(Random rand)
        {
            return (decimal)(rand.NextDouble() * rand.Next(0, 10));
        }

        /// <summary>
        /// Saves and then immediately gets an object from the database to ensure an
        /// ID is associated with the object
        /// </summary>
        /// <param name="fixture">Test slice fixture</param>
        /// <param name="toSave">Entity to save</param>
        /// <typeparam name="TEntity">Must </typeparam>
        /// <returns>Entity from database with ID</returns>
        public static async Task<TEntity> SaveAndGet<TEntity>(SliceFixture fixture, TEntity toSave)
            where TEntity : class
        {
            fixture.GetNisshiContext().Add<TEntity>(toSave);

            await fixture.GetNisshiContext().SaveChangesAsync();

            return await fixture.GetNisshiContext().Set<TEntity>().LastOrDefaultAsync();
        }
    }
}