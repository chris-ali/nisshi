using System.Collections.Generic;

namespace Nisshi.Models
{
    /// <summary>
    /// Representation of a manufacturer of aircraft
    /// </summary>
    public class Manufacturer : BaseEntity
    {
        public string ManufacturerName { get; set; }

        public virtual List<Model> Models { get; set; } = new();
    }
}
