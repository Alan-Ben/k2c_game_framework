package NPUSServer.NPUserMsgDispather.p041_MarsExploreOp;

import GC2GS.p041_MarsExploreOp.GC2GS_041_023_ReqGuildMarsMineShareFlag;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;

/**
 * 火星探险-请求最新的联盟分享火星矿的标志
 * @author mj
 *
 */
public class MsgDealer_GC2GS_041_023_ReqGuildMarsMineShareFlag extends NPUserMsgDealer<GC2GS_041_023_ReqGuildMarsMineShareFlag>
{
	@Override
	protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_041_023_ReqGuildMarsMineShareFlag _msg)
	{
		// 获取玩家数据
		NPUSUserData userData = _commiter.getUserData();
		if (null == userData)
			return;

		if(userData.getGuildComponent().getGuildId() <= 0)
		{
			return;
		}

		//转化为统一跨服消息进行处理
		getUSServer().dealGuildMsg(
				_commiter,
				_commiter.getUserData().getCid(),
				_commiter.getUserData().getGuildComponent().getGuildId(),
				_msg,
				null
		);
	}
}
