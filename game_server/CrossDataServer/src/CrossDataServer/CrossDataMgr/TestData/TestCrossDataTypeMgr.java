package CrossDataServer.CrossDataMgr.TestData;

import CrossDataServer.CrossDataMgr._ACrossDataTypeMgr;
import CrossDataServer.CrossDataMgr._ICrossDataMgr;

public class TestCrossDataTypeMgr extends _ACrossDataTypeMgr {
    public TestCrossDataTypeMgr(int _dataType) {
        super(_dataType);
    }

    @Override
    protected _ICrossDataMgr _createNewGroupDataMgr(long _groupId) {
        return new TestCrossDataMgr(_groupId);
    }
}
