using System.Runtime.Serialization;

namespace DemoWebshopApi.Common.CustomExceptions
{
    [Serializable]
    public class InvalidLengthException : Exception
    {
        public InvalidLengthException()
        {
        }

        public InvalidLengthException(string? message) : base(message)
        {
        }

        public InvalidLengthException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected InvalidLengthException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}