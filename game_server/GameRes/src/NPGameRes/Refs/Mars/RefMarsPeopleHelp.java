package NPGameRes.Refs.Mars;

import Common.MarsEnum.EMarsPeopleHelpType;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;

@RefTable(tableName = "mars_people_help")
public class RefMarsPeopleHelp extends RefBase
{
    private static RefMarsPeopleHelpMgr _g_mgr = new RefMarsPeopleHelpMgr();

    public static RefMarsPeopleHelpMgr getMgr()
    {
        return _g_mgr;
    }
    
    @Override
    public RefMarsPeopleHelpMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefMarsPeopleHelpMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefMarsPeopleHelp newRef = (RefMarsPeopleHelp) _newRef;
        id = newRef.id;
        type = newRef.type;
    }

    @Override
    public long Id()
    {
        return id;
    }

    public static class RefMarsPeopleHelpMgr extends RefTableContainer<RefMarsPeopleHelp>
    {
        // 配表加载完成后的处理逻辑
        @Override
        protected void _onTableLoaded()
        {
        }
    }

    public long id;
    public EMarsPeopleHelpType type = EMarsPeopleHelpType.NONE;//求助类型
}