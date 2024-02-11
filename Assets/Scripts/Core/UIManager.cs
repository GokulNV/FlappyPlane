using FlappyPlane.Events;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace FlappyPlane.Core
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private TMP_Text _scoreText;
        [SerializeField] private TMP_Text _gameOverScoreText;
        [SerializeField] private Image _fuelImage;
        [SerializeField] private GameObject _gameOverScreen;

        /// <summary>
        /// Subscribes to various in-game events when the object is enabled.
        /// </summary>
        private void OnEnable()
        {
            InGameEventHandler.OnFuelUpdate += OnFuelUpdate;
            InGameEventHandler.OnGameOver += OnGameOver;
            InGameEventHandler.ScoreUpdate += UpdateScore;
        }

        /// <summary>
        /// Unsubscribes from various in-game events when the object is disabled.
        /// </summary>
        private void OnDisable()
        {
            InGameEventHandler.OnFuelUpdate -= OnFuelUpdate;
            InGameEventHandler.OnGameOver -= OnGameOver;
            InGameEventHandler.ScoreUpdate -= UpdateScore;
        }

        /// <summary>
        /// Updates the fuel UI element based on the provided value.
        /// </summary>
        /// <param name="value">The current fuel value.</param>
        private void OnFuelUpdate(float value)
        {
            _fuelImage.fillAmount = value;
        }

        /// <summary>
        /// Updates the score UI element with the provided score.
        /// </summary>
        /// <param name="score">The current score.</param>
        private void UpdateScore(int score)
        {
            _scoreText.SetText(score.ToString());
        }

        /// <summary>
        /// Handles UI updates and activation when the game is over.
        /// </summary>
        private void OnGameOver()
        {
            _gameOverScoreText.SetText("Score: " + _scoreText.text);
            _gameOverScreen.SetActive(true);
        }

        /// <summary>
        /// Handles the retry button click event by reloading the game scene.
        /// </summary>
        public void OnClickRetryButton()
        {
            SceneManager.LoadScene("GameScene");
        }
    }
}