using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChooseScenePanel : BasePanel
{
    public Transform contentRoot;   // ScrollView Content
    public SceneBtn levelBtnPrefab; // 关卡按钮预制体

    public Button btnBack;
    public Button btnPrevPage;        // 上一页按钮
    public Button btnNextPage;        // 下一页按钮

    public int pageSize = 8;          // 每页显示8个关卡

    private int currentPageIndex = 0;

    public Button btnWarPreparePanel;
    public Button btnCyclopaedia;
    public Button btnSetting;


    public override void Init()
    {
        btnSetting.onClick.AddListener(() =>
        {
            UIManager.Instance.ShowPanel<SettingPanel>();
        });
        btnBack.onClick.AddListener(() =>
        {
            UIManager.Instance.HidePanel<ChooseScenePanel>();
            UIManager.Instance.HidePanel<LevelPanel>();
            SceneManager.LoadScene("BeginScene");
        });
        
        btnWarPreparePanel.onClick.AddListener(() =>
        {
            UIManager.Instance.ShowPanel<WarPreparePanel>();
        });
        btnCyclopaedia.onClick.AddListener(() =>
        {
            UIManager.Instance.ShowPanel<CyclopaediaPanel>();
        });
        
        // 翻页按钮的事件
        btnPrevPage.onClick.RemoveAllListeners();
        btnPrevPage.onClick.AddListener(() =>
        {
            ChangePage(currentPageIndex - 1);
        });

        btnNextPage.onClick.RemoveAllListeners();
        btnNextPage.onClick.AddListener(() =>
        {
            ChangePage(currentPageIndex + 1);
        });

        CreateLevelButtons();
    }

    private void CreateLevelButtons()
    {
        // 清空旧的
        for (int i = contentRoot.childCount - 1; i >= 0; i--)
        {
            Destroy(contentRoot.GetChild(i).gameObject);
        }

        var list = GameDataMgr.Instance.sceneDataList;
        int startIndex = currentPageIndex * pageSize;
        int endIndex = Mathf.Min(startIndex + pageSize, list.Count);

        for (int i = startIndex; i < endIndex; i++)
        {
            SceneBtn btn = Instantiate(levelBtnPrefab, contentRoot);
            btn.gameObject.SetActive(true);
            btn.Init(list[i]);
        }

        // 强制刷新布局
        LayoutRebuilder.ForceRebuildLayoutImmediate(contentRoot.GetComponent<RectTransform>());
    }
    private void ChangePage(int newPage)
    {
        var list = GameDataMgr.Instance.sceneDataList;

        if (list == null || list.Count == 0)
        {
            Debug.LogError("Scene列表为空！");
            return;
        }

        int totalPage = Mathf.CeilToInt(list.Count / (float)pageSize);

        // 防止 totalPage = 0
        if (totalPage <= 0)
            totalPage = 1;

        newPage = Mathf.Clamp(newPage, 0, totalPage - 1);

        currentPageIndex = newPage;

        CreateLevelButtons();
    }
}
