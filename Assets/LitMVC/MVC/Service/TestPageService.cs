using System.Collections.Generic;
using System.Threading.Tasks;

namespace LitMVC {
    public class TestPageService {
        private TestPageDataModel _dataModel = new TestPageDataModel();
        public List<TestPageData> data;
        public async Task Init() {
            if (!_dataModel.Loaded) {
                await _dataModel.LoadData();
            }

            data = _dataModel.data;
        }
    }
}