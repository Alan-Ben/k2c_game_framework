package NPGameRes.Refs.GiftPack;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

import java.util.List;

@RefTable(tableName = "gift_pack_group")
public class RefGiftPackGroup extends RefBase
{
    private static RefGiftPackGroupMgr _g_mgr = new RefGiftPackGroupMgr();

    public static RefGiftPackGroupMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefGiftPackGroupMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefGiftPackGroupMgr) _mgr;
    }

    public static class RefGiftPackGroupMgr extends RefTableContainer<RefGiftPackGroup>
    {
        @Override
        public void _onTableLoaded()
        {

        }
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefGiftPackGroup newRef = (RefGiftPackGroup) _newRef;
        id = newRef.id;
        gift_pack_id_list = newRef.gift_pack_id_list;
        show_condition = newRef.show_condition;
        show_type = newRef.show_type;
        activity_id = newRef.activity_id;
    }

    /**
     * 获取对象数据Id，尽量唯一
     */
    @Override
    public long Id()
    {
        return id;
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public long id; // 唯一id
    public List<Long> gift_pack_id_list; // 礼包列表（礼包id不能重复出现在不同group）
    public String show_condition; // 显示条件
    public String show_type; // 用于礼包界面分组展示的类型.(举例:活动Id,每日,每周,超值),当配置为活动Id以及超值时,activity_id字段有效
    public long activity_id; // 活动id
}