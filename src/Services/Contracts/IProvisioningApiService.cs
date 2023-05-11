using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.SaaS.Accelerator.Services.Contracts;

/// <summary>
/// Contract for Provisioning API.
/// </summary>
public interface IProvisioningApiService
{
    Task<object> ProvisionSubscriptionAsync(Guid subscriptionId, string tenantName, string companyName);
    Task<object> DeprovisionSubscriptionAsync(Guid subscriptionId, string tenantName, string companyName);
}
