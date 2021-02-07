namespace Exchange.Shared.Exceptions
{
    public class CustomerUnauthenticatedException : DomainException
    {
        public CustomerUnauthenticatedException()
            : base("User not authorized")
        {
        }

        public override string Code => "user_not_authorized";

        public override string TranslationKey => "User_Not_authorized";
    }
}