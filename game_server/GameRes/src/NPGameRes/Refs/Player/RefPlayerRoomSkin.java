package NPGameRes.Refs.Player;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPEnum.ENPTimeAddType;

/**
 * 房间皮肤配置表
 *
 * TODO: 此为临时占位类，需要从Excel配置表生成
 */
@RefTable(tableName = "player_room_skin")
public class RefPlayerRoomSkin extends _ARefExpiredItem
{
    private static RefTableContainer<RefPlayerRoomSkin> _g_mgr = new RefTableContainer<RefPlayerRoomSkin>();

    public static RefTableContainer<RefPlayerRoomSkin> getMgr()
    {
        return _g_mgr;
    }

    @Override
    public RefTableContainer<RefPlayerRoomSkin> getStaticContainer()
    {
        return getMgr();
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTableContainer<RefPlayerRoomSkin>) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefPlayerRoomSkin newRef = (RefPlayerRoomSkin) _newRef;
        id = newRef.id;
        add_type = newRef.add_type;
    }

    @Override
    public long Id() {
        return id;
    }

    /**
     * 房间皮肤ID
     */
    public long id;

    /**
     * 增加类型
     */
    public ENPTimeAddType add_type;

    @Override
    public ENPTimeAddType getAddType() {
        return add_type;
    }
}
