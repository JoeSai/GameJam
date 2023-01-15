using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI recordText;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject gameStartPanel;
    [SerializeField] private GameObject gameCompletePanel;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button startButton;
    [SerializeField] private Button exitButton;

    private bool isClicked = false;

    private int score = 0;
    private int highScore = 0;
    
    private static GUIManager _instance;
    public static GUIManager Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        highScore = PlayerPrefs.GetInt("HighScore", 0);
        recordText.text = highScore.ToString();
    }

    public void UpdateHealthUI(int currentHealth)
    {
        healthText.text = "x" + currentHealth;
    }

    public void UpdateScoreUI()
    {
        score += 1;
        if(score > highScore)
        {
            highScore = score;
            recordText.text = highScore.ToString();
            PlayerPrefs.SetInt("HighScore", highScore);
        }
        scoreText.text = "x" + score.ToString();
    }


    public void OnGameOver()
    {
        gameOverPanel.SetActive(true);
    }

    public void OnGameComplete()
    {
        gameCompletePanel.SetActive(true);
    }
    public void OnClickExit()
    {
        if (isClicked)
        {
            return;
        }
        isClicked = true;
        SoundManager.Instance.PlayButtonClickedSound();
        StartCoroutine(RestartGame());
    }
    public void OnClickRestart()
    {
        if (isClicked)
        {
            return;
        }
        isClicked = true;
        SoundManager.Instance.PlayButtonClickedSound();
        StartCoroutine(RestartGame());
    }

    public void OnClickStart()
    {
        if (isClicked)
        {
            return;
        }
        isClicked = true;
        SoundManager.Instance.PlayButtonClickedSound();
        StartCoroutine(StartGame());
    }

    private IEnumerator StartGame()
    {
        Color flashColor = Color.white;
        Color originalColor = startButton.image.color;
        float flashDuration = 0.5f;
        float flashInterval = 0.05f;
        float elapsedTime = 0f;
        while (elapsedTime < flashDuration)
        {
            startButton.image.color = flashColor;
            yield return new WaitForSeconds(flashInterval);
            startButton.image.color = originalColor;
            yield return new WaitForSeconds(flashInterval);
            elapsedTime += flashInterval * 2;
        }
        isClicked = false;

        gameStartPanel.SetActive(false);
        GameManager.Instance.StartGame();
    }


    private IEnumerator RestartGame()
    {
        Color flashColor = Color.white;
        Color originalColor = restartButton.image.color;
        float flashDuration = 0.5f;
        float flashInterval = 0.05f;
        float elapsedTime = 0f;
        while (elapsedTime < flashDuration)
        {
            restartButton.image.color = flashColor;
            yield return new WaitForSeconds(flashInterval);
            restartButton.image.color = originalColor;
            yield return new WaitForSeconds(flashInterval);
            elapsedTime += flashInterval * 2;
        }
        isClicked = false;

        GameManager.Instance.RestartGame();
    }

    private IEnumerator ExitGame()
    {
        Color flashColor = Color.white;
        Color originalColor = exitButton.image.color;
        float flashDuration = 0.5f;
        float flashInterval = 0.05f;
        float elapsedTime = 0f;
        while (elapsedTime < flashDuration)
        {
            exitButton.image.color = flashColor;
            yield return new WaitForSeconds(flashInterval);
            exitButton.image.color = originalColor;
            yield return new WaitForSeconds(flashInterval);
            elapsedTime += flashInterval * 2;
        }
        isClicked = false;

        GameManager.Instance.ExitGame();
    }
}
