using System.Collections;
using System.Diagnostics.Tracing;
using UnityEngine;

public class RotateAround : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private Transform _pivotObject;
    [SerializeField] private PlaneController _planeController;
    private Transform _myTransform;
    private float _initialSpeed;
    private float _speedReductionFactor = 0.05f;
    private bool _canRotate = true;
    private Coroutine _speedUpdateCoroutine;
    private bool _isAccelerating = false;

    private void Awake()
    {
        _canRotate = true;
        _myTransform = transform;
        _initialSpeed = _rotationSpeed;
    }

    private void OnEnable()
    {
        _planeController.PointerDown += DecreaseSpeed;
        _planeController.PointerUp += NormaliseSpeed;
        _planeController.OnGameOver += StopRotation;
    }

    private void OnDisable()
    {
        _planeController.PointerDown -= DecreaseSpeed;
        _planeController.PointerUp -= NormaliseSpeed;
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
        ChangeSpeed(false);
    }

    private void NormaliseSpeed()
    {
        ChangeSpeed(true);
    }

    private void ChangeSpeed(bool isIncrease)
    {
        _isAccelerating = !isIncrease;
        if (_speedUpdateCoroutine != null)
        {
            StopCoroutine(nameof(UpdateSpeed));
            _speedUpdateCoroutine = null;
        }

        _speedUpdateCoroutine = StartCoroutine(UpdateSpeed(isIncrease));
    }

    private IEnumerator UpdateSpeed(bool isIncrease)
    {
        if(isIncrease)
        {
            while (!_isAccelerating && _rotationSpeed < _initialSpeed)
            {
                UpdateRotationValue(_rotationSpeed + _speedReductionFactor);
                yield return null;
            }
        }
        else
        {
            while (_isAccelerating && _rotationSpeed > _initialSpeed / 2)
            {
                UpdateRotationValue(_rotationSpeed - _speedReductionFactor);
                yield return null;
            }
        }
    }

    private void UpdateRotationValue(float value)
    {
        _rotationSpeed = Mathf.Clamp(value, _initialSpeed / 2, _initialSpeed);
    }
}
