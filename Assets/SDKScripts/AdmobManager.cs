using UnityEngine;
using GoogleMobileAds.Api;
using System;
using UnityEngine.UI;
public class AdmobManager : MonoBehaviour
{
    /// <summary>
    /// Rewarded Ads Usage
    /// 
    ///  AdmobManager.instance.ShowRewardedAd(() =>
    //    {
    //       //Give the player a reward
    //    });
    // Insterstitial Ad Usage
    //
    //  AdmobManager.instance.ShowInterstitialAd();
    /// </summary>    

    public static AdmobManager instance = null;   

    [Header("App ID")]
    public string AndroidAppID;
    public string IOSAppID;

    [Header("Admob Interstitial")]
    public string adMobAppInterstitialIOS;
    public string adMobAppInterstitialAndroid;

    [Header("Admob Reward")]
    public string adMobAppRewardIOS;
    public string adMobAppRewardAndroid;

    [Header("Admob Banner")]
    public string adMobBannerIOS;
    public string adMobBannerAndroid;

    [Space(10)]
    public bool ShowTestAds;
    public bool AdsEnabled;
    public bool ShowBanner;
    public bool RewardedAdEnabled;

    public static bool RewardAdAvailable;
    public static bool InterstitialAvailable;

    private InterstitialAd _interstitial;
    private RewardBasedVideoAd _rewardBasedVideo;
    private BannerView bannerView;

    private Action _RewardVideoCallback;
    private Action _InterstitialCallback;

    private bool _live = false;
    private float timer = 0;

    protected const string TestInsterstitialID = "ca-app-pub-3940256099942544/1033173712";
    protected const string TestBannerID = "ca-app-pub-3940256099942544/6300978111";
    protected const string TestRewardedVideoID = "ca-app-pub-3940256099942544/5224354917";

    void Awake()
    {
        if (instance == null)
        {
            _live = true;
            DontDestroyOnLoad(gameObject);
            instance = this;

            if (AdsEnabled)
            {
                Loaded();
            }
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if(!InterstitialAvailable)
        {
            timer += Time.deltaTime;

            if(timer >=20)
            {
                timer = 0;
                RequestInterstitialAd(_interstitial);
            }
        }
    }

    void OnDestroy()
    {
        if (!_live)
            return;

        HideInterstitialAd();
    }

    private void Loaded()
    {
#if UNITY_ANDROID
        MobileAds.Initialize(AndroidAppID);
#elif UNITY_IOS
        MobileAds.Initialize(IOSAppID);
#endif

        _interstitial = CreateInterstitialAd();

          if (RewardedAdEnabled)
          {
            _rewardBasedVideo = RewardBasedVideoAd.Instance;
            _rewardBasedVideo.OnAdRewarded += HandleRewardBasedVideoRewarded;
            _rewardBasedVideo.OnAdClosed += HandleRewardBasedVideoClosed;
            _rewardBasedVideo.OnAdFailedToLoad += HandleRewardBasedVideoFailedToLoad;
            _rewardBasedVideo.OnAdLoaded += HandleRewardBasedVideoLoaded;
            _rewardBasedVideo.OnAdOpening += HandleRewardBasedVideoOpened;

            RequestRewardBasedVideo();
          }

        RequestInterstitialAd(_interstitial);


        if (ShowBanner)
        {
            RequestBanner();
        }
    }
   

    private void RequestRewardBasedVideo()
    {
        string adUnitId;
        if (!ShowTestAds)
        {
#if UNITY_ANDROID
            adUnitId = adMobAppRewardAndroid;
#elif UNITY_IPHONE
        adUnitId = adMobAppRewardIOS;
#endif
        }
        else
        {
            adUnitId = TestRewardedVideoID;
        }

        AdRequest request = CreateAdRequest();
        _rewardBasedVideo.LoadAd(request, adUnitId);
        Debug.Log("!@#$ RequestRewardBasedVideo: " + adUnitId);
    }

    private bool ShowVideoRewardAd(Action callback = null)
    {
        bool val = false;
#if UNITY_IOS || UNITY_ANDROID
        if (_rewardBasedVideo == null)
        {
            return false;
        }

        if (!_rewardBasedVideo.IsLoaded())
        {
            return false;
        }

        _RewardVideoCallback = callback;

        _rewardBasedVideo.Show();
        val = true;
#endif
        return val;
    }

    public bool ShowInterstitialAd(Action callback = null)
    {
        bool val = false;
#if UNITY_IOS || UNITY_ANDROID
        val = _interstitial.IsLoaded();
        if (val)
        {
            _interstitial.Show();
            _InterstitialCallback = callback;
        }
#endif
        return val;
    }

    public void HideInterstitialAd()
    {
#if UNITY_IOS || UNITY_ANDROID
        if (_interstitial != null)
            _interstitial.Destroy();
#endif
    }

    private void RequestInterstitialAd(InterstitialAd ad)
    {
        ad.LoadAd(CreateAdRequest());
    }

    private AdRequest CreateAdRequest()
    {
        return new AdRequest.Builder().Build();
    }

    private InterstitialAd CreateInterstitialAd()
    {
        string adUnitId = "unexpected_platform";
        if (!ShowTestAds)
        {
#if UNITY_ANDROID
        adUnitId = adMobAppInterstitialAndroid;
#elif UNITY_IPHONE
        adUnitId = adMobAppInterstitialIOS;
#endif
        }
        else
        {
            adUnitId = TestInsterstitialID;
        }

        InterstitialAd interstitial = new InterstitialAd(adUnitId);
        interstitial.OnAdFailedToLoad += HandleInterstitialAdFailedToLoad;
        interstitial.OnAdOpening += HandleInterstitialAdOnAdOpened;
        interstitial.OnAdClosed += HandleInterstitialAdOnAdClosed;
        interstitial.OnAdLoaded += HandleInsterstitialAdLoaded;

        return interstitial;
    }

    private void RequestBanner()
    {
        string adUnitId = null;

        if (!ShowTestAds)
        {
#if UNITY_ANDROID
            adUnitId = adMobBannerAndroid;
#elif UNITY_IPHONE
            adUnitId = adMobBannerIOS;
#endif
        }
        else
        {
            adUnitId = TestBannerID;
        }

        // Create a 320x50 banner at the top of the screen.
        this.bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);
    }

    public void ShowRewardedAd(Action callback = null)
    {
        if (_rewardBasedVideo.IsLoaded())
        {
            ShowVideoRewardAd(callback);
        }        
    }   

    //Insterstitial
    public void HandleInterstitialAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        Debug.Log("!@#$ HandleInterstitialAdFailedToLoad event received with message: " + args.Message);
    }

    public void HandleInterstitialAdOnAdOpened(object sender, EventArgs args)
    {
        InterstitialAvailable = false;
    }

    public void HandleInsterstitialAdLoaded(object sender,EventArgs args)
    {
        InterstitialAvailable = true;
    }

    public void HandleInterstitialAdOnAdClosed(object sender, EventArgs args)
    {
        InterstitialAvailable = false;

        if (_InterstitialCallback != null)
        {
            _InterstitialCallback();
            _InterstitialCallback = null;
        }

        _interstitial = CreateInterstitialAd();
        RequestInterstitialAd(_interstitial);
    }

    // Reward
    public void HandleRewardBasedVideoOpened(object sender, EventArgs args)
    {
        RewardAdAvailable = false;

        Debug.Log("!@#$ HandleRewardBasedVideoOpened event received: " + args);
    }

    public void HandleRewardBasedVideoClosed(object sender, EventArgs args)
    {
        RewardAdAvailable = false;

        Debug.Log("!@#$ HandleRewardBasedVideoClosed: " + args);
        RequestRewardBasedVideo();
    }

    public void HandleRewardBasedVideoRewarded(object sender, Reward args)
    {
        RewardAdAvailable = false;

        //args.Amount true reward

        Debug.Log("!@#$ HandleRewardBasedVideoRewarded: " + args);
        if (_RewardVideoCallback != null)
        {
            _RewardVideoCallback();
            _RewardVideoCallback = null;
        }
    }

    public void HandleRewardBasedVideoFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        Debug.Log("!@#$ HandleRewardBasedVideoFailedToLoad event received with message: " + args.Message);
    }

    public void HandleRewardBasedVideoLoaded(object sender, EventArgs args)
    {
        RewardAdAvailable = true;
        Debug.Log("!@#$ HandleRewardBasedVideoLoaded: " + args);
    }



}
