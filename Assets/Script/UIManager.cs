using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace LitMVC {
    public class UIManager : MonoBehaviour {
        public static UIManager instance;

        private List<DataModel> _dataModels = new() { TestPageDataModel.Instance };
        private List<Task> _dataLoadTasks = new();
        private void Awake() {
            instance = this;
        }

        // Start is called before the first frame update
        void Start() {
            StartCoroutine(LoadData());
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void ShowUI(UIInfo uiInfo,Action callback=null) {
            
        }

        public void CloseUI(UIInfo uiInfo,Action callback=null) {
            
        }

        private void LoadUIAsync(UIInfo uiInfo,Action callback) {
            Addressables.InstantiateAsync(uiInfo.assetPath,transform.Find("Container"));
        }

        private IEnumerator LoadData() {
            foreach (var dataModel in _dataModels) {
                yield return dataModel.LoadData();
            }

            LoadUIAsync(UIMap.TestPage, null);
        }
        
        
    }
}
