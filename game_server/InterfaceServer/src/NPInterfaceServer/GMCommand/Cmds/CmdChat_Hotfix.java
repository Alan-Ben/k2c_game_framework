package NPInterfaceServer.GMCommand.Cmds;

import ISDB.Bo.ChatRoomBO;
import NPCommon.DB.BM.BM;
import NPCommon.GMCommand.Annotation.ACommand;
import NPCommon.GMCommand.Annotation.ACommander;
import NPCommon.GMCommand.CmdClassBase;
import NPCommon.Log.CommLog;
import NPInterfaceServer.ChatMgr.ChatRoomMgr.ChatRoomInfo;
import NPInterfaceServer.ChatMgr.ChatRoomMgr.ChatRoomMgr;
import NPInterfaceServer.NPInterfaceServer;

import java.lang.reflect.Field;
import java.lang.reflect.Method;
import java.util.ArrayList;

/**
 * 聊天相关GM命令热更新类
 *
 * 通过反射实现新增方法，支持热更新
 */
@ACommander(comment = "聊天相关命令", name = "chat_hotfix")
public class CmdChat_Hotfix extends CmdClassBase
{
    /**
     * 移除聊天房间（不向SDK反注册）- 热更新版本
     *
     * 通过反射从内存和数据库中删除房间信息，但SDK房间继续存在
     *
     * @param serverType 服务器类型
     * @param serverTypeId 服务器类型ID
     * @param roomType 房间类型
     * @param roomTypeId 房间类型ID
     * @return 操作结果
     */
    @ACommand(comment = "移除聊天房间（不向SDK反注册）")
    public String removeRoom(int serverType, int serverTypeId, int roomType, long roomTypeId)
    {
        try
        {
            ChatRoomMgr mgr = ChatRoomMgr.getInstance();

            // 反射获取 _lock 方法
            Method lockMethod = ChatRoomMgr.class.getDeclaredMethod("_lock");
            lockMethod.setAccessible(true);

            // 反射获取 _unlock 方法
            Method unlockMethod = ChatRoomMgr.class.getDeclaredMethod("_unlock");
            unlockMethod.setAccessible(true);

            // 反射获取 lookup 方法
            Method lookupMethod = ChatRoomMgr.class.getDeclaredMethod("lookup", int.class, int.class, int.class, long.class);
            lookupMethod.setAccessible(true);

            // 反射获取 _m_alRoomList 字段
            Field roomListField = ChatRoomMgr.class.getDeclaredField("_m_alRoomList");
            roomListField.setAccessible(true);

            // 加锁
            lockMethod.invoke(mgr);

            try
            {
                // 查找房间
                ChatRoomInfo room = (ChatRoomInfo) lookupMethod.invoke(mgr, serverType, serverTypeId, roomType, roomTypeId);
                if(null == room)
                {
                    return String.format("移除房间失败: 房间不存在, serverType=%d, serverTypeId=%d, roomType=%d, roomTypeId=%d",
                            serverType, serverTypeId, roomType, roomTypeId);
                }

                // 获取房间列表并移除
                @SuppressWarnings("unchecked")
                ArrayList<ChatRoomInfo> roomList = (ArrayList<ChatRoomInfo>) roomListField.get(mgr);
                roomList.remove(room);

                // 从数据库删除记录
                BM bm = NPInterfaceServer.getInstance().getBM();
                bm.getBM(ChatRoomBO.class).delAll("id", room.getId());

                return String.format("移除房间成功: serverType=%d, serverTypeId=%d, roomType=%d, roomTypeId=%d",
                        serverType, serverTypeId, roomType, roomTypeId);
            }
            finally
            {
                // 解锁
                unlockMethod.invoke(mgr);
            }
        }
        catch(Exception e)
        {
            String errorMsg = String.format("移除房间失败: 反射调用异常, serverType=%d, serverTypeId=%d, roomType=%d, roomTypeId=%d, error=%s",
                    serverType, serverTypeId, roomType, roomTypeId, e.getMessage());
            CommLog.error(errorMsg, e);
            return errorMsg;
        }
    }
}
