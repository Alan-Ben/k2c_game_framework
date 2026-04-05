package ActivitiesV01.Refs.TileMatch;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefListContainer;

@RefTable(tableName = "tilematch_other", isSingletonKey = false)
public class RefTileMatchOther_Process extends RefBase
{
    private static RefTileMatchOther_ProcessMgr _g_mgr = new RefTileMatchOther_ProcessMgr();

    public static RefTileMatchOther_ProcessMgr getMgr()
    {
        return _g_mgr;
    }

    public static class RefTileMatchOther_ProcessMgr extends RefListContainer<RefTileMatchOther_Process>
    {
        @Override
        public boolean resetRef(RefTileMatchOther_Process _newRef)
        {
            for (RefTileMatchOther_Process refProcess : getList())
            {
                if (refProcess.key.equals(_newRef.key))
                {
                    refProcess.value = _newRef.value;
                    return true;
                }
            }
            return false;
        }

        @Override
        public void onLoaded()
        {
            if (isEmpty())
                return;

            for (RefTileMatchOther_Process ref : getList())
            {
                if (null == ref)
                    continue;

                RefTileMatchOther.getMgr().getRef().internalSetValue(ref.key, ref.value);
            }

            RefTileMatchOther.Ref().NewAssert();
        }
    }

    //////////////////////////////

    @Override
    public RefTileMatchOther_ProcessMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTileMatchOther_ProcessMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefTileMatchOther_Process newRef = (RefTileMatchOther_Process) _newRef;
        key = newRef.key;
        value = newRef.value;
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
        return 0;
    }

    //////////////////////////////
    public String key;
    public String value;
}
