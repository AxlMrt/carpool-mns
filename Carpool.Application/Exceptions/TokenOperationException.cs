public class TokenOperationException : Exception
{
    public TokenOperationException() { }

    public TokenOperationException(string message) : base(message) { }

    public TokenOperationException(string message, Exception innerException) : base(message, innerException) { }
}