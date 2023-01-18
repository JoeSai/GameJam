using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    public enum GameState { WaitingStart, Playing, GameOver }
    public GameState gameState;

    [SerializeField] private GameObject player;

    public int Score { get; set; }

    public int HighScore
    {
        get
        {
            return PlayerPrefs.GetInt("HighScore", 0);
        }
        set
        {
            PlayerPrefs.SetInt("HighScore", value);
        }
    }

    public static bool isTEST = false;
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
        gameState = GameState.WaitingStart;
    }



    public void Start()
    {

    }

    public void RestartGame()
    {
        gameState = GameState.Playing;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        if (GameObject.FindGameObjectsWithTag("Player") == null)
        {
            Instantiate(player);
        }
    }

    public void StartGame()
    {
        gameState = GameState.Playing;
        SoundManager.Instance.PlayGameStartSound();
        if (GameObject.FindGameObjectsWithTag("Player") == null)
        {
            Instantiate(player);
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public bool IsGamePlaying()
    {
        return gameState == GameState.Playing;
    }

    public void GameOver()
    {
        gameState = GameState.GameOver;
        GUIManager.Instance.OnGameOver();
        SoundManager.Instance.PlayGameOverSound();
    }

    public void GameComplete()
    {
        gameState = GameState.GameOver;
        GUIManager.Instance.OnGameComplete();
        SoundManager.Instance.PlayGameCompleteSound();
    }

    public void UpdateScore()
    {
        Score += 1;
        if (Score > HighScore)
        {
            HighScore = Score;
        }

        GUIManager.Instance.UpdateScoreUI();
    }

}