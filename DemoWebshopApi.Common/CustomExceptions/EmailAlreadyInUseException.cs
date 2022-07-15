using System.Runtime.Serialization;

namespace DemoWebshopApi.Services.Services
{
    [Serializable]
    public class EmailAlreadyInUseException : Exception
    {
        public EmailAlreadyInUseException()
        {
        }

        public EmailAlreadyInUseException(string? message) : base(message)
        {
        }

        public EmailAlreadyInUseException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected EmailAlreadyInUseException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}