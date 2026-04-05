package NPUSServer.CrossDataBasicPack.TestObj;

import ALServerLog.ALServerLog;
import GOM2CD_R.gom_p001_DataOp.TestCD_Data;
import NPUSServer.CrossDataBasicPack._ATCrossDataMgr;
import NPUSServer.NPUserServer;

import java.util.ArrayList;

public class TestCrossDataMgr extends _ATCrossDataMgr<TestCD_Data, TestCrossDataInfo>
{
    private ArrayList<TestCrossDataInfo> _m_lDataList;

    public TestCrossDataMgr(NPUserServer _usServer, int _dataType) {
        super(_usServer, _dataType);

        _m_lDataList = new ArrayList<TestCrossDataInfo>();
    }

    @Override
    protected int _getInitSyncDataPerPageCount() {
        return 100;
    }

    @Override
    protected void _onAddData_InLock(TestCrossDataInfo _data) {
        if(null == _data)
            return ;

        _m_lDataList.add(_data);

        ALServerLog.Sys("add data id:" + _data.getDataId() + " string:" + _data.getStr() + " value:" + _data.getValue() + " remain size:" + _m_lDataList.size());
    }

    @Override
    protected void _onRmvData_InLock(TestCrossDataInfo _data) {
        if(null == _data)
            return ;

        _m_lDataList.removeIf(_dataObj -> _dataObj.getDataId() == _data.getDataId());

        ALServerLog.Sys("remove data id:" + _data.getDataId() + " string:" + _data.getStr() + " value:" + _data.getValue() + " remain size:" + _m_lDataList.size());
    }
}
