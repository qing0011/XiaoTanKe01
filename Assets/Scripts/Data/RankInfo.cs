using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 排行榜单条数据
/// </summary>
public class RankData
{
    public string name;
    public int score;
    public float time;
    public int bestLevel;

    public RankData()
    {

    }

    public RankData(string name, int score, float time, int bestLevel)
    {
        this.name = name;
        this.score = score;
        this.time = time;
        this.bestLevel = bestLevel;
    }
}

/// <summary>
/// 排行榜列表
/// </summary>
public class RankList
{
    public List<RankData> list;

    public RankList()
    {
        list = new List<RankData>();
    }
}
