using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneMain : MonoBehaviour
{
    private void Start()
    {
        PlayBackgroundMusic();
        StartCoroutine(ShowPanelDelayed());
    }

    private void PlayBackgroundMusic()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayMusic(null);
        }
        else
        {
            Debug.LogError("AudioManager.Instance is null!");
        }
    }

    private IEnumerator ShowPanelDelayed()
    {
        yield return null; // 等待一帧
        if (UIManager.Instance != null)
        {
            UIManager.Instance.ShowPanel<ChooseScenePanel>();
        }
        else
        {
            Debug.LogError("UIManager.Instance is null!");
        }
    }


}
