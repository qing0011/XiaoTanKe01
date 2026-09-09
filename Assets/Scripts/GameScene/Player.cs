using UnityEngine;
public class Player : MonoBehaviour
{
    public System.Action<int> OnScore;  
    public System.Action OnDead;        
    private int score;

    private Animator playerAnimator;
    private bool isOnBan = false; // 追踪是否在Ban层上
    void Start()
    {
        score = 0;
        GameDataMgr.Instance.labScore = 0;
        playerAnimator = GetComponent<Animator>();
    }

    public void AddScore()
    {
        score++;
        GameDataMgr.Instance.labScore = score;
        OnScore?.Invoke(score); 
        // 更新GamePanel显示
        var gamePanel = UIManager.Instance.GetPanel<GamePanel>();
        if (gamePanel != null)
        {
            gamePanel.UpdateScore(score);
            // 检查是否是新的最高分
            if (score > GameDataMgr.Instance.scoreData.maxScore)
            {
                GameDataMgr.Instance.TryRefreshMaxScore(score);
                gamePanel.UpdateBest(GameDataMgr.Instance.scoreData.maxScore);
            }
        }
    }
    public void AddScore(int value)
    {
        score += value;
        GameDataMgr.Instance.labScore = score;
        OnScore?.Invoke(score);
        // 更新GamePanel显示
        var gamePanel = UIManager.Instance.GetPanel<GamePanel>();
        if (gamePanel != null)
        {
            gamePanel.UpdateScore(score);
            // 检查是否是新的最高分
            if (score > GameDataMgr.Instance.scoreData.maxScore)
            {
                GameDataMgr.Instance.TryRefreshMaxScore(score);
                gamePanel.UpdateBest(GameDataMgr.Instance.scoreData.maxScore);
            }
        }
    }
    public void Dead()
    {
        // 记录当前关卡的最高分
        GameDataMgr.Instance.TryRefreshMaxScore(score);
        // 播放死亡动画
        if (TryGetComponent<Animator>(out var animator))
        {
            animator.SetTrigger("Death");
        }

        // 播放死亡音效
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySound("Step");
        }
    
        OnDead?.Invoke(); 
    }
    public int GetScore()
    {
        return score;
    }
    // 触发器碰撞检测
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enmy"))
        {
            Dead();
             // 播放踩花瓣的音效
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySound("Lose");
            }
            Time.timeScale = 0f;

            UIManager.Instance.ShowPanel<FailPanel>();
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("ScoreBar"))
        {
            AddScore();
            // 播放踩花瓣特效
           PlayStepScoreEffect(other.transform.position);
            other.gameObject.SetActive(false);
            // 播放踩花瓣的音效
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySound("Score");
            }
        }

        Debug.Log($"触发碰撞: {other.gameObject.name}, Layer: {LayerMask.LayerToName(other.gameObject.layer)}");
        // 接触到 Ban 层级后触发 Sit 动画（触发器版本）
        if (other.gameObject.layer == LayerMask.NameToLayer("Ban"))
            {
                Debug.Log("检测到Ban层！准备播放动画");
                isOnBan = true;
                HandleBanCollision(other.transform);
            }
    }
    // 物理碰撞检测（处理非触发器的Ban层对象）
    private void OnTriggerExit(Collider other)
    {
        // 离开 Ban 层级时停止玩家的 Step 动画
        if (other.gameObject.layer == LayerMask.NameToLayer("Ban"))
        {
            Debug.Log("Player exited Ban layer (Trigger)");
            isOnBan = false;
        }
        // 关闭特效
       StopStepEffect(other.transform);
    }
    // 物理碰撞检测（处理非触发器的Ban层对象）
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ban"))
        {
            Debug.Log("Player entered Ban layer (Collision), position: " + collision.transform.position);
            isOnBan = true;
            HandleBanCollision(collision.transform);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ban"))
        {
            Debug.Log("Player exited Ban layer (Collision)");
            isOnBan = false;
        }
        // 关闭特效
       StopStepEffect(collision.transform);
    }
    // 处理Ban层碰撞的统一逻辑
    private void HandleBanCollision(Transform banTransform)
    {
        // 触发玩家的 Jump 动画（通过 Trigger）
        if (playerAnimator != null)
        {
            playerAnimator.SetTrigger("Jump");
        }

        // 播放坐下音效
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySound("Step");
        }

        // 播放踩花瓣特效
        PlayStepEffect(banTransform);
    }
   
    private void PlayStepEffect(Transform banTransform)
    {
        Debug.Log("Playing Step01 effect on Ban object: " + banTransform.name);
        Transform stepEffectTransform = banTransform.Find("Step01");
        if (stepEffectTransform != null)
        {
            Debug.Log("Step01 effect found, activating it");
            stepEffectTransform.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogError("Step01 effect not found as child of Ban object: " + banTransform.name);
        }
    }
     private void PlayStepScoreEffect(Vector3 position)
    {
        Debug.Log("Playing Score01 effect at position: " + position);
        GameObject stepEffect = Resources.Load<GameObject>("Effects/Score01");
        if (stepEffect != null)
        {
            Debug.Log("Score01 effect loaded successfully");
            GameObject effectInstance = Instantiate(stepEffect, position, Quaternion.identity);
            ParticleSystem particleSystem = effectInstance.GetComponent<ParticleSystem>();
            if (particleSystem != null)
            {
                Destroy(effectInstance, particleSystem.main.duration);
            }
            else
            {
                Destroy(effectInstance, 1f);
            }
        }
        else
        {
            Debug.LogError("Score01 effect not found at Resources/Effects/Score01");
        }
    }
    private void StopStepEffect(Transform banTransform)
{
    Debug.Log("Stopping Step01 effect on Ban object: " + banTransform.name);

    Transform stepEffectTransform = banTransform.Find("Step01");

    if (stepEffectTransform != null)
    {
        stepEffectTransform.gameObject.SetActive(false);
    }
}
}