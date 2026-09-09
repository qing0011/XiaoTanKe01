using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PausePanel : BasePanel
{
    public Button btnReset;
    public Button btnExit;
    public override void Init()
    {
        AudioManager.Instance.StopMusic();
        btnReset.onClick.RemoveAllListeners();
        btnReset.onClick.AddListener(() =>
        {
            GameDataMgr.Instance.ResetGameData();
            UIManager.Instance.HidePanel<PausePanel>();
            Time.timeScale = 1f;
            AudioManager.Instance.PlayGameBackgroundMusic();
        });
        btnExit.onClick.RemoveAllListeners();
        btnExit.onClick.AddListener(() =>
        {
            // 恢复游戏时间（防止退出后GamePanel继续倒计时导致显示失败界面）
            Time.timeScale = 1f;
            
            // 暂停后退出关卡，扣除1体力
            GameDataMgr.Instance.ConsumeStamina();

            // 记录当前关卡的最高分
            GameDataMgr.Instance.TryRefreshMaxScore(GameDataMgr.Instance.labScore);
            UIManager.Instance.HidePanel<PausePanel>();
            // 使用 isFade = false 立即销毁GamePanel，防止其Update继续运行导致显示失败界面
            UIManager.Instance.HidePanel<GamePanel>(false);
            SceneManager.LoadScene("LevelScene");
        });
    }

   
}
