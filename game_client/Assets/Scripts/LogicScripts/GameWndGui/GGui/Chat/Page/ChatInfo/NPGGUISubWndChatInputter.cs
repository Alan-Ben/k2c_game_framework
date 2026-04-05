
using System;
using ALPackage;
using ChatPackage;
using UnityEngine;
using JetBrains.Annotations;
using System.Collections.Generic;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 聊天的输入框
    /// </summary>
    public class NPGGUISubWndChatInputter : _ANPGGUIBasicSubWnd<NPGGUIMonoChatInputter>
    {
        // 当前的聊天会话
        private _AChatInfo _m_chatInfo;
        // cd 任务池
        [NotNull] private List<CDTask> _m_cdTaskPool;
        // cd 任务
        private CDTask _m_cdTask;
        private GGUIWndChatEmotePage _m_wndChatEmotePage;
        private GGUIWndCustomAddList _m_wndCustomAddList;
        //发送消息的音效实例id
        private long _m_lSendMsgAudioInstanceId;

        public event Action<bool> onAddPageIsShow;
        
        public NPGGUISubWndChatInputter(NPGGUIMonoChatInputter _wnd)
            : base(_wnd)
        {
            _m_cdTaskPool = new List<CDTask>(4);

            initWnd();
        }

        /// <summary>
        /// 当前的聊天的聊天会话
        /// </summary>
        public _AChatInfo chatInfo { get { return _m_chatInfo; } }
        /// <summary>
        /// 这个聊天会话的相关配置
        /// </summary>
        public _INPChatInfo chatRef { get { return _m_chatInfo as _INPChatInfo; } }

        protected override void _onShowWnd()
        {
            _refreshWnd();
            onAddPageIsShow?.Invoke(false);
            WinMsg.RegisterMsgAct(WinMsgType.ON_CHAT_SENDED, refreshShowState);
        }

        protected override void _onHideWnd()
        {
            _m_wndChatEmotePage?.hideWndWithoutAni();
            _m_wndCustomAddList?.hideWndWithoutAni();
            PlayAudioMgr.instance.stopClip(_m_lSendMsgAudioInstanceId);
            onAddPageIsShow?.Invoke(false);
            // 停止cd刷新任务
            _stopCDTask();
            WinMsg.UnregisterMsgAct(WinMsgType.ON_CHAT_SENDED, refreshShowState);
        }

        protected override void _onReset()
        {
            _m_chatInfo = null;
            
            _m_wndChatEmotePage?.hideWndWithoutAni();
            _m_wndCustomAddList?.hideWndWithoutAni();
            onAddPageIsShow?.Invoke(false);
        }

        protected override void _onDiscard()
        {
            _m_chatInfo = null;

            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnSend, _onBtnSendClick);
                ALUGUICommon.uncombineBtnClick(wnd.btnEmoji, _onBtnSendEmojiClick);
                ALUGUICommon.uncombineBtnClick(wnd.btnAddtion, _onBtnAddtionClick);
                if (wnd.inputField != null)
                    wnd.inputField.onValueChanged.RemoveListener(_onInputterValueChg);
            }
            
            _m_wndChatEmotePage?.discard();
            _m_wndChatEmotePage = null;
            
            _m_wndCustomAddList?.discard();
            _m_wndCustomAddList = null;
            
            onAddPageIsShow = null;
        }

        protected override void _onWndInitDone()
        {
            if (wnd != null)
            {
                ALUGUICommon.combineBtnClick(wnd.btnSend, _onBtnSendClick);
                ALUGUICommon.combineBtnClick(wnd.btnEmoji, _onBtnSendEmojiClick);
                ALUGUICommon.combineBtnClick(wnd.btnAddtion, _onBtnAddtionClick);
                if (wnd.inputField != null)
                    wnd.inputField.onValueChanged.AddListener(_onInputterValueChg);

                if (null != wnd.chatEmotePage)
                {
                    _m_wndChatEmotePage = new GGUIWndChatEmotePage(wnd.chatEmotePage);
                    _m_wndChatEmotePage.onClickEmoteItem += _onClickEmoteItem;
                    _m_wndChatEmotePage.onClickHide += _onClickHidePage;
                    _m_wndChatEmotePage.hideWnd();
                }

                if (null != wnd.customAddList)
                {
                    _m_wndCustomAddList = new GGUIWndCustomAddList(wnd.customAddList);
                    _m_wndCustomAddList.onClickHide += _onClickHidePage;
                    _m_wndCustomAddList.hideWnd();
                }
            }
        }

        private void _onClickHidePage()
        {
            onAddPageIsShow?.Invoke(false);
        }

        /// <summary>
        /// 设置显示参数
        /// </summary>
        public void setShowData(_AChatInfo _chatInfo)
        {
            _m_chatInfo = _chatInfo;

            // 如果已经显示了，那么就需要刷新
            if (_m_bIsShow)
                _refreshWnd();
        }
        /// <summary>
        /// 刷新
        /// </summary>
        public void refreshShowState()
        {
            // 如果还没有加载成功就不处理
            if (wnd == null)
                return;

            // 如果输入参数异常，就显示NONE页面
            if (_m_chatInfo == null)
                wnd.setShowData(NPGGUIMonoChatInputterShowType.NONE);
            else
            {
                // todo:加入玩家被封禁的显示
                
                // 先判断是否在cd中
                long timeToLastSend = FpsAndPingMgr.instance.serverTimeTag - _m_chatInfo.lastSendTime;
                if (chatRef != null && chatRef.getSendCD() > timeToLastSend)
                {
                    // 在cd中执行倒计时任务，并设置为cd页面
                    _startCDTask();
                    wnd.setShowData(NPGGUIMonoChatInputterShowType.CD);
                }
                else
                {
                    // 不在cd中，如果有输入字符，就显示等待发送页面
                    if (wnd.inputField != null && !string.IsNullOrEmpty(wnd.inputField.text))
                        wnd.setShowData(NPGGUIMonoChatInputterShowType.SEND_WAITING);
                    else
                    // 否则就显示等待输入页面
                        wnd.setShowData(NPGGUIMonoChatInputterShowType.INPUT_WAITING);
                }
            }
        }

        private void _refreshWnd()
        {
            if (wnd == null)
                return;
            
            refreshShowState();
            _hideAllAddWnd();
        }

        /// <summary>
        /// 点击发送表情按钮
        /// </summary>
        /// <param name="_"></param>
        private void _onBtnSendEmojiClick(GameObject _)
        {
            _m_wndCustomAddList?.hideWndWithoutAni();
            _m_wndChatEmotePage?.showWnd();
            
            onAddPageIsShow?.Invoke(true);
            //NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.common_sysUnOpen_none));
        }

        private void _onBtnAddtionClick(GameObject _)
        {
            _m_wndChatEmotePage?.hideWndWithoutAni();
            _m_wndCustomAddList?.showWnd();
            onAddPageIsShow?.Invoke(true);
        }
        

        private void _onClickEmoteItem(GChatEmoteItemRefObj _itemRefObj)
        {
            if (null == _m_chatInfo)
                return;
            _m_chatInfo.sendMsg(NPMsgDetailInfoFactory.createEmoteInfo(_itemRefObj.id));
            _m_wndChatEmotePage?.hideWnd();
            
            onAddPageIsShow?.Invoke(false);
        }

        /// <summary>
        /// 隐藏所有弹出的wnd
        /// </summary>
        private void _hideAllAddWnd()
        {
            _m_wndChatEmotePage?.hideWndToAniEnd();
            _m_wndCustomAddList?.hideWndToAniEnd();
            
            onAddPageIsShow?.Invoke(false);
        }

        private void _onBtnSendClick(GameObject _)
        {
            if (_m_chatInfo == null || wnd == null || wnd.inputField == null)
                return;

            //判断是否可能是作弊处理
            if(!string.IsNullOrEmpty(wnd.inputField.text) && wnd.inputField.text[0] == '&')
            {
                //此时当做作弊处理
                CheatMgr.instance.reqGmCommand(wnd.inputField.text.Substring(1));
                // 发送后清空文字，并刷新
                wnd.inputField.text = string.Empty;
                return;
            }

            // 判断是否在cd中
            long timeToLastSend = FpsAndPingMgr.instance.serverTimeTag - _m_chatInfo.lastSendTime;
            if (chatRef != null)
            {
                long cdMilliseconds = chatRef.getSendCD() - timeToLastSend;
                if (cdMilliseconds > 0)
                {
                    NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.chat_sendInCd_seconds, cdMilliseconds / 1000));
                    return;
                }
            }
            
            //输入功能未解锁
            if (!GCommon.isFuncUnlock(ENPFunctionType.CHAT_INPUT, true))
            {
                return;
            }

            string output = wnd.inputField.text;
            if (string.IsNullOrEmpty(output))
            {
                NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.chat_inputTextNull_none));
                return;
            }
            
            // 替换屏蔽字
            CharacterDetermineMgr.instance.replaceIllegalCharacter(true,ref output);
            // 发送文字消息
            _m_chatInfo.sendMsg(NPMsgDetailInfoFactory.createTextInfo(output));
            // 发送后清空文字，并刷新
            wnd.inputField.text = string.Empty;
            // 播放发送音效
            _m_lSendMsgAudioInstanceId = PlayAudioMgr.instance.playClip(wnd.sendMsgAudioId);
            refreshShowState();
        }

        private void _onInputterValueChg(string _)
        {
            refreshShowState();
        }

        private void _pushBackTask(CDTask _cdTask)
        {
            _m_cdTaskPool.Add(_cdTask);
        }

        private void _startCDTask()
        {
            _m_cdTask?.stop();
            _m_cdTask = _m_cdTaskPool.GetLastAndRemove() ?? new CDTask(this);
            _m_cdTask.restart();
        }

        private void _stopCDTask()
        {
            _m_cdTask?.stop();
            _m_cdTask = null;
        }

        /// <summary>
        /// 刷新cd显示
        /// </summary>
        protected void _onCdRefresh(long _cdCount)
        {
            if (wnd == null)
                return;
            
            ALUGUICommon.setLabelTxt(wnd.txtCD, TextTranslate.instance.getLanguage(TransKeyConst.chat_cdShow_seconds, _cdCount / 1000));
        }

        /// <summary>
        /// cd任务
        /// </summary>
        private class CDTask : _IALBaseMonoTask
        {
            [NotNull]
            private NPGGUISubWndChatInputter _m_instance;
            private bool _m_isEnable;
            
            public CDTask([NotNull] NPGGUISubWndChatInputter _instance)
            {
                _m_instance = _instance;
            }
            
            public _AChatInfo chatInfo { get { return _m_instance._m_chatInfo; } }
            public _INPChatInfo chatRef { get { return _m_instance.chatRef; } }

            public void deal()
            {
                if (!_m_isEnable || chatInfo == null)
                {
                    _m_instance._pushBackTask(this);
                    return;
                }

                long timeToLastSend = FpsAndPingMgr.instance.serverTimeTag - chatInfo.lastSendTime;
                long cd = chatRef == null ? 0 : chatRef.getSendCD();
                long cdMilliseconds = cd - timeToLastSend;
                if (cdMilliseconds <= 0)
                {
                    _m_instance._pushBackTask(this);
                    _m_instance.refreshShowState();
                    return;
                }

                _m_instance._onCdRefresh(cdMilliseconds);
                ALMonoTaskMgr.instance.addMonoTask(this, 0.5f);
            }

            public void restart()
            {
                _m_isEnable = true;
                deal();
            }

            public void stop()
            {
                _m_isEnable = false;
            }
        }
    }
}