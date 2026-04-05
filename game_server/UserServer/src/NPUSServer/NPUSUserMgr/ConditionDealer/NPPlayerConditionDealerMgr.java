package NPUSServer.NPUSUserMgr.ConditionDealer;

import NPEnum.ENPPlayerConditionType;
import NPGameRes.GameObjs.CommonObj.Condition._ATNPBasicConditionDealerMgr;
import NPGameRes.GameObjs.CommonObj.VarInfo.NPVarInfo;
import NPGameRes.GameObjs.PlayerCondition.NPPlayerConditionGroupObj;
import NPGameRes.GameObjs.PlayerCondition._ANPBasicPlayerCondition;
import NPGameRes.Refs.RefSimpleUnlock;
import NPUSServer.NPUSUserMgr.ConditionDealer.Dealer.*;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.USLog;

public class NPPlayerConditionDealerMgr
        extends _ATNPBasicConditionDealerMgr<ENPPlayerConditionType, _ANPBasicPlayerCondition, NPPlayerConditionGroupObj, NPUSUserData>
{
    public static NPPlayerConditionDealerMgr _g_instance = new NPPlayerConditionDealerMgr();

    public static NPPlayerConditionDealerMgr getInstance()
    {
        return _g_instance;
    }

    public NPPlayerConditionDealerMgr()
    {
        super(ENPPlayerConditionType.class);

        //注册处理对象
        _regDealer(new NPPlayerConditionDealer_NONE());
        _regDealer(new NPPlayerConditionDealer_CS_VALUE());
        _regDealer(new NPPlayerConditionDealer_CS_BUF_LAYER());
        _regDealer(new NPPlayerConditionDealer_CS_HAS_ITEM());
        _regDealer(new NPPlayerConditionDealer_CS_RECORD_PARAM());
        _regDealer(new NPPlayerConditionDealer_CS_QUEST_COUNT());
        _regDealer(new NPPlayerConditionDealer_CS_QUEST_DOING());
        _regDealer(new NPPlayerConditionDealer_CS_ID_JUDGE());
        _regDealer(new NPPlayerConditionDealer_CS_EVENT_RECORD());
        _regDealer(new NPPlayerConditionDealer_CS_QUEST_STEP_IS_DONE());
        _regDealer(new NPPlayerConditionDealer_CS_VARIABLE());
        _regDealer(new NPPlayerConditionDealer_CS_SPECIAL());
        _regDealer(new NPPlayerConditionDealer_S_HAS_HERO());
        _regDealer(new NPPlayerConditionDealer_CS_CHAPTER_STAGE_PASSED());
        _regDealer(new NPPlayerConditionDealer_CS_CONSORT_SKILL());
        _regDealer(new NPPlayerConditionDealer_CS_JUD_SIM_UNLOCK());
        _regDealer(new NPPlayerConditionDealer_CS_HERO_REACH_STAR_NUM());
        _regDealer(new NPPlayerConditionDealer_CS_CONSORT_RES_COUNT());
        _regDealer(new NPPlayerConditionDealer_CS_CONSORT_LIKE_COUNT());
        _regDealer(new NPPlayerConditionDealer_CS_BUILDING_FUNC_LEVEL());
        _regDealer(new NPPlayerConditionDealer_CS_HAS_BUILDING());
        _regDealer(new NPPlayerConditionDealer_S_HAS_CONSORT());
        _regDealer(new NPPlayerConditionDealer_CS_HAD_UNLOCK_INN_DISH());
        _regDealer(new NPPlayerConditionDealer_CS_HAD_UNLOCK_BUILDING_PRODUCT());
        _regDealer(new NPPlayerConditionDealer_S_IS_SYSTEM_QUEST_GROUP_DONE());
        _regDealer(new NPPlayerConditionDealer_CS_MARS_BUILDING_LVL());
        _regDealer(new NPPlayerConditionDealer_CS_MARS_TECH_LVL());
        _regDealer(new NPPlayerConditionDealer_CS_MARS_PEOPLE_NUM());
        _regDealer(new NPPlayerConditionDealer_CS_MARS_IDLE_PEOPLE_NUM());
        _regDealer(new NPPlayerConditionDealer_CS_CONSORT_UNLOCK_CG_NUM());
        _regDealer(new NPPlayerConditionDealer_CS_CHECK_PLAYER_PERMISSION());
    }

    ///////////////////////////////////////////////////////////////

    /**
     * 判断是否满足条件
     * @param _simpleUnlockId  simpleUnlockId
     * @param _userData        玩家信息
     * @param _varVariableInfo 变量信息
     * @return 是否满足条件
     */
    public static boolean IsEnable(long _simpleUnlockId, NPUSUserData _userData, NPVarInfo _varVariableInfo)
    {
        if (_simpleUnlockId == 0)
            return true;

        RefSimpleUnlock simpleUnlockRef = RefSimpleUnlock.getMgr().get(_simpleUnlockId);
        if (simpleUnlockRef == null)
        {
            USLog.error(_userData.getUSServer(), "NPPlayerConditionDealerMgr IsEnable simpleUnlockRef not found, _simpleUnlockId:{}", _simpleUnlockId);
            return false;
        }

        return getInstance().judgeEnable(simpleUnlockRef.condition_info, _userData, _varVariableInfo);
    }

    /**
     * 判断是否满足条件
     * @param _conditionGroupObj 条件组
     * @param _userData          玩家信息
     * @param _varVariableInfo   变量信息
     * @return 是否满足条件
     */
    public static boolean IsEnable(NPPlayerConditionGroupObj _conditionGroupObj, NPUSUserData _userData, NPVarInfo _varVariableInfo)
    {
        return getInstance().judgeEnable(_conditionGroupObj, _userData, _varVariableInfo);
    }

    public static boolean isRange(long _value, long _minValue, long _maxValue)
    {
        //如果预设的值已经是-1，表明出现错误（比如配置不存在等）
        if (-1 == _value)
            return false;

        if (-1 != _minValue && _value < _minValue)
            return false;

        if (-1 != _maxValue && _value > _maxValue)
            return false;

        return true;
    }
}
