using System.Runtime.Serialization;

namespace DemoWebshopApi.Common.CustomExceptions
{
    [Serializable]
    public class IdentityResultException : Exception
    {
        public IdentityResultException()
        {
        }

        public IdentityResultException(string? message) : base(message)
        {
        }

        public IdentityResultException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected IdentityResultException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
