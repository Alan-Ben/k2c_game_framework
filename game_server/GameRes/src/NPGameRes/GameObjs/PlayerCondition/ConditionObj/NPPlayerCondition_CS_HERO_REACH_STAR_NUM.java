package NPGameRes.GameObjs.PlayerCondition.ConditionObj;

import ALServerLog.ALServerLog;
import NPCommon.CommonObj.NPStringReader;
import NPEnum.ENPPlayerConditionType;
import NPGameRes.GameObjs.PlayerCondition._ANPBasicPlayerCondition;

/**
 * 达到指定星级大臣数量 CS_HERO_REACH_STAR_NUM:星级:min（:max）
 */
public class NPPlayerCondition_CS_HERO_REACH_STAR_NUM extends _ANPBasicPlayerCondition
{
    //技能ID
    private int _m_starNum;
    //最小数量
    private int _m_minNum;
    //最大数量 默认-1
    private int _m_maxNum;

    public NPPlayerCondition_CS_HERO_REACH_STAR_NUM()
    {
        _m_maxNum = -1;
    }

    public int getStarNum()
    {
        return _m_starNum;
    }

    public int getMinNum()
    {
        return _m_minNum;
    }

    public int getMaxNum()
    {
        return _m_maxNum;
    }

    @Override
    public ENPPlayerConditionType conditionType()
    {
        return ENPPlayerConditionType.CS_HERO_REACH_STAR_NUM;
    }

    public static NPPlayerCondition_CS_HERO_REACH_STAR_NUM readStr(NPStringReader _reader)
    {
        NPPlayerCondition_CS_HERO_REACH_STAR_NUM cond = new NPPlayerCondition_CS_HERO_REACH_STAR_NUM();

        String rawStarNum = _reader.readItem();
        String rawMinNum = _reader.readItem();

        if (null == rawStarNum || null == rawMinNum)
        {
            ALServerLog.Error("Can not read str for ENPPlayerConditionType.CS_HERO_REACH_STAR_NUM[" + _reader.getSrcString() + "]");
            return null;
        }

        cond._m_starNum = Integer.parseInt(rawStarNum);
        cond._m_minNum = Integer.parseInt(rawMinNum);

        //有配置最大数量
        String rawMaxNum = _reader.readItem();
        if (null != rawMaxNum)
            cond._m_maxNum = Integer.parseInt(rawMaxNum);

        return cond;
    }
}
