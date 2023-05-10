using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Marketplace.SaaS.Accelerator.Services.Models;
public class PipelineAttributes
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("iid")]
    public int Iid { get; set; }

    [JsonPropertyName("ref")]
    public string Ref { get; set; }

    [JsonPropertyName("status")]
    public PipelineStatus Status { get; set; }

    [JsonPropertyName("variables")]
    public List<PipelineVariable> Variables { get; set; }
}
