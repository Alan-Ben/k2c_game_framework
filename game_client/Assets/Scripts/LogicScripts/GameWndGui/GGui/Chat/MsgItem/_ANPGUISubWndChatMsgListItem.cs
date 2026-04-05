
using System;
using ALPackage;
using ChatPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public abstract class _ANPGUISubWndChatMsgListItem<T_MONO, T_DATA> : _AGUISubWndChatMsgListItem<T_MONO, T_DATA>
        where T_MONO : _AGUIMonoChatMsgListItem
        where T_DATA : _IMsgItemData
    {
        protected _ANPGUISubWndChatMsgListItem(T_DATA _detailInfo, Transform _parent, [NotNull] GUICacheMgrChatMsgItem _cacheMgr) : base(_detailInfo, _parent, _cacheMgr)
        {
        }
    }
}