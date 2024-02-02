using System;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    internal Action<Collectible> OnCollected;
    private const string AIRPLANE_LAYER = "AirPlane";

    private void OnCollisionEnter(Collision collision)
    {
        var collidingLayer = LayerMask.LayerToName(collision.collider.gameObject.layer);
        Debug.Log(collidingLayer);
        if (collidingLayer == AIRPLANE_LAYER)
        {
            OnCollected?.Invoke(this);
        }
    }
}