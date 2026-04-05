package NPUSServer.GMCommand.Cmds;

import NPCommon.GMCommand.Annotation.ACommand;
import NPCommon.GMCommand.Annotation.ACommander;
import NPEnum.ENPChatRoomType;
import NPUSServer.GMCommand.UsCmdBase;

/**
 * 聊天相关GM命令热更新类
 * <p>
 * 通过反射实现新增方法，支持热更新
 */
@ACommander(comment = "聊天相关命令", name = "chat_hotfix")
public class CmdChat_Hotfix extends UsCmdBase
{
    /**
     * 注册聊天房间
     * <p>
     * 创建并注册新的聊天房间，如果房间已存在则返回现有房间
     * @param roomType   房间类型枚举值
     * @param roomTypeId 房间类型ID（单房间对象默认设置0）
     * @return 操作结果
     */
    @ACommand(comment = "注册聊天房间[房间类型枚举值][房间类型ID]")
    public String regRoom(ENPChatRoomType roomType, long roomTypeId)
    {
//        getUserServer().getChatRoomMgr().regRoom(roomType, roomTypeId);
//        return String.format("注册房间成功: roomType=%s, roomTypeId=%d", roomType.name(), roomTypeId);
        return "no use";
    }

    /**
     * 移除聊天房间（不向IS反注册）- 热更新版本
     * <p>
     * 通过反射从内存中删除房间信息，但不向IS发送反注册请求
     * @param roomType   房间类型枚举值
     * @param roomTypeId 房间类型ID
     * @return 操作结果
     */
    @ACommand(comment = "移除聊天房间（不向IS反注册）[房间类型枚举值][房间类型ID]")
    public String removeRoom(ENPChatRoomType roomType, long roomTypeId)
    {
//        try
//        {
//            ChatRoomUserMgr mgr = getUserServer().getChatRoomMgr();
//
//            // 反射获取 _lock 方法
//            Method lockMethod = ChatRoomUserMgr.class.getDeclaredMethod("_lock");
//            lockMethod.setAccessible(true);
//
//            // 反射获取 _unlock 方法
//            Method unlockMethod = ChatRoomUserMgr.class.getDeclaredMethod("_unlock");
//            unlockMethod.setAccessible(true);
//
//            // 反射获取 lookupRoom 方法
//            Method lookupRoomMethod = ChatRoomUserMgr.class.getDeclaredMethod("lookupRoom", ENPChatRoomType.class, long.class);
//            lookupRoomMethod.setAccessible(true);
//
//            // 反射获取 _m_alRoomList 字段
//            Field roomListField = ChatRoomUserMgr.class.getDeclaredField("_m_alRoomList");
//            roomListField.setAccessible(true);
//
//            // 加锁
//            lockMethod.invoke(mgr);
//
//            try
//            {
//                // 查找房间
//                ChatRoomUserInfo room = (ChatRoomUserInfo) lookupRoomMethod.invoke(mgr, roomType, roomTypeId);
//                if (null == room)
//                {
//                    return String.format("移除房间失败: 房间不存在, roomType=%s, roomTypeId=%d", roomType.name(), roomTypeId);
//                }
//
//                // 获取房间列表并移除
//                @SuppressWarnings("unchecked")
//                ArrayList<ChatRoomUserInfo> roomList = (ArrayList<ChatRoomUserInfo>) roomListField.get(mgr);
//                roomList.remove(room);
//
//                return String.format("移除房间成功: roomType=%s, roomTypeId=%d", roomType.name(), roomTypeId);
//            } finally
//            {
//                // 解锁
//                unlockMethod.invoke(mgr);
//            }
//        } catch (Exception e)
//        {
//            String errorMsg = String.format("移除房间失败: 反射调用异常, roomType=%s, roomTypeId=%d, error=%s",
//                    roomType.name(), roomTypeId, e.getMessage());
//            CommLog.error(errorMsg, e);
//            return errorMsg;
//        }

        return "no use";
    }
}
