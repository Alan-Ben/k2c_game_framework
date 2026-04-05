package NPGameRes.Refs;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.GameObjs.PlayerCondition.NPPlayerConditionGroupObj;
import NPGameRes.Refs.Parse.NPItemListParse;
import NPGameRes.Refs.Parse.NPPlayerEffectListParse;

/*********************
 * 段位信息表
 *
 * @author jeb
 *
 */
@RefTable(tableName = "remote_effect")
public class RefRemoteEffect extends RefBase
{
    private static RefTableContainer<RefRemoteEffect> _g_mgr = new RefTableContainer<RefRemoteEffect>();

    public static RefTableContainer<RefRemoteEffect> getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTableContainer<RefRemoteEffect> getStaticContainer()
    {
        return getMgr();
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTableContainer<RefRemoteEffect>) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefRemoteEffect newRef = (RefRemoteEffect) _newRef;
        id = newRef.id;
        condition = newRef.condition;
        effect_list = newRef.effect_list;
        cost_item_list = newRef.cost_item_list;
        ignore_notice_bonus = newRef.ignore_notice_bonus;
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

    public long id;//数据Id
    public NPPlayerConditionGroupObj condition;//条件列表
    public NPPlayerEffectListParse effect_list;//效果列表
    public NPItemListParse cost_item_list = new NPItemListParse();//消耗物品列表
    public boolean ignore_notice_bonus;//是否context的物品奖励信息不通知客户端

}
