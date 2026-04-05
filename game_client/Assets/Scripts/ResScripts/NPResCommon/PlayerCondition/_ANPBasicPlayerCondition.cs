using System;
using System.Collections.Generic;
using ALPackage;
using GOE.Condition;
using NPEnum;

namespace GOE
{
    /*******************
    * 通用条件类型对象
    **/
    public abstract class _ANPBasicPlayerCondition : _ITNPBasicCondition<ENPPlayerConditionType
#if NP_GAME
            , NPPlayer
#endif
            , NPVarInfo
        >
    {
        /******************
        * 获取条件类型
        */
        public abstract ENPPlayerConditionType conditionType { get; }
        /*********
         * 判断条件是否满足
         **/
        public abstract bool isEnable(NPVarInfo _varVariableInfo);

#if NP_GAME
        /// <summary>
        /// 判断条件是否匹配
        /// </summary>
        /// <param name="_cond"></param>
        /// <param name="_data"></param>
        /// <param name="_varVariableInfo"></param>
        /// <returns></returns>
        public bool isEnable(NPPlayer _data, NPVarInfo _varVariableInfo)
        {
            return isEnable(_varVariableInfo);
        }
#endif


        /**************
            * 将字符串转化为本对象
            **/
        public static _ANPBasicPlayerCondition readCondition(string _str)
        {
            ALStringReader stringReader = new ALStringReader(_str);
            string condTypeS = stringReader.readItem(':');

            if (null == condTypeS)
            {
                ALLog.Warning("空的条件字符串:" + _str);
                return null;
            }

            ENPPlayerConditionType condType = ENPPlayerConditionType.NONE;
            try
            {
                //读取类型枚举
                condType = (ENPPlayerConditionType)ALCommon.EnumParse(typeof(ENPPlayerConditionType), condTypeS, true);
            }
            catch (Exception) { }

            if (condType == ENPPlayerConditionType.NONE)
            {
#if UNITY_EDITOR
                ALLog.Warning("错误的条件类型:" + _str);
#endif
                return null;
            }

            //创建对象
            return readCondition(condType, stringReader);
        }

        /********************
        * 从节点中读取相关信息
        */
        public static _ANPBasicPlayerCondition readCondition(ENPPlayerConditionType _conditionType, ALStringReader _reader)
        {
            switch (_conditionType)
            {
                case ENPPlayerConditionType.NONE:
                    return new NPPlayerConditionFalse();
                case ENPPlayerConditionType.CS_VALUE:
                    return NPPlayerCondition_CS_VALU.readStr(_reader);
                case ENPPlayerConditionType.CS_BUF_LAYER:
                    return NPPlayerCondition_CS_BUF_LAYER.readStr(_reader);
                case ENPPlayerConditionType.CS_HAS_ITEM:
                    return NPPlayerCondition_CS_HAS_ITEM.readStr(_reader);
                case ENPPlayerConditionType.CS_HAS_BUILDING:
                    return NPPlayerCondition_CS_HAS_BUILDING.readStr(_reader);
                case ENPPlayerConditionType.CS_SPECIAL:
                    return NPPlayerCondition_CS_SPECIAL.readStr(_reader);
                case ENPPlayerConditionType.CS_JUD_SIM_UNLOCK:
                    return NPPlayerCondition_CS_JUD_SIM_UNLOCK.readStr(_reader);
                case ENPPlayerConditionType.C_ID_JUDGE:
                    return NPPlayerCondition_C_ID_JUDGE.readStr(_reader);
                case ENPPlayerConditionType.CS_ID_JUDGE:
                    return NPPlayerCondition_CS_ID_JUDGE.readStr(_reader);
                case ENPPlayerConditionType.CS_RECORD_PARAM:
                    return NPPlayerCondition_CS_RECORD_PARAM.readStr(_reader);
                case ENPPlayerConditionType.CS_QUEST_COUNT:
                    return NPPlayerCondition_CS_QUEST_COUNT.readStr(_reader);
                case ENPPlayerConditionType.CS_QUEST_DOING:
                    return NPPlayerCondition_CS_QUEST_DOING.readStr(_reader);
                case ENPPlayerConditionType.CS_EVENT_RECORD:
                    return NPPlayerCondition_CS_EVENT_RECORD.readStr(_reader);
                // case ENPPlayerConditionType.C_PET_LIST_HAS_RED_TIP:
                //     return NPPlayerCondition_C_PET_LIST_HAS_RED_TIP.readStr(_reader);
                case ENPPlayerConditionType.CS_QUEST_STEP_IS_DONE:
                    return NPPlayerConditionCS_QUEST_STEP_IS_DONE.readStr(_reader);
                case ENPPlayerConditionType.CS_VARIABLE:
                    return NPPlayerCondition_CS_VARIABLE.readStr(_reader);
                case ENPPlayerConditionType.C_PLAYER_CAN_LEVEL_UP:
                    return NPPlayerCondition_C_PLAYER_CAN_LEVEL_UP.readStr(_reader);
                case ENPPlayerConditionType.CS_HAS_HERO:
                    return NPPlayerCondition_CS_HAS_HERO.readStr(_reader);
                case ENPPlayerConditionType.CS_HAS_CONSORT:
                    return NPPlayerCondition_CS_HAS_CONSORT.readStr(_reader);
                case ENPPlayerConditionType.CS_CHAPTER_STAGE_PASSED:
                    return NPPlayerCondition_CS_CHAPTER_STAGE_PASSED.readStr(_reader);
                case ENPPlayerConditionType.CS_CONSORT_SKILL:
                    return NPPlayerCondition_CS_CONSORT_SKILL.readStr(_reader);
                case ENPPlayerConditionType.C_FUNC_UNLOCK_IS_SHOW:
                    return NPPlayerCondition_C_FUNC_UNLOCK_IS_SHOW.readStr(_reader);
                case ENPPlayerConditionType.C_ACHIEVE_IS_DONE:
                    return NPPlayerCondition_C_ACHIEVE_IS_DONE.readStr(_reader);
                case ENPPlayerConditionType.C_WEEK_CARD_STAT:
                    return NPPlayerCondition_C_WEEK_CARD_STAT.readStr(_reader);
                case ENPPlayerConditionType.C_ADD_PACK_NEED_DOWNLOAD:
                    return NPPlayerCondition_C_ADD_PACK_NEED_DOWNLOAD.readStr(_reader);
                case ENPPlayerConditionType.C_CONSORT_INTIMACY_STEP_REACHED:
                    return NPPlayerCondition_C_CONSORT_INTIMACY_STEP_REACHED.readStr(_reader);
                case ENPPlayerConditionType.C_GUILD_SELF_HAVE_PERMISSION:
                    return NPPlayerCondition_C_GUILD_SELF_HAVE_PERMISSION.readStr(_reader);
                case ENPPlayerConditionType.CS_CONSORT_RES_COUNT:
                    return NPPlayerCondition_CS_CONSORT_RES_COUNT.readStr(_reader);
                case ENPPlayerConditionType.CS_CONSORT_LIKE_COUNT:
                    return NPPlayerCondition_CS_CONSORT_LIKE_COUNT.readStr(_reader);
                case ENPPlayerConditionType.CS_HERO_REACH_STAR_NUM:
                    return NPPlayerCondition_CS_HERO_REACH_STAR_NUM.readStr(_reader);
                case ENPPlayerConditionType.CS_BUILDING_FUNC_LEVEL:
                    return NPPlayerCondition_CS_BUILDING_FUNC_LEVEL.readStr(_reader);
                case ENPPlayerConditionType.C_HAS_OWNER_DINNER_REWARD:
                    return NPPlayerCondition_C_HAS_OWNER_DINNER_REWARD.readStr(_reader);
                case ENPPlayerConditionType.C_HOTFIX_CONDITION:
                    return NPPlayerCondition_C_HOTFIX_CONDITION.readStr(_reader);
                case ENPPlayerConditionType.C_EVENING_DUNGEON_ACTIVITY_STATE:
                    return NPPlayerCondition_C_EVENING_DUNGEON_ACTIVITY_STATE.readStr(_reader);
                case ENPPlayerConditionType.C_MIDDAY_DUNGEON_ACTIVITY_STATE:
                    return NPPlayerCondition_C_MIDDAY_DUNGEON_ACTIVITY_STATE.readStr(_reader);
                case ENPPlayerConditionType.C_SEVEN_DAY_LOGIN_HAD_DRAW_COUNT:
                    return NPPlayerCondition_C_SEVEN_DAY_LOGIN_HAD_DRAW_COUNT.readStr(_reader);
                case ENPPlayerConditionType.C_ACTIVITY_STATE:
                    return NPPlayerCondition_C_ACTIVITY_STATE.readStr(_reader);
                case ENPPlayerConditionType.CS_HAD_UNLOCK_INN_DISH:
                    return NPPlayerCondition_CS_HAD_UNLOCK_INN_DISH.readStr(_reader);
                case ENPPlayerConditionType.CS_HAD_UNLOCK_BUILDING_PRODUCT:
                    return NPPlayerCondition_CS_HAD_UNLOCK_BUILDING_PRODUCT.readStr(_reader);
                case ENPPlayerConditionType.C_CONSORT_CALL_DIALOG_NOT_SHOW_CG:
                    return NPPlayerCondition_C_CONSORT_CALL_DIALOG_NOT_SHOW_CG.readStr(_reader);
                case ENPPlayerConditionType.CS_MARS_BUILDING_LVL:
                    return NPPlayerCondition_CS_MARS_BUILDING_LVL.readStr(_reader);
                case ENPPlayerConditionType.CS_MARS_TECH_LVL:
                    return NPPlayerCondition_CS_MARS_TECH_LVL.readStr(_reader);
                case ENPPlayerConditionType.CS_CONSORT_UNLOCK_CG_NUM:
                    return NPPlayerCondition_CS_CONSORT_UNLOCK_CG_NUM.readStr(_reader);
                case ENPPlayerConditionType.CS_CHECK_PLAYER_PERMISSION:
                    return NPPlayerCondition_CS_CHECK_PLAYER_PERMISSION.readStr(_reader);
                case ENPPlayerConditionType.CS_MARS_PEOPLE_NUM:
                    return NPPlayerCondition_CS_MARS_PEOPLE_NUM.readStr(_reader);
                case ENPPlayerConditionType.CS_MARS_IDLE_PEOPLE_NUM:
                    return NPPlayerCondition_CS_MARS_IDLE_PEOPLE_NUM.readStr(_reader);
                case ENPPlayerConditionType.C_LOVER_COLLECT_HAS_TARGET:
                    return NPPlayerCondition_C_LOVER_COLLECT_HAS_TARGET.readStr(_reader);
                case ENPPlayerConditionType.C_LOVER_COLLECT_HAD_DRAW:
                    return NPPlayerCondition_C_LOVER_COLLECT_HAD_DRAW.readStr(_reader);
                case ENPPlayerConditionType.C_SPECIAL:
                    return NPPlayerCondition_C_SPECIAL.readStr(_reader);
                default:
#if UNITY_EDITOR
                    Debug.LogError($"找不到condition：{_conditionType}==={_reader.srcString}");
#endif
                    return new NPPlayerConditionFalse();
            }
        }

        //检查数量范围
        public static bool isRange(long _value, long _minValue, long _maxValue)
        {
            if (-1 == _value)
                return false;

            if (-1 != _minValue && _value < _minValue)
                return false;

            if (-1 != _maxValue && _value > _maxValue)
                return false;

            return true;
        }
    }
}
