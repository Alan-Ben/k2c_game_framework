package NPUSServer.DinnerMgr;

import ALBasicServer.ALTask._IALSynTask;
import NPUSServer.NPUSUserMgr.NPUSUserData;

/**
 * 检查宴会NPC数据来源
 * 1. 玩家拥有的大臣列表
 * 
 * @author mj
 *
 */
public class DinnerCheckNpcTask implements _IALSynTask
{
	private NPUSUserData _m_udUserData;
	
	public DinnerCheckNpcTask(NPUSUserData _userData)
	{
		_m_udUserData = _userData;
	}

	@Override
	public void run() 
	{
		DinnerInfo dinner = _m_udUserData.getUSServer().getDinnerPool().lookupByOwnerCid(_m_udUserData.getCid());
		if(null == dinner)
			return;
		
		dinner.checkNpc(_m_udUserData);
	}
}
