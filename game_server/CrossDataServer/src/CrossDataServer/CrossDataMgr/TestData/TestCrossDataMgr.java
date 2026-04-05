package CrossDataServer.CrossDataMgr.TestData;

import ALServerLog.ALServerLog;
import CrossDataServer.CrossDataMgr._ATCrossDataMgr;
import GOM2CD_R.gom_p001_DataOp.TestCD_Data;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;

import java.nio.ByteBuffer;

/**
 * 测试类，方便判断是否有函数未重载，做完之后删除
 */
public class TestCrossDataMgr extends _ATCrossDataMgr<TestCD_Data, TestCrossDataInfo>
{
    public TestCrossDataMgr(long _groupId) {
        super(_groupId);
    }

    @Override
    protected long _getDataId(TestCD_Data _dataProtocol) {
        if(null == _dataProtocol)
            return 0;

        return _dataProtocol.getId();
    }

    @Override
    protected TestCD_Data _readData(ByteBuffer _data) {
        if(null == _data)
            return null;

        TestCD_Data data = new TestCD_Data();
        data.readPackage(_data);

        return data;
    }

    @Override
    protected TestCrossDataInfo _createNewDataInfo(TestCD_Data _dataProtocol) {
        if(null == _dataProtocol)
            return null;

        return new TestCrossDataInfo(_dataProtocol);
    }

    @Override
    protected void _onAddData_InLock(TestCrossDataInfo _data) {
        //测试类不管理，输出日志
        ALServerLog.Sys("Add group:" + getGroupId() + " id:" + _data.getDataId() + " string:" + _data.getStr() + " value:" + _data.getValue());
    }

    @Override
    protected void _onRmvData_InLock(TestCrossDataInfo _data) {
        //测试类不管理，输出日志
        ALServerLog.Sys("Rmv group:" + getGroupId() + " id:" + _data.getDataId() + " string:" + _data.getStr() + " value:" + _data.getValue());
    }

    @Override
    public void _dealCustomOp(_IWCGBasicRequestCommiter _commiter, ByteBuffer _protocol) {

    }
}
