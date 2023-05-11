using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Marketplace.SaaS.Accelerator.DataAccess.Contracts;
using Marketplace.SaaS.Accelerator.Services.Configurations;
using Marketplace.SaaS.Accelerator.Services.Exceptions;
using Marketplace.SaaS.Accelerator.Services.Models;
using Marketplace.SaaS.Accelerator.Services.Services;
using Marketplace.SaaS.Accelerator.Services.StatusHandlers;
using Marketplace.SaaS.Accelerator.Services.WebHook;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Marketplace.SaaS.Accelerator.AdminSite.Controllers.WebHook;

/// <summary>
/// Azure Web hook.
/// </summary>
/// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
[Route("api/[controller]")]
[ApiController]
public class CiCdWebhookController : ControllerBase
{
    /// <summary>
    /// The application log repository.
    /// </summary>
    private readonly IApplicationLogRepository applicationLogRepository;

    /// <summary>
    /// The subscriptions repository.
    /// </summary>
    private readonly ISubscriptionsRepository subscriptionsRepository;

    /// <summary>
    /// The current configuration
    /// </summary>
    private readonly SaaSApiClientConfiguration configuration;

    /// <summary>
    /// The plan repository.
    /// </summary>
    private readonly IPlansRepository planRepository;

    /// <summary>
    /// The subscriptions log repository.
    /// </summary>
    private readonly ISubscriptionLogRepository subscriptionsLogRepository;

    /// <summary>
    /// The application log service.
    /// </summary>
    private readonly ApplicationLogService applicationLogService;

    /// <summary>
    /// The subscription service.
    /// </summary>
    private readonly SubscriptionService subscriptionService;

    /// <summary>
    /// The web hook processor.
    /// </summary>
    private readonly ICiCdWebhookProcessor webhookProcessor;

    /// <summary>
    /// The users repository.
    /// </summary>
    private readonly IUsersRepository userRepository;

    private readonly ILoggerFactory loggerFactory;

    /// <summary>
    /// Initializes a new instance of the <see cref="AzureWebhookController"/> class.
    /// </summary>
    /// <param name="applicationLogRepository">The application log repository.</param>
    /// <param name="webhookProcessor">The Web hook log repository.</param>
    /// <param name="subscriptionsLogRepository">The subscriptions log repository.</param>
    /// <param name="planRepository">The plan repository.</param>
    /// <param name="subscriptionsRepository">The subscriptions repository.</param>
    /// <param name="userRepository">The user repository.</param>
    /// <param name="loggerFactory">The logger factory.</param>
    /// <param name="configuration">The SaaSApiClientConfiguration from ENV</param>
    public CiCdWebhookController(IApplicationLogRepository applicationLogRepository, ICiCdWebhookProcessor webhookProcessor, ISubscriptionLogRepository subscriptionsLogRepository, IPlansRepository planRepository, ISubscriptionsRepository subscriptionsRepository, IUsersRepository userRepository, ILoggerFactory loggerFactory, SaaSApiClientConfiguration configuration)
    {
        this.applicationLogRepository = applicationLogRepository;
        this.webhookProcessor = webhookProcessor;
        this.subscriptionsRepository = subscriptionsRepository;
        this.userRepository = userRepository;
        this.configuration = configuration;
        this.planRepository = planRepository;
        this.subscriptionsLogRepository = subscriptionsLogRepository;
        this.applicationLogService = new ApplicationLogService(this.applicationLogRepository);
        this.subscriptionService = new SubscriptionService(this.subscriptionsRepository, this.planRepository);
        this.loggerFactory = loggerFactory;
    }

    /// <summary>
    /// Posts the specified request.
    /// </summary>
    /// <param name="request">The request.</param>
    // [AllowAnonymous]
    public async Task<IActionResult> Post([FromHeader(Name = "X-Gitlab-Token")] string token, [FromBody] Pipeline pipeline) // WebhookPayload request)
    {
        try
        {

            await this.applicationLogService.AddApplicationLog("The CI/CD Webhook Triggered.").ConfigureAwait(false);

            if(string.IsNullOrEmpty(token) || token != configuration.ProvisionWebHookToken)
            {
                throw new MarketplaceException("Unauthorized request to CI/CID webhook.");
            }

            if(string.IsNullOrEmpty(pipeline.ObjectKind) || pipeline.ObjectKind != "pipeline")
            {
                throw new MarketplaceException("Incorrect object kind.");
            }

            var subscriptionId = pipeline.ObjectAttributes.Variables.FirstOrDefault(v => v.Key == "CLIENT_REF")?.Value;
            if(subscriptionId == null)
            {
                throw new MarketplaceException("Pipeline was not triggered by this application.");
            }

            var pipelineOperation = pipeline.ObjectAttributes.Variables.FirstOrDefault(v => v.Key == "OPERATION")?.Value;
            if (pipelineOperation == null || (pipelineOperation != "provision" && pipelineOperation != "destroy" ))
            {
                throw new MarketplaceException("Unknown pipeline operation.");
            }

            if (pipeline.ObjectAttributes?.Status == PipelineStatus.Success || 
                pipeline.ObjectAttributes?.Status == PipelineStatus.Failed || 
                pipeline.ObjectAttributes?.Status == PipelineStatus.Cancelled || 
                pipeline.ObjectAttributes?.Status == PipelineStatus.Skipped)
            {
                await this.webhookProcessor.ProcessWebhookNotificationAsync(new Guid(subscriptionId), Enum.Parse<PipelineOperation>(pipelineOperation, true), pipeline.ObjectAttributes.Status).ConfigureAwait(false);
            }
            else
            {
                await this.applicationLogService.AddApplicationLog($"Received pipeline event for status {pipeline.ObjectAttributes?.Status} that is not processed").ConfigureAwait(false);
            }

            return Ok();

        }
        catch (MarketplaceException ex)
        {
            await this.applicationLogService.AddApplicationLog(
                    $"An error occurred while attempting to process a webhook notification: [{ex.Message}].")
                .ConfigureAwait(false);
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            await this.applicationLogService.AddApplicationLog(
                    $"An error occurred while attempting to process a webhook notification: [{ex.Message}].")
                .ConfigureAwait(false);
            return StatusCode(500);
        }
    }
}