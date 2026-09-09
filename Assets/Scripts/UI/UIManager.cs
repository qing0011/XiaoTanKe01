using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class UIManager
{
//    单例模式的一种写法
//instance 是一个静态对象，程序运行时自动创建
//外部通过 UIManager.Instance 使用 UIManage
//构造函数是私有的，所以外部无法 new UIManager
   private static UIManager instance=new UIManager();
    public static UIManager Instance=>instance;
    //字典保存当前显示中的“面板”
    //作用： 防止重复显示面板， 方便保存每个面板实例
    private Dictionary<string,BasePanel> panelDic = new Dictionary<string,BasePanel>();
    private Transform canvasTrans;
    //初始化 UIManager 的时候会自动创建 Canvas
    private UIManager()
    {
        //得到工程中的canvas预设
        GameObject canvas = GameObject.Instantiate(Resources.Load<GameObject>("UI/Canvas"));
        canvasTrans = canvas.transform;
        GameObject.DontDestroyOnLoad(canvas);//确保只有一个canvas
    }
    //显示面板
    public T ShowPanel<T>() where T: BasePanel
    {
        //以后只需要保证 命名T和 预制体名字 是 一个 唯一的一个规则 就可以非常方便的让外部使用它
        string panelName = typeof(T).Name;
        //判断 字典里 是否已经显示过该面板
        if (panelDic.ContainsKey(panelName))
            return panelDic[panelName] as T;
        //显示面板 就要动态的创建预制体 设置父对象
        GameObject panelObj = GameObject.Instantiate(Resources.Load<GameObject>("UI/" + panelName));
        //Debug.Log("prefab: " + panelName);
       // Debug.Log("canvas: " + canvasTrans);
        //设置父对象 放到场景中的 Canvas下面
        panelObj.transform.SetParent(canvasTrans, false);
        //指定面板 显示逻辑 应该把面板挂上去
        T panel = panelObj.GetComponent<T>();
        //Debug.Log("panel: " + panel);
        //把面板脚本 存入字典 便于之后 获取 或 移除
        panelDic.Add(panelName, panel);
        //调用自己的显示逻辑
        panel.ShowMe();
        return panel;
    }
    //隐藏面板
    public void HidePanel<T>(bool isFade = true) where T : BasePanel
    {
        //
        string panelName = typeof(T).Name;
        //判断当前是否显示过
        if (panelDic.ContainsKey(panelName))
        {
                if (isFade)
                {
                    //面板淡出完成后删除面板
                    panelDic[panelName].HideMe(() =>
                    {
                        //删除面板
                        GameObject.Destroy(panelDic[panelName].gameObject);
                        //删除字典中存储的面板脚本
                        panelDic.Remove(panelName);
                    });
                }
                else
                {
                    //删除面板
                    GameObject.Destroy(panelDic[panelName].gameObject);
                    //删除字典中存储的面板脚本
                    panelDic.Remove(panelName);
                }

        }

    }
    //得到面板 不会创建 只在字典。
    public T GetPanel<T>() where T : BasePanel
    {
        string panelName = typeof (T).Name;
        if(panelDic.ContainsKey(panelName))
            return panelDic[panelName] as T;
        //没有返回空
        return null;
    }
    //隐藏所有面板
    public void HideAllPanels()
    {
        List<string> panelNames = new List<string>(panelDic.Keys);
        foreach (string panelName in panelNames)
        {
            GameObject.Destroy(panelDic[panelName].gameObject);
        }
        panelDic.Clear();
    }
    //注册已存在的面板到UIManager（用于场景中直接放置的面板）
    public void RegisterPanel(BasePanel panel)
    {
        string panelName = panel.GetType().Name;
        if (!panelDic.ContainsKey(panelName))
        {
            panelDic.Add(panelName, panel);
        }
    }


}
