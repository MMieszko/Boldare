namespace Application.Utils;

public record ValidationError(string Message, string PropertyName) : Error(Message);