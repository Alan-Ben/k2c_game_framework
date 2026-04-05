package AllRpcData.US_Service.Player;

import ALLRPC.US.Player.UsSendMail_Req;
import ALLRPC.US.Player.UsSendMail_Return;
import AllRpcData.ERpcClassName;
import RPC.Annotation.ARpc;
import RPC._ARPCBase;

@ARpc(comment ="发送玩家邮件")
public class UsSendMail extends _ARPCBase<UsSendMail_Req, UsSendMail_Return>
{

	@Override
	protected UsSendMail_Req createRequest()
	{
		return new UsSendMail_Req();
	}

	@Override
	protected UsSendMail_Return createResponse()
	{	
		return new UsSendMail_Return();
	}

	public int getClassId()
	{
		return ERpcClassName.UsSendMail.ordinal();
	}
}
