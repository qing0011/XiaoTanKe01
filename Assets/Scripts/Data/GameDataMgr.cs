using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

/// <summary>
/// 这个是游戏数据管理类 是一个单例模式对象
/// </summary>
public class GameDataMgr
{
    private static GameDataMgr instance = new GameDataMgr();
    public static GameDataMgr Instance => instance;


    // ========= 签到存档 =========
    public SignInSaveData signInSaveData;
    //签到数据对象列表
    public List<SignInInfo> signInInfoList;
    //大奖励配置列表
    public List<BigRewardInfo> bigRewardInfoList;


    //音效数据对象
    public MusicData musicData;
    //排行榜数据对象
    public RankList rankData;
    //邮件数据对象
    public EmailList emailData;

    // 邮件数据线程安全锁
    private readonly object emailDataLock = new object();

    /// <summary>
    /// 获取邮件数据锁对象（供UI层使用）
    /// </summary>
    public object GetEmailLock()
    {
        return emailDataLock;
    }

    //  在这里添加这两行（放在邮件数据对象后面）
    private bool isEmailDataLoaded = false;
    public bool IsEmailDataLoaded => isEmailDataLoaded;

    //关卡数据对象
    public int currentSceneId = 1;
    public SceneData currentSceneData;
    public List<SceneData> sceneDataList;
    //xixixixxi
    public int currentLevelId;
    public int maxLevelId;
    public LevelData currentLevelData;
    public List<LevelData> levelDataList;

    //怪物

    public List<MonsterData> monsterDataList;

    //需要重置的数据
    public int maxHP = 100;
    public int playerHP;
    public int labScore;
    public int labTime;


    //玩家最高分记录
    public ScoreData scoreData;

    private const string SCORE_SAVE_FILE = "scoreData";
    //继续游戏基础价格
    private const int CONTINUE_BASE_COST = 10;

    //玩家成长数据
    public PlayerRuntimeData playerData = new PlayerRuntimeData();

    // 武器预制体列表
    public List<GameObject> weaponPrefabList = new List<GameObject>();

    
    //记录选择的角色数据 用于之后在游戏场景中创建
    public RoleData nowSelRole;

    //所有的角色数据
    public List<RoleData> roleDataList;
    // 添加加载完成标志
    public bool IsRoleDataLoaded { get; private set; } = false;
    private GameDataMgr()
    {
        // 初始化默认数据
        musicData = new MusicData();
        rankData = new RankList();
        emailData = new EmailList();
        sceneDataList = new List<SceneData>();
        monsterDataList = new List<MonsterData>();
        signInInfoList = new List<SignInInfo>();
        bigRewardInfoList = new List<BigRewardInfo>();
        signInSaveData = new SignInSaveData { signInCount = 0, lastSignInTime = "", currentBigRewardRound = 1, consecutiveDays = 0, bigRewardAvailable = false };
        scoreData = new ScoreData();
        
        //读取角色数据
        //roleDataList = JsonMgr.Instance.LoadData<List<RoleData>>("RoleData");
        // 添加：初始时标记为未加载
        isEmailDataLoaded = false;
        // 开始异步加载数据
        LoadAllDataAsync();

        // 初始化武器预制体列表
       // LoadWeaponPrefabs();
    }
    public void SavePlayerData()
    {
        JsonMgr.Instance.SaveData(playerData, "PlayerData");
    }
    private void LoadAllDataAsync()
    {
        // 在协程中加载所有数据
        GameObject dataLoader = new GameObject("DataLoader");
        DataLoaderComponent loader = dataLoader.AddComponent<DataLoaderComponent>();
        loader.LoadData(this);
    }

    // 数据加载组件
    private class DataLoaderComponent : MonoBehaviour
    {
        private GameDataMgr gameDataMgr;
       
        public void LoadData(GameDataMgr mgr)
        {
            gameDataMgr = mgr;
            StartCoroutine(LoadDataCoroutine());
        }

        private IEnumerator LoadDataCoroutine()
        {
            // 加载分数数据
            yield return JsonMgr.Instance.LoadDataAsync<ScoreData>(SCORE_SAVE_FILE, (data) => {
                if (data != null)
                    gameDataMgr.scoreData = data;
            });

            // 加载音乐数据
            yield return JsonMgr.Instance.LoadDataAsync<MusicData>("MusicData", (data) => {
                if (data != null)
                    gameDataMgr.musicData = data;
            });

            // 加载排行榜数据
            yield return JsonMgr.Instance.LoadDataAsync<RankList>("Rank", (data) => {
                if (data != null)
                    gameDataMgr.rankData = data;
            });

            // 加载场景数据
            yield return JsonMgr.Instance.LoadDataAsync<List<SceneData>>("SceneData", (data) => {
                if (data != null)
                    gameDataMgr.sceneDataList = data;
            });

            // 加载怪物数据
            yield return JsonMgr.Instance.LoadDataAsync<List<MonsterData>>("MonsterData", (data) => {
                if (data != null)
                    gameDataMgr.monsterDataList = data;
            });

            // 加载签到信息
            yield return JsonMgr.Instance.LoadDataAsync<List<SignInInfo>>("SignInInfo", (data) => {
                if (data != null)
                    gameDataMgr.signInInfoList = data;
            });

            // 加载签到存档
            yield return JsonMgr.Instance.LoadDataAsync<SignInSaveData>("SignInSave", (data) => {
                gameDataMgr.signInSaveData = data;
                if (gameDataMgr.signInSaveData == null)
                {
                    gameDataMgr.signInSaveData = new SignInSaveData
                    {
                        signInCount = 0,
                        lastSignInTime = "",
                        currentBigRewardRound = 1,
                        consecutiveDays = 0,
                        bigRewardAvailable = false
                    };
                    gameDataMgr.SaveSignInData();
                }
            });
            
            // 加载大奖励配置
            yield return JsonMgr.Instance.LoadDataAsync<List<BigRewardInfo>>("BigRewardInfo", (data) => {
                if (data != null)
                    gameDataMgr.bigRewardInfoList = data;
            });
            
            //邮件
            yield return JsonMgr.Instance.LoadDataAsync<EmailList>("EmailData", (data) =>
            {
                if (data != null)
                {
                    // 数据完整性检查
                    if (data.list == null)
                    {
                        data.list = new List<EmailData>();
                       // Debug.LogWarning("【邮件系统】修复空列表");
                    }
                    
                    // 验证邮件数据完整性
                    int invalidCount = 0;
                    for (int i = data.list.Count - 1; i >= 0; i--)
                    {
                        var email = data.list[i];
                        if (!gameDataMgr.IsEmailDataValid(email))
                        {
                            data.list.RemoveAt(i);
                            invalidCount++;
                        }
                    }
                    
                    if (invalidCount > 0)
                    {
                       // Debug.LogWarning($"【邮件系统】清理了 {invalidCount} 条无效邮件数据");
                    }
                    
                    gameDataMgr.emailData = data;
                }
                else
                {
                    // 如果没数据，确保邮件列表不为空
                    if (gameDataMgr.emailData == null)
                    {
                        gameDataMgr.emailData = new EmailList();
                    }
                }
                
                // 自动清理7天以上的邮件
                gameDataMgr.RemoveOldEmails(7);
                
                gameDataMgr.isEmailDataLoaded = true;  //  标记加载完成
               // Debug.Log("邮件数据加载完成");
            });
            //修改：安全的邮件数据加载

            // 加载玩家数据（体力相关）
            yield return JsonMgr.Instance.LoadDataAsync<PlayerRuntimeData>("PlayerData", (data) => {
                // 检查存档文件是否存在
                string savePath = Application.persistentDataPath + "/PlayerData.json";
                bool saveFileExists = System.IO.File.Exists(savePath);
                
                if (saveFileExists && data != null)
                {
                    // 文件存在，加载保存的数据
                    gameDataMgr.playerData = data;
                   // Debug.Log("【体力系统】玩家数据加载成功，体力: " + gameDataMgr.playerData.currentStamina + "，金币: " + gameDataMgr.playerData.coin);
                }
                else
                {
                    // 文件不存在，保持当前数据不变（不覆盖已有的金币等数据）
                   // Debug.Log("【体力系统】存档文件不存在，保持当前数据不变");
                }
                // 检查并重置体力（根据日期）
                gameDataMgr.CheckAndResetStamina();
                
                // 刷新 BeginPanel 的体力显示和金币显示
                BeginPanel beginPanel = UnityEngine.Object.FindObjectOfType<BeginPanel>();
                if (beginPanel != null)
                {
                    beginPanel.UpdateStaminaDisplay();
                    beginPanel.UpdateTotalScoreDisplay();
                    beginPanel.UpdateScoreDisplay();
                    //Debug.Log("【体力系统】已刷新 BeginPanel 显示");
                }
            });
            // 加载角色数据
            yield return JsonMgr.Instance.LoadDataAsync<List<RoleData>>("RoleData", (data) =>
            {
                if (data != null)
                {
                    gameDataMgr.roleDataList = data;
                    gameDataMgr.IsRoleDataLoaded = true;

                    //Debug.Log($"角色数据加载成功 数量:{data.Count}");
                }
                else
                {
                    Debug.LogError("RoleData 加载失败");
                }
            });

            // 数据加载完成后销毁协程运行器
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 进入新关卡时调用
    /// </summary>
    public void OnEnterLevel(int levelIndex)
    {
        // 重置当前关卡分数
        labScore = 0;
        labTime = 0;
        
        if (levelIndex == 1)
        {
            // 只在【新游戏】时重置
            playerData.level = 1;
            playerData.hp = playerData.maxHp;
            return;
        }


        // 后续关卡：只成长，不重建
        playerData.level = levelIndex;
        playerData.attack += 5;
        playerData.defense += 3;
        playerData.maxHp += 0;
        playerData.hp = playerData.maxHp;

        //恢复子弹数量（很容易玩家没有子弹）
        playerData.bulletCount = 9999;
    }

    /// <summary>
    /// 新游戏 / 返回开始界面时调用 重置
    /// </summary>
    public void ResetGameData()
    {
        //playerHP = maxHP;
        labScore = 0;
        labTime = 0;
        //重置本局继续次数
        scoreData.continueCount = 0;
        SaveScoreData();
        // 重置玩家成长数据
        playerData = new PlayerRuntimeData();
       // Debug.Log("【GameDataMgr】游戏数据已重置");
    }
    public SceneData GetCurrentSceneData()
    {
        return sceneDataList.Find(s => s.sceneId == currentSceneId);
    }

    public void SaveSignInData()
    {
        JsonMgr.Instance.SaveData(signInSaveData, "SignInSave");
    }
    //提供一些API给外部 方便数据的改变存储

    //提供一个 在排行榜中添加数据的方法
    public void SaveMusicData()
    {
        JsonMgr.Instance.SaveData(musicData, "MusicData");
    }
    public void AddRankInfo(string name, int score, float time, int bestLevel)
    {
        rankData.list.Add(new RankData(name, score, time, bestLevel));
        rankData.list.Sort((a, b) => b.bestLevel.CompareTo(a.bestLevel));
        for (int i = rankData.list.Count - 1; i >= 5; i--)
        {
            rankData.list.RemoveAt(i);
        }
        JsonMgr.Instance.SaveData(rankData, "Rank");
    }

    public void GenerateTestRankData()
    {
        rankData.list.Clear();

        string[] names = { "玩家A", "玩家B", "玩家C", "玩家D", "玩家E" };
        int[] scores = { 1500, 1300, 1200, 1100, 1000 };
        float[] times = { 45.23f, 52.67f, 58.91f, 62.34f, 68.78f };
        int[] bestLevels = { 15, 12, 10, 8, 5 };

        for (int i = 0; i < names.Length; i++)
        {
            rankData.list.Add(new RankData(names[i], scores[i], times[i], bestLevels[i]));
        }

        rankData.list.Sort((a, b) => b.bestLevel.CompareTo(a.bestLevel));
        JsonMgr.Instance.SaveData(rankData, "Rank");

        Debug.Log("【测试数据】已生成5条排行榜测试数据");
    }

    public void UploadScoreToWeChat(int score)
    {
        Debug.Log($"【微信排行榜】准备上报分数: {score}");

#if UNITY_WEBGL && !UNITY_EDITOR
    try
    {
        var openDataContext = WeChatWASM.WX.GetOpenDataContext();
        // 使用正确的 JSON 格式
        string jsonData = $"{{\"type\":\"setUserRecord\",\"score\":{score}}}";
        openDataContext.PostMessage(jsonData);
        Debug.Log($"【微信排行榜】已发送消息: {jsonData}");
    }
    catch (System.Exception e)
    {
        Debug.LogError($"【微信排行榜】上报失败: {e.Message}");
    }
#else
        Debug.Log("【微信排行榜】非WebGL环境，跳过分数上报");
#endif
    }


    public void SaveSiginInfo()
    {
        //signInInfo.Add(new SignInInfo(id, name));
    }



    /// <summary>
    /// 获取本次继续所需的积分
    /// 100 → 200 → 400 → ...
    /// </summary>
    public int GetContinueCost()
    {
        // 100 * 2^continueCount
        return CONTINUE_BASE_COST * (1 << scoreData.continueCount);
    }

    //最高积分方法

    public void SaveScoreData()
    {
        JsonMgr.Instance.SaveData(scoreData, SCORE_SAVE_FILE);
    }
    //l累计总计分（每次完成游戏后获得的积分与目前拥有的积分累加）
    public void TryRefreshTotalScoreData()
    {
        //在ScoreData里面声明了两个变量数据：haveScore：是她原本拥有的，初始化为0；
        //buyContinue：这个是购买的需要花费的钱。购买这个数据写死的话是不需要的。

        //没有购买的话则就是完成关卡的积分+原视界面有的金粉进行刷新
        //如果购买了按钮的话就会从总分里面减去购买的金币或主界面的金币进行刷新

        if (labScore <= 0)
            return;

        scoreData.haveScore += labScore;
        SaveScoreData();

        //Debug.Log($"【积分结算】本局:{labScore}  当前总积分:{scoreData.haveScore}");
    }
    //购买了继续积分（购买了按钮之后需要扣除相应的积分
    public bool BuyContinueScoreData()
    {
        //就会减去购买的钱数这里时写死的比如说购买一次100金币。
        int cost = GetContinueCost();

        if (scoreData.haveScore < cost)
        {
           // Debug.Log("【购买失败】积分不足");
            return false;
        }

        scoreData.haveScore -= cost;
        scoreData.continueCount++; // 关键点：成功才递增
        SaveScoreData();

        //Debug.Log($"【购买继续】消耗:{cost} 剩余:{scoreData.haveScore}");
        return true;
    }
    //用“本局积分”尝试刷新最高分
    public void TryRefreshMaxScore(int fistScore)
    {
        if (fistScore > scoreData.maxScore)
        {
            scoreData.maxScore = fistScore;
            SaveScoreData();
        }
    }
    /// 重置最高分
    public void ResetMaxScore()
    {
        scoreData.maxScore = 0;
        SaveScoreData();
    }
    /// 重置最高分
    public void ResetTotalScore()
    {
        scoreData.haveScore = 0;
        SaveScoreData();
    }

    /// <summary>
    /// 重置金币（测试用）
    /// </summary>
    public void ResetCoin()
    {
        playerData.coin = 0;
        SavePlayerData();
      //  Debug.Log("【测试】金币已重置");
    }

    //public void TryRefreshLevelMaxScore(int levelId, int fistScore)
    //{

    //}

   

    // ============ 金币系统 ============

    /// <summary>
    /// 获取当前金币
    /// </summary>
    public int GetCoin()
    {
        return playerData.coin;
    }

    /// <summary>
    /// 增加金币
    /// </summary>
    public void AddCoin(int amount)
    {
        if (amount <= 0) return;
        playerData.coin += amount;
        SavePlayerData();
       // Debug.Log($"【金币】+{amount}，当前: {playerData.coin}");
    }

    /// <summary>
    /// 消耗金币
    /// </summary>
    /// <returns>是否成功</returns>
    public bool SpendCoin(int amount)
    {
        if (playerData.coin < amount)
        {
           // Debug.Log($"【金币】不足，需要{amount}，当前{playerData.coin}");
            return false;
        }
        playerData.coin -= amount;
        SavePlayerData();
       // Debug.Log($"【金币】-{amount}，剩余: {playerData.coin}");
        return true;
    }

    // ============ 金币购买体力（涨价） ============

    private const int STAMINA_BUY_BASE_PRICE = 100;  // 基础价格100金币
    private const int STAMINA_BUY_AMOUNT = 1;        // 每次购买获得1体力

    /// <summary>
    /// 检查并重置今日购买次数（每天0点重置）
    /// </summary>
    private void CheckAndResetBuyStaminaCount()
    {
        string today = System.DateTime.Now.ToString("yyyy-MM-dd");
        if (playerData.lastBuyResetDate != today)
        {
            playerData.todayBuyStaminaCount = 0;
            playerData.lastBuyResetDate = today;
            SavePlayerData();
           // Debug.Log("【金币购买】今日购买次数已重置");
        }
    }

    /// <summary>
    /// 获取下一次购买体力需要多少金币（按涨价规则）
    /// 第1次：100，第2次：200，第3次：400，第4次：800 ...
    /// </summary>
    public int GetNextBuyStaminaPrice()
    {
        CheckAndResetBuyStaminaCount();
        int count = playerData.todayBuyStaminaCount;
        // 价格 = 100 * (2 ^ count)
        return STAMINA_BUY_BASE_PRICE * (1 << count);
    }

    /// <summary>
    /// 获取今日已购买次数
    /// </summary>
    public int GetTodayBuyStaminaCount()
    {
        CheckAndResetBuyStaminaCount();
        return playerData.todayBuyStaminaCount;
    }

    /// <summary>
    /// 金币购买体力（核心方法）
    /// </summary>
    /// <returns>是否购买成功</returns>
    public bool BuyStaminaWithCoin()
    {
        CheckAndResetBuyStaminaCount();

        // 1. 检查体力是否已满
        if (playerData.currentStamina >= MAX_STAMINA)
        {
           // Debug.Log("【金币购买】体力已满，无法购买");
            return false;
        }

        // 2. 计算本次价格
        int price = GetNextBuyStaminaPrice();

        // 3. 检查金币是否足够
        if (!SpendCoin(price))
        {
          //  Debug.Log($"【金币购买】金币不足，需要{price}金币");
            return false;
        }

        // 4. 增加体力（不超过上限）
        playerData.currentStamina = Mathf.Min(playerData.currentStamina + STAMINA_BUY_AMOUNT, MAX_STAMINA);

        // 5. 增加购买次数
        playerData.todayBuyStaminaCount++;

        // 6. 保存数据
        SavePlayerData();

       // Debug.Log($"【金币购买】成功！消耗{price}金币，体力+1，今日第{playerData.todayBuyStaminaCount}次购买");
        return true;
    }

    /// <summary>
    /// 关卡胜利后结算金币（在积分结算时调用）
    /// 每获得10积分 → 1金币（比例可调）
    /// </summary>
    public void SettleCoinFromScore(int stageScore)
    {
        if (stageScore <= 0) return;

        // 转换比例：10积分 = 1金币（可配置）
        int coinReward = stageScore / 10;

        if (coinReward > 0)
        {
            AddCoin(coinReward);
         //   Debug.Log($"【金币结算】本局积分{stageScore}，获得{coinReward}金币");
        }
    }

    ///邮件
    ///

    // ============ 数据完整性检查 ============

    /// <summary>
    /// 验证单封邮件数据是否有效
    /// </summary>
    private bool IsEmailDataValid(EmailData email)
    {
        if (email == null)
            return false;

        // 验证必需字段
        if (string.IsNullOrEmpty(email.id))
            return false;
        
        if (string.IsNullOrEmpty(email.title))
            return false;
        
        if (string.IsNullOrEmpty(email.content))
            return false;
        
        // 验证日期格式
        if (!string.IsNullOrEmpty(email.sendTime))
        {
            if (!System.DateTime.TryParse(email.sendTime, out _))
            {
               // Debug.LogWarning($"【邮件系统】邮件ID {email.id} 的日期格式无效: {email.sendTime}");
                return false;
            }
        }
        
        return true;
    }

    /// <summary>
    /// 验证并修复邮件列表数据完整性
    /// </summary>
    public void ValidateEmailData()
    {
        lock (emailDataLock)
        {
            if (emailData == null)
            {
                emailData = new EmailList();
               // Debug.LogWarning("【邮件系统】创建新的邮件列表");
                return;
            }
            
            if (emailData.list == null)
            {
                emailData.list = new List<EmailData>();
                Debug.LogWarning("【邮件系统】修复空的邮件列表");
                return;
            }
            
            // 验证并修复每条数据
            int invalidCount = 0;
            for (int i = emailData.list.Count - 1; i >= 0; i--)
            {
                if (!IsEmailDataValid(emailData.list[i]))
                {
                    emailData.list.RemoveAt(i);
                    invalidCount++;
                }
            }
            
            if (invalidCount > 0)
            {
               // Debug.LogWarning($"【邮件系统】清理了 {invalidCount} 条无效邮件");
                SaveEmailData();
            }
        }
    }

    // ============ 邮件相关方法 ============

    public void AddEmail(string title,string content,string rewardType, int rewardAmount)
    {
        lock (emailDataLock)
        {
            string id = Guid.NewGuid().ToString();

            EmailData email =
                new EmailData(
                    id,
                    title,
                    content,
                    rewardType,
                    rewardAmount);

            emailData.list.Add(email);

            SaveEmailData();
        }
    }

    // 确保 ClaimEmailReward 方法存在（返回bool）
    public bool ClaimEmailReward(string emailId)
    {
        lock (emailDataLock)
        {
            EmailData email =
                emailData.list.Find(e => e.id == emailId);

            if (email == null)
                return false;

            if (email.isClaimed)
                return false;

            switch (email.rewardType)
            {
                case "Coin":
                    AddCoin(email.rewardAmount);
                    break;

                case "Stamina":
                    AddStamina(email.rewardAmount);
                    break;

                case "Score":
                    scoreData.haveScore += email.rewardAmount;
                    SaveScoreData();
                    break;
            }

            email.isClaimed = true;
            email.isRead = true;

            SaveEmailData();

            return true;
        }
    }

    public void SaveEmailData()
    {
        JsonMgr.Instance.SaveData(emailData, "EmailData");
    }

    public void RemoveOldEmails(int days = 7)
    {
        lock (emailDataLock)
        {
            System.DateTime cutoffDate = System.DateTime.Now.AddDays(-days);
            emailData.list.RemoveAll(email => {
                if (System.DateTime.TryParse(email.expireTime,out DateTime expireTime))
                {
                    return expireTime <= DateTime.Now;//真正过期
                }
                return false;
            });
            SaveEmailData();
        }
    }
    public int GetUnreadEmailCount()
    {
        lock (emailDataLock)
        {
            if (emailData == null || emailData.list == null)
                return 0;

            return emailData.list.Count(email => !email.isDeleted &&!email.isRead);
        }
    }

    public bool HasUnclaimedRewards()
    {
        lock (emailDataLock)
        {
            if (emailData == null || emailData.list == null)
                return false;

            return emailData.list.Any(email => !email.isDeleted &&!email.isClaimed);
        }
    }

    public bool DeleteEmail(string emailId)
    {
        lock (emailDataLock)
        {
            var email = emailData.list.Find(e => e.id == emailId);
            if (email != null)
            {
                emailData.list.Remove(email);
                SaveEmailData(); // 保存到本地
                return true;
            }
            return false;
        }
    }

    public int DeleteAllEmails()
    {
        lock (emailDataLock)
        {
            int count = emailData.list.Count;
            emailData.list.Clear();
            SaveEmailData();
            return count;
        }
    }

    // 批量领取奖励（优化：只保存一次）
    public int ClaimAllRewardsBatch()
    {
        lock (emailDataLock)
        {
            if (emailData == null || emailData.list == null)
                return 0;

            int totalReward = 0;
            int claimCount = 0;
            bool scoreChanged = false;
            foreach (var email in emailData.list)
            {
                if (!email.isClaimed)
                {
                    email.isClaimed = true;
                    email.isRead = true; // 领取奖励时同时标记为已读
                    claimCount++;
                    totalReward += email.rewardAmount;
                    switch (email.rewardType)
                    {
                        case "Coin":
                            AddCoin(email.rewardAmount);
                            break;

                        case "Stamina":
                            AddStamina(email.rewardAmount);
                            break;

                        case "Score":
                            scoreData.haveScore += email.rewardAmount;
                            scoreChanged = true;
                            break;
                    }
                }
            }
            
            if (scoreChanged)
            {
                SaveScoreData();
            }

            if (claimCount > 0)
            {
                SaveEmailData(); // 只保存一次
            }
            
            return totalReward;
        }
    }

    // 批量标记已读（优化：只保存一次）
    public int MarkAllAsRead()
    {
        lock (emailDataLock)
        {
            if (emailData == null || emailData.list == null)
                return 0;

            int count = 0;
            foreach (var email in emailData.list)
            {
                if (!email.isRead)
                {
                    email.isRead = true;
                    count++;
                }
            }
            
            if (count > 0)
            {
                SaveEmailData(); // 只保存一次
            }
            
            return count;
        }
    }

    // 确保 MarkEmailAsRead 方法存在（返回void）
    public void MarkEmailAsRead(string emailId)
    {
        lock (emailDataLock)
        {
            if (emailData == null || emailData.list == null)
                return;

            var email = emailData.list.Find(e => e.id == emailId);
            if (email != null && !email.isRead)
            {
                email.isRead = true;
                SaveEmailData(); // 保存到本地
            }
        }
    }

    // ============ 体力系统 ============

    private const int MAX_STAMINA = 3;           // 体力上限
    private const int MAX_AD_COUNT = 3;          // 每日最大广告次数

    /// <summary>
    /// 检查并重置体力（每天凌晨0点重置）
    /// </summary>
    public void CheckAndResetStamina()
    {
        string today = System.DateTime.Now.ToString("yyyy-MM-dd");
        if (playerData.lastResetDate != today)
        {
            playerData.currentStamina = MAX_STAMINA;
            playerData.adCountToday = 0;
            playerData.lastResetDate = today;
            SavePlayerData();
           // Debug.Log("【体力系统】体力已重置为3");
        }
    }

    /// <summary>
    /// 手动重置体力为上限（测试用）
    /// </summary>
    public void ResetStamina()
    {
        playerData.currentStamina = MAX_STAMINA;
        SavePlayerData();
        Debug.Log("【体力系统】手动重置体力为3");
    }

    /// <summary>
    /// 检查是否有足够体力
    /// </summary>
    public bool HasEnoughStamina()
    {
        CheckAndResetStamina();
        return playerData.currentStamina > 0;
    }

    /// <summary>
    /// 获取当前体力
    /// </summary>
    public int GetCurrentStamina()
    {
        CheckAndResetStamina();
        return playerData.currentStamina;
    }

    /// <summary>
    /// 获取体力上限
    /// </summary>
    public int GetMaxStamina()
    {
        return MAX_STAMINA;
    }

    /// <summary>
    /// 消耗体力（进入关卡时调用）
    /// </summary>
    /// <returns>是否消耗成功</returns>
    public bool ConsumeStamina()
    {
        CheckAndResetStamina();
        if (playerData.currentStamina > 0)
        {
            playerData.currentStamina--;
            SavePlayerData();
           // Debug.Log($"【体力系统】消耗1体力，剩余体力: {playerData.currentStamina}");
            return true;
        }
       // Debug.Log("【体力系统】体力不足，无法消耗");
        return false;
    }

    /// <summary>
    /// 检查是否可以看广告恢复体力
    /// </summary>
    public bool CanWatchAd()
    {
        CheckAndResetStamina();
        return playerData.currentStamina < MAX_STAMINA && playerData.adCountToday < MAX_AD_COUNT;
    }

    /// <summary>
    /// 获取今日剩余广告次数
    /// </summary>
    public int GetRemainingAdCount()
    {
        CheckAndResetStamina();
        return MAX_AD_COUNT - playerData.adCountToday;
    }

    /// <summary>
    /// 广告观看成功，恢复体力
    /// </summary>
    public void RestoreStaminaByAd()
    {
        CheckAndResetStamina();
        if (playerData.currentStamina < MAX_STAMINA)
        {
            playerData.currentStamina++;
            playerData.adCountToday++;
            SavePlayerData();
           // Debug.Log($"【体力系统】广告恢复体力成功，当前体力: {playerData.currentStamina}，今日广告次数: {playerData.adCountToday}");
        }
    }

    /// <summary>
    /// 取消广告（不扣次数，不恢复体力）
    /// </summary>
    public void CancelAd()
    {
        Debug.Log("【体力系统】广告已取消");
    }

    /// <summary>
    /// 增加体力（用于签到等奖励）
    /// </summary>
    public void AddStamina(int amount)
    {
        CheckAndResetStamina();
        playerData.currentStamina = Mathf.Min(playerData.currentStamina + amount, MAX_STAMINA);
        SavePlayerData();
        //Debug.Log($"【体力系统】增加{amount}体力，当前体力: {playerData.currentStamina}");
    }
}
