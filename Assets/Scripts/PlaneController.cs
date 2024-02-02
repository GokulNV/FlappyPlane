using System;
using UnityEngine;
using UnityEngine.UI;

public class PlaneController : MonoBehaviour
{
    [SerializeField] private float _velocity = 2f;
    [SerializeField] private float _rotationSpeed = 10f;
    [SerializeField] private float _fuelUsageFactor = 0.01f;
    [SerializeField] private float _responsiveness = 5f;
    [SerializeField] private InputController _inputController;
    [SerializeField] private Image _fuelImage;
    [SerializeField] private LayerMask _groundLayer;

    private Rigidbody _rb;
    private Transform _myTransform;
    private float _currentFuel;
    private float _pitch;
    private bool _isAccelerating;
    private const string GROUND_LAYER = "Ground";

    private float _responseModifier
    {
        get
        {
            return _rb.mass / 10f * _responsiveness;
        }
    }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _myTransform = transform;
        _currentFuel = 1f;
    }

    private void OnEnable()
    {
        _inputController.PointerDown += Accelerate;
        _inputController.PointerUp += StopAccelerate;
    }

    private void OnDisable()
    {
        _inputController.PointerDown -= Accelerate;
        _inputController.PointerUp -= StopAccelerate;
    }


    private void Update()
    {
        // if (_currentFuel != 0)
        //     _rb.velocity = Vector3.right * _velocity;
    }

    private void FixedUpdate()
    {
        //ToDo: Chcek not rotating
        // transform.rotation = Quaternion.Euler(0, 0, _rb.rotation.y * _rotationSpeed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        var collidingLayer = LayerMask.LayerToName(collision.collider.gameObject.layer);
        Debug.Log(collidingLayer);
        if (collidingLayer == GROUND_LAYER)
        {
            _currentFuel = 0;
            UpdateFuel(0f);
        }
    }

    private void Accelerate()
    {
        if (_currentFuel <= 0)
            return;

        _isAccelerating = true;

        _rb.velocity = Vector3.up * _velocity;
        _rb.AddTorque(_myTransform.forward * _velocity * _responseModifier);

        _currentFuel = Mathf.Clamp(_currentFuel - _fuelUsageFactor, 0f, 1f);
        UpdateFuel(_currentFuel);
    }

    private void StopAccelerate()
    {
        _isAccelerating = false;
    }

    private void UpdateFuel(float fuelValue)
    {
        _fuelImage.fillAmount = fuelValue;
    }

    [ContextMenu("Refill Fuel")]
    private void RefillFuel()
    {
        _fuelImage.fillAmount = 1f;
        _currentFuel = 1f;
    }
}