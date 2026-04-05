package NPGameRes.Refs.TreasureHunt;

import NPCommon.CommonObj.NPCommonItem;
import NPCommon.CommonObj.NPRefreshTimeObj;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

/**
 * 奇物产出配表 - 配置每个奇物的产出基础值、产出道具和刷新时间
 * 
 * 主要功能：
 * 1. 定义奇物的基础产出数值
 * 2. 配置奇物产出的具体道具信息
 * 3. 设置奇物产出的刷新时间间隔
 * 
 * 表结构：treasure_id | basic_value | output_item | refresh_time
 * 
 * @author mark
 */
@RefTable(tableName = "treasure_hunt_treasure_output")
public class RefTreasureHuntTreasureOutput extends RefBase
{
    private static RefTreasureHuntTreasureOutputMgr _g_mgr = new RefTreasureHuntTreasureOutputMgr();

    public static RefTreasureHuntTreasureOutputMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTreasureHuntTreasureOutputMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTreasureHuntTreasureOutputMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefTreasureHuntTreasureOutput newRef = (RefTreasureHuntTreasureOutput) _newRef;
        treasure_id = newRef.treasure_id;
        basic_value = newRef.basic_value;
        output_item = newRef.output_item;
        refresh_time = newRef.refresh_time;
    }

    public static class RefTreasureHuntTreasureOutputMgr extends RefTableContainer<RefTreasureHuntTreasureOutput>
    {
        @Override
        protected void _onTableLoaded()
        {
            // 表加载完成后的初始化逻辑
        }
    }

    /**
     * 获取对象数据Id，尽量唯一
     * 
     * @return 奇物ID作为唯一标识
     */
    @Override
    public long Id()
    {
        return treasure_id;
    }

    // ////////////////////////////////////////////////////////////////////////////////////////////////////////////

    // 奇物ID，关联RefTreasureHuntTreasure表的id字段
    public long treasure_id;
    public long basic_value;// 基础产出数值
    public NPCommonItem output_item;// 产出道具
    public NPRefreshTimeObj refresh_time = new NPRefreshTimeObj();// 产出刷新时间
}