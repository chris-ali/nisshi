using System;

namespace Nisshi.Models
{
    /// <summary>
    /// Base entity with fundamental properties shared by 
    /// all models
    /// </summary>
    public class BaseEntity
    {
        /// <summary>
        /// Primary key of the entity
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Date entity was first created
        /// </summary>
        /// <value></value>
        public DateTime? DateCreated { get; set; }

        /// <summary>
        /// Date entity was last updated
        /// </summary>
        public DateTime? DateUpdated { get; set; }
    }
}
