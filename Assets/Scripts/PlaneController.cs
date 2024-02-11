using UnityEngine;
using UnityEngine.EventSystems;

public class PlaneController : MonoBehaviour
{
    [SerializeField] private float _velocity = 2f;
    [SerializeField] private float _responsiveness = 5f;

    private Rigidbody _rb;
    private Transform _myTransform;
    private bool _isAccelerating = false;
    private float _currentFuel;
    private float _responseModifier
    {
        get
        {
            return _rb.mass / 10f * _responsiveness;
        }
    }

    private void Awake()
    {
        _rb = GetComponentInChildren<Rigidbody>();
        _myTransform = transform;
        _currentFuel = 1f;
    }

    private void OnEnable()
    {
        InGameEventHandler.OnFuelUpdate += UpdateFuel;
    }

    private void OnDisable()
    {
        InGameEventHandler.OnFuelUpdate -= UpdateFuel;
    }

    private void Update()
    {
        if (!_isAccelerating && EventSystem.current.IsPointerOverGameObject())
            return;

        if (Input.GetMouseButtonDown(0))
        {
            _isAccelerating = true;
            InGameEventHandler.PointerDown?.Invoke();
        }

        if (_isAccelerating && Input.GetMouseButton(0))
            Accelerate();
        else if (_isAccelerating)
            StopAccelerate();
    }

    private void Accelerate()
    {
        if (_currentFuel <= 0)
            return;

        _isAccelerating = true;
        _rb.velocity = Vector3.up * _velocity;
        _rb.AddTorque(_myTransform.forward * _velocity * _responseModifier);

        InGameEventHandler.Accelerate?.Invoke();
    }

    private void StopAccelerate()
    {
        _isAccelerating = false;
        InGameEventHandler.PointerUp?.Invoke();
    }

    private void UpdateFuel(float fuelValue)
    {
        _currentFuel = fuelValue;
    }
}