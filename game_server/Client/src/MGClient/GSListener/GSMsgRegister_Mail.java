package MGClient.GSListener;

import ALBasicProtocolPack.BasicObj._IALProtocolReceiver;
import GS2GC.p009_MailOp.*;
import NPCommon.Dispather.NPCustomMsgDispatcher;

public class GSMsgRegister_Mail
{
    public static void regist(GSMsgDispather _dispatcher)
    {
        _dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_009_051_OnMailAdded>()
        {
            @Override
            protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_009_051_OnMailAdded _msg)
            {
                GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
            }
        });

        _dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_009_052_OnMailRemoved>()
        {
            @Override
            protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_009_052_OnMailRemoved _msg)
            {
                GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
            }
        });

        _dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_009_053_OnMailLockedUpdated>()
        {
            @Override
            protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_009_053_OnMailLockedUpdated _msg)
            {
                GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
            }
        });
        _dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_009_054_OnMailReaded>()
        {
            @Override
            protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_009_054_OnMailReaded _msg)
            {
                GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
            }
        });

        _dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_009_055_OnMailRewardTaken>()
        {
            @Override
            protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_009_055_OnMailRewardTaken _msg)
            {
                GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
            }
        });
        _dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_009_056_OnMailExDataUpdated>()
        {
            @Override
            protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_009_056_OnMailExDataUpdated _msg)
            {
                GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
            }
        });
        _dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_009_057_OnMailReadOver>()
        {
            @Override
            protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_009_057_OnMailReadOver _msg)
            {
                GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
            }
        });
    }
}
