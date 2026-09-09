using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadingPanel : MonoBehaviour
{
    public Slider sliderProgress;
    public TextMeshProUGUI txtProgress;
    public TextMeshProUGUI txtTips;

    public void UpdateProgress(int progress, string tips = "游戏加载中...")
    {
        if (sliderProgress != null)
            sliderProgress.value = progress;
        if (txtProgress != null)
            txtProgress.text = (int)(Mathf.Clamp01(progress) * 100f) + "%";
        if (txtTips != null)
            txtTips.text = tips;
    }
}
