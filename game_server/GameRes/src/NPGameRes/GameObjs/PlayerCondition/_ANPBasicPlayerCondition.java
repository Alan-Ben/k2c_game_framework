package NPGameRes.GameObjs.PlayerCondition;

import ALServerLog.ALServerLog;
import NPCommon.CommonObj.NPStringReader;
import NPEnum.ENPPlayerConditionType;
import NPGameRes.GameObjs.CommonObj.Condition.InterfaceObj._ITNPBasicCondition;
import NPGameRes.GameObjs.PlayerCondition.ConditionObj.*;

abstract public class _ANPBasicPlayerCondition implements _ITNPBasicCondition<ENPPlayerConditionType>
{
    /**************
     * 将字符串转化为本对象
     **/
    public static _ANPBasicPlayerCondition readCondition(String _str)
    {
        NPStringReader stringReader = new NPStringReader(_str);
        //读取类型字符串
        String condTypeS = stringReader.readItem(':');

        if (null == condTypeS)
        {
            ALServerLog.Error("空的条件字符串:" + _str);
            return null;
        }

        ENPPlayerConditionType condType = ENPPlayerConditionType.NONE;
        try
        {
            //读取类型枚举
            condType = ENPPlayerConditionType.valueOf(condTypeS.toUpperCase());
        } catch (Exception e)
        {
        }

        if (condType == ENPPlayerConditionType.NONE)
        {
            ALServerLog.Error("错误的条件类型:" + _str);
            return null;
        }

        return readCondition(condType, stringReader);
    }

    /********************
     * 从节点中读取相关信息
     */
    public static _ANPBasicPlayerCondition readCondition(ENPPlayerConditionType _conditionType, NPStringReader _reader)
    {
        switch (_conditionType)
        {
            case NONE:
                return new NPPlayerCondition_NONE();
            case CS_VALUE:
                return NPPlayerCondition_CS_VALUE.readStr(_reader);
            case CS_BUF_LAYER:
                return NPPlayerCondition_CS_BUF_LAYER.readStr(_reader);
            case CS_HAS_ITEM:
                return NPPlayerCondition_CS_HAS_ITEM.readStr(_reader);
            case CS_JUD_SIM_UNLOCK:
                return NPPlayerCondition_CS_JUD_SIM_UNLOCK.readStr(_reader);
            case CS_RECORD_PARAM:
                return NPPlayerCondition_CS_RECORD_PARAM.readStr(_reader);
            case CS_QUEST_COUNT:
                return NPPlayerCondition_CS_QUEST_COUNT.readStr(_reader);
            case CS_QUEST_DOING:
                return NPPlayerCondition_CS_QUEST_DOING.readStr(_reader);
            case CS_ID_JUDGE:
                return NPPlayerCondition_CS_ID_JUDGE.readStr(_reader);
            case CS_EVENT_RECORD:
                return NPPlayerCondition_CS_EVENT_RECORD.readStr(_reader);
            case CS_QUEST_STEP_IS_DONE:
                return NPPlayerCondition_CS_QUEST_STEP_IS_DONE.readStr(_reader);
            case CS_VARIABLE:
                return NPPlayerCondition_CS_VARIABLE.readStr(_reader);
            case CS_SPECIAL:
                return NPPlayerCondition_CS_SPECIAL.readStr(_reader);
            case CS_HAS_HERO:
                return NPPlayerCondition_S_HAS_HERO.readStr(_reader);
            case CS_HAS_CONSORT:
                return NPPlayerCondition_S_HAS_CONSORT.readStr(_reader);
            case CS_CHAPTER_STAGE_PASSED:
                return NPPlayerCondition_CS_CHAPTER_STAGE_PASSED.readStr(_reader);
            case CS_CONSORT_SKILL:
            	return NPPlayerCondition_CS_CONSORT_SKILL.readStr(_reader);
            case CS_LEVY_SUM:
            	return NPPlayerCondition_CS_LEVY_SUM.readStr(_reader);
            case CS_HERO_REACH_STAR_NUM:
            	return NPPlayerCondition_CS_HERO_REACH_STAR_NUM.readStr(_reader);
            case CS_CONSORT_RES_COUNT:
            	return NPPlayerCondition_CS_CONSORT_RES_COUNT.readStr(_reader);
            case CS_CONSORT_LIKE_COUNT:
            	return NPPlayerCondition_CS_CONSORT_LIKE_COUNT.readStr(_reader);
            case CS_HAS_BUILDING:
            	return NPPlayerCondition_CS_HAS_BUILDING.readStr(_reader);
            case CS_BUILDING_FUNC_LEVEL:
            	return NPPlayerCondition_CS_BUILDING_FUNC_LEVEL.readStr(_reader);
            case CS_HAD_UNLOCK_INN_DISH:
            	return NPPlayerCondition_CS_HAD_UNLOCK_INN_DISH.readStr(_reader);
            case CS_HAD_UNLOCK_BUILDING_PRODUCT:
            	return NPPlayerCondition_CS_HAD_UNLOCK_BUILDING_PRODUCT.readStr(_reader);
            case S_IS_SYSTEM_QUEST_GROUP_DONE:
                return NPPlayerCondition_S_IS_SYSTEM_QUEST_GROUP_DONE.readStr(_reader);
            case CS_MARS_BUILDING_LVL:
            	return NPPlayerCondition_CS_MARS_BUILDING_LVL.readStr(_reader);
            case CS_MARS_TECH_LVL:
            	return NPPlayerCondition_CS_MARS_TECH_LVL.readStr(_reader);
            case CS_MARS_PEOPLE_NUM:
            	return NPPlayerCondition_CS_MARS_PEOPLE_NUM.readStr(_reader);
            case CS_MARS_IDLE_PEOPLE_NUM:
            	return NPPlayerCondition_CS_MARS_IDLE_PEOPLE_NUM.readStr(_reader);
            case CS_CONSORT_UNLOCK_CG_NUM:
            	return NPPlayerCondition_CS_CONSORT_UNLOCK_CG_NUM.readStr(_reader);
            case CS_CHECK_PLAYER_PERMISSION:
            	return NPPlayerCondition_CS_CHECK_PLAYER_PERMISSION.readStr(_reader);
            default:
                return new NPPlayerCondition_NONE();
        }
    }
}
