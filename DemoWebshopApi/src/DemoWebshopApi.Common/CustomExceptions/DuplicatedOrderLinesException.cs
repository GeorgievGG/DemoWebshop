using System.Runtime.Serialization;

namespace DemoWebshopApi.Services.Services
{
    [Serializable]
    public class DuplicatedOrderLinesException : Exception
    {
        private object duplicatedOrderLines;

        public DuplicatedOrderLinesException()
        {
        }

        public DuplicatedOrderLinesException(object duplicatedOrderLines)
        {
            this.duplicatedOrderLines = duplicatedOrderLines;
        }

        public DuplicatedOrderLinesException(string? message) : base(message)
        {
        }

        public DuplicatedOrderLinesException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected DuplicatedOrderLinesException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}