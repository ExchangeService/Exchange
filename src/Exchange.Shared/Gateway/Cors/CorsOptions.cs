namespace Exchange.Shared.Gateway.Cors
{
    public class CorsOptions
    {
        public string[]? Domains { get; set; }

        public bool Enabled { get; set; }

        public string[]? ExposedHeaders { get; set; }

        public string[]? Headers { get; set; }

        public string[]? Methods { get; set; }
    }
}