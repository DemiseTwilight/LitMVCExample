using System.Threading.Tasks;

namespace LitMVC {
    public abstract class DataModel {
        
        public bool Loaded { get; protected set; } = false;

        public abstract Task LoadData();
    }
}