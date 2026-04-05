package NPGameRes.GameObjs.CommonObj.Condition;

import NPCommon.Property._TBaseEnumObj;
import NPGameRes.GameObjs.CommonObj.Condition.InterfaceObj._ITNPBasicCondition;
import NPGameRes.GameObjs.CommonObj.Condition.InterfaceObj._ITNPConditionDealerData;
import NPGameRes.GameObjs.CommonObj.Condition._ATNPBasicConditionGroupObj.ENPJudgeChar;
import NPGameRes.GameObjs.CommonObj.VarInfo.NPVarInfo;

import java.util.ArrayList;

/****************
 * 条件判断的处理注册类
 * @author mj
 *
 */
public abstract class _ATNPBasicConditionDealerMgr
        <E extends Enum<E>
                , C extends _ITNPBasicCondition<E>
                , G extends _ATNPBasicConditionGroupObj<E, C, G>
                , D extends _ITNPConditionDealerData>
{
    /**
     * 枚举信息对象
     */
    protected _TBaseEnumObj<E> _m_eoEnumObj;

    //条件处理对象数组
    private _ITNPBasicConditionDealer<E, C, D>[] _m_arrDealerArr;

    @SuppressWarnings("unchecked")
    public _ATNPBasicConditionDealerMgr(Class<E> _enumClass)
    {
        _m_eoEnumObj = _TBaseEnumObj.getBaseEnum(_enumClass);

        _m_arrDealerArr = new _ITNPBasicConditionDealer[_m_eoEnumObj.getEnumLength()];
    }

    protected void _regDealer(_ITNPBasicConditionDealer<E, C, D> _dealer)
    {
        _m_arrDealerArr[_dealer.conditionType().ordinal()] = _dealer;
    }

    protected _ITNPBasicConditionDealer<E, C, D> getDealer(int _conditionIndex)
    {
        return _m_arrDealerArr[_conditionIndex];
    }

    //////////////////////////////////////////////////

    public boolean judgeEnable(G _conditionGroupObj, D _dealData, NPVarInfo _varVariableInfo)
    {
        if (null == _conditionGroupObj || !_conditionGroupObj.hasCondition())
            return true;

        //如果为none表示使用子队列条件
        if (null != _conditionGroupObj._m_cConditionObj)
            return _judgeEnable(_conditionGroupObj._m_cConditionObj, _dealData, _varVariableInfo) == _conditionGroupObj._m_bTrueEnable;

        return _judgeEnable(_conditionGroupObj._m_lChildConditionList, _dealData, _varVariableInfo, _conditionGroupObj._m_eJudgeType) == _conditionGroupObj._m_bTrueEnable;
    }

    private boolean _judgeEnable(C _condition, D _userData, NPVarInfo _varVariableInfo)
    {
        if (null == _condition)
            return true;

        //如果无数据，直接返回false
        if(null == _userData)
            return false;

        _ITNPBasicConditionDealer<E, C, D> dealer = getDealer(_condition.conditionType().ordinal());
        if (null == dealer)
            return false;

        return dealer.isEnable(_condition, _userData, _varVariableInfo);
    }

    private boolean _judgeEnable(ArrayList<G> _condGroupList, D _dealData, NPVarInfo _varVariableInfo, ENPJudgeChar _type)
    {
        if (null == _condGroupList || _condGroupList.size() <= 0)
            return true;

        //遍历条件判断
        G tmpCond = null;
        for (int i = 0; i < _condGroupList.size(); i++)
        {
            tmpCond = _condGroupList.get(i);
            if (null == tmpCond)
                continue;

            if (_type == ENPJudgeChar.OR)
            {
                //只要一个组合通过则通过
                if (judgeEnable(tmpCond, _dealData, _varVariableInfo))
                    return true;
            } else if (_type == ENPJudgeChar.AND)
            {
                //只要一个组合不通过则不通过
                if (!judgeEnable(tmpCond, _dealData, _varVariableInfo))
                    return false;
            } else
            {
                //只要一个组合不通过则不通过
                if (!judgeEnable(tmpCond, _dealData, _varVariableInfo))
                    return false;
            }
        }

        if (_type == ENPJudgeChar.OR)
            return false;
        else if (_type == ENPJudgeChar.AND)
            return true;

        return true;
    }
}
