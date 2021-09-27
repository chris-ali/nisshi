using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Nisshi.Models
{
    public class Manufacturer : BaseEntity
    {
        public string ManufacturerName { get; set; }

        public virtual List<Model> Models { get; set; } = new();
    }
}