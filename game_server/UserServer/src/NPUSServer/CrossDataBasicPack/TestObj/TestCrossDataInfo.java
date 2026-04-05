package NPUSServer.CrossDataBasicPack.TestObj;

import GOM2CD_R.gom_p001_DataOp.TestCD_Data;
import NPUSServer.CrossDataBasicPack._ATCrossDataInfo;

public class TestCrossDataInfo extends _ATCrossDataInfo<TestCD_Data> {
    private TestCD_Data _m_dData;

    public TestCrossDataInfo(TestCD_Data _data)
    {
        _m_dData = _data;
    }

    public String getStr() {return _m_dData.getTestStr();}
    public int getValue() {return _m_dData.getValue();}

    public TestCD_Data getDataObj() {return _m_dData;}


    @Override
    public long getDataId() {
        return _m_dData.getId();
    }

    @Override
    public TestCD_Data makeCrossDataProtocolObj() {
        return _m_dData;
    }
}
