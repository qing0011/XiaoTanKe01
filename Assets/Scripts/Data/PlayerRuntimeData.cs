using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerRuntimeData
{
    public int level = 1;

    public int attack = 10;
    public int defense = 5;

    public int bulletCount = 50;

    public int maxHp = 100;
    public int hp = 100;
    // 当前装备的武器 ID
    public int weaponId = -1; // -1 = 未装备武器

    // ============ 体力系统 ============
    public int maxStamina = 3;       // 体力上限
    public int currentStamina = 3;   // 当前体力
    public int adCountToday = 0;     // 今日广告次数
    public string lastResetDate = ""; // 上次重置日期（用于判断是否需要重置体力）

    // ========== 金币相关 ==========
    public int coin = 0;                    // 当前金币数量
    public int todayBuyStaminaCount = 0;    // 今日已购买体力次数
    public string lastBuyResetDate = " ";   // 上次重置购买次数的日期

    // 构造函数：初始化日期为今天
    public PlayerRuntimeData()
    {
        // 首次创建时设置日期为今天，防止数据加载前误重置体力
        if (string.IsNullOrEmpty(lastResetDate))
        {
            lastResetDate = System.DateTime.Now.ToString("yyyy-MM-dd");
        }
    }
}

