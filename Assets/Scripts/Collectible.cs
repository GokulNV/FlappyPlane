using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField] private CollectibleType type;
    public CollectibleType Type => type;

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