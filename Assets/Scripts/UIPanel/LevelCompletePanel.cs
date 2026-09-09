using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelCompletePanel : BasePanel
{

    public Button btnNextLevel;

    public override void Init()
    {
        AudioManager.Instance.PlaySound("Perfect");
        btnNextLevel.onClick.RemoveAllListeners();

        btnNextLevel.onClick.AddListener(() =>
        {
            // 通关后重置最高分
            GameDataMgr.Instance.ResetMaxScore();
            
            // 结算累计积分
            GameDataMgr.Instance.TryRefreshTotalScoreData();
            
            // 结算金币（积分转金币）
            GameDataMgr.Instance.SettleCoinFromScore(GameDataMgr.Instance.labScore);
            
            GameLevelMgr.Instance.ContinueNextLevel();

            UIManager.Instance.HidePanel<LevelCompletePanel>();

        });

    }

}
