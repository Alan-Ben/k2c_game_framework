package NPUSServer.NPUserMsgDispather.p007_CommOp;

import CommonEnum.ERedDotType;
import GC2GS.p007_CommOp.GC2GS_007_032_ReqCheckRedDot;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.RedDotComp.RedDotComponent;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_007_CommOp;

/**
 * 检查红点状态请求处理器
 *
 * 功能说明：
 * 处理客户端的检查红点请求，通过业务处理器检查红点是否应该显示
 * 对比当前状态和检查结果，自动推送状态变化
 */
public class MsgDealer_GC2GS_007_032_ReqCheckRedDot extends NPUserMsgDealer<GC2GS_007_032_ReqCheckRedDot> {

	@Override
	protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_007_032_ReqCheckRedDot _msg) {
		NPUSUserData userData = _commiter.getUserData();
		if (null == userData) {
			return;
		}

		ERedDotType redDotType = _msg.getRedDotType();
		if (redDotType == null || redDotType == ERedDotType.NONE) {
			_commiter.commitSucRes(US2GCWriter_007_CommOp.make_032_RetCheckRedDot());
			return;
		}

		RedDotComponent redDotComp = userData.getRedDotComponent();

		// 检查红点是否应该显示
		redDotComp.checkAndUpdateRedDot(redDotType, true);

		// 返回成功响应
		_commiter.commitSucRes(US2GCWriter_007_CommOp.make_032_RetCheckRedDot());
	}
}
