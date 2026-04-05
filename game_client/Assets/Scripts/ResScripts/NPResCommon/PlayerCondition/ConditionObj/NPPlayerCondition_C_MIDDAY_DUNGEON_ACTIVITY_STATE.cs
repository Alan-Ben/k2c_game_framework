using NPEnum;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 午间副本活动状态 C_EVENING_DUNGEON_ACTIVITY_STATE:(EMiddayDungeonActivityState:PREVIEW:ONGOING:END:CLOSE)
    /// </summary>
    public class NPPlayerCondition_C_MIDDAY_DUNGEON_ACTIVITY_STATE : _ANPBasicPlayerCondition
    {
        private EMiddayDungeonActivityState _m_activityState;
		
        public override ENPPlayerConditionType conditionType { get { return ENPPlayerConditionType.C_MIDDAY_DUNGEON_ACTIVITY_STATE; } }

        /// <summary>
        /// 读取条件信息
        /// </summary>
        /// <param name="_reader"></param>
        /// <returns></returns>
        public static NPPlayerCondition_C_MIDDAY_DUNGEON_ACTIVITY_STATE readStr(ALStringReader _reader)
        {
            NPPlayerCondition_C_MIDDAY_DUNGEON_ACTIVITY_STATE cond = new NPPlayerCondition_C_MIDDAY_DUNGEON_ACTIVITY_STATE();

            string activityStateStr = _reader.readItem(':');//活动状态
            if (string.IsNullOrEmpty(activityStateStr) || !ALCommon.TryEnumParse(typeof(EEveningDungeonActivityState), activityStateStr, out cond._m_activityState))
            {
                Debug.LogError($"C_MIDDAY_DUNGEON_ACTIVITY_STATE条件配置错误, 正确格式:C_MIDDAY_DUNGEON_ACTIVITY_STATE:PREVIEW(|ONGOING|END|CLOSE)");
            }
            return cond;
        }
        public override bool isEnable(NPVarInfo _varVariableInfo)
        {
#if NP_GAME
            EMiddayDungeonActivityState activityState = NPPlayer.instance.middayDungeonComp.activityState;
            if (activityState == _m_activityState)
                return true;

            return false;
#else
	        return false;
#endif
        }
    }
}