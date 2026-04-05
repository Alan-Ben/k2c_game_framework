package NPGameRes.Refs.Dungeon.Midday;

import Common.DungeonEnum.EDungeonBoxType;
import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

import java.util.List;

@RefTable(tableName = "midday_dungeon_box")
public class RefMiddayDungeonBox extends RefBase
{
    private static RefMiddayDungeonBoxMgr _g_mgr = new RefMiddayDungeonBoxMgr();

    public static RefMiddayDungeonBoxMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefMiddayDungeonBoxMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefMiddayDungeonBoxMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefMiddayDungeonBox newRef = (RefMiddayDungeonBox) _newRef;
        id = newRef.id;
        box_type = newRef.box_type;
        drop_draw_item_list = newRef.drop_draw_item_list;
        item_list = newRef.item_list;
        can_draw_limit = newRef.can_draw_limit;
        duration_sec = newRef.duration_sec;
        draw_box_fixed_cd_id = newRef.draw_box_fixed_cd_id;
    }

    public static class RefMiddayDungeonBoxMgr extends RefTableContainer<RefMiddayDungeonBox>
    {
    }

    /**********
     * 获取对象数据Id，尽量唯一
     */
    @Override
    public long Id()
    {
        return id;
    }

    // ////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public long id; //唯一id
    public EDungeonBoxType box_type;//宝箱类型
    public List<NPCommonCostItem> drop_draw_item_list;//掉落时领取的奖励
    public List<NPCommonCostItem> item_list;//领取时的奖励
    public int can_draw_limit;//宝箱可被领取次数
    public int duration_sec;//宝箱有效期（秒）
    public long draw_box_fixed_cd_id;//领取宝箱次数上限（fixedCdId）
}