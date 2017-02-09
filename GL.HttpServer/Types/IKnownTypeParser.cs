using System;

namespace GL.HttpServer.Types
{
    public interface IKnownTypeParser<T>
    {
        T Parse(string input);
    }

    public interface IKnownTypeParser
    {
        Type Type { get; }
        bool CanParse(string input);
        object ParseObject(string input);
    }
}