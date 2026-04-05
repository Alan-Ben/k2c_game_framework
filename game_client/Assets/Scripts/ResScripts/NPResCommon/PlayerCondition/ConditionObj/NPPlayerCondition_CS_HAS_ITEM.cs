using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPEnum;

using GOE;
using ALPackage;


namespace GOE
{
	//判断是否有小兵（条件符合的）
	public class NPPlayerCondition_CS_HAS_ITEM : _ANPBasicPlayerCondition
	{
	    public override ENPPlayerConditionType conditionType { get { return ENPPlayerConditionType.CS_HAS_ITEM; } }

	    private NPEnum.ENPItemType _m_eItemType;//物品类型
	    private long _m_lItemId;//物品id
	    //判断数量
	    private WCGLongRange _m_lCountRange; // 默认值为-1忽略判断

	    public NPEnum.ENPItemType itemType { get { return _m_eItemType; } }
	    public long itemId { get { return _m_lItemId; } }
	    public WCGLongRange checkCountRange { get { return _m_lCountRange; } }

	    /// <summary>
	    /// 读取条件信息
	    /// </summary>
	    /// <param name="_reader"></param>
	    /// <returns></returns>
	    public static NPPlayerCondition_CS_HAS_ITEM readStr(ALStringReader _reader)
	    {
	        string itemTypeS = _reader.readItem('-');
	        string itemIdS = _reader.readItem(':');
	        if(null == itemTypeS || null == itemIdS)
	        {
	            UnityEngine.Debug.LogError("Can not read str for ENPPlayerConditionType.CS_HAS_ITEM[" + _reader.srcString + "]");
	            return null;
	        }

	        NPPlayerCondition_CS_HAS_ITEM cond = new NPPlayerCondition_CS_HAS_ITEM();

	        cond._m_eItemType = (NPEnum.ENPItemType)ALPackage.ALCommon.EnumParse(typeof(NPEnum.ENPItemType), itemTypeS, true);
	        cond._m_lItemId = long.Parse(itemIdS);

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

	    public override bool isEnable(NPVarInfo _varVariableInfo)
	    {
#if NP_GAME
	        return _m_lCountRange.inRange(GCommon.getItemCount(_m_eItemType, _m_lItemId));
#else
	        return false;
#endif
	    }
	}
}