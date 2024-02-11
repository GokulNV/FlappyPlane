using System.Collections;
using UnityEngine;

public class RotateAround : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private Transform _pivotObject;
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
        InGameEventHandler.StartGame += StartRotation;
        InGameEventHandler.PointerDown += DecreaseSpeed;
        InGameEventHandler.PointerUp += NormaliseSpeed;
        InGameEventHandler.OnGameOver += StopRotation;
    }

    private void OnDisable()
    {
        InGameEventHandler.StartGame -= StartRotation;
        InGameEventHandler.PointerDown -= DecreaseSpeed;
        InGameEventHandler.PointerUp -= NormaliseSpeed;
        InGameEventHandler.OnGameOver -= StopRotation;
    }

    private void StartRotation()
    {
        _canRotate = true;
        StartCoroutine(nameof(RotatePlane));
    }

    private IEnumerator RotatePlane()
    {
        while (_canRotate)
        {
            _myTransform.RotateAround(_pivotObject.position, -Vector3.up, _rotationSpeed * Time.deltaTime);
            yield return null;
        }
    }

    private void StopRotation()
    {
        _canRotate = false;
        StopCoroutine(nameof(RotatePlane));
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
        if (isIncrease)
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
