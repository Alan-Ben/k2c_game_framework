package NPUSServer.NPUserMsgDispather.p004_PlayerOp;

import GC2GS.p004_PlayerOp.GC2GS_004_013_ReqReportPlayer;
import NPCommon.ErrMain.PlayerErr;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_004_PlayerOp;

/**
 * 处理举报玩家协议 GC2GS_004_013_ReqReportPlayer
 */
public class MsgDealer_GC2GS_004_013_ReqReportPlayer extends NPUserMsgDealer<GC2GS_004_013_ReqReportPlayer>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_004_013_ReqReportPlayer _msg)
    {
        //获取用户对象，基类有做空判断，这边不做处理
        NPUSUserData userData = _commiter.getUserData();

        // 检查目标玩家，不能是自身数据
        if (_msg.getTargetCid() == userData.getCid())
        {
            _commiter.commitFailRes(PlayerErr.PLAYER_NOT_REPORT_SELF.getCode());
            return;
        }

        // 检查举报内容，不能为空且长度不能超过500
        String content = _msg.getContent();
        if (content == null || content.trim().isEmpty() || content.length() > 500)
        {
            _commiter.commitFailRes(PlayerErr.PLAYER_REPORT_CONTENT_ERROR.getCode());
            return;
        }

        userData.getReportComponent().addReport(_msg.getTargetCid(), content);

        _commiter.commitSucRes(US2GCWriter_004_PlayerOp.make_013_RetReportPlayer()); // 0: 成功
    }
}
