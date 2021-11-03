using System.Collections.Generic;
using FluentValidation;

namespace Nisshi.Models
{
    /// <summary>
    /// Representation of a manufacturer of aircraft
    /// </summary>
    public class Manufacturer : BaseEntity
    {
        public string ManufacturerName { get; set; }

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
