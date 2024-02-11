using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField] private CollectibleType type;
    public CollectibleType Type => type;

    /// <summary>
    /// Triggers when another collider enters the trigger collider.
    /// If the collider belongs to the airplane, the collectible is acquired.
    /// </summary>
    private void OnTriggerEnter(Collider collider)
    {
        var collidingLayer = LayerMask.LayerToName(collider.gameObject.layer);
        if (collidingLayer == GlobalConstants.AIRPLANE_LAYER)
        {
            InGameEventHandler.CollectibleAcquired?.Invoke(this);
            gameObject.SetActive(false);
        }
    }
}