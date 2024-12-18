using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace LitMVC {
    public class UIManager : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        private void LoadUIAsync(string assetName,Action callback) {
            Addressables.InstantiateAsync(assetName,transform);
        }
    }
}
