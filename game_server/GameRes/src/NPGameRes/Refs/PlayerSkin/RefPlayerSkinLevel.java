package NPGameRes.Refs.PlayerSkin;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPGameRes.GameObjs.CommonObj.LevelObj._ILevelBasicObj;
import NPGameRes.GameObjs.NPPlayerProperty.NPPlayerPropertyModifier;


@RefTable(tableName = "player_skin_level")
public class RefPlayerSkinLevel extends RefBase implements _ILevelBasicObj
{
    private static RefTableContainer<RefPlayerSkinLevel> _g_mgr = new RefTableContainer<RefPlayerSkinLevel>();

    public static RefTableContainer<RefPlayerSkinLevel> getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTableContainer<RefPlayerSkinLevel> getStaticContainer()
    {
        return getMgr();
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTableContainer<RefPlayerSkinLevel>) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefPlayerSkinLevel newRef = (RefPlayerSkinLevel) _newRef;
        id = newRef.id;
        player_skin_id = newRef.player_skin_id;
        skin_level = newRef.skin_level;
        upgrade_cost = newRef.upgrade_cost;
        player_property = newRef.player_property;
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
		return skin_level;
	}
	
    ////////////////////////

    public long id;
    public long player_skin_id;
    public int skin_level;
    public NPCommonCostItem upgrade_cost;
    public NPPlayerPropertyModifier player_property; //玩家属性加成
}
