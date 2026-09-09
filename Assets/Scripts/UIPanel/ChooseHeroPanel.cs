using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class ChooseHeroPanel : BasePanel
{
    
    public Button btnLeft;
    public Button btnRight;
    public Button btnStart;
    public Button btnBack;
   
    public TextMeshProUGUI txtName;

   
    private Transform heroPos;

    
    private GameObject heroObj;
    
    private RoleData nowRoleData;
   
    private int nowIndex;

    public override void Init()
    {

       
        nowIndex = 0;
        // 安全的查找方式
        if (heroPos == null)
        {
            GameObject heroPosObj = GameObject.Find("HeroPos");
            if (heroPosObj != null)
            {
                heroPos = heroPosObj.transform;
                Debug.Log("成功找到 HeroPos");
            }
            else
            {
                Debug.LogError("无法找到 HeroPos！请确保场景中有一个名为 'HeroPos' 的 GameObject");
                return;  // 找不到就提前退出
            }
        }

       // heroPos = GameObject.Find("HeroPos").transform;

        
       // txtMoney.text = GameDataMgr.Instance.playerData.haveMoney.ToString();

        btnLeft.onClick.AddListener(() =>
        {
            --nowIndex;
            if (nowIndex < 0)
                nowIndex = GameDataMgr.Instance.roleDataList.Count - 1;

            StartCoroutine(WaitRoleData());
        });
        btnRight.onClick.AddListener(() =>
        {
            ++nowIndex;
            if (nowIndex >= GameDataMgr.Instance.roleDataList.Count)
                nowIndex = 0;

            StartCoroutine(WaitRoleData());
        });
     
        btnStart.onClick.AddListener(() =>
        {
            //记录当前选择的角色
            GameDataMgr.Instance.nowSelRole = nowRoleData;

            //关闭面板显示选择场景
            UIManager.Instance.HidePanel<ChooseHeroPanel>();
            SceneManager.LoadScene("LevelScene");
           // UIManager.Instance.ShowPanel<ChooseScenePanel>();
        });
        btnBack.onClick.AddListener(() =>
        {
            UIManager.Instance.HidePanel<ChooseHeroPanel>();
            UIManager.Instance.ShowPanel<BeginPanel>();

        });


        StartCoroutine(WaitRoleData());

    }
    private IEnumerator WaitRoleData()
    {
        Debug.Log("等待角色数据加载...");

        while (!GameDataMgr.Instance.IsRoleDataLoaded)
        {
            yield return null;
        }

        Debug.Log("角色数据加载完成");

        ChangeHero();
    }
    private void ChangeHero()
    {
        if (heroObj != null)
        {
            Destroy(heroObj);
            heroObj = null;
        }

        if (GameDataMgr.Instance.roleDataList == null || GameDataMgr.Instance.roleDataList.Count == 0)
        {
            Debug.LogError("roleDataList 为空，无法显示英雄");
            return;
        }

        if (nowIndex < 0 || nowIndex >= GameDataMgr.Instance.roleDataList.Count)
        {
            Debug.LogWarning($"nowIndex={nowIndex} 无效，重置为0");
            nowIndex = 0;
            if (GameDataMgr.Instance.roleDataList.Count == 0) return;
        }

        nowRoleData = GameDataMgr.Instance.roleDataList[nowIndex];
        txtName.text = nowRoleData.name;
        heroObj = Instantiate(Resources.Load<GameObject>(nowRoleData.res), heroPos.position, heroPos.rotation);
        
        Animator animator = heroObj.GetComponent<Animator>();
        if (animator != null)
        {
            animator.applyRootMotion = false;
        }
    }


    public override void HideMe(UnityAction callBack)
    {
        base.HideMe(callBack);
        
        if (heroObj != null)
        {
            DestroyImmediate(heroObj);
            heroObj = null;
        }
    }


}
