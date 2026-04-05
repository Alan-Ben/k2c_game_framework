package NPGameRes.Refs.Player;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.GameObjs.Levy.LevySolderCritWeiObj;
import NPGameRes.GameObjs.NPPlayerProperty.NPPlayerPropertyModifier;
import NPGameRes.GameObjs.PlayerCondition.NPPlayerConditionGroupObj;

import java.util.ArrayList;

@RefTable(tableName = "player_lvl")
public class RefPlayerLevel extends RefBase
{
    private static RefTableContainer<RefPlayerLevel> _g_mgr = new RefTableContainer<RefPlayerLevel>();

    public static RefTableContainer<RefPlayerLevel> getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTableContainer<RefPlayerLevel> getStaticContainer()
    {
        return getMgr();
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTableContainer<RefPlayerLevel>) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefPlayerLevel newRef = (RefPlayerLevel) _newRef;
        lvl = newRef.lvl;
        exp = newRef.exp;
        earnings = newRef.earnings;
        current_lvl_consume = newRef.current_lvl_consume;
        reward_item_list = newRef.reward_item_list;
        lvl_up_cond = newRef.lvl_up_cond;
        player_property = newRef.player_property;
        levy_solder_crit_pro = newRef.levy_solder_crit_pro;
        levy_solder_crit_multiple_wei_list = newRef.levy_solder_crit_multiple_wei_list;
        child_educate_get_base_earnings = newRef.child_educate_get_base_earnings;
        child_graduate_get_earnings_add = newRef.child_graduate_get_earnings_add;
        child_educate_cost = newRef.child_educate_cost;
        child_educate_get_hero_exp = newRef.child_educate_get_hero_exp;
        daily_reward_item = newRef.daily_reward_item;
    }

    /**********
     * 获取对象数据Id，尽量唯一
     *
     * @author alzq.z
     * @time 2019年4月3日 下午11:35:24
     */
    @Override
    public long Id()
    {
        return lvl;
    }

    public int lvl; // 等级
    public long exp; // 升到当级所需经验(不扣除)
    public long earnings;//升级到当前等级所需要的赚速（不扣除）
    public ArrayList<NPCommonCostItem> current_lvl_consume; // 升级到当前等级额外消耗
    public ArrayList<NPCommonCostItem> reward_item_list; // 升级奖励
    public NPPlayerConditionGroupObj lvl_up_cond;//升级条件
    public NPPlayerPropertyModifier player_property;  //该等级的玩家加成属性
    
    //征收字段
    public int levy_solder_crit_pro;//士兵征收暴击概率
    public LevySolderCritWeiObj levy_solder_crit_multiple_wei_list = new LevySolderCritWeiObj();//士兵征收暴击倍数权重列表
    
    //子嗣相关字段
    public long child_educate_get_base_earnings;//子嗣上课基础村庄收益
    public int child_graduate_get_earnings_add;//子嗣毕业收益加成万分比
    public long child_educate_cost;//子嗣上课金币消耗
    public long child_educate_get_hero_exp;//子嗣上课获得伙伴经验

    //其他字段
    public NPCommonCostItem daily_reward_item;//每日奖励
}

