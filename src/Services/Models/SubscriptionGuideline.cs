using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace Marketplace.SaaS.Accelerator.Services.Models;

/// <summary>
/// Subscription Guideline.
/// </summary>
public class SubscriptionGuideline
{
    /// <summary>
    /// Gets or sets the error message.
    /// </summary>
    /// <value>
    /// The error message.
    /// </value>
    public string ErrorMessage { get; set; }

    /// <summary>
    /// Gets or sets the error status code.
    /// </summary>
    /// <value>
    /// The error status code.
    /// </value>
    public string ErrorStatusCode { get; set; }

    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    /// <value>
    /// The identifier.
    /// </value>
    [JsonPropertyName("subscriptionIdd")]
    [DisplayName("Subscription Id")]
    public Guid SubscriptionId { get; set; }

    /// <summary>
    /// Gets or sets the plan identifier.
    /// </summary>
    /// <value>
    /// The plan identifier.
    /// </value>
    [DisplayName("Plan Id")]
    [JsonPropertyName("planId")]
    public string PlanId { get; set; }

    /// <summary>
    /// Gets or sets guideline's readme
    /// </summary>
    /// <value>
    /// Guideline as HTML
    /// </value>
    public string Readme { get; set; }

}