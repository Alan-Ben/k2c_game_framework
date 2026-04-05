package NPGameRes.Refs.PlayerBuff;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.GameObjs.NPPlayerProperty.NPPlayerPropertyModifier;
import NPGameRes.GameObjs.PlayerMarsProperty.PlayerMarsPropertyModifier;
import NPGameRes.GameObjs.RefUnionBonus.UnionBonus;
import NPGameRes.Refs.Parse.NPPlayerEffectListParse;


@RefTable(tableName = "player_buff")
public class RefPlayerBuff extends RefBase
{
    private static RefTableContainer<RefPlayerBuff> _g_mgr = new RefTableContainer<RefPlayerBuff>();

    public static RefTableContainer<RefPlayerBuff> getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTableContainer<RefPlayerBuff> getStaticContainer()
    {
        return getMgr();
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTableContainer<RefPlayerBuff>) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefPlayerBuff newRef = (RefPlayerBuff) _newRef;
        id = newRef.id;
        max_layer = newRef.max_layer;
        layer_empty_remove = newRef.layer_empty_remove;
        can_add_layer = newRef.can_add_layer;
        bonus_add = newRef.bonus_add;
        player_pro_add = newRef.player_pro_add;
        mars_pro_add = newRef.mars_pro_add;
        on_init_effect = newRef.on_init_effect;
        on_remove_effect = newRef.on_remove_effect;
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
        return id;
    }
    ////////////////////////

    public long id;
    public int max_layer; //最大层数
    public boolean layer_empty_remove; //层数为0是否移除
    public boolean can_add_layer; //是否可以增加层数
    public UnionBonus bonus_add; //buff的bonus加成
    public NPPlayerPropertyModifier player_pro_add; //buff的玩家属性
    public PlayerMarsPropertyModifier mars_pro_add; //buff的火星属性
    public NPPlayerEffectListParse on_init_effect; //buff初始时的效果
    public NPPlayerEffectListParse on_remove_effect; //buff移除时的效果
}
