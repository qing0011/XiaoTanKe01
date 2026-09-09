using System;
using System.Collections.Generic;

/// <summary>
/// 单封邮件数据
/// </summary>
[Serializable]
public class EmailData
{

    public string id;           // 邮件唯一ID /// 邮件唯一ID// 本地模式使用自增ID// 云端模式使用服务器ID
    public string title;        // 邮件标题
    public string content;      // 邮件内容
    public string sendTime;     // 发送时间
    public string expireTime; // 邮件过期时间
    public string mailType; // 邮件类型
    public string rewardType;// 奖励类型
    public int rewardAmount;// 奖励数量
    public bool isRead;         // 是否已读
    public bool isClaimed;      // 是否已领取奖励
    public bool isDeleted;//

    // 无参构造函数（用于JSON序列化）
    public EmailData() 
    {
    }

    // 带参构造函数（用于创建新邮件）
    public EmailData(string id, string title, string content, string rewardType, int rewardAmount)
    {
        this.id = id;
        this.title = title;
        this.content = content;

        this.rewardType = rewardType;
        this.rewardAmount = rewardAmount;

        this.sendTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        this.expireTime = DateTime.Now.AddDays(7).ToString("yyyy-MM-dd HH:mm:ss");

        this.mailType = "System";

        this.isRead = false;
        this.isClaimed = false;
        this.isDeleted = false;
    }
}

/// <summary>
/// 邮件列表数据
/// </summary>
[Serializable]
public class EmailList
{
    public List<EmailData> list = new();
}