using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EndLevel : MonoBehaviour
{
    public Button nextLevel;
    public string levelName;
    public float delayBeforeLoad = 2f;  // 延迟时间

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            // 激活按钮
            nextLevel.gameObject.SetActive(true);

            // 延迟加载场景，让按钮有时间显示
            StartCoroutine(LoadSceneWithDelay());
        }
    }

    IEnumerator LoadSceneWithDelay()
    {
        yield return new WaitForSeconds(delayBeforeLoad);
        SceneManager.LoadScene(levelName);
    }
}