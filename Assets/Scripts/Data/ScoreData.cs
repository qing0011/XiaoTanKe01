using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ScoreData
{
   public int maxScore=0;
    //添加
    //当前拥有积分
    public int haveScore =0;
    // 本局已继续的次数（重点）
    public int continueCount = 0;
}

