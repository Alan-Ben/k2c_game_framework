package ActivitiesV02.Refs.NumMerge;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefListContainer;

/**
 * 数字合并其他配置处理类
 *
 * 功能：
 * 1. 从配置表加载key-value对
 * 2. 通过反射填充到RefNumMergeOther单例对象
 * 3. 支持热更新
 */
@RefTable(tableName = "num_merge_other", isSingletonKey = false)
public class RefNumMergeOther_Process extends RefBase
{
    // 全局管理器实例
    private static RefNumMergeOther_ProcessMgr _g_mgr = new RefNumMergeOther_ProcessMgr();

    public static RefNumMergeOther_ProcessMgr getMgr()
    {
        return _g_mgr;
    }

    /**
     * 配置处理管理器
     */
    public static class RefNumMergeOther_ProcessMgr extends RefListContainer<RefNumMergeOther_Process>
    {
        /**
         * 热更新时重置配置值
         */
        @Override
        public boolean resetRef(RefNumMergeOther_Process _newRef)
        {
            for (RefNumMergeOther_Process refProcess : getList())
            {
                if (refProcess.key.equals(_newRef.key))
                {
                    refProcess.value = _newRef.value;
                    return true;
                }
            }
            return false;
        }

        /**
         * 配置加载完成后的处理
         *
         * 执行流程：
         * 1. 遍历所有配置项
         * 2. 调用RefNumMergeOther的internalSetValue设置字段值
         * 3. 调用NewAssert进行配置校验
         */
        @Override
        public void onLoaded()
        {
            if (isEmpty())
                return;

            for (RefNumMergeOther_Process ref : getList())
            {
                if (null == ref)
                    continue;

                RefNumMergeOther.getMgr().getRef().internalSetValue(ref.key, ref.value);
            }

            RefNumMergeOther.Ref().NewAssert();
        }
    }

    //////////////////////////////

    @Override
    public RefNumMergeOther_ProcessMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefNumMergeOther_ProcessMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefNumMergeOther_Process newRef = (RefNumMergeOther_Process) _newRef;
        key = newRef.key;
        value = newRef.value;
    }

    /**
     * 获取对象数据Id
     */
    @Override
    public long Id()
    {
        return 0;
    }

    //////////////////////////////
    // 配置键
    public String key;

    // 配置值
    public String value;
}
