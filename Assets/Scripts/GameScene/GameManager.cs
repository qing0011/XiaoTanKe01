using UnityEngine;
using TMPro;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private int currentScore = 0;
    private int bestScore = 0;
    public GamePanel gamePanel; // 在 Inspector 中拖拽赋值
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        // 加载最佳分数
        LoadBestScore();
        // 初始化 UI
        if (gamePanel != null)
        {
            gamePanel.UpdateScore(currentScore);
            gamePanel.UpdateBest(bestScore);
        }
    }
    // 加分方法
    public void AddScore(int points)
    {
        currentScore += points;

        // 更新 UI
        if (gamePanel != null)
        {
            gamePanel.UpdateScore(currentScore);

            // 检查是否打破记录
            if (currentScore > bestScore)
            {
                bestScore = currentScore;
                gamePanel.UpdateBest(bestScore);
                SaveBestScore();
            }
        }
    }
    // 加载最佳分数
    void LoadBestScore()
    {
        bestScore = PlayerPrefs.GetInt("Best", 0);

        // 如果有 JSON 数据，也可以从 JSON 加载
        // GameData gameData = JsonMgr.Instance.LoadData<GameData>("GameData");
        // if (gameData != null)
        // {
        //     bestScore = gameData.bestScore;
        // }
    }
    // 保存最佳分数
    void SaveBestScore()
    {
        PlayerPrefs.SetInt("Best", bestScore);
        PlayerPrefs.Save();

        // 同时保存到 JSON
        // GameData gameData = JsonMgr.Instance.LoadData<GameData>("GameData");
        // if (gameData == null) gameData = new GameData();
        // gameData.bestScore = bestScore;
        // JsonMgr.Instance.SaveData(gameData, "GameData");
    }
    // 获取当前分数
    public int GetCurrentScore()
    {
        return currentScore;
    }
    // 重置分数（新游戏时调用）
    public void ResetScore()
    {
        currentScore = 0;
        if (gamePanel != null)
        {
            gamePanel.UpdateScore(currentScore);
        }
    }
}