package NPGameRes.GameObjs.CommonObj.Variable;

import NPCommon.Enum.NPCommonEnum.ENPVariableCalType;
import NPCommon.Log.CommLog;
import NPCommon.Property._TBaseEnumObj;
import NPGameRes.GameObjs.CommonObj.VarInfo.NPVarInfo;
import NPGameRes.GameObjs.CommonObj.Variable.InterfaceObj._ITNPBasicVariable;
import NPGameRes.GameObjs.CommonObj.Variable.InterfaceObj._ITNPVariableDealerData;

import java.util.ArrayList;

public abstract class _ATNPBasicVariableDealMgr
        <E extends Enum<E>
                , V extends _ITNPBasicVariable<E>
                , G extends _ATNPBasicVariableGroupObj<E, V, G>
                , D extends _ITNPVariableDealerData<E>>
{
    /**
     * 枚举信息对象
     */
    protected _TBaseEnumObj<E> _m_eoEnumObj;

    //处理对象队列
    private _ITNPBasicVariableDealer<E, V, D>[] _m_arrDealerList;

    @SuppressWarnings("unchecked")
    public _ATNPBasicVariableDealMgr(Class<E> _enumClass)
    {
        _m_eoEnumObj = _TBaseEnumObj.getBaseEnum(_enumClass);

        _m_arrDealerList = new _ITNPBasicVariableDealer[_m_eoEnumObj.getEnumLength()];
    }

    protected void _regDealer(_ITNPBasicVariableDealer<E, V, D> _dealer)
    {
        if (null == _dealer)
            return;

        _m_arrDealerList[(int) _dealer.VariableType().ordinal()] = _dealer;
    }

    /***********
     * 计算结果
     * @param _userData
     * @param _variableObj
     * @param _variableInfo
     * @return
     */
    public long CalculateVariableResult(D _data, G _groupObj, NPVarInfo _variableInfo)
    {
        if (null == _groupObj || !_groupObj.hasVariable())
            return 0;

        if (_groupObj._m_lChildGroupList.size() > 0)
        {
            //如果是减法，返回的是负值
            if (_groupObj._m_eCalType == ENPVariableCalType.SUB)
                return -_calculateVariableResult(_data, _groupObj._m_lChildGroupList, _variableInfo);
            else
                return _calculateVariableResult(_data, _groupObj._m_lChildGroupList, _variableInfo);
        }

        return _calculateVariableResult(_data, _groupObj._m_vVariableObj, _variableInfo);
    }

    /**************
     * 根据AI主体判断条件是否匹配
     **/
    public long _calculateVariableResult(D _data, ArrayList<G> _groupObjList, NPVarInfo _variableInfo)
    {
        if (null == _groupObjList || _groupObjList.size() <= 0)
            return 0;

        //遍历条件判断
        long result = 0;
        G tmpCond = null;
        for (int i = 0; i < _groupObjList.size(); i++)
        {
            tmpCond = _groupObjList.get(i);
            if (null == tmpCond)
                continue;

            switch (tmpCond._m_eCalType)
            {
                case ADD:
                case NONE:
                {
                    result += CalculateVariableResult(_data, tmpCond, _variableInfo);
                    break;
                }
                case SUB:
                {
                    result -= CalculateVariableResult(_data, tmpCond, _variableInfo);
                    break;
                }
                case MUL:
                {
                    result *= CalculateVariableResult(_data, tmpCond, _variableInfo);
                    break;
                }
                case DIV:
                {
                    long tmpValue = CalculateVariableResult(_data, tmpCond, _variableInfo);
                    if (tmpValue != 0)
                        result /= tmpValue;
                    else
                        result = 0;
                    break;
                }
                case MAX:
                {
                    long tmpValue = CalculateVariableResult(_data, tmpCond, _variableInfo);
                    if (result < tmpValue)
                        result = tmpValue;

                    break;
                }
                case MIN:
                {
                    long tmpValue = CalculateVariableResult(_data, tmpCond, _variableInfo);
                    if (result > tmpValue)
                        result = tmpValue;

                    break;
                }
            }
        }

        return result;
    }

    private long _calculateVariableResult(D _data, V _variableObj, NPVarInfo _variableInfo)
    {
        if (_variableObj == null || null == _data)
            return 0;

        _ITNPBasicVariableDealer<E, V, D> dealer = _m_arrDealerList[(int) _variableObj.variableType().ordinal()];
        if (dealer == null)
        {
            CommLog.error("处理器没有注册:" + _variableObj.variableType());
            return 0;
        }

        return dealer.PlayerVariableValue(_data, _variableObj, _variableInfo);
    }
}
