package ActivitiesV01.Refs.TileMatch;

import Hotfix.V01.Enum.TileMatchEnum.ETileMatch_ModeType;
import NPCommon.Game.WeightLongValueList;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.GameObjs.PlayerCondition.NPPlayerConditionGroupObj;

@RefTable(tableName = "tilematch_mode")
public class RefTileMatchMode extends RefBase
{
    private static RefTileMatchModeMgr _g_mgr = new RefTileMatchModeMgr();

    public static RefTileMatchModeMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTileMatchModeMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTileMatchModeMgr) _mgr;
    }

    public static class RefTileMatchModeMgr extends RefTableContainer<RefTileMatchMode>
    {
        @Override
        public void _onTableLoaded()
        {

        }
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefTileMatchMode newRef = (RefTileMatchMode) _newRef;
        type = newRef.type;
        unlock_need_score = newRef.unlock_need_score;
        unlock_condition = newRef.unlock_condition;
        multiple = newRef.multiple;
        task_rand_list = newRef.task_rand_list;
    }

    /**
     * 获取对象数据Id，尽量唯一
     */
    @Override
    public long Id()
    {
        return type.ordinal();
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public ETileMatch_ModeType type;
    public long unlock_need_score;//解锁所需分数
    public NPPlayerConditionGroupObj unlock_condition = new NPPlayerConditionGroupObj();
    public int multiple;
    public WeightLongValueList task_rand_list = new WeightLongValueList();
}