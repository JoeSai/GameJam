using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    public enum GameState { WaitingStart, Playing, GameOver }
    public GameState gameState;

    [SerializeField] private GameObject player;
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
        Instantiate(player);
    }

    public void StartGame()
    {
        gameState = GameState.Playing;
        SoundManager.Instance.PlayGameStartSound();
        Instantiate(player);
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

}