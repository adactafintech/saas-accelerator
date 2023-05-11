namespace Marketplace.SaaS.Accelerator.Services.Models;

/// <summary>
/// Enumerator witrh new  status.
/// </summary>
public enum SubscriptionStatusEnumExtension
{
    /// <summary>
    /// The pending fulfillment start
    /// </summary>
    PendingFulfillmentStart,

    /// <summary>
    /// Pending Activation
    /// </summary>
    PendingActivation,

    /// <summary>
    /// Pending Provisioning
    /// </summary>
    PendingProvisioning,

    /// <summary>
    /// The subscribed
    /// </summary>
    Subscribed,

    /// <summary>
    /// The unsubscribed
    /// </summary>
    Unsubscribed,

    /// <summary>
    /// When status cannot be parsed to any of the other Status types
    /// </summary>
    UnRecognized,

    /// <summary>
    /// The activation failed
    /// </summary>
    ActivationFailed,

    /// <summary>
    /// Provisioning Failed
    /// </summary>
    ProvisioningFailed,

    /// <summary>
    /// The pending unsubscribe
    /// </summary>
    PendingUnsubscribe,

    /// <summary>
    /// The unsubscribe failed
    /// </summary>
    UnsubscribeFailed,

    /// <summary>
    /// The pending deprovisioning
    /// </summary>
    PendingDeprovisioning,

    /// <summary>
    /// The deprovisioned
    /// </summary>
    Deprovisioned,

    /// <summary>
    /// Deprovisioning Failed
    /// </summary>
    DeprovisioningFailed,

    /// <summary>
    /// The Suspend 
    /// </summary>
    Suspend,
}