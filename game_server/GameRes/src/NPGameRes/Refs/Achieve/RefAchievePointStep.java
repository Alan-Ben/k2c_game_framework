package NPGameRes.Refs.Achieve;

import CommonEnum.EAchieveType;
import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

import java.util.ArrayList;

/**
 * @author mark 通用成就配置
 */
@RefTable(tableName = "achieve_point_step")
public class RefAchievePointStep extends RefBase
{
    private static RefTableContainer<RefAchievePointStep> _g_mgr = new RefTableContainer<RefAchievePointStep>();

    public static RefTableContainer<RefAchievePointStep> getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTableContainer<RefAchievePointStep> getStaticContainer()
    {
        return getMgr();
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTableContainer<RefAchievePointStep>) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefAchievePointStep newRef = (RefAchievePointStep) _newRef;
        id = newRef.id;
        step_id = newRef.step_id;
        type = newRef.type;
        need_point = newRef.need_point;
        reward_item_list = newRef.reward_item_list;
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
    // ////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public long id;
    public int step_id;//阶段id
    public EAchieveType type;//成就类型(EAchieveType)
    public NPCommonCostItem need_point;//奖励物品列表
    public ArrayList<NPCommonCostItem> reward_item_list;//奖励物品列表

    // //////////////////////////////////////////////////////////////////////////////////////////////////////////////
}
