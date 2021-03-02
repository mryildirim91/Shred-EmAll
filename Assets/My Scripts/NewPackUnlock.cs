using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class NewPackUnlock : MonoBehaviour
{
    [Serializable]
    private struct Packs
    {
        public string packName;
        public GameObject packObj;
    }
    
    [Serializable]
    private struct Buttons
    {
        public int price;
        public Text text;
        public Button button;
    }

    private int _currentPack;
    [SerializeField] private Button _buyButton, _selectButton;
    [SerializeField] private Text _priceTag;
    [SerializeField] private GameObject _coinImage;
    [SerializeField] private Packs[] _packs;
    [SerializeField] private Buttons[] _buttons;
    private void Awake()
    {
        AvailablePack();
        _priceTag.text = "300";
        _selectButton.gameObject.SetActive(false);
        _buyButton.gameObject.SetActive(false);
        _currentPack = PlayerPrefs.GetInt("CurrentPack");
        
        for (int i = 0; i < _packs.Length; i++)
        {
            if (i == _currentPack)
            {
                _packs[_currentPack].packObj.SetActive(true);
            }
            else
            {
                _packs[i].packObj.SetActive(false);
            }
        }
    }

    private void OnEnable()
    {
        GameEvents.ONStartGame += PackPlayTimeIncrease;
    }

    private void OnDisable()
    {
        GameEvents.ONStartGame -= PackPlayTimeIncrease;
    }

    private void AvailablePack()
    {
        if (!PlayerPrefs.HasKey("PackName" + 0))
        {
            PlayerPrefs.SetInt("PackName" + 0, 0);
        }
        
        for (int i = 0; i < _buttons.Length; i++)
        {
            if (!PlayerPrefs.HasKey("PackName" + i) && PlayerPrefs.GetInt("Points") < _buttons[i].price) 
            {
                _buttons[i].button.interactable = false;
                _buttons[i].button.image.DOFade(_buttons[i].button.image, 0.5f, 0.1f);
                _buttons[i].text.text = "";
            }
            else if(PlayerPrefs.HasKey("PackName" + i))
            {
                _buttons[i].text.text = PlayerPrefs.GetInt("PlayTime" + i) + " Time(s) Played";
            }
        }
    }

    private void PackPlayTimeIncrease()
    {
        PlayerPrefs.SetInt("PlayTime" + _currentPack, PlayerPrefs.GetInt("PlayTime" + _currentPack) + 1);
    }
    
    public void PickPack(int index)
    {
        _currentPack = index;

        if (!PlayerPrefs.HasKey("PackName" + index) && index > 0)
        {
            _priceTag.text = _buttons[_currentPack].price.ToString();
            _selectButton.gameObject.SetActive(false);
            _buyButton.gameObject.SetActive(true);
            _coinImage.SetActive(true);
        }
        else
        {
            _priceTag.text = "";
            _selectButton.gameObject.SetActive(true);
            _buyButton.gameObject.SetActive(false);
            _coinImage.SetActive(false);
        }
    }

    public void SelectPack()
    {
        for (int i = 0; i < _packs.Length; i++)
        {
            if (i == _currentPack)
            {
                _packs[_currentPack].packObj.SetActive(true);
                PlayerPrefs.SetInt("CurrentPack", _currentPack);
            }
            else
            {
                _packs[i].packObj.SetActive(false);
            }
        }
    }

    public void BuyPack()
    {
        if (!PlayerPrefs.HasKey("PackName" + _currentPack))
        {
            _priceTag.text = "";
            _coinImage.SetActive(false);
            _selectButton.gameObject.SetActive(true);
            _buyButton.gameObject.SetActive(false);
            PlayerPrefs.SetString("PackName" + _currentPack, _packs[_currentPack].packName);
            PlayerPrefs.SetInt("PlayTime" + _currentPack, 0);
            _buttons[_currentPack].text.text = PlayerPrefs.GetInt("PlayTime") + " Time(s) Played";
            GameEvents.BuyItem(_buttons[_currentPack].price);
        }
        
        for (int i = 0; i < _packs.Length; i++)
        {
            if (PlayerPrefs.GetInt("Points") < _buttons[i].price && !PlayerPrefs.HasKey("PackName" + i))
            {
                _buttons[i].button.interactable = false;
            }
            
            if (i == _currentPack)
            {
                _packs[_currentPack].packObj.SetActive(true);
                PlayerPrefs.SetInt("CurrentPack", _currentPack);
            }
            else
            {
                _packs[i].packObj.SetActive(false);
            }
        }
    }
}
