package NPUSServer.NPUSUserMgr.ItemDealer.Share;

import NPEnum.ENPChatRoomType;
import NPEnum.ENPShareItemType;
import NPGameRes.Refs.Share.RefShare;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;

import java.nio.ByteBuffer;
import java.util.ArrayList;

public abstract class _AShareItemDealer
{
    /***************
     * 分享子类型
     * @return
     */
    abstract public ENPShareItemType getShareItem();

    /*********************
     * 获得分享物件的处理
     *  @param _userData
     * @param _shareRef
     * @param _count
     * @param _context
     */
    abstract public void gainItem(NPUSUserData _userData, RefShare _shareRef, long _count, NPPlayerContext _context);

    /********************
     * 发送消息到聊天频道
     *
     * @param _userData
     * @param _roomTypeList
     * @param _byteContent
     */
    abstract public void sendChat(NPUSUserData _userData, ArrayList<ENPChatRoomType> _roomTypeList, ByteBuffer _byteContent);
}
