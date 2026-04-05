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
	/// 是否有自身宴会奖励未领取 C_HAS_OWNER_DINNER_REWARD
	/// </summary>
	public class NPPlayerCondition_C_HAS_OWNER_DINNER_REWARD : _ANPBasicPlayerCondition
	{
	    public override ENPPlayerConditionType conditionType { get { return ENPPlayerConditionType.C_HAS_OWNER_DINNER_REWARD; } }

	    /// <summary>
	    /// 读取条件信息
	    /// </summary>
	    /// <param name="_reader"></param>
	    /// <returns></returns>
	    public static NPPlayerCondition_C_HAS_OWNER_DINNER_REWARD readStr(ALStringReader _reader)
	    {
		    NPPlayerCondition_C_HAS_OWNER_DINNER_REWARD cond = new NPPlayerCondition_C_HAS_OWNER_DINNER_REWARD();
	        return cond;
	    }
	    public override bool isEnable(NPVarInfo _varVariableInfo)
	    {
#if NP_GAME
		    return NPPlayer.instance.dinnerComp.hasOwenrReward;
#else
	        return false;
#endif
	    }
	}
}