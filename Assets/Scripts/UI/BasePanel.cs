using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class BasePanel : MonoBehaviour
{
    //控制面板透明度组件（CanvasGroup 可控制透明、交互、点击穿透）
    private CanvasGroup canvasGroup;
    //淡入淡出速度
    private float alphaSpeed=20;
    //标记当前隐藏还是显示
    public bool isShow=false;
    //隐藏完成之后要执行的方法
    private UnityAction hideCallBack = null;
    protected virtual void Awake()
    {
        canvasGroup = this.GetComponent<CanvasGroup>();//获取组件
        //如果没有组件，直接创建组件
        if(canvasGroup == null )
            canvasGroup= this.gameObject.AddComponent<CanvasGroup>();

    }
    protected virtual void Start()
    {
        Init();
    }
    /// <summary>
    /// 注册控件事件的方法 所有的子面板 都需要去注册一些控件事件
    /// 所以写成抽象方法 让子类必须去实现
    public abstract void Init();
    /// <summary>
    /// 显示界面需要的逻辑
    /// </summary>
    public virtual void ShowMe()
    {
        // 确保游戏对象是激活的
        if (!gameObject.activeSelf)
            gameObject.SetActive(true);
        canvasGroup.alpha = 1;
        isShow = true;
    }
    /// <summary>
    /// 隐藏界面需要的逻辑
    /// </summary>
    /// 当 alpha 降为 0 后，再执行 hideCallBack。
    /// <param name="callBack"></param>
    public virtual void HideMe(UnityAction callBack)
    {
        canvasGroup.alpha = 1;
        isShow = false;
        hideCallBack = callBack;
    }
    protected virtual void Update()
    {
        //当处于显示状态时。如果透明度不为1，需要加到1。后停止变化
        //淡入
        if( isShow && canvasGroup.alpha != 1)
        {
            canvasGroup.alpha += alphaSpeed * Time.deltaTime;
            if(canvasGroup.alpha >= 1)
                canvasGroup.alpha = 1;

        }
        //淡出
        else if(!isShow&& canvasGroup.alpha != 0)
        {
            canvasGroup.alpha -= alphaSpeed * Time.deltaTime;
            if (canvasGroup.alpha <= 0)
            {
                canvasGroup.alpha = 0;
                hideCallBack?.Invoke();//完全淡出后执行回调
            }
        }
    }
}
