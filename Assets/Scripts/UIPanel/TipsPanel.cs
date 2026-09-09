using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class TipsPanel : BasePanel
{
    public Button btnContinue;  // 继续游戏按钮
    public Button btnClose;     // 关闭按钮
    public TextMeshProUGUI txtTips;  // 提示文字

    protected override void Awake()
    {
        base.Awake();

        // 绑定按钮事件
        if (btnContinue != null)
        {
            btnContinue.onClick.RemoveAllListeners();
            btnContinue.onClick.AddListener(OnContinueClick);
        }
        if (btnClose != null)
        {
            btnClose.onClick.RemoveAllListeners();
            btnClose.onClick.AddListener(OnCloseClick);
        }
    }

    public override void Init()
    {
        // 按钮事件已在 Awake 中绑定
    }

    public override void ShowMe()
    {
        base.ShowMe();
        // 暂停游戏时间
        Time.timeScale = 0f;
    }

    /// <summary>
    /// 设置提示文字
    /// </summary>
    public void SetTips(string tips)
    {
        if (txtTips != null)
        {
            txtTips.text = tips;
        }
    }

    /// <summary>
    /// 继续游戏按钮点击（重新开启当前关卡）
    /// </summary>
    void OnContinueClick()
    {
        // 恢复游戏时间
        Time.timeScale = 1f;
        // 隐藏此面板
        UIManager.Instance.HidePanel<TipsPanel>();
        
        // 获取当前关卡ID
        int levelId = GameLevelMgr.Instance.currentLevelId;
        if (levelId <= 0)
        {
            levelId = GameDataMgr.Instance.currentLevelId;
        }
        
        // 重新进入当前关卡
        if (levelId > 0)
        {
            GameDataMgr.Instance.OnEnterLevel(levelId);
            Debug.Log($"【TipsPanel】重新进入关卡 {levelId}");
        }
        else
        {
            Debug.LogWarning("【TipsPanel】无法获取当前关卡ID");
        }
    }

    /// <summary>
    /// 关闭按钮点击
    /// </summary>
    void OnCloseClick()
    {
        // 恢复游戏时间
        Time.timeScale = 1f;
        // 隐藏此面板
        UIManager.Instance.HidePanel<TipsPanel>();
        Debug.Log("【TipsPanel】关闭面板");
    }
}