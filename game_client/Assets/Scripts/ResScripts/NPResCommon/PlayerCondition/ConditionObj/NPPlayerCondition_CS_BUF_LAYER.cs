using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPEnum;

using GOE;
using ALPackage;


namespace GOE
{
	public class NPPlayerCondition_CS_BUF_LAYER : _ANPBasicPlayerCondition
	{
	    public override ENPPlayerConditionType conditionType { get { return ENPPlayerConditionType.CS_BUF_LAYER; } }

	    private long _m_lBuffId;
	    private long _m_lMinValue; // -1:不限制最小值
	    private long _m_lMaxValue; //-1:不限制最大值

	    public long buffId { get { return _m_lBuffId; } }
	    public long minValue { get { return _m_lMinValue; } }
	    public long maxValue { get { return _m_lMaxValue; } }

	    /// <summary>
	    /// 读取条件信息
	    /// </summary>
	    /// <param name="_reader"></param>
	    /// <returns></returns>
	    public static NPPlayerCondition_CS_BUF_LAYER readStr(ALStringReader _reader)
	    {
	        string buffIdS = _reader.readItem(':');
	        string minValueS = _reader.readItem(':');
        
	        if (null == buffIdS || null == minValueS)
	        {
	            UnityEngine.Debug.LogError("Can not read str for ENPPlayerConditionType.CS_BUF_LAYER[" + _reader.srcString + "]");
	            return null;
	        }

	        NPPlayerCondition_CS_BUF_LAYER cond = new NPPlayerCondition_CS_BUF_LAYER();
        
	        cond._m_lBuffId = long.Parse(buffIdS);
	        cond._m_lMinValue = long.Parse(minValueS);

	        string maxValueS = _reader.readItem(':');
	        if (null != maxValueS)
	            cond._m_lMaxValue = long.Parse(maxValueS);
	        else
	            cond._m_lMaxValue = -1;

	        return cond;
	    }
	    public override bool isEnable(NPVarInfo _varVariableInfo)
	    {
#if NP_GAME
	        int layer = 0;
	        NPPlayerBuffInfo info = NPPlayer.instance.playerBuffComp.lookup(_m_lBuffId);
	        if (null != info)
	            layer = info.layer;

	        return isRange(layer, _m_lMinValue, _m_lMaxValue);
#else
	        return false;
#endif
	    }
	}
}