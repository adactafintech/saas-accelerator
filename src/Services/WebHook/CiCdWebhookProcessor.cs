using System;
using System.Threading.Tasks;
using Marketplace.SaaS.Accelerator.Services.Configurations;
using Marketplace.SaaS.Accelerator.Services.Contracts;
using Marketplace.SaaS.Accelerator.Services.Models;

namespace Marketplace.SaaS.Accelerator.Services.WebHook;

/// <summary>
/// The CI/CD webhook processor.
/// </summary>
/// <seealso cref="Microsoft.Marketplace.SaasKit.WebHook.ICiCdWebhookProcessor" />
public class CiCdWebhookProcessor : ICiCdWebhookProcessor
{
    /// <summary>
    /// The webhook handler.
    /// </summary>
    private readonly ICiCdWebhookHandler webhookHandler;

    /// <summary>
    /// Defines the _apiClient.
    /// </summary>
    private IFulfillmentApiService apiClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="WebhookProcessor"/> class.
    /// </summary>
    /// <param name="apiClient">The API client.</param>
    /// <param name="webhookHandler">The webhook handler.</param>
    public CiCdWebhookProcessor(IFulfillmentApiService apiClient, ICiCdWebhookHandler webhookHandler)
    {
        this.apiClient = apiClient;
        this.webhookHandler = webhookHandler;
    }

    /// <summary>
    /// Processes the webhook notification asynchronous.
    /// </summary>
    /// <param name="payload">The payload.</param>
    /// <param name="config">Current environmental configuration</param>
    /// <returns> Notification.</returns>
    public async Task ProcessWebhookNotificationAsync(Guid subscriptionId, PipelineStatus status)
    {
        switch (status)
        {
            case PipelineStatus.Failed:
            case PipelineStatus.Cancelled:
            case PipelineStatus.Skipped:
                await this.webhookHandler.ProvisioningFailureAsync(subscriptionId, status).ConfigureAwait(false);
                break;

            default:
                await this.webhookHandler.ProvisioningSuccessAsync(subscriptionId).ConfigureAwait(false);
                break;
        }
    }
}