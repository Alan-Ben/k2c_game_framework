package NPGameRes.Refs;


import NPCommon.CommonObj.NPRefreshTimeObj;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPEnum.ENPPlayerPropertyType;


/*********************
 * 通用cd表
 */
@RefTable(tableName = "player_fixed_cd")
public class RefPlayerFixedCd extends RefBase
{
    private static RefTableContainer<RefPlayerFixedCd> _g_mgr = new RefTableContainer<RefPlayerFixedCd>();

    public static RefTableContainer<RefPlayerFixedCd> getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTableContainer<RefPlayerFixedCd> getStaticContainer()
    {
        return _g_mgr;
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTableContainer<RefPlayerFixedCd>) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefPlayerFixedCd newRef = (RefPlayerFixedCd) _newRef;
        id = newRef.id;
        refresh_rule = newRef.refresh_rule;
        count_max = newRef.count_max;
        property_add_count_max = newRef.property_add_count_max;
        is_can_excceed_limit = newRef.is_can_excceed_limit;
        add_count_per_time = newRef.add_count_per_time;
        property_add_count_per_time = newRef.property_add_count_per_time;
    }

    @Override
    public long Id()
    {
        return id;
    }

    public int id;//CD类型ID
    public NPRefreshTimeObj refresh_rule;//刷新规则
    public int count_max; //计数最大值
    public ENPPlayerPropertyType property_add_count_max;//玩家数据增加的计数最大值修正
    public boolean is_can_excceed_limit = false;//是否能超出上限
    public int add_count_per_time = 1; //每次固定恢复点数
    public ENPPlayerPropertyType property_add_count_per_time = ENPPlayerPropertyType.NONE;//玩家属性中每次恢复的点数加值.
}
