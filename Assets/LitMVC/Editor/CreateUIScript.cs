using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Reflection;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UI;

public class CreateUIScript : Editor {
    private static UIConfig _mvcUIConfig;
    private static Dictionary<string, string> _subViewDic = new Dictionary<string, string>();

    private static List<Type> _recognizable = new() {
        typeof(Button), typeof(Dropdown), typeof(Text), typeof(InputField), typeof(Toggle),
        typeof(Slider), typeof(Scrollbar), typeof(ScrollRect),
        typeof(TMP_InputField), typeof(TMP_Dropdown), typeof(TMP_Text),
        typeof(Image),
    };

    private static List<Type> _recognizable2 = new() {
        typeof(RectTransform),
    };
    private struct ComponentData {
        public string name;
        public string varName;
        public string type;
        public Component component;
    }
    private static Dictionary<string,ComponentData> _componentData=new Dictionary<string,ComponentData>();
    private static  Assembly _assembly = Assembly.Load("Assembly-CSharp");
    
    [MenuItem("Assets/MVC/CreateUIView", false, 10)]
    private static void GenUIView() {
        try {
            AssetDatabase.StartAssetEditing();
            GameObject uiPrefab = Selection.activeGameObject as GameObject;
            if (!uiPrefab) return;
            if (!_mvcUIConfig) _mvcUIConfig = UIConfig.GetUIConfig();
            CreateUIBind(uiPrefab);
        }
        catch (Exception e) {
            Debug.LogError(e);
            throw;
        }
        finally {
            AssetDatabase.StopAssetEditing();
            AssetDatabase.Refresh();
        }
    }

    private static void CreateUIBind(GameObject uiPrefab) {
        StringBuilder uiScriptsPath = new StringBuilder();
        //View生成，刷新并绑定
        uiScriptsPath.Clear();
        _componentData.Clear();
        TotalComponentList(uiPrefab.transform);
        var scriptName = uiPrefab.name + "View";
        StringBuilder scriptText = new StringBuilder();
        scriptText.AppendLine("using UnityEngine;");
        scriptText.AppendLine("using UnityEngine.UI;");
        scriptText.AppendLine("using TMPro;");
        scriptText.AppendLine("namespace LitMVC {");
        scriptText.Append($"\tpublic partial class {scriptName} ");
        var baseName = "UIView";
        if (PrefabUtility.IsPartOfVariantPrefab(uiPrefab)) {
            var originalPrefab=PrefabUtility.GetCorrespondingObjectFromOriginalSource(uiPrefab);
            if (_mvcUIConfig.TryGetData(originalPrefab, out var originalViewData)) {
                baseName = originalViewData.scriptName;
            } 
        }
        scriptText.AppendLine($": {baseName} {{");
        scriptText.AppendLine($"\t\tpublic const string VIEW_NAME = \"{uiPrefab.name}\";");
        
        foreach (var component in _componentData.Values) {
            scriptText.AppendFormat("\t\tpublic {0} {1};", component.type, component.name).AppendLine();
        }
        scriptText.AppendLine("\t}").AppendLine("}");

        var viewFilePath = uiScriptsPath.Append(_mvcUIConfig.GetViewPath()).Append("/")
            .Append(uiPrefab.name).Append("View.cs").ToString();
        File.WriteAllText(viewFilePath, scriptText.ToString());
        
        //Logic只生成，不刷新
        uiScriptsPath.Clear();
        var controllerFilePath = uiScriptsPath.Append(_mvcUIConfig.GetLogicPath()).Append("/")
            .Append(uiPrefab.name).Append("Logic.cs").ToString();
        scriptText.Clear();
        if (!File.Exists(controllerFilePath)) {
            scriptText.AppendLine("namespace LitMVC {");
            scriptText.Append("\tpublic partial class ").Append(scriptName).Append($" : {baseName} {{").AppendLine()
                .AppendLine("\t}").AppendLine("}");

            File.WriteAllText(controllerFilePath, scriptText.ToString());
        }

        //记录绑定数据
        var uiBindData = new ViewData() {
            scriptName = "LitMVC." + scriptName, uiPrefab = uiPrefab, 
            components = _componentData.Values.Select(data=> new ComponentMap() { 
                handleName = data.name,
                component = data.component }
            ).ToList()
        };
        _mvcUIConfig.AddOrUpdate(uiBindData);

        EditorUtility.SetDirty(_mvcUIConfig);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

    }

    [DidReloadScripts(1)]
    private static void AutoBindUI() {
        if (!_mvcUIConfig) _mvcUIConfig = UIConfig.GetUIConfig();
        _mvcUIConfig.RemoveLapseData();
        var bindData=_mvcUIConfig.UiMap;
        for (int i = 0; i < bindData.Count; i++) {
            BindUI(bindData[i]);
        }
        EditorUtility.SetDirty(_mvcUIConfig);
        AssetDatabase.SaveAssets();
    }
    
    [MenuItem("Assets/MVC/ManualBindUI", false, 11)]
    private static void ManualBindUI() {
        try {
            AssetDatabase.StartAssetEditing();
            GameObject uiPrefab = Selection.activeGameObject as GameObject;
            if (!uiPrefab) return;
            if (!_mvcUIConfig) _mvcUIConfig = UIConfig.GetUIConfig();
            if (_mvcUIConfig.TryGetData(uiPrefab,out var viewData)) {
                BindUI(viewData);
            }
        }
        catch (Exception e) {
            Debug.LogError(e);
            throw;
        }
        finally {
            AssetDatabase.StopAssetEditing();
            AssetDatabase.Refresh();
        }
    }

    private static void BindUI(ViewData viewData) {
        var viewType = _assembly.GetType(viewData.scriptName);
        Component script = null;
        if (!viewData.uiPrefab.TryGetComponent(viewType, out script)) {
            script = viewData.uiPrefab.AddComponent(viewType);
            Debug.Log($"脚本{viewData.scriptName}绑定成功");
        }

        //绑定组件
        if (script != null) {
            foreach (var componentMap in viewData.components) {
                var fieldInfo = viewType.GetField(componentMap.handleName);
                // if (!fieldInfo.FieldType.IsAssignableFrom(componentMap.component.GetType())) {
                //     Debug.LogError($"字段 {componentMap.handleName} 的类型 {fieldInfo.FieldType} 与组件类型 {componentMap.component.GetType()} 不匹配");
                //     continue;
                // }
                fieldInfo.SetValue(script, componentMap.component);
            }
        }
    }

    private static void TotalComponentList(Transform ui,bool onlyAdded=false) {
        foreach (Transform child in ui.transform) {
            // 排除根节点
            if (child == ui) continue;
            //如果子对象是预制体或预制体变体,查找注册表。如果不存在的话按照通常规则进行
            if (PrefabUtility.IsAnyPrefabInstanceRoot(child.gameObject)) {
                var subPrefab = PrefabUtility.GetCorrespondingObjectFromSource(child.gameObject);
                if (_mvcUIConfig.TryGetData(subPrefab, out var viewData)) {
                    if (child.TryGetComponent(_assembly.GetType(viewData.scriptName) , out var component)) {
                        AddComponent($"m_{child.name}_SubView", viewData.scriptName, component);
                    }

                    TotalComponentList(child, true);
                    continue;
                }
            }
            if (child.name.Contains('_')) {
                //如果根节点是预制体变体，只需统计新增组件
                if ((onlyAdded || PrefabUtility.IsPartOfVariantPrefab(ui.gameObject)) 
                    && !PrefabUtility.IsAddedGameObjectOverride(child.gameObject)) {
                    continue;
                }
                //筛选拥有下划线命名的组件,如果存在更高匹配等级将不会匹配更低等级组件
                int recognizableLevel = 2;
                foreach (var type in _recognizable) {
                    if (child.TryGetComponent(type, out var component)) {
                        AddComponent($"m_{child.name}_{type.Name}", type.Name, component);
                        recognizableLevel = 1;
                    }
                }

                if (recognizableLevel == 2) {
                    recognizableLevel = 3;
                    foreach (var type in _recognizable2) {
                        if (child.TryGetComponent(type, out var component)) {
                            AddComponent($"m_{child.name}", type.Name, component);
                            recognizableLevel = 2;
                        }
                    }
                }
            }

            TotalComponentList(child);
        }
    }
    private static void AddComponent(string childName,string type,Component component) {
        var data = new ComponentData() { name = childName, type = type,component = component};
        //不能添加说明有重复对象：
        int i = 0;
        var originalName=data.name;
        while (!_componentData.TryAdd(data.name, data)) {
            i++;
            data.name = originalName;
            data.name += i.ToString();
            Debug.LogWarning("有重复命名的对象，已更名为"+data.name);
        }
    }
}
