package NPGameRes.Refs.PrivilegeCard;

import Common.PrivilegeCardEnum.EPrivilegeCardType;
import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.GameObjs.NPPlayerProperty.NPPlayerPropertyModifier;
import NPGameRes.GameObjs.PlayerBonusProperty.PlayerBonusPropertyModifier;

import java.util.ArrayList;


@RefTable(tableName = "privilege_card")
public class RefPrivilegeCard extends RefBase
{
    private static RefTableContainer<RefPrivilegeCard> _g_mgr = new RefTableContainer<RefPrivilegeCard>();

    public static RefTableContainer<RefPrivilegeCard> getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTableContainer<RefPrivilegeCard> getStaticContainer()
    {
        return getMgr();
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTableContainer<RefPrivilegeCard>) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefPrivilegeCard newRef = (RefPrivilegeCard) _newRef;
        privilege_card_type = newRef.privilege_card_type;
        effect_days = newRef.effect_days;
        daily_gain_item_list = newRef.daily_gain_item_list;
        daily_gain_silver_mins = newRef.daily_gain_silver_mins;
        player_pro = newRef.player_pro;
        bonus_prop_modifier = newRef.bonus_prop_modifier;
        player_permission_list = newRef.player_permission_list;
        mail_id = newRef.mail_id;
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
        return privilege_card_type.ordinal();
    }
    ////////////////////////

    public EPrivilegeCardType privilege_card_type = EPrivilegeCardType.NONE;//权益卡类型
    public long effect_days;//有效天数
    public ArrayList<NPCommonCostItem> daily_gain_item_list = new ArrayList<>();//每日可以领取的物品列表
    public int daily_gain_silver_mins;//可以领取的金币时间（分钟）
    public NPPlayerPropertyModifier player_pro;//玩家属性
    public PlayerBonusPropertyModifier bonus_prop_modifier;//属性加成 PlayerBonusPropertyModifier类型
    public ArrayList<Long> player_permission_list = new ArrayList<>();//玩家权限列表
    public long mail_id;//邮件ID
}
