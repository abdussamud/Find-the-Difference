using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.Events;
using UnityEngine.UI;

public class RewardedAdsButton : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    #region Fields
    public bool skipLevel;
    public UnityEvent adsInvoke;
    [SerializeField] private Button _showAdButton;
    [SerializeField] private string _androidAdUnitId = "Rewarded_Android";
    [SerializeField] private string _iOSAdUnitId = "Rewarded_iOS";
    private string _adUnitId = null;
    #endregion

    #region Unity Methods
    private void Awake()
    {
#if UNITY_IOS
        _adUnitId = _iOSAdUnitId;
#elif UNITY_ANDROID
        _adUnitId = _androidAdUnitId;
#endif
        if (skipLevel)
        {
            _showAdButton.interactable = false;
            LoadAd();
        }
    }
    #endregion

    #region Public Methhods
    public void ShowOneHint()
    {
        if (GameplayUI.gui.hintCount <= 0) { return; }
        adsInvoke.Invoke();
        _showAdButton.interactable = false;
        LoadAd();
    }

    public void LoadAd() { Advertisement.Load(_adUnitId, this); }

    public void ShowAd()
    {
        _showAdButton.interactable = false;
        Advertisement.Show(_adUnitId, this);
    }
    #endregion

    #region Callback
    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        if (adUnitId.Equals(_adUnitId))
        {
            _showAdButton.onClick.AddListener(ShowAd);
            _showAdButton.interactable = true;
        }
    }

    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        if (adUnitId.Equals(_adUnitId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            adsInvoke.Invoke();
            LoadAd();
        }
    }

    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message) { }

    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message) { }

    public void OnUnityAdsShowStart(string adUnitId) { }

    public void OnUnityAdsShowClick(string adUnitId) { }

    private void OnDestroy() { _showAdButton.onClick.RemoveAllListeners(); }
    #endregion
}
