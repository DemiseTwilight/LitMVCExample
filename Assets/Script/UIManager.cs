using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace LitMVC {
    public class UIManager : MonoBehaviour {
        public static UIManager instance;

        private void Awake() {
            instance = this;
        }

        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void ShowUI(UIInfo uiInfo,Action callback=null) {
            
        }

        public void CloseUI(UIInfo uiInfo,Action callback=null) {
            
        }

        private void LoadUIAsync(string assetName,Action callback) {
            Addressables.InstantiateAsync(assetName,transform);
        }
    }
}
