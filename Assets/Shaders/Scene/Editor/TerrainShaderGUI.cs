using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TerrainShaderGUI : ShaderGUI
{
    MaterialProperty[] _Properties;
    MaterialEditor _Editor;
    Material _Target;

    GUIStyle _FoldoutStyle;
    GUIStyle _LabelStyle;

    bool[] mLayerVisable = null;

    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        this._Properties = properties;
        this._Editor = materialEditor;
        this._Target = _Editor.target as Material;

        _FoldoutStyle = new GUIStyle(EditorStyles.foldout);
        _FoldoutStyle.fontStyle = FontStyle.Bold;

        _LabelStyle = new GUIStyle(EditorStyles.label);
        _LabelStyle.fontStyle = FontStyle.Bold;

        DrawMask();
        DrawLayer(0);
        DrawLayer(1);
        DrawLayer(2);
        DrawLayer(3);
    }

    void DrawMask()
    {
        GUILayout.BeginVertical("Mask");

        EditorGUI.indentLevel += 1;
        var maskTex = FindProperty("_MaskTex");
        _Editor.ShaderProperty(maskTex, "_MaskTex");
        EditorGUI.indentLevel -= 1;

        GUILayout.EndVertical();
    }

    void DrawLayer(int layer)
    {
        DrawLayerFoldout(layer);
        if (mLayerVisable[layer])
        {
            GUILayout.BeginVertical(LayerName("layer", layer));

            EditorGUI.indentLevel += 1;

            var layerName = LayerName("_MainTex", layer);
            _Editor.ShaderProperty(FindProperty(layerName), layerName);

            layerName = LayerName("_Color", layer);
            _Editor.ShaderProperty(FindProperty(layerName), layerName);

            layerName = LayerName("_MetallicGlossMap", layer);
            _Editor.ShaderProperty(FindProperty(layerName), layerName);

            layerName = LayerName("_Smoothness", layer);
            _Editor.ShaderProperty(FindProperty(layerName), layerName);

            layerName = LayerName("_BumpMap", layer);
            _Editor.ShaderProperty(FindProperty(layerName), layerName);

            layerName = LayerName("_BumpMapScale", layer);
            _Editor.ShaderProperty(FindProperty(layerName), layerName);

            EditorGUI.indentLevel -= 1;

            GUILayout.EndVertical();
        }
    }
    void DrawLayerFoldout(int layer)
    {
        if (mLayerVisable == null)
        {
            mLayerVisable = new bool[4];
            for (int i = 0; i < 4; ++i)
            {
                mLayerVisable[i] = true;
            }
        }
        mLayerVisable[layer] = EditorGUILayout.Foldout(mLayerVisable[layer], LayerName("layer ",layer), _FoldoutStyle);
    }

    MaterialProperty FindProperty(string name)
    {
        return FindProperty(name, _Properties);
    }

    string LayerName(string name, int layer)
    {
        switch (layer)
        {
            case 0:
                return name + "R";
            case 1:
                return name + "G";
            case 2:
                return name + "B";
            default:
                return name + "A";
        }
    }
}
