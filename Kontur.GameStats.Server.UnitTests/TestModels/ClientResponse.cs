namespace Kontur.GameStats.Server.UnitTests.TestModels
{
    public class ClientResponse
    {
        public ClientResponse()
        {
        }

        public string JsonString { get; set; }
        public string ErrorMessage { get; set; }
        public string StatusCode { get; set; }
    }
}
