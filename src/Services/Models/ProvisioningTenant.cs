namespace Marketplace.SaaS.Accelerator.Services.Models;
public class ProvisioningTenant
{
    public string Customer { get; set; }

    public bool CustomerNew { get; set; }

    public string Company { get; set; }

    public bool CompanyNew { get; set; }

    public string TenantName { get; set; }

    // public string Tier { get; set; }

    public string PlatformVersion { get; set; }

    public int DatabasePasswordSerial { get; set; }

    public string Vertical { get; set; }
}
