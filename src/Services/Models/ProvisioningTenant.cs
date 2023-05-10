using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.SaaS.Accelerator.Services.Models;
public class ProvisioningTenant
{
    public string Corporation { get; set; }

    public string Company { get; set; }

    public string TenantName { get; set; }

    public string TenantType { get; set; }

    public string AdinsureConfigurationVersion { get; set; }

    public bool RequireModifyApproval { get; set; }

    public bool RequirePatchApproval { get; set; }

    public int DatabasePasswordSerial { get; set; }
}
