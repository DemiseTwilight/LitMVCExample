using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class TestDataModel {
    public List<TestData> dataModel;

    public bool Loaded { get; private set; } = false;

    public void LoadData() {
        Addressables.LoadAssetAsync<TextAsset>("Assets/DataModel/TestData.json").Completed += OnLoadData;
    }

    private void OnLoadData(AsyncOperationHandle<TextAsset> handle) {
        if (handle.Status == AsyncOperationStatus.Succeeded) {
            string jsonContent = handle.Result.text;
            Debug.Log("JSON Loaded: " + jsonContent);

            // 解析JSON
            
            dataModel = JsonUtility.FromJson<TestDataList>(jsonContent).items;
            Loaded = true;
        }
        else {
            Debug.LogError("Failed to load JSON.");
        }
    }
    
}
[SerializeField]
public class TestDataList {
    public List<TestData> items;
}
