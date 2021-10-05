namespace Nisshi.Infrastructure.Errors
{
    public class Messages
    {
        public const string ALREADY_EXISTS = "already exists in the system. Please try a different value.";
        public const string DOES_NOT_EXIST = "does not exist in the system.";
        public const string NOT_NULL = "data must be present for this request and cannot be null.";
        public const string NOT_LOGGED_IN = "Logged in user was not found! Please try to authenticate again.";
        public const string INVALID_CREDENTIALS = "Invalid Username/Password! Please try again.";
    }
}