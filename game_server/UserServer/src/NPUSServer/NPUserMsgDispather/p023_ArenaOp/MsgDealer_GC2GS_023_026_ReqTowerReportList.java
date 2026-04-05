package NPUSServer.NPUserMsgDispather.p023_ArenaOp;

import Common.TowerObj.Tower_ReportInfo;
import GC2GS.p023_ArenaOp.GC2GS_023_026_ReqTowerReportList;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Util.Delegate.HandlerTwo;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_023_ArenaOp;

import java.util.List;

public class MsgDealer_GC2GS_023_026_ReqTowerReportList extends NPUserMsgDealer<GC2GS_023_026_ReqTowerReportList>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_023_026_ReqTowerReportList _msg)
    {
        //获取用户对象，基类有做空判断，这边不做处理
        NPUSUserData userData = _commiter.getUserData();

        userData.getTowerComponent().getDefenceReportList().makeDataList(new HandlerTwo<Result, List<Tower_ReportInfo>>() 
        {	
			@Override
			public void handle(Result _res, List<Tower_ReportInfo> _dataList) 
			{
				if(!_res.isSucc())
				{
					_commiter.commitFailRes(_res.getCode());
				}
				else
				{
					_commiter.commitSucRes(US2GCWriter_023_ArenaOp.make_026_RetTowerReportList(_dataList));
				}
			}
		});
    
	}
}
