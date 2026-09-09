using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GamePanel : BasePanel
{
    public Button btnPause;
    public TextMeshProUGUI best;           // 最佳分数显示
    public TextMeshProUGUI currentScore;   // 当前分数显示
    public TextMeshProUGUI currentLevel;   // 当前关卡显示
    public TextMeshProUGUI currentTime;    // 当前时间显示
    public Button btnSetting;
    public Button btnBack;           
    //public TextMeshProUGUI txtStamina;     // 体力显示
    public TextMeshProUGUI totalScore;     // 金币显示
    private int bestScore;
    private float remainingTime;

    protected override void Awake()
    {
        base.Awake();
        // 监听场景切换事件
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        // 取消监听场景切换事件
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 当离开 SceneGame 场景时，自动隐藏 GamePanel
        if (scene.name != "SceneGame")
        {
            UIManager.Instance.HidePanel<GamePanel>(false);
        }
    }

    public override void Init()
    {
        // 注册到UIManager，使其可以被HideAllPanels管理
        UIManager.Instance.RegisterPanel(this);
        btnPause.onClick.RemoveAllListeners();
        btnPause.onClick.AddListener(() =>
        {
            UIManager.Instance.ShowPanel<PausePanel>();
            Time.timeScale = 0f;
        });
        // 设置
        btnSetting.onClick.RemoveAllListeners();
        btnSetting.onClick.AddListener(() =>
        {
            UIManager.Instance.ShowPanel<SettingPanel>();
        });
        // 返回上一界面按钮（返回LevelScene场景）
        btnBack.onClick.RemoveAllListeners();
        btnBack.onClick.AddListener(() =>
        {
            // 恢复游戏时间
            Time.timeScale = 1f;
            // 隐藏所有面板（包括当前GamePanel）
            UIManager.Instance.HideAllPanels();
            // 加载关卡选择场景
            UnityEngine.SceneManagement.SceneManager.LoadScene("LevelScene");
        });
        // 加载最佳分数
        bestScore = GameDataMgr.Instance.scoreData.maxScore;
        best.text = "Best: " + bestScore;

        // 初始化当前分数
        if (currentScore != null)
        {
            currentScore.text = "Score: " + GameDataMgr.Instance.labScore;
        }

        // 初始化当前关卡
        if (currentLevel != null)
        {
            UpdateCurrentLevelDisplay();
        }

        // 初始化倒计时时间
        int currentLevelId = GameDataMgr.Instance.playerData.level;
        LevelData levelData = GameLevelMgr.Instance.GetLevel(currentLevelId);
        if (levelData != null && currentTime != null)
        {
            remainingTime = levelData.timeLimit;
            UpdateTimeDisplay();
        }

        // 初始化体力显示
       

        // 初始化金币显示
        UpdateTotalScoreDisplay();
    }

    private void Update()
    {
        // 只有在游戏场景中才执行倒计时逻辑
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "SceneGame")
        {
            return;
        }
        
        // 更新倒计时
        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            if (remainingTime < 0)
            {
                remainingTime = 0;
                // 时间到，游戏结束
                ShowGameOver();
            }
            UpdateTimeDisplay();
        }
        
        // 更新当前关卡显示（确保关卡切换时正确显示）
        UpdateCurrentLevelDisplay();
    }

    private void UpdateTimeDisplay()
    {
        if (currentTime != null)
        {
            int minutes = Mathf.FloorToInt(remainingTime / 60);
            int seconds = Mathf.FloorToInt(remainingTime % 60);
            currentTime.text = string.Format("Time: {0:00}:{1:00}", minutes, seconds);
        }
    }

    // 重置游戏时间
    public void ResetGameTime()
    {
        int currentLevelId = GameDataMgr.Instance.playerData.level;
        LevelData levelData = GameLevelMgr.Instance.GetLevel(currentLevelId);
        if (levelData != null)
        {
            remainingTime = levelData.timeLimit;
            UpdateTimeDisplay();
        }
    }

    // 更新当前分数
    public void UpdateScore(int score)
    {
        if (currentScore != null)
        {
            currentScore.text = "Score: " + score;
        }
    }

    // 更新当前关卡
    public void UpdateLevel(int level)
    {
        if (currentLevel != null)
        {
            currentLevel.text = "Level: " + level;
        }
    }

    // 更新当前关卡显示（显示当前地图的关卡编号）
    public void UpdateCurrentLevelDisplay()
    {
        if (currentLevel == null) return;
        
        // 获取当前关卡的全局ID
        int currentLevelId = GameLevelMgr.Instance.currentLevelId;
        
        // 如果currentLevelId无效，使用GameDataMgr中的值
        if (currentLevelId <= 0)
        {
            currentLevelId = GameDataMgr.Instance.currentLevelId;
        }
        
        // 获取当前关卡所属的场景ID
        int sceneId = GetSceneIdByLevelId(currentLevelId);
        
        // 获取该场景的关卡范围
        var range = GameLevelMgr.Instance.GetLevelRange(sceneId);
        
        // 计算当前场景内的本地关卡编号
        int localLevel = currentLevelId - range.start + 1;
        
        // 显示格式：场景X-关卡Y
        currentLevel.text = string.Format("关卡 {0}-{1}", sceneId, localLevel);
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

    // 更新最佳分数
    public void UpdateBest(int score)
    {
        bestScore = score;
        if (best != null)
        {
            best.text = "Best: " + bestScore;
        }

        // 最佳分数已经通过GameDataMgr保存
    }

    public void ShowGameOver()
    {
        Debug.Log("游戏结束 UI");
        Time.timeScale = 0f;
        UIManager.Instance.ShowPanel<FailPanel>();
    }

   

    /// <summary>
    /// 更新金币显示
    /// </summary>
    public void UpdateTotalScoreDisplay()
    {
        if (totalScore != null)
        {
            int coin = GameDataMgr.Instance.GetCoin();
            totalScore.text = $"金币: {coin}";
        }
    }
}