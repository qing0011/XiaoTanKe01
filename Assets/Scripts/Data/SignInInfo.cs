using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 每日签到奖励配置
public class SignInInfo 
{
    public int id;
    public string res;
    public int reward;
    public string rewardType = "金币"; // 奖励类型
}

// 大奖励配置
public class BigRewardInfo
{
    public int round;           // 大奖励轮次 (1-5)
    public string name;         // 大奖励名称
    public int reward;          // 奖励数量
    public string rewardType;   // 奖励类型
}
