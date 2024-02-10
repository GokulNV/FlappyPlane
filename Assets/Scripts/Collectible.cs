using System;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    internal Action<Collectible> OnCollected;
    private const string AIRPLANE_LAYER = "Airplane";

    private void OnTriggerEnter(Collider collider)
    {
        var collidingLayer = LayerMask.LayerToName(collider.gameObject.layer);
        Debug.Log(collidingLayer);
        if (collidingLayer == AIRPLANE_LAYER)
        {
            OnCollected?.Invoke(this);
        }
    }
}