using UnityEditor;
using UnityEngine;

public class PrefabReplacer : EditorWindow
{
    private GameObject replacementPrefab;
    private string targetName;
    private Vector3 scaleMultiplier = Vector3.one;

    [MenuItem("Tools/美术工具/批量修改工具/等比例预制替换工具")]
    public static void ShowWindow()
    {
        GetWindow<PrefabReplacer>("等比例预制替换工具");
    }

    private void OnGUI()
    {
        GUILayout.Label("等比例预制替换工具", EditorStyles.boldLabel);
        GUILayout.Space(10);

        replacementPrefab = EditorGUILayout.ObjectField("Replacement Prefab", replacementPrefab, typeof(GameObject), true) as GameObject;
        targetName = EditorGUILayout.TextField("Target Name", targetName);
        scaleMultiplier = EditorGUILayout.Vector3Field("Scale Multiplier", scaleMultiplier);

        GUILayout.Space(10);

        if (GUILayout.Button("Replace Prefabs"))
        {
            ReplacePrefabs();
        }
    }

    private void ReplacePrefabs()
    {
        if (replacementPrefab == null)
        {
            Debug.LogError("Replacement prefab is not assigned.");
            return;
        }

        GameObject[] sceneObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in sceneObjects)
        {
            if (obj.name == targetName)
            {
                GameObject newObject = PrefabUtility.InstantiatePrefab(replacementPrefab) as GameObject;
                newObject.transform.position = obj.transform.position;
                newObject.transform.rotation = obj.transform.rotation;
                newObject.transform.localScale = Vector3.Scale(obj.transform.localScale, scaleMultiplier);

                newObject.transform.SetParent(obj.transform.parent);

                Undo.RegisterCreatedObjectUndo(newObject, "Replace Prefab");

                ReplaceObjectReferences(obj, newObject);

                DestroyImmediate(obj);
            }
        }

        Debug.Log("Prefab replacement completed.");
    }

    private void ReplaceObjectReferences(GameObject oldObject, GameObject newObject)
    {
        SerializedObject serializedObject = new SerializedObject(newObject);
        SerializedProperty property = serializedObject.GetIterator();

        while (property.NextVisible(true))
        {
            if (property.propertyType == SerializedPropertyType.ObjectReference)
            {
                if (property.objectReferenceValue == oldObject)
                {
                    property.objectReferenceValue = newObject;
                }
            }
        }

        serializedObject.ApplyModifiedProperties();
    }
}