package  NPUSServer.NPUserMsgDispather.p014_ChildOp;

import GC2GS.p014_ChildOp.GC2GS_014_004_ReqSetChildGraduate;
import NPCommon.ErrMain.ChildErr;
import NPCommon.ErrMain.CommErr;
import NPEnum.ENPGameEvent;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.Common.Event.Events.Event_P_TRAN_ADULT;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.ChildComp.Adult.UnmarryAdultInfo;
import NPUSServer.NPUSUserMgr.UserComp.ChildComp.Child.ChildInfo;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_014_ChildOp;
import NPUSServer.USLog;
import USLOGDB.OptBo.Opt014004ChildGraduateBO;
public class  MsgDealer_GC2GS_014_004_ReqSetChildGraduate extends NPUserMsgDealer<GC2GS_014_004_ReqSetChildGraduate>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_014_004_ReqSetChildGraduate _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;

        ChildInfo child = userData.getChildComponent().getChildMgr().lookupChild(_msg.getId());
        if(null == child)
        {
        	_commiter.commitFailRes(ChildErr.CHILD_NOT_EXISTS.getCode());
        	return;
        }
        
        //检查最大等级
        if(child.getMaxLvl() == 0)
        {
        	_commiter.commitFailRes(CommErr.OBJ_ERR.getCode());
        	return;
        }
        
        //子嗣尚未满级
        if(child.getLvl() < child.getMaxLvl())
        {
        	_commiter.commitFailRes(ChildErr.CHILD_NOT_LVL_FULL.getCode());
        	return;
        }
        
        //成年未婚子嗣数量
        if(userData.getChildComponent().getAdultMgr().getUnmarryAdultCount() >= RefGeneral.Ref().unmarry_adult_limit)
        {
        	_commiter.commitFailRes(ChildErr.ADULT_COUNT_UNMARRY_FULL.getCode());
        	return;
        }
        
        //子嗣毕业流程
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.CHILD_GRADUATE);
        //创建成年子嗣
        UnmarryAdultInfo adult = userData.getChildComponent().tranChildToAdult(_msg.getId(), context);
        if(null == adult)
        {
        	USLog.error(getUSServer(), "player:{} child:{} create adult fail.", userData.getCid(), child.getChildId());
        	_commiter.commitFailRes(CommErr.OBJ_ERR.getCode());
        	return;
        }
        
        //回包处理
        _commiter.commitSucRes(US2GCWriter_014_ChildOp.make_004_RetSetChildGraduate(context, adult));
        
        //触发事件
        Event_P_TRAN_ADULT evt = new Event_P_TRAN_ADULT(context, adult.getQuality(), 1);
        userData.onLogicEvent(evt);
        
        //日志
        Opt014004ChildGraduateBO optBo = new Opt014004ChildGraduateBO();
        optBo.setChildId(getUSServer().getBM(), _msg.getId());
        optBo.setAdultId(getUSServer().getBM(), adult.getAdultId());
        userData.logEvent(optBo, context);
    }
}