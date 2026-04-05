package NPUSServer.CommonActivityMgr.Core.Rank;

import ALBasicServer.ALTask._IALSynTask;
import Common.GraveObj.GraveObj_NewInfo;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;

/**
 * 检查活动排行榜奖励title数据，如果有 title 奖励，下发所有相关US的 grave系统内
 * @author mj
 *
 */
public class SendGravePlayerTitleTask implements _IALSynTask
{
	private NPUserServer _m_usUserServer;
	private long _m_lCid;
	private long _m_lTitleId;

	public SendGravePlayerTitleTask(NPUserServer _usServer, long _cid, long _titleId)
	{
		_m_usUserServer = _usServer;
		_m_lCid = _cid;
		_m_lTitleId = _titleId;
	}
	
	@Override
	public void run() 
	{
		GraveObj_NewInfo info = new GraveObj_NewInfo(_m_lTitleId, _m_lCid);
		
		_m_usUserServer.getGraveMgr().addGraveNewInfo(info);
		
		//输出日志
		USLog.info(_m_usUserServer, "give grave title reward to player, cid:{}, titleId:{}", info.getCid(), info.getTitleId());
	}
}
