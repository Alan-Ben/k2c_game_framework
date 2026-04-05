package NPGameRes.GameObjs.PlayerCondition.ConditionObj;

import ALServerLog.ALServerLog;
import NPCommon.CommonObj.NPStringReader;
import NPEnum.ENPPlayerConditionType;
import NPEnum.ENPPlayer_CS_SpecialCondition;
import NPGameRes.GameObjs.PlayerCondition._ANPBasicPlayerCondition;

/**
 * 特殊判断条件 CS_SPECIAL:ENPPlayer_CS_SpecialCondition
 * @author mark
 */
public class NPPlayerCondition_CS_SPECIAL extends _ANPBasicPlayerCondition
{
    private ENPPlayer_CS_SpecialCondition _m_eType;

    public ENPPlayer_CS_SpecialCondition type()
    {
        return _m_eType;
    }

    @Override
    public ENPPlayerConditionType conditionType()
    {
        return ENPPlayerConditionType.CS_SPECIAL;
    }

    public static NPPlayerCondition_CS_SPECIAL readStr(NPStringReader _reader)
    {
        NPPlayerCondition_CS_SPECIAL cond = new NPPlayerCondition_CS_SPECIAL();

        String typeS = _reader.readItem();

        if (null == typeS)
        {
            ALServerLog.Error("Can not read str for ENPPlayerConditionType.CS_SPECIAL[" + _reader.getSrcString() + "]");
            return null;
        }

        cond._m_eType = ENPPlayer_CS_SpecialCondition.valueOf(typeS.toUpperCase());

        return cond;
    }
}
