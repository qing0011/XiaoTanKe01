using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LoadingSceneCtrl : MonoBehaviour
{
    public Slider sliderProgress;
    public TextMeshProUGUI txtProgress;
    public TextMeshProUGUI txtTips;

    public float totalLoadingTime = 5f;
    public float delayBeforeLoad = 2f;
    public string targetSceneName = "BeginScene";

    private float currentProgress = 0f;
    private float targetProgress = 0f;
    private bool startLoading = false;

    void Start()
    {
        currentProgress = 0f;
        targetProgress = 0f;
        startLoading = false;
        UpdateUI(0f);

        if (txtTips != null)
        {
            txtTips.text = "   健康游戏忠告\n抵制不良游戏   拒绝盗版游戏\n注意自我保护   谨防受骗上当\n适度游戏益脑   沉迷游戏伤身 \n合理安排时间   享受健康生活";
        }

        StartCoroutine(LoadingCoroutine());
    }

    IEnumerator LoadingCoroutine()
    {
        float elapsedTime = 0f;
        float fakeProgress = 0f;

        while (elapsedTime < delayBeforeLoad)
        {
            elapsedTime += Time.deltaTime;
            fakeProgress = (elapsedTime / delayBeforeLoad) * 0.3f;
            UpdateUI(fakeProgress);
            yield return null;
        }

        fakeProgress = 0.3f;
        UpdateUI(fakeProgress);

        AsyncOperation asyncOp = SceneManager.LoadSceneAsync(targetSceneName);
        asyncOp.allowSceneActivation = false;

        while (!asyncOp.isDone)
        {
            float realProgress = asyncOp.progress / 0.9f;
            targetProgress = 0.3f + realProgress * 0.7f;

            while (currentProgress < targetProgress)
            {
                currentProgress += Time.deltaTime * 0.5f;
                if (currentProgress > targetProgress)
                    currentProgress = targetProgress;
                UpdateUI(currentProgress);
                yield return null;
            }

            if (asyncOp.progress >= 0.9f && currentProgress >= 0.95f)
            {
                asyncOp.allowSceneActivation = true;
            }

            yield return null;
        }
    }

    void UpdateUI(float progress)
    {
        if (sliderProgress != null)
            sliderProgress.value = progress;

        if (txtProgress != null)
            txtProgress.text = (int)(Mathf.Clamp01(progress) * 100f) + "%";
    }
}