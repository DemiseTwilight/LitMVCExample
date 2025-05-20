using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LitMVC {
    public class TestPageService {
        private TestPageDataModel _dataModel = new TestPageDataModel();
        public List<TestPageData> data;
        public async Task Init() {
            if (!_dataModel.Loaded) {
                await _dataModel.Load();
            }

            data = _dataModel.Data;
        }
    }
}