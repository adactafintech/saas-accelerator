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

    private JsonSerializerOptions serializeOptions { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ProvisioningApiService" /> class.
    /// </summary>
    /// <param name="sdkSettings">The SDK settings.</param>
    /// <param name="logger">The logger.</param>
    public ProvisioningApiService(SaaSApiClientConfiguration sdkSettings,
        ILogger logger) : base(logger)
    {
        this.ClientConfiguration = sdkSettings;
        this.serializeOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }

    /// <summary>
    /// Provisions the subscription.
    /// </summary>
    /// <returns>
    /// Data of created pipeline.
    /// </returns>
    public async Task<object> ProvisionSubscriptionAsync(Guid subscriptionId, string tenantName, string customerName, bool customerNew, string companyName, bool companyNew, string tier, string vertical)
    {
        this.Logger?.Info("ProvisioningApiService starting provisioning");

        this.Logger?.Info($"Inside ProvisionSubscriptionAsync() of ProvisioningApiService, trying to Provision Subscription :: {subscriptionId}");

        var tenantInfo = new ProvisioningTenant()
        {
            Customer = customerName,
            CustomerNew = customerNew,
            Company = companyName,
            CompanyNew = companyNew,
            TenantName = tenantName,
            // Tier = tier,
            PlatformVersion = "25.0.0",
            DatabasePasswordSerial = 1681707929,
            Vertical = vertical,
        };

        var parameters = new Dictionary<string, string> {
            { "token", this.ClientConfiguration.ProvisionToken },
            { "ref", this.ClientConfiguration.ProvisionBranch },
            { "variables[API_VERSION]", "v1"},
            { "variables[OPERATION]", "provision"},
            { "variables[TENANT]", JsonSerializer.Serialize<ProvisioningTenant>(tenantInfo, this.serializeOptions)},
            { "variables[CLIENT_REF]", subscriptionId.ToString()},
            { "variables[PIPELINE_NAME]", $"Tenant provision (Admin) - {tenantInfo.Customer} - {tenantInfo.Company} - {tenantInfo.TenantName} - {tenantInfo.PlatformVersion}"}

            // WARNING: use this only on not-production environments and for development
            //{ "variables[TEST_PIPELINE]", "true"},
        };
        var encodedContent = new FormUrlEncodedContent(parameters);

        using (var httpClient = new HttpClient())
        using (var httpResonse = await httpClient.PostAsync(this.ClientConfiguration.ProvisionAPIBaseUrl, encodedContent))
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

    /// <summary>
    /// Deprovisions the subscription.
    /// </summary>
    /// <returns>
    /// Data of created pipeline.
    /// </returns>
    public async Task<object> DeprovisionSubscriptionAsync(Guid subscriptionId, string tenantName, string customerName, string companyName)
    {
        this.Logger?.Info($"Inside DeprovisionSubscriptionAsync() of ProvisioningApiService, trying to Deprovision Subscription :: {subscriptionId}");

        var tenantInfo = new DeprovisioningTenant()
        {
            Customer = customerName,
            Company = companyName,
            TenantName = tenantName,
            CleanupCustomerData = false,
        };

        var parameters = new Dictionary<string, string> {
            { "token", this.ClientConfiguration.ProvisionToken },
            { "ref", this.ClientConfiguration.ProvisionBranch },
            { "variables[API_VERSION]", "v1"},
            { "variables[OPERATION]", "destroy"},
            { "variables[TENANT]", JsonSerializer.Serialize<DeprovisioningTenant>(tenantInfo, this.serializeOptions)},
            { "variables[CLIENT_REF]", subscriptionId.ToString()},
            { "variables[PIPELINE_NAME]", $"Tenant deprovision (Admin) - {tenantInfo.Customer} - {tenantInfo.Company} - {tenantInfo.TenantName}"}

            // WARNING: use this only on not-production environments and for development
            //{ "variables[TEST_PIPELINE]", "true"},
        };
        var encodedContent = new FormUrlEncodedContent(parameters);

        using (var httpClient = new HttpClient())
        using (var httpResonse = await httpClient.PostAsync(this.ClientConfiguration.ProvisionAPIBaseUrl, encodedContent))
        {
            try
            {
                httpResonse.EnsureSuccessStatusCode();

                return await httpResonse.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException hre)
            {
                this.Logger?.Error($"Error while completing the deprovisioning request with Code: {httpResonse.StatusCode}, Message: " + JsonSerializer.Serialize(new { Error = hre.Message, }));
                throw new MarketplaceException("Deprovisioning request failed, please check logs!");
            }
        }
    }
}
