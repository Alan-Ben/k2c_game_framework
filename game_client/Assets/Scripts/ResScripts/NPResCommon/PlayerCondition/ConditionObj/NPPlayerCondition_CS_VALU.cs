using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPEnum;

using GOE;
using ALPackage;


namespace GOE
{
	public class NPPlayerCondition_CS_VALU : _ANPBasicPlayerCondition
	{
	    public override ENPPlayerConditionType conditionType { get { return ENPPlayerConditionType.CS_VALUE; } }

	    private ENPPlayerValueType _m_ePlayerValueType;
	    private long _m_lMinValue; // -1:不限制最小值
	    private long _m_lMaxValue; //-1:不限制最大值

	    public ENPPlayerValueType playerValueType { get { return _m_ePlayerValueType; } }
	    public long minValue { get { return _m_lMinValue; } }
	    public long maxValue { get { return _m_lMaxValue; } }

	    /// <summary>
	    /// 读取条件信息
	    /// </summary>
	    /// <param name="_reader"></param>
	    /// <returns></returns>
	    public static NPPlayerCondition_CS_VALU readStr(ALStringReader _reader)
	    {
	        string valueTypeS = _reader.readItem(':');
	        string minVS = _reader.readItem(':');
	        if (null == valueTypeS || null == minVS)
	        {
	            UnityEngine.Debug.LogError("Can not read str for ENPPlayerConditionType.CS_VALUE[" + _reader.srcString + "]");
	            return null;
	        }

	        NPPlayerCondition_CS_VALU cond = new NPPlayerCondition_CS_VALU();
        
	        cond._m_ePlayerValueType = (ENPPlayerValueType)ALPackage.ALCommon.EnumParse(typeof(ENPPlayerValueType), valueTypeS, true);

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
	        return isRange(NPPlayer.instance.getValue(_m_ePlayerValueType), _m_lMinValue, _m_lMaxValue);
#else
	        return false;
#endif
	    }
	}
}