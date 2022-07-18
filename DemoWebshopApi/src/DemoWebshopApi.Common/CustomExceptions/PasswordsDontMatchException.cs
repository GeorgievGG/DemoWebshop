using System.Runtime.Serialization;

namespace DemoWebshopApi.Services.Services
{
    [Serializable]
    public class PasswordsDontMatchException : Exception
    {
        private object passwordsDontMatch;

        public PasswordsDontMatchException()
        {
        }

        public PasswordsDontMatchException(object passwordsDontMatch)
        {
            this.passwordsDontMatch = passwordsDontMatch;
        }

        public PasswordsDontMatchException(string? message) : base(message)
        {
        }

        public PasswordsDontMatchException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected PasswordsDontMatchException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}