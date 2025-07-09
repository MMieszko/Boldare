namespace Application.Utils;

public record Error(string Message, Exception? Exception)
{
    public Error(string message) : this(message, null) { }
    public Error(Exception exception) : this(exception.Message, exception) { }

    public override string ToString() => Exception switch
    {
        null => Message,
        _ => $"{Message} {Environment.NewLine} {Exception}"
    };
}