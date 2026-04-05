package NPGameRes.GameObjs.PlayerCondition.ConditionObj;

import ALServerLog.ALServerLog;
import NPCommon.CommonObj.NPStringReader;
import NPEnum.ENPPlayerConditionType;
import NPGameRes.GameObjs.PlayerCondition._ANPBasicPlayerCondition;

/**
 * 检查玩家是否拥有指定权限 CS_CHECK_PLAYER_PERMISSION:权限配置ID
 * @author mark
 */
public class NPPlayerCondition_CS_CHECK_PLAYER_PERMISSION extends _ANPBasicPlayerCondition
{
    private long _m_lId;
    
    public long id() {return _m_lId;}

    @Override
    public ENPPlayerConditionType conditionType()
    {
        return ENPPlayerConditionType.CS_CHECK_PLAYER_PERMISSION;
    }

    public static NPPlayerCondition_CS_CHECK_PLAYER_PERMISSION readStr(NPStringReader _reader)
    {
        NPPlayerCondition_CS_CHECK_PLAYER_PERMISSION cond = new NPPlayerCondition_CS_CHECK_PLAYER_PERMISSION();

        String idVS = _reader.readItem();

        if (null == idVS)
        {
            ALServerLog.Error("Can not read str for ENPPlayerConditionType.CS_CHECK_PLAYER_PERMISSION[" + _reader.getSrcString() + "]");
            return null;
        }

        cond._m_lId = Long.parseLong(idVS);

        return cond;
    }
}
