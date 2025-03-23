using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class TestService
{
    private TestDataModel _dataModel = new TestDataModel();
    public List<TestData> testDatas = new List<TestData>();
    public bool DataLoaded { get; private set; } = false;
    public async Task Init() {
        await _dataModel.LoadData();
        testDatas = _dataModel.dataModel;
    }
    
     
}
