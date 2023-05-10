using System.Text.Json.Serialization;

namespace Marketplace.SaaS.Accelerator.Services.Models;
public class Pipeline
{
    [JsonPropertyName("object_kind")]
    public string ObjectKind { get; set; }

    [JsonPropertyName("object_attributes")]
    public PipelineAttributes ObjectAttributes { get; set; }

    [JsonPropertyName("project")]
    public PipelineProject Project { get; set; }

    [JsonPropertyName("source_pipeline")]
    public PipelineSource PipelineSource { get; set; }
}
