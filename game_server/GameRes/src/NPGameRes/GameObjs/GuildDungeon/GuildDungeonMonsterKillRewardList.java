package NPGameRes.GameObjs.GuildDungeon;

import Common.GuildDungeonEnum.EGuildDungeon_MonsterType;
import NPCommon.Log.CommLog;
import NPCommon.RefData._IParseFromStringable;
import NPCommon.Util.CommonFunc;

import java.util.ArrayList;

public class GuildDungeonMonsterKillRewardList implements _IParseFromStringable
{
    private ArrayList<GuildDungeonMonsterKillReward> _m_list = new ArrayList<>();
    
    public ArrayList<GuildDungeonMonsterKillReward> getKillRewardList() {return _m_list;}

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
                
                GuildDungeonMonsterKillReward pair = GuildDungeonMonsterKillReward.fromString(str, '|');
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

    @Override
    public String toString()
    {
        return CommonFunc.list2String(_m_list);
    }

    public void clear()
    {
        _m_list.clear();
    }
    
    /**
     * 查找指定怪物类型的奖励
     * @param _monsterType
     * @return
     */
    public GuildDungeonMonsterKillReward lookReward(EGuildDungeon_MonsterType _monsterType)
    {
    	ArrayList<GuildDungeonMonsterKillReward> list = _m_list;
    	
    	for(int i = 0; i < list.size(); i++)
    	{
    		GuildDungeonMonsterKillReward reward = list.get(i);
    		if(null == reward)
    			continue;
    		
    		if(reward.getType() == _monsterType)
    			return reward;
    	}
    	
    	return null;
    }
}
