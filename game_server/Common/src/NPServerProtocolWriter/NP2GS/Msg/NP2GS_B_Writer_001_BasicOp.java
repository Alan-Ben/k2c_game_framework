package NPServerProtocolWriter.NP2GS.Msg;

import Common.NpServerObj.NpServerObj_SYS_ServerIndexInfo;
import NP2GS_B.p001_BasicOp.NP2GS_B_001_005_USVersionChg;
import NP2GS_B.p001_BasicOp.NP2GS_B_001_006_USIndexListInit;
import NP2GS_B.p001_BasicOp.NP2GS_B_001_007_USIndexListChg;

import java.util.Collection;
import java.util.List;

public class NP2GS_B_Writer_001_BasicOp
{
    public static NP2GS_B_001_005_USVersionChg make_005_USVersionChg(int _usId, String _serverVersion, String _resVersion)
    {
        NP2GS_B_001_005_USVersionChg protocol = new NP2GS_B_001_005_USVersionChg();
        protocol.setUsId(_usId);
        protocol.setServerVersion(_serverVersion);
        protocol.setResVersion(_resVersion);
        return protocol;
    }

    public static NP2GS_B_001_006_USIndexListInit make_006_USIndexListInit(Collection<? extends NpServerObj_SYS_ServerIndexInfo> _itemList)
    {
        NP2GS_B_001_006_USIndexListInit protocol = new NP2GS_B_001_006_USIndexListInit();
        protocol.getServerIndexList().addAll(_itemList);
        return protocol;
    }

    public static NP2GS_B_001_007_USIndexListChg make_007_USIndexListChg(List<NpServerObj_SYS_ServerIndexInfo> _itemList)
    {
        NP2GS_B_001_007_USIndexListChg protocol = new NP2GS_B_001_007_USIndexListChg();
        protocol.getServerIndexList().addAll(_itemList);
        return protocol;
    }
}
