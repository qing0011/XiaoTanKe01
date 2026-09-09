using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

     [Header("音频源")]
    private AudioSource musicSource;
    private AudioSource sfxSource;

    [Header("背景音乐")]
    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField] private AudioClip gameBackgroundMusic; // 游戏场景背景音乐

    [Header("音量设置")]
    [Range(0f, 1f)]
    [SerializeField] private float masterVolume = 1f;
    [Range(0f, 1f)]
    [SerializeField] private float musicVolume = 1f;
    [Range(0f, 1f)]
    [SerializeField] private float sfxVolume = 1f;

    [Header("音频资源库")]
    [SerializeField] private List<AudioClipInfo> soundLibrary;

    private Dictionary<string, AudioClip> soundDict;

    [System.Serializable]
    public class AudioClipInfo
    {
        public string soundName;
        public AudioClip clip;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Initialize();
        }
        else
        {
            Destroy(gameObject);
        }
    }

  private void Initialize()
{
    // =========================
    // 创建背景音乐 AudioSource
    // =========================
    if (musicSource == null)
    {
        musicSource = gameObject.AddComponent<AudioSource>();
    }

    musicSource.loop = true;
    musicSource.playOnAwake = false;
    musicSource.spatialBlend = 0f;
    musicSource.dopplerLevel = 0f;

    // 设置背景音乐
    musicSource.clip = backgroundMusic;

    // =========================
    // 创建音效 AudioSource
    // =========================
    if (sfxSource == null)
    {
        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.playOnAwake = false;
        sfxSource.spatialBlend = 0f;
        sfxSource.dopplerLevel = 0f;
    }

    // =========================
    // 初始化音效字典
    // =========================
    soundDict = new Dictionary<string, AudioClip>();

    if (soundLibrary != null)
    {
        foreach (var item in soundLibrary)
        {
            if (!soundDict.ContainsKey(item.soundName))
            {
                soundDict.Add(item.soundName, item.clip);
            }
        }
    }

    // =========================
    // 加载音量设置
    // =========================
    LoadVolumeSettings();

    // 应用音量
    ApplyVolumes();

    // =========================
    // 自动播放背景音乐
    // =========================
    if (backgroundMusic != null && musicVolume > 0)
    {
        musicSource.Play();

        Debug.Log("【AudioManager】背景音乐开始播放");
    }
    else
    {
        Debug.LogWarning("【AudioManager】背景音乐未播放");
    }
}
    
    

    private void LoadVolumeSettings()
    {
        if (GameDataMgr.Instance != null && GameDataMgr.Instance.musicData != null)
        {
            MusicData data = GameDataMgr.Instance.musicData;
            musicVolume = data.musicOpen ? data.musicValue : 0f;
            sfxVolume = data.soundOpen ? data.soundValue : 0f;
            Debug.Log($"【AudioManager】已加载音量设置 - 音乐: {musicVolume}, 音效: {sfxVolume}");
        }
        else
        {
            Debug.LogWarning("【AudioManager】GameDataMgr尚未初始化，使用默认音量设置");
        }
    }

    #region 背景音乐控制

    public void PlayMusic(AudioClip music, bool restart = false)
    {
        // 如果传入 null，使用默认背景音乐
        if (music == null)
        {
            music = backgroundMusic;
        }
        
        if (music == null)
        {
            Debug.LogWarning("无法播放空的背景音乐");
            return;
        }

        // 如果音乐已关闭（音量为0），不播放
        if (musicVolume <= 0)
        {
            Debug.Log("【AudioManager】音乐已关闭，不播放");
            return;
        }

        if (!restart && musicSource.clip == music && musicSource.isPlaying)
            return;

        musicSource.clip = music;
        musicSource.Play();
    }

    public void PlayMusicByName(string musicName)
    {
        var clip = GetClipByName(musicName);
        if (clip != null)
            PlayMusic(clip);
    }

    /// <summary>
    /// 播放游戏场景背景音乐
    /// </summary>
    public void PlayGameBackgroundMusic()
    {
        PlayMusic(gameBackgroundMusic);
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void PauseMusic()
    {
        musicSource.Pause();
    }

    public void ResumeMusic()
    {
        musicSource.UnPause();
    }

    #endregion

    #region 音效控制

    public void PlaySound(AudioClip clip, float volumeScale = 1f)
    {
        if (clip == null)
        {
            Debug.LogWarning("无法播放空音效");
            return;
        }

        sfxSource.PlayOneShot(clip, volumeScale);
    }

    public void PlaySound(string soundName, float volumeScale = 1f)
    {
        var clip = GetClipByName(soundName);
        if (clip != null)
        {
            PlaySound(clip, volumeScale);
        }
        else
        {
            Debug.LogWarning($"未找到音效: {soundName}");
        }
    }

    public void PlaySoundOneShot(string soundName, float volumeScale = 1f)
    {
        var clip = GetClipByName(soundName);
        if (clip != null)
        {
            sfxSource.PlayOneShot(clip, volumeScale);
        }
    }

    public void PlaySoundAtPosition(string soundName, Vector3 position, float volumeScale = 1f)
    {
        var clip = GetClipByName(soundName);
        if (clip != null)
        {
            PlayClipAtPointInternal(clip, position, volumeScale);
        }
    }

    public void PlaySoundAtPosition(AudioClip clip, Vector3 position, float volumeScale = 1f)
    {
        if (clip != null)
        {
            PlayClipAtPointInternal(clip, position, volumeScale);
        }
    }

    private void PlayClipAtPointInternal(AudioClip clip, Vector3 position, float volumeScale)
    {
        GameObject tempObj = new GameObject("TempAudio");
        tempObj.transform.position = position;
        AudioSource tempSource = tempObj.AddComponent<AudioSource>();
        tempSource.clip = clip;
        tempSource.volume = volumeScale * sfxVolume * masterVolume;
        tempSource.spatialBlend = 1f;
        tempSource.dopplerLevel = 0f;
        tempSource.Play();
        MonoBehaviour.Destroy(tempObj, clip.length + 0.1f);
    }

    #endregion

    #region 音量控制

    public void SetMasterVolume(float volume)
    {
        masterVolume = Mathf.Clamp01(volume);
        ApplyVolumes();
    }

    public void SetMusicVolume(float volume)
    {
        float oldVolume = musicVolume;
        musicVolume = Mathf.Clamp01(volume);
        ApplyVolumes();
        
        // 当音量从有变为无，停止播放
        if (oldVolume > 0 && musicVolume == 0 && musicSource != null && musicSource.isPlaying)
        {
            musicSource.Stop();
        }
        // 当音量从无变为有，恢复播放
        else if (oldVolume == 0 && musicVolume > 0 && musicSource != null && musicSource.clip != null && !musicSource.isPlaying)
        {
            musicSource.Play();
        }
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        ApplyVolumes();
    }

    private void ApplyVolumes()
    {
        if (musicSource != null)
            musicSource.volume = masterVolume * musicVolume;

        if (sfxSource != null)
            sfxSource.volume = masterVolume * sfxVolume;
    }

    public float GetMasterVolume() => masterVolume;
    public float GetMusicVolume() => musicVolume;
    public float GetSFXVolume() => sfxVolume;

    #endregion

    #region 其他方法

    /// <summary>
    /// 刷新音量设置（从GameDataMgr重新加载）
    /// 用于处理数据异步加载完成后的同步
    /// </summary>
    public void RefreshVolumeSettings()
    {
        LoadVolumeSettings();
        ApplyVolumes();
        Debug.Log($"【AudioManager】音量设置已刷新 - 音乐: {musicVolume}, 音效: {sfxVolume}");
    }

    private AudioClip GetClipByName(string name)
    {
        if (soundDict == null)
            return null;

        soundDict.TryGetValue(name, out AudioClip clip);
        return clip;
    }

    public void AddSoundToLibrary(string soundName, AudioClip clip)
    {
        if (soundDict.ContainsKey(soundName))
        {
            Debug.LogWarning($"音效 {soundName} 已存在，将被替换");
            soundDict[soundName] = clip;
        }
        else
        {
            soundDict.Add(soundName, clip);
            soundLibrary.Add(new AudioClipInfo { soundName = soundName, clip = clip });
        }
    }

    public void MuteAll(bool mute)
    {
        AudioListener.pause = mute;
    }

    #endregion
}