
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Nisshi.Models
{
    /// <summary>
    /// Category and class designation for a model of aircraft
    /// </summary>
    public class CategoryClass : BaseEntity
    {
        /// <summary>
        /// Aircraft Category and Class
        /// </summary>
        public string CatClass { get; set; }

        public string Category { get; set; }

        public string Class { get; set; }

        /// <summary>
        /// Secondary category/class for airplanes that are part-time on floats or amphibious
        /// </summary>
        public int AlternateCatClass { get; set; }

        /// <summary>
        /// List of models associated with this category class
        /// </summary>
        [JsonIgnore]
        public virtual List<Model> Models { get; set; }
    }
}
