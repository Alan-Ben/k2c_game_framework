package NPGameRes.GameObjs.Market;

import NPCommon.Game.WeightValueList;

public class MarketRewardCritWeiObj
{
	//集市奖励暴击权重
	private WeightValueList<MarketRewardObj> _m_alWeiObjList;
	
	public MarketRewardCritWeiObj()
	{
		_m_alWeiObjList = new WeightValueList<>();
	}
	
	/**
	 * 增加权重选项数据
	 * @param _obj
	 */
    public void addWeiObj(MarketRewardObj _obj)
    {
    	_m_alWeiObjList.add(_obj, _obj.weight);
    }
    
    /**
     * 随机事件类型权重
     * @return
     */
    public MarketRewardObj rndCrit()
    {
    	return _m_alWeiObjList.random();
    }
}
