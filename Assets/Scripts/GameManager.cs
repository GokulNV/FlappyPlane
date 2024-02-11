using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int _scoreValue = 1;
    private int _score;

    private void OnEnable()
    {
        InGameEventHandler.CollectibleAcquired += UpdateScore;
    }

    private void OnDisable()
    {
        InGameEventHandler.CollectibleAcquired -= UpdateScore;
    }

    /// <summary>
    /// Invokes the game start event when the game manager is initialized.
    /// </summary>
    private void Start()
    {
        InGameEventHandler.StartGame?.Invoke();
    }

    /// <summary>
    /// Updates the score when a collectible is acquired.
    /// </summary>
    /// <param name="collectible">The collectible that was acquired.</param>
    private void UpdateScore(Collectible collectible)
    {
        if (collectible.Type == CollectibleType.Star)
        {
            _score += _scoreValue;
            InGameEventHandler.ScoreUpdate(_score);
        }
    }
}
