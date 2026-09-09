using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
#if UNITY_WEBGL && !UNITY_EDITOR
using WeChatWASM;
#endif
using System.Runtime.InteropServices;

public class BeginPanel : BasePanel
{

    public Button btnSimple;
    public Button btnHard;

    public Button btnSetting;
    public Button btnSignIn;
    public Button btnGame;
    //public Button btnHome;
    public Button btnRank;
    public Button btnEmail;
    public Button btnResetStamina;  // 重置体力测试按钮
    public Button btnResetCoin;     // 重置金币测试按钮
    public Button btnResetScore;    // 重置积分测试按钮
    public Button btnBuyStamina;    // 购买体力按钮
    public TextMeshProUGUI BestLevel;
    public TextMeshProUGUI totalScore;   // 金币显示
    public TextMeshProUGUI txtStamina;   // 体力显示
    public TextMeshProUGUI txtScore;     // 积分显示

    public override void Init()
    {
        //锁定屏幕Game试图
        //Cursor.lockState = CursorLockMode.Confined;
        //简单模式
        btnSimple.onClick.AddListener(() =>
        {
            UIManager.Instance.HidePanel<BeginPanel>();
            UIManager.Instance.ShowPanel<ChooseScenePanel>();
        });
        //困难模式
        btnHard.onClick.AddListener(() =>
        {
            UIManager.Instance.HidePanel<BeginPanel>();
            UIManager.Instance.ShowPanel<ChooseScenePanel>();
        });
        //设置
        btnSetting.onClick.AddListener(() =>
        {
            UIManager.Instance.ShowPanel<SettingPanel>();
        });
        ////主页
        // btnHome.onClick.AddListener(() =>
        // {
        //     UIManager.Instance.ShowPanel<BeginPanel>();
        // });

        //排行榜
        btnRank.onClick.AddListener(() =>
        {
            UIManager.Instance.ShowPanel<RankPanel>();
            
        });
        //邮件
        btnEmail.onClick.AddListener(() =>
        {
            
             UIManager.Instance.ShowPanel<EmailPanel>();
            
        });
        // 签到
        btnSignIn.onClick.AddListener(() =>
        {
            UIManager.Instance.ShowPanel<SignInPanel>();
        });
        // 游戏圈
        btnGame.onClick.AddListener(() =>
        {
            UIManager.Instance.ShowPanel<QualPanel>();
            //Debug.Log("=== 按钮点击事件触发了 ==="); // 这个必须打印
            //Debug.Log("点击游戏圈");
            //WXPageManager pageManager = WX.CreatePageManager();
            //pageManager.Show(new ShowOption
            //{
            //    openlink = "-SSEykJvFV3pORt5kTNpS6XaS2756Ti3nY7_ReKTBQF6rzjc_rsy2-1Xi9AHrSNUKpNd8PRi-XbC7mjDaHMOSLrAuura9BnxXgwI95Pu-0O1hdkZDeZ9XKVGKKh4XxPsFmgAcBaTPKjiULw9lyFN9YTNXR8J9jxQumj0j9oFA6tdQ0VBaIsuffJ_FuoHJq7DctR8FWS03McguReVCSl3CwUV4kuq9_dKOZkmj-FPFFJC-IvT2ct2v9_AylRCINg57c4-DrPh9BXrMJwmoi7iBz8teb0JUgxJLN0KX2k8lAwEg-IUoDlkINGcFzcOQ1JSfjps9RSWq_YDW4eQ2eSAcw",
            //    //success = (res) =>
            //    //{
            //    //    Debug.Log("游戏圈打开成功");
            //    //},

            //    //fail = (err) =>
            //    //{
            //    //    Debug.LogError("游戏圈打开失败");
            //    //    Debug.LogError(JsonUtility.ToJson(err));
            //    //},

            //    //complete = (res) =>
            //    //{
            //    //    Debug.Log("游戏圈调用完成");
            //    //}
            //});
        });
        // 重置体力测试按钮
        btnResetStamina.onClick.AddListener(() =>
        {
            GameDataMgr.Instance.ResetStamina();
            UpdateStaminaDisplay();
            Debug.Log("【测试】已重置体力");
        });
        // 重置金币测试按钮
        btnResetCoin.onClick.AddListener(() =>
        {
            GameDataMgr.Instance.ResetCoin();
            UpdateTotalScoreDisplay();
            Debug.Log("【测试】已重置金币");
        });
        // 重置积分测试按钮
        btnResetScore.onClick.AddListener(() =>
        {
            GameDataMgr.Instance.ResetTotalScore();
            UpdateScoreDisplay();
            Debug.Log("【测试】已重置积分");
        });
        // 购买体力按钮
        btnBuyStamina.onClick.AddListener(() =>
        {
            UIManager.Instance.ShowPanel<StaminaPanel>();
        });
        // 时间缩放
        Time.timeScale = 1.0f;
        
        // 显示解锁的最新关卡进度
        UpdateBestLevelDisplay();
        
        // 更新体力显示
        UpdateStaminaDisplay();
        
        // 更新金币显示
        UpdateTotalScoreDisplay();
        
        // 更新积分显示
        UpdateScoreDisplay();
        
    }
    
    /// <summary>
    /// 更新体力显示
    /// </summary>
    public void UpdateStaminaDisplay()
    {
        if (txtStamina != null)
        {
            int current = GameDataMgr.Instance.GetCurrentStamina();
            int max = GameDataMgr.Instance.GetMaxStamina();
            txtStamina.text = $"{current}/{max}";
        }
    }
    
    /// <summary>
    /// 更新金币显示
    /// </summary>
    public void UpdateTotalScoreDisplay()
    {
        if (totalScore != null)
        {
            int coin = GameDataMgr.Instance.GetCoin();
            totalScore.text = $"{coin}";
        }
    }

    /// <summary>
    /// 更新积分显示
    /// </summary>
    public void UpdateScoreDisplay()
    {
        if (txtScore != null)
        {
            int score = GameDataMgr.Instance.scoreData.haveScore;
            txtScore.text = $"{score}";
        }
    }
    // 更新最佳关卡显示
    private void UpdateBestLevelDisplay()
    {
        if (BestLevel == null) return;
        
        // 获取最高解锁关卡
        int maxUnlockLevel = GameLevelMgr.Instance.saveData.maxUnlockLevel;
        
        // 检查场景数据是否正确加载
        if (GameLevelMgr.Instance.sceneList == null || GameLevelMgr.Instance.sceneList.Count == 0)
        {
            // 场景数据未加载，显示默认格式
            BestLevel.text = $"1-{maxUnlockLevel}";
            return;
        }
        
        // 计算对应的场景和本地关卡编号
        int sceneId = GetSceneIdByLevelId(maxUnlockLevel);
        var range = GameLevelMgr.Instance.GetLevelRange(sceneId);
        
        // 检查关卡范围是否有效
        if (range.start == 0 && range.end == 0)
        {
            BestLevel.text = $"1-{maxUnlockLevel}";
            return;
        }
        
        int localLevel = maxUnlockLevel - range.start + 1;
        
        // 显示格式：X-Y
        BestLevel.text = $"{sceneId}-{localLevel}";
    }
    
    // 根据关卡ID获取场景ID
    private int GetSceneIdByLevelId(int levelId)
    {
        if (GameLevelMgr.Instance.sceneList == null) return 1;
        
        int start = 1;
        foreach (var scene in GameLevelMgr.Instance.sceneList)
        {
            int end = start + scene.levelNum - 1;
            if (levelId >= start && levelId <= end)
            {
                return scene.sceneId;
            }
            start = end + 1;
        }
        return 1;
    }

}
