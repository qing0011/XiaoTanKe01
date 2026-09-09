using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SignInSaveData
{
    // 基础签到数据
    public int signInCount;
    public string lastSignInTime;
    
    // 大奖励系统数据
    public int currentBigRewardRound;     // 当前大奖励轮次 (1-5)
    public int consecutiveDays;            // 当前连续签到天数 (0-6)
    public bool bigRewardAvailable;        // 当前大奖励是否可领取（连续签到满7天）
    
    // 各轮大奖励领取状态（索引0不用，1-5对应5个大奖励）
    public bool[] bigRewardClaimed = new bool[6];
}