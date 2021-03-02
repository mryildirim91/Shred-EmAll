using UnityEngine;
using UnityEngine.Advertisements;
public class AdsManager : MonoBehaviour, IUnityAdsListener
{
    #if UNITY_ANDROID
        private string _storeID = "3896777";
    #elif UNITY_IPHONE
        private string _storeID = "3896776";
    #else
        private string _storeID = "unexpected_platform";
    #endif
    
    private bool _testMode = false;
    
    private string _rewardedAd = "RewardedAd";
    private string _rewardedAdType;
    private void Awake()
    {
        InitializeAds();
    }

    private void InitializeAds()
    {
        Advertisement.Initialize(_storeID, _testMode);
        Advertisement.AddListener (this);
        GameEvents.ONLevelComplete += ShowInterstitialAd;
    }
    private void ShowInterstitialAd() 
    {
        if (PlayerPrefs.GetInt("Level") % 2 == 1)
        {
            if (Advertisement.IsReady()) 
            {
                Advertisement.Show();
            }
        }
    }
    
    public void ShowRewardedAd(string adType)
    {
        _rewardedAdType = adType;
        
        if (Advertisement.IsReady(_rewardedAd)) 
        {
            Advertisement.Show(_rewardedAd);
        }
    }
    
    public void OnUnityAdsDidFinish (string placementId, ShowResult showResult) 
    {
        if (_rewardedAdType == "Revive")
        {
            switch (showResult)
            {
                case  ShowResult.Finished:
                    GameEvents.LevelContinue();
                    break;
                case ShowResult.Skipped:
                    break;
                case ShowResult.Failed:
                    break;
            }
        }
        else if (_rewardedAdType == "PowerUp")
        {
            switch (showResult)
            {
                case ShowResult.Finished:
                    GameEvents.PowerUpBuy();
                    break;
                case ShowResult.Skipped:
                    //No Reward
                    break;
                case ShowResult.Failed:
                    //No Reward
                    break;
            }
        }
        else if (_rewardedAdType == "Points")
        {
            switch (showResult)
            {
                case ShowResult.Finished:
                    GameEvents.GiveMorePoints();
                    break;
                case ShowResult.Skipped:
                    //No Reward;
                    break;
                case ShowResult.Failed:
                    //No Reward
                    break;
            }
        }
    }

    public void OnUnityAdsReady (string placementId) 
    {
    }

    public void OnUnityAdsDidError (string message) 
    {
        //GameManager.Instance.NextLevel();
    }

    public void OnUnityAdsDidStart (string placementId)
    {
        //GameManager.Instance.PauseGame();
    } 
    
    private void OnDestroy() 
    {
        GameEvents.ONLevelComplete -= ShowInterstitialAd;
        Advertisement.RemoveListener(this);
    }
}
