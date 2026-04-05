package NPGameRes.Refs.DailyCheck;

import Common.DailyCheckObj.DailyCheck_RewardShowInfo;

/**
 * 每日登录阶段奖励展示部分数据
 * 
 * @author mj
 *
 */
public class DailyCheckLoopRewardShow 
{
	private RefDailyCheckLoopReward _m_ref;
	private int _m_iCurDay;
	
	public DailyCheckLoopRewardShow(RefDailyCheckLoopReward _ref, int _curDay)
	{
		_m_ref = _ref;
		_m_iCurDay = _curDay;
	}
	
	public RefDailyCheckLoopReward getRef() {return _m_ref;}
	public int getCurDay() {return _m_iCurDay;}
	
	public DailyCheck_RewardShowInfo toShowObj()
	{
		DailyCheck_RewardShowInfo showObj = new DailyCheck_RewardShowInfo();
		showObj.setRefId(_m_ref.id);
		showObj.setCurDay(_m_iCurDay);
	
		return showObj;
	}
}
