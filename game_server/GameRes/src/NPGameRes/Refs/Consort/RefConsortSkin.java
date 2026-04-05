package NPGameRes.Refs.Consort;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefField;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.GameObjs.CommonObj.LevelObj._TLevelMapMgr;

import java.util.ArrayList;

/**
 * @author mark
 */
@RefTable(tableName = "consort_skin")
public class RefConsortSkin extends RefBase
{
    private static RefConsortSkinMgr _g_mgr = new RefConsortSkinMgr();
    public static RefConsortSkinMgr getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTableContainer<RefConsortSkin> getStaticContainer()
    {
        return getMgr();
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefConsortSkinMgr) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefConsortSkin newRef = (RefConsortSkin) _newRef;
        id = newRef.id;
        consort_id = newRef.consort_id;
        unlock_cost_item = newRef.unlock_cost_item;
        unlock_gain_item_list = newRef.unlock_gain_item_list;
    }
    
    public static class RefConsortSkinMgr extends RefTableContainer<RefConsortSkin>
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

    // ////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public long id;
    public long consort_id;
    public NPCommonCostItem unlock_cost_item = new NPCommonCostItem();//解锁消耗
    public ArrayList<NPCommonCostItem> unlock_gain_item_list = new ArrayList<>();//解锁时获得的道具列表

    /**
     * 对应子数据等级的处理
     */
    @RefField(isIgnore = true)
    private _TLevelMapMgr<RefConsortSkinLvl> _m_lmLevelMapMgr = new _TLevelMapMgr<RefConsortSkinLvl>();
    public void setLevelMapMgr(_TLevelMapMgr<RefConsortSkinLvl> _mgr) {_m_lmLevelMapMgr = _mgr;}
    public _TLevelMapMgr<RefConsortSkinLvl> getLevelMapMgr() {return _m_lmLevelMapMgr;}
}
