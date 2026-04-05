package NPGameRes.GameObjs.PlayerCondition.ConditionObj;

import ALServerLog.ALServerLog;
import NPCommon.CommonObj.NPStringReader;
import NPEnum.ENPPlayerConditionType;
import NPGameRes.GameObjs.PlayerCondition._ANPBasicPlayerCondition;

/**
 * 情人技能解锁检查 CS_CONSORT_SKILL:情人ID:技能ID（:是否解锁 默认是已解锁）
 */
public class NPPlayerCondition_CS_CONSORT_SKILL extends _ANPBasicPlayerCondition
{
	//情人ID
    private long _m_lConsortId;
    //技能ID
    private long _m_lSkillId;
    //最小等级
    private int _m_iMinLvl;
    //最大等级 默认-1
    private int _m_iMaxLvl;
    
    public NPPlayerCondition_CS_CONSORT_SKILL()
    {
    	_m_iMaxLvl = -1;
    }
    
    public long getConsortId() {return _m_lConsortId;}
    public long getSkillId() {return _m_lSkillId;}
    public int getMinLvl() {return _m_iMinLvl;}
    public int getMaxLvl() {return _m_iMaxLvl;}

    @Override
    public ENPPlayerConditionType conditionType()
    {
        return ENPPlayerConditionType.CS_CONSORT_SKILL;
    }

    public static NPPlayerCondition_CS_CONSORT_SKILL readStr(NPStringReader _reader)
    {
        NPPlayerCondition_CS_CONSORT_SKILL cond = new NPPlayerCondition_CS_CONSORT_SKILL();

        String rawConsortIdS = _reader.readItem();
        String rawSkillIdS = _reader.readItem();
        String rawMinLvlS = _reader.readItem();
        
        if(null == rawConsortIdS || null == rawSkillIdS || null == rawMinLvlS)
        {
            ALServerLog.Error("Can not read str for ENPPlayerConditionType.CS_CONSORT_SKILL[" + _reader.getSrcString() + "]");
            return null;
        }
        
        cond._m_lConsortId = Long.parseLong(rawConsortIdS);
        cond._m_lSkillId = Long.parseLong(rawSkillIdS);
        cond._m_iMinLvl = Integer.parseInt(rawMinLvlS);

        //有配置“技能解锁条件”再处理
        String rawMaxLvlS = _reader.readItem();
        if(null != rawMaxLvlS)
        	cond._m_iMaxLvl = Integer.parseInt(rawMaxLvlS);
        
        return cond;
    }
}
