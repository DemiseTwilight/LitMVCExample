using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace LitMVC {
    public class UIManager : MonoBehaviour {
        public static UIManager instance;
        private void Awake() {
            instance = this;
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
