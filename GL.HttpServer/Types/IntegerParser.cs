namespace GL.HttpServer.Types
{
    public class IntegerParser : KnownTypeParser<int?>
    {
        public override bool CanParse(string input)
        {
            var value = 0;
            return int.TryParse(input, out value);
        }

        public override int? Parse(string input)
        {
            var value = 0;
            if (int.TryParse(input, out value))
                return value;
            return null;
        }
    }
}