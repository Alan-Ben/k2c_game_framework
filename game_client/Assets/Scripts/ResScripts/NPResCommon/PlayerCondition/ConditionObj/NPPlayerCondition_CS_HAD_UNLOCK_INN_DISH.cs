using NPEnum;
using ALPackage;

namespace GOE
{
    /// <summary>
	/// 是否解锁旅店菜品 CS_HAD_UNLOCK_INN_DISH:菜品id
	/// </summary>
    public class NPPlayerCondition_CS_HAD_UNLOCK_INN_DISH : _ANPBasicPlayerCondition
	{
	    public override ENPPlayerConditionType conditionType { get { return ENPPlayerConditionType.CS_HAD_UNLOCK_INN_DISH; } }

	    private long _m_lDishId;//菜品id

	    /// <summary>
	    /// 读取条件信息
	    /// </summary>
	    /// <param name="_reader"></param>
	    /// <returns></returns>
	    public static NPPlayerCondition_CS_HAD_UNLOCK_INN_DISH readStr(ALStringReader _reader)
	    {
	        string dishId = _reader.readItem(':');
	        if(null == dishId)
	        {
	            UnityEngine.Debug.LogError("Can not read str for ENPPlayerConditionType.CS_HAD_UNLOCK_INN_DISH[" + _reader.srcString + "]");
	            return null;
	        }

	        NPPlayerCondition_CS_HAD_UNLOCK_INN_DISH cond = new NPPlayerCondition_CS_HAD_UNLOCK_INN_DISH();

	        cond._m_lDishId = ALCommon.ParseLong(dishId);
	        return cond;
	    }

	    public override bool isEnable(NPVarInfo _varVariableInfo)
	    {
#if NP_GAME
            InnDishInfo dishInfo = NPPlayer.instance.innComp.getDishInfoById(_m_lDishId);
            return dishInfo is { isUnlock: true };
#else
	        return false;
#endif
	    }
	}
}