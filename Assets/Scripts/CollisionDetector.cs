using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        var collidingLayer = LayerMask.LayerToName(collision.collider.gameObject.layer);
        if (collidingLayer == GlobalConstants.GROUND_LAYER)
        {
            InGameEventHandler.CollisionWithGround?.Invoke();
        }
    }
}
