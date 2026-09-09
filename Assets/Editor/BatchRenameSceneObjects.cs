using UnityEditor;
using UnityEngine;

// 创建一个名为 BatchRenameSelectedObjects 的类，继承自 EditorWindow，用于批量重命名场景中的选定对象
public class BatchRenameSelectedObjects : EditorWindow
{
    // 定义用于搜索和替换的字符串变量
    private string searchString;
    private string replaceString;

    // 通过MenuItem按钮创建一个窗口，命名为“场景内批量重命名”
    [MenuItem("Tools/美术工具/批量修改工具/场景内批量重命名")]
    public static void ShowWindow()
    {
        // 显示一个名为“Batch Rename Selected Objects”的窗口
        GetWindow<BatchRenameSelectedObjects>("Batch Rename Selected Objects");
    }

    // 定义窗口中的 GUI
    private void OnGUI()
    {
        // 在窗口顶部显示标题“场景内批量重命名”
        GUILayout.Label("场景内批量重命名", EditorStyles.boldLabel);
        GUILayout.Space(10);

        // 显示文本字段供用户输入要搜索和替换的内容
        searchString = EditorGUILayout.TextField("被替换的内容: ", searchString);
        replaceString = EditorGUILayout.TextField("替换的内容: ", replaceString);

        GUILayout.Space(10);

        // 当用户点击“Batch Rename”按钮时，执行批量重命名操作
        if (GUILayout.Button("Batch Rename"))
        {
            BatchRename();
        }
    }

    // 执行批量重命名操作
    private void BatchRename()
    {
        // 获取当前选中的所有游戏对象
        GameObject[] selectedObjects = Selection.gameObjects;

        // 遍历每一个选中的对象
        foreach (GameObject selectedObject in selectedObjects)
        {
            // 记录撤销操作，方便后续撤销重命名
            Undo.RecordObject(selectedObject, "Rename Object");

            // 将对象的名称中的搜索内容替换为指定的替换内容
            selectedObject.name = selectedObject.name.Replace(searchString, replaceString);
        }
    }
}
