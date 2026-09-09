using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class NameTool : Editor
{
    const string sInvalidTarget = "无效的目标";
    [MenuItem("GameObject/美术工具/批量改名/删除小括号、空格", false, 2)]
    static void NameTool1()
    {
        if (Selection.gameObjects == null || Selection.gameObjects.Length == 0)
        {
            Debug.LogError(sInvalidTarget);
            return;
        }
        foreach (var target in Selection.gameObjects)
        {
            NameTool1(target);
        }
    }
    static void NameTool1(GameObject target)
    {
        target.name = target.name.Replace(" ", "");
        var indexLeft = target.name.IndexOf("(");
        var indexRight = target.name.LastIndexOf(")");
        if (indexRight > indexLeft && indexLeft != 0 && indexRight != 0)
        {
            var newName = target.name.Substring(0, indexLeft) + target.name.Substring(indexRight, target.name.Length - indexRight - 1);
            target.name = newName;
        }
        var count = target.transform.childCount;
        for (int i = 0; i < count; ++i)
        {
            NameTool1(target.transform.GetChild(i).gameObject);
        }
    }

    [MenuItem("GameObject/美术工具/批量改名/列出所有名字包含空格的物体", false, 3)]
    static void NameTool2()
    {
        if (Selection.gameObjects == null || Selection.gameObjects.Length == 0)
        {
            Debug.LogError(sInvalidTarget);
            return;
        }
        foreach (var target in Selection.gameObjects)
        {
            NameTool2(target);
        }
    }
    static void NameTool2(GameObject target)
    {
        if (target.name.Contains(" "))
        {
            Debug.LogError(target.name);
        }
        var count = target.transform.childCount;
        for (int i = 0; i < count; ++i)
        {
            NameTool2(target.transform.GetChild(i).gameObject);
        }
    }
}
