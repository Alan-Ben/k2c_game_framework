package NPGameRes.GameObjs.PlayerCondition.ConditionObj;

import ALServerLog.ALServerLog;
import NPCommon.CommonObj.NPStringReader;
import NPEnum.ENPPlayerConditionType;
import NPEnum.ENPPlayer_CS_IdJudgeFunc;
import NPGameRes.GameObjs.PlayerCondition._ANPBasicPlayerCondition;

/**
 * 玩家计数 cs_record:ENPPlayerRecordParam:min_value:max_value
 * @author mark
 */
public class NPPlayerCondition_CS_ID_JUDGE extends _ANPBasicPlayerCondition
{
    private ENPPlayer_CS_IdJudgeFunc _m_eType;
    private long _m_lId;

    public ENPPlayer_CS_IdJudgeFunc type()
    {
        return _m_eType;
    }

    public long id()
    {
        return _m_lId;
    }

    @Override
    public ENPPlayerConditionType conditionType()
    {
        return ENPPlayerConditionType.CS_ID_JUDGE;
    }

    public static NPPlayerCondition_CS_ID_JUDGE readStr(NPStringReader _reader)
    {
        NPPlayerCondition_CS_ID_JUDGE cond = new NPPlayerCondition_CS_ID_JUDGE();

        String typeS = _reader.readItem();
        String idS = _reader.readItem();

        if (null == typeS || null == idS)
        {
            ALServerLog.Error("Can not read str for ENPPlayerConditionType.CS_ID_JUDGE[" + _reader.getSrcString() + "]");
            return null;
        }

        cond._m_eType = ENPPlayer_CS_IdJudgeFunc.valueOf(typeS.toUpperCase());
        cond._m_lId = Long.parseLong(idS);

        return cond;
    }
}
