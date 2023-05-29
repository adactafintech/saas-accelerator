using System;
using System.Text.Json;
using System.Threading.Tasks;
using Marketplace.SaaS.Accelerator.DataAccess.Contracts;
using Marketplace.SaaS.Accelerator.DataAccess.Entities;
using Marketplace.SaaS.Accelerator.Services.Contracts;
using Marketplace.SaaS.Accelerator.Services.Exceptions;
using Marketplace.SaaS.Accelerator.Services.Models;
using Marketplace.SaaS.Accelerator.Services.Services;
using Marketplace.SaaS.Accelerator.Services.StatusHandlers;
using Marketplace.SaaS.Accelerator.Services.WebHook;
using Microsoft.Extensions.Logging;

namespace Marketplace.SaaS.Accelerator.AdminSite.WebHook;

/// <summary>
/// Handler For the WebHook Actions.
/// </summary>
/// <seealso cref="Microsoft.Marketplace.SaasKit.WebHook.IWebhookHandler" />
public class CiCdWebHookHandler : ICiCdWebhookHandler
{
    ///// <summary>
    ///// The application log repository.
    ///// </summary>
    //private readonly IApplicationLogRepository applicationLogRepository;

    /// <summary>
    /// The subscriptions repository.
    /// </summary>
    private readonly ISubscriptionsRepository subscriptionsRepository;

    ///// <summary>
    ///// The plan repository.
    ///// </summary>
    //private readonly IPlansRepository planRepository;

    ///// <summary>
    ///// The subscription service.
    ///// </summary>
    //private readonly SubscriptionService subscriptionService;

    ///// <summary>
    ///// The application log service.
    ///// </summary>
    //private readonly ApplicationLogService applicationLogService;

    ///// <summary>
    ///// The application configuration repository.
    ///// </summary>
    //private readonly IApplicationConfigRepository applicationConfigRepository;

    ///// <summary>
    ///// The email template repository.
    ///// </summary>
    //private readonly IEmailTemplateRepository emailTemplateRepository;

    ///// <summary>
    ///// The plan events mapping repository.
    ///// </summary>
    //private readonly IPlanEventsMappingRepository planEventsMappingRepository;

    ///// <summary>
    ///// The events repository.
    ///// </summary>
    //private readonly IEventsRepository eventsRepository;

    /// <summary>
    /// The fulfill API client.
    /// </summary>
    private readonly IFulfillmentApiService fulfillApiService;

    /// <summary>
    /// The users repository.
    /// </summary>
    private readonly IUsersRepository usersRepository;

    /// <summary>
    /// The subscriptions log repository.
    /// </summary>
    private readonly ISubscriptionLogRepository subscriptionsLogRepository;

    //private readonly ISubscriptionStatusHandler notificationStatusHandlers;

    /// <summary>
    /// The logger.
    /// </summary>
    private readonly ILogger<CiCdWebHookHandler> logger;


    /// <summary>
    /// Initializes a new instance of the <see cref="WebHookHandler" /> class.
    /// </summary>
    ///// <param name="applicationLogRepository">The application log repository.</param>
    /// <param name="subscriptionsLogRepository">The subscriptions log repository.</param>
    /// <param name="subscriptionsRepository">The subscriptions repository.</param>
    /// <param name="fulfillApiClient">The fulfill API client.</param>
    /// <param name="usersRepository">The users repository.</param>
    public CiCdWebHookHandler(
            ISubscriptionLogRepository subscriptionsLogRepository, 
            ISubscriptionsRepository subscriptionsRepository, 
            IFulfillmentApiService fulfillApiService, 
            IUsersRepository usersRepository, 
            ILogger<CiCdWebHookHandler> logger
        )
    {
        this.subscriptionsRepository = subscriptionsRepository;
        this.subscriptionsLogRepository = subscriptionsLogRepository;
        this.usersRepository = usersRepository;
        this.fulfillApiService = fulfillApiService;
        this.logger = logger;
    }

    /// <summary>
    /// Processes provisioning success asynchronous.
    /// </summary>
    /// <param name="subscriptionId">The subscription ID.</param>
    /// <returns>Provisioning Success Async</returns>
    public async Task ProvisioningSuccessAsync(Guid subscriptionId)
    {
        this.logger?.LogInformation("CiCdWebHookHandler provisioning success {0}", subscriptionId);
        var subscription = this.subscriptionsRepository.GetById(subscriptionId);
        this.logger?.LogInformation("Result subscription : {0}", JsonSerializer.Serialize(subscription.AmpplanId));
        this.logger?.LogInformation("Get User");
        var userdeatils = this.usersRepository.Get(subscription.UserId.GetValueOrDefault());
        string oldstatus = subscription.SubscriptionStatus;

        if (subscription.SubscriptionStatus == SubscriptionStatusEnumExtension.PendingProvisioning.ToString() ||
            subscription.SubscriptionStatus == SubscriptionStatusEnumExtension.ProvisioningFailed.ToString())
        {
            try
            {
                this.logger?.LogInformation("Get attributelsit");

                var subscriptionData = this.fulfillApiService.ActivateSubscriptionAsync(subscriptionId, subscription.AmpplanId).ConfigureAwait(false).GetAwaiter().GetResult();

                this.logger?.LogInformation("UpdateSubscriptionStatus");

                this.subscriptionsRepository.UpdateStatusForSubscription(subscriptionId, SubscriptionStatusEnumExtension.Subscribed.ToString(), true);

                SubscriptionAuditLogs auditLog = new SubscriptionAuditLogs()
                {
                    Attribute = SubscriptionLogAttributes.Status.ToString(),
                    SubscriptionId = subscription.Id,
                    NewValue = SubscriptionStatusEnumExtension.Subscribed.ToString(),
                    OldValue = oldstatus,
                    CreateBy = userdeatils.UserId,
                    CreateDate = DateTime.Now,
                };
                this.subscriptionsLogRepository.Save(auditLog);

                this.subscriptionsLogRepository.LogStatusDuringProvisioning(subscriptionId, "Provisioned", SubscriptionStatusEnumExtension.Subscribed.ToString());
            }
            catch (Exception ex)
            {
                string errorDescriptin = string.Format("Exception: {0} :: Inner Exception:{1}", ex.Message, ex.InnerException);
                this.subscriptionsLogRepository.LogStatusDuringProvisioning(subscriptionId, errorDescriptin, SubscriptionStatusEnumExtension.ActivationFailed.ToString());
                this.logger?.LogInformation(errorDescriptin);

                this.subscriptionsRepository.UpdateStatusForSubscription(subscriptionId, SubscriptionStatusEnumExtension.ActivationFailed.ToString(), false);

                // Set the status as ProvisioningFailed.
                SubscriptionAuditLogs auditLog = new SubscriptionAuditLogs()
                {
                    Attribute = SubscriptionLogAttributes.Status.ToString(),
                    SubscriptionId = subscription.Id,
                    NewValue = SubscriptionStatusEnumExtension.ActivationFailed.ToString(),
                    OldValue = subscription.SubscriptionStatus,
                    CreateBy = userdeatils.UserId,
                    CreateDate = DateTime.Now,
                };
                this.subscriptionsLogRepository.Save(auditLog);
            }
        }

        await Task.CompletedTask;
    }

    /// <summary>
    /// Processes provisioning failure asynchronous.
    /// </summary>
    /// <param name="payload">The payload.</param>
    /// <param name="status">The pipeline status.</param>
    /// <returns>Provisioning Failure Async</returns>
    public async Task ProvisioningFailureAsync(Guid subscriptionId, PipelineStatus status)
    {
        this.logger?.LogInformation("CiCdWebHookHandler provisioning failure {0}", subscriptionId);
        var subscription = this.subscriptionsRepository.GetById(subscriptionId);
        this.logger?.LogInformation("Result subscription : {0}", JsonSerializer.Serialize(subscription.AmpplanId));
        this.logger?.LogInformation("Get User");
        var userdeatils = this.usersRepository.Get(subscription.UserId.GetValueOrDefault());
        string oldstatus = subscription.SubscriptionStatus;

        if (subscription.SubscriptionStatus == SubscriptionStatusEnumExtension.PendingProvisioning.ToString())
        {
            this.logger?.LogInformation("UpdateSubscriptionStatus");

            this.subscriptionsRepository.UpdateStatusForSubscription(subscriptionId, SubscriptionStatusEnumExtension.ProvisioningFailed.ToString(), true);

            SubscriptionAuditLogs auditLog = new SubscriptionAuditLogs()
            {
                Attribute = SubscriptionLogAttributes.Status.ToString(),
                SubscriptionId = subscription.Id,
                NewValue = SubscriptionStatusEnumExtension.ProvisioningFailed.ToString(),
                OldValue = oldstatus,
                CreateBy = userdeatils.UserId,
                CreateDate = DateTime.Now,
            };
            this.subscriptionsLogRepository.Save(auditLog);

            this.subscriptionsLogRepository.LogStatusDuringProvisioning(subscriptionId, "Provisioned", SubscriptionStatusEnumExtension.ProvisioningFailed.ToString());
        }

        await Task.CompletedTask;
    }

    /// <summary>
    /// Processes deprovisioning success asynchronous.
    /// </summary>
    /// <param name="subscriptionId">The subscription ID.</param>
    /// <returns>Deprovisioning Success Async</returns>
    public async Task DeprovisioningSuccessAsync(Guid subscriptionId)
    {
        this.logger?.LogInformation("CiCdWebHookHandler deprovisioning success {0}", subscriptionId);
        var subscription = this.subscriptionsRepository.GetById(subscriptionId);
        this.logger?.LogInformation("Result subscription : {0}", JsonSerializer.Serialize(subscription.AmpplanId));
        this.logger?.LogInformation("Get User");
        var userdeatils = this.usersRepository.Get(subscription.UserId.GetValueOrDefault());
        string oldstatus = subscription.SubscriptionStatus;

        if (subscription.SubscriptionStatus == SubscriptionStatusEnumExtension.PendingDeprovisioning.ToString())
        {
            this.logger?.LogInformation("UpdateSubscriptionStatus");

            this.subscriptionsRepository.UpdateStatusForSubscription(subscriptionId, SubscriptionStatusEnumExtension.Deprovisioned.ToString(), true);

            SubscriptionAuditLogs auditLog = new SubscriptionAuditLogs()
            {
                Attribute = SubscriptionLogAttributes.Status.ToString(),
                SubscriptionId = subscription.Id,
                NewValue = SubscriptionStatusEnumExtension.Deprovisioned.ToString(),
                OldValue = oldstatus,
                CreateBy = userdeatils.UserId,
                CreateDate = DateTime.Now,
            };
            this.subscriptionsLogRepository.Save(auditLog);

            this.subscriptionsLogRepository.LogStatusDuringProvisioning(subscriptionId, "Deprovisioned", SubscriptionStatusEnumExtension.Deprovisioned.ToString());
        }

        await Task.CompletedTask;
    }

    /// <summary>
    /// Processes deprovisioning failure asynchronous.
    /// </summary>
    /// <param name="subscriptionId">The subscription ID.</param>
    /// <param name="status">The pipeline status.</param>
    /// <returns>Deprovisioning Failure Async</returns>
    public async Task DeprovisioningFailureAsync(Guid subscriptionId, PipelineStatus status)
    {
        this.logger?.LogInformation("CiCdWebHookHandler deprovisioning failure {0}", subscriptionId);
        var subscription = this.subscriptionsRepository.GetById(subscriptionId);
        this.logger?.LogInformation("Result subscription : {0}", JsonSerializer.Serialize(subscription.AmpplanId));
        this.logger?.LogInformation("Get User");
        var userdeatils = this.usersRepository.Get(subscription.UserId.GetValueOrDefault());
        string oldstatus = subscription.SubscriptionStatus;

        if (subscription.SubscriptionStatus == SubscriptionStatusEnumExtension.PendingDeprovisioning.ToString() ||
            subscription.SubscriptionStatus == SubscriptionStatusEnumExtension.DeprovisioningFailed.ToString())
        {
            this.logger?.LogInformation("UpdateSubscriptionStatus");

            this.subscriptionsRepository.UpdateStatusForSubscription(subscriptionId, SubscriptionStatusEnumExtension.DeprovisioningFailed.ToString(), true);

            SubscriptionAuditLogs auditLog = new SubscriptionAuditLogs()
            {
                Attribute = SubscriptionLogAttributes.Status.ToString(),
                SubscriptionId = subscription.Id,
                NewValue = SubscriptionStatusEnumExtension.DeprovisioningFailed.ToString(),
                OldValue = oldstatus,
                CreateBy = userdeatils.UserId,
                CreateDate = DateTime.Now,
            };
            this.subscriptionsLogRepository.Save(auditLog);

            this.subscriptionsLogRepository.LogStatusDuringProvisioning(subscriptionId, "Deprovisioned", SubscriptionStatusEnumExtension.DeprovisioningFailed.ToString());
        }

        await Task.CompletedTask;
    }
}