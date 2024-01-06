namespace Domain.Errors;

public static partial class Errors
{
    public static class Auth
    {
        public const string FailedToVerify = "Failed to verify";
        public const string FailedToSignIn = "Failed to sign in";
        public const string FailedToSignUp = "Failed to sign up";
        public const string RoleIsEmpty = "Failed to get the user role";
        public const string EmailIsEmpty = "Failed to get the user email";
    }
}