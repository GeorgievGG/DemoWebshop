using System.Runtime.Serialization;

namespace DemoWebshopApi.Services.Services
{
    [Serializable]
    public class UserAlreadyAnAdminException : Exception
    {
        public UserAlreadyAnAdminException()
        {
        }

        public UserAlreadyAnAdminException(string? message) : base(message)
        {
        }

        public UserAlreadyAnAdminException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected UserAlreadyAnAdminException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}