using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public bool IsGameStarted { get; private set; }
    public bool IsGameOver { get; private set; }
    public bool IsLevelComplete { get; private set; }

    private float _startCountdown = 4;
    [SerializeField] private Text _countDownText;
    private void Awake()
    {
        Instance = this;
        GameEvents.ONLevelContinue += LevelContinue;
        GameEvents.ONLevelComplete += LevelComplete;
        GameEvents.ONGameOver += GameOver;
    }
    
    public void StartGame()
    {
        InvokeRepeating(nameof(StartCountDown),0,1);
    }

    private void StartCountDown()
    {
        if (_startCountdown > 0)
        {
            _startCountdown--;
            _countDownText.text = "" + (int)_startCountdown;

            if (_startCountdown <= 0)
            {
                _countDownText.text = "";
                CancelInvoke(nameof(StartCountDown));
                GameEvents.StartGame();
                IsGameStarted = true;
            }
        }
    }

    public void RestartGame()
    {
        PlayerPrefs.DeleteKey("Score");
        SceneManager.LoadSceneAsync("Game");
    }

    public void NextLevel()
    {
        SceneManager.LoadSceneAsync("Game");
    }
    
    private void LevelContinue()
    {
        IsGameOver = false;
    }
    
    private void LevelComplete()
    {
        IsLevelComplete = true;
    }
    
    private void GameOver()
    {
        IsGameOver = true;
    }
    private void OnDestroy()
    {
        GameEvents.ONLevelContinue -= LevelContinue;
        GameEvents.ONLevelComplete -= LevelComplete;
        GameEvents.ONGameOver -= GameOver;
    }
}
