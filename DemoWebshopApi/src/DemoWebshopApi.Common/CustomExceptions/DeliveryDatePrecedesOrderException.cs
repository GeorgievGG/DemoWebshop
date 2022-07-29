using System.Runtime.Serialization;

namespace DemoWebshopApi.Common.CustomExceptions
{
    [Serializable]
    public class DeliveryDatePrecedesOrderException : Exception
    {
        private object duplicatedOrderLines;

        public DeliveryDatePrecedesOrderException()
        {
        }

        public DeliveryDatePrecedesOrderException(object duplicatedOrderLines)
        {
            this.duplicatedOrderLines = duplicatedOrderLines;
        }

        public DeliveryDatePrecedesOrderException(string? message) : base(message)
        {
        }

        public DeliveryDatePrecedesOrderException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected DeliveryDatePrecedesOrderException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
