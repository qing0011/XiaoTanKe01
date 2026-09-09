using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FailPanel : BasePanel
{
    public Button Restart;
    public Button btnBack;

    public override void Init()
    {
        AudioManager.Instance.PlaySound("Fail");
        AudioManager.Instance.StopMusic();
        GameDataMgr.Instance.ConsumeStamina();

        btnBack.onClick.RemoveAllListeners();
        btnBack.onClick.AddListener(() =>
        {
            UIManager.Instance.HidePanel<FailPanel>(false);
            UIManager.Instance.HidePanel<GamePanel>(false);
            UnityEngine.SceneManagement.SceneManager.LoadScene("LevelScene");
        });

        Restart.onClick.RemoveAllListeners();
        Restart.onClick.AddListener(() =>
        {
            if (!GameDataMgr.Instance.HasEnoughStamina())
            {
                UIManager.Instance.HidePanel<FailPanel>();
                UIManager.Instance.ShowPanel<StaminaPanel>();
                return;
            }

            GameLevelMgr.Instance.RestartLevel();
            UIManager.Instance.HidePanel<FailPanel>();
        });
    }
}
