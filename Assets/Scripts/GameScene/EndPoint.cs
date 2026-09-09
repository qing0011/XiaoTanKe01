using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndPoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
       // Debug.Log("[EndPoint] 检测到物体: " + other.name + ", 标签: " + other.tag);
        if (other.CompareTag("Player"))
        {
            //Debug.Log("[EndPoint] 检测到玩家到达终点！关卡ID: " + GameDataMgr.Instance.currentSceneId);
            // 由于 isRunning 保护级别限制，无法直接访问，移除相关日志
            //调用GameLevelMgr的OnLevelFinish方法处理关卡完成逻辑
            GameLevelMgr.Instance.OnLevelFinish();
            
        }
    }
}
