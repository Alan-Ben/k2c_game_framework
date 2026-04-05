package MGClient.GSListener;

import ALBasicProtocolPack.BasicObj._IALProtocolReceiver;
import GS2GC.p004_PlayerOp.GS2GC_004_051_PushCurrencyInfo;
import GS2GC.p004_PlayerOp.GS2GC_004_054_OnPlayerParamUpdated;
import GS2GC.p006_BagItemOp.GS2GC_006_050_PushBagItemInfo;
import GS2GC.p006_BagItemOp.GS2GC_006_051_PushRemoveBagItem;
import GS2GC.p007_CommOp.GS2GC_007_050_OnGainItemList;
import GS2GC.p007_CommOp.GS2GC_007_051_OnCommError;
import GS2GC.p007_CommOp.GS2GC_007_053_OnDebugMsg;
import NPCommon.Dispather.NPCustomMsgDispatcher;
import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.Result.ResultMgr;
import NPCommon.Log.CommLog;

public class GSMsgRegister_Misc
{
    public static void regist(GSMsgDispather _dispatcher)
    {
        _dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_007_050_OnGainItemList>()
        {
            @Override
            protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_007_050_OnGainItemList _msg)
            {
                GSListener dealer = (GSListener) _receiver;
            }
        });
        _dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_007_051_OnCommError>()
        {
            @Override
            protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_007_051_OnCommError _msg)
            {
                GSListener dealer = (GSListener) _receiver;
                Result result = ResultMgr.getInstance().lookupResult(_msg.getErrCode());
                if (result == null)
                {
                    CommLog.error("UNKONW ERR:{}", _msg.getErrCode());
                }
                else
                {
                    CommLog.error(result.toString());
                }
            }
        });

        _dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_004_054_OnPlayerParamUpdated>()
        {
            @Override
            protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_004_054_OnPlayerParamUpdated _msg)
            {
                GSListener dealer = (GSListener) _receiver;
            }
        });
        _dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_006_050_PushBagItemInfo>()
        {
            @Override
            protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_006_050_PushBagItemInfo _msg)
            {
                GSListener dealer = (GSListener) _receiver;
            }
        });
        _dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_006_051_PushRemoveBagItem>()
        {
            @Override
            protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_006_051_PushRemoveBagItem _msg)
            {
                GSListener dealer = (GSListener) _receiver;
            }
        });
        _dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_007_053_OnDebugMsg>()
        {
            @Override
            protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_007_053_OnDebugMsg _msg)
            {
                GSListener dealer = (GSListener) _receiver;
            }
        });
        _dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_004_051_PushCurrencyInfo>()
        {
            @Override
            protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_004_051_PushCurrencyInfo _msg)
            {
                GSListener dealer = (GSListener) _receiver;
            }
        });


    }
}
