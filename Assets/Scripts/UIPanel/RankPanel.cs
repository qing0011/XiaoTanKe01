using System.Collections;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_WEBGL && !UNITY_EDITOR
using WeChatWASM;
#endif

public class RankPanel : BasePanel
{
    public Button btnClose;
    public Button btnGenerateTestData;
    public RawImage rawImageRank;
    public Transform rankListParent;
    public GameObject rankItemPrefab;

#if UNITY_WEBGL && !UNITY_EDITOR
    private WXOpenDataContext openDataContext;
    private Coroutine delayCoroutine;
#endif

#if UNITY_WEBGL && !UNITY_EDITOR
    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern float WX_GetDevicePixelRatio();
#endif

    public override void Init()
    {
        btnClose.onClick.AddListener(() =>
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            HideOpenData();
#endif
            UIManager.Instance.HidePanel<RankPanel>();
        });

        if (btnGenerateTestData != null)
            btnGenerateTestData.onClick.AddListener(OnGenerateTestData);

#if UNITY_WEBGL && !UNITY_EDITOR
        StartCoroutine(InitOpenData());
#else
        ShowRankList();
#endif
    }

#if UNITY_WEBGL && !UNITY_EDITOR
    private IEnumerator InitOpenData()
    {
        yield return null;

        Debug.Log("[RankPanel] InitOpenData called - getting OpenDataContext");
        openDataContext = WX.GetOpenDataContext();
        Debug.Log("[RankPanel] OpenDataContext obtained");

        if (rawImageRank != null)
        {
            rawImageRank.gameObject.SetActive(false);
        }

        yield return null;

        ShowFriendRankList();
    }

    private void ShowFriendRankList()
    {
        rawImageRank.gameObject.SetActive(true);

        if (delayCoroutine != null)
            StopCoroutine(delayCoroutine);

        delayCoroutine = StartCoroutine(DelayShow());
    }

    private IEnumerator DelayShow()
    {
        yield return new WaitForEndOfFrame();

        if (rawImageRank != null)
        {
            RectTransform rt = rawImageRank.GetComponent<RectTransform>();
            
            Vector2 size = rt.rect.size;
            int logicalWidth = Mathf.RoundToInt(size.x);
            int logicalHeight = Mathf.RoundToInt(size.y);
            
            logicalWidth = Mathf.Max(logicalWidth, 100);
            logicalHeight = Mathf.Max(logicalHeight, 100);

            float dpr = 1.0f;
#if UNITY_WEBGL && !UNITY_EDITOR
            dpr = WX_GetDevicePixelRatio();
#else
            dpr = Screen.dpi / 96f;
#endif
            dpr = Mathf.Clamp(dpr, 1f, 3f);
            
            int physicalWidth = Mathf.RoundToInt(logicalWidth * dpr);
            int physicalHeight = Mathf.RoundToInt(logicalHeight * dpr);

            Vector2 screenPos = GetScreenPosition(rt);
            int screenX = Mathf.RoundToInt(screenPos.x * dpr);
            int screenY = Mathf.RoundToInt(screenPos.y * dpr);

            Debug.Log($"[RankPanel] RawImage logical size: {logicalWidth}x{logicalHeight}, DPR: {dpr}, physical size: {physicalWidth}x{physicalHeight}, screen position: ({screenX}, {screenY})");

            if (rawImageRank.texture == null)
            {
                Debug.LogWarning("[RankPanel] Creating temporary texture");
                Texture2D tempTex = new Texture2D(physicalWidth, physicalHeight, TextureFormat.RGBA32, false);
                rawImageRank.texture = tempTex;
            }

            if (rawImageRank.texture != null)
            {
                Debug.Log($"[RankPanel] Calling WX.ShowOpenData with physical size: {physicalWidth}x{physicalHeight}, position: ({screenX}, {screenY})");
                WX.ShowOpenData(rawImageRank.texture, screenX, screenY, physicalWidth, physicalHeight);
            }
            else
            {
                Debug.LogError("[RankPanel] Failed to create valid texture!");
                yield break;
            }

            yield return new WaitForEndOfFrame();

            string message = $"{{\"type\":\"showFriendsRank\",\"width\":{physicalWidth},\"height\":{physicalHeight},\"devicePixelRatio\":{dpr}}}";
            Debug.Log($"[RankPanel] Sending showFriendsRank message with data: {message}");
            openDataContext?.PostMessage(message);
        }
        else
        {
            Debug.LogError("[RankPanel] rawImageRank is null!");
            yield break;
        }
    }

    private void HideOpenData()
    {
        openDataContext?.PostMessage("{\"type\":\"WXDestroy\"}");
        
        WX.HideOpenData();

        if (delayCoroutine != null)
        {
            StopCoroutine(delayCoroutine);
            delayCoroutine = null;
        }

        if (rawImageRank != null)
            rawImageRank.gameObject.SetActive(false);
    }
#endif

    private Vector2 GetScreenPosition(RectTransform rectTransform)
    {
        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);
        
        Camera mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Canvas canvas = rectTransform.GetComponentInParent<Canvas>();
            if (canvas != null && canvas.renderMode == RenderMode.ScreenSpaceOverlay)
            {
                return new Vector2(corners[0].x, Screen.height - corners[0].y);
            }
            return Vector2.zero;
        }

        Vector3 screenPos = mainCamera.WorldToScreenPoint(corners[0]);
        return new Vector2(screenPos.x, Screen.height - screenPos.y);
    }

    private void OnGenerateTestData()
    {
        GameDataMgr.Instance.GenerateTestRankData();
        ShowRankList();
    }

    public void ShowRankList()
    {
        foreach (Transform child in rankListParent)
            Destroy(child.gameObject);

        var data = GameDataMgr.Instance.rankData;
        if (data?.list == null) return;

        for (int i = 0; i < data.list.Count; i++)
        {
            var item = Instantiate(rankItemPrefab, rankListParent);
            var btn = item.GetComponent<btnRank>();

            if (btn != null)
            {
                btn.SetRankData(
                    i + 1,
                    data.list[i].name,
                    data.list[i].score,
                    data.list[i].bestLevel
                );
            }
        }
    }

    private void OnDestroy()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        HideOpenData();
#endif
    }
}
