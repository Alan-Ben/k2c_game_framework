
using System;
using System.Collections.Generic;
using ALPackage;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 聊天室配置
    /// </summary>
    [Serializable]
    public class NPChatRoomRefObj : _IALBasicRefObj
    {
        public long _refId { get { return (long) type; } }

        public ENPChatRoomType type; // 聊天室类型
        public string name; // 聊天室名字
        public NPGTextureIndex icon; 
        public int send_cd;
        public _NPPlayerConditionSerializeInfo unlock_condition;//频道解锁条件
        public _NPPlayerConditionSerializeInfo input_unlock_condition;//输入功能解锁条件
        public string unlock_condition_desc;
        public List<string> unlock_condition_desc_args;
        public string input_unlock_condition_desc;
        public List<string> input_unlock_condition_desc_args;
    }
    public class NPGSOChatRoomRefSet : _TALSOBasicRefSet<NPChatRoomRefObj>
    {
        /************
         * 资源加载路径
         **/
        public static string assetPath { get { return "refdata/game_refdata.unity3d"; } }
        public static string objName { get { return "chat_room"; } }
    }
}