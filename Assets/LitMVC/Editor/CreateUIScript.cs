using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Reflection;
using LitMVC;
using TMPro;
using Unity.EditorCoroutines.Editor;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UI;

public class CreateUIScript : Editor {
    private static UIConfig _mvcUIConfig;
    private static Dictionary<string, string> _subViewDic = new Dictionary<string, string>();

    private struct ComponentData {
        public string name;
        public string type;
        public Component component;
    }
    private static Dictionary<string,ComponentData> _componentDatas=new Dictionary<string,ComponentData>();
    
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
        _componentDatas.Clear();
        TotalComponentList(uiPrefab);
        var scriptName = uiPrefab.name + "View";
        StringBuilder scriptText = new StringBuilder();
        scriptText.AppendLine("using UnityEngine;");
        scriptText.AppendLine("using UnityEngine.UI;");
        scriptText.AppendLine("using TMPro;");
        scriptText.AppendLine("using LitMVC;");
        scriptText.AppendLine("namespace LitMVC {");
        scriptText.Append("\tpublic partial class ").Append(scriptName).Append(" : UIView {").AppendLine();
        foreach (var component in _componentDatas.Values) {
            scriptText.AppendFormat("\t\tpublic {0} {1};", component.type, component.name).AppendLine();
        }
        scriptText.AppendLine("\t}").AppendLine("}");

        var viewFilePath = uiScriptsPath.Append(_mvcUIConfig.GetViewPath()).Append("/")
            .Append(uiPrefab.name).Append("View.cs").ToString();
        File.WriteAllText(viewFilePath, scriptText.ToString());
        
        //Logic只生成，不刷新
        uiScriptsPath.Clear();
        var controllerFilePath = uiScriptsPath.Append(_mvcUIConfig.GetLogicPath()).Append("/")
            .Append(uiPrefab.name).Append("View.cs").ToString();
        scriptText.Clear();
        if (!File.Exists(controllerFilePath)) {
            scriptText.AppendLine("namespace LitMVC {");
            scriptText.Append("\tpublic partial class ").Append(scriptName).Append(" : UIView {").AppendLine()
                .AppendLine("\t}").AppendLine("}");

            File.WriteAllText(controllerFilePath, scriptText.ToString());
        }

        //记录绑定数据
        var uiBindData = new ViewData() {
            scriptName = "LitMVC." + scriptName, uiPrefab = uiPrefab, 
            components = _componentDatas.Values.Select(data=> new ComponentMap() { 
                handleName = data.name,
                component = data.component }
            ).ToList()
        };
        if (!_mvcUIConfig.UiMap.Contains(uiBindData)) {
            _mvcUIConfig.UiMap.Add(uiBindData);
            EditorUtility.SetDirty(_mvcUIConfig);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

    }

    [DidReloadScripts(1)]
    static void BindUI() {
        if (!_mvcUIConfig) _mvcUIConfig = UIConfig.GetUIConfig();
        var assembly = Assembly.Load("Assembly-CSharp");
        List<ViewData> removeList = new List<ViewData>();
        var bindData=_mvcUIConfig.UiMap;
        for (int i = 0; i < bindData.Count; i++) {
            var viewType = assembly.GetType(bindData[i].scriptName);
            //剔除已经失效的绑定数据：
            if (viewType == null || !bindData[i].uiPrefab || bindData[i].uiPrefab.TryGetComponent(viewType, out _)) {
                removeList.Add(bindData[i]);
                continue;
            } else {
                var script = bindData[i].uiPrefab.AddComponent(viewType);
                //绑定组件
                foreach (var componentMap in bindData[i].components) {
                    var fieldInfo = viewType.GetField(componentMap.handleName);
                    fieldInfo.SetValue(script,componentMap.component);
                }
                Debug.Log($"脚本{bindData[i].scriptName}绑定成功");
            }
        }
        
        //清理失效的绑定数据：
        foreach (var removeData in removeList) {
            _mvcUIConfig.UiMap.Remove(removeData);
        }
        EditorUtility.SetDirty(_mvcUIConfig);
        AssetDatabase.SaveAssets();
    }
    private static void TotalComponentList(GameObject ui) {
        foreach (Transform child in ui.GetComponentsInChildren<Transform>(true)) {
            //筛选拥有下划线命名的组件
            if (!child.name.Contains('_')) {
                continue;
            }

            List<Type> recognizable = new List<Type>() {
                typeof(Button), typeof(Dropdown),typeof(Text),typeof(InputField),typeof(Toggle),
                typeof(Slider),typeof(Selectable), typeof(Scrollbar),typeof(ScrollRect),
                typeof(TMP_InputField),typeof(TMP_Dropdown),typeof(TMP_Text),
                typeof(Image),
                typeof(RectTransform),
            };
            foreach (var type in recognizable) {
                if (child.TryGetComponent(type, out var component)) {
                    AddComponent(child,type.Name,component);
                    break;
                }
            }
        }
    }
    private static void AddComponent(Transform child,string type,Component component) {
        var data = new ComponentData() { name = "m_"+child.name, type = type,component = component};
        //不能添加说明有重复对象：
        int i = 0;
        var originalName=data.name;
        while (!_componentDatas.TryAdd(data.name, data)) {
            i++;
            data.name = originalName;
            data.name += i.ToString();
            Debug.LogWarning("有重复命名的对象，已更名为"+data.name);
        }
    }
}
