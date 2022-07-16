using System.Runtime.Serialization;

namespace DemoWebshopApi.Services.Services
{
    [Serializable]
    public class ValueInvalidException : Exception
    {
        public ValueInvalidException()
        {
        }

        public ValueInvalidException(string? message) : base(message)
        {
        }

        public ValueInvalidException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected ValueInvalidException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}