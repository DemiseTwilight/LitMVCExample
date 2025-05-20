using System;
using System.Threading.Tasks;
using LitMVC.MVC.Model;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace LitMVC {
    public abstract class DataModel :IDataModelImport{
        public bool Loaded = false;
        protected virtual string Path { get; } = default;

        public async Task Load() {
            try {
                Debug.Log(Path);
                var asset = await Addressables.LoadAssetAsync<TextAsset>(Path).Task;
                DeserializeData(asset);

                Addressables.Release(asset);
            }
            catch (Exception e) {
                Debug.LogError(e);
                Loaded = false;
                throw;
            }

        }

        public void ImportData(TextAsset asset) {
            DeserializeData(asset);
        }
        /// <summary>
        /// 反序列化数据
        /// 子类必须实现这个，将资源转化为数据
        /// </summary>
        /// <param name="asset"></param>
        protected abstract void DeserializeData(TextAsset asset);
        
    }
}
