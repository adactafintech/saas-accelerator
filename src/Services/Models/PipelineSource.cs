using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.SaaS.Accelerator.Services.Models;
public class PipelineSource
{
    public int JobId { get; set; }
    public int PipelineId { get; set; }
    public PipelineProject Project { get; set; }
}
