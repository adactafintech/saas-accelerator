﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for license information.

using System;
using System.Linq;
using System.Text.Json;
using Marketplace.SaaS.Accelerator.DataAccess.Contracts;
using Marketplace.SaaS.Accelerator.DataAccess.Entities;
using Marketplace.SaaS.Accelerator.Services.Contracts;
using Marketplace.SaaS.Accelerator.Services.Models;
using Marketplace.SaaS.Accelerator.Services.Services;
using Microsoft.Extensions.Logging;

namespace Marketplace.SaaS.Accelerator.Services.StatusHandlers;

/// <summary>
/// Status handler to handle the unsubscription event.
/// </summary>
/// <seealso cref="Microsoft.Marketplace.SaasKit.Provisioning.Webjob.StatusHandlers.AbstractSubscriptionStatusHandler" />
public class UnsubscribeStatusHandler : AbstractSubscriptionStatusHandler
{
    /// <summary>
    /// The fulfillment apiclient.
    /// </summary>
    private readonly IFulfillmentApiService fulfillmentApiService;

    /// <summary>
    /// The provisioning apiclient.
    /// </summary>
    private readonly IProvisioningApiService provisioningApiService;

    /// <summary>
    /// The subscription service.
    /// </summary>
    private readonly SubscriptionService subscriptionService;

    /// <summary>
    /// The subscription log repository.
    /// </summary>
    private readonly ISubscriptionLogRepository subscriptionLogRepository;

    /// <summary>
    /// The logger.
    /// </summary>
    private readonly ILogger<UnsubscribeStatusHandler> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="UnsubscribeStatusHandler" /> class.
    /// </summary>
    /// <param name="provisioningApiService">The provisioning API client.</param>
    /// <param name="fulfillApiService">The fulfill API client.</param>
    /// <param name="subscriptionsRepository">The subscriptions repository.</param>
    /// <param name="subscriptionLogRepository">The subscription log repository.</param>
    /// <param name="plansRepository">The plans repository.</param>
    /// <param name="usersRepository">The users repository.</param>
    /// <param name="logger">The logger.</param>
    public UnsubscribeStatusHandler(
        IFulfillmentApiService fulfillApiService,
        IProvisioningApiService provisioningApiService,
        ISubscriptionsRepository subscriptionsRepository,
        ISubscriptionLogRepository subscriptionLogRepository,
        IPlansRepository plansRepository,
        IUsersRepository usersRepository,
        ILogger<UnsubscribeStatusHandler> logger)
        : base(subscriptionsRepository, plansRepository, usersRepository)
    {
        this.fulfillmentApiService = fulfillApiService;
        this.provisioningApiService = provisioningApiService;
        this.subscriptionLogRepository = subscriptionLogRepository;
        this.subscriptionService = new SubscriptionService(this.subscriptionsRepository, this.plansRepository);
        this.logger = logger;
    }

    /// <summary>
    /// Processes the specified subscription identifier.
    /// </summary>
    /// <param name="subscriptionID">The subscription identifier.</param>
    public override void Process(Guid subscriptionID)
    {
        this.logger?.LogInformation("UnsubscribeStatusHandler {0}", subscriptionID);
        var subscription = this.GetSubscriptionById(subscriptionID);
        this.logger?.LogInformation("Result subscription : {0}", JsonSerializer.Serialize(subscription.AmpplanId));

        this.logger?.LogInformation("Get User");
        var userdeatils = this.GetUserById(subscription.UserId);
        string status = subscription.SubscriptionStatus;
        if (subscription.SubscriptionStatus == SubscriptionStatusEnumExtension.PendingUnsubscribe.ToString() || subscription.SubscriptionStatus == SubscriptionStatusEnumExtension.UnsubscribeFailed.ToString())
        {
            try
            {
                var subscriptionData = this.fulfillmentApiService.DeleteSubscriptionAsync(subscriptionID, subscription.AmpplanId).ConfigureAwait(false).GetAwaiter().GetResult();

                this.subscriptionsRepository.UpdateStatusForSubscription(subscriptionID, SubscriptionStatusEnumExtension.Unsubscribed.ToString(), false);

                SubscriptionAuditLogs auditLog = new SubscriptionAuditLogs()
                {
                    Attribute = SubscriptionLogAttributes.Status.ToString(),
                    SubscriptionId = subscription.Id,
                    NewValue = SubscriptionStatusEnumExtension.Unsubscribed.ToString(),
                    OldValue = status,
                    CreateBy = userdeatils.UserId,
                    CreateDate = DateTime.Now,
                };
                this.subscriptionLogRepository.Save(auditLog);

                this.subscriptionLogRepository.LogStatusDuringProvisioning(subscriptionID, "Unsubscribed", SubscriptionStatusEnumExtension.Unsubscribed.ToString());
            }
            catch (Exception ex)
            {
                string errorDescriptin = string.Format("Unsubscribe exception: {0} :: Inner Exception:{1}", ex.Message, ex.InnerException);
                this.subscriptionLogRepository.LogStatusDuringProvisioning(subscriptionID, errorDescriptin, SubscriptionStatusEnumExtension.UnsubscribeFailed.ToString());
                this.logger?.LogInformation(errorDescriptin);

                this.subscriptionsRepository.UpdateStatusForSubscription(subscriptionID, SubscriptionStatusEnumExtension.UnsubscribeFailed.ToString(), true);

                SubscriptionAuditLogs auditLog = new SubscriptionAuditLogs()
                {
                    Attribute = SubscriptionLogAttributes.Status.ToString(),
                    SubscriptionId = subscription.Id,
                    NewValue = SubscriptionStatusEnumExtension.UnsubscribeFailed.ToString(),
                    OldValue = subscription.SubscriptionStatus,
                    CreateBy = userdeatils.UserId,
                    CreateDate = DateTime.Now,
                };
                this.subscriptionLogRepository.Save(auditLog);

                // if error in unsubscribing then do not deprovision tenant
                return;
            }
        }

        if (subscription.SubscriptionStatus == SubscriptionStatusEnumExtension.Unsubscribed.ToString() || subscription.SubscriptionStatus == SubscriptionStatusEnumExtension.DeprovisioningFailed.ToString())
        {

            try
            {
                SubscriptionResultExtension subscriptionDetail = new SubscriptionResultExtension();
                this.logger?.LogInformation("Deprovisioning tenant {0}", subscriptionID);

                subscriptionDetail = this.subscriptionService.GetSubscriptionsBySubscriptionId(subscriptionID);
                Plans planDetail = this.plansRepository.GetById(subscriptionDetail.PlanId);
                var subscriptionParmaeters = this.subscriptionService.GetSubscriptionsParametersById(subscriptionID, planDetail.PlanGuid);

                var pipelineData = this.provisioningApiService.DeprovisionSubscriptionAsync(subscriptionID,
                    subscriptionParmaeters.Where(p => p.DisplayName == "Tenant Name").Single().Value,
                    subscriptionParmaeters.Where(p => p.DisplayName == "Customer Name").Single().Value,
                    subscriptionParmaeters.Where(p => p.DisplayName == "Company Name").Single().Value).ConfigureAwait(false).GetAwaiter().GetResult();

                this.subscriptionsRepository.UpdateStatusForSubscription(subscriptionID, SubscriptionStatusEnumExtension.PendingDeprovisioning.ToString(), false);

                SubscriptionAuditLogs auditLog = new SubscriptionAuditLogs()
                {
                    Attribute = SubscriptionLogAttributes.Status.ToString(),
                    SubscriptionId = subscription.Id,
                    NewValue = SubscriptionStatusEnumExtension.PendingDeprovisioning.ToString(),
                    OldValue = status,
                    CreateBy = userdeatils.UserId,
                    CreateDate = DateTime.Now,
                };
                this.subscriptionLogRepository.Save(auditLog);

                this.subscriptionLogRepository.LogStatusDuringProvisioning(subscriptionID, "Unsubscribed", SubscriptionStatusEnumExtension.PendingDeprovisioning.ToString());
            }
            catch (Exception ex)
            {
                string errorDescriptin = string.Format("Deprovision exception: {0} :: Inner Exception:{1}", ex.Message, ex.InnerException);
                this.subscriptionLogRepository.LogStatusDuringProvisioning(subscriptionID, errorDescriptin, SubscriptionStatusEnumExtension.DeprovisioningFailed.ToString());
                this.logger?.LogInformation(errorDescriptin);

                this.subscriptionsRepository.UpdateStatusForSubscription(subscriptionID, SubscriptionStatusEnumExtension.DeprovisioningFailed.ToString(), true);

                SubscriptionAuditLogs auditLog = new SubscriptionAuditLogs()
                {
                    Attribute = SubscriptionLogAttributes.Status.ToString(),
                    SubscriptionId = subscription.Id,
                    NewValue = SubscriptionStatusEnumExtension.DeprovisioningFailed.ToString(),
                    OldValue = subscription.SubscriptionStatus,
                    CreateBy = userdeatils.UserId,
                    CreateDate = DateTime.Now,
                };
                this.subscriptionLogRepository.Save(auditLog);
            }
        }
    }
}