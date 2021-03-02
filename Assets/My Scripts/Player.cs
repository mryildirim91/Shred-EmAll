using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Serializable]
    private struct PowerUp
    {
        public Button button;
        public PowerUpType type;
        public GameObject powerUp;
    }
    
    private enum PowerUpType
    {
        Shield,
        Fireball
    }

    private int _currentPowerUp;
    private float _flameTimeUp = 16;
    private const int _price = 500;
    private bool _isShieldActive, _isFireBallActive;

    private ObjectPool _pool;
    private GameObject _powerUpClone;

    private float _turnSpeed = 200;
    
    [SerializeField] private PowerUp[] _powerUps;
    [SerializeField] private Text _priceTag, _countDownText;
    [SerializeField] private GameObject _coinImage;
    [SerializeField] private Button _buyButton;
    

    private void Awake()
    {
        _pool = FindObjectOfType<ObjectPool>();

        _priceTag.text = _price.ToString();
        
        for (int i = 0; i < _powerUps.Length; i++)
        {
            if (PlayerPrefs.GetInt("Points") < _price)
            {
                _powerUps[i].button.interactable = false;
                _buyButton.interactable = false;
                _powerUps[i].button.image.DOFade(_powerUps[i].button.image, 0.5f, 0.1f);
                _buyButton.image.DOFade(_buyButton.image, 0.5f, 0.1f);
            }
        }
    }
    
    private void Update()
    {
        if (GameManager.Instance.IsGameStarted && _isFireBallActive)
        {
            if (_flameTimeUp > 0)
            {
                _countDownText.text = "Flame is gone in " + (int)_flameTimeUp;
                _flameTimeUp -= Time.deltaTime;
                
                if (_flameTimeUp <= 0)
                {
                    _turnSpeed /= 2;
                    _countDownText.text = "";
                    _isFireBallActive = false;
                    CancelInvoke(nameof(Fire));
                }
            }
        }
    }

    private void OnEnable()
    {
        GameEvents.ONStartGame += PowerUpActivator;
        GameEvents.ONPowerUpBuy += BuyPowerUp;
    }

    private void OnDisable()
    {
        GameEvents.ONStartGame -= PowerUpActivator;
        GameEvents.ONPowerUpBuy -= BuyPowerUp;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Destructible"))
        {
            IDestructible destructible = other.GetComponent<IDestructible>();
            GameEvents.CollidedWithDestructibles();
            destructible?.Destruct(other.transform.position);
        }

        if (other.CompareTag("Bomb"))
        {
            AudioSource audioSource = GetComponentInParent<AudioSource>();
            audioSource.Play();

            IBomb bomb = other.GetComponent<IBomb>();
            bomb?.Bomb();

            _isFireBallActive = false;
            _countDownText.text = "";
            
            if (_isShieldActive)
            {
                _isShieldActive = false;
                _pool.ReturnGameObject(_powerUpClone);
                _turnSpeed /= 2;
            }
            else
            {
                GameEvents.GameOver();
                gameObject.SetActive(false);
            }
        }
    }

    public void RotateAround()
    {
        transform.Rotate(Vector3.back, _turnSpeed * Time.deltaTime);
    }
    
    public void SelectPowerUp(int index)
    {
        _currentPowerUp = index;
    }

    private void BuyPowerUp()
    {
        if (_powerUps[_currentPowerUp].type == PowerUpType.Shield)
        {
            Debug.Log("PowerUp Active");
            _isShieldActive = true;
        }
        else if(_powerUps[_currentPowerUp].type == PowerUpType.Fireball)
        {
            _isFireBallActive = true;
            Debug.Log("PowerUp Active");
        }
        
        for (int i = 0; i < _powerUps.Length; i++)
        {
            _powerUps[i].button.interactable = false;
            _powerUps[i].button.image.DOFade(_powerUps[i].button.image, 0.5f, 0.1f);
            _buyButton.interactable = false;
            _buyButton.image.DOFade(_buyButton.image, 0.5f, 0.1f);
        }

        _priceTag.text = "";
        _coinImage.SetActive(false);
        
        GameEvents.BuyItem(_price);
    }

    private void PowerUpActivator()
    {
        if (_isShieldActive)
        {
            _turnSpeed *= 2;
            _powerUpClone = _pool.GetObject(_powerUps[_currentPowerUp].powerUp);
            _powerUpClone.transform.position = transform.position + new Vector3(0,0,5);
            _powerUpClone.transform.parent = transform.parent;
            return;
        }

        if (_isFireBallActive)
        {
            InvokeRepeating(nameof(Fire), 0, 1);
        }
    }

    private void Fire()
    {
        _turnSpeed *= 2;
        GameObject obj = _pool.GetObject(_powerUps[_currentPowerUp].powerUp);
        obj.transform.position = transform.parent.position;
    }
}
