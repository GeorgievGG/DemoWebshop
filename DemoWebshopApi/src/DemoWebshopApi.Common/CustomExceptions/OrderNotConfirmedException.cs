using System.Runtime.Serialization;

namespace DemoWebshopApi.Common.CustomExceptions
{
    [Serializable]
    public class OrderNotConfirmedException : Exception
    {
        private object orderNotConfirmed;

        public OrderNotConfirmedException()
        {
        }

        public OrderNotConfirmedException(object orderNotConfirmed)
        {
            this.orderNotConfirmed = orderNotConfirmed;
        }

        public OrderNotConfirmedException(string? message) : base(message)
        {
        }

        public OrderNotConfirmedException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected OrderNotConfirmedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}