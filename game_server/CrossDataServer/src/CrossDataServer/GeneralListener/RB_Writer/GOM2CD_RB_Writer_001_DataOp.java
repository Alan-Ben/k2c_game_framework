package CrossDataServer.GeneralListener.RB_Writer;

import ALBasicProtocolPack._IALProtocolStructure;
import GOM2CD_RB.gom_p001_DataOp.*;

public class GOM2CD_RB_Writer_001_DataOp
{
    public static GOM2CD_RB_001_001_RetInitData make_001_RetInitData()
    {
        GOM2CD_RB_001_001_RetInitData protocol = new GOM2CD_RB_001_001_RetInitData();

        return protocol;
    }

    public static GOM2CD_RB_001_002_RetSyncData make_002_RetSyncData()
    {
        GOM2CD_RB_001_002_RetSyncData protocol = new GOM2CD_RB_001_002_RetSyncData();

        return protocol;
    }

    public static GOM2CD_RB_001_003_RetRmvData make_003_RetRmvData()
    {
        GOM2CD_RB_001_003_RetRmvData protocol = new GOM2CD_RB_001_003_RetRmvData();

        return protocol;
    }

    public static GOM2CD_RB_001_005_RetClearUSFromGroup make_005_RetClearUSFromGroup()
    {
        GOM2CD_RB_001_005_RetClearUSFromGroup protocol = new GOM2CD_RB_001_005_RetClearUSFromGroup();

        return protocol;
    }

    public static GOM2CD_RB_001_010_RetCustomDataOp make_010_RetCustomDataOp(_IALProtocolStructure _protocol)
    {
        GOM2CD_RB_001_010_RetCustomDataOp protocol = new GOM2CD_RB_001_010_RetCustomDataOp();

        //此处不写入头部
        protocol.setOpData(_protocol.makePackage());

        return protocol;
    }
}
