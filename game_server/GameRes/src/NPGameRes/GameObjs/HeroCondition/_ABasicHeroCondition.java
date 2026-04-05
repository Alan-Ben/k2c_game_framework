package NPGameRes.GameObjs.HeroCondition;

import ALServerLog.ALServerLog;
import Common.ConditionEnum.EHeroConditionType;
import NPCommon.CommonObj.NPStringReader;
import NPGameRes.GameObjs.CommonObj.Condition.InterfaceObj._ITNPBasicCondition;
import NPGameRes.GameObjs.HeroCondition.ConditionObj.*;

abstract public class _ABasicHeroCondition implements _ITNPBasicCondition<EHeroConditionType>
{
    /**************
     * 将字符串转化为本对象
     **/
    public static _ABasicHeroCondition readCondition(String _str)
    {
        NPStringReader stringReader = new NPStringReader(_str);
        //读取类型字符串
        String condTypeS = stringReader.readItem(':');

        if (null == condTypeS)
        {
            ALServerLog.Error("空的条件字符串:" + _str);
            return null;
        }

        EHeroConditionType condType = EHeroConditionType.NONE;
        try
        {
            //读取类型枚举
            condType = EHeroConditionType.valueOf(condTypeS.toUpperCase());
        } catch (Exception e)
        {
        }

        if (condType == EHeroConditionType.NONE)
        {
            ALServerLog.Error("错误的条件类型:" + _str);
            return null;
        }

        return readCondition(condType, stringReader);
    }

    /********************
     * 从节点中读取相关信息
     */
    public static _ABasicHeroCondition readCondition(EHeroConditionType _conditionType, NPStringReader _reader)
    {
        switch (_conditionType)
        {
            case NONE:
                return new HeroCondition_NONE();
            case CS_STAR:
                return HeroCondition_CS_STAR.readCond(_reader);
            case CS_ATTR:
                return HeroCondition_CS_ATTR.readCond(_reader);
            case CS_LEVEL:
                return HeroCondition_CS_LEVEL.readCond(_reader);
            case CS_STEP:
                return HeroCondition_CS_STEP.readCond(_reader);
            default:
                return new HeroCondition_NONE();
        }
    }
}
