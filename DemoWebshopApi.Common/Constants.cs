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
        public const string UserHasDependenciesOrder = "The user can't be deleted because he has an order!";
        public const string UserHasDependenciesBasket = "The user can't be deleted because he has an active basket!";
    }
}