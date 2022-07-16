using System.Runtime.Serialization;

namespace DemoWebshopApi.Services.Services
{
    [Serializable]
    public class UserNotExistException : Exception
    {
        private object userDoesNotExist;

        public UserNotExistException()
        {
        }

        public UserNotExistException(object userDoesNotExist)
        {
            this.userDoesNotExist = userDoesNotExist;
        }

        public UserNotExistException(string? message) : base(message)
        {
        }

        public UserNotExistException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected UserNotExistException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}