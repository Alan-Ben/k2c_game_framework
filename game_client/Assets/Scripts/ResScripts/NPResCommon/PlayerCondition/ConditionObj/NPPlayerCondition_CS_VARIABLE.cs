using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPEnum;

using GOE;
using ALPackage;


namespace GOE
{
	public class NPPlayerCondition_CS_VARIABLE : _ANPBasicPlayerCondition
	{
	    public override ENPPlayerConditionType conditionType { get { return ENPPlayerConditionType.CS_VARIABLE; } }

	    private _NPPlayerVariableSerializeInfo _m_playerVariable;
	    
	    //范围
	    private WCGLongRange _m_rCountRng;

		/// <summary>
		/// 高级公式 
		/// </summary>
	    public _NPPlayerVariableSerializeInfo playerValueType { get { return _m_playerVariable; } }
		/// <summary>
		/// 范围
		/// </summary>
	    public WCGLongRange countRng { get { return _m_rCountRng; } }

	    /// <summary>
	    /// 读取条件信息
	    /// </summary>
	    /// <param name="_reader"></param>
	    /// <returns></returns>
	    public static NPPlayerCondition_CS_VARIABLE readStr(ALStringReader _reader)
	    {
		    
		    //解析字符串
		    string minS = _reader.readItem(':');
		    string maxS = _reader.readItem(':');
		    string variableS = _reader.readItem(':');
	        if (null == minS || null == maxS || null == variableS)
	        {
	            UnityEngine.Debug.LogError("Can not read str for ENPPlayerConditionType.CS_VARIABLE[" + _reader.srcString + "]");
	            return null;
	        }
	     
		     NPPlayerCondition_CS_VARIABLE cond = new NPPlayerCondition_CS_VARIABLE();

		     cond._m_playerVariable = new _NPPlayerVariableSerializeInfo();
		     cond._m_playerVariable.s_variable = variableS;
		     long min = -1;
		     long max = -1;
		     if (null != minS)
			     min = long.Parse(minS);
		     if (null != maxS)
			     max = long.Parse(maxS);
		     cond._m_rCountRng = new WCGLongRange(min, max);
		     
		     return cond;
	    }
	    public override bool isEnable(NPVarInfo _varVariableInfo)
	    {
#if NP_GAME
	        return _m_rCountRng.inRange(_m_playerVariable.CalculateVariableResult(null));
	        
#else
	        return false;
#endif
	    }
	}
}