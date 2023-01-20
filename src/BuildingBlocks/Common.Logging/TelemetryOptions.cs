namespace Common.Logging
{
    public class TelemetryOptions
    {
        public string ServiceName { get; set; } = default!;
        public string JaegerEndpoint { get; set; } = default!;
        public string ZipkinEndpoint { get; set; } = default!;
    }
}
