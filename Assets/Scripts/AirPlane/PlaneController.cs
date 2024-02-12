using FlappyPlane.Events;
using UnityEngine;
using UnityEngine.EventSystems;

namespace FlappyPlane.AirPlane
{
    public class PlaneController : MonoBehaviour
    {
        [SerializeField] private float _velocity = 2f;
        [SerializeField] private float _responsiveness = 5f;

        private Rigidbody _rb;
        private Transform _myTransform;
        private bool _isAccelerating = false;
        private float _currentFuel;
        private bool _isTouchDevice;

        private float _responseModifier
        {
            get
            {
                return _rb.mass / 10f * _responsiveness;
            }
        }

        /// <summary>
        /// Initializes the Rigidbody, current fuel value and set the gravity to false.
        /// </summary>
        private void Awake()
        {
            _rb = GetComponentInChildren<Rigidbody>();
            _myTransform = transform;
            _currentFuel = 1f;
            _rb.useGravity = false;
            _isTouchDevice = Application.platform == RuntimePlatform.Android ||
            Application.platform == RuntimePlatform.IPhonePlayer;
        }

        /// <summary>
        /// Subscribes to the necessary events when the object is enabled.
        /// </summary>
        private void OnEnable()
        {
            InGameEventHandler.StartGame += StartGame;
            InGameEventHandler.OnFuelUpdate += UpdateFuel;
        }

        /// <summary>
        /// Unsubscribes from the events when the object is disabled.
        /// </summary>
        private void OnDisable()
        {
            InGameEventHandler.StartGame -= StartGame;
            InGameEventHandler.OnFuelUpdate -= UpdateFuel;
        }

        /// <summary>
        /// Checks for user input to control the acceleration of the plane.
        /// </summary>
        private void Update()
        {
            if (!_isAccelerating)
            {
                if (_isTouchDevice && IsPointerOverUIObject(Input.GetTouch(0).position) ||
                    !_isTouchDevice && EventSystem.current.IsPointerOverGameObject())
                    return;
            }

            if (_isTouchDevice)
                HandleTouchDeviceControl();
            else
                HandleMouseControl();
        }

        /// <summary>
        /// Handles control input for touch devices.
        /// </summary>
        private void HandleTouchDeviceControl()
        {
            if (!_isAccelerating && Input.touchCount > 0)
            {
                _isAccelerating = true;
                InGameEventHandler.PointerDown?.Invoke();
            }

            if (_isAccelerating && Input.touchCount > 0)
                Accelerate();
            else if (_isAccelerating)
                StopAccelerate();
        }

        /// <summary>
        /// Handles control input for mouse.
        /// </summary>
        private void HandleMouseControl()
        {
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

        /// <summary>
        /// Accelerates the plane based on user input and fuel availability.
        /// </summary>
        private void Accelerate()
        {
            if (_currentFuel <= 0)
                return;

            _isAccelerating = true;
            _rb.velocity = Vector3.up * _velocity;
            _rb.AddTorque(_myTransform.forward * _velocity * _responseModifier);

            InGameEventHandler.Accelerate?.Invoke();
        }

        /// <summary>
        /// Stops the plane from accelerating.
        /// </summary>
        private void StopAccelerate()
        {
            _isAccelerating = false;
            InGameEventHandler.PointerUp?.Invoke();
        }

        /// <summary>
        /// Updates the current fuel value.
        /// </summary>
        /// <param name="fuelValue">The updated fuel value.</param>
        private void UpdateFuel(float fuelValue)
        {
            _currentFuel = fuelValue;
        }

        /// <summary>
        /// Resets the current fuel value when the game starts and set the Gravity to True.
        /// </summary>
        private void StartGame()
        {
            _currentFuel = 1f;
            _rb.useGravity = true;
        }

        /// <summary>
        /// Function to check if the touch position is over any UI element.
        /// </summary>
        /// <param name="touchPosition">The position of the touch.</param>
        /// <returns>True if the touch is over a UI element, false otherwise.</returns>
        private bool IsPointerOverUIObject(Vector2 touchPosition)
        {
            EventSystem eventSystem = EventSystem.current;
            PointerEventData eventDataCurrentPosition = new PointerEventData(eventSystem);
            eventDataCurrentPosition.position = touchPosition;
            System.Collections.Generic.List<RaycastResult> results = new System.Collections.Generic.List<RaycastResult>();
            eventSystem.RaycastAll(eventDataCurrentPosition, results);
            return results.Count > 0;
        }
    }
}