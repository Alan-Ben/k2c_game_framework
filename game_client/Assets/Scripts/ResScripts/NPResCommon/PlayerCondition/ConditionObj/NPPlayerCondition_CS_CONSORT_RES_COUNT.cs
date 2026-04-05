using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPEnum;

using GOE;
using ALPackage;


namespace GOE
{
	//指定家人指定资源数量 
	public class NPPlayerCondition_CS_CONSORT_RES_COUNT : _ANPBasicPlayerCondition
	{
	    public override ENPPlayerConditionType conditionType { get { return ENPPlayerConditionType.CS_CONSORT_RES_COUNT; } }

	    //妃子id
	    private long _m_lConsortId;
	    //妃子资源类型
	    private Common.BagItemUseEnum.EBagItemUse_ConsortDrawShowType _m_eConsortResType;
	    
	    //判断数量
	    private WCGLongRange _m_lCountRange; // 默认值为-1忽略判断

	    
	    public WCGLongRange checkCountRange { get { return _m_lCountRange; } }

	    /// <summary>
	    /// 读取条件信息
	    /// </summary>
	    /// <param name="_reader"></param>
	    /// <returns></returns>
	    public static NPPlayerCondition_CS_CONSORT_RES_COUNT readStr(ALStringReader _reader)
	    {
		    try
		    {
			    string consortIdS = _reader.readItem(':');
			    string resTypeS = _reader.readItem(':');
			    if(string.IsNullOrEmpty(consortIdS) || string.IsNullOrEmpty(resTypeS))
			    {
				    UnityEngine.Debug.LogError("Can not read str for ENPPlayerConditionType.CS_CONSORT_RES_COUNT[" + _reader.srcString + "]");
				    return null;
			    }

			    NPPlayerCondition_CS_CONSORT_RES_COUNT cond = new NPPlayerCondition_CS_CONSORT_RES_COUNT();

			    cond._m_lConsortId = long.Parse(consortIdS);
			    cond._m_eConsortResType = (Common.BagItemUseEnum.EBagItemUse_ConsortDrawShowType)ALPackage.ALCommon.EnumParse(typeof(Common.BagItemUseEnum.EBagItemUse_ConsortDrawShowType), resTypeS, true);

			    string minVS = _reader.readItem(':');
			    if(null != minVS)
			    {
				    string maxVS = _reader.readItem(':');
				    if (null != maxVS)
					    cond._m_lCountRange = new WCGLongRange(minVS, maxVS);
				    else
					    cond._m_lCountRange = new WCGLongRange(minVS, "-1");
			    }
			    else
			    {
				    cond._m_lCountRange = new WCGLongRange(1, -1);
			    }

			    return cond;
		    }
		    catch (Exception e)
		    {
			    UnityEngine.Debug.LogError($"Can not read str for ENPPlayerConditionType.CS_CONSORT_RES_COUNT[{_reader.srcString}], error : {e}");
			    return null;
		    }
	    }

	    public override bool isEnable(NPVarInfo _varVariableInfo)
	    {
		    if (_m_lCountRange == null)
			    return true;
#if NP_GAME
		    long value = 0;
		    GGottenConsortInfo consortInfo = NPPlayer.instance.consortComp.getConsortInfo(_m_lConsortId);
		    switch (_m_eConsortResType)
		    {
			    case Common.BagItemUseEnum.EBagItemUse_ConsortDrawShowType.INTIMACY:
				    value = consortInfo?.intimacy ?? 0;
				    break;
			    
			    case Common.BagItemUseEnum.EBagItemUse_ConsortDrawShowType.CHARM:
				    value = consortInfo?.charm ?? 0;
				    break;
			    
			    case Common.BagItemUseEnum.EBagItemUse_ConsortDrawShowType.CHARM_POINT:
				    value = consortInfo?.charmPoint ?? 0;
				    break;
		    }
		    
	        return _m_lCountRange.inRange(value);
#else
	        return false;
#endif
	    }
	}
}