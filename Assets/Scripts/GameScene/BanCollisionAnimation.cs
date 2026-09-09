
using UnityEngine;

public class BanCollisionAnimation : MonoBehaviour
{
    private Animator animator;
    private bool isPlayerInside = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
            Debug.LogWarning($"{gameObject.name} 没有 Animator 组件");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsPlayer(other.gameObject) && !isPlayerInside)
        {
            isPlayerInside = true;
            PlayAnimation();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (IsPlayer(other.gameObject) && isPlayerInside)
        {
            isPlayerInside = false;
            StopAnimation();
        }
    }

    // 添加公共方法供PlayerController调用
    public void PlayAnimation()
    {
        if (animator != null)
        {
            animator.SetTrigger("Jump");
            Debug.Log($"玩家进入 {gameObject.name}，播放 Jump 动画");
        }
    }

    public void StopAnimation()
    {
        if (animator != null)
        {
            // Jump 使用 Trigger，不需要手动停止
            Debug.Log($"玩家离开 {gameObject.name}，Jump 动画已自然结束");
        }
    }

    private bool IsPlayer(GameObject obj)
    {
        // 直接检查Tag
        if (obj.CompareTag("Player"))
            return true;

        // 检查父物体（支持玩家有子碰撞器的情况）
        Transform parent = obj.transform.parent;
        while (parent != null)
        {
            if (parent.CompareTag("Player"))
                return true;
            parent = parent.parent;
        }

        return false;
    }
}