using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// 创建一个名为 MeshChecker 的类，继承自 EditorWindow，用于检查场景中重复的网格
public class MeshChecker : EditorWindow
{
    // 定义按钮名称
    const string sCheckMesh = "检查重复网格";

    // 保存检查到的重复网格对象的列表
    static List<GameObject> mMeshList = new List<GameObject>();

    // 通过MenuItem按钮创建这个对话框
    [MenuItem("Tools/美术工具/重复网格检查工具")]
    public static void ConfigDialog()
    {
        // 创建并显示一个 EditorWindow 窗口
        EditorWindow.GetWindow(typeof(MeshChecker));
    }

    // 当窗口被启用时清空列表
    private void OnEnable()
    {
        mMeshList.Clear();
    }

    // 定义窗口中的 GUI
    private void OnGUI()
    {
        // 如果点击按钮，则执行检查重复网格的方法
        if (GUILayout.Button(sCheckMesh))
        {
            CheckMesh();
        }

        // 显示所有检测到的重复网格对象
        int count = mMeshList.Count;
        for (int i = 0; i < count; ++i)
        {
            EditorGUILayout.ObjectField(mMeshList[i], typeof(GameObject), true);
        }
    }

    // 定义最大距离误差，超过这个值则认为两个物体位置不同
    const float sMaxDist = 0.01f;

    // 检查场景中的网格是否重复
    void CheckMesh()
    {
        // 清空保存结果的列表
        mMeshList.Clear();

        // 获取场景中所有的 Transform 组件
        var transArr1 = GameObject.FindObjectsOfType<Transform>();
        if (transArr1 == null || transArr1.Length == 0)
        {
            return;
        }

        // 用于保存可能重复的 Transform 列表
        List<Transform> checkList2 = new List<Transform>();
        int count = transArr1.Length;

        // 遍历所有 Transform，检查是否有重复的
        for (int i = 0; i < count; ++i)
        {
            var checkTarget = transArr1[i];
            for (int j = 0; j < count; ++j)
            {
                var temp = transArr1[j];
                if (CheckTransform(checkTarget, temp))
                {
                    checkList2.Add(checkTarget);
                    break;
                }
            }
        }

        // 保存根节点 Transform 的列表
        List<Transform> rootList3 = new List<Transform>();
        count = checkList2.Count;
        for (int i = 0; i < count; ++i)
        {
            if (checkList2[i].parent == null)
            {
                rootList3.Add(checkList2[i]);
            }
        }

        // 检查根节点是否有重复的网格
        for (int i = 0; i < rootList3.Count; ++i)
        {
            var target = rootList3[i];
            bool isSame = false;
            for (int j = 0; j < checkList2.Count; ++j)
            {
                if (CheckShareMesh(target, checkList2[j]))
                {
                    mMeshList.Add(target.gameObject);
                    isSame = true;
                    break;
                }
            }
            // 如果根节点没有重复的网格，则将其子物体添加到根节点列表中继续检查
            if (!isSame)
            {
                for (int j = 0; j < target.childCount; ++j)
                {
                    rootList3.Add(target.GetChild(j));
                }
            }
        }
    }

    // 检查两个 Transform 是否共享相同的网格
    bool CheckShareMesh(Transform left, Transform right)
    {
        if (!CheckTransform(left, right))
        {
            return false;
        }

        // 获取两个 Transform 的 MeshFilter 组件数组
        var leftFilterArr = left.GetComponentsInChildren<MeshFilter>();
        var rightFilterArr = right.GetComponentsInChildren<MeshFilter>();

        if (leftFilterArr == null || rightFilterArr == null)
        {
            return false;
        }
        if (leftFilterArr.Length != rightFilterArr.Length || leftFilterArr.Length == 0)
        {
            return false;
        }

        // 转换为列表并比较两个列表中的网格
        var leftList = new List<MeshFilter>(leftFilterArr);
        var rightList = new List<MeshFilter>(rightFilterArr);
        var leftCount = leftList.Count;
        int sameCount = 0;

        for (int i = 0; i < leftCount; ++i)
        {
            var leftMesh = leftList[i] ? leftList[i].sharedMesh : null;
            var rightCount = rightList.Count;
            for (int j = 0; j < rightCount; ++j)
            {
                var rightMesh = rightList[j] ? rightList[j].sharedMesh : null;

                if ((leftMesh == rightMesh) && CheckTransform(leftList[i].transform, rightList[j].transform))
                {
                    ++sameCount;
                    rightList.RemoveAt(j);
                    break;
                }
            }
        }
        return (sameCount == leftCount);
    }

    // 检查两个 Transform 是否相同（位置、旋转、缩放）
    bool CheckTransform(Transform left, Transform right)
    {
        if (left == right)
        {
            return false;
        }
        if (Vector3.Distance(left.position, right.position) > sMaxDist)
        {
            return false;
        }
        if (left.rotation != right.rotation)
        {
            return false;
        }
        if (left.lossyScale != right.lossyScale)
        {
            return false;
        }
        return true;
    }
}
