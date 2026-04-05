package NPUSServer.NPGeneralListener.MsgDispather;

import ALBasicProtocolPack.BasicObj._IALProtocolReceiver;
import NP2US.p002_UserOp.NP2US_002_005_OnServerMailUpdate;
import NP2US.p002_UserOp.NP2US_002_006_OnAnnouncementVersionChg;
import NPCommon.Dispather.NPCustomMsgDispatcher.NPCustomMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_007_CommOp;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;

public class NPUSGeneral_002_MsgDispather_UserOp
{
    public static void init(NPUSGeneralMsgDispather _dispather)
    {
        final NPUserServer usServer = _dispather.getUSServer();

        _dispather.regHandler(new NPCustomMsgDealer<NP2US_002_005_OnServerMailUpdate>()
        {
            @Override
            protected void _dealMessage(_IALProtocolReceiver _receiver, NP2US_002_005_OnServerMailUpdate _msg)
            {
                usServer.getAllServerMailTemplateMgr().refreshMailTemplate(_isSucc ->
                {
                    USLog.info(usServer, "NP2US_002_005_OnServerMailUpdate deal :{}", _isSucc);
                });
            }
        });

        _dispather.regHandler(new NPCustomMsgDealer<NP2US_002_006_OnAnnouncementVersionChg>()
        {
            @Override
            protected void _dealMessage(_IALProtocolReceiver _receiver, NP2US_002_006_OnAnnouncementVersionChg _msg)
            {
                usServer.getUsUserMgr().broadCastMessage(US2GCWriter_007_CommOp.make_052_OnAnnouncementVersionChg(_msg.getVersion()));
            }
        });
    }
}
