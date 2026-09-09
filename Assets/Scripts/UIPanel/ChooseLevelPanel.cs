using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChooseLevelPanel : BasePanel
{
    public Button[] levelBtns;   // 40个关卡按钮
    private int currentSceneId;

    public override void Init()
    {

    }

    public void InitLevel(int sceneId)
    {
        currentSceneId = sceneId;

        for (int i = 0; i < levelBtns.Length; i++)
        {
            int levelIndex = (sceneId - 1) * 40 + i + 1;

            levelBtns[i].onClick.RemoveAllListeners();

            bool isUnlock = levelIndex <= GameDataMgr.Instance.maxLevelId;

            SetLevelBtnState(levelBtns[i], isUnlock);

            if (isUnlock)
            {
                levelBtns[i].onClick.AddListener(() =>
                {
                    EnterLevel(levelIndex);
                });
            }
        }
    }

    void EnterLevel(int levelId)
    {
        Debug.Log("选择关卡" + levelId);

        // 检查体力
        if (!GameDataMgr.Instance.HasEnoughStamina())
        {
            // 体力不足，显示体力不足提示面板
            UIManager.Instance.ShowPanel<StaminaPanel>();
            return;
        }

        // 消耗体力
        GameDataMgr.Instance.ConsumeStamina();

        GameDataMgr.Instance.currentLevelId = levelId;
        GameLevelMgr.Instance.currentLevelId = levelId;

        // 这里的关卡名称是Level1、Level2...
        SceneManager.LoadScene("Level" + levelId);
    }

    void SetLevelBtnState(Button btn, bool isUnlock)
    {
        Image img = btn.GetComponent<Image>();

        if (isUnlock)
        {
            img.color = Color.white;
            btn.interactable = true;
        }
        else
        {
            img.color = Color.gray;
            btn.interactable = false;
        }
    }
}
