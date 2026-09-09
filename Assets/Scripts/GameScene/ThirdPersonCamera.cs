using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target;
    public float distance = 10.0f;
    public float height = 5.0f;
    public float smoothSpeed = 0.5f;
    
    void LateUpdate()
    {
        if (target != null)
        {
            // 计算目标位置：在目标上方和后方
            Vector3 targetPosition = target.position + Vector3.up * height - target.forward * distance;
            
            // 平滑移动摄像机
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);
            
            // 让摄像机始终看向目标
            transform.LookAt(target);
        }
    }
}