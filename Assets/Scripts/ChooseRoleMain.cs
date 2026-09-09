using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseRoleMain : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(InitGame());
    }

    IEnumerator InitGame()
    {
        // 初始化GameLevelMgr（存档数据）
        GameLevelMgr.Instance.Init();

        // 异步加载配置文件（支持WebGL平台）
        yield return GameLevelMgr.Instance.LoadConfigAsync();

        // 配置加载完成后再显示面板
        UIManager.Instance.ShowPanel<ChooseHeroPanel>();
    }
}
