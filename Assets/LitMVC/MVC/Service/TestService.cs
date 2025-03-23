using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestService
{
    private TestDataModel _dataModel = new TestDataModel();
    public List<TestData> testDatas = new List<TestData>();
    public bool DataLoaded { get; private set; } = false;
    public void Init() {
        _dataModel.LoadData();
    }
    
}
