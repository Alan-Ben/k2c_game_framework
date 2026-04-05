using System;
using ALPackage;
using NPEnum;

namespace GOE
{
	/// <summary>
	/// 播放系统解锁表现过程
	/// </summary>
	public class NPPlayerEffectC_PLAY_FUNC_UNLOCK_PROCESS : _ANPPlayerEffectInfo
	{
		//系统功能类型
	    private ENPFunctionType _m_eFuncType;
		//是否展示notice类型
        private bool _m_bUseNotice;

	    public NPPlayerEffectC_PLAY_FUNC_UNLOCK_PROCESS()
	    {
	        _m_eFuncType = ENPFunctionType.NONE;
	    }

	    /************
	     * 效果类型
	     **/
	    public override ENPPlayerEffectType effectType { get { return ENPPlayerEffectType.C_PLAY_FUNC_UNLOCK_PROCESS; } }

	    public override void DealPlayerEffect(NPVarInfo _varVariableInfo)
	    {
#if NP_GAME
            FuncUnlockInfo funcUnlockInfo = NPPlayer.instance.funcUnlockComp.getFuncUnlockInfo(_m_eFuncType);
			if(_m_bUseNotice)
                NPUINoticeMgr.instance.addDealer(new NPNoticeDealer_FuncUnlockTip(funcUnlockInfo, true));
			else
				QueueMgr.instance.AddNode(new NPGAddQueueNoticeDealerNode(new NPNoticeDealer_FuncUnlockTip(funcUnlockInfo, true)));
#endif
        }

	    public static NPPlayerEffectC_PLAY_FUNC_UNLOCK_PROCESS readEffect(string _str)
	    {
	        String[] strs = _str.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
	        if (strs.Length < 1)
	        {
	            UnityEngine.Debug.LogError("Can not read str for NPPlayerEffectC_PLAY_FUNC_UNLOCK_PROCESS[" + _str + "]");
	            return null;
	        }

	        NPPlayerEffectC_PLAY_FUNC_UNLOCK_PROCESS effectObj = new NPPlayerEffectC_PLAY_FUNC_UNLOCK_PROCESS();
	        effectObj._m_eFuncType = ENPFunctionType.NONE;

	        try
	        {
	            effectObj._m_eFuncType = (ENPFunctionType)ALCommon.EnumParse(typeof(ENPFunctionType), strs[0], true);
                effectObj._m_bUseNotice = false;
                if (strs.Length > 1)
                    effectObj._m_bUseNotice = ALCommon.GetBool(strs[1]);

	            return effectObj;
	        }
	        catch(Exception)
	        {
	            UnityEngine.Debug.LogError("玩家效果——播放系统解锁表现过程——配置错误 - C_PLAY_FUNC_UNLOCK_PROCESS   example: C_PLAY_FUNC_UNLOCK_PROCESS:ENPFunctionType Error Str: " + _str);
	            return null;
	        }
	    }
	}
}