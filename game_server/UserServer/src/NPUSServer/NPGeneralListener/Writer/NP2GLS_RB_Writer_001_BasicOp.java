package NPUSServer.NPGeneralListener.Writer;

import ALBasicProtocolPack._IALProtocolStructure;
import NP2GLS_R.p001_BasicOp.NP2GLS_R_001_007_QuitInstance;
import NP2GLS_R.p001_BasicOp.NP2GLS_R_001_008_RegUs;
import NP2GLS_R.p001_BasicOp.NP2GLS_R_001_009_SyncGroupTeamData;
import NP2GLS_R.p001_BasicOp.NP2GLS_R_001_010_RemoveGroupByTeam;
import NP2GLS_R.p001_BasicOp.NP2GLS_R_001_020_DealMsg;
import NPUSServer.CommonActivityMgr.Core._AActivityBase;

public class NP2GLS_RB_Writer_001_BasicOp
{
    public static NP2GLS_R_001_007_QuitInstance make_007_QuitInstance(_AActivityBase _activity)
    {
        NP2GLS_R_001_007_QuitInstance proto = new NP2GLS_R_001_007_QuitInstance();
        proto.setInstanceId(_activity.getGameLogicInstanceId());
        proto.setUsId(_activity.getUSServer().getServerTypeId());

        return proto;
    }

    public static NP2GLS_R_001_008_RegUs make_008_RegUs(_AActivityBase _activity)
    {
        NP2GLS_R_001_008_RegUs proto = new NP2GLS_R_001_008_RegUs();
        proto.setInstanceId(_activity.getGameLogicInstanceId());
        proto.setUsId(_activity.getUSServer().getServerTypeId());

        return proto;
    }

    public static NP2GLS_R_001_009_SyncGroupTeamData make_009_SyncGroupTeamData(_AActivityBase _activity, long _groupId, long _teamId)
    {
        NP2GLS_R_001_009_SyncGroupTeamData proto = new NP2GLS_R_001_009_SyncGroupTeamData();
        proto.setInstanceId(_activity.getGameLogicInstanceId());
        proto.setUsId(_activity.getUSServer().getServerTypeId());
        proto.setGroupId(_groupId);
        proto.setTeamId(_teamId);
        return proto;
    }

    public static NP2GLS_R_001_010_RemoveGroupByTeam make_010_RemoveGroupByTeam(_AActivityBase _activity, long _groupId)
    {
        NP2GLS_R_001_010_RemoveGroupByTeam proto = new NP2GLS_R_001_010_RemoveGroupByTeam();
        proto.setInstanceId(_activity.getGameLogicInstanceId());
        proto.setUsId(_activity.getUSServer().getServerTypeId());
        proto.setGroupId(_groupId);
        return proto;
    }

    public static NP2GLS_R_001_020_DealMsg make_020_DealMsg(_AActivityBase _activity, long _cid, long _playerGroupId, _IALProtocolStructure _msg, _IALProtocolStructure _addInfo)
    {
        NP2GLS_R_001_020_DealMsg proto = new NP2GLS_R_001_020_DealMsg();
        proto.setInstanceId(_activity.getGameLogicInstanceId());
        proto.setUsId(_activity.getUSServer().getServerTypeId());
        proto.setCid(_cid);
        proto.setPlayerGroupId(_playerGroupId);
        proto.setMsg(_msg.makeFullPackage());

        if(null != _addInfo)
            proto.setAddInfo(_addInfo.makePackage());

        return proto;
    }
}
