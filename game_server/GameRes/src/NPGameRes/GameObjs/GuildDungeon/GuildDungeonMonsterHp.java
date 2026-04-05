package NPGameRes.GameObjs.GuildDungeon;

import Common.GuildDungeonEnum.EGuildDungeon_MonsterType;
import NPCommon.Log.CommLog;
import NPCommon.RefData._IParseFromStringable;
import NPCommon.Util.CommonFunc;

public class GuildDungeonMonsterHp implements _IParseFromStringable
{
    private EGuildDungeon_MonsterType _m_eType;
    private long _m_value;

    public GuildDungeonMonsterHp()
    {
    	_m_eType = EGuildDungeon_MonsterType.NONE;
        _m_value = 0;
    }

    public EGuildDungeon_MonsterType getType()
    {
        return _m_eType;
    }

    public long getValue()
    {
        return _m_value;
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
            String[] strs = CommonFunc.charSplit(sValue, token, 2);

            this._m_eType = EGuildDungeon_MonsterType.valueOf(strs[0].toUpperCase());
            this._m_value = Long.parseLong(strs[1]);
        } 
        catch (Exception e)
        {
            CommLog.error("GuildDungeonMonsterHp parse failed,str =" + sValue, e);
            return false;
        }

        return true;
    }
    
    public static GuildDungeonMonsterHp fromString(String sValue, char token)
    {
    	GuildDungeonMonsterHp pair = new GuildDungeonMonsterHp();
        if (!pair.parseFromString(sValue, token))
        {
            return null;
        }
        
        return pair;
    }
}
