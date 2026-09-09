using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class BuyStaminaConfirmPanel : BasePanel
{
    public Button btnClose;
    public Button btnBuyWithCoin;
    public TextMeshProUGUI txtBuyStaminaPrice;
    public TextMeshProUGUI txtTodayBuyCount;

    protected override void Awake()
    {
        base.Awake();

        if (btnClose != null)
        {
            btnClose.onClick.RemoveAllListeners();
            btnClose.onClick.AddListener(OnCloseClick);
        }
        if (btnBuyWithCoin != null)
        {
            btnBuyWithCoin.onClick.RemoveAllListeners();
            btnBuyWithCoin.onClick.AddListener(OnBuyWithCoinClick);
        }
    }

    public override void Init()
    {
    }

    public override void ShowMe()
    {
        base.ShowMe();
        Time.timeScale = 0f;
        UpdateUI();
    }

    private void UpdateUI()
    {
        int price = GameDataMgr.Instance.GetNextBuyStaminaPrice();
        int buyCount = GameDataMgr.Instance.GetTodayBuyStaminaCount();

        if (txtBuyStaminaPrice != null)
        {
            txtBuyStaminaPrice.text = $"购买体力 ({price}金币)";
        }

        if (txtTodayBuyCount != null)
        {
            txtTodayBuyCount.text = $"今日购买数量: {buyCount}";
        }

        int coin = GameDataMgr.Instance.GetCoin();
        bool canBuy = coin >= price;
        btnBuyWithCoin.interactable = canBuy;
        
        if (btnBuyWithCoin != null)
        {
            btnBuyWithCoin.GetComponent<Image>().color = canBuy ? Color.yellow : Color.gray;
        }
    }

    void OnCloseClick()
    {
        Time.timeScale = 1f;
        // 使用 isFade = false 立即销毁面板，防止渐隐动画未完成就切换场景
        UIManager.Instance.HidePanel<BuyStaminaConfirmPanel>(false);
        UIManager.Instance.HidePanel<GamePanel>(false);
        UIManager.Instance.HidePanel<StaminaPanel>(false);
        UIManager.Instance.HidePanel<PausePanel>(false);
        UIManager.Instance.HidePanel<FailPanel>(false);
        
        // 返回关卡选择场景
        SceneManager.LoadScene("LevelScene");
    }

    void OnBuyWithCoinClick()
    {
        int currentStamina = GameDataMgr.Instance.GetCurrentStamina();
        int maxStamina = GameDataMgr.Instance.GetMaxStamina();

        if (currentStamina >= maxStamina)
        {
            Debug.Log("【体力系统】体力已满，无法购买");
            return;
        }

        bool success = GameDataMgr.Instance.BuyStaminaWithCoin();
        
        if (success)
        {
            Debug.Log("【体力系统】金币购买体力成功");

            // 隐藏所有相关面板
            UIManager.Instance.HidePanel<BuyStaminaConfirmPanel>(false);
            UIManager.Instance.HidePanel<GamePanel>(false);
            UIManager.Instance.HidePanel<StaminaPanel>(false);
            UIManager.Instance.HidePanel<PausePanel>(false);
            UIManager.Instance.HidePanel<FailPanel>(false);
            UIManager.Instance.HidePanel<ChooseLevelPanel>(false);
            // 调用 GameLevelMgr.RestartLevel 来真正重启关卡
            GameLevelMgr.Instance.RestartLevel();
           
        }
        else
        {
            Debug.Log("【体力系统】金币不足，购买失败");
        }
    }
}
