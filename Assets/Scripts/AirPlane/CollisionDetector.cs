using FlappyPlane.Constants;
using FlappyPlane.Events;
using UnityEngine;

namespace FlappyPlane.AirPlane
{
    [RequireComponent(typeof(PlaneController))]
    public class CollisionDetector : MonoBehaviour
    {
        /// <summary>
        /// Detects collisions with other objects.
        /// </summary>
        /// <param name="collision">The collision data.</param>
        private void OnCollisionEnter(Collision collision)
        {
            var collidingLayer = LayerMask.LayerToName(collision.collider.gameObject.layer);
            if (collidingLayer == GlobalConstants.GROUND_LAYER)
            {
                InGameEventHandler.CollisionWithGround?.Invoke();
            }
        }
    }
}