package NPUSServer.NPUserMsgDispather.p004_PlayerOp;

import GC2GS.p004_PlayerOp.GC2GS_004_042_ReqGraveRecordList;
import NPCommon.ErrMain.CommErr;
import NPGameRes.Refs.Grave.RefGraveType;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_004_PlayerOp;

public class MsgDealer_GC2GS_004_042_ReqGraveRecordList extends NPUserMsgDealer<GC2GS_004_042_ReqGraveRecordList>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_004_042_ReqGraveRecordList _msg)
    {
    	//获取用户对象，基类有做空判断，这边不做处理
        NPUSUserData userData = _commiter.getUserData();
        
        RefGraveType ref = RefGraveType.getMgr().get(_msg.getTypeId());
        if(null == ref)
        {
        	_commiter.commitFailRes(CommErr.REF_NOT_FOUND.getCode());
        	return;
        }
        
        _commiter.commitSucRes(US2GCWriter_004_PlayerOp.make_042_RetGraveRecordList(ref.player_title_id_list, _msg.getCurPage(), _msg.getPageCount(), userData));
    }
}
