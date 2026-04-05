package NPUSServer.NPUserMsgDispather.p039_MarsBuildingOp;

import GC2GS.p039_MarsBuildingOp.GC2GS_039_009_ReqCollectHomeOutput;
import NPCommon.ErrMain.MarsErr;
import NPCommon.ErrMain.Result.Result;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.MarsStep2_BuildingComp.MarsHomeBuildingFunc;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import USLOGDB.OptBo.Opt039009MarsHomeCollectBO;

public class MsgDealer_GC2GS_039_009_ReqCollectHomeOutput extends NPUserMsgDealer<GC2GS_039_009_ReqCollectHomeOutput>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_039_009_ReqCollectHomeOutput _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;

        MarsHomeBuildingFunc homeFunc = userData.getMarsBuildingComponent().getHomeFunc();
        if(null == homeFunc || null == homeFunc.getHomeLvlRef())
        {
        	_commiter.commitFailRes(MarsErr.MARS_BUILDING_NOT_FOUND.getCode());
        	return;
        }

        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.MARS_HOME_COLLECT);
        homeFunc.collectOutput(context);

        //直接返回成功代码的失败，客户端不会做额外处理
        _commiter.commitFailRes(Result.SUCC.getCode());

        //日志数据
        Opt039009MarsHomeCollectBO optBo = new Opt039009MarsHomeCollectBO();
        optBo.setHomeLvl(getUSServer().getBM(), homeFunc.getHomeLvlRef().level);
        _commiter.getUserData().logEvent(optBo, context);
    }
}
