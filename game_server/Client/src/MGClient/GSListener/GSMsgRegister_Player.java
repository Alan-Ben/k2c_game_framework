package MGClient.GSListener;

import ALBasicProtocolPack.BasicObj._IALProtocolReceiver;
import GC2GS.p002_InitOp.GC2GS_002_001_ReqPlayerInfo;
import GS2GC.p002_InitOp.GS2GC_002_001_RetPlayerInfo;
import MGClient.ClientPlayer.GamePlayerMgr;
import NPCommon.Dispather.NPCustomMsgDispatcher.NPCustomMsgDealer;
import NPCommon.Log.CommLog;
import NPGC2GS.p001_BasicOp.NPGC2GS_001_007_ReqQueueInfo;
import NPGS2GC.p001_BasicOp.*;

public class GSMsgRegister_Player
{
    public static void regist(GSMsgDispather _dispatcher)
    {

        _dispatcher.regHandler(new NPCustomMsgDealer<NPGS2GC_001_012_RetMostRecommendedUSInfo>()
        {
            @Override
            protected void _dealMessage(_IALProtocolReceiver _receiver, NPGS2GC_001_012_RetMostRecommendedUSInfo _msg)
            {

            }
        });
        _dispatcher.regHandler(new NPCustomMsgDealer<NPGS2GC_001_011_RetPlayerJoinedUSList>()
        {
            @Override
            protected void _dealMessage(_IALProtocolReceiver _receiver, NPGS2GC_001_011_RetPlayerJoinedUSList _msg)
            {

            }
        });

        _dispatcher.regHandler(new NPCustomMsgDealer<NPGS2GC_001_005_EnterUSRes>()
        {
            @Override
            protected void _dealMessage(_IALProtocolReceiver _receiver, NPGS2GC_001_005_EnterUSRes _msg)
            {
                GSListener dealer = (GSListener) _receiver;
                if (_msg.getError() == 0)
                {
                    CommLog.info("[login]enter queue index:{}", _msg.getQueueIndex());
                }

                dealer.send(new NPGC2GS_001_007_ReqQueueInfo());
            }
        });
        _dispatcher.regHandler(new NPCustomMsgDealer<NPGS2GC_001_007_RetQueueInfo>()
        {
            @Override
            protected void _dealMessage(_IALProtocolReceiver _receiver, NPGS2GC_001_007_RetQueueInfo _msg)
            {
                GSListener dealer = (GSListener) _receiver;
                CommLog.info("[login]007  queue index:{}", _msg.getCurEnterIndex());
            }
        });

        _dispatcher.regHandler(new NPCustomMsgDealer<NPGS2GC_001_004_OnUSEnterDone>()
        {
            @Override
            protected void _dealMessage(_IALProtocolReceiver _receiver, NPGS2GC_001_004_OnUSEnterDone _msg)
            {
                GSListener dealer = (GSListener) _receiver;
                if (_msg.getErrCode() != 0)
                {
                    //登录错误，退出
                    dealer.getOwner().logout();
                } else
                {
                    dealer.getOwner().onLoginGame();
                    GC2GS_002_001_ReqPlayerInfo proto = new GC2GS_002_001_ReqPlayerInfo();
                    dealer.getOwner().sendGameMsg(proto);
                }
            }
        });
        _dispatcher.regHandler(new NPCustomMsgDealer<GS2GC_002_001_RetPlayerInfo>()
        {
            @Override
            protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_002_001_RetPlayerInfo _msg)
            {
                GSListener dealer = (GSListener) _receiver;

                dealer.getOwner().setCid(_msg.getCid());
                GamePlayerMgr.getInstance().addLoginedPlayer(dealer.getOwner());
            }
        });
    }
}
