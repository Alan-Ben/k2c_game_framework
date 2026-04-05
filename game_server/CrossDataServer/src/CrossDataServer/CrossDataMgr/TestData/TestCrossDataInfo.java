package CrossDataServer.CrossDataMgr.TestData;

import ALServerLog.ALServerLog;
import CrossDataServer.CrossDataMgr._ITCrossDataInfo;
import GOM2CD_R.gom_p001_DataOp.TestCD_Data;

public class TestCrossDataInfo implements  _ITCrossDataInfo<TestCD_Data>
{
    private TestCD_Data _m_dData;

    public TestCrossDataInfo(TestCD_Data _data)
    {
        _m_dData = _data;
    }

    public String getStr() {return _m_dData.getTestStr();}
    public int getValue() {return _m_dData.getValue();}


    public long getDataId() {
        return _m_dData.getId();
    }


    public void syncData(TestCD_Data _data) {
        //测试类不管理，输出日志
        ALServerLog.Sys("Sync id:" + getDataId() + " string:" + getStr() + " value:" + getValue());

        _m_dData.setTestStr(_data.getTestStr());
        _m_dData.setValue(_data.getValue());

        //测试类不管理，输出日志
        ALServerLog.Sys("Sync ed id:" + getDataId() + " string:" + getStr() + " value:" + getValue());
    }
}
