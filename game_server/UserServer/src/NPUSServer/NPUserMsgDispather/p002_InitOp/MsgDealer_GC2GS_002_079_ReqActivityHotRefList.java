package NPUSServer.NPUserMsgDispather.p002_InitOp;

import Common.ActivityObj.Activity_HotRefInfo;
import GC2GS.p002_InitOp.GC2GS_002_079_ReqActivityHotRefList;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_002_InitOp;

import java.util.List;

public class MsgDealer_GC2GS_002_079_ReqActivityHotRefList extends NPUserMsgDealer<GC2GS_002_079_ReqActivityHotRefList>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_002_079_ReqActivityHotRefList _msg)
    {
        //获取用户对象，基类有做空判断，这边不做处理
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;

        List<Activity_HotRefInfo> hotRefList = getUSServer().getUsActivityScheduleMgr().makeHotRefList();

        _commiter.commitSucRes(US2GCWriter_002_InitOp.make_079_RetActivityHotRefList(hotRefList));
    }
}
