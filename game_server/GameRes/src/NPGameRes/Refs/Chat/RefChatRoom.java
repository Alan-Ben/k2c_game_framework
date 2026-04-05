package NPGameRes.Refs.Chat;

import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.Ref.RefTable;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPCommon.RefData.RefContainer.RefTableContainer;
import NPEnum.ENPChatRoomType;
import NPGameRes.GameObjs.PlayerCondition.NPPlayerConditionGroupObj;

@RefTable(tableName = "chat_room")
public class RefChatRoom extends RefBase
{
    private static RefChatRoomMgr _g_mgr = new RefChatRoomMgr();
    public static RefChatRoomMgr getMgr() {return _g_mgr;}

    @Override
    public RefChatRoomMgr getStaticContainer()
    {
        return _g_mgr;
    }

    @Override
    public void setStaticContainer(RefContainerBase<? extends RefBase> _mgr)
    {
        _g_mgr = (RefChatRoomMgr) _mgr;
    }

    public static class RefChatRoomMgr extends RefTableContainer<RefChatRoom>
    {
    }

    @Override
    public void resetRef(RefBase _newRef)
    {
        RefChatRoom newRef = (RefChatRoom) _newRef;
        type = newRef.type;
        send_cd = newRef.send_cd;
        input_unlock_condition = newRef.input_unlock_condition;
    }

    @Override
    public long Id()
    {
        return type.ordinal();
    }

    public ENPChatRoomType type = ENPChatRoomType.NONE;//聊天室类型
    public int send_cd;//发送cd（毫秒）
    public NPPlayerConditionGroupObj input_unlock_condition; //输入功能解锁条件
}
