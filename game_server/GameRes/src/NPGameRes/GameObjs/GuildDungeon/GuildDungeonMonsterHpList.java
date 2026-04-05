package NPGameRes.GameObjs.GuildDungeon;

import Common.GuildDungeonEnum.EGuildDungeon_MonsterType;
import NPCommon.Log.CommLog;
import NPCommon.RefData._IParseFromStringable;
import NPCommon.Util.CommonFunc;

import java.util.ArrayList;

public class GuildDungeonMonsterHpList implements _IParseFromStringable
{
    private ArrayList<GuildDungeonMonsterHp> _m_list = new ArrayList<>();

    @Override
    public boolean parseFromString(String sValue)
    {
        if (null == sValue || sValue.trim().isEmpty())
            return true;
        
        _m_list.clear();
        
        try
        {
            String[] strs = CommonFunc.charSplit(sValue, new char[]{';'}, true);
            for (String str : strs)
            {
                if (str == null || str.trim().isEmpty())
                    continue;
                
                GuildDungeonMonsterHp pair = GuildDungeonMonsterHp.fromString(str, ':');
                if (null != pair)
                {
                    _m_list.add(pair);
                }
            }
        } 
        catch (Exception e)
        {
            CommLog.error("GuildDungeonMonsterHp parse failed, str =" + sValue, e);
            return false;
        }

        return true;
    }

    public static GuildDungeonMonsterHpList fromString(String sValue)
    {
        GuildDungeonMonsterHpList list = new GuildDungeonMonsterHpList();
        if (!list.parseFromString(sValue))
        {
            return null;
        }
        return list;
    }

    @Override
    public String toString()
    {
        return CommonFunc.list2String(_m_list);
    }

    public void clear()
    {
        _m_list.clear();
    }
    
    public long getValue(EGuildDungeon_MonsterType _type)
    {
    	for(int i = 0; i < _m_list.size(); i++)
    	{
    		GuildDungeonMonsterHp obj = _m_list.get(i);
    		if(null == obj)
    			continue;
    	
    		if(obj.getType() == _type)
    			return obj.getValue();
    	}
    	
    	return 0;
    }
}