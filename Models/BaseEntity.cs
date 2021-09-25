using System;
using System.Text.Json.Serialization;

namespace Nisshi.Models
{
    public class BaseEntity
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("createdDate")]
        public DateTime DateCreated { get; set; }

        [JsonPropertyName("updatedDate")]
        public DateTime DateUpdated { get; set; }
    }
}