package NPUSServer.NPUserMsgDispather.p004_PlayerOp;

import GC2GS.p004_PlayerOp.GC2GS_004_103_ReqRushExchangeReward;
import NPCommon.ErrMain.Result.Result;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.RushExchangeComp.RushExchangeComponent;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_004_PlayerOp;
import USLOGDB.OptBo.Opt004103RushExchangeClaimRewardBO;

/**
 * 领取急速兑换奖励消息处理器
 */
public class MsgDealer_GC2GS_004_103_ReqRushExchangeReward extends NPUserMsgDealer<GC2GS_004_103_ReqRushExchangeReward> {

    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _committer, GC2GS_004_103_ReqRushExchangeReward _msg) {
        // 获取用户对象
        NPUSUserData userData = _committer.getUserData();
        if (null == userData) {
            return;
        }

        // 创建操作上下文
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.RUSH_EXCHANGE_REWARD);

        RushExchangeComponent rushExchangeComponent = userData.getRushExchangeComponent();
        // 调用组件方法
        Result result = rushExchangeComponent.requestClaimReward(context);

        // 处理结果
        if (!result.isSucc()) {
            _committer.commitFailRes(result.getCode());
            return;
        }

        userData.sendMsgToGC(context.getCollector().toProto());

        // 返回成功响应
        _committer.commitSucRes(US2GCWriter_004_PlayerOp.make_103_RetRushExchangeReward());

        //日志
        Opt004103RushExchangeClaimRewardBO optBo = new Opt004103RushExchangeClaimRewardBO();
        optBo.setGroupId(getUSServer().getBM(), rushExchangeComponent.getGroupId());
        optBo.setRefId(getUSServer().getBM(), rushExchangeComponent.getRefId());
        userData.logEvent(optBo, context);
    }
}
