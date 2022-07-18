using System.Runtime.Serialization;

namespace DemoWebshopApi.Services.Services
{
    [Serializable]
    public class ProductAlreadyExistsException : Exception
    {
        private object productAlreadyExists;

        public ProductAlreadyExistsException()
        {
        }

        public ProductAlreadyExistsException(object productAlreadyExists)
        {
            this.productAlreadyExists = productAlreadyExists;
        }

        public ProductAlreadyExistsException(string? message) : base(message)
        {
        }

        public ProductAlreadyExistsException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected ProductAlreadyExistsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}