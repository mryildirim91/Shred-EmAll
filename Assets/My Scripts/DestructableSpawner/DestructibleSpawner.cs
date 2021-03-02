using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public abstract class DestructibleSpawner : MonoBehaviour
{
    private int _numOfDestructible;
    private int _destructableCount;

    [SerializeField] private GameObject[] _gameObjects;
    [SerializeField] private Text[] _destructibleText;
    [SerializeField] private Image[] _image;
    [SerializeField] private Sprite _sprite;
    private void Awake()
    {
        if (!PlayerPrefs.HasKey("NumOfDestructible"))
        {
            PlayerPrefs.SetInt("NumOfDestructible", 10);
        }

        _numOfDestructible = PlayerPrefs.GetInt("NumOfDestructible");
        _destructableCount = _numOfDestructible;
    }

    private void OnEnable()
    {
        GameEvents.ONCollidedWithDestructibles += CountDownDestructibles;
        GameEvents.ONStartGame += Spawn;

        for (int i = 0; i < _image.Length; i++)
        {
            _image[i].sprite = _sprite;
            _destructibleText[i].text = _destructableCount.ToString();
        }
    }

    private void Spawn()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        for (int i = 0; i < _numOfDestructible; i++)
        {
            Vector3 pos = new Vector3(Random.Range(-ScreenArea.FitHrz, ScreenArea.FitHrz),  Random.Range(ScreenArea.FitVer + 10 , ScreenArea.FitVer + 10), 0);

            GameObject obj = Instantiate(_gameObjects[Random.Range(0, _gameObjects.Length)]);
            obj.transform.position = pos;
            obj.transform.parent = transform;

            yield return BetterWaitForSeconds.Wait(1);
        }
    }
    
    private void CountDownDestructibles()
    {
        if (_destructableCount > 0)
        {
            _destructableCount--;
            _destructibleText[0].text = _destructableCount.ToString();
            
            if (_destructableCount == 0)
            {
                GameEvents.LevelComplete();
                PlayerPrefs.SetInt("NumOfDestructible" ,PlayerPrefs.GetInt("NumOfDestructible") + Random.Range(3,8));
            }
        }
    }
    
    protected virtual void OnDisable()
    {
        GameEvents.ONCollidedWithDestructibles -= CountDownDestructibles;
        GameEvents.ONStartGame -= Spawn;
    }

}
