using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Marketplace.SaaS.Accelerator.Services.Models;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum PipelineStatus
{
    [EnumMember(Value = "created")]
    Created,

    [EnumMember(Value = "waiting_for_resource")]
    WaitingForResponse,

    [EnumMember(Value = "preparing")]
    Preparing,

    [EnumMember(Value = "pending")]
    Pending,

    [EnumMember(Value = "running")]
    Running,

    /// (When the pipeline provisioned tenant)
    /// <summary>
    /// The success
    /// </summary>
    [EnumMember(Value = "success")]
    Success,

    /// (When the pipeline failed to provision tenant)
    /// <summary>
    /// The failure
    /// </summary>
    [EnumMember(Value = "failed")]
    Failed,

    /// (When the pipeline was cancelled)
    /// <summary>
    /// The cancelation
    /// </summary>
    [EnumMember(Value = "canceled")]
    Cancelled,

    /// (When the pipeline was skipped)
    /// <summary>
    /// The skip
    /// </summary>
    [EnumMember(Value = "skipped")]
    Skipped,

    [EnumMember(Value = "manual")]
    Manual,

    [EnumMember(Value = "scheduled")]
    Scheduled,
}