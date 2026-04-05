package NPUSServer.NPUserMsgDispather.p007_CommOp;

import CommonEnum.ERedDotType;
import GC2GS.p007_CommOp.GC2GS_007_031_ReqClearRedDot;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_007_CommOp;

import java.util.List;

/**
 * 清除红点请求处理器
 *
 * 功能说明：
 * 处理客户端的清除红点请求，将指定的红点标记为已读
 */
public class MsgDealer_GC2GS_007_031_ReqClearRedDot extends NPUserMsgDealer<GC2GS_007_031_ReqClearRedDot> {

	@Override
	protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_007_031_ReqClearRedDot _msg) {
		NPUSUserData userData = _commiter.getUserData();
		if (null == userData) {
			return;
		}

		// 获取要清除的红点类型列表
		List<ERedDotType> redDotTypes = _msg.getRedDotTypes();
		if (redDotTypes == null || redDotTypes.isEmpty()) {
			_commiter.commitSucRes(US2GCWriter_007_CommOp.make_031_RetClearRedDot());
			return;
		}

		// 批量清除红点
		userData.getRedDotComponent().clearRedDots(redDotTypes, true);

		// 返回清除响应
		_commiter.commitSucRes(US2GCWriter_007_CommOp.make_031_RetClearRedDot());
	}
}
