namespace GL.HttpServer.Types
{
    public class EndpointParser : KnownTypeParser<Endpoint>
    {
        public override bool CanParse(string input)
        {
            Endpoint endpoint;
            if (Endpoint.TryParse(input, out endpoint))
                return true;
            return false;
        }

        public override Endpoint Parse(string input)
        {
            Endpoint endpoint;
            if (Endpoint.TryParse(input, out endpoint))
                return endpoint;
            return null;
        }
    }
}