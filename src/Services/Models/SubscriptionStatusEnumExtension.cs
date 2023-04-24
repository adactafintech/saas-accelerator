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
    /// Pending Activation
    /// </summary>
    PendingActivation,

    /// <summary>
    /// Pending Provisioning
    /// </summary>
    PendingProvisioning,

    /// <summary>
    /// The pending unsubscribe
    /// </summary>
    PendingUnsubscribe,

    /// <summary>
    /// Provisioning Failed
    /// </summary>
    ProvisioningFailed,

    /// <summary>
    /// The unsubscribe failed
    /// </summary>
    UnsubscribeFailed,

    /// <summary>
    /// The Suspend 
    /// </summary>
    Suspend,
}