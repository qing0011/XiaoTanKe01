using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 这里最大的问题是按钮被遮挡，，没有事实出发，修复了很久
/// </summary>
public class LevelBtn : MonoBehaviour
{
    public TextMeshProUGUI txtLevel;
    public GameObject lockObj;
    public Button btn;

    private LevelData levelData;

    void Awake()
    {
        if (btn == null)
        {
            btn = GetComponent<Button>();
        }
    }

    public void Init(LevelData data, int localIndex)
    {
       

        levelData = data;
        txtLevel.text = "关卡 " + localIndex;

        bool unlocked = GameLevelMgr.Instance.IsLevelUnlocked(levelData.levelId);

        lockObj.SetActive(!unlocked);
        btn.interactable = unlocked;

        // 清除之前的监听，添加新的
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        // 检查体力
        if (!GameDataMgr.Instance.HasEnoughStamina())
        {
            // 体力不足，显示体力不足提示面板
            UIManager.Instance.ShowPanel<StaminaPanel>();
            return;
        }

        // 消耗体力
       // GameDataMgr.Instance.ConsumeStamina();

        // 设置当前关卡ID
        GameDataMgr.Instance.currentLevelId = levelData.levelId;
        GameLevelMgr.Instance.currentLevelId = levelData.levelId;
        
        // 检查场景是否存在
        string sceneName = "SceneGame";
        // 列出所有已添加到 Build Settings 的场景
        int sceneCount = UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings;

        for (int i = 0; i < sceneCount; i++)
        {
            string scenePath = UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(i);
            string sceneNameFromPath = System.IO.Path.GetFileNameWithoutExtension(scenePath);
        }
        
        // 尝试加载（hasLoaded 会在场景加载时自动重置）
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        UIManager.Instance.HidePanel<LevelPanel>();
    }
}