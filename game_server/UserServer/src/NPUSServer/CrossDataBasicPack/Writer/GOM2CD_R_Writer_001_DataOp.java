package NPUSServer.CrossDataBasicPack.Writer;

import ALBasicProtocolPack._IALProtocolStructure;
import GOM2CD_R.gom_p001_DataOp.*;

import java.util.ArrayList;

public class GOM2CD_R_Writer_001_DataOp
{
    public static GOM2CD_R_001_001_InitData make_001_InitData(int _dataType, long _groupId, int _usId)
    {
        GOM2CD_R_001_001_InitData protocol = new GOM2CD_R_001_001_InitData();

        protocol.setDataType(_dataType);
        protocol.setGroupId(_groupId);
        protocol.setUsId(_usId);

        return protocol;
    }

    public static GOM2CD_R_001_002_SyncData make_002_SyncData(int _dataType, long _groupId, int _usId, _IALProtocolStructure _data)
    {
        GOM2CD_R_001_002_SyncData protocol = new GOM2CD_R_001_002_SyncData();

        protocol.setDataType(_dataType);
        protocol.setGroupId(_groupId);
        protocol.setUsId(_usId);

        protocol.getDataList().add(_data.makePackage().array());

        return protocol;
    }
    public static GOM2CD_R_001_002_SyncData make_002_SyncData(int _dataType, long _groupId, int _usId, ArrayList<_IALProtocolStructure> _dataList)
    {
        GOM2CD_R_001_002_SyncData protocol = new GOM2CD_R_001_002_SyncData();

        protocol.setDataType(_dataType);
        protocol.setGroupId(_groupId);
        protocol.setUsId(_usId);

        for(_IALProtocolStructure data : _dataList)
        {
            if(null == data)
                continue;

            protocol.getDataList().add(data.makePackage().array());
        }

        return protocol;
    }

    public static GOM2CD_R_001_003_RmvData make_003_RmvData(int _dataType, long _groupId, int _usId, long _dataId)
    {
        GOM2CD_R_001_003_RmvData protocol = new GOM2CD_R_001_003_RmvData();

        protocol.setDataType(_dataType);
        protocol.setGroupId(_groupId);
        protocol.setUsId(_usId);

        protocol.getDataId().add(_dataId);

        return protocol;
    }
    public static GOM2CD_R_001_003_RmvData make_003_RmvData(int _dataType, long _groupId, int _usId, ArrayList<Long> _dataIdList)
    {
        GOM2CD_R_001_003_RmvData protocol = new GOM2CD_R_001_003_RmvData();

        protocol.setDataType(_dataType);
        protocol.setGroupId(_groupId);
        protocol.setUsId(_usId);

        protocol.getDataId().addAll(_dataIdList);

        return protocol;
    }

    public static GOM2CD_R_001_005_ClearUSFromGroup make_005_ClearUSFromGroup(int _dataType, long _groupId, int _usId)
    {
        GOM2CD_R_001_005_ClearUSFromGroup protocol = new GOM2CD_R_001_005_ClearUSFromGroup();

        protocol.setDataType(_dataType);
        protocol.setGroupId(_groupId);
        protocol.setUsId(_usId);

        return protocol;
    }

    public static GOM2CD_R_001_010_CustomDataOp make_010_CustomDataOp(int _dataType, long _groupId, _IALProtocolStructure _protocol)
    {
        GOM2CD_R_001_010_CustomDataOp protocol = new GOM2CD_R_001_010_CustomDataOp();

        protocol.setDataType(_dataType);
        protocol.setGroupId(_groupId);

        //此处需要写入头部，可以用dispather处理
        protocol.setOpData(_protocol.makeFullPackage());

        return protocol;
    }
}
