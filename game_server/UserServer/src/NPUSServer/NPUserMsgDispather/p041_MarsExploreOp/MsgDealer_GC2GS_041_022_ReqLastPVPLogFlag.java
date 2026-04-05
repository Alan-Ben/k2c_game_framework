package NPUSServer.NPUserMsgDispather.p041_MarsExploreOp;

import GC2GS.p041_MarsExploreOp.GC2GS_041_022_ReqLastPVPLogFlag;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_041_MarsExploreOp;

/**
 * 客户端请求该协议，推送当前最新战报的标记（创建时间戳）
 * @author mj
 *
 */
public class MsgDealer_GC2GS_041_022_ReqLastPVPLogFlag extends NPUserMsgDealer<GC2GS_041_022_ReqLastPVPLogFlag>
{
	@Override
	protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_041_022_ReqLastPVPLogFlag _msg)
	{
		// 获取玩家数据
		NPUSUserData userData = _commiter.getUserData();
		if (null == userData)
			return;

		userData.sendMsgToGC(US2GCWriter_041_MarsExploreOp.make_061_OnMarsExplorePVPLogAdd(
				userData.getMarsExploreComponent().getExploreInfo().getPVPLogLastCreated()));
	}
}
