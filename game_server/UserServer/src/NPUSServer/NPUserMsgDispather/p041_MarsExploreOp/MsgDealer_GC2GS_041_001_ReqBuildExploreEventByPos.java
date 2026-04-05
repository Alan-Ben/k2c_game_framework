package NPUSServer.NPUserMsgDispather.p041_MarsExploreOp;

import GC2GS.p041_MarsExploreOp.GC2GS_041_001_ReqBuildExploreEventByPos;
import NPCommon.ErrMain.CommErr;
import NPEnum.ENPGameEvent;
import NPEnum.ENPItemType;
import NPGameRes.Refs.Mars.RefMarsExploreLvl;
import NPGameRes.Refs.Mars.RefMarsExplorePos;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_041_MarsExploreOp;

public class MsgDealer_GC2GS_041_001_ReqBuildExploreEventByPos extends NPUserMsgDealer<GC2GS_041_001_ReqBuildExploreEventByPos>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_041_001_ReqBuildExploreEventByPos _msg)
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

        //检查CD
        if (!userData.hasItem(ENPItemType.LAZY_CD, RefGeneral.Ref().mars_explore_cd, 1))
        {
            _commiter.commitFailRes(CommErr.ITEM_NOT_ENOUGH.getCode());
            return;
        }

        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.CONSORT_RND_CALL);
        //检查客户端传递的位置信息，如果为0，则与随机事件处理逻辑一致
        if(_msg.getPos() == 0) //随机位置随机事件
        {
            //消耗CD
            if (!userData.spendItem(ENPItemType.LAZY_CD, RefGeneral.Ref().mars_explore_cd, 1, context))
            {
                _commiter.commitFailRes(CommErr.CONSUME_FAIL.getCode());
                return;
            }
            
            //创建事件
            userData.getMarsExploreComponent().getEventMgr().addRefreshEvent(1, context);
        }
        else //指定位置随机事件
        {
        	//检查位置
            RefMarsExplorePos posRef = RefMarsExplorePos.getMgr().get(_msg.getPos());
            if(null == posRef)
            {
                _commiter.commitFailRes(CommErr.REF_NOT_FOUND.getCode());
                return;
            }

            //消耗CD
            if (!userData.spendItem(ENPItemType.LAZY_CD, RefGeneral.Ref().mars_explore_cd, 1, context))
            {
                _commiter.commitFailRes(CommErr.CONSUME_FAIL.getCode());
                return;
            }
            
            //创建事件
            userData.getMarsExploreComponent().getEventMgr().refreshEventByPos(posRef, context);
        }
        
        _commiter.commitSucRes(US2GCWriter_041_MarsExploreOp.make_002_RetBuildExploreEvent());

        //日志数据
    }
}
