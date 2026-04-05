using UnityEngine;
using System.Collections;
using System;

using ALPackage;
using NPEnum;

using GOE;


namespace GOE
{
	public class NPPlayerVariableItemCount : _ANPBasicPlayerVariableObj
	{
	    private NPEnum.ENPItemType _m_eItemType;//类型
	    private long _m_lSubId;

	    public NPEnum.ENPItemType itemType { get { return _m_eItemType; } }
	    public long subId { get { return _m_lSubId; } }

	    protected NPPlayerVariableItemCount()
	    {
	    }

	    /******************
	   * 获取条件类型
	   */
	    public override ENPPlayerVariableType variableType { get { return ENPPlayerVariableType.CS_ITEM_COUNT; } }

	    public override long calPlayerValue(NPVarInfo _variableInfo)
	    {
#if NP_GAME
	        return GCommon.getItemCount(_m_eItemType, _m_lSubId);
#else
	        return 0L;
#endif
	    }

	    public static NPPlayerVariableItemCount readVariable(ALStringReader _reader)
	    {
	        NPPlayerVariableItemCount variableObj = new NPPlayerVariableItemCount();

	        //解析字符串
	        string itemTypeS = _reader.readItem('@');
	        string subIdS = _reader.readItem('@');
	        //逐个判断
	        if (null == itemTypeS || null == subIdS)
	        {
	            UnityEngine.Debug.LogError("高级公式——物品数量 - item_count example: enum@itemType@subId Error Str: " + _reader.srcString);
	            return null;
	        }

	        try
	        {
	            //解析品质列表，品质配置至少1种
	            variableObj._m_eItemType = (NPEnum.ENPItemType)ALCommon.EnumParse(typeof(NPEnum.ENPItemType), itemTypeS, true);
	            variableObj._m_lSubId = long.Parse(subIdS);

	            return variableObj;
	        }
	        catch (Exception)
	        {
	            UnityEngine.Debug.LogError("高级公式——物品数量 - item_count example: enum@itemType@subId Error Str: " + _reader.srcString);
	            return null;
	        }
	    }
	}
}