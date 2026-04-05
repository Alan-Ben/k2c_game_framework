package NPGameRes.Refs;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefListContainer;

@RefTable(tableName = "general", isSingletonKey = false)
public class RefGeneral_Process extends RefBase
{
    private static RefGeneral_ProcessMgr _g_mgr = new RefGeneral_ProcessMgr();

    public static RefGeneral_ProcessMgr getMgr()
    {
        return _g_mgr;
    }

    public static class RefGeneral_ProcessMgr extends RefListContainer<RefGeneral_Process>
    {
        @Override
        public boolean resetRef(RefGeneral_Process _newRef)
        {
            for (RefGeneral_Process refGeneralProcess : getList())
            {
                if (refGeneralProcess.name.equals(_newRef.name))
                {
                    refGeneralProcess.value = _newRef.value;
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

            for (RefGeneral_Process ref : getList())
            {
                if (null == ref)
                    continue;

                RefGeneral.getMgr().getRef().internalSetValue(ref.name, ref.value);
            }

            RefGeneral.Ref().NewAssert();
        }
    }

    //////////////////////////////

    @Override
    public RefGeneral_ProcessMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefGeneral_ProcessMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefGeneral_Process newRef = (RefGeneral_Process) _newRef;
        name = newRef.name;
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
    public String name;
    public String value;
}
