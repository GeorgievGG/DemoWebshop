using System.Runtime.Serialization;

namespace DemoWebshopApi.Services.Services
{
    [Serializable]
    public class UserHasDependenciesException : Exception
    {
        public UserHasDependenciesException()
        {
        }

        public UserHasDependenciesException(string? message) : base(message)
        {
        }

        public UserHasDependenciesException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected UserHasDependenciesException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}