package NPGameRes.GameObjs.Travel;

import NPCommon.Game.WeightValueList;
import NPCommon.Log.CommLog;
import NPCommon.RefData._IParseFromStringable;
import NPCommon.Util.CommonFunc;

/**
 * 专用形式的权重列表 true:权重1;false:权重2
 * true/false 外部自行定义
 * 
 */
public class TravelEventWeiObj implements _IParseFromStringable
{
	//事件刷新权重(情人:NPC)，即第一个数值固定情人事件权重，第二个数值固定NPC权重
	//例：100;80，即情人事件权重100，NPC事件权重80
	//---------------------------------------- 分隔线 ----------------------------------------
	//事件刷新未获得/已获得情人权重(未获得情人事件权重:已获得情人事件权重)
	//例：100;80，即未获得情人事件权重100，已获得情人事件权重80
	private WeightValueList<Boolean> _m_alWeiObjList;
	
	public TravelEventWeiObj()
	{
		_m_alWeiObjList = new WeightValueList<>();
	}
	/**
	 * 用于默认值
	 * @param _str
	 */
	public TravelEventWeiObj(String _str)
	{
		_m_alWeiObjList = new WeightValueList<>();
		
		parseFromString(_str);
	}
	
    @Override
    public boolean parseFromString(String sValue)
    {
    	if (null == sValue || sValue.trim().isEmpty())
            return true;

        String[] strs = CommonFunc.charSplit(sValue, ';', 2);
        if(strs.length != 2)
        {
            CommLog.error("Travel Wei failed,str =" + sValue);
            return false;
        }
        
        _m_alWeiObjList.add(true, Integer.parseInt(strs[0].trim()));
        _m_alWeiObjList.add(false, Integer.parseInt(strs[1].trim()));

    	return true;
    }
    
    /**
     * 随机事件类型权重
     * @return
     */
    public boolean rndEvent()
    {
    	return _m_alWeiObjList.random();
    }
}
