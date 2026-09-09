using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class ReplaceShaders : EditorWindow
{
    private string _searchPath = "Assets/";
    private Shader _oldShader;
    private Shader _newShader;

    [MenuItem("Tools/美术工具/批量修改工具/替换shader")]
    public static void ShowWindow()
    {
        GetWindow<ReplaceShaders>("Replace Shaders");
    }

    private void OnGUI()
    {
        GUILayout.Label("Replace Shaders", EditorStyles.boldLabel);

        GUILayout.Space(10);

        _searchPath = EditorGUILayout.TextField("Search Path", _searchPath);

        GUILayout.Space(10);

        _oldShader = EditorGUILayout.ObjectField("Old Shader", _oldShader, typeof(Shader), false) as Shader;
        _newShader = EditorGUILayout.ObjectField("New Shader", _newShader, typeof(Shader), false) as Shader;

        GUILayout.Space(10);

        if (GUILayout.Button("Replace"))
        {
            ReplaceShadersInPath(_searchPath, _oldShader, _newShader);
        }
    }

    private void ReplaceShadersInPath(string searchPath, Shader oldShader, Shader newShader)
    {
        if (oldShader == null)
        {
            Debug.LogError("Old shader is not set");
            return;
        }

        if (newShader == null)
        {
            Debug.LogError("New shader is not set");
            return;
        }

        // 获取指定路径下的所有材质球文件路径
        var guids = AssetDatabase.FindAssets("t:Material", new[] { searchPath });

        var replacedMaterialCount = 0;

        foreach (var guid in guids)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);

            var material = AssetDatabase.LoadAssetAtPath<Material>(path);

            if (material == null)
            {
                continue;
            }

            if (material.shader == oldShader)
            {
                material.shader = newShader;

                replacedMaterialCount++;
            }
        }

        Debug.Log($"Replaced {replacedMaterialCount} materials with shader '{newShader.name}'");
    }
}