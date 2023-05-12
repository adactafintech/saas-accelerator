// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for license information.

using System;
using System.Text.Json;
using Marketplace.SaaS.Accelerator.DataAccess.Contracts;
using Marketplace.SaaS.Accelerator.DataAccess.Entities;
using Marketplace.SaaS.Accelerator.Services.Contracts;
using Marketplace.SaaS.Accelerator.Services.Models;
using Marketplace.SaaS.Accelerator.Services.Services;
using Microsoft.Extensions.Logging;

namespace Marketplace.SaaS.Accelerator.Services.StatusHandlers;

/// <summary>
/// Status handler to handle the subscriptions that are in PendingActivation status.
/// </summary>
/// <seealso cref="Microsoft.Marketplace.SaasKit.Provisioning.Webjob.StatusHandlers.AbstractSubscriptionStatusHandler" />
public class PendingActivationStatusHandler : AbstractSubscriptionStatusHandler
{
    /// <summary>
    /// The plan repository.
    /// </summary>
    private readonly IPlansRepository planRepository;

    /// <summary>
    /// The subscription service.
    /// </summary>
    private readonly SubscriptionService subscriptionService;

    /// <summary>
    /// The provisioning apiclient.
    /// </summary>
    private readonly IProvisioningApiService provisioningApiService;

    /// <summary>
    /// The subscription log repository.
    /// </summary>
    private readonly ISubscriptionLogRepository subscriptionLogRepository;

    /// <summary>
    /// The logger.
    /// </summary>
    private readonly ILogger<PendingActivationStatusHandler> logger;

    private readonly ISubscriptionsRepository subscriptionRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="PendingActivationStatusHandler"/> class.
    /// </summary>
    /// <param name="provisioningApiService">The provisioning API client.</param>
    /// <param name="subscriptionsRepository">The subscriptions repository.</param>
    /// <param name="subscriptionLogRepository">The subscription log repository.</param>
    /// <param name="subscriptionTemplateParametersRepository">The subscription template parameters repository.</param>
    /// <param name="plansRepository">The plans repository.</param>
    /// <param name="usersRepository">The users repository.</param>
    /// <param name="logger">The logger.</param>
    public PendingActivationStatusHandler(
        IProvisioningApiService provisioningApiService,
        ISubscriptionsRepository subscriptionsRepository,
        ISubscriptionLogRepository subscriptionLogRepository,
        IPlansRepository plansRepository,
        IUsersRepository usersRepository,
        ILogger<PendingActivationStatusHandler> logger)
        : base(subscriptionsRepository, plansRepository, usersRepository)
    {
        this.provisioningApiService = provisioningApiService;
        this.planRepository = plansRepository;
        this.subscriptionLogRepository = subscriptionLogRepository;
        this.subscriptionRepository = subscriptionsRepository;
        this.subscriptionService = new SubscriptionService(this.subscriptionRepository, this.planRepository);
        this.logger = logger;
    }

    /// <summary>
    /// Processes the specified subscription identifier.
    /// </summary>
    /// <param name="subscriptionID">The subscription identifier.</param>
    public override void Process(Guid subscriptionID)
    {
        this.logger?.LogInformation("PendingActivationStatusHandler {0}", subscriptionID);
        var subscription = this.GetSubscriptionById(subscriptionID);
        this.logger?.LogInformation("Result subscription : {0}", JsonSerializer.Serialize(subscription.AmpplanId));
        this.logger?.LogInformation("Get User");
        var userdeatils = this.GetUserById(subscription.UserId);
        string oldstatus = subscription.SubscriptionStatus;

        if (subscription.SubscriptionStatus == SubscriptionStatusEnumExtension.PendingActivation.ToString())
        {
            try
            {
                SubscriptionResultExtension subscriptionDetail = new SubscriptionResultExtension();
                this.logger?.LogInformation("Trigger provisioning");

                // var oldValue = this.subscriptionService.GetPartnerSubscription(this.CurrentUserEmailAddress, subscriptionID, true).FirstOrDefault();
                subscriptionDetail = this.subscriptionService.GetSubscriptionsBySubscriptionId(subscriptionID);
                Plans planDetail = this.planRepository.GetById(subscriptionDetail.PlanId);
                var subscriptionParameters = this.subscriptionService.GetSubscriptionsParametersById(subscriptionID, planDetail.PlanGuid);

                this.logger?.LogInformation($"Number of parameters ::  {subscriptionParameters?.Count}");
                if (subscriptionParameters?.Count > 0)
                {
                    foreach(var parameter in subscriptionParameters)
                    {
                        this.logger?.LogInformation($"{parameter.DisplayName} ::  {parameter.Value}");
                    }
                }

                var pipelineData = this.provisioningApiService.ProvisionSubscriptionAsync(subscriptionID, subscriptionParameters[0].Value, subscriptionParameters[1].Value).ConfigureAwait(false).GetAwaiter().GetResult();

                this.logger?.LogInformation("UpdateWebJobSubscriptionStatus");

                this.subscriptionsRepository.UpdateStatusForSubscription(subscriptionID, SubscriptionStatusEnumExtension.PendingProvisioning.ToString(), true);

                SubscriptionAuditLogs auditLog = new SubscriptionAuditLogs()
                {
                    Attribute = SubscriptionLogAttributes.Status.ToString(),
                    SubscriptionId = subscription.Id,
                    NewValue = SubscriptionStatusEnumExtension.PendingProvisioning.ToString(),
                    OldValue = oldstatus,
                    CreateBy = userdeatils.UserId,
                    CreateDate = DateTime.Now,
                };
                this.subscriptionLogRepository.Save(auditLog);
            }
            catch (Exception ex)
            {
                string errorDescriptin = string.Format("Exception: {0} :: Inner Exception:{1}", ex.Message, ex.InnerException);
                this.logger?.LogInformation(errorDescriptin);

                this.subscriptionsRepository.UpdateStatusForSubscription(subscriptionID, SubscriptionStatusEnumExtension.ProvisioningFailed.ToString(), false);

                // Set the status as ActivationFailed.
                SubscriptionAuditLogs auditLog = new SubscriptionAuditLogs()
                {
                    Attribute = SubscriptionLogAttributes.Status.ToString(),
                    SubscriptionId = subscription.Id,
                    NewValue = SubscriptionStatusEnumExtension.ProvisioningFailed.ToString(),
                    OldValue = subscription.SubscriptionStatus,
                    CreateBy = userdeatils.UserId,
                    CreateDate = DateTime.Now,
                };
                this.subscriptionLogRepository.Save(auditLog);
            }
        }
    }
}