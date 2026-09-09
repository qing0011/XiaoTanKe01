using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class LevelPanel : BasePanel
{
    public Transform contentRoot;
    public LevelBtn levelBtnPrefab;
    public Button btnBack;

    private int sceneId;

    
    /////////// 关卡levelId = 唯一的“真实编号”（用于逻辑）

    /////////// localIndex = 仅用于 UI 显示
  
    public void Init(int id)
    {
        btnBack.onClick.AddListener(() =>
        {
            UIManager.Instance.HidePanel<LevelPanel>();
            UIManager.Instance.ShowPanel<ChooseScenePanel>();
            
        });

        sceneId = id;
        Create();

    }
    public override void Init()
    {
        // 可以先空着，或者写初始化逻辑
    }
    void Create()
    {
        foreach (Transform child in contentRoot)
            Destroy(child.gameObject);
        // 添加预制体空值检查
        if (levelBtnPrefab == null)
        {
           
            return;
        }

        var list = GameLevelMgr.Instance.GetLevelsByScene(sceneId);

        if (list == null || list.Count == 0)
        {
            return;
        }


        var range = GameLevelMgr.Instance.GetLevelRange(sceneId);
        int start = range.start;

        for (int i = 0; i < list.Count; i++)
        {

            LevelBtn btn = Instantiate(levelBtnPrefab, contentRoot);

            //  检查实例化是否成功
            if (btn == null)
            {
                continue;
            }

            int localIndex = list[i].levelId - start + 1;
            btn.Init(list[i], localIndex);
        }
    }
}