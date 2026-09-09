using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

public class GameLevelMgr
{
    private static GameLevelMgr instance = new GameLevelMgr();
    public static GameLevelMgr Instance => instance;
    public List<SceneData> sceneList;
    public List<LevelData> levelList;
    public LevelSaveData saveData;
    public int currentLevelId;
    // ================= 初始化 =================
    public void Init()
    { 
        LoadSave();
    }
    // ================= 异步加载配置（支持WebGL）=================
    public IEnumerator LoadConfigAsync()
    {
        yield return JsonMgr.Instance.LoadDataAsync<List<SceneData>>("SceneData", (data) => {
            sceneList = data;
        });
        
        yield return JsonMgr.Instance.LoadDataAsync<List<LevelData>>("LevelData", (data) => {
            levelList = data;
        });

        yield return FilterUnavailableLevelsAsync();
    }

    private IEnumerator FilterUnavailableLevelsAsync()
    {
        if (levelList == null || levelList.Count == 0)
        {
            levelList = new List<LevelData>();
            yield break;
        }

        List<LevelData> availableLevels = new List<LevelData>();

        foreach (LevelData level in levelList)
        {
            if (level == null || string.IsNullOrEmpty(level.address))
                continue;

            AsyncOperationHandle<IList<IResourceLocation>> handle =
                Addressables.LoadResourceLocationsAsync(level.address, typeof(GameObject));

            yield return handle;

            if (handle.Status == AsyncOperationStatus.Succeeded && handle.Result != null && handle.Result.Count > 0)
            {
                availableLevels.Add(level);
            }
            else
            {
                Debug.LogWarning($"【关卡配置】Addressables 中未找到关卡资源: {level.address}，已从可选关卡中过滤");
            }

            Addressables.Release(handle);
        }

        levelList = availableLevels;

        int maxAvailableLevel = GetMaxAvailableLevelId();
        if (saveData != null && maxAvailableLevel > 0 && saveData.maxUnlockLevel > maxAvailableLevel)
        {
            saveData.maxUnlockLevel = maxAvailableLevel;
            Save();
        }
    }
    // ================= 存档 =================
    void LoadSave()
    {
        saveData = JsonMgr.Instance.LoadData<LevelSaveData>("LevelSave");
        if (saveData == null)
        {
            saveData = new LevelSaveData();
        }
        if (saveData.maxUnlockLevel <= 0)
            saveData.maxUnlockLevel = 1;
    }
    public void Save()
    {
        JsonMgr.Instance.SaveData(saveData, "LevelSave");
    }
    // ================= 解锁 =================
    public bool IsLevelUnlocked(int levelId)
    {
        return IsLevelAvailable(levelId) && levelId <= saveData.maxUnlockLevel;
    }
    
    public bool IsSceneUnlocked(int sceneId)
    {
        var range = GetLevelRange(sceneId);
        if (range.start == 0 && range.end == 0)
        {
            return false;
        }
        // 判断场景的第一个关卡是否已解锁
        return IsLevelUnlocked(range.start);
    }
    public void UnlockNextLevel()
    {
        int nextLevelId = saveData.maxUnlockLevel + 1;
        if (IsLevelAvailable(nextLevelId))
        {
            saveData.maxUnlockLevel = nextLevelId;
            Save();
        }
    }

    public bool IsLevelAvailable(int levelId)
    {
        return GetLevel(levelId) != null;
    }

    public int GetMaxAvailableLevelId()
    {
        if (levelList == null || levelList.Count == 0)
            return 0;

        int maxLevelId = 0;
        foreach (LevelData level in levelList)
        {
            if (level != null && level.levelId > maxLevelId)
                maxLevelId = level.levelId;
        }

        return maxLevelId;
    }
    // ================= 重置 =================
    public void ResetAllLevels()
    {
        // 重置解锁进度
        saveData.maxUnlockLevel = 1;
        // 保存重置后的数据
        Save();
        // 重置当前关卡ID
        currentLevelId = 1;
        // 重置游戏数据
        GameDataMgr.Instance.ResetGameData();
    }
    // ================= 工具 =================
    // 根据 Scene 获取关卡范围
    public (int start, int end) GetLevelRange(int sceneId)
    {
       // Debug.Log("sceneList = " + sceneList);

        if (sceneList != null)
        {
           // Debug.Log("sceneList.Count = " + sceneList.Count);
        }
        else
        {
           // Debug.LogError("❌ sceneList is NULL!");
            return (0, 0);
        }
        int start = 1;
        foreach (var scene in sceneList)
        {
            //Debug.Log($"scene item: sceneId={scene.sceneId}, levelNum={scene.levelNum}");
            int end = start + scene.levelNum - 1;
            if (scene.sceneId == sceneId)
                return (start, end);
            start = end + 1;
        }
        //Debug.LogWarning($"⚠️ 未找到 sceneId={sceneId} 的场景");
        return (0, 0);
    }
    // 获取某个 Scene 的所有关卡
    public List<LevelData> GetLevelsByScene(int sceneId)
    {
        var range = GetLevelRange(sceneId);
        if (levelList == null)
        {
            //Debug.LogError("❌ levelList is NULL!");
            return new List<LevelData>();
        }
        if (range.start == 0 && range.end == 0)
        {
            //Debug.LogError($"❌ 未找到 sceneId={sceneId} 的范围");
            return new List<LevelData>();
        }
       // Debug.Log($"查找关卡范围: {range.start} - {range.end}");

        var result = levelList.FindAll(l =>
            l.levelId >= range.start && l.levelId <= range.end);

        //Debug.Log($"找到 {result.Count} 个关卡");
        return result;
    }
    public void ContinueNextLevel()
    {
        // 通关进入下一关不扣除体力

        // 恢复游戏时间
        Time.timeScale = 1f;
        
        // 增加当前关卡ID
        currentLevelId++;

        if (!IsLevelAvailable(currentLevelId))
        {
            Debug.LogWarning($"【关卡配置】关卡 {currentLevelId} 尚未配置可加载资源，返回选关界面");
            currentLevelId = saveData.maxUnlockLevel;
            UIManager.Instance.HidePanel<LevelCompletePanel>(false);
            UnityEngine.SceneManagement.SceneManager.LoadScene("LevelScene");
            return;
        }
        
        // 进入新关卡时重置本局分数
        GameDataMgr.Instance.OnEnterLevel(currentLevelId);
        
        // 加载下一关（hasLoaded 会在场景加载时自动重置）
        UnityEngine.SceneManagement.SceneManager.LoadScene("SceneGame");
    }
    // 关卡完成处理
    public void OnLevelFinish()
    {
        // 获取本局最终分数
        int currentScore = GameDataMgr.Instance.labScore;

        // 刷新全局最高分
        GameDataMgr.Instance.TryRefreshMaxScore(currentScore);

        // 上报到微信排行榜
        GameDataMgr.Instance.UploadScoreToWeChat(currentScore);
        // 显示通关面板
        UIManager.Instance.ShowPanel<LevelCompletePanel>();
        // 暂停游戏时间
        Time.timeScale = 0f;
        // 只有通关当前最高解锁关卡时，才解锁下一关
        if (currentLevelId >= saveData.maxUnlockLevel)
        {
            UnlockNextLevel();
        }
        // 保存游戏进度
        Save();
    }
    //重新玩当前关卡
    public void RestartLevel()
    {
        // 恢复游戏时间
        Time.timeScale = 1f;
        // 重置当前关卡分数
        GameDataMgr.Instance.OnEnterLevel(currentLevelId);
        // 重新加载当前关卡（hasLoaded 会在场景加载时自动重置）
        UnityEngine.SceneManagement.SceneManager.LoadScene("SceneGame");
    }
    // 获取关卡
    public LevelData GetLevel(int levelId)
    {
        if (levelList == null)
            return null;

        return levelList.Find(l => l.levelId == levelId);
    }
}
