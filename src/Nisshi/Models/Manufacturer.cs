using System.Collections.Generic;
using System.Text.Json.Serialization;
using FluentValidation;

namespace Nisshi.Models
{
    /// <summary>
    /// Representation of a manufacturer of aircraft
    /// </summary>
    public class Manufacturer : BaseEntity
    {
        public string ManufacturerName { get; set; }

        [JsonIgnore]
        public virtual List<Model> Models { get; set; } = new();

        public class ManufacturerValidator : AbstractValidator<Manufacturer>
        {
            public ManufacturerValidator()
            {
                RuleFor(x => x.ManufacturerName).NotEmpty().WithMessage("NotEmpty")
                    .MaximumLength(45).WithMessage("Length45");
            }
        }
    }
}
