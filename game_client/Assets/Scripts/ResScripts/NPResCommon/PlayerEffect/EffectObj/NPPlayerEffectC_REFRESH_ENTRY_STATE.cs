using System;
using ALPackage;
using NPEnum;

namespace GOE
{
	/// <summary>
	/// 刷新入口状态
	/// </summary>
	public class NPPlayerEffectC_REFRESH_ENTRY_STATE : _ANPPlayerEffectInfo
	{
	    private ENPFunctionType _m_eFuncType;

	    public NPPlayerEffectC_REFRESH_ENTRY_STATE()
	    {
	        _m_eFuncType = ENPFunctionType.NONE;
	    }

	    /************
	     * 效果类型
	     **/
	    public override ENPPlayerEffectType effectType { get { return ENPPlayerEffectType.C_REFRESH_ENTRY_STATE; } }

	    public override void DealPlayerEffect(NPVarInfo _varVariableInfo)
	    {
#if NP_GAME
            FuncUnlockInfo funcUnlockInfo = NPPlayer.instance.funcUnlockComp.getFuncUnlockInfo(_m_eFuncType);
            if (funcUnlockInfo == null)
                return;

            if (funcUnlockInfo.isUnlock)
				NPPlayer.instance.funcUnlockComp.setShowTipDone(_m_eFuncType);

			//发送消息刷新入口状态
			WinMsg.SendMsg(WinMsgType.ON_REFRESH_ENTRY_STATE, _m_eFuncType);
#endif
        }

	    public static NPPlayerEffectC_REFRESH_ENTRY_STATE readEffect(string _str)
	    {
	        String[] strs = _str.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
	        if (strs.Length < 1)
	        {
	            UnityEngine.Debug.LogError("Can not read str for NPPlayerEffectC_REFRESH_ENTRY_STATE[" + _str + "]");
	            return null;
	        }

	        NPPlayerEffectC_REFRESH_ENTRY_STATE effectObj = new NPPlayerEffectC_REFRESH_ENTRY_STATE();
	        effectObj._m_eFuncType = ENPFunctionType.NONE;

	        try
	        {
	            effectObj._m_eFuncType = (ENPFunctionType)ALCommon.EnumParse(typeof(ENPFunctionType), strs[0], true);
	            return effectObj;
	        }
	        catch(Exception)
	        {
	            UnityEngine.Debug.LogError("玩家效果——刷新入口状态——配置错误 - C_REFRESH_ENTRY_STATE   example: C_REFRESH_ENTRY_STATE:ENPFunctionType Error Str: " + _str);
	            return null;
	        }
	    }
	}
}