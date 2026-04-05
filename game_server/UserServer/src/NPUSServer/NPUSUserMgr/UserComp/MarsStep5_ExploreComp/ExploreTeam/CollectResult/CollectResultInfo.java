package NPUSServer.NPUSUserMgr.UserComp.MarsStep5_ExploreComp.ExploreTeam.CollectResult;

import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import NPCommon.CommonObj.NPCommonCostItem;
import NPGameRes.Refs.Mars.RefMarsExploreMine;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.UserComp.MarsStep5_ExploreComp.ExploreTeam.MarsExploreTeam;
import NPUSServer.USLog;
import USDB.Bo.PlayerMarsTeamCollectResultBO;

/**
 * 队伍采集结果
 * @author mj
 *
 */
public class CollectResultInfo 
{
	//队伍数据
	private MarsExploreTeam _m_team;
	//采集结果数据
	private PlayerMarsTeamCollectResultBO _m_bo;
	
	public CollectResultInfo(MarsExploreTeam _team, PlayerMarsTeamCollectResultBO _bo)
	{
		_m_team = _team;
		
		_m_bo = _bo;
	}
	
	public MarsExploreTeam getTeam() {return _m_team;}
	public PlayerMarsTeamCollectResultBO getBo() {return _m_bo;}
	
	protected void _dispatch(NPPlayerContext _context) 
	{
		RefMarsExploreMine ref = RefMarsExploreMine.getMgr().get(getBo().getRefId());
		if(null == ref)
		{
			USLog.error(getTeam().getUSServer(), "player:{} mras mine:{} dispatch reward can not find ref.", getTeam().getCid(), getBo().getRefId());
			return;
		}
		
		if(getBo().getResNum() > 0)
		{
			ALSynTaskManager.getInstance().regTask(() -> 
			{
				NPCommonCostItem gainItem = new NPCommonCostItem(ref.res_type, getBo().getResNum());
				getTeam().getUserData().gainItem(gainItem, _context);
			});
		}
	}
}
