package NPGameRes.Refs.Mars;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.Game.WeightQualityValueList;
import NPCommon.Game.WeightValueList;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefField;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.Pair.WCGPairIntList;
import NPGameRes.GameObjs.CommonObj.CommonCostItemListGroup;
import NPGameRes.GameObjs.Mars.MarsBattleQuality;
import NPGameRes.GameObjs.NPPlayerProperty.NPPlayerPropertyModifier;

import java.util.ArrayList;
import java.util.HashSet;

@RefTable(tableName = "mars_explore_lvl")
public class RefMarsExploreLvl extends RefBase
{
    private static RefMarsExploreLvlMgr _g_mgr = new RefMarsExploreLvlMgr();

    public static RefMarsExploreLvlMgr getMgr()
    {
        return _g_mgr;
    }
    
    @Override
    public RefMarsExploreLvlMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefMarsExploreLvlMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefMarsExploreLvl newRef = (RefMarsExploreLvl) _newRef;
        explore_level = newRef.explore_level;
        player_property = newRef.player_property;
        upgrade_need_explore_num = newRef.upgrade_need_explore_num;
        explore_event_exist_limit = newRef.explore_event_exist_limit;
        upgrade_gain_item_list = newRef.upgrade_gain_item_list;
        pos_list = newRef.pos_list;
        refresh_event_quality_list = newRef.refresh_event_quality_list;
        battle_event_quality_solider_power_list = newRef.battle_event_quality_solider_power_list;
        battle_event_quality_solider_num_list = newRef.battle_event_quality_solider_num_list;
        battle_event_quality_reward_list = newRef.battle_event_quality_reward_list;
        mine_pos_list = newRef.mine_pos_list;
        refresh_mine_list = newRef.refresh_mine_list;
        new_mine_per = newRef.new_mine_per;
    }

    @Override
    public long Id()
    {
        return explore_level;
    }

    public static class RefMarsExploreLvlMgr extends RefTableContainer<RefMarsExploreLvl>
    {
        // 配表加载完成后的处理逻辑
        @Override
        protected void _onTableLoaded()
        {
        }
    }

    public int explore_level;//探索等级
    public NPPlayerPropertyModifier player_property;//玩家属性
    public int upgrade_need_explore_num;//升到下一级处理的探索次数
    public int explore_event_exist_limit;//存在情报上限
    public ArrayList<NPCommonCostItem> upgrade_gain_item_list = new ArrayList<>(); //升级奖励
    public ArrayList<Long> pos_list = new ArrayList<>();//位置列表
    public WeightQualityValueList refresh_event_quality_list = new WeightQualityValueList();//当前等级刷新事件品质列表
    public ArrayList<Long> battle_event_quality_solider_power_list = new ArrayList<>();//当前等级战斗事件品质对应战力列表
    public ArrayList<Long> battle_event_quality_solider_num_list = new ArrayList<>();//当前等级战斗事件品质对应战力列表
    public CommonCostItemListGroup battle_event_quality_reward_list = new CommonCostItemListGroup();//当前等级战斗事件品质对应奖励列表
    public ArrayList<Long> mine_pos_list = new ArrayList<>();//矿场位置列表
    public WCGPairIntList refresh_mine_list = new WCGPairIntList();
    public ArrayList<Integer> new_mine_per = new ArrayList<>();
    
    @RefField(isIgnore = true)
    public ArrayList<RefMarsExplorePos> posRefList = new ArrayList<>();
    
    @RefField(isIgnore = true)
    public ArrayList<RefMarsExplorePos> minePosRefList = new ArrayList<>();
    
    @RefField(isIgnore = true)
    public WeightValueList<MarsExploreMineRndInfo> mineIdList = new WeightValueList<MarsExploreMineRndInfo>();
    
    @RefField(isIgnore = true)
    public ArrayList<MarsBattleQuality> battleQualityList = new ArrayList<>();

    public static class MarsExploreMineRndInfo
    {
        //矿等级
        public int mineLvl;
        //随机新矿概率
        public int newPer;
    }
    
    /**
     * 获取可以放置随机事件的位置
     * @param _usedPosList
     * @return
     */
    public RefMarsExplorePos randPosRef(HashSet<Long> _usedPosList)
    {
    	ArrayList<RefMarsExplorePos> list = this.posRefList;
    	if(list.isEmpty())
    		return null;
    	
    	ArrayList<RefMarsExplorePos> canExplorePosList = new ArrayList<>();
    	for(int i = 0; i < list.size(); i++)
    	{
    		RefMarsExplorePos posRef = list.get(i);
    		if(null == posRef)
    			continue;
    		
    		if(_usedPosList.contains(posRef.id))
    			continue;
    		
    		canExplorePosList.add(posRef);
    	}

    	//GOB-6344【优化-0】火星探索-刷新出新事件表现完善 https://www.teambition.com/task/690c1f8c558e2db2d8dbf867
    	//如果没有可用的pos，则对全部的pos进行随机，允许覆盖
    	if(canExplorePosList.isEmpty())
    	{
    		int idx = CommonFunc.randomInt(list.size() - 1);
    		return list.get(idx);
    	}
    	
    	int idx = CommonFunc.randomInt(canExplorePosList.size() - 1);
    	return canExplorePosList.get(idx);
    }
    
    /**
     * 获取可以放置随机矿产的位置
     * @param _usedPosList
     * @return
     */
    public RefMarsExplorePos randExploreMinePos(HashSet<Long> _usedPosList)
    {
    	ArrayList<RefMarsExplorePos> list = this.minePosRefList;
    	if(list.isEmpty())
    		return null;
    	
    	ArrayList<RefMarsExplorePos> canExplorePosList = new ArrayList<>();
    	for(int i = 0; i < list.size(); i++)
    	{
    		RefMarsExplorePos posRef = list.get(i);
    		if(null == posRef)
    			continue;
    		
    		if(null != _usedPosList && _usedPosList.contains(posRef.id))
    			continue;
    		
    		canExplorePosList.add(posRef);
    	}
    	
    	if(canExplorePosList.isEmpty())
    	{
    		int idx = CommonFunc.randomInt(list.size() - 1);
    		return list.get(idx);
    	}
    	
    	int idx = CommonFunc.randomInt(canExplorePosList.size() - 1);
    	return canExplorePosList.get(idx);
    }
    
    /**
     * 获取战斗事件需要的实力
     * @param _quality
     * @return
     */
    public MarsBattleQuality getQualityBattle(int _quality)
    {
        ArrayList<MarsBattleQuality> list = this.battleQualityList;
        for(int i = 0; i < list.size(); i++)
        {
            MarsBattleQuality obj = list.get(i);
            if(null != obj.getQuality() && obj.getQuality().ordinal() == _quality)
                return obj;
        }

        return null;
    }
    public long getSoliderPower(int _quality)
    {
        ArrayList<MarsBattleQuality> list = this.battleQualityList;
        for(int i = 0; i < list.size(); i++)
        {
            MarsBattleQuality obj = list.get(i);
            if(null != obj.getQuality() && obj.getQuality().ordinal() == _quality)
                return obj.getSoliderPower();
        }

        return 0;
    }
    public long getSoliderNum(int _quality)
    {
        ArrayList<MarsBattleQuality> list = this.battleQualityList;
        for(int i = 0; i < list.size(); i++)
        {
            MarsBattleQuality obj = list.get(i);
            if(null != obj.getQuality() && obj.getQuality().ordinal() == _quality)
                return obj.getSoliderNum();
        }

        return 0;
    }
    
    /**
     * 获取对应品质的奖励列表
     * @param _quality
     * @return
     */
    public ArrayList<NPCommonCostItem> getQualityRewardItemList(int _quality)
    {
    	ArrayList<MarsBattleQuality> list = this.battleQualityList;
    	for(int i = 0; i < list.size(); i++)
    	{
    		MarsBattleQuality obj = list.get(i);
    		if(null != obj.getQuality() && obj.getQuality().ordinal() == _quality)
    			return obj.getItemList();
    	}
    	
    	return null;
    }
}