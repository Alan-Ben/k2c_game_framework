package NPGameRes.Refs.Player;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.GameObjs.NPPlayerProperty.NPPlayerPropertyModifier;

import java.util.List;

@RefTable(tableName = "vip")
public class RefVip extends RefBase
{
    private static RefTableContainer<RefVip> _g_mgr = new RefTableContainer<RefVip>();

    public static RefTableContainer<RefVip> getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTableContainer<RefVip> getStaticContainer()
    {
        return getMgr();
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTableContainer<RefVip>) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefVip newRef = (RefVip) _newRef;
        vip_lvl = newRef.vip_lvl;
        vip_exp = newRef.vip_exp;
        gain_item_list = newRef.gain_item_list;
        recharge_reward_list = newRef.recharge_reward_list;
        player_property = newRef.player_property;
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
        return vip_lvl;
    }

    public int vip_lvl; // 等级
    public long vip_exp; // 升到当级所需经验(不扣除)
    public List<NPCommonCostItem> gain_item_list; // 奖励
    public List<NPCommonCostItem> recharge_reward_list; // 充值奖励列表
    public NPPlayerPropertyModifier player_property = new NPPlayerPropertyModifier(); // VIP属性加成
}

