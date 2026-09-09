using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneGame : MonoBehaviour
{
    private Transform levelRoot;
    private Transform playerRoot;
    private AsyncOperationHandle<GameObject> currentLoadHandle;
     private int score = 0;
    private GamePanel gamePanel;
    public static bool hasLoaded = false; // 使用静态变量，跨实例生效
    private static bool isFirstInstance = true; // 标记是否是第一个实例
    
    void Awake()
    {
        // 注册场景加载回调，确保在场景加载完成时重置 hasLoaded
        SceneManager.sceneLoaded += OnSceneLoaded;
        
        // 如果当前场景就是 SceneGame，直接重置标志（处理第一次进入场景的情况）
        if (SceneManager.GetActiveScene().name == "SceneGame")
        {
            // 重置加载标志
            hasLoaded = false;
            isFirstInstance = true;
            Debug.Log("SceneGame Awake, hasLoaded and isFirstInstance reset");
        }
    }
    
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 只有当加载的是 SceneGame 场景时才重置
        if (scene.name == "SceneGame")
        {
            // 重置加载标志
            hasLoaded = false;
            isFirstInstance = true;
            Debug.Log("SceneGame loaded, hasLoaded and isFirstInstance reset");
        }
    }
    
    void Start()
    {
        // 播放游戏场景背景音乐
        PlayGameBackgroundMusic();
        
        // 查找场景中已存在的根节点，如果不存在则创建
        FindOrCreateRoots();
        
        // 重置当前关卡分数
        GameDataMgr.Instance.labScore = 0;
        // 加载GamePanel
        gamePanel = UIManager.Instance.ShowPanel<GamePanel>();
        // 重置游戏时间
        if (gamePanel != null)
        {
            gamePanel.ResetGameTime();
        }
        LoadLevel();
    }
    
    void FindOrCreateRoots()
    {
        // 直接在场景根级别查找 LevelRoot
        GameObject levelRootObj = GameObject.Find("LevelRoot");
        if (levelRootObj != null)
        {
            levelRoot = levelRootObj.transform;
            // 清理 LevelRoot 下已存在的关卡对象
            ClearChildren(levelRoot);
            Debug.Log("Found existing LevelRoot in scene");
        }
        else
        {
            GameObject rootObj = new GameObject("LevelRoot");
            levelRoot = rootObj.transform;
            Debug.Log("Created new LevelRoot");
        }
        
        // 直接在场景根级别查找 PlayerRoot
        GameObject playerRootObj = GameObject.Find("PlayerRoot");
        if (playerRootObj != null)
        {
            playerRoot = playerRootObj.transform;
            // 清理 PlayerRoot 下已存在的玩家对象
            ClearChildren(playerRoot);
            Debug.Log("Found existing PlayerRoot in scene");
        }
        else
        {
            GameObject rootObj = new GameObject("PlayerRoot");
            playerRoot = rootObj.transform;
            Debug.Log("Created new PlayerRoot");
        }
    }
    
    void ClearChildren(Transform parent)
    {
        foreach (Transform child in parent)
        {
            Destroy(child.gameObject);
        }
    }
    void LoadLevel()
    {
        // 确保只有第一个 SceneGame 实例可以执行加载
        if (!isFirstInstance)
        {
            Debug.LogWarning("Not the first SceneGame instance, skipping load");
            return;
        }
        
        // 防止重复加载
        if (hasLoaded)
        {
            Debug.LogWarning("Level already loaded, skipping");
            return;
        }
        
        // 标记为已加载，并设置为非第一个实例（防止其他实例加载）
        hasLoaded = true;
        isFirstInstance = false;
        
        if (GameLevelMgr.Instance == null)
        {
            Debug.LogError("GameLevelMgr.Instance is null");
            return;
        }
        int levelId = GameLevelMgr.Instance.currentLevelId;
        // 确保levelId不为0，防止加载错误的关卡
        if (levelId <= 0)
        {
            // 如果levelId为0，使用GameDataMgr中的currentLevelId
            levelId = GameDataMgr.Instance.currentLevelId;
            // 如果仍然为0，默认为1
            if (levelId <= 0)
            {
                levelId = 1;
                Debug.LogWarning("没有选择关卡，默认加载第1关");
            }

            // 更新GameLevelMgr中的currentLevelId
            GameLevelMgr.Instance.currentLevelId = levelId;
            GameDataMgr.Instance.currentLevelId = levelId;
        }

        if (!GameLevelMgr.Instance.IsLevelAvailable(levelId))
        {
            Debug.LogError($"关卡 {levelId} 没有可加载的 Addressables 资源");
            hasLoaded = false;
            isFirstInstance = true;
            UnityEngine.SceneManagement.SceneManager.LoadScene("LevelScene");
            return;
        }

        LevelData data = GameLevelMgr.Instance.GetLevel(levelId);
        if (data == null || string.IsNullOrEmpty(data.address))
        {
            Debug.LogError($"Invalid level data for id: {levelId}");
            hasLoaded = false;  // 重置标志，允许重试
            isFirstInstance = true;
            return;
        }
        currentLoadHandle = Addressables.LoadAssetAsync<GameObject>(data.address);
        currentLoadHandle.Completed += OnLoadCompleted;
    }
    void OnLoadCompleted(AsyncOperationHandle<GameObject> handle)
    {
        if (handle.Status != AsyncOperationStatus.Succeeded)
        {
            Debug.LogError($"Failed to load level: {handle.OperationException}");
            hasLoaded = false;
            isFirstInstance = true;
            UnityEngine.SceneManagement.SceneManager.LoadScene("LevelScene");
            return;
        }
        // 验证父对象是否有效
        if (!IsParentValid(levelRoot))
        {
            Debug.LogWarning("levelRoot is not valid, creating temporary parent");
           // CreateLevelRoot();
        }
        // 安全地实例化对象
        GameObject levelInstance;

        try
        {
            levelInstance = Instantiate(handle.Result, levelRoot);
            Debug.Log($"Level loaded successfully: {levelInstance.name}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to instantiate level: {e.Message}");
            // 降级方案：不带父级实例化
            levelInstance = Instantiate(handle.Result);
        }
        
        // 在关卡中生成用户选择的角色
        SpawnSelectedPlayer(levelInstance);
        
        // 更新关卡显示
        if (gamePanel != null)
        {
            gamePanel.UpdateCurrentLevelDisplay();
        }
    }
    
    private void SpawnSelectedPlayer(GameObject levelInstance)
    {
        // 获取用户选择的角色数据
        RoleData selectedRole = GameDataMgr.Instance.nowSelRole;
        if (selectedRole == null)
        {
            Debug.LogWarning("No role selected!");
            return;
        }
        
        Debug.Log($"Spawning selected role: {selectedRole.name} ({selectedRole.res})");
        
        // 查找关卡中的Player对象（可能存在默认Player）
        Player existingPlayer = levelInstance.GetComponentInChildren<Player>();
        
        Vector3 spawnPosition = Vector3.zero;
        Quaternion spawnRotation = Quaternion.identity;
        
        if (existingPlayer != null)
        {
            // 如果存在默认Player，获取其位置并销毁它
            spawnPosition = existingPlayer.transform.position;
            spawnRotation = existingPlayer.transform.rotation;
            Destroy(existingPlayer.gameObject);
            Debug.Log("Removed default player, using its position");
        }
        else
        {
            // 如果没有默认Player，查找名为"PlayerPos"或类似的生成点
            Transform spawnPoint = levelInstance.transform.Find("PlayerPos");
            if (spawnPoint != null)
            {
                spawnPosition = spawnPoint.position;
                spawnRotation = spawnPoint.rotation;
                Debug.Log("Found PlayerPos spawn point");
            }
            else
            {
                // 使用关卡原点作为默认生成位置
                Debug.LogWarning("No PlayerPos found, using level origin");
            }
        }
        
        // 加载并实例化用户选择的角色（放在单独的PlayerRoot下）
        GameObject rolePrefab = Resources.Load<GameObject>(selectedRole.res);
        if (rolePrefab == null)
        {
            Debug.LogError($"Failed to load role prefab: {selectedRole.res}");
            return;
        }
        
        GameObject playerObj = Instantiate(rolePrefab, spawnPosition, spawnRotation, playerRoot);
        playerObj.name = "Player_" + selectedRole.name;
        
        // 禁用Animator的根运动，防止动画导致角色上升
         Animator animator = playerObj.GetComponent<Animator>();
         if (animator != null)
         {
            animator.applyRootMotion = false;
            Debug.Log("Disabled root motion on player animator");
         }
        
        // 获取玩家的Player组件并订阅死亡事件
        Player player = playerObj.GetComponent<Player>();
        if (player == null)
        {
            // 如果角色预制体没有Player组件，动态添加一个
            player = playerObj.AddComponent<Player>();
            Debug.Log("Player component not found on role prefab, added dynamically");
        }
        player.OnDead += OnPlayerDead;
        
        // 设置摄像机跟随玩家
        SetupCameraFollow(playerObj.transform);
        
        Debug.Log("Successfully spawned selected player");
    }
    
    private void SetupCameraFollow(Transform playerTransform)
    {
        ThirdPersonCamera cameraFollow = FindObjectOfType<ThirdPersonCamera>();
        if (cameraFollow != null)
        {
            cameraFollow.target = playerTransform;
            Debug.Log("Camera follow target set to: " + playerTransform.name);
        }
        else
        {
            Debug.LogWarning("ThirdPersonCamera not found in scene. Adding to main camera...");
            Camera mainCamera = Camera.main;
            if (mainCamera != null)
            {
                cameraFollow = mainCamera.gameObject.AddComponent<ThirdPersonCamera>();
                cameraFollow.target = playerTransform;
                Debug.Log("Added ThirdPersonCamera component to main camera");
            }
        }
    }
    
    private void OnPlayerDead()
    {
        Time.timeScale = 0f;
        UIManager.Instance.ShowPanel<FailPanel>();
    }
    private bool IsParentValid(Transform parent)
    {
        if (parent == null) return false;
        if (parent.gameObject == null) return false;

        // 检查父对象是否在已加载的场景中
        try
        {
            return parent.gameObject.scene.isLoaded && parent.gameObject.activeInHierarchy;
        }
        catch
        {
            return false;
        }
    }
    void OnDestroy()
    {
        // 取消注册场景加载回调
        SceneManager.sceneLoaded -= OnSceneLoaded;
        
        // 清理资源
        if (currentLoadHandle.IsValid())
        {
            currentLoadHandle.Completed -= OnLoadCompleted;
            Addressables.Release(currentLoadHandle);
        }
    }
    
    /// <summary>
    /// 播放游戏场景背景音乐
    /// </summary>
    private void PlayGameBackgroundMusic()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayGameBackgroundMusic();
            Debug.Log("【SceneGame】播放游戏场景背景音乐");
        }
        else
        {
            Debug.LogWarning("【SceneGame】AudioManager.Instance is null, cannot play background music");
        }
    }
}
