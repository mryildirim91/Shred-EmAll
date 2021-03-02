using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    private int _score, _bestScore, _points, _currentLevel, _rndNum;
    [SerializeField] private Text _pointsText, _bestScoreText, _levelText, _morePointsText;
    [SerializeField] private GameObject _scorePanel, _gameOverPanel,_levelCompletePanel;

    private void Awake()
    {
        _rndNum = Random.Range(40, 60);
        GameEvents.ONGameOver += GameOver;
        GameEvents.ONCollidedWithDestructibles += UpdatePoints;
        GameEvents.ONCollidedWithDestructibles += UpdateScore;
        GameEvents.ONLevelComplete += LevelComplete;
        GameEvents.ONMorePoints += MorePoints;
        GameEvents.ONBuyItem += UpdateCoins;
        _scorePanel.SetActive(true);
        _score = PlayerPrefs.GetInt("Score");
        _points = PlayerPrefs.GetInt("Points");
        _bestScore = PlayerPrefs.GetInt("BestScore");
        _currentLevel = PlayerPrefs.GetInt("Level") + 1;
        _pointsText.text = PlayerPrefs.GetInt("Points").ToString();
        _bestScoreText.text = PlayerPrefs.GetInt("Score") + "\nBest " + PlayerPrefs.GetInt("BestScore");
        _levelText.text = "Level " + _currentLevel;
    }

    private void GameOver()
    {
        StartCoroutine(GameOverRoutine());
    }

    private IEnumerator GameOverRoutine()
    {
        yield return BetterWaitForSeconds.Wait(3);
        _scorePanel.SetActive(true);
        _gameOverPanel.SetActive(true);
        SetBestScore();
    }

    private void LevelComplete()
    {
        StartCoroutine(LevelCompleteRoutine());
    }

    private IEnumerator LevelCompleteRoutine()
    {
        yield return BetterWaitForSeconds.Wait(0.5f);
        _levelCompletePanel.SetActive(true);
        _scorePanel.SetActive(true);
        _levelText.text = "Level " + _currentLevel + " Cleared";
        _morePointsText.text = "Get " + _rndNum;
        PlayerPrefs.SetInt("Level", _currentLevel++);
        SetBestScore();
    }

    private void UpdatePoints()
    {
        _points++;
        _pointsText.text = _points.ToString();
        PlayerPrefs.SetInt("Points", _points);
    }

    private void UpdateScore()
    {
        _score++;
        PlayerPrefs.SetInt("Score", _score);
    }

    private void SetBestScore()
    {
        if (_score > _bestScore)
        {
            PlayerPrefs.SetInt("BestScore", _score);
            _bestScoreText.text = "New Best " + PlayerPrefs.GetInt("BestScore");
        }
        else
        {
            _bestScoreText.text = PlayerPrefs.GetInt("Score") + "\nBest " + PlayerPrefs.GetInt("BestScore");
        }
    }

    private void MorePoints()
    {
        _points += _rndNum;
        _pointsText.text = _points.ToString();
        PlayerPrefs.SetInt("Points", _points);
    }

    private void UpdateCoins(int amount)
    {
        _points -= amount;
        _pointsText.text = _points.ToString();
        PlayerPrefs.SetInt("Points", _points);
        Debug.Log(PlayerPrefs.GetInt("Points"));
    }
    
    private void OnDestroy()
    {
        GameEvents.ONGameOver -= GameOver;
        GameEvents.ONCollidedWithDestructibles -= UpdatePoints;
        GameEvents.ONCollidedWithDestructibles -= UpdateScore;
        GameEvents.ONLevelComplete -= LevelComplete;
        GameEvents.ONMorePoints -= MorePoints;
        GameEvents.ONBuyItem -= UpdateCoins;
    }
}
