﻿using Marketplace.SaaS.Accelerator.Services.Models;
using System;
using System.Threading.Tasks;

namespace Marketplace.SaaS.Accelerator.Services.WebHook;

/// <summary>
/// CI/CD Web Hook Handler Interface
/// </summary>
public interface ICiCdWebhookHandler
{
    /// <summary>
    /// Processes provisioning success asynchronous.
    /// </summary>
    /// <param name="subscriptionId">The subscription ID.</param>
    /// <returns>Provisioning Success Async</returns>
    Task ProvisioningSuccessAsync(Guid subscriptionId);

    /// <summary>
    /// Processes provisioning failure asynchronous.
    /// </summary>
    /// <param name="payload">The payload.</param>
    /// <param name="status">The pipeline status.</param>
    /// <returns>Provisioning Failure Async</returns>
    Task ProvisioningFailureAsync(Guid subscriptionId, PipelineStatus status);

    /// <summary>
    /// Processes deprovisioning success asynchronous.
    /// </summary>
    /// <param name="subscriptionId">The subscription ID.</param>
    /// <returns>Deprovisioning Success Async</returns>
    Task DeprovisioningSuccessAsync(Guid subscriptionId);

    /// <summary>
    /// Processes deprovisioning failure asynchronous.
    /// </summary>
    /// <param name="subscriptionId">The subscription ID.</param>
    /// <param name="status">The pipeline status.</param>
    /// <returns>Deprovisioning Failure Async</returns>
    Task DeprovisioningFailureAsync(Guid subscriptionId, PipelineStatus status);

}