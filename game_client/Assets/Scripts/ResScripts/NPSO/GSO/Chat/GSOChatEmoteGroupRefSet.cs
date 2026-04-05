
using System;
using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 聊天表情组
    /// </summary>
    [Serializable]
    public class GChatEmoteGroupRefObj : _IALBasicRefObj
    {
        public long _refId { get { return id; } }

        public long id; // id
        public _NPPlayerConditionSerializeInfo show_cond; // 显示条件
        public int sort_id; // 排序id
        public string unlock_cond_desc; // 解锁描述（解锁就按照有没有这个item来判断）
        public List<string> unlock_cond_desc_args; // 解锁描述参数

#if NP_GAME

        [System.NonSerialized]
        private List<GChatEmoteItemRefObj> _m_emoteItemList;

        public List<GChatEmoteItemRefObj> emoteItemList
        {
            get
            {
                if (null == _m_emoteItemList)
                    _m_emoteItemList = new List<GChatEmoteItemRefObj>();
                return _m_emoteItemList;
            }
        }

        public void addEmoteItem(GChatEmoteItemRefObj _itemRef)
        {
            if (emoteItemList.Contains(_itemRef))
                return;
            emoteItemList.Add(_itemRef);
        }
#endif
        
    }
    public class GSOChatEmoteGroupRefSet : _TALSOBasicRefSet<GChatEmoteGroupRefObj>
    {
        /************
         * 资源加载路径
         **/
        public static string assetPath { get { return "refdata/game_refdata.unity3d"; } }
        public static string objName { get { return "chat_emote_group"; } }
    }
}