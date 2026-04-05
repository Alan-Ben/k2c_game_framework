using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPEnum;

using GOE;
using ALPackage;


namespace GOE
{
	/// <summary>
	/// 指定家人（未获得）好感度数值  CS_CONSORT_LIKE_COUNT:家人ID:min:max
	/// </summary>
	public class NPPlayerCondition_CS_CONSORT_LIKE_COUNT : _ANPBasicPlayerCondition
	{
	    public override ENPPlayerConditionType conditionType { get { return ENPPlayerConditionType.CS_CONSORT_LIKE_COUNT; } }

	    private long _m_lConsortId;//妃子id
	    private long _m_lMinValue; // -1:不限制最小值
	    private long _m_lMaxValue; //-1:不限制最大值

	    public long consortId { get { return _m_lConsortId; } }
	    public long minValue { get { return _m_lMinValue; } }
	    public long maxValue { get { return _m_lMaxValue; } }

	    /// <summary>
	    /// 读取条件信息
	    /// </summary>
	    /// <param name="_reader"></param>
	    /// <returns></returns>
	    public static NPPlayerCondition_CS_CONSORT_LIKE_COUNT readStr(ALStringReader _reader)
	    {
	        string consortIdS = _reader.readItem(':');
	        string minVS = _reader.readItem(':');
	        if (null == consortIdS || null == minVS)
	        {
	            UnityEngine.Debug.LogError("Can not read str for ENPPlayerConditionType.CS_CONSORT_LIKE_COUNT[" + _reader.srcString + "]");
	            return null;
	        }

	        NPPlayerCondition_CS_CONSORT_LIKE_COUNT cond = new NPPlayerCondition_CS_CONSORT_LIKE_COUNT();

	        long.TryParse(consortIdS, out cond._m_lConsortId);

	        cond._m_lMinValue = long.Parse(minVS);

	        string maxVS = _reader.readItem(':');
	        if (null != maxVS)
	            cond._m_lMaxValue = long.Parse(maxVS);
	        else
	            cond._m_lMaxValue = -1;

	        return cond;
	    }
	    public override bool isEnable(NPVarInfo _varVariableInfo)
	    {
#if NP_GAME
		    TravelConsortInfo travelConsortInfo = NPPlayer.instance.travelComp.getTravelConsortInfo(_m_lConsortId);
	        return isRange(travelConsortInfo?.like ?? 0, _m_lMinValue, _m_lMaxValue);
#else
	        return false;
#endif
	    }
	}
}