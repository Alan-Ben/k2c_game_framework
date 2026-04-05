
using System;
using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 聊天表情
    /// </summary>
    [Serializable]
    public class GChatEmoteItemRefObj : _IALBasicRefObj
    {
        public long _refId { get { return id; } }

        public long id; // id
        public long group_id; // 所属组id
        public string name; // 名字
        public NPGGoIndex go_index; // 资源
        
    }
    public class GSOChatEmoteItemRefSet : _TALSOBasicRefSet<GChatEmoteItemRefObj>
    {
        /************
         * 资源加载路径
         **/
        public static string assetPath { get { return "refdata/game_refdata.unity3d"; } }
        public static string objName { get { return "chat_emote_item"; } }
    }
}