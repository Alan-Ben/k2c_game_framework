package NPGameRes.Refs.PlayerSkin;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefField;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.GameObjs.CommonObj.LevelObj._TLevelMapMgr;

import java.util.ArrayList;


@RefTable(tableName = "player_skin")
public class RefPlayerSkin extends RefBase
{
    private static RefTableContainer<RefPlayerSkin> _g_mgr = new RefTableContainer<RefPlayerSkin>();

    public static RefTableContainer<RefPlayerSkin> getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTableContainer<RefPlayerSkin> getStaticContainer()
    {
        return getMgr();
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTableContainer<RefPlayerSkin>) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefPlayerSkin newRef = (RefPlayerSkin) _newRef;
        id = newRef.id;
        unlock_item = newRef.unlock_item;
        unlock_gain_item_list = newRef.unlock_gain_item_list;
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
    ////////////////////////

    public long id;
    public NPCommonCostItem unlock_item;
    public ArrayList<NPCommonCostItem> unlock_gain_item_list = new ArrayList<>();

    /**
     * 对应子数据等级的处理
     */
    @RefField(isIgnore = true)
    private _TLevelMapMgr<RefPlayerSkinLevel> _m_lmLevelMapMgr = new _TLevelMapMgr<RefPlayerSkinLevel>();
    public _TLevelMapMgr<RefPlayerSkinLevel> getLevelMapMgr()
    {
        return _m_lmLevelMapMgr;
    }

    public void setLevelMapMgr(_TLevelMapMgr<RefPlayerSkinLevel> _mgr)
    {
        _m_lmLevelMapMgr = _mgr;
    }
}
