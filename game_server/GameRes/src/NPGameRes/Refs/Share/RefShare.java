package NPGameRes.Refs.Share;

import NPCommon.Enum.ERefType;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefField;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPEnum.ENPChatRoomType;
import NPEnum.ENPShareItemType;

import java.util.ArrayList;

/**
 * @author mark 主城表
 */
@RefTable(tableName = "share")
public class RefShare extends RefBase
{
    private static RefTableContainer<RefShare> _g_mgr = new RefTableContainer<RefShare>();

    public static RefTableContainer<RefShare> getMgr()
    {
        return _g_mgr;
    }

    public static ERefType getRefTypeStatic()
    {
        return ERefType.share;
    }

    @Override
    public RefTableContainer<RefShare> getStaticContainer()
    {
        return getMgr();
    }

    @SuppressWarnings("unchecked")
    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefTableContainer<RefShare>) _mgr;
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefShare newRef = (RefShare) _newRef;
        id = newRef.id;
        chat_room_list = newRef.chat_room_list;
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

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public long id;
    public ArrayList<ENPChatRoomType> chat_room_list = new ArrayList<>();

    @RefField(isIgnore = true)
    public ENPShareItemType share_item_type = ENPShareItemType.NONE;
}
