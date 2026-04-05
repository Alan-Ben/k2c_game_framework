package NPGameRes.GameObjs.GuildDungeon;

import Common.GuildDungeonEnum.EGuildDungeon_MonsterType;
import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.Log.CommLog;
import NPCommon.RefData._IParseFromStringable;
import NPCommon.Util.CommonFunc;

public class GuildDungeonMonsterKillReward implements _IParseFromStringable
{
    private EGuildDungeon_MonsterType _m_eType;
    private int _m_value;
    private NPCommonCostItem _m_gainItem;

    public GuildDungeonMonsterKillReward()
    {
    	_m_eType = EGuildDungeon_MonsterType.NONE;
        _m_value = 0;
        _m_gainItem = new NPCommonCostItem();
    }

    public EGuildDungeon_MonsterType getType()
    {
        return _m_eType;
    }

    public int getValue()
    {
        return _m_value;
    }
    
    public NPCommonCostItem getGainItem()
    {
    	return _m_gainItem;
    }

    @Override
    public boolean parseFromString(String sValue)
    {
        return parseFromString(sValue, ':');
    }

    public boolean parseFromString(String sValue, char token)
    {
        if (null == sValue || sValue.trim().isEmpty())
            return true;

        try
        {
            String[] strs = CommonFunc.charSplit(sValue, token);

            this._m_eType = EGuildDungeon_MonsterType.valueOf(strs[0].toUpperCase());
            this._m_value = Integer.parseInt(strs[1]);
            this._m_gainItem.parseFromString(strs[2]);
        } 
        catch (Exception e)
        {
            CommLog.error("GuildDungeonMonsterKillReward parse failed,str =" + sValue, e);
            return false;
        }

        return true;
    }
    
    public static GuildDungeonMonsterKillReward fromString(String sValue, char token)
    {
    	GuildDungeonMonsterKillReward pair = new GuildDungeonMonsterKillReward();
        if (!pair.parseFromString(sValue, token))
        {
            return null;
        }
        
        return pair;
    }
}
