using NPEnum;
using ALPackage;
using Common.ActivityEnum;

namespace GOE
{
    /// <summary>
    /// 活动状态 C_ACTIVITY_STATE:EActivityState:activityId
    /// </summary>
    public class NPPlayerCondition_C_ACTIVITY_STATE : _ANPBasicPlayerCondition
	{
		private EActivityState _m_activityState;
        private long _m_lActivityId;
		
	    public override ENPPlayerConditionType conditionType { get { return ENPPlayerConditionType.C_ACTIVITY_STATE; } }

	    /// <summary>
	    /// 读取条件信息
	    /// </summary>
	    /// <param name="_reader"></param>
	    /// <returns></returns>
	    public static NPPlayerCondition_C_ACTIVITY_STATE readStr(ALStringReader _reader)
	    {
		    NPPlayerCondition_C_ACTIVITY_STATE cond = new NPPlayerCondition_C_ACTIVITY_STATE();

		    string activityStateStr = _reader.readItem(':');//活动状态
		    if (string.IsNullOrEmpty(activityStateStr) || !ALCommon.TryEnumParse(typeof(EActivityState), activityStateStr, out cond._m_activityState))
		    {
			    Debug.LogError($"C_ACTIVITY_STATE条件配置错误, 正确格式:C_ACTIVITY_STATE:EActivityState:activityId");
		    }
            string activityIdStr = _reader.readItem(':');//活动id

            if (string.IsNullOrEmpty(activityIdStr))
            {
                Debug.LogError($"C_ACTIVITY_STATE条件配置错误, 正确格式:C_ACTIVITY_STATE:EActivityState:activityId");
            }
			else
				cond._m_lActivityId = ALCommon.ParseLong(activityIdStr);
            return cond;
	    }
	    public override bool isEnable(NPVarInfo _varVariableInfo)
	    {
#if NP_GAME
            _ABaseActivityInfo activityInfo = NPPlayer.instance.commonActivityComp.getValidActivityInfoByActivityId(_m_lActivityId);
            EActivityState activityState = activityInfo != null ? activityInfo.activityState : EActivityState.CLOSED;
		    if (activityState == _m_activityState)
			    return true;

		    return false;
#else
	        return false;
#endif
	    }
	}
}