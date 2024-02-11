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

    private void Start()
    {
        InGameEventHandler.StartGame?.Invoke();
    }

    private void UpdateScore(Collectible collectible)
    {
        if (collectible.Type == CollectibleType.Star)
        {
            _score += _scoreValue;
            InGameEventHandler.ScoreUpdate(_score);
        }
    }
}
