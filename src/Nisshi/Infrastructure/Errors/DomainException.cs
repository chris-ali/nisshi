using System;

namespace Nisshi.Infrastructure.Errors
{
    /// <summary>
    /// Exception encountered when manipulating objects in the database
    /// </summary>
    public class DomainException : Exception
    {
        /// <summary>
        /// The type of entity that generated the exception
        /// </summary>
        public Type EntityType { get; }
        /// <summary>
        /// Message code 
        /// </summary>
        public Message MessageCode { get; }

        public DomainException(Type entityType, Message message)
        {
            EntityType = entityType;
            MessageCode = message;
        }
    }
}
