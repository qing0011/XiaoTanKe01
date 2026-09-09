using UnityEngine;

public class RewardVideoManager : MonoBehaviour
{
    public static RewardVideoManager Instance;

    public string adUnitId = "adunit-73c9ccf20031a85a";

    private string adId;

    private System.Action rewardCallback;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            EnsureCallbackHandler();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void EnsureCallbackHandler()
    {
        GameObject handlerObj = GameObject.Find("WXSDKManagerHandler");
        if (handlerObj == null)
        {
            handlerObj = new GameObject("WXSDKManagerHandler");
            DontDestroyOnLoad(handlerObj);
        }

        if (handlerObj.GetComponent<WXSDKCallbackHandler>() == null)
        {
            handlerObj.AddComponent<WXSDKCallbackHandler>();
        }
    }

    public void ShowRewardVideo(System.Action reward)
    {
        rewardCallback = reward;

#if UNITY_EDITOR
        rewardCallback?.Invoke();
#elif UNITY_WEBGL && !UNITY_EDITOR
        CreateRewardAd();
#endif
    }

#if UNITY_WEBGL && !UNITY_EDITOR
    private void CreateRewardAd()
    {
        string config = "{\"adUnitId\":\"" + adUnitId + "\"}";
        adId = WeChatWASM.WX.CallJSFunctionWithReturn("WXWASMSDK", "WXCreateRewardedVideoAd", new object[] { config });
    }

    public void ShowAd()
    {
        if (!string.IsNullOrEmpty(adId))
        {
            WeChatWASM.WX.CallJSFunction("WXWASMSDK", "WXShowAd", new object[] { adId, "", "" });
        }
    }
#else
    public void ShowAd() { }
#endif

    public void ADOnVideoCloseCallback(string data)
    {
        Debug.Log("ADOnVideoCloseCallback: " + data);
        try
        {
            AdCloseResponse response = JsonUtility.FromJson<AdCloseResponse>(data);
            if (response.isEnded)
            {
                rewardCallback?.Invoke();
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("ADOnVideoCloseCallback error: " + e.Message);
        }
    }

    public void ADOnLoadCallback(string data)
    {
        Debug.Log("ADOnLoadCallback: " + data);
        ShowAd();
    }

    public void ADOnErrorCallback(string data)
    {
        Debug.LogError("ADOnErrorCallback: " + data);
    }

    [System.Serializable]
    private class AdCloseResponse
    {
        public bool isEnded;
        public string callbackId;
        public string errMsg;
    }
}