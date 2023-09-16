using UnityEngine;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    #region Fields
    public static AdsManager adM;
    [SerializeField] private bool _testMode;
    [SerializeField] private string _androidGameId;
    [SerializeField] private string _iOSGameId;
    [SerializeField] private string _androidAdUnitId = "Interstitial_Android";
    [SerializeField] private string _iOSAdUnitId = "Interstitial_iOS";
    private string _gameId;
    private string _adUnitId;
    private bool haveAd;
    #endregion

    #region Unity Methods
    private void Awake()
    {
        adM = this;
#if UNITY_IOS
            _gameId = _iOSGameId;
#elif UNITY_ANDROID
        _gameId = _androidGameId;
#elif UNITY_EDITOR
            _gameId = _androidGameId; //Only for testing the functionality in the Editor
#endif
        _adUnitId = (Application.platform == RuntimePlatform.IPhonePlayer) ? _iOSAdUnitId : _androidAdUnitId;
    }

    public void Start()
    {
        if (Advertisement.isSupported)
        {
            //Debug.Log(Application.platform + " supported by Advertisement");
        }
        Advertisement.Initialize(_gameId, _testMode, this);
    }
    #endregion

    #region Interstitial
    public void LevelCompleted() { if (haveAd) { ShowAd(); } else { LoadAd(); } }

    public void LoadAd() { Advertisement.Load(_adUnitId, this); }

    public void ShowAd() { Advertisement.Show(_adUnitId, this); }
    #endregion

    #region Callback
    public void OnInitializationComplete() { LoadAd(); }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message) { }

    public void OnUnityAdsAdLoaded(string adUnitId) { haveAd = true; }

    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message) { }

    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message) { }

    public void OnUnityAdsShowStart(string adUnitId)
    {
        haveAd = false;
        LoadAd();
    }

    public void OnUnityAdsShowClick(string adUnitId) { }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState) { }
    #endregion
}
