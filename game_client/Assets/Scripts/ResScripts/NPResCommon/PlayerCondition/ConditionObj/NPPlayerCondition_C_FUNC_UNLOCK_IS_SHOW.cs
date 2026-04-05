using NPEnum;
using ALPackage;

namespace GOE
{
	/// <summary>
	/// 系统解锁弹窗是否已经展示过 C_FUNC_UNLOCK_IS_SHOW:ENPFunctionType
	/// </summary>
	public class NPPlayerCondition_C_FUNC_UNLOCK_IS_SHOW : _ANPBasicPlayerCondition
	{
	    public override ENPPlayerConditionType conditionType { get { return ENPPlayerConditionType.C_FUNC_UNLOCK_IS_SHOW; } }

	    private ENPFunctionType _m_eType;
	    public ENPFunctionType functionType { get { return _m_eType; } }

	    /// <summary>
	    /// 读取条件信息
	    /// </summary>
	    /// <param name="_reader"></param>
	    /// <returns></returns>
	    public static NPPlayerCondition_C_FUNC_UNLOCK_IS_SHOW readStr(ALStringReader _reader)
	    {
	        string type = _reader.readItem(':');
	        if (null == type)
	        {
	            UnityEngine.Debug.LogError("C_FUNC_UNLOCK_IS_SHOW配置错误 解析不了枚举：" + _reader.srcString + "");
	            return null;
	        }

	        NPPlayerCondition_C_FUNC_UNLOCK_IS_SHOW cond = new NPPlayerCondition_C_FUNC_UNLOCK_IS_SHOW();

            cond._m_eType = (ENPFunctionType) ALCommon.EnumParse(typeof(ENPFunctionType), type, true);

	        return cond;
	    }
	    public override bool isEnable(NPVarInfo _varVariableInfo)
	    {
#if NP_GAME
	        //获取数据
            FuncUnlockInfo funcUnlockInfo = NPPlayer.instance.funcUnlockComp.getFuncUnlockInfo(_m_eType);
	        if(null == funcUnlockInfo || funcUnlockInfo.functionUnlockRef == null)
	            return true;

	        if(funcUnlockInfo.functionUnlockRef.ignore_pop_unlock_tip)
	            return true;

            if (!funcUnlockInfo.isUnlock)
                return false;
        
	        return funcUnlockInfo.hasShowTip;
#else
	        return true;
#endif
	    }
	}
}