package MGClient.GSListener;
import ALBasicProtocolPack.BasicObj._IALProtocolReceiver;
import GS2GC.p042_GuildRelatedOp.GS2GC_042_007_RetGainGuildRewardBox;
import NPCommon.Dispather.NPCustomMsgDispatcher;

public class GSMsgRegister_GuildBox
{
	public  static void regist(GSMsgDispather _dispatcher)
	{
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<GS2GC_042_007_RetGainGuildRewardBox>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, GS2GC_042_007_RetGainGuildRewardBox _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
	}
}