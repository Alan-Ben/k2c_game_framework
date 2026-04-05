package NPServerProtocolWriter.NP2US.Msg;

import NP2US.p002_UserOp.NP2US_002_001_PveBattleEnd;
import NP2US.p002_UserOp.NP2US_002_005_OnServerMailUpdate;

public class NP2US_Writer_002_UserOp
{
    public static NP2US_002_001_PveBattleEnd make_001_PveBattleEnd(long _uid, boolean _isWin, long _roomSerialize, long _stageSerialize, long _battleMS)
    {
        NP2US_002_001_PveBattleEnd protocol = new NP2US_002_001_PveBattleEnd();

        protocol.setCid(_uid);
        protocol.setIsWin(_isWin);
        protocol.setRoomSerial(_roomSerialize);
        protocol.setStageSerial(_stageSerialize);
        protocol.setBattleMs(_battleMS);

        return protocol;
    }
	
    public static NP2US_002_005_OnServerMailUpdate make_005_OnServerMailUpdate(long _phpMailId)
    {
        NP2US_002_005_OnServerMailUpdate protocol = new NP2US_002_005_OnServerMailUpdate();
        protocol.setPhpMailId(_phpMailId);

        return protocol;
    }
}
