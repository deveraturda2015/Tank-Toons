using UnityEngine;
using GoogleMobileAds.Api;
using System;
using UnityEngine.UI;
using System.Collections;
using GoogleMobileAds.Common;
using GoogleMobileAds.Ump.Api;
using System.Collections.Generic;

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

  //  public Text testTxt;

    public static AdmobManager instance = null;   
   
    [Header("Admob Interstitial")]
    public string adMobAppInterstitialIOS;
    public string adMobAppInterstitialAndroid;

    [Header("Admob Reward")]
    public string adMobAppRewardIOS;
    public string adMobAppRewardAndroid;

    [Header("Admob Banner")]
    public string adMobBannerIOS;
    public string adMobBannerAndroid;

    [Header("Admob Open Ad")]
    public string adMobOpenAdIOS;
    public string adMobOpenAdAndroid;   

    [Space(10)]
    public bool ShowTestAds;
    public bool AdsEnabled;
    public bool ShouldShowBanner;
    public bool RewardedAdEnabled;
    public bool OpenAdEnabled;
    public bool HasAdmobFormConsent;

    public static bool RewardAdAvailable;

    private InterstitialAd _interstitial;
    private RewardedAd _rewardBasedVideo;
    private BannerView bannerView;
    private AppOpenAd appOpenAd;

    private Action _RewardVideoCallback;

    private bool _live = false;
    private float timer = 0;
    private System.DateTime _expireTime;

    protected const string TestInsterstitialID = "ca-app-pub-3940256099942544/1033173712";
    protected const string TestBannerID = "ca-app-pub-3940256099942544/6300978111";
    protected const string TestRewardedVideoID = "ca-app-pub-3940256099942544/5224354917";
    protected const string TestOpenAdID = "ca-app-pub-3940256099942544/3419835294";

    protected const string IOSTestInsterstitialID = "ca-app-pub-3940256099942544/4411468910";
    protected const string IOSTestBannerID = "ca-app-pub-3940256099942544/2934735716";
    protected const string IOSTestRewardedVideoID = "ca-app-pub-3940256099942544/1712485313";
    protected const string IOSTestOpenAdID = "ca-app-pub-3940256099942544/5662855259";

    public static bool adsremoved;

    public bool IsOpenAdAvailable
    {
        get
        {
            return appOpenAd != null
                   && appOpenAd.CanShowAd()
                   && DateTime.Now < _expireTime;
        }
    }

    void Awake()
    {
        if (instance == null)
        {
            _live = true;
            DontDestroyOnLoad(gameObject);
            instance = this;

            //UNCOMMENT THIS IF YOU WANT TO DEBUG THE CONSENT UI
            //var debugSettings = new ConsentDebugSettings
            //{
            //    // Geography appears as in EEA for debug devices.
            //    DebugGeography = DebugGeography.EEA,
            //    TestDeviceHashedIds = new List<string>
            //    {
            //     "638FA9E50919A6DDA579F0AD11C39841"
            //    }
            //};

            if (AdsEnabled)
            {
                if (HasAdmobFormConsent)
                {
                    // Set tag for under age of consent.
                    // Here false means users are not under age of consent.
                    ConsentRequestParameters request = new ConsentRequestParameters
                    {
                        TagForUnderAgeOfConsent = false,
                        // ConsentDebugSettings = debugSettings,
                    };

                    // Check the current consent information status.
                    ConsentInformation.Update(request, OnConsentInfoUpdated);
                }
                else
                {
                    Loaded();
                }

            }
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        AppStateEventNotifier.AppStateChanged += OnAppStateChanged;

    }
   

    void OnDestroy()
    {
        if (!_live)
            return;

        HideInterstitialAd();
        AppStateEventNotifier.AppStateChanged -= OnAppStateChanged;

    }

    void OnConsentInfoUpdated(FormError consentError)
    {
        if (consentError != null)
        {
            // Handle the error.
            UnityEngine.Debug.LogError(consentError);
            return;
        }
        // If the error is null, the consent information state was updated.
        // You are now ready to check if a form is available.
        ConsentForm.LoadAndShowConsentFormIfRequired((FormError formError) =>
        {
            if (formError != null)
            {
                // Consent gathering failed.
                UnityEngine.Debug.LogError(consentError);
                return;
            }

            // Consent has been gathered.
            if (ConsentInformation.CanRequestAds())
            {
                Debug.Log("!@#CONSENT HAS BEEN GATHERED. MAY NOW LOAD ADS");
                Loaded();
            }
        });


    }
    private void Loaded()
    {
        Debug.Log("$#@Initializing");

        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            Debug.Log("$#@INITIALIZED COMPLETE");
            if (!adsremoved)
            {
                StartCoroutine(AdLoadDelay());
            }
            // This callback is called once the MobileAds SDK is initialized.
        });

        MobileAds.SetiOSAppPauseOnBackground(false);
    }

    IEnumerator AdLoadDelay()
    {
        yield return new WaitForSeconds(1);
        if (ShouldShowBanner)
        {
            RequestBanner();
        }
       
        yield return new WaitForSeconds(1);
        if (RewardedAdEnabled)
        {
           RequestRewardBasedVideo();
        }
        yield return new WaitForSeconds(1);
        LoadInterstitialAd();
        yield return new WaitForSeconds(1);
        if (OpenAdEnabled)
        {
            LoadAppOpenAd();
        }
    }

    /// <summary>
    /// Loads the app open ad.
    /// </summary>
    public void LoadAppOpenAd()
    {
        string adUnitId;
        if (!ShowTestAds)
        {
#if UNITY_ANDROID
            adUnitId = adMobOpenAdAndroid;
#elif UNITY_IPHONE
        adUnitId = adMobOpenAdIOS;
#endif
        }
        else
        {
#if UNITY_ANDROID
            adUnitId = TestOpenAdID;
#elif UNITY_IPHONE
        adUnitId = IOSTestOpenAdID;
#endif
        }

        // Clean up the old ad before loading a new one.
        if (appOpenAd != null)
        {
            appOpenAd.Destroy();
            appOpenAd = null;
        }

        Debug.Log("Loading the app open ad.");

        // Create our request used to load the ad.
        var adRequest = CreateAdRequest();

        // send the request to load the ad.
        AppOpenAd.Load(adMobOpenAdIOS, adRequest,
           (AppOpenAd ad, LoadAdError error) =>
           {
               // if error is not null, the load r            if (error != null || ad == null)
               {
                   Debug.LogError("app open ad failed to load an ad " +
                                  "with error : " + error);
                   return;
               }

               Debug.Log("App open ad loaded with response : "
                         + ad.GetResponseInfo());

               appOpenAd = ad;
               RegisterEventHandlers(ad);
           });
    }

    /// <summary>
    /// Shows the app open ad.
    /// </summary>
    public void ShowAppOpenAd()
    {
        if (appOpenAd != null && appOpenAd.CanShowAd())
        {
            Debug.Log("Showing app open ad.");
            appOpenAd.Show();
        }
        else
        {
            Debug.LogError("App open ad is not ready yet.");
        }
    }

    private void OnAppStateChanged(AppState state)
    {
        Debug.Log("App State changed to : " + state);

        // if the app is Foregrounded and the ad is available, show it.
        if (state == AppState.Foreground)
        {
            if (IsOpenAdAvailable)
            {
                ShowAppOpenAd();
            }
        }
    }


    public void RequestRewardBasedVideo()
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
#if UNITY_ANDROID
            adUnitId = TestRewardedVideoID;
#elif UNITY_IPHONE
        adUnitId = IOSTestRewardedVideoID;
#endif
        }

        Debug.Log("!@#$ REQUESTING REWARDED VIDEO ADS");


        // Clean up the old ad before loading a new one.
        if (_rewardBasedVideo != null)
        {
            _rewardBasedVideo.Destroy();
            _rewardBasedVideo = null;
        }

        Debug.Log("Loading the rewarded ad.");

        // create our request used to load the ad.
        var adRequest = CreateAdRequest();

        // send the request to load the ad.
        RewardedAd.Load(adUnitId, adRequest,
            (RewardedAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {

                    Debug.LogError("Rewarded ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("Rewarded ad loaded with response : "
                          + ad.GetResponseInfo());
                RewardAdAvailable = true;
                _rewardBasedVideo = ad;
            });
    }

    public void ShowInterstitialAd()
    {
        if (!adsremoved)
        { 
        StartCoroutine(DelayLoadInterstitial());

        if (_interstitial != null && _interstitial.CanShowAd())
        {
            Debug.Log("!@#Showing interstitial ad.");
            _interstitial.Show();
        }
        else
        {
            Debug.LogError("!@#Interstitial ad is not ready yet.");
        }
        }
    }

    IEnumerator DelayLoadInterstitial()
    {
        yield return new WaitForSeconds(3);
        LoadInterstitialAd();
    }

    public void HideInterstitialAd()
    {
#if UNITY_IOS || UNITY_ANDROID
        if (_interstitial != null)
            _interstitial.Destroy();
#endif
    }
    
    /// <summary>
    /// Loads the interstitial ad.
    /// </summary>
    public void LoadInterstitialAd()
    {
        string adUnitId;

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
#if UNITY_ANDROID
            adUnitId = TestInsterstitialID;
#elif UNITY_IPHONE
        adUnitId = IOSTestInsterstitialID;
#endif
        }

        // Clean up the old ad before loading a new one.
        if (_interstitial != null)
        {
            _interstitial.Destroy();
            _interstitial = null;
        }

        Debug.Log("!@#Loading the interstitial ad.");
        // create our request used to load the ad.
        var adRequest = CreateAdRequest();

        // send the request to load the ad.
        InterstitialAd.Load(adUnitId, adRequest,
            (InterstitialAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {

                    Debug.LogError("!@#interstitial ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }
                Debug.Log("!@#Interstitial ad loaded with response : "
                          + ad.GetResponseInfo());

                _interstitial = ad;
            });

    }


    private AdRequest CreateAdRequest()
    {
        return new AdRequest();
    }   

    private void RequestBanner()
    {
        if (!adsremoved)
        {
            string adUnitId;

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
#if UNITY_ANDROID
                adUnitId = TestBannerID;
#elif UNITY_IPHONE
        adUnitId = IOSTestBannerID;
#endif
            }

            // Create a 320x50 banner at the top of the screen.
            this.bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Top);

            AdRequest request = CreateAdRequest();
            bannerView.LoadAd(request);
        }
    }

    public void ShowBanner()
    {
        StartCoroutine(ChangeBannerStatus(false));
    }

    public void HideBanner()
    {
        StartCoroutine(ChangeBannerStatus(true));
    }

    IEnumerator ChangeBannerStatus(bool hide)
    {
        if(hide)
        {
            yield return new WaitForSeconds(0.2f);
            Debug.Log("BANNER HIDE");

            if (bannerView !=null)
            {
                bannerView.Hide();
            }
                 
        }
        else
        {
            yield return new WaitForSeconds(0.2f);
            Debug.Log("BANNER SHOW");
            if (bannerView != null)
            {
                bannerView.Show();
            }
                    
        }
    }

    public void ShowRewardedAd(Action callback = null)
    {
        _RewardVideoCallback = callback;

        if (_rewardBasedVideo != null && _rewardBasedVideo.CanShowAd())
        {
            _rewardBasedVideo.Show((Reward reward) =>
            {
                RewardAdAvailable = false;
                // TODO: Reward the user.
                if (_RewardVideoCallback != null)
                {
                    _RewardVideoCallback();
                    _RewardVideoCallback = null;
                }

                RequestRewardBasedVideo();
            });
        }
    }

    //Interstitial
    private void RegisterEventHandlers(InterstitialAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Interstitial ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Interstitial ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
            Debug.Log("Interstitial ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Interstitial ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Interstitial ad full screen content closed.");
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Interstitial ad failed to open full screen content " +
                           "with error : " + error);
        };
    }

    // Reward
    private void RegisterEventHandlers(RewardedAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Rewarded ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Rewarded ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
            Debug.Log("Rewarded ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Rewarded ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Rewarded ad full screen content closed.");
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Rewarded ad failed to open full screen content " +
                           "with error : " + error);
        };
    }

    private void RegisterEventHandlers(AppOpenAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("App open ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("App open ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
            Debug.Log("App open ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("App open ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("App open ad full screen content closed.");

            LoadAppOpenAd();
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("App open ad failed to open full screen content " +
                           "with error : " + error);
        };
    }


}
