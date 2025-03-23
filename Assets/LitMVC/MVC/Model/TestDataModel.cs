using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LitJson;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class TestDataModel {
    public List<TestData> dataModel;

    public bool Loaded { get; private set; } = false;

    public async Task LoadData() {
        try {
            var jsonContent = await Addressables.LoadAssetAsync<TextAsset>("Assets/DataModel/TestData.json").Task;
            if (jsonContent != null) {
                Debug.Log("JSON Loaded: " + jsonContent);
                // 解析JSON
                dataModel = JsonMapper.ToObject<List<TestData>>(jsonContent.text);
                Loaded = true;
            } else {
                Debug.LogError("Failed to load JSON: Asset is null.");
            }
        }
        catch (Exception e) {
            Debug.LogError($"Failed to load JSON: Asset is {e.Message}.");
        }
        
        // Addressables.LoadAssetAsync<TextAsset>("Assets/DataModel/TestData.json").Completed += OnLoadData;
    }

    // private void OnLoadData(AsyncOperationHandle<TextAsset> handle) {
    //     if (handle.Status == AsyncOperationStatus.Succeeded) {
    //         string jsonContent = handle.Result.text;
    //         Debug.Log("JSON Loaded: " + jsonContent);
    //
    //         // 解析JSON
    //         dataModel = JsonMapper.ToObject<List<TestData>>(jsonContent);
    //         Loaded = true;
    //     }
    //     else {
    //         Debug.LogError("Failed to load JSON.");
    //     }
    // }
    
}
