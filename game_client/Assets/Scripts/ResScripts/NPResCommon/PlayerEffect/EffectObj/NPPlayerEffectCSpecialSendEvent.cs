using System;
using ALPackage;
using NPEnum;

namespace GOE
{
	/// <summary>
	/// 发送第三方埋点 C_SPECIAL_SEND_EVENT:EThirdCustomEventType
	/// </summary>
	public class NPPlayerEffectCSpecialSendEvent : _ANPPlayerEffectInfo
	{
	    private EThirdCustomEventType _m_eSpecialSendEventType;//第三方特殊埋点类型

		public NPPlayerEffectCSpecialSendEvent()
	    {
            _m_eSpecialSendEventType = EThirdCustomEventType.NONE;
	    }

		/// <summary>
		/// 效果类型
		/// </summary>
		public override ENPPlayerEffectType effectType { get { return ENPPlayerEffectType.C_SPECIAL_SEND_EVENT; } }

	    public override void DealPlayerEffect(NPVarInfo _varVariableInfo)
	    {
#if NP_GAME
            if (_m_eSpecialSendEventType != EThirdCustomEventType.NONE)
			{
				//发送第三方埋点
                GCommon.sendAllThirdCustomEvent(_m_eSpecialSendEventType);
			}
            else
            {
				UnityEngine.Debug.LogError($"玩家效果——发送第三方埋点——配置错误,请检查是否已接入对应类型,type:{_m_eSpecialSendEventType}");
			}
#endif
		}

	    public static NPPlayerEffectCSpecialSendEvent readEffect(string _str)
	    {
	        NPPlayerEffectCSpecialSendEvent effectObj = new NPPlayerEffectCSpecialSendEvent();

	        try
	        {
	            effectObj._m_eSpecialSendEventType = (EThirdCustomEventType)ALCommon.EnumParse(typeof(EThirdCustomEventType), _str, true);
	            return effectObj;
	        }
	        catch(Exception)
	        {
	            UnityEngine.Debug.LogError("玩家效果——发送第三方埋点——配置错误 - C_SPECIAL_SEND_EVENT   example: C_SPECIAL_SEND_EVENT:EThirdCustomEventType Error Str: " + _str);
	            return null;
	        }
	    }
	}
}