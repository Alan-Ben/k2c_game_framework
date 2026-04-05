package NPGameRes.Refs;


import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPEnum.ENPPlayerPropertyType;


/*********************
 * 通用cd表
 */
@RefTable(tableName = "player_lazy_cd")
public class RefPlayerLazyCd extends RefBase
{
    private static RefTableContainer<RefPlayerLazyCd> _g_mgr = new RefTableContainer<RefPlayerLazyCd>();

    public static RefTableContainer<RefPlayerLazyCd> getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTableContainer<RefPlayerLazyCd> getStaticContainer()
    {
        return _g_mgr;
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTableContainer<RefPlayerLazyCd>) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefPlayerLazyCd newRef = (RefPlayerLazyCd) _newRef;
        cd_id = newRef.cd_id;
        durantion = newRef.durantion;
        count_max = newRef.count_max;
        property_add_count_max = newRef.property_add_count_max;
        is_can_excceed_limit = newRef.is_can_excceed_limit;
        add_count_per_time = newRef.add_count_per_time;
        property_add_count_per_time = newRef.property_add_count_per_time;
        property_add_durantion = newRef.property_add_durantion;
        init_count = newRef.init_count;
        relative_activity_id = newRef.relative_activity_id;
    }

    @Override
    public long Id()
    {
        return cd_id;
    }

    public int cd_id;//CD类型ID
    public int durantion;//CD时长
    public int count_max; //计数最大值
    public ENPPlayerPropertyType property_add_count_max;//玩家数据增加的计数最大值修正
    public boolean is_can_excceed_limit = false;//附加值是否能超出上限
    public int add_count_per_time = 1; //每次固定恢复点数
    public ENPPlayerPropertyType property_add_count_per_time = ENPPlayerPropertyType.NONE;//玩家属性中每次恢复的点数加值.
    public ENPPlayerPropertyType property_add_durantion = ENPPlayerPropertyType.NONE;//玩家属性中对CD时长的加值.

    public int init_count;//玩家初始化cd值，如果大于0，则使用该值，否则就直接补满
    public long relative_activity_id; // 关联的活动ID，如果有的话

    /**
     * 计算初始值
     * @param _addMaxCount 附加的最大值
     * @return 初始值
     */
    public int calInitCount(long _addMaxCount)
    {
        return init_count > 0 ? init_count : (int) (count_max + _addMaxCount);
    }
}
