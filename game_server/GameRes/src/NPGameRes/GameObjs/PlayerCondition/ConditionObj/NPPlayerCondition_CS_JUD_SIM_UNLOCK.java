package NPGameRes.GameObjs.PlayerCondition.ConditionObj;

import ALServerLog.ALServerLog;
import NPCommon.CommonObj.NPStringReader;
import NPEnum.ENPPlayerConditionType;
import NPGameRes.GameObjs.PlayerCondition._ANPBasicPlayerCondition;

public class NPPlayerCondition_CS_JUD_SIM_UNLOCK extends _ANPBasicPlayerCondition
{
    public ENPPlayerConditionType conditionType()
    {
        return ENPPlayerConditionType.CS_JUD_SIM_UNLOCK;
    }

    private long _m_lSimUnlockId;

    public long simUnlockId()
    {
        return _m_lSimUnlockId;
    }

    public static NPPlayerCondition_CS_JUD_SIM_UNLOCK readStr(NPStringReader _reader)
    {
        NPPlayerCondition_CS_JUD_SIM_UNLOCK cond = new NPPlayerCondition_CS_JUD_SIM_UNLOCK();

        String unlockIdS = _reader.readItem(':');
        if (null == unlockIdS)
        {
            ALServerLog.Error("Can not read str for ENPPlayerConditionType.CS_JUD_SIM_UNLOCK[" + _reader.getSrcString() + "]");
            return null;
        }

        cond._m_lSimUnlockId = Long.parseLong(unlockIdS);

        return cond;
    }

}