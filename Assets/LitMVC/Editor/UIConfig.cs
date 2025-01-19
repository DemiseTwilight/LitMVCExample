using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LitMVC;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Config",menuName = "MVC/Config")]
public class UIConfig : ScriptableObject
{
    [SerializeField]private string _viewPath;
    [SerializeField]private string _logicPath;
    [SerializeField]private string _servicePath;
    [SerializeField]private string _modelPath;
    
    [SerializeField] public List<ViewData> UiMap=new List<ViewData>();
    public string GetViewPath() {
        if (string.IsNullOrWhiteSpace(_viewPath)) {
            var path = GetPath();
        }
        return _viewPath;
    }
    public string GetLogicPath() {
        if (string.IsNullOrWhiteSpace(_logicPath)) {
            var path = GetPath();
        }
        return _logicPath;
    }
    public string GetServicePath() {
        if (string.IsNullOrWhiteSpace(_servicePath)) {
            var path = GetPath();
        }
        return _servicePath;
    }
    public string GetModelPathPath() {
        if (string.IsNullOrWhiteSpace(_modelPath)) {
            var path = GetPath();
        }
        return _modelPath;
    }
    public string GetPath() {
        string thisName = "LitMVC";
        var guids = AssetDatabase.FindAssets(thisName);
        foreach (var guid in guids) {
            var fullPath = AssetDatabase.GUIDToAssetPath(guid);
            if (Directory.Exists(fullPath)) {
                _viewPath = Path.Combine(fullPath, "MVC", "View");
                _logicPath = Path.Combine(fullPath, "MVC", "Logic");
                _servicePath = Path.Combine(fullPath, "MVC", "Service");
                _modelPath = Path.Combine(fullPath, "MVC", "Model");
                
                EditorUtility.SetDirty(this);
                AssetDatabase.SaveAssets();
                Debug.Log("更新MVC路径："+ fullPath);
                return fullPath;
            } 
        }

        return "";
    }

    public static UIConfig GetUIConfig() {
        string pathName = "LitMVC";
        var guids = AssetDatabase.FindAssets(pathName);
        foreach (var guid in guids) {
            var fullPath = AssetDatabase.GUIDToAssetPath(guid);
            var configPath = Path.Combine(fullPath, "Editor","config.asset");
            var mvcConfig = AssetDatabase.LoadAssetAtPath<UIConfig>(configPath);
            // mvcConfig.SynchronousDictionary();
            return mvcConfig;
        }
        return null;
    }

    public bool TryGetData(string scriptName,out ViewData data) {
        data=UiMap.FirstOrDefault(data => data.scriptName == scriptName);
        return data.scriptName != default;
    }
    public bool TryGetData(GameObject prefab,out ViewData data) {
        data=UiMap.FirstOrDefault(data => data.uiPrefab == prefab);
        return data.scriptName != default;
    }

    public void AddOrUpdate(ViewData data) {
        for (int i = 0; i < UiMap.Count; i++) {
            if (UiMap[i].scriptName == data.scriptName) {
                UiMap[i] = data;
                return;
            }
        }
        UiMap.Add(data);
    }
    public void Remove(string scriptName) {
        for (int i = 0; i < UiMap.Count; i++) {
            if (UiMap[i].scriptName == scriptName) {
                UiMap[i] = UiMap[^1];
                UiMap.RemoveAt(UiMap.Count-1);
                return;
            }
        }
    }
    /// <summary>
    /// 清理失效的绑定数据
    /// </summary>
    public void RemoveLapseData() {
        for (int i = 0; i < UiMap.Count; i++) {
            if (!UiMap[i].uiPrefab) {
                UiMap[i] = UiMap[^1];
                UiMap.RemoveAt(UiMap.Count-1);
            }
        }
    }
}
[CustomEditor(typeof(UIConfig))]
public class ConfigEditor : Editor {
    public override void OnInspectorGUI() {
        serializedObject.Update();
        if (GUILayout.Button("自动填充")) {
            var config = serializedObject.targetObject as UIConfig;
            var result = config.GetPath();
            Debug.Log("已设置路径:"+result);
        }

        serializedObject.ApplyModifiedProperties();
        base.OnInspectorGUI();
    }
}

[Serializable]
public struct ViewData {
    public string scriptName;
    public GameObject uiPrefab;
    public List<ComponentMap> components;

    public override bool Equals(object obj) {
        return obj is ViewData other && uiPrefab.Equals(other.uiPrefab);
    }

    public override int GetHashCode() {
        return uiPrefab.GetHashCode();
    }
}
[Serializable]
public struct ComponentMap {
    public string handleName;
    public Component component;
}