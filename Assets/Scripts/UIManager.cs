using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private TMP_Text _gameOverScoreText;
    [SerializeField] private Image _fuelImage;
    [SerializeField] private GameObject _gameOverScreen;

    private void OnEnable()
    {
        InGameEventHandler.OnFuelUpdate += OnFuelUpdate;
        InGameEventHandler.OnGameOver += OnGameOver;
        InGameEventHandler.ScoreUpdate += UpdateScore;
    }

    private void OnDisable()
    {
        InGameEventHandler.OnFuelUpdate -= OnFuelUpdate;
        InGameEventHandler.OnGameOver -= OnGameOver;
        InGameEventHandler.ScoreUpdate -= UpdateScore;
    }

    private void OnFuelUpdate(float value)
    {
        _fuelImage.fillAmount = value;
    }

    private void UpdateScore(int score)
    {
        _scoreText.SetText(score.ToString());
    }

    private void OnGameOver()
    {
        _gameOverScoreText.SetText("Score: " + _scoreText.text);
        _gameOverScreen.SetActive(true);
    }

    public void OnClickRetryButton()
    {
        SceneManager.LoadScene("GameScene");
    }
}