package USServer.RPCDispatcher.Player;

import AllRpcData.US_Service.Player.UsSendMail;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.GameSystem.MailSystem.MailSystem;
import NPUSServer.NPUserServer;
import RPC._IAutoRegistHandler;
import USServer.RPCDispatcher._ATBasicUSRpc_Handler;

/*******
 * 处理逻辑：发送玩家邮件
 */
public class UsSendMail_Handler extends _ATBasicUSRpc_Handler<UsSendMail> implements _IAutoRegistHandler
{
    @Override
    protected void _deal(NPUserServer _usServer, UsSendMail _rpc)
	{
    	//构建上下文
        ENPGameEvent event = ENPGameEvent.ENPGameEvent_FromInt(_rpc.req().getGameEvent());
        NPPlayerContext context = NPPlayerContext.createNew(event);

        //发送邮件
        MailSystem.addLocalMail(_usServer, _rpc.req().getCid(), _rpc.req().getMailData(), context);

		_rpc.commit();
	}
}
