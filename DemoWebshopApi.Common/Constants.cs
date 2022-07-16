namespace DemoWebshopApi.Common
{
    public static class Constants
    {
        public const string InvalidLength = "The field {0} must have a minimum length of {1}";
        public const string InvalidEmail = "The email must be valid";
        public const string UsernameAreadyInUse = "The username already exists";
        public const string EmailAreadyInUse = "The email already exists";
        public const string NotFound = "{0} not found";
        public const string PasswordsDontMatch = "Provided passwords don't match!";
        public const string UserIsAdmin = "User is already an admin";
        public const string ObjectHasDependencies = "The {0} can't be deleted because he has a {1}!";
        public const string ProductAlreadyExists = "Such products already exists";
        public const string ValueIsNotValid = "{0} is not a valid value for {1}";
        public const string OrderNotConfirmed = "Can't set delivery date for unconfirmed order";
        public const string DuplicatedOrderLines = "Can't create an order with duplicated product lines!";
    }
}