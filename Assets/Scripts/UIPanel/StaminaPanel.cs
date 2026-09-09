using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class StaminaPanel : BasePanel
{
    
    public Button btnWatchAd;
    public Button btnClose;
    public Button btnBuyStaminaWithCoin;  // 金币购买体力按钮
    public TextMeshProUGUI txtAdCount;
    public TextMeshProUGUI txtAdButton;
    public TextMeshProUGUI txtBuyStaminaPrice;  // 购买体力价格显示

    protected override void Awake()
    {
        base.Awake();
       
        // 在 Awake 中绑定按钮事件，确保面板显示时按钮已可响应
        if (btnClose != null)
        {
            btnClose.onClick.RemoveAllListeners();
            btnClose.onClick.AddListener(OnCloseClick);
        }
        if (btnWatchAd != null)
        {
            btnWatchAd.onClick.RemoveAllListeners();
            btnWatchAd.onClick.AddListener(OnWatchAdClick);
        }
        if (btnBuyStaminaWithCoin != null)
        {
            btnBuyStaminaWithCoin.onClick.RemoveAllListeners();
            btnBuyStaminaWithCoin.onClick.AddListener(OnBuyStaminaWithCoinClick);
        }
    }

    public override void Init()
    {
        // 按钮事件已在 Awake 中绑定，Init 保持为空
    }

    public override void ShowMe()
    {
        base.ShowMe();
        // 暂停游戏时间
        Time.timeScale = 0f;
        UpdateUI();
    }

    private void UpdateUI()
    {
        // 更新广告按钮状态
        bool canWatchAd = GameDataMgr.Instance.CanWatchAd();
        btnWatchAd.interactable = canWatchAd;

        if (canWatchAd)
        {
            int remainingCount = GameDataMgr.Instance.GetRemainingAdCount();
            txtAdCount.text = $"今日剩余恢复次数: {remainingCount}/3";
            btnWatchAd.GetComponent<Image>().color = Color.green;
        }
        else
        {
            txtAdCount.text = "今日恢复次数已达上限";
            btnWatchAd.GetComponent<Image>().color = Color.gray;
        }

        // 更新金币购买按钮状态
        UpdateBuyStaminaButton();
    }

    /// <summary>
    /// 更新金币购买体力按钮状态
    /// </summary>
    private void UpdateBuyStaminaButton()
    {
        if (btnBuyStaminaWithCoin == null || txtBuyStaminaPrice == null)
            return;

        int currentStamina = GameDataMgr.Instance.GetCurrentStamina();
        int maxStamina = GameDataMgr.Instance.GetMaxStamina();
        int price = GameDataMgr.Instance.GetNextBuyStaminaPrice();
        int coin = GameDataMgr.Instance.GetCoin();
        int buyCount = GameDataMgr.Instance.GetTodayBuyStaminaCount();

        // 体力已满时不可购买
        if (currentStamina >= maxStamina)
        {
            btnBuyStaminaWithCoin.interactable = false;
            txtBuyStaminaPrice.text = "体力已满";
            btnBuyStaminaWithCoin.GetComponent<Image>().color = Color.gray;
        }
        else
        {
            bool canBuy = coin >= price;
            btnBuyStaminaWithCoin.interactable = canBuy;
            txtBuyStaminaPrice.text = $"购买体力 ({price}金币)";
            
            if (canBuy)
            {
                btnBuyStaminaWithCoin.GetComponent<Image>().color = Color.yellow;
            }
            else
            {
                btnBuyStaminaWithCoin.GetComponent<Image>().color = Color.gray;
            }
        }
    }

    void OnWatchAdClick()
    {
        if (!GameDataMgr.Instance.CanWatchAd())
        {
            Debug.Log("【体力系统】今日恢复次数已达上限");
            return;
        }

        // 模拟广告观看（实际项目中需要调用广告SDK）
        Debug.Log("【体力系统】开始播放广告...");

        // 模拟广告播放完成（实际项目中应该在广告回调中调用）
        // 这里直接调用成功回调模拟广告观看成功
        OnAdComplete(true);
    }

    /// <summary>
    /// 金币购买体力按钮点击 - 显示确认弹窗
    /// </summary>
    void OnBuyStaminaWithCoinClick()
    {
        int currentStamina = GameDataMgr.Instance.GetCurrentStamina();
        int maxStamina = GameDataMgr.Instance.GetMaxStamina();

        // 检查体力是否已满
        if (currentStamina >= maxStamina)
        {
            Debug.Log("【体力系统】体力已满，无法购买");
            return;
        }

        // 关闭当前的StaminaPanel弹窗
        UIManager.Instance.HidePanel<StaminaPanel>();
        
        // 显示购买确认弹窗
        BuyStaminaConfirmPanel confirmPanel = UIManager.Instance.ShowPanel<BuyStaminaConfirmPanel>();
        if (confirmPanel != null)
        {
            Debug.Log("【体力系统】显示购买确认弹窗");
        }
    }

    /// <summary>
    /// 广告完成回调
    /// </summary>
    /// <param name="success">是否成功观看</param>
    void OnAdComplete(bool success)
    {
        if (success)
        {
            // 广告观看成功，恢复体力
            GameDataMgr.Instance.RestoreStaminaByAd();
            Debug.Log($"【体力系统】广告观看成功，体力+1");
        }
        else
        {
            // 广告未成功观看（用户退出或网络断开），不扣次数，不恢复体力
            GameDataMgr.Instance.CancelAd();
            Debug.Log("【体力系统】广告未完成，不恢复体力");
        }

        // 更新UI
        UpdateUI();

        // 如果体力已恢复，自动关闭面板
        if (GameDataMgr.Instance.HasEnoughStamina())
        {
            UIManager.Instance.HidePanel<StaminaPanel>();
        }
    }

    void OnCloseClick()
    {
        // 恢复游戏时间
        Time.timeScale = 1f;
        
        // 获取当前场景名称
        string currentScene = SceneManager.GetActiveScene().name;
        
        // 如果当前是游戏场景，关闭后跳转到关卡选择场景
        if (currentScene == "SceneGame")
        {
            // 隐藏体力面板和关卡面板
            UIManager.Instance.HidePanel<StaminaPanel>();
            UIManager.Instance.HidePanel<LevelPanel>();
            // 返回关卡选择场景
            SceneManager.LoadScene("LevelScene");
        }
        else
        {
            // 其他界面，只关闭体力面板即可
            UIManager.Instance.HidePanel<StaminaPanel>();
        }
    }
}