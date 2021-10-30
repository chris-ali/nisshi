using System;
using Nisshi.Models;
using Xunit;

namespace Nisshi.UnitTests.Models
{
    /// <summary>
    /// Tests logbook entry validation in various scenarios
    /// </summary>
    public class LogbookEntryTests
    {
        [Fact]
        public void Should_Pass_Validation()
        {
            var duration = 1.5m;
            var count = 1;
            var logbookEntry = new LogbookEntry
            {
                Comments = "Test comments.",
                Route = "KTTN - KABE",
                FlightDate = DateTime.Now,
                CrossCountry = duration,
                DualGiven = duration,
                DualReceived = duration,
                GroundSim = duration,
                HobbsEnd = duration,
                HobbsStart = duration,
                IMC = duration,
                MultiEngine = duration,
                Night = duration,
                PIC = duration,
                SIC = duration,
                SimulatedInstrument = duration,
                TotalFlightTime = duration,
                NumFullStopLandings = count,
                NumInstrumentApproaches = count,
                NumLandings = count,
                NumNightLandings = count,
            };

            var errors = new LogbookEntry.LogbookEntryValidator().Validate(logbookEntry);
            Assert.Empty(errors.Errors);
        }

        [Fact]
        public void Should_Fail_Validation_Too_Long_Empty()
        {
            var duration = 1.5m;
            var count = 1;
            var comments = @"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam ac tortor mauris. Curabitur condimentum leo in accumsan laoreet. Donec at blandit risus. In lectus nisi, hendrerit a urna eu, porta fermentum turpis. Duis consequat nisi dui, varius placerat dui gravida et. Nam sagittis velit tellus. Cras non metus sit amet leo faucibus rutrum. Nam at mi orci. Maecenas feugiat nisi odio, ac consequat orci aliquet at. In bibendum odio id nibh ornare, id convallis turpis euismod. Curabitur vitae nunc lorem. Nunc accumsan lectus at nisl suscipit lacinia. Vivamus diam justo, lacinia eu tempus sed, efficitur sed neque.

Quisque cursus tellus posuere, lacinia enim quis, imperdiet ex. Nulla facilisi. Sed tincidunt tincidunt placerat. Fusce vestibulum ipsum pellentesque nisl scelerisque, vitae fringilla justo varius. Sed at luctus metus, id tincidunt est. Mauris egestas mauris at pretium elementum. Nulla pellentesque nulla eget mauris fermentum posuere. Ut facilisis vehicula egestas.

Ut rutrum lectus augue, ut feugiat lectus vulputate in. Vestibulum ultricies sollicitudin imperdiet. Aliquam vitae vehicula neque, laoreet interdum tortor. Phasellus facilisis, libero eu imperdiet tempus, orci odio porta justo, eget dapibus nunc ipsum sit amet nulla. Maecenas vulputate elit sed lacus laoreet, id fringilla velit porttitor. Ut dignissim aliquam nisl, et sodales neque rutrum id. Sed ac eros pharetra, tincidunt ex in, euismod nisl. Donec commodo orci vitae euismod porta. Morbi vel mi quam. Pellentesque at enim ipsum. Maecenas a ullamcorper massa. Mauris imperdiet purus ante, sit amet ultrices ex porttitor ac. Donec pretium, est in pharetra tempor, nisi enim lacinia quam, sit amet porta velit sem vitae neque. Ut ligula orci, convallis id consequat viverra, efficitur sed nisl. Nulla sollicitudin arcu lacus, nec efficitur augue ornare in. Sed at eleifend mi.

Nullam non pretium arcu. Ut efficitur ultricies enim, eu euismod metus mollis eget. Mauris at tortor neque. Nunc eleifend, urna vel fermentum convallis, tortor ligula scelerisque tortor, eget congue dui quam ornare augue. Orci varius natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. In facilisis turpis eros, vitae lobortis urna tempus sed. Maecenas vulputate risus urna, at feugiat neque consequat ut.

Proin at mi ac ligula placerat vestibulum eu at felis. Proin tempus nibh diam, at ullamcorper lorem ullamcorper ac. Aliquam facilisis sem a ultricies pulvinar. Vivamus pellentesque pretium lorem nec scelerisque. Duis finibus convallis mauris in pellentesque. Etiam faucibus velit ut dolor auctor, suscipit pharetra nisl consequat. Aliquam bibendum augue in vehicula sodales. Nullam tincidunt augue a vestibulum tristique. Nulla vel diam felis. Pellentesque pellentesque nisl eu tempus pellentesque. Cras nisl nibh, vehicula eget orci bibendum, lacinia egestas diam. Maecenas quis felis ac purus blandit commodo. Nulla ultricies gravida dui, vel interdum quam congue a. Aliquam tincidunt purus ut nunc condimentum, nec sollicitudin turpis dapibus. Duis enim enim, consequat nec hendrerit venenatis, pulvinar sit amet dolor. ";
            var route = "KTTN - KABE - KTTN - KABE - KTTN - KABE - KTTN - KABE - KTTN - KABE - KTTN - KABE - KTTN - KABE - KTTN - KABE - KTTN - KABE - KTTN - KABE - KTTN - KABE - KTTN - KABE - KTTN - KABE - KTTN - KABE - KTTN - KABE - KTTN - KABE";
            var logbookEntry = GenerateEntry(duration, count, route, comments);

            var errors = new LogbookEntry.LogbookEntryValidator().Validate(logbookEntry);
            Assert.Equal(2, errors.Errors.Count);

            logbookEntry.Route = "";

            errors = new LogbookEntry.LogbookEntryValidator().Validate(logbookEntry);
            Assert.Equal(2, errors.Errors.Count);
        }

        [Fact]
        public void Should_Fail_Validation_Bad_Numerics()
        {
            var duration = -1.5m;
            var count = -1;
            var logbookEntry = GenerateEntry(duration, count, "Test comments.", "KTTN - KABE");

            var errors = new LogbookEntry.LogbookEntryValidator().Validate(logbookEntry);
            Assert.Equal(17, errors.Errors.Count);

            duration = 25m;
            logbookEntry = GenerateEntry(duration, count, "Test comments.", "KTTN - KABE");

            errors = new LogbookEntry.LogbookEntryValidator().Validate(logbookEntry);
            Assert.Equal(17, errors.Errors.Count);
        }

        private static LogbookEntry GenerateEntry(decimal duration, int count, string route, string comments) => new LogbookEntry
        {
            Comments = comments,
            Route = route,
            FlightDate = DateTime.Now,
            CrossCountry = duration,
            DualGiven = duration,
            DualReceived = duration,
            GroundSim = duration,
            HobbsEnd = duration,
            HobbsStart = duration,
            IMC = duration,
            MultiEngine = duration,
            Night = duration,
            PIC = duration,
            SIC = duration,
            SimulatedInstrument = duration,
            TotalFlightTime = duration,
            NumFullStopLandings = count,
            NumInstrumentApproaches = count,
            NumLandings = count,
            NumNightLandings = count,
        };
    }
}
