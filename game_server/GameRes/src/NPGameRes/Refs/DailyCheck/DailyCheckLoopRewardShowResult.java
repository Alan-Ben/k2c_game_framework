package NPGameRes.Refs.DailyCheck;

import java.util.ArrayList;

/**
 * 每日登录阶段奖励展示部分数据
 * 
 * @author mj
 *
 */
public class DailyCheckLoopRewardShowResult 
{
	//当前分组数据列表
	private ArrayList<DailyCheckLoopRewardShow> _m_alShowList;
	//当前分组的前一条数据
	private DailyCheckLoopRewardShow _m_preShow;
	//当前分组的后一条数据
	private DailyCheckLoopRewardShow _m_nextShow;
	
	public DailyCheckLoopRewardShowResult()
	{
		_m_alShowList = new ArrayList<>();
	}

	public void addCurShow(DailyCheckLoopRewardShow _show) {_m_alShowList.add(_show);}
	public void setPreShow(DailyCheckLoopRewardShow _show) {_m_preShow = _show;}
	public void setNextShow(DailyCheckLoopRewardShow _show) {_m_nextShow = _show;}
	
	public ArrayList<DailyCheckLoopRewardShow> getShowList() {return _m_alShowList;}
	public DailyCheckLoopRewardShow getPreShow() {return _m_preShow;}
	public DailyCheckLoopRewardShow getNextShow() {return _m_nextShow;}
	
	/**
	 * 获取当前列表的最后一天
	 * @return
	 */
	public int getCurLastDay()
	{
		if(_m_alShowList.isEmpty())
			return -1;
		
		return _m_alShowList.get(_m_alShowList.size() - 1).getCurDay();
	}
}
