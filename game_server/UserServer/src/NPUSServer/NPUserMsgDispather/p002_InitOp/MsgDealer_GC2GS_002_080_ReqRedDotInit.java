package NPUSServer.NPUserMsgDispather.p002_InitOp;

import Common.Common_RedDotInfo;
import GC2GS.p002_InitOp.GC2GS_002_080_ReqRedDotInit;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_002_InitOp;

import java.util.List;

/**
 * 红点初始化请求处理器
 *
 * 功能说明：
 * 处理客户端的红点初始化请求，返回当前所有激活的红点列表
 */
public class MsgDealer_GC2GS_002_080_ReqRedDotInit extends NPUserMsgDealer<GC2GS_002_080_ReqRedDotInit> {

	@Override
	protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_002_080_ReqRedDotInit _msg) {
		NPUSUserData userData = _commiter.getUserData();
		if (null == userData) {
			return;
		}

		// 获取所有激活的红点列表
		List<Common_RedDotInfo> redDotList = userData.getRedDotComponent().getAllRedDots();

		// 返回初始化响应
		_commiter.commitSucRes(US2GCWriter_002_InitOp.make_080_RetRedDotInit(redDotList));
	}
}
