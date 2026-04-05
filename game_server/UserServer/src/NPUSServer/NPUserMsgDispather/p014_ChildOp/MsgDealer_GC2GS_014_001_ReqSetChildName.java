package  NPUSServer.NPUserMsgDispather.p014_ChildOp;
import GC2GS.p014_ChildOp.GC2GS_014_001_ReqSetChildName;
import NPCommon.ErrMain.ChildErr;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.ChildComp.Child.ChildInfo;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_014_ChildOp;
import USLOGDB.OptBo.Opt014001ChildSetNameBO;
public class  MsgDealer_GC2GS_014_001_ReqSetChildName extends NPUserMsgDealer<GC2GS_014_001_ReqSetChildName>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_014_001_ReqSetChildName _msg)
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
        
        //不能重复命名
        if(!child.getName().isEmpty())
        {
        	_commiter.commitFailRes(ChildErr.CHILD_NAME_EXIST.getCode());
        	return;
        }
        
        String name = _msg.getName().trim();
        //名字长度检查
        if(name.isEmpty() || name.length() > 200)
        {
        	_commiter.commitFailRes(ChildErr.CHILD_NAME_ERROR.getCode());
        	return;
        }
        
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.CHILD_SET_NAME);
        //修改子嗣名称
        child.setName(name, context);

        _commiter.commitSucRes(US2GCWriter_014_ChildOp.make_001_RetSetChildName());

        //日志
        Opt014001ChildSetNameBO optBo = new Opt014001ChildSetNameBO();
        optBo.setChildId(getUSServer().getBM(), _msg.getId());
        optBo.setCurName(getUSServer().getBM(), _msg.getName());
        userData.logEvent(optBo, context);
    }
}