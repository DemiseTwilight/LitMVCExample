using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using LitJson;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace LitMVC{
    public class TestPageDataModel : DataModel {
    public List<TestPageData> data;
    private static TestPageDataModel _instance;
    public static TestPageDataModel Instance => _instance ??= new TestPageDataModel();

    public override async Task LoadData() {
        try {
            var asset = await Addressables.LoadAssetAsync<TextAsset>("Assets/Data/TestPageData.json").Task;
            if (asset!=null) {
                data = JsonMapper.ToObject<List<TestPageData>>(asset.text);
            } else {
                Debug.LogError("Failed to load JSON: Asset is null.");
                Loaded = false;
            }
        }
        catch (Exception e) {
            Debug.LogError($"Failed to load JSON: {e}");
            Loaded = false;
        }
        Loaded = true;
    }
}

public class TestPageData {
    public int id;
    public string text1;
    public string text2;
}

}