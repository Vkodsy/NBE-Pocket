namespace UserAuthApi.Services;
public class DuplicateEmailException : Exception
{
    public DuplicateEmailException()
    : base("The registration could not be completed.")
    {
    }
}
