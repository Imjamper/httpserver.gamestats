using System;

namespace GL.HttpServer.Types
{
    public abstract class KnownTypeParser<T> : KnownTypeParser, IKnownTypeParser<T>
    {
        public override Type Type
        {
            get { return typeof(T); }
        }

        public abstract T Parse(string input);

        public override object ParseObject(string input)
        {
            return Parse(input);
        }
    }

    public abstract class KnownTypeParser : IKnownTypeParser
    {
        public abstract Type Type { get; }

        public abstract bool CanParse(string input);

        public abstract object ParseObject(string input);
    }
}