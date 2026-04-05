using System;
using GS2GC.p002_InitOp;
using System.Collections.Generic;
using Common.NpPlayerInfoObj;
using GS2GC.p021_PlayerInfo;

namespace GOE
{
    /// <summary>
    /// 聊天组件
    /// </summary>
    /// <remarks>
    /// 负责和聊天包的数据接入
    /// </remarks>
    public partial class NPPlayerChatComponent
    {
        private List<GChatEmoteGroup> _m_emoteGroupList = new List<GChatEmoteGroup>();
        public event Action emoteGroupChg;

        public GChatEmoteGroup getEmoteGroup(long _refId)
        {
            foreach (GChatEmoteGroup emoteGroup in _m_emoteGroupList)
            {
                if (emoteGroup.id == _refId)
                    return emoteGroup;
            }

            return null;
        }
        
        /// <summary>
        /// 是否有这个表情包
        /// </summary>
        /// <param name="_refid"></param>
        /// <returns></returns>
        public bool hasEmoteGroup(long _refid)
        {
            GChatEmoteGroup emoteGroup = getEmoteGroup(_refid);
            return emoteGroup != null && !emoteGroup.isExpire;
        }

        private void _reqEmoteGroupInit()
        {
            NPGSClientListener.sendMsg(GSWriter_002_InitOp.make_028_ReqChatEmoteGroupInit());
        }
        
        public void reqViewPlayerChatEmoteGroup(long _refId)
        {
            NPGSClientListener.sendMsg(NPGSWriter_021_PlayerInfoOp.make_049_ReqViewPlayerChatEmoteGroup(_refId));
        }

        public void retEmoteGroupInit(GS2GC_002_028_RetChatEmoteGroupInit _msg)
        {
            _m_emoteGroupList?.Clear();
            foreach (PlayerInfo_ChatEmoteGroup chatEmoteGroup in _msg.getEmoteGroupList())
            {
                _m_emoteGroupList.Add(new GChatEmoteGroup(chatEmoteGroup));
            }
            
            _m_initStepCounter.addDoneStepCount();
        }
        
        public void onChatEmoteGroupDel(GS2GC_021_083_OnChatEmoteGroupDel _msg)
        {
            foreach (GChatEmoteGroup chatEmoteGroup in _m_emoteGroupList)
            {
                if (chatEmoteGroup.id == _msg.getRefId())
                {
                    _m_emoteGroupList.Remove(chatEmoteGroup);
                    break;
                }
            }
            emoteGroupChg?.Invoke();
        }
        
        public void onChatEmoteGroupChg(GS2GC_021_084_OnChatEmoteGroupChg _msg)
        {
            GChatEmoteGroup chatEmoteGroup = getEmoteGroup(_msg.getInfo().getId());
            if (chatEmoteGroup == null)
            {
                chatEmoteGroup = new GChatEmoteGroup(_msg.getInfo());
                _m_emoteGroupList.Add(chatEmoteGroup);
            }
            else
            {
                chatEmoteGroup.updateInfo(_msg.getInfo());
            }
            emoteGroupChg?.Invoke();
        }

        public void onChatEmoteGroupAdd(GS2GC_021_087_OnChatEmoteGroupAdd _msg)
        {
            GChatEmoteGroup chatEmoteGroup = getEmoteGroup(_msg.getInfo().getId());
            if (chatEmoteGroup == null)
            {
                chatEmoteGroup = new GChatEmoteGroup(_msg.getInfo());
                _m_emoteGroupList.Add(chatEmoteGroup);
            }
            else
            {
                chatEmoteGroup.updateInfo(_msg.getInfo());
            }
            emoteGroupChg?.Invoke();
        }
    }
}
