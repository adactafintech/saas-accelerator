using System.Runtime.Serialization;

namespace Marketplace.SaaS.Accelerator.Services.Models;
public enum PipelineOperation
{
    [EnumMember(Value = "provision")]
    Provision,

    [EnumMember(Value = "destroy")]
    Destroy,
}
