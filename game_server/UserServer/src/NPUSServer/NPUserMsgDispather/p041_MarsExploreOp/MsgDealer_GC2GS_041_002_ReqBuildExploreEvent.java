package NPUSServer.NPUserMsgDispather.p041_MarsExploreOp;

import GC2GS.p041_MarsExploreOp.GC2GS_041_002_ReqBuildExploreEvent;
import NPCommon.ErrMain.CommErr;
import NPEnum.ENPGameEvent;
import NPEnum.ENPItemType;
import NPGameRes.Refs.Mars.RefMarsExploreLvl;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_041_MarsExploreOp;
import USLOGDB.OptBo.Opt041002MarsExploreBuildEventBO;

public class MsgDealer_GC2GS_041_002_ReqBuildExploreEvent extends NPUserMsgDealer<GC2GS_041_002_ReqBuildExploreEvent>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_041_002_ReqBuildExploreEvent _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;
        
        //检查是否有足够位置
        RefMarsExploreLvl exploreLvlRef = userData.getMarsExploreComponent().getExploreInfo().getLvlRef();
        if(null == exploreLvlRef)
        {
        	_commiter.commitFailRes(CommErr.REF_NOT_FOUND.getCode());
        	return;
        }
        
//        ArrayList<Long> posList = exploreLvlRef.pos_list;
//        if(!userData.getMarsExploreComponent().getEventMgr().checkPosListIdle(posList))
//        {
//        	_commiter.commitFailRes(MarsErr.MARS_EXPLORE_NO_IDLE_POS.getCode());
//        	return;
//        }
        
        //检查CD
        if (!userData.hasItem(ENPItemType.LAZY_CD, RefGeneral.Ref().mars_explore_cd, 1))
        {
            _commiter.commitFailRes(CommErr.ITEM_NOT_ENOUGH.getCode());
            return;
        }

        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.CONSORT_RND_CALL);
        
        //消耗CD
        if (!userData.spendItem(ENPItemType.LAZY_CD, RefGeneral.Ref().mars_explore_cd, 1, context))
        {
            _commiter.commitFailRes(CommErr.CONSUME_FAIL.getCode());
            return;
        }
        
        //创建事件
        userData.getMarsExploreComponent().getEventMgr().addRefreshEvent(1, context);
        
        _commiter.commitSucRes(US2GCWriter_041_MarsExploreOp.make_002_RetBuildExploreEvent());

        //日志数据
        Opt041002MarsExploreBuildEventBO optBo = new Opt041002MarsExploreBuildEventBO();
        _commiter.getUserData().logEvent(optBo, context);
    }
}
