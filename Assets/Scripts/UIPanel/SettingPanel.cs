using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
#if UNITY_WEBGL && !UNITY_EDITOR
using WeChatWASM;  //只在微信小游戏平台引用
#endif

public class SettingPanel : BasePanel
{
    public Button btnClose;
    public Toggle togMusic;
    public Toggle togSound;
    public Button btnResetLevels;
    //public Slider sliderMusic;
    //public Slider sliderSound;
    //public Button btnQuit;
    public override void Init()
    {
        //初始化面板显示的内容 根绝本地存储的设置数据来初始化
        MusicData data = GameDataMgr.Instance.musicData;
        // ❗先清掉监听
        togMusic.onValueChanged.RemoveAllListeners();
        togSound.onValueChanged.RemoveAllListeners();
        //初始化开关控制的状态
        togMusic.isOn = data.musicOpen;
        togSound.isOn = data.soundOpen;

        ////退出游戏
        //btnQuit.onClick.AddListener(() =>
        //{
        //    Debug.Log("退出游戏");

        //   #if UNITY_WEBGL && !UNITY_EDITOR
        //// 微信小游戏退出方式
        //    WeChatWASM.WX.ExitMiniProgram();
        //   #else
        //    Application.Quit();
        //   #endif
        //});
        //关闭按钮
        btnClose.onClick.AddListener(() =>
        {
            SyncUIToData();
            // 调试当前值
            Debug.Log($"保存前 - 音量: {GameDataMgr.Instance.musicData.soundValue}, 开关: {GameDataMgr.Instance.musicData.soundOpen}");
            //只有关闭面板才会记录数据（节省性能）
            GameDataMgr.Instance.SaveMusicData();
            //隐藏自己印象设置面板
            UIManager.Instance.HidePanel<SettingPanel>();
        });
        togMusic.onValueChanged.AddListener((v) =>
            {
                //记录开关数据
                GameDataMgr.Instance.musicData.musicOpen = v;
                //让背景音乐进行开关（统一由AudioManager控制）
                if (AudioManager.Instance != null)
                {
                    AudioManager.Instance.SetMusicVolume(v ? GameDataMgr.Instance.musicData.musicValue : 0f);
                }
            });
        togSound.onValueChanged.AddListener((v) =>
        {
            //记录开关数据
            GameDataMgr.Instance.musicData.soundOpen = v;
            //让音效进行开关
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.SetSFXVolume(v ? GameDataMgr.Instance.musicData.soundValue : 0f);
            }
        });
        
        // 重置所有关卡按钮
        if (btnResetLevels != null)
        {
            btnResetLevels.onClick.RemoveAllListeners();
            btnResetLevels.onClick.AddListener(() =>
            {
                // 重置所有关卡
                GameLevelMgr.Instance.ResetAllLevels();
                // 显示提示信息
                Debug.Log("所有关卡已重置，将从第一关重新开始");
                // 可以在这里添加一个提示面板
            });
        }
       
    }

    public override void ShowMe()
    {
        base.ShowMe();
        // 每次显示面板时，重新初始化开关状态，确保与GameDataMgr中的实际状态一致
        MusicData data = GameDataMgr.Instance.musicData;
        togMusic.isOn = data.musicOpen;
        togSound.isOn = data.soundOpen;
    }
    // 新增方法：同步UI到数据
    private void SyncUIToData()
    {
        // 直接使用Toggle的当前isOn值
        GameDataMgr.Instance.musicData.musicOpen = togMusic.isOn;
        GameDataMgr.Instance.musicData.soundOpen = togSound.isOn;

        Debug.Log($"同步UI到数据 - 音乐UI: {togMusic.isOn}, 音效UI: {togSound.isOn}");
    }
}
