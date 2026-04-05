package NPGameRes.GameObjs.PlayerVariable.PlayerVariableObj;

import ALServerLog.ALServerLog;
import NPCommon.CommonObj.NPStringReader;
import NPEnum.ENPPlayerVariableType;
import NPGameRes.GameObjs.PlayerCondition.NPPlayerConditionGroupObj;
import NPGameRes.GameObjs.PlayerVariable._ANPBasicPlayerVariableObj;

public class NPPlayerVariable_CS_CONDITION_BOOLEAN_V extends _ANPBasicPlayerVariableObj
{
    private NPPlayerConditionGroupObj _m_objConditionObj;

    public NPPlayerConditionGroupObj condition()
    {
        return _m_objConditionObj;
    }

    protected NPPlayerVariable_CS_CONDITION_BOOLEAN_V()
    {
    }

    /******************
     * 获取条件类型
     */
    @Override
    public ENPPlayerVariableType variableType()
    {
        return ENPPlayerVariableType.CS_CONDITION_BOOLEAN_V;
    }


    public static NPPlayerVariable_CS_CONDITION_BOOLEAN_V readVariable(NPStringReader _reader)
    {
        NPPlayerVariable_CS_CONDITION_BOOLEAN_V variableObj = new NPPlayerVariable_CS_CONDITION_BOOLEAN_V();

        try
        {
            String conditonS = _reader.readItem('@');
            if (null == conditonS)
            {
                ALServerLog.Error("高级公式——返回条件的布尔值 CS_CONDITION_BOOLEAN_V@（条件）条件外的括号比较特殊，不可省略 Error Str: " + _reader.getSrcString());
                return null;
            }

            variableObj._m_objConditionObj = new NPPlayerConditionGroupObj();
            if (!variableObj._m_objConditionObj.parseFromString(conditonS))
            {
                ALServerLog.Error("高级公式——返回条件的布尔值 CS_CONDITION_BOOLEAN_V@（条件）条件外的括号比较特殊，不可省略 Error Str: " + _reader.getSrcString());
                return null;
            }

            return variableObj;
        } catch (Exception e)
        {
            ALServerLog.Error("高级公式——返回条件的布尔值 CS_CONDITION_BOOLEAN_V@（条件）条件外的括号比较特殊，不可省略 Error Str: " + _reader.getSrcString());
            return null;
        }
    }
}
