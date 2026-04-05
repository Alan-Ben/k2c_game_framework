package NPUSServer.NPUserMsgDispather.p004_PlayerOp;

import GC2GS.p004_PlayerOp.GC2GS_004_101_ReqRushExchange;
import NPCommon.ErrMain.Result.Result;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.RushExchangeComp.RushExchangeComponent;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_004_PlayerOp;
import USLOGDB.OptBo.Opt004101RushExchangeRequestBO;

/**
 * 请求急速兑换消息处理器
 */
public class MsgDealer_GC2GS_004_101_ReqRushExchange extends NPUserMsgDealer<GC2GS_004_101_ReqRushExchange> {

    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _committer, GC2GS_004_101_ReqRushExchange _msg) {
        // 获取用户对象
        NPUSUserData userData = _committer.getUserData();
        if (null == userData) {
            return;
        }

        // 创建操作上下文
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.RUSH_EXCHANGE);

        RushExchangeComponent rushExchangeComponent = userData.getRushExchangeComponent();
        // 调用组件方法
        Result result = rushExchangeComponent.requestExchange(
                _msg.getUseGemSupplement(), context);

        // 处理结果
        if (!result.isSucc()) {
            _committer.commitFailRes(result.getCode());
            return;
        }

        // 返回成功响应
        _committer.commitSucRes(US2GCWriter_004_PlayerOp.make_101_RetRushExchange());

        //日志
        Opt004101RushExchangeRequestBO optBo = new Opt004101RushExchangeRequestBO();
        optBo.setGroupId(getUSServer().getBM(), rushExchangeComponent.getGroupId());
        optBo.setRefId(getUSServer().getBM(), rushExchangeComponent.getRefId());
        optBo.setUseGemSupplement(getUSServer().getBM(), _msg.getUseGemSupplement());
        userData.logEvent(optBo, context);
    }
}
