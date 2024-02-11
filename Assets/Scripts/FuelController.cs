using UnityEngine;

public class FuelController : MonoBehaviour
{
    [SerializeField] private float _fuelUsageFactor = 0.01f;

    private float _currentFuel;
    public float CurrentFuel
    {
        get => _currentFuel;
        protected set
        {
            _currentFuel = value;
            InGameEventHandler.OnFuelUpdate?.Invoke(_currentFuel);

            if (_currentFuel == 0)
                InGameEventHandler.OnGameOver?.Invoke();
        }
    }

    /// <summary>
    /// Subscribes to necessary events when the object is enabled.
    /// </summary>
    private void OnEnable()
    {
        InGameEventHandler.StartGame += RefillFuel;
        InGameEventHandler.Accelerate += UpdateFuel;
        InGameEventHandler.CollisionWithGround += EmptyFuel;
    }

    /// <summary>
    /// Unsubscribes from events when the object is disabled.
    /// </summary>
    private void OnDisable()
    {
        InGameEventHandler.StartGame -= RefillFuel;
        InGameEventHandler.Accelerate -= UpdateFuel;
        InGameEventHandler.CollisionWithGround -= EmptyFuel;
    }

    /// <summary>
    /// Updates the fuel level based on acceleration.
    /// </summary>
    private void UpdateFuel()
    {
        CurrentFuel = Mathf.Clamp(CurrentFuel - _fuelUsageFactor, 0f, 1f);
    }

    /// <summary>
    /// Empties the fuel tank when a collision with the ground occurs.
    /// </summary>
    private void EmptyFuel()
    {
        CurrentFuel = 0f;
    }

    /// <summary>
    /// Refills the fuel tank when the game starts.
    /// </summary>
    private void RefillFuel()
    {
        CurrentFuel = 1f;
    }
}