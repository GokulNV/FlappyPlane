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

    private void Awake()
    {
        CurrentFuel = 1f;
    }

    private void OnEnable()
    {
        InGameEventHandler.Accelerate += UpdateFuel;
        InGameEventHandler.CollisionWithGround += EmptyFuel;
    }

    private void OnDisable()
    {
        InGameEventHandler.Accelerate -= UpdateFuel;
        InGameEventHandler.CollisionWithGround += EmptyFuel;
    }

    private void UpdateFuel()
    {
        CurrentFuel = Mathf.Clamp(CurrentFuel - _fuelUsageFactor, 0f, 1f);
    }

    private void EmptyFuel()
    {
        CurrentFuel = 0f;
    }
}
