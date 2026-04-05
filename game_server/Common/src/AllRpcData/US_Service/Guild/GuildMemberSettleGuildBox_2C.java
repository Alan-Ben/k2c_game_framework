package AllRpcData.US_Service.Guild;

import ALLRPC.US.Guild.GuildMemberSettleGuildBox_2C_Req;
import ALLRPC.US.Guild.GuildMemberSettleGuildBox_2C_Return;
import AllRpcData.ERpcClassName;
import RPC.Annotation.ARpc;
import RPC._ARPCBase;

@ARpc(comment ="玩家公会宝箱发送的处理")
public class GuildMemberSettleGuildBox_2C extends _ARPCBase<GuildMemberSettleGuildBox_2C_Req, GuildMemberSettleGuildBox_2C_Return>
{
	@Override
	protected GuildMemberSettleGuildBox_2C_Req createRequest()
	{
		return new GuildMemberSettleGuildBox_2C_Req();
	}

	@Override
	protected GuildMemberSettleGuildBox_2C_Return createResponse()
	{	
		return new GuildMemberSettleGuildBox_2C_Return();
	}

	public int getClassId()
	{
		return ERpcClassName.GuildMemberSettleGuildBox_2C.ordinal();
	}
}
