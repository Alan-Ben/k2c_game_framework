using System.Collections.Generic;
using ALPackage;
using GOE;
using NPEnum;

[System.Serializable]
public class NPSOShareRefObj : _IALBasicRefObj
{
    public long _refId { get { return id; } }

    public long id;//唯一id
    public List<ENPChatRoomType> chat_room_list;//聊天房间列表


    [System.NonSerialized]
    private _IShareItemTypeRefObj _m_iShareItemTypeRefObj;//字表数据
    public _IShareItemTypeRefObj shareItemTypeRefObj
    {
        get { return _m_iShareItemTypeRefObj; }
        set { _m_iShareItemTypeRefObj = value; }
    }
}

/**************
 * 通用分享表
 **/
public class NPSOShareRefSet : _TALSOBasicRefSet<NPSOShareRefObj>
{
    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return "refdata/game_refdata.unity3d"; } }
    public static string objName { get { return "share"; } }
}
