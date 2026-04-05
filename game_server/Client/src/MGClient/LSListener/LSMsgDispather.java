package MGClient.LSListener;

import ALBasicProtocolPack.BasicObj._IALProtocolReceiver;
import MGClient.ClientPlayer.ClientPlayer;
import MGClient.ClientPlayer.ClientPlayer.EClientPlayerType;
import NPCommon.Dispather.NPCustomMsgDispatcher;
import NPCommon.Log.CommLog;
import NPGC2LS.p001_BasicOp.NPGC2LS_001_002_EnterGame;
import NPLS2GC.p001_BasicOp.NPLS2GC_001_001_RetBasicInfo;
import NPLS2GC.p001_BasicOp.NPLS2GC_001_002_EnterGameRes;


public class LSMsgDispather extends NPCustomMsgDispatcher
{
    private static LSMsgDispather _g_instance = new LSMsgDispather();

    public static LSMsgDispather getInstance()
    {
        return _g_instance;
    }

    public LSMsgDispather()
    {
        this.regHandler(new NPCustomMsgDealer<NPLS2GC_001_001_RetBasicInfo>()
        {
            @Override
            protected void _dealMessage(_IALProtocolReceiver _receiver, NPLS2GC_001_001_RetBasicInfo _msg)
            {
                LSListener dealer = (LSListener) _receiver;
                dealer.getOwner().setAccountId(String.valueOf(_msg.getUid()));
                NPGC2LS_001_002_EnterGame proto = new NPGC2LS_001_002_EnterGame();
                dealer.send(proto);

            }
        });

        this.regHandler(new NPCustomMsgDealer<NPLS2GC_001_002_EnterGameRes>()
        {
            @Override
            protected void _dealMessage(_IALProtocolReceiver _receiver, NPLS2GC_001_002_EnterGameRes _msg)
            {
                LSListener dealer = (LSListener) _receiver;

//				StringBuilder sb= new StringBuilder();
//				CommonFunc.GetInfoPropertys(_msg, sb);
//				CommLog.info(sb.toString());

                ClientPlayer player = dealer.getOwner();
                if (!_msg.getRes())
                {
                    CommLog.info(" NPLS2GC_001_002_EnterGameRes failed");
                    dealer.logout();
                    return;
                }
                if (player.getPlayerType() == EClientPlayerType.enter_game_test)
                {
                    player.OnReceiveGateInfo.onEvent(true);
                    CommLog.info("enter game  test stop here,uid:{},gate ip:{},port:{}", _msg.getUid(), _msg.getGateServerIp(), _msg.getGateServerPort());
                } else
                {
                    dealer.getOwner().onReceiveGateServerInfo(_msg.getUid(), _msg.getGateServerIp(), _msg.getGateServerPort(), _msg.getCheckCode());
                    player.OnReceiveGateInfo.onEvent(false);
                }
                dealer.logout();
                return;
            }
        });


    }
}