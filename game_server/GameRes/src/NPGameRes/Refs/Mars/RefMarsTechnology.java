package NPGameRes.Refs.Mars;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefField;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.GameObjs.CommonObj.LevelObj._TLevelMapMgr;

import java.util.ArrayList;

@RefTable(tableName = "mars_technology")
public class RefMarsTechnology extends RefBase
{
    private static RefMarsTechnologyMgr _g_mgr = new RefMarsTechnologyMgr();

    public static RefMarsTechnologyMgr getMgr()
    {
        return _g_mgr;
    }
    
    @Override
    public RefMarsTechnologyMgr getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefMarsTechnologyMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefMarsTechnology newRef = (RefMarsTechnology) _newRef;
        id = newRef.id;
        parent_list = newRef.parent_list;
    }

    @Override
    public long Id()
    {
        return id;
    }

    public static class RefMarsTechnologyMgr extends RefTableContainer<RefMarsTechnology>
    {
        // 配表加载完成后的处理逻辑
        @Override
        protected void _onTableLoaded()
        {
        }
    }

    public long id;//科技id
    public ArrayList<Long> parent_list = new ArrayList<>();//分布层级

    @RefField(isIgnore = true)
    private _TLevelMapMgr<RefMarsTechnologyLevel> _m_lmLevelMapMgr = new _TLevelMapMgr<RefMarsTechnologyLevel>();
    public void setLevelMapMgr(_TLevelMapMgr<RefMarsTechnologyLevel> _mgr) {_m_lmLevelMapMgr = _mgr;}
    public _TLevelMapMgr<RefMarsTechnologyLevel> getLevelMapMgr() {return _m_lmLevelMapMgr;}
}