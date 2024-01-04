public class JwtGenerationException : Exception
{
    public JwtGenerationException() { }

    public JwtGenerationException(string message) : base(message) { }

    public JwtGenerationException(string message, Exception innerException) : base(message, innerException) { }
}