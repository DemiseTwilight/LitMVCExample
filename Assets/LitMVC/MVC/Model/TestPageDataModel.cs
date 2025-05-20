using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using LitJson;
using LitMVC.MVC.Model;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace LitMVC{
    public class TestPageDataModel : DataModel { 
        protected override string Path { get; } = "Assets/Data/TestPageData.json";
        public List<TestPageData> Data { get; private set; }
        private static TestPageDataModel _instance;
        public static TestPageDataModel Instance => _instance ??= new TestPageDataModel();
    
        protected override void DeserializeData(TextAsset asset) {
            if (asset != null) {
                Data = JsonMapper.ToObject<List<TestPageData>>(asset.text);
                Loaded = true;
            } else {
                Debug.LogError($"Failed to load JSON: Asset {Path} is null.");
                Loaded = false;
            }
        }
    }

public class TestPageData {
    public int id;
    public string text1;
    public string text2;
}

}