package NPGameRes.Refs.Consort;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.GameObjs.CommonObj.LevelObj._ILevelBasicObj;

/**
 * @author mark
 */
@RefTable(tableName = "consort_skin_lvl")
public class RefConsortSkinLvl extends RefBase implements _ILevelBasicObj
{
    private static RefConsortSkinLvlMgr _g_mgr = new RefConsortSkinLvlMgr();
    public static RefConsortSkinLvlMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTableContainer<RefConsortSkinLvl> getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefConsortSkinLvlMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefConsortSkinLvl newRef = (RefConsortSkinLvl) _newRef;
        id = newRef.id;
        consort_skin_id = newRef.consort_skin_id;
        lvl = newRef.lvl;
    }
    
    public static class RefConsortSkinLvlMgr extends RefTableContainer<RefConsortSkinLvl>
    {
    	@Override
        public void _onTableLoaded()
        {
        }
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
    
    @Override
    public int getLevel()
    {
    	return lvl;
    }

    // ////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public long id;
    public long consort_skin_id;
    public int lvl;
    
}
