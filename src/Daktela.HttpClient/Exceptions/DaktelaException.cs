using System;

namespace Daktela.HttpClient.Exceptions;

public abstract class DaktelaException : Exception
{
    protected DaktelaException()
    {
    }

    protected DaktelaException(string message) : base(message)
    {
    }
}
