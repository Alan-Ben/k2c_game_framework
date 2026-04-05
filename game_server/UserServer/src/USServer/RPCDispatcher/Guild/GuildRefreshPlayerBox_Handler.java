package USServer.RPCDispatcher.Guild;

import AllRpcData.US_Service.Guild.GuildRefreshPlayerBox;
import Common.GuildObj.Guild_BoxInfo;
import NPCommon.ErrMain.GuildErr;
import NPUSServer.Guild.GuildBox.GuildBoxTypeMgr;
import NPUSServer.Guild.GuildInfo;
import NPUSServer.Guild.Member.GuildMemberInfo;
import NPUSServer.NPUserServer;
import RPC._IAutoRegistHandler;
import USServer.RPCDispatcher._ATBasicUSRpc_Handler;

import java.util.ArrayList;

/*******
 * 增加玩家的自动互助
 */
public class GuildRefreshPlayerBox_Handler extends _ATBasicUSRpc_Handler<GuildRefreshPlayerBox>  implements _IAutoRegistHandler
{
    @Override
    protected void _deal(NPUserServer _usServer, GuildRefreshPlayerBox _rpc)
	{
		_usServer.getLoaderMgr().safeCall(()->
		{
			//获取公会信息
			GuildInfo guildInfo = _usServer.getGuildMgr().lookupGuild(_rpc.req().getGuildId());
			if(null == guildInfo)
			{
				_rpc.commitFail(GuildErr.GUILD_NOT_EXIST.getCode());
				return ;
			}

			GuildMemberInfo guildMember = guildInfo.getMemberMgr().lookup(_rpc.req().getCid());
			if(null == guildMember)
			{
				_rpc.commitFail(GuildErr.NOT_MEMBER_OF_GUILD.getCode());
				return ;
			}

			GuildBoxTypeMgr typeMgr = guildInfo.getGuildBoxMgr().getTypeMgr(_rpc.req().getBoxType());
			if(null == typeMgr)
			{
				_rpc.commitFail(GuildErr.GUILD_BOX_TYPE_ERR.getCode());
				return ;
			}

			//刷新玩家新宝箱数据
			ArrayList<Guild_BoxInfo> newBoxList = typeMgr.refreshPlayerBoxList(guildMember, _rpc.req().getNeedCount());

			if(null != newBoxList)
				_rpc.retObj().getBoxList().addAll(newBoxList);

			_rpc.commit();
		});
	}
}
