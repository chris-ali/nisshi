using System;
using Nisshi.Models;
using Xunit;

namespace Nisshi.UnitTests.MaintenanceEntries
{
    /// <summary>
    /// Tests maintenance entry validation in various scenarios
    /// </summary>
    public class MaintenanceEntryTests
    {
        [Fact]
        public void Should_Pass_Validation()
        {
            var maintenance = new MaintenanceEntry {
                DatePerformed = DateTime.Today,
                Duration = 5,
                WorkDescription = "Test work performed",
                MilesPerformed = 104000,
                PerformedBy = "Test user",
                RepairPrice = 100,
                Type = Infrastructure.Enums.MaintenanceType.Preventative,
                Comments = "Test comments",
            };

            var errors = new MaintenanceEntry.MaintenanceEntryValidator().Validate(maintenance);
            Assert.Empty(errors.Errors);

            maintenance.DatePerformed = null;

            errors = new MaintenanceEntry.MaintenanceEntryValidator().Validate(maintenance);
            Assert.Empty(errors.Errors);
        }

        [Fact]
        public void Should_Fail_Validation_Too_Long_Bad_Numerics()
        {
            var maintenance = new MaintenanceEntry {
                DatePerformed = DateTime.Today,
                Duration = -5,
                WorkDescription = @"Test work performed Test work performed Test work performed
                Test work performed Test work performed Test work performed Test work performed
                Test work performed Test work performed Test work performed Test work performed
                Test work performed Test work performed Test work performed Test work performed
                Test work performed Test work performed Test work performed Test work performed
                Test work performed Test work performed Test work performed Test work performed
                Test work performed Test work performed Test work performed Test work performed
                Test work performed Test work performed Test work performed Test work performed
                Test work performed Test work performed Test work performed Test work performed
                Test work performed Test work performed Test work performed Test work performed
                Test work performed Test work performed Test work performed Test work performed
                Test work performed Test work performed Test work performed Test work performed
                Test work performed Test work performed Test work performed Test work performed
                Test work performed Test work performed Test work performed Test work performed ",
                MilesPerformed = -104000,
                PerformedBy = "Test user Test user Test user Test user Test user Test user Test user ",
                RepairPrice = -100,
                Type = Infrastructure.Enums.MaintenanceType.Preventative,
                Comments = @"Test comments Test comments Test comments Test comments Test comments
                Test comments Test comments Test comments Test comments Test comments Test comments
                Test comments Test comments Test comments Test comments Test comments Test comments
                Test comments Test comments Test comments Test comments Test comments Test comments ",
            };

            var errors = new MaintenanceEntry.MaintenanceEntryValidator().Validate(maintenance);
            Assert.Equal(6, errors.Errors.Count);
        }
    }
}
