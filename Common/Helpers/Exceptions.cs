namespace Common.Helpers
{
    public class EntityNotFoundException(string? message) : Exception(message)
    {
    }
}
