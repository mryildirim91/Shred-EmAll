using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BombSpawner : MonoBehaviour
{
    private int _numOfBombs;
    private bool _startGame;
    
    [SerializeField] private GameObject[] _gameObjects;
    [SerializeField] private Text _bombText;

    
    private void Awake()
    {
        if (!PlayerPrefs.HasKey("NumOfBombs"))
        {
            PlayerPrefs.SetInt("NumOfBombs", 2);
        }

        _numOfBombs = PlayerPrefs.GetInt("NumOfBombs");
        
        _bombText.text = _numOfBombs.ToString();
    }

    private void OnEnable()
    {
        GameEvents.ONStartGame += Spawn;
        GameEvents.ONLevelContinue += Spawn;
        GameEvents.ONLevelComplete += IncreaseBombNum;
    }

    private void Spawn()
    {
        StartCoroutine(SpawnRoutine());
    }
    
    private IEnumerator SpawnRoutine()
    {
        for (int i = 0; i < _numOfBombs; i++)
        {
            Vector3 pos = new Vector3(Random.Range(-ScreenArea.FitHrz, ScreenArea.FitHrz),  Random.Range(ScreenArea.FitVer + 10, ScreenArea.FitVer + 20), 0);

            GameObject obj = Instantiate(_gameObjects[Random.Range(0, _gameObjects.Length)]);
            obj.transform.position = pos;
            obj.transform.parent = transform;
            yield return BetterWaitForSeconds.Wait(0.35f);
        }
    }

    private void IncreaseBombNum()
    {
        if (PlayerPrefs.GetInt("NumOfBombs") < 4)
        {
            PlayerPrefs.SetInt("NumOfBombs", PlayerPrefs.GetInt("NumOfBombs") + 1);
        }
        else
        {
            PlayerPrefs.DeleteKey("NumOfBombs");
        }
    }

    private void OnDisable()
    {
        GameEvents.ONStartGame -= Spawn;
        GameEvents.ONLevelContinue -= Spawn;
        GameEvents.ONLevelComplete -= IncreaseBombNum;
    }
}
