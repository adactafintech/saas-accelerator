using Marketplace.SaaS.Accelerator.Services.Configurations;
using Marketplace.SaaS.Accelerator.Services.Contracts;
using Marketplace.SaaS.Accelerator.Services.Exceptions;
using Marketplace.SaaS.Accelerator.Services.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Marketplace.SaaS.Accelerator.Services.Services;
public class ProvisioningApiService : BaseApiService, IProvisioningApiService
{
    /// <summary>
    /// Gets or sets the SDK settings.
    /// </summary>
    /// <value>
    /// The SDK settings.
    /// </value>
    protected SaaSApiClientConfiguration ClientConfiguration { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ProvisioningApiService" /> class.
    /// </summary>
    /// <param name="sdkSettings">The SDK settings.</param>
    /// <param name="logger">The logger.</param>
    public ProvisioningApiService(SaaSApiClientConfiguration sdkSettings,
        ILogger logger) : base(logger)
    {
        this.ClientConfiguration = sdkSettings;
    }

    /// <summary>
    /// Provisions the subscription.
    /// </summary>
    /// <returns>
    /// Data of created pipeline.
    /// </returns>
    public async Task<object> ProvisionSubscriptionAsync(Guid subscriptionId, string tenantName, string companyName)
    {
        this.Logger?.Info($"Inside ProvisionSubscriptionAsync() of ProvisioningApiService, trying to Provision Subscription :: {subscriptionId}");

        var tenant = new ProvisioningTenant()
        {
            Corporation = "Adacta Fintech",
            Company = companyName,
            TenantType = "sandbox",
            TenantName = tenantName,
            AdinsureConfigurationVersion = "19.1.3",
            RequireModifyApproval = false,
            RequirePatchApproval = false,
            DatabasePasswordSerial = 1681707929,
        };

        var serializeOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var requestUrl = string.Format(this.ClientConfiguration.ProvisionAPIBaseUrl, this.ClientConfiguration.ProvisionToken);
        var parameters = new Dictionary<string, string> {
            { "token", this.ClientConfiguration.ProvisionToken },
            { "ref", this.ClientConfiguration.ProvisionBranch },
            { "variables[OPERATION]", "provision"},
            { "variables[TENANT]", JsonSerializer.Serialize<ProvisioningTenant>(tenant, serializeOptions)},
            { "variables[CLIENT_REF]", subscriptionId.ToString()},
            //{ "variables[TEST_PIPELINE]", "true"},
        };
        var encodedContent = new FormUrlEncodedContent(parameters);

        using (var httpClient = new HttpClient())
        using (var httpResonse = await httpClient.PostAsync(requestUrl, encodedContent))
        {
            try
            {
                httpResonse.EnsureSuccessStatusCode();

                return await httpResonse.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException hre)
            {
                this.Logger?.Error($"Error while completing the provisioning request with Code: {httpResonse.StatusCode}, Message: " + JsonSerializer.Serialize(new { Error = hre.Message, }));
                throw new MarketplaceException("Provisioning request failed, please check logs!");
            }
        }
    }
}
