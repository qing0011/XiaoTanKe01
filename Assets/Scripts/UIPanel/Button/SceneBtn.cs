using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SceneBtn : MonoBehaviour
{
    public TextMeshProUGUI txtInfo;
    public Image imgeScene;
    public Button btn;
    public GameObject lockObj;

    private SceneData sceneData;

   

    void Awake()
    {
        if (btn == null)
        {
            btn = GetComponent<Button>();
        }
    }

    // 给外部初始化
    public void Init(SceneData data)
    {
        sceneData = data;

        imgeScene.sprite = Resources.Load<Sprite>(sceneData.imgRes);
        txtInfo.text = sceneData.name;

        bool unlocked = GameLevelMgr.Instance.IsSceneUnlocked(sceneData.sceneId);
        
        if (lockObj != null)
        {
            lockObj.SetActive(!unlocked);
        }
        btn.interactable = unlocked;
        
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(OnClickEnterScene);
       
      

    }

    private void OnClickEnterScene()
    {
        if (!GameLevelMgr.Instance.IsSceneUnlocked(sceneData.sceneId))
        {
            Debug.Log("该场景尚未解锁！");
            // 可以这里添加提示面板，例如：
            // UIManager.Instance.ShowPanel<TipsPanel>();
            return;
        }
        
        // 关闭选择场景
        UIManager.Instance.HidePanel<ChooseScenePanel>();
        LevelPanel panel = UIManager.Instance.ShowPanel<LevelPanel>();
        panel.Init(sceneData.sceneId);
    }
}
