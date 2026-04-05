using UnityEngine;
using System.Collections;
using System;

using ALPackage;
using NPEnum;

using GOE;


namespace GOE
{
	public class NPPlayerVariableIdValue : _ANPBasicPlayerVariableObj
	{
	    private ENPPlayerIdValueType _m_eIdValueType;//类型
	    private long _m_lId;

	    public ENPPlayerIdValueType idValueType { get { return _m_eIdValueType; } }
	    public long id { get { return _m_lId; } }

	    protected NPPlayerVariableIdValue()
	    {
	    }

	    /******************
	   * 获取条件类型
	   */
	    public override ENPPlayerVariableType variableType { get { return ENPPlayerVariableType.CS_VALUE_ID; } }

	    public override long calPlayerValue(NPVarInfo _variableInfo)
	    {
#if NP_GAME
	        switch(_m_eIdValueType)
	        {
		        case ENPPlayerIdValueType.CONSORT_INTIMACY:
		        {
			        GGottenConsortInfo consortInfo = NPPlayer.instance.consortComp.getConsortInfo(_m_lId);
			        return consortInfo?.intimacy ?? 0;
		        }
		        case ENPPlayerIdValueType.INN_STATION_LEVEL:
		        {
			        InnStationInfo stationInfo = NPPlayer.instance.innComp.getStationInfoById(_m_lId);
			        if (stationInfo is not { isBuilt: true })
				        return 0;
			        
			        return stationInfo.level;
		        }
                case ENPPlayerIdValueType.BUILDING_EARNINGS:
                {
                    BusinessBuildingInfo buildingInfo = NPPlayer.instance.buildingComp.getBusinessBuildingInfo(_m_lId);
                    return buildingInfo != null ? buildingInfo.earningsPerS : 0;
                }
	            default:
	                return 0L;
	        }
#else
	        return 0L;
#endif
	    }

	    public static NPPlayerVariableIdValue readVariable(ALStringReader _reader)
	    {
	        NPPlayerVariableIdValue variableObj = new NPPlayerVariableIdValue();

	        //解析字符串
	        string valueS = _reader.readItem('@');
	        string idS = _reader.readItem('@');
	        //逐个判断
	        if (null == valueS || null == idS)
	        {
	            UnityEngine.Debug.LogError("高级公式——Id值 - CS_VALUE_ID example: enum@type@id Error Str: " + _reader.srcString);
	            return null;
	        }

	        try
	        {
	            //解析品质列表，品质配置至少1种
	            variableObj._m_eIdValueType = (ENPPlayerIdValueType)ALCommon.EnumParse(typeof(ENPPlayerIdValueType), valueS, true);
	            variableObj._m_lId = long.Parse(idS);

	            return variableObj;
	        }
	        catch (Exception)
	        {
	            UnityEngine.Debug.LogError("高级公式——Id值 - CS_VALUE_ID example: enum@type@ Error Str: " + _reader.srcString);
	            return null;
	        }
	    }
	}
}