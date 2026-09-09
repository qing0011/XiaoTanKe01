using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class EmailPanel : BasePanel
{
    public Button btnClose;
    public Transform emailListParent;
    public GameObject emailItemPrefab;

    // 新增UI组件
    public Button btnClaimAll;
    public Button btnDeleteAll;
    public Button btnMarkAllRead;
    public TextMeshProUGUI txtUnreadCount;

    // 存储邮件项引用，用于高效更新
    private Dictionary<string, GameObject> emailItems = new Dictionary<string, GameObject>();
    private List<EmailData> currentEmailList = new List<EmailData>();

    [SerializeField] private ScrollRect emailScrollRect; // 拖拽赋值
    private void RefreshScrollRect()
    {
        if (emailScrollRect == null)
            emailScrollRect = GetComponentInParent<ScrollRect>();

        if (emailScrollRect != null)
        {
            Canvas.ForceUpdateCanvases();
            LayoutRebuilder.ForceRebuildLayoutImmediate(emailScrollRect.content);
            emailScrollRect.verticalNormalizedPosition = 1f; // 滚动到顶部
        }
    }
    public override void Init()
    {
        btnClose.onClick.AddListener(() =>
        {
            UIManager.Instance.HidePanel<EmailPanel>();
        });

        // 新增按钮事件
        if (btnClaimAll != null)
        {
            btnClaimAll.onClick.AddListener(ClaimAllRewards);
        }

        if (btnDeleteAll != null)
        {
            btnDeleteAll.onClick.AddListener(DeleteAllEmails);
        }

        if (btnMarkAllRead != null)
        {
            btnMarkAllRead.onClick.AddListener(MarkAllAsRead);
        }

        // 初始化时显示邮件列表
        ShowEmailList();
    }

    public void ShowEmailList()
    {
        // 等待数据加载完成
        if (!GameDataMgr.Instance.IsEmailDataLoaded)
        {
            Debug.Log("邮件数据加载中，稍后重试");
            // 可以延迟一秒再试
            StartCoroutine(WaitForDataAndShow());
            return;
        }
        // 检查必要的引用是否存在
        if (emailListParent == null)
        {
            Debug.LogError("EmailPanel: emailListParent is not assigned");
            return;
        }

        if (emailItemPrefab == null)
        {
            Debug.LogError("EmailPanel: emailItemPrefab is not assigned");
            return;
        }

        // 获取邮件数据（使用锁保护）
        List<EmailData> sortedEmails = null;
        lock (GameDataMgr.Instance.GetEmailLock())
        {
            var emailData = GameDataMgr.Instance.emailData;
            if (emailData == null || emailData.list == null)
            {
                return;
            }

            // 创建副本并排序，避免修改原始数据
            sortedEmails = new List<EmailData>(emailData.list);
            sortedEmails.Sort((a, b) => {
                if (System.DateTime.TryParse(a.sendTime, out System.DateTime timeA) &&
                    System.DateTime.TryParse(b.sendTime, out System.DateTime timeB))
                {
                    return timeB.CompareTo(timeA);
                }
                return 0;
            });
            // 最后刷新滚动区域
            RefreshScrollRect();
        }

        currentEmailList = sortedEmails;

        // 更新未读邮件数量显示
        UpdateUnreadCount();

        // 更新一键领取按钮状态
        UpdateClaimAllButtonState();

        // 更新全部已读按钮状态
        UpdateMarkAllReadButtonState();

        // 清理不存在的邮件项
        List<string> keysToRemove = new List<string>();
        foreach (var kvp in emailItems)
        {
            if (!currentEmailList.Exists(e => e.id == kvp.Key))
            {
                keysToRemove.Add(kvp.Key);
                Destroy(kvp.Value);
            }
        }
        foreach (string key in keysToRemove)
        {
            emailItems.Remove(key);
        }

        // 显示或更新邮件列表
        foreach (var email in currentEmailList)
        {
            if (emailItems.ContainsKey(email.id))
            {
                // 更新现有邮件项
                UpdateEmailItem(emailItems[email.id], email);
            }
            else
            {
                // 创建新邮件项
                CreateEmailItem(email);
            }
        }
    }
    private IEnumerator WaitForDataAndShow()
    {
        yield return new WaitForSeconds(0.5f);
        ShowEmailList();
    }
    private void CreateEmailItem(EmailData email)
    {
        var emailItem = Instantiate(emailItemPrefab, emailListParent);
        emailItems[email.id] = emailItem;

        // 设置邮件项的内容和事件
        SetupEmailItem(emailItem, email);
    }

    private void SetupEmailItem(GameObject emailItem, EmailData email)
    {
        // 获取各个组件
        var titleText = emailItem.transform.Find("TitleText")?.GetComponent<Text>();
        var contentText = emailItem.transform.Find("ContentText")?.GetComponent<Text>();
        var timeText = emailItem.transform.Find("TimeText")?.GetComponent<Text>();
        var rewardText = emailItem.transform.Find("RewardText")?.GetComponent<Text>();
        var readFlag = emailItem.transform.Find("ReadFlag");
        var claimButton = emailItem.transform.Find("ClaimButton")?.GetComponent<Button>();
        var deleteButton = emailItem.transform.Find("DeleteButton")?.GetComponent<Button>();
        var backgroundImage = emailItem.GetComponent<Image>();

        // 设置文本内容
        if (titleText != null) titleText.text = email.title;
        if (contentText != null) contentText.text = email.content;
        if (timeText != null) timeText.text = email.sendTime;

        // 设置奖励文本
        if (rewardText != null)
        {
            if (email.isClaimed)
            {
                rewardText.text = "奖励: 已领取";
            }
            else
            {
                rewardText.text =$"{email.rewardType} x{email.rewardAmount}";
            }
        }

        // 设置已读/未读状态（未读邮件高亮显示）
        if (readFlag != null)
            readFlag.gameObject.SetActive(!email.isRead);

        // 未读邮件高亮背景
        if (backgroundImage != null)
        {
            if (!email.isRead)
            {
                // 未读邮件：浅黄色背景
                backgroundImage.color = new Color(1f, 0.98f, 0.8f, 1f);
            }
            else
            {
                // 已读邮件：正常背景色
                backgroundImage.color = Color.white;
            }
        }

        // 设置领取按钮
        if (claimButton != null)
        {
            claimButton.gameObject.SetActive(!email.isClaimed);
            // 清除之前的监听器，避免重复添加
            claimButton.onClick.RemoveAllListeners();
            claimButton.onClick.AddListener(() => {
                ClaimEmailReward(email.id);
            });
        }

        // 设置删除按钮
        if (deleteButton != null)
        {
            deleteButton.onClick.RemoveAllListeners();
            deleteButton.onClick.AddListener(() => {
                DeleteSingleEmail(email.id);
            });
        }

        // 添加邮件点击事件（点击邮件主体标记为已读）
        var emailButton = emailItem.GetComponent<Button>();
        if (emailButton == null)
        {
            emailButton = emailItem.AddComponent<Button>();
        }
        emailButton.onClick.RemoveAllListeners();
        emailButton.onClick.AddListener(() => {
            OnEmailClicked(email.id);
        });
    }

    private void UpdateEmailItem(GameObject emailItem, EmailData email)
    {
        // 更新各个组件的显示
        var claimButton = emailItem.transform.Find("ClaimButton")?.GetComponent<Button>();
        var readFlag = emailItem.transform.Find("ReadFlag");
        var backgroundImage = emailItem.GetComponent<Image>();
        var rewardText = emailItem.transform.Find("RewardText")?.GetComponent<Text>();

        if (claimButton != null)
        {
            claimButton.gameObject.SetActive(!email.isClaimed);
        }

        if (readFlag != null)
        {
            readFlag.gameObject.SetActive(!email.isRead);
        }

        // 更新未读邮件高亮
        if (backgroundImage != null)
        {
            if (!email.isRead)
            {
                backgroundImage.color = new Color(1f, 0.98f, 0.8f, 1f);
            }
            else
            {
                backgroundImage.color = Color.white;
            }
        }

        if (rewardText != null)
        {
            if (email.isClaimed)
            {
                rewardText.text = "奖励: 已领取";
            }
            else
            {
                rewardText.text =$"{email.rewardType} x{email.rewardAmount}";
            }
        }
    }

    private void OnEmailClicked(string emailId)
    {
        // 标记邮件为已读（方法返回void，直接调用）
        GameDataMgr.Instance.MarkEmailAsRead(emailId);

        // 更新UI显示（使用锁保护）
        if (emailItems.ContainsKey(emailId))
        {
            EmailData email = null;
            lock (GameDataMgr.Instance.GetEmailLock())
            {
                var emailData = GameDataMgr.Instance.emailData;
                if (emailData != null && emailData.list != null)
                {
                    email = emailData.list.Find(e => e.id == emailId);
                }
            }
            if (email != null)
            {
                UpdateEmailItem(emailItems[emailId], email);
            }
        }

        // 更新未读邮件数量
        UpdateUnreadCount();
    }

    private void ClaimEmailReward(string emailId)
    {
        bool success = GameDataMgr.Instance.ClaimEmailReward(emailId);
        if (success)
        {
            // 刷新邮件列表
            ShowEmailList();
            // 显示领取成功提示
            ShowTip("领取成功！获得积分奖励");
        }
        else
        {
            ShowTip("领取失败，请重试");
        }
    }

    private void ClaimAllRewards()
    {
        // 先统计未领取数量（使用锁保护）
        int claimCount = 0;
        lock (GameDataMgr.Instance.GetEmailLock())
        {
            var emailData = GameDataMgr.Instance.emailData;
            if (emailData != null && emailData.list != null)
            {
                claimCount = emailData.list.Count(e => !e.isClaimed);
            }
        }
        
        // 使用批量领取方法，只保存一次，提高性能
        int totalReward = GameDataMgr.Instance.ClaimAllRewardsBatch();
        
        if (totalReward > 0)
        {
            ShowTip($"成功领取 {claimCount} 封邮件的奖励，奖励总数 {totalReward}");
            ShowEmailList(); // 刷新列表
        }
        else
        {
            ShowTip("没有可领取的奖励");
        }
    }

    private void DeleteSingleEmail(string emailId)
    {
        // 可以添加确认对话框
        bool success = GameDataMgr.Instance.DeleteEmail(emailId);
        if (success)
        {
            // 从字典中移除并销毁GameObject
            if (emailItems.ContainsKey(emailId))
            {
                Destroy(emailItems[emailId]);
                emailItems.Remove(emailId);
            }

            ShowTip("邮件已删除");
            UpdateUnreadCount();
            UpdateClaimAllButtonState();
            UpdateMarkAllReadButtonState();
        }
    }

    private void DeleteAllEmails()
    {
        // 可以添加确认对话框
        int deleteCount = GameDataMgr.Instance.DeleteAllEmails();
        if (deleteCount > 0)
        {
            // 清空所有邮件项
            ClearAllEmailItems();
            ShowTip($"已删除 {deleteCount} 封邮件");
            UpdateUnreadCount();
            UpdateClaimAllButtonState();
            UpdateMarkAllReadButtonState();
        }
        else
        {
            ShowTip("没有可删除的邮件");
        }
    }

    private void MarkAllAsRead()
    {
        // 调用批量标记已读方法
        int markedCount = GameDataMgr.Instance.MarkAllAsRead();
        
        if (markedCount > 0)
        {
            ShowTip($"已标记 {markedCount} 封邮件为已读");
            // 刷新UI显示
            ShowEmailList();
        }
        else
        {
            ShowTip("没有未读邮件");
        }
    }

    /// <summary>
    /// 清空所有邮件UI项
    /// </summary>
    private void ClearAllEmailItems()
    {
        foreach (var kvp in emailItems)
        {
            if (kvp.Value != null)
            {
                Destroy(kvp.Value);
            }
        }
        emailItems.Clear();
        currentEmailList.Clear();
    }

    /// <summary>
    /// 面板销毁时清理资源
    /// </summary>
    private void OnDestroy()
    {
        ClearAllEmailItems();
    }

    private void UpdateUnreadCount()
    {
        if (txtUnreadCount != null)
        {
            int unreadCount = GameDataMgr.Instance.GetUnreadEmailCount();
            txtUnreadCount.text = $"未读邮件: {unreadCount}";

            // 如果有未读邮件，可以改变文本颜色
            if (unreadCount > 0)
            {
                txtUnreadCount.color = Color.red;
            }
            else
            {
                txtUnreadCount.color = Color.green;
            }
        }
    }

    private void UpdateClaimAllButtonState()
    {
        if (btnClaimAll != null)
        {
            bool hasUnclaimed = GameDataMgr.Instance.HasUnclaimedRewards();
            btnClaimAll.interactable = hasUnclaimed;
        }
    }

    private void UpdateMarkAllReadButtonState()
    {
        if (btnMarkAllRead != null)
        {
            int unreadCount = GameDataMgr.Instance.GetUnreadEmailCount();
            btnMarkAllRead.interactable = unreadCount > 0;
        }
    }

    private void ShowTip(string message)
    {
        Debug.Log(message);
        // 如果有Toast提示系统，可以调用
        // UIManager.Instance.ShowToast(message);
    }
}
