using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SignInPanel : BasePanel
{
    // 最大连续签到天数（每个大奖励周期）
    private const int MaxConsecutiveDays = 7;
    // 最大大奖励轮次
    private const int MaxBigRewardRound = 4;

    // 上次签到日期
    private DateTime _lastDay;
    // 当前签到次数（本月/全局）
    private int _signInCount;
    // 当前连续签到天数
    private int _consecutiveDays;
    // 当前大奖励轮次
    private int _currentBigRewardRound;
    // 本轮大奖励是否可领取（连续签到满7天）
    private bool _bigRewardAvailable;
    // 各轮大奖励领取状态（1-5轮）
    private bool[] _bigRewardClaimed = new bool[MaxBigRewardRound + 1];

    // 是否已达到最大签到次数
    private bool _isMaxSignInCount;
    // 是否显示下次签到倒计时
    private bool _showNextSignInTime;

    // UI元素
    public Toggle[] _SignInToggleTips;          // 签到格子按钮数组（7个）
    public Button _signInBtn;                    // 签到按钮
    public TextMeshProUGUI _SignInBtnContent;    // 签到按钮文字
    public Button _CloseBtn;                     // 关闭按钮
    public TextMeshProUGUI _ConsecutiveDaysText; // 连续签到天数显示
    public Image _TipsMax; // 连续签到天数显示

    // 大奖励按钮（5个）
    public Button[] _BigRewardButtons;           // 大奖励按钮数组（5个）
    public TextMeshProUGUI[] _BigRewardButtonTexts; // 大奖励按钮文字数组
    public TextMeshProUGUI[] _BigRewardStatusTexts; // 大奖励状态文字数组

    // 测试按钮忽略连续签到中断检测
    private bool _ignoreConsecutiveCheckForTest = false;

    [Header("测试按钮")]
   // public Button _TestClearBtn;
    public Button _TestNextDayBtn;

    public override void Init()
    {
        _signInBtn.onClick.RemoveAllListeners();
        _signInBtn.onClick.AddListener(OnSignInBtnClick);

        _CloseBtn.onClick.RemoveAllListeners();
        _CloseBtn.onClick.AddListener(() =>
        {
            UIManager.Instance.HidePanel<SignInPanel>();
        });

        // 初始化大奖励按钮事件
        for (int i = 0; i < _BigRewardButtons.Length && i < MaxBigRewardRound; i++)
        {
            int round = i + 1;
            if (_BigRewardButtons[i] != null)
            {
                _BigRewardButtons[i].onClick.RemoveAllListeners();
                _BigRewardButtons[i].onClick.AddListener(() => OnBigRewardClick(round));
            }
        }

        //if (_TestClearBtn != null)
        //{
        //    _TestClearBtn.gameObject.SetActive(true);
        //    _TestClearBtn.onClick.AddListener(ClearSignInForTest);
        //}

        if (_TestNextDayBtn != null)
        {
            _TestNextDayBtn.gameObject.SetActive(true);
            _TestNextDayBtn.onClick.RemoveAllListeners();
            _TestNextDayBtn.onClick.AddListener(SimulateNextDay);
        }
    }

    private void ClearSignInForTest()
    {
        Debug.Log("测试：重置签到数据");
        SignInSaveData save = GameDataMgr.Instance.signInSaveData;
        save.signInCount = 0;
        save.lastSignInTime = "";
        save.consecutiveDays = 0;
        save.currentBigRewardRound = 1;
        save.bigRewardAvailable = false;
        for (int i = 1; i <= MaxBigRewardRound; i++)
        {
            save.bigRewardClaimed[i] = false;
        }
        GameDataMgr.Instance.SaveSignInData();
        LoadData();
        UpdateUI();
        UpdateBigRewardUI();
        _signInBtn.interactable = true;
        _SignInBtnContent.text = "签到";
        _showNextSignInTime = false;
    }

    private void SimulateNextDay()
    {
        Debug.Log("测试：模拟到前一天");
        _ignoreConsecutiveCheckForTest = true;

        SignInSaveData save = GameDataMgr.Instance.signInSaveData;
        if (!string.IsNullOrEmpty(save.lastSignInTime))
        {
            DateTime last = DateTime.Parse(save.lastSignInTime);
            save.lastSignInTime = last.AddDays(-1).ToString();
        }
        else
        {
            save.lastSignInTime = DateTime.Now.AddDays(-1).ToString();
        }
        GameDataMgr.Instance.SaveSignInData();

        LoadData();
        UpdateUI();
        UpdateBigRewardUI();
        _signInBtn.interactable = true;
        _SignInBtnContent.text = "签到";
        _showNextSignInTime = false;
    }

    private void OnEnable()
    {
        LoadData();
       // _isMaxSignInCount = _bigRewardAvailable && _bigRewardClaimed[_currentBigRewardRound];
        
        UpdateUI();
        UpdateBigRewardUI();

        if (CanSignToday())
        {
            _signInBtn.interactable = true;
            _SignInBtnContent.text = "签到";
            _showNextSignInTime = false;
        }
        else
        {
            _signInBtn.interactable = false;
            _showNextSignInTime = true;
        }
    }

    protected override void Update()
    {
        base.Update();

        if (_showNextSignInTime)
        {
            TimeSpan interval = DateTime.Now.Date.AddDays(1) - DateTime.Now;
            if (interval > TimeSpan.Zero)
            {
                _SignInBtnContent.text =$"{interval.Hours:D2}:{interval.Minutes:D2}:{interval.Seconds:D2}";
            }
            else
            {
                _showNextSignInTime = false;
                _signInBtn.interactable = true;
                _SignInBtnContent.text = "签到";
            }
        }
    }

    private void LoadData()
    {
        SignInSaveData save = GameDataMgr.Instance.signInSaveData;
        _signInCount = save.signInCount;
        _consecutiveDays = save.consecutiveDays;
        _currentBigRewardRound = save.currentBigRewardRound;
        _bigRewardAvailable = save.bigRewardAvailable;
        
        // 加载各轮大奖励领取状态
        for (int i = 1; i <= MaxBigRewardRound; i++)
        {
            _bigRewardClaimed[i] = save.bigRewardClaimed[i];
        }

        if (string.IsNullOrEmpty(save.lastSignInTime))
        {
            _lastDay = DateTime.MinValue;
        }
        else
        {
            _lastDay = DateTime.Parse(save.lastSignInTime);
        }

        // 检测连续签到是否中断（非测试模式）
        if (!_ignoreConsecutiveCheckForTest && !IsConsecutiveSignIn())
        {
            ResetConsecutiveData();
        }

        _ignoreConsecutiveCheckForTest = false;
    }

    private bool IsConsecutiveSignIn()
    {
        if (_lastDay == DateTime.MinValue)
            return true;

        DateTime expectedLastDay = DateTime.Now.Date.AddDays(-1);
        return _lastDay.Date >= expectedLastDay.Date;
    }

    private void ResetConsecutiveData()
    {
        Debug.Log("连续签到中断，重置连续签到数据");
        _consecutiveDays = 0;
        _bigRewardAvailable = false;
        
        SignInSaveData save = GameDataMgr.Instance.signInSaveData;
        save.consecutiveDays = 0;
        save.bigRewardAvailable = false;
        save.currentBigRewardRound = 1;
        _currentBigRewardRound = 1;
        GameDataMgr.Instance.SaveSignInData();
    }

    private void OnSignInBtnClick()
    {
        if (!CanSignToday())
            return;

        _signInCount++;
        _consecutiveDays++;
        _lastDay = DateTime.Now;

        SignInSaveData save = GameDataMgr.Instance.signInSaveData;
        save.signInCount = _signInCount;
        save.consecutiveDays = _consecutiveDays;
        save.lastSignInTime = _lastDay.ToString();

        GiveDailyReward();

        int currentRound = (_consecutiveDays - 1) / MaxConsecutiveDays + 1;

        if (_consecutiveDays > MaxBigRewardRound * MaxConsecutiveDays)
        {
            Debug.Log("【签到系统】已完成全部大奖励，重置循环！");

            _consecutiveDays = _consecutiveDays % (MaxBigRewardRound * MaxConsecutiveDays);
            if (_consecutiveDays == 0)
                _consecutiveDays = MaxBigRewardRound * MaxConsecutiveDays;

            save.consecutiveDays = _consecutiveDays;

            for (int i = 1; i <= MaxBigRewardRound; i++)
            {
                _bigRewardClaimed[i] = false;
                save.bigRewardClaimed[i] = false;
            }
            _bigRewardAvailable = false;
            save.bigRewardAvailable = false;

            currentRound = (_consecutiveDays - 1) / MaxConsecutiveDays + 1;
        }

        if (currentRound <= MaxBigRewardRound)
        {
            _currentBigRewardRound = currentRound;
            save.currentBigRewardRound = currentRound;
        }

        if (_consecutiveDays % MaxConsecutiveDays == 0 && _consecutiveDays > 0)
        {
            if (currentRound <= MaxBigRewardRound && !_bigRewardClaimed[currentRound])
            {
                _bigRewardAvailable = true;
                save.bigRewardAvailable = true;
                Debug.Log($"【提示】恭喜连续签到{_consecutiveDays}天！大奖励#{currentRound}已解锁，请前往领取！");
            }
        }

        GameDataMgr.Instance.SaveSignInData();
        UpdateUI();
        UpdateBigRewardUI();

        RefreshBeginPanelDisplay();

        _signInBtn.interactable = false;
        _showNextSignInTime = true;
    }

    private void OnBigRewardClick(int round)
    {
        SignInSaveData save = GameDataMgr.Instance.signInSaveData;
        
        if (round > MaxBigRewardRound)
        {
            Debug.Log($"【大奖励】大奖励#{round}不存在");
            return;
        }

        if (_consecutiveDays < round * MaxConsecutiveDays)
        {
            Debug.Log($"【大奖励】需要连续签到{round * MaxConsecutiveDays}天才能领取");
            return;
        }

        if (_bigRewardClaimed[round])
        {
            Debug.Log($"【大奖励】大奖励#{round}已领取");
            return;
        }

        GiveBigReward(round);
        
        _bigRewardClaimed[round] = true;
        save.bigRewardClaimed[round] = true;

        if (round == _currentBigRewardRound)
        {
            _bigRewardAvailable = false;
            save.bigRewardAvailable = false;
        }

        GameDataMgr.Instance.SaveSignInData();

        UpdateUI();
        UpdateBigRewardUI();

        RefreshBeginPanelDisplay();

        if (CanSignToday())
        {
            _signInBtn.interactable = true;
            _SignInBtnContent.text = "签到";
            _showNextSignInTime = false;
        }
    }

    /// <summary>
    /// 刷新BeginPanel的显示
    /// </summary>
    private void RefreshBeginPanelDisplay()
    {
        BeginPanel beginPanel = UnityEngine.Object.FindObjectOfType<BeginPanel>();
        if (beginPanel != null)
        {
            beginPanel.UpdateTotalScoreDisplay();
            beginPanel.UpdateStaminaDisplay();
            beginPanel.UpdateScoreDisplay();
            Debug.Log("【签到奖励】已刷新BeginPanel显示");
        }
    }

    private void GiveDailyReward()
    {
        // 根据当前连续签到天数获取对应的每日奖励（1-7天）
        int dayIndex = (_consecutiveDays - 1) % MaxConsecutiveDays;
        if (dayIndex < GameDataMgr.Instance.signInInfoList.Count)
        {
            SignInInfo rewardInfo = GameDataMgr.Instance.signInInfoList[dayIndex];
            if (rewardInfo.rewardType == "金币")
            {
                GameDataMgr.Instance.AddCoin(rewardInfo.reward);
                Debug.Log($"【签到奖励】每日奖励 +{rewardInfo.reward} 金币");
            }
            else if (rewardInfo.rewardType == "体力")
            {
                GameDataMgr.Instance.AddStamina(rewardInfo.reward);
                Debug.Log($"【签到奖励】每日奖励 +{rewardInfo.reward} 体力");
            }
        }
    }

    private void GiveBigReward(int round)
    {
        BigRewardInfo bigReward = GameDataMgr.Instance.bigRewardInfoList.Find(r => r.round == round);
        if (bigReward != null)
        {
            if (bigReward.rewardType == "金币")
            {
                GameDataMgr.Instance.AddCoin(bigReward.reward);
                Debug.Log($"【签到奖励】大奖励#{round} +{bigReward.reward} 金币");
            }
            else if (bigReward.rewardType == "体力")
            {
                GameDataMgr.Instance.AddStamina(bigReward.reward);
                Debug.Log($"【签到奖励】大奖励#{round} +{bigReward.reward} 体力");
            }
        }
    }

    private void UpdateUI()
    {
        bool canSignToday = CanSignToday();

        int displayDays;
        int currentRound;

        if (_consecutiveDays == 0)
        {
            displayDays = 0;
            currentRound = 1;
        }
        else
        {
            displayDays = _consecutiveDays % MaxConsecutiveDays;
            if (displayDays == 0)
                displayDays = MaxConsecutiveDays;

            currentRound = (_consecutiveDays - 1) / MaxConsecutiveDays + 1;
        }

        if (_ConsecutiveDaysText != null)
        {
            _ConsecutiveDaysText.text = $"{displayDays}/{MaxConsecutiveDays} 天";
        }

        for (int i = 0; i < _SignInToggleTips.Length; i++)
        {
            bool signed = i < displayDays;
            _SignInToggleTips[i].isOn = signed;

            if (signed)
            {
                _SignInToggleTips[i].interactable = false;
            }
            else
            {
                _SignInToggleTips[i].interactable =
                    canSignToday &&
                    i == displayDays;
            }
        }
    }

    private void UpdateBigRewardUI()
    {
        for (int i = 0; i < _BigRewardButtons.Length && i < MaxBigRewardRound; i++)
        {
            int round = i + 1;
            Button btn = _BigRewardButtons[i];
            TextMeshProUGUI btnText = _BigRewardButtonTexts.Length > i ? _BigRewardButtonTexts[i] : null;
            TextMeshProUGUI statusText = _BigRewardStatusTexts.Length > i ? _BigRewardStatusTexts[i] : null;

            BigRewardInfo rewardInfo = GameDataMgr.Instance.bigRewardInfoList.Find(r => r.round == round);

            if (btn != null)
            {
                bool isClaimed = _bigRewardClaimed[round];
                bool isUnlocked = _consecutiveDays >= round * MaxConsecutiveDays;

                btn.interactable = isUnlocked && !isClaimed;

                if (btnText != null)
                {
                    if (rewardInfo != null)
                    {
                        btnText.text = $"大奖励#{round}\n{rewardInfo.reward}金币";
                    }
                    else
                    {
                        btnText.text = $"大奖励#{round}";
                    }
                }

                if (statusText != null)
                {
                    if (isClaimed)
                    {
                        statusText.text = "已领取";
                        statusText.color = Color.gray;
                    }
                    else if (isUnlocked)
                    {
                        statusText.text = "可领取";
                        statusText.color = Color.green;
                    }
                    else
                    {
                        int targetDays = round * MaxConsecutiveDays;
                        int remaining = targetDays - _consecutiveDays;
                        statusText.text = $"需{remaining}天";
                        statusText.color = Color.gray;
                    }
                }
            }
        }
    }

    private bool CanSignToday()
    {
        if (_lastDay == DateTime.MinValue)
            return true;

        return DateTime.Now.Date > _lastDay.Date;
    }
}