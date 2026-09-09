using LitJson;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class JsonMgr
{
    private static JsonMgr instance = new JsonMgr();
    public static JsonMgr Instance => instance;
    private JsonMgr() { }
    // 判断是否是存档文件
    private bool IsSaveFile(string fileName)
    {
        return fileName == "SignInSave"
            || fileName == "scoreData"
            || fileName == "Rank"
            || fileName == "LevelSave"
            || fileName == "EmailData"
            || fileName == "PlayerData";
    }
    // 保存数据（统一用 LitJson）
    public void SaveData(object data, string fileName)
    {
        string path = Application.persistentDataPath + "/" + fileName + ".json";
        string jsonStr = JsonMapper.ToJson(data);
        File.WriteAllText(path, jsonStr);
    }
    public T LoadData<T>(string fileName) where T : new()
    {
        string path;
        // ===== 存档文件 =====
        if (IsSaveFile(fileName))
        {
            path = Application.persistentDataPath + "/" + fileName + ".json";
            if (!File.Exists(path))
            {
                Debug.LogWarning("存档不存在，创建默认数据: " + fileName);
                return new T();
            }
            string jsonStr = File.ReadAllText(path);
            return JsonMapper.ToObject<T>(jsonStr);
        }
        // ===== 配置文件（StreamingAssets）=====
        path = Application.streamingAssetsPath + "/" + fileName + ".json";
        
        // WebGL平台需要使用UnityWebRequest加载
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            // 在WebGL上同步加载可能会有问题，建议使用异步方法
            Debug.LogWarning("WebGL平台建议使用LoadDataAsync方法加载配置文件");
            return new T();
        }
        
        if (File.Exists(path))
        {
            string jsonStr = File.ReadAllText(path);
            return JsonMapper.ToObject<T>(jsonStr);
        }
        else
        {
            Debug.LogError("StreamingAssets中找不到文件: " + path);
            return new T();
        }
    }
    // 异步加载（支持WebGL平台）
    public IEnumerator LoadDataAsync<T>(string fileName, System.Action<T> callback) where T : new()
    {
        T data = new T();
        string path;
        // ===== 存档文件 =====
        if (IsSaveFile(fileName))
        {
            path = Application.persistentDataPath + "/" + fileName + ".json";
            if (File.Exists(path))
            {
                string jsonStr = File.ReadAllText(path);
                data = JsonMapper.ToObject<T>(jsonStr);
            }
            callback?.Invoke(data);
            yield break;
        }
        // ===== 配置文件（StreamingAssets）=====
        path = Application.streamingAssetsPath + "/" + fileName + ".json";
        
        // WebGL平台或远程URL需要使用UnityWebRequest
        if (Application.platform == RuntimePlatform.WebGLPlayer || path.StartsWith("http://") || path.StartsWith("https://"))
        {
            using (UnityWebRequest www = UnityWebRequest.Get(path))
            {
                yield return www.SendWebRequest();
                
                if (www.result == UnityWebRequest.Result.Success)
                {
                    string jsonStr = www.downloadHandler.text;
                    data = JsonMapper.ToObject<T>(jsonStr);
                }
                else
                {
                    Debug.LogError("加载StreamingAssets文件失败: " + path + " Error: " + www.error);
                }
            }
        }
        else
        {
            if (File.Exists(path))
            {
                string jsonStr = File.ReadAllText(path);
                data = JsonMapper.ToObject<T>(jsonStr);
            }
            else
            {
                Debug.LogError("StreamingAssets中找不到文件: " + path);
            }
        }
        callback?.Invoke(data);
        yield return null;
    }
}