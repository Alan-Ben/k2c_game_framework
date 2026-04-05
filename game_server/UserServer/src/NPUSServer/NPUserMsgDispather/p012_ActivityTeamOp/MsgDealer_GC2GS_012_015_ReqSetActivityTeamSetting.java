package NPUSServer.NPUserMsgDispather.p012_ActivityTeamOp;

import GC2GS.p012_ActivityTeamOp.GC2GS_012_015_ReqSetActivityTeamSetting;
import NPCommon.ErrMain.ActivityErr;
import NPCommon.Util.CommonFunc;
import NPUSServer.CommonActivityMgr.Core.Team.ActivityTeamDealer;
import NPUSServer.CommonActivityMgr.Core._AActivityBase;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_012_ActivityTeamOp;

/**
 * 修改队伍设置处理器（012_015）
 * 支持同时修改：队伍名称、队伍宣言、加入方式、申请条件
 */
public class MsgDealer_GC2GS_012_015_ReqSetActivityTeamSetting extends NPUserMsgDealer<GC2GS_012_015_ReqSetActivityTeamSetting>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_012_015_ReqSetActivityTeamSetting _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;

        // 通过teamId反查对应的组队活动
        long groupId = CommonFunc.parseActivityTeamGroupId(_msg.getTeamId());
        _AActivityBase activity = userData.getUSServer().getCommActivityMgr().lookupActivityByGroupId(groupId);
        if (null == activity)
        {
            _commiter.commitFailRes(ActivityErr.ACTIVITY_NOT_FOUND.getCode());
            return;
        }

        // 检查活动的组队接口
        ActivityTeamDealer dealer = activity.getTeamDealer();
        if (null == dealer)
        {
            _commiter.commitFailRes(ActivityErr.ACTIVITY_TYPE_ERROR.getCode());
            return;
        }

        dealer.setTeamSetting(_msg.getTeamId(), userData.getCid(),
                _msg.getTeamName(), _msg.getTeamDec(),
                _msg.getJoinType(), _msg.getJoinCond(),
                result ->
                {
                    if (!result.isSucc())
                    {
                        _commiter.commitFailRes(result.getCode());
                        return;
                    }
                    _commiter.commitSucRes(US2GCWriter_012_ActivityTeamOp.make_015_RetSetActivityTeamSetting());
                });
    }
}

