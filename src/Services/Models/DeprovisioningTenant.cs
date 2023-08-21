namespace Marketplace.SaaS.Accelerator.Services.Models;
public class DeprovisioningTenant
{
    public string Customer { get; set; }

    public string Company { get; set; }

    public string TenantName { get; set; }

    public bool CleanupCustomerData { get; set; }
}
