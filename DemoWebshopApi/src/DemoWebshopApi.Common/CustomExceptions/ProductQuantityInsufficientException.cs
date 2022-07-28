using System.Runtime.Serialization;

namespace DemoWebshopApi.Common.CustomExceptions
{
    [Serializable]
    public class ProductQuantityInsufficientException : Exception
    {
        private object duplicatedOrderLines;

        public ProductQuantityInsufficientException()
        {
        }

        public ProductQuantityInsufficientException(object duplicatedOrderLines)
        {
            this.duplicatedOrderLines = duplicatedOrderLines;
        }

        public ProductQuantityInsufficientException(string? message) : base(message)
        {
        }

        public ProductQuantityInsufficientException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected ProductQuantityInsufficientException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
