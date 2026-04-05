
using System;
using ALPackage;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 聊天室配置
    /// </summary>
    [Serializable]
    public class NPChatNPCRefObj : _IALBasicRefObj
    {
        public long _refId { get { return id; } }

        public long id; // id
        public string name; // 发送者名称
        public NPGTextureIndex icon; //头像
        public NPGTextureIndex icon_bkg; //头像框
        public long bubble_id; //气泡框ID
        
    }
    public class NPGSOChatNPCRefSet : _TALSOBasicRefSet<NPChatNPCRefObj>
    {
        /************
         * 资源加载路径
         **/
        public static string assetPath { get { return "refdata/game_refdata.unity3d"; } }
        public static string objName { get { return "chat_npc"; } }
    }
}