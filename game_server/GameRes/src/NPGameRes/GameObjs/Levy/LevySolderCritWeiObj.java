package NPGameRes.GameObjs.Levy;

import NPCommon.Game.WeightValueList;
import NPCommon.Log.CommLog;
import NPCommon.RefData._IParseFromStringable;
import NPCommon.Util.CommonFunc;

public class LevySolderCritWeiObj implements _IParseFromStringable
{
	//士兵征收暴击权重
	private WeightValueList<Integer> _m_alWeiObjList;
	
	public LevySolderCritWeiObj()
	{
		_m_alWeiObjList = new WeightValueList<>();
	}
	
    @Override
    public boolean parseFromString(String sValue)
    {
    	try
    	{
        	if (null == sValue || sValue.trim().isEmpty())
                return true;

        	String[] strs = CommonFunc.charSplit(sValue, ';');
            
            for(int i = 0; i < strs.length; i++)
            {
            	String[] critStr = CommonFunc.charSplit(strs[i], ':', 2);
            	if(critStr.length != 2)
                {
                    CommLog.error("Levy Solder Crit Wei failed,str =" + strs[i]);
                    return false;
                }

                //权重对象
                _m_alWeiObjList.add(Integer.parseInt(critStr[0].trim()), Integer.parseInt(critStr[1].trim()));
            }
            
        	return true;
    	}
    	catch(Exception e)
    	{
    		CommLog.error("", e);
    		return false;
    	}
    }
    
    /**
     * 随机事件类型权重
     * @return
     */
    public int rndCrit()
    {
    	return _m_alWeiObjList.random();
    }
}
