using System;
using System.Threading.Tasks;
using Marketplace.SaaS.Accelerator.Services.Configurations;
using Marketplace.SaaS.Accelerator.Services.Models;

namespace Marketplace.SaaS.Accelerator.Services.WebHook;

/// <summary>
/// CI/CD Web hook Processor Interface
/// </summary>
public interface ICiCdWebhookProcessor
{
    /// <summary>
    /// Processes the Web hook notification asynchronous.
    /// </summary>
    /// <param name="subscriptionId">Subscription ID.</param>
    /// <param name="status">Status of the pipeline</param>
    /// <returns>Processes the CI/CD Web hook notification</returns>
    Task ProcessWebhookNotificationAsync(Guid subscriptionId, PipelineStatus status);
}