using System.Collections;
using System.Diagnostics.Tracing;
using UnityEngine;

public class RotateAround : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private Transform _pivotObject;
    [SerializeField] private InputController _inputController;
    [SerializeField] private PlaneController _planeController;
    private Transform _myTransform;
    private float _initialSpeed;
    private float _speedReductionFactor = 0.05f;
    private bool _canRotate = true;
    private Coroutine _accelerateCoroutine;
    private bool _isAccelerating = false;

    private void Awake()
    {
        _canRotate = true;
        _myTransform = transform;
        _initialSpeed = _rotationSpeed;
    }

    private void OnEnable()
    {
        _inputController.PointerDown += DecreaseSpeed;
        _inputController.PointerUp += NormaliseSpeed;
        _planeController.OnGameOver += StopRotation;
    }

    private void OnDisable()
    {
        _inputController.PointerDown -= DecreaseSpeed;
        _inputController.PointerUp -= NormaliseSpeed;
        _planeController.OnGameOver -= StopRotation;
    }

    private void Update()
    {
        if (_canRotate)
            _myTransform.RotateAround(_pivotObject.position, -Vector3.up, _rotationSpeed * Time.deltaTime);
    }

    private void StopRotation()
    {
        _canRotate = false;
    }

    private void DecreaseSpeed()
    {
        if (_rotationSpeed <= _initialSpeed / 2)
            return;

        _isAccelerating = true;
        if (_accelerateCoroutine != null)
        {
            StopCoroutine(nameof(AccelerateSpeed));
            _accelerateCoroutine = null;
        }

        UpdateRotationSpeed(_rotationSpeed - _speedReductionFactor);
    }

    private void NormaliseSpeed()
    {
        _isAccelerating = false;
        _accelerateCoroutine = StartCoroutine(nameof(AccelerateSpeed));
    }

    private IEnumerator AccelerateSpeed()
    {
        while (!_isAccelerating && _rotationSpeed < _initialSpeed)
        {
            UpdateRotationSpeed(_rotationSpeed + _speedReductionFactor);
            yield return null;
        }
    }

    private void UpdateRotationSpeed(float value)
    {
        _rotationSpeed = Mathf.Clamp(value, _rotationSpeed / 2, _rotationSpeed);
    }
}
