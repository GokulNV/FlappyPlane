using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    // public static InputController Instance;
    private static InputController _instance;

    internal Action PointerDown;
    internal Action PointerUp;

    private bool _isHolding;

    private void Awake()
    {
        _instance = this;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _isHolding = true;
        StartCoroutine(nameof(InvokeAction));
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        PointerUp?.Invoke();
        _isHolding = false;
    }

    private IEnumerator InvokeAction()
    {
        while (_isHolding)
        {
            PointerDown?.Invoke();
            yield return null;
        }
    }
}