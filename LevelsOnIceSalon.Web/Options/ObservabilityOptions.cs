using System.ComponentModel.DataAnnotations;

namespace LevelsOnIceSalon.Web.Options;

public sealed class ObservabilityOptions
{
    public const string SectionName = "Observability";

    [Required]
    public string ServiceName { get; set; } = "LevelsOnIceSalon.Web";

    public string? ServiceVersion { get; set; }

    public OtlpOptions Otlp { get; set; } = new();

    public ConsoleExporterOptions Console { get; set; } = new();

    public sealed class OtlpOptions
    {
        public string? Endpoint { get; set; }

        [RegularExpression("^(grpc|http/protobuf)$", ErrorMessage = "Observability:Otlp:Protocol must be 'grpc' or 'http/protobuf'.")]
        public string Protocol { get; set; } = "grpc";
    }

    public sealed class ConsoleExporterOptions
    {
        public bool TracingEnabled { get; set; }

        public bool MetricsEnabled { get; set; }
    }
}
