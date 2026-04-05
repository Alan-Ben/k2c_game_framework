package MGClient.GSListener;
import ALBasicProtocolPack.BasicObj._IALProtocolReceiver;
import GS2GC.p021_PlayerInfo.GS2GC_021_030_RetSendFriendApply;
import GS2GC.p021_PlayerInfo.GS2GC_021_031_RetDealFriendApply;
import GS2GC.p021_PlayerInfo.GS2GC_021_032_RetRemoveFriend;
import GS2GC.p021_PlayerInfo.GS2GC_021_033_RetFriendApplyExpired;
import NPCommon.Dispather.NPCustomMsgDispatcher;

public class GSMsgRegister_Friend
{
	public  static void regist(GSMsgDispather _dispatcher)
	{
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_021_030_RetSendFriendApply>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_021_030_RetSendFriendApply _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_021_031_RetDealFriendApply>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_021_031_RetDealFriendApply _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_021_032_RetRemoveFriend>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_021_032_RetRemoveFriend _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_021_033_RetFriendApplyExpired>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_021_033_RetFriendApplyExpired _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
	}
}