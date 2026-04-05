using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPEnum;

using GOE;
using ALPackage;


namespace GOE
{
	public class NPPlayerCondition_CS_JUD_SIM_UNLOCK : _ANPBasicPlayerCondition
	{
	    public override ENPPlayerConditionType conditionType { get { return ENPPlayerConditionType.CS_JUD_SIM_UNLOCK; } }

	    private long _m_lSimUnlockId;
	    public long simUnlockId { get { return _m_lSimUnlockId; } }

	    /// <summary>
	    /// 读取条件信息
	    /// </summary>
	    /// <param name="_reader"></param>
	    /// <returns></returns>
	    public static NPPlayerCondition_CS_JUD_SIM_UNLOCK readStr(ALStringReader _reader)
	    {
	        string unlockIdS = _reader.readItem(':');
	        if (null == unlockIdS)
	        {
	            UnityEngine.Debug.LogError("CS_JUD_SIM_UNLOCK配置错误 解析不了id：" + _reader.srcString + "");
	            return null;
	        }

	        NPPlayerCondition_CS_JUD_SIM_UNLOCK cond = new NPPlayerCondition_CS_JUD_SIM_UNLOCK();
        
	        cond._m_lSimUnlockId = long.Parse(unlockIdS);

	        return cond;
	    }
	    public override bool isEnable(NPVarInfo _varVariableInfo)
	    {
#if NP_GAME
	        //获取数据
	        NPSimpleUnlockRef unlockRef = GRefdataCoreMgr.instance.simpleUnlockMap.getRef(_m_lSimUnlockId);
	        if(null == unlockRef)
	            return false;

	        //判断玩家等级与关卡
	        if(unlockRef.isConditionEnable(null))
	            return true;
        
	        return false;
#else
	        return false;
#endif
	    }
	}
}