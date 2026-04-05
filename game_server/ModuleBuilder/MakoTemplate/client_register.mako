package MGClient.GSListener;
import ALBasicProtocolPack.BasicObj._IALProtocolReceiver;
${importList}
import NPCommon.Dispather.NPCustomMsgDispatcher;

public class ${class_name}
{
	public  static void regist(GSMsgDispather _dispatcher)
	{
% for proto_name in proto_List:
		_dispatcher.regHandler(new NPCustomMsgDispatcher.NPCustomMsgDealer<${proto_name}>()
		{
			@Override
			protected void _dealMessage(_IALProtocolReceiver _receiver, ${proto_name} _msg)
			{
				GSListener dealer = (GSListener) _receiver;
				dealer.getOwner().onReceiveMsg(_msg);
			}
		});
% endfor
	}
}