package NPGameRes.GameObjs.PlayerVariable;

import ALServerLog.ALServerLog;
import NPCommon.CommonObj.NPStringReader;
import NPEnum.ENPPlayerVariableType;
import NPGameRes.GameObjs.CommonObj.Variable.InterfaceObj._ITNPBasicVariable;
import NPGameRes.GameObjs.PlayerVariable.PlayerVariableObj.*;

/*******************
 * 玩家参数信息的基类
 * @author mj
 *
 */
public abstract class _ANPBasicPlayerVariableObj implements _ITNPBasicVariable<ENPPlayerVariableType>
{
    /**************
     * 将字符串转化为本对象
     **/
    public static _ANPBasicPlayerVariableObj readVariable(String _str)
    {
        NPStringReader stringReader = new NPStringReader(_str);

        String vTypeS = stringReader.readItem('@');

        if (null == vTypeS)
        {
            ALServerLog.Error("Empty Variable Type: " + _str);
            return null;
        }

        //读取类型枚举
        ENPPlayerVariableType varType = ENPPlayerVariableType.NONE;

        try
        {
            varType = ENPPlayerVariableType.valueOf(vTypeS.toUpperCase());
        } catch (Exception e)
        {
        }

        if (varType == ENPPlayerVariableType.NONE)
        {
            ALServerLog.Error("Empty Variable Type: " + _str);
            return null;
        }

        return _readVariable(varType, stringReader);
    }

    /********************
     * 从节点中读取相关信息
     */
    private static _ANPBasicPlayerVariableObj _readVariable(ENPPlayerVariableType _variableType, NPStringReader _reader)
    {
        switch (_variableType)
        {
            case S_RND:
                return NPPlayerVariable_S_RND.readVariable(_reader);
            case CS_NUM:
                return NPPlayerVariable_CS_NUM.readVariable(_reader);
            case CS_VALUE:
                return NPPlayerVariable_CS_VALUE.readVariable(_reader);
            case CS_PROPERTY:
                return NPPlayerVariable_CS_PROPERTY.readVariable(_reader);
            case CS_BUF_L:
                return NPPlayerVariable_CS_BUF_L.readVariable(_reader);
            case CS_BUFF_HAD_ACTIVE_DAY:
                return NPPlayerVariable_CS_BUFF_HAD_ACTIVE_DAY.readVariable(_reader);
            case CS_ITEM_COUNT:
                return NPPlayerVariable_CS_ITEM_COUNT.readVariable(_reader);
            case CS_PARAM:
                return NPPlayerVariable_CS_PARAM.readVariable(_reader);
            case CS_VALUE_ID:
                return NPPlayerVariable_CS_VALUE_ID.readVariable(_reader);
            case CS_VAR_V:
                return NPPlayerVariable_CS_VAR_V.readVariable(_reader);
            case CS_CONDITION_BOOLEAN_V:
                return NPPlayerVariable_CS_CONDITION_BOOLEAN_V.readVariable(_reader);
            case CS_RECORD_PARAM:
                return NPPlayerVariable_CS_RECORD_PARAM.readVariable(_reader);
            case CS_EVENT_RECORD:
                return NPPlayerVariable_CS_EVENT_RECORD.readVariable(_reader);
            case CS_SCOPE_ITEM_COUNT:
                return NPPlayerVariable_CS_SCOPE_ITEM_COUNT.readVariable(_reader);
            case S_RND_ATTR_HERO:
                return NPPlayerVariable_S_RND_ATTR_HERO.readVariable(_reader);
            case S_RND_FROM_SCOPE:
                return NPPlayerVariable_S_RND_FROM_SCOPE.readVariable(_reader);
            case S_RND_CONSORT:
                return NPPlayerVariable_S_RND_CONSORT.readVariable();
            case CS_HERO_NUM:
                return PlayerVariable_CS_HERO_NUM.readVariable(_reader);
            case S_RND_VALUE_BY_WEIGHT:
                return PlayerVariable_S_RND_VALUE_BY_WEIGHT.readVariable(_reader);
            case CS_LEVY_SUM:
            	return PlayerVariable_CS_LEVY_SUM.readVariable(_reader);
            case CS_CONSORT_RES_COUNT:
            	return PlayerVariable_CS_CONSORT_RES_COUNT.readVariable(_reader);
            case CS_HAS_BUILDING:
            	return PlayerVariable_CS_HAS_BUILDING.readVariable(_reader);
            case CS_BUSINESS_BUILDING_WORKER_NUM:
            	return PlayerVariable_CS_BUSINESS_BUILDING_WORKER_NUM.readVariable(_reader);
            case CS_BUILDING_LEVEL:
            	return PlayerVariable_CS_BUILDING_LEVEL.readVariable(_reader);
            case CS_MARS_BUILDING_LVL:
            	return NPPlayerVariable_CS_MARS_BUILDING_LVL.readVariable(_reader);
            case CS_MARS_BUILDING_EQUIP_LVL:
            	return NPPlayerVariable_CS_MARS_BUILDING_EQUIP_LVL.readVariable(_reader);
            case CS_MARS_BUILDING_DISPATCH_NUM:
            	return NPPlayerVariable_CS_MARS_BUILDING_DISPATCH_NUM.readVariable(_reader);
            default:
                return null;
        }
    }
}
