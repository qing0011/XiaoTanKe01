using UnityEngine;

public class WXSDKCallbackHandler : MonoBehaviour
{
    public static WXSDKCallbackHandler Instance;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void ADOnVideoCloseCallback(string data)
    {
        Debug.Log("ADOnVideoCloseCallback received: " + data);
        RewardVideoManager.Instance?.ADOnVideoCloseCallback(data);
    }

    public void ADOnLoadCallback(string data)
    {
        Debug.Log("ADOnLoadCallback received: " + data);
        RewardVideoManager.Instance?.ADOnLoadCallback(data);
    }

    public void ADOnErrorCallback(string data)
    {
        Debug.LogError("ADOnErrorCallback received: " + data);
        RewardVideoManager.Instance?.ADOnErrorCallback(data);
    }
}