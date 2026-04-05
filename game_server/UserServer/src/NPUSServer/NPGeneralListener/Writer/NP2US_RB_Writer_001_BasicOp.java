package NPUSServer.NPGeneralListener.Writer;

import ALBasicProtocolPack._IALProtocolStructure;
import Common.ServerObj.ServerObj_PHPPlayer;
import NP2US_R.p001_BasicOp.NP2US_R_001_020_GuildMsg;
import NP2US_RB.p001_BasicOp.NP2US_RB_001_002_ChgGeneralRes;
import NP2US_RB.p001_BasicOp.NP2US_RB_001_003_ChgRefRes;
import NP2US_RB.p001_BasicOp.NP2US_RB_001_010_PHPGetPlayerInfo;

public class NP2US_RB_Writer_001_BasicOp
{
    public static NP2US_RB_001_002_ChgGeneralRes make_002_ChgGeneralRes(boolean _res)
    {
        NP2US_RB_001_002_ChgGeneralRes protocol = new NP2US_RB_001_002_ChgGeneralRes();
        protocol.setRes(_res);
        return protocol;
    }

    public static NP2US_RB_001_003_ChgRefRes make_003_ChgRefRes(boolean _res)
    {
        NP2US_RB_001_003_ChgRefRes protocol = new NP2US_RB_001_003_ChgRefRes();
        protocol.setRes(_res);
        return protocol;
    }

    public static NP2US_RB_001_010_PHPGetPlayerInfo make_010_PHPGetPlayerInfo(ServerObj_PHPPlayer _player)
    {
        NP2US_RB_001_010_PHPGetPlayerInfo protocol = new NP2US_RB_001_010_PHPGetPlayerInfo();
        protocol.setPlayer(_player);
        return protocol;
    }

    public static NP2US_R_001_020_GuildMsg make_020_GuildMsg(long _cid, long _guidId, _IALProtocolStructure _msg)
    {
        NP2US_R_001_020_GuildMsg protocol = new NP2US_R_001_020_GuildMsg();
        protocol.setCid(_cid);
        protocol.setGuildId(_guidId);
        protocol.setMsg(_msg.makeFullPackage());

        return protocol;
    }
    public static NP2US_R_001_020_GuildMsg make_020_GuildMsg(long _cid, long _guidId, _IALProtocolStructure _msg, _IALProtocolStructure _addInfo)
    {
        NP2US_R_001_020_GuildMsg protocol = new NP2US_R_001_020_GuildMsg();
        protocol.setCid(_cid);
        protocol.setGuildId(_guidId);
        protocol.setMsg(_msg.makeFullPackage());

        if(null != _addInfo)
            protocol.setAddInfo(_addInfo.makePackage());

        return protocol;
    }
}
