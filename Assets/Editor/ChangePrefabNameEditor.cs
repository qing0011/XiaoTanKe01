using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class ChangePrefabNameEditor : EditorWindow
{
    [MenuItem("Tools/美术工具/批量修改工具/修改地块名称为场景名称")]
    public static void OpenWindow()
    {
        GetWindow<ChangePrefabNameEditor>("修改地块名称为场景名称");
    }

    void OnGUI()
    {
        if (GUILayout.Button("Change Names"))
        {
            ChangePrefabNames();
        }
    }

    void ChangePrefabNames()
    {
        //获取当前所有打开的场景
        UnityEngine.SceneManagement.Scene[] scenes = EditorSceneManager.GetAllScenes();

        //遍历所有场景
        foreach (UnityEngine.SceneManagement.Scene scene in scenes)
        {
            //获取当前场景的名称
            string sceneName = scene.name;

            //标记场景为需要保存
            EditorSceneManager.MarkSceneDirty(scene);

            //遍历当前场景中的所有预制体
            foreach (GameObject obj in scene.GetRootGameObjects())
            {
                //检查预制体名称是否包含“heightfield”
                if (obj.name.Contains("heightfield"))
                {
                    //更改预制体的名称
                    obj.name = "heightfield_" + sceneName;
                    //标记此物体为已更改，以便Unity Editor能够保存更改
                    EditorUtility.SetDirty(obj);
                    //保存所有更改的场景
                    EditorSceneManager.SaveOpenScenes();
                    //获取预制体资源路径
                    string prefabPath = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(obj);
                    //更改预制体资源名称
                    AssetDatabase.RenameAsset(prefabPath, "heightfield_" + sceneName);
                    //标记此预制体资源为已更改
                    AssetDatabase.SaveAssets();

                }
            }
        }

      
    }
}