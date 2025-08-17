namespace RealEstate.Domain.Exceptions
{
    public class UserAlreadyRegisteredException : Exception
    {
        public UserAlreadyRegisteredException(string? message) : base(message)
        {
        }
    }
}
