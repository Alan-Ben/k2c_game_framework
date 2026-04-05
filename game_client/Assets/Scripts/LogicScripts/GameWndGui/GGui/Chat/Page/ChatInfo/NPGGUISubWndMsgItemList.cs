using System;
using System.Collections.Generic;
using ALPackage;
using ChatPackage;
using ChatPackage.Internal;
using Common.NpChatObj;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class NPGGUISubWndMsgItemList : GUISubWndChatMsgList<NPGGUIMonoMsgItemList>
    {
        private bool _m_isSeeNewest;
        private bool _m_isShowUnreadList;
        //加载的历史消息的数量
        private int _m_loadHistoryMsgCount = 0;
        
        public NPGGUISubWndMsgItemList(NPGGUIMonoMsgItemList _wnd, [NotNull] GUICacheMgrChatMsgItem _itemCacheMgr) : base(_wnd, _itemCacheMgr)
        {
            _m_loadHistoryMsgCount = 0;
        }

        protected override void _onDiscard()
        {
            base._onDiscard();

            if (wnd == null)
                return;
            _m_loadHistoryMsgCount = 0;
            ALUGUICommon.uncombineBtnClick(wnd.toBottomBtn, _onBottomBtnClick);
            onSeeNewest -= _onSeeNewest;
            WinMsg.UnregisterMsg(WinMsgType.ON_TO_SHOW_ALL_ITEM,_onToShowAllItem);
        }

        protected override void _onWndInitDone()
        {
            base._onWndInitDone();

            if (wnd == null)
                return;
            
            ALUGUICommon.combineBtnClick(wnd.toBottomBtn, _onBottomBtnClick);
            onSeeNewest += _onSeeNewest;
            WinMsg.RegisterMsg(WinMsgType.ON_TO_SHOW_ALL_ITEM,_onToShowAllItem);
        }

        protected override void _beforeReceiveMsgModify(MsgInfo _newMsg)
        {
            if (_newMsg == null)
                return;

            MsgInfo lastMsgInfo = chatInfo.getHistoryListSync(_newMsg.msgId, 1).GetLast();
            if (lastMsgInfo == null)
                _addLastMsgItem(new NPChatTimeMsgItemInfo(_newMsg.timeMs));
            else if (_newMsg.timeMs - lastMsgInfo.timeMs > GRefdataCoreMgr.instance.npGeneral.chat_show_time_condition_ms) 
                _addLastMsgItem(new NPChatTimeMsgItemInfo(_newMsg.timeMs));

            if (!_m_isSeeNewest && wnd != null && !_m_isShowUnreadList)
            {
                ALUGUICommon.setGameObjEnable(wnd.unreadMsgShowList, true);
                _m_isShowUnreadList = true;
            }
            else
                ALUGUICommon.setGameObjEnable(wnd?.unreadMsgShowList, false);
            //设置已读由外层处理
            // NPPlayer.instance.chatComp.setCurChatReaded(chatInfo);
        }

        protected override List<_IMsgItemData> _initMsgListModify(List<MsgInfo> _msgInfoList)
        {
            //设置默认隐藏未读消息提示
            ALUGUICommon.setGameObjEnable(wnd?.unreadMsgShowList, false);
            _m_loadHistoryMsgCount = _msgInfoList.Count;
            return _makeMsgItemData(_msgInfoList);
        }

        protected override List<_IMsgItemData> _historyMsgListModify(List<MsgInfo> _msgInfoList)
        {
            //历史消息获取已达上限，不再继续获取历史数据
            if (_m_loadHistoryMsgCount >= wnd.maxHistroyInfoCount)
            {
#if UNITY_EDITOR
                ALLog.Warning($"历史消息获取数量已达上限，当前：{_m_loadHistoryMsgCount}，上限：{wnd.maxHistroyInfoCount}");
#endif
                return null;
            }
            _m_loadHistoryMsgCount += _msgInfoList.Count;
            return _makeMsgItemData(_msgInfoList);
        }

        private List<_IMsgItemData> _makeMsgItemData(List<MsgInfo> _msgInfoList)
        {
            if (_msgInfoList == null)
                return null;
            //对消息进行排序
            _msgInfoList.Sort((_x, _y) =>
            {
                if (_x.timeMs > _y.timeMs)
                    return 1;
                if (_x.timeMs < _y.timeMs)
                    return -1;
                return 0;
            });

            List<_IMsgItemData> itemDataList = new List<_IMsgItemData>();
            bool isFirst = true;
            for (int i = 0; i < _msgInfoList.Count; i++)
            {
                MsgInfo msgInfo = _msgInfoList[i];
                if(msgInfo == null)
                    continue;
                
                //屏蔽列表内的，不显示
                NPCommon_ChatPlayerContent sender = new NPCommon_ChatPlayerContent();

                //增加容错，防止解析异常导致整个消息列表加载失败
                try
                {
                    sender.readPackage(msgInfo.detailInfo.getSenderBytesData());
                }
                catch (Exception exception)
                {
                    Debug.LogError_EditorOnly($"(仅Editor)有消息解析失败：MsgId:{msgInfo.msgId}，异常信息：{exception}");
                    continue;
                }

                if(NPPlayer.instance.friendsComp.isShield(sender.getCid()))
                    continue;
                
                // 第一条固定添加一个时间标记
                if (i == 0 || isFirst)
                {
                    itemDataList.Add(new NPChatTimeMsgItemInfo(msgInfo.timeMs));
                    isFirst = false;
                }
                else
                {
                    // 和上一条消息间隔过久也添加一个时间标记
                    MsgInfo lastMsgInfo = _msgInfoList[i - 1];
                    if (msgInfo.timeMs - lastMsgInfo.timeMs > GRefdataCoreMgr.instance.npGeneral.chat_show_time_condition_ms)
                    {
                        itemDataList.Add(new NPChatTimeMsgItemInfo(msgInfo.timeMs)); 
                    }
                }
                itemDataList.Add(_msgInfoList[i].detailInfo);
            }

            return itemDataList;
        }

        private void _onBottomBtnClick(GameObject _)
        {
            if (wnd == null || wnd.scrollRect == null)
                return;

            wnd.scrollRect.verticalNormalizedPosition = 0;
        }

        private void _onSeeNewest(bool _isSeeNewest)
        {
            _m_isSeeNewest = _isSeeNewest;
            if (_m_isSeeNewest && wnd != null && _m_isShowUnreadList)
            {
                ALUGUICommon.setGameObjEnable(wnd.unreadMsgShowList, false);
                _m_isShowUnreadList = false;
            }
        }
        
        
        private void _onToShowAllItem(object[] __objs)
        {
            if(__objs.Length == 0)
                return;
            if (__objs[0] is RectTransform)
            {
                if(null ==  wnd.scrollRect)
                    return;
                
                RectTransform targetRectTransform = __objs[0] as RectTransform;
                //目标点的世界坐标
                Vector3 centerWorldPos = targetRectTransform.transform.TransformPoint(targetRectTransform.rect.center);
                //目标点的屏幕坐标
                Vector2 centerScreenPos = RectTransformUtility.WorldToScreenPoint(Game.instance.mainCamera.fullCanvas.worldCamera, centerWorldPos);

                //目标点的UGUI坐标
                Vector2 uiPos;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    wnd.scrollRect.viewport,
                    centerScreenPos,
                    Game.instance.mainCamera.uiCamera, 
                    out uiPos);
                RectTransform scrollRect = wnd.scrollRect.viewport;
                float offSetY = 0;
                //计算偏移值
                if (uiPos.y - targetRectTransform.rect.height / 2 < -scrollRect.rect.height / 2)//超出最底下
                {
                    offSetY = (uiPos.y - targetRectTransform.rect.height / 2) + scrollRect.rect.height / 2;
                }
                else if(uiPos.y + targetRectTransform.rect.height / 2 > scrollRect.rect.height / 2) // 超出最上面
                {
                    offSetY = (uiPos.y + targetRectTransform.rect.height / 2) - scrollRect.rect.height / 2;
                }
                
                //把列表移动相应的偏移值
                wnd.scrollRect.content.localPosition += new Vector3(0, -offSetY,0);
            } 
        }

        public void moveToButtom()
        {
            wnd.scrollRect.verticalNormalizedPosition = 0;
        }
    }
}