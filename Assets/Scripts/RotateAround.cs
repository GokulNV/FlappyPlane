using UnityEngine;

public class RotateAround : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private Transform _pivotObject;
    [SerializeField] private InputController _inputController;
    [SerializeField] private PlaneController _planeController;
    private Transform _myTransform;
    private bool _isAccelerating = false;

    private void Awake()
    {
        _myTransform = transform;
    }

    private void OnEnable()
    {
        // _inputController.PointerDown += StopRotation;
        // _inputController.PointerUp += ContinueRotation;
        _planeController.OnGameOver += StopRotation;
    }

    private void OnDisable()
    {
        // _inputController.PointerDown -= StopRotation;
        // _inputController.PointerUp -= ContinueRotation;
        _planeController.OnGameOver -= StopRotation;
    }

    private void Update()
    {
        if (!_isAccelerating)
            _myTransform.RotateAround(_pivotObject.position, -Vector3.up, _rotationSpeed * Time.deltaTime);
    }

    private void StopRotation()
    {
        _isAccelerating = true;
    }

    private void ContinueRotation()
    {
        _isAccelerating = false;
    }
}
