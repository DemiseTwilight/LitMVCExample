using UnityEngine;

namespace LitMVC.MVC.Model {
    public interface IDataModelImport {
        public void ImportData(TextAsset asset);
    }
}