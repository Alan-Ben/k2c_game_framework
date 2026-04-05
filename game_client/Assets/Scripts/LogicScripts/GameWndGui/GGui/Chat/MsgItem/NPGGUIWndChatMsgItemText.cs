
using ALPackage;
using ChatPackage;
using Common.NpChatObj;
using UnityEngine;
using JetBrains.Annotations;
using GS2GC.p004_PlayerOp;

namespace GOE
{
    /// <summary>
    /// 一条文字信息的msgItem
    /// </summary>
    public class NPGGUIWndChatMsgItemText : _ATNPGGUIWndPlayerChatMsgItem<NPGGUIMonoChatMsgItemText, NPChatTextMsgDetailInfo>
    {
        // 真正加载出来使用的气泡窗口
        private NPGGUIWndChatMsgItemTextBubble _m_textBubbleWnd;

        // 这个msgItem的高度
        private float _m_fHeight;
        // 用来计算高度的气泡模板mono
        private NPGGUIMonoChatMsgItemTextBubble _m_bubbleOriginMono;
        
        public NPGGUIWndChatMsgItemText(NPChatTextMsgDetailInfo _detailInfo, Transform _parent, [NotNull] GUICacheMgrChatMsgItem _cacheMgr) : base(_detailInfo, _parent, _cacheMgr)
        {
            _m_fHeight = 0;
        }

        protected override long senderPlayerCid { get { return detailInfo == null ? 0 : detailInfo.sender.getCid(); } }

        protected override void _onShowWndEx()
        {
            _refreshWnd();
            _m_textBubbleWnd?.showWnd();

            // NPPlayer.instance.chatComp.onPlayerInfoChg += _onPlayerInfoChg;
        }

        protected override void _onHideWndEx()
        {
            // NPPlayer.instance.chatComp.onPlayerInfoChg -= _onPlayerInfoChg;
            
            _m_textBubbleWnd?.hideWnd();
        }

        protected override void _onResetEx()
        {
            _m_textBubbleWnd?.resetWnd();

            _m_fHeight = 0;
        }

        protected override void _onDiscardEx()
        {
            if (detailInfo != null)
            {
                // 回收气泡框
                NPGGUIChatMsgItemFactory.instance.msgItemTextBubbleCacheMgr.pushBackItem(detailInfo.sender.getBubbleId(), detailInfo.isMyMsg, _m_textBubbleWnd);
            }
            _m_textBubbleWnd = null;
        }

        protected override void _onWndInitDoneEx()
        {
            if (wnd != null)
            {
                if (detailInfo != null)
                {
                    // 加载气泡框
                    NPGGUIChatMsgItemFactory.instance.msgItemTextBubbleCacheMgr.popItem(detailInfo.sender.getBubbleId(), wnd.bubbleParent, detailInfo.isMyMsg,
                        (_wnd) =>
                        {
                            if (_m_lLoadSerialize == -1)
                            {
                                NPGGUIChatMsgItemFactory.instance.msgItemTextBubbleCacheMgr.pushBackItem(detailInfo.sender.getBubbleId(), detailInfo.isMyMsg, _wnd);
                                return;
                            }

                            if (_m_textBubbleWnd != null)
                                NPGGUIChatMsgItemFactory.instance.msgItemTextBubbleCacheMgr.pushBackItem(detailInfo.sender.getBubbleId(), detailInfo.isMyMsg, _m_textBubbleWnd);

                            _m_textBubbleWnd = _wnd;
                            if (_m_bIsShow && _m_textBubbleWnd != null)
                            {
                                _refreshTextBubble();
                                _m_textBubbleWnd.showWnd();
                            }
                        });
                }
            }
        }

        public override float getHeight()
        {
            if (_m_fHeight > 0)
                return _m_fHeight;

            // 如果两个模板有一个没取到，就返回0
            if (_originMono == null || detailInfo == null)
                _m_fHeight = 0;
            else
            {
                // 获取rectTransform
                RectTransform originRectTransform = _originMono.transform as RectTransform;
                if (originRectTransform != null)
                    _m_fHeight = originRectTransform.rect.height;

                RectTransform originBubbleRectTransform = null;
                if (_m_bubbleOriginMono != null)
                    originBubbleRectTransform = _m_bubbleOriginMono.transform as RectTransform;
                
                // 计算气泡的所带来的高度
                if (originBubbleRectTransform != null && _originMono.bubbleParent != null)
                {
                    // 计算顶端到气泡的高度
                    float topToBubble;
                    // 如果气泡的父节点刚好是最外层对象，就直接用高度
                    if (_originMono.bubbleParent.parent == _originMono.transform)
                        topToBubble = Mathf.Abs(_originMono.bubbleParent.localPosition.y);
                    else
                    {
                        // 如果不是，就转换坐标，算出顶端到气泡的距离
                        Vector3 worldPos = _originMono.bubbleParent.TransformPoint(Vector3.zero);
                        topToBubble = Mathf.Abs(_originMono.transform.InverseTransformPoint(worldPos).y);
                    }

                    // 计算气泡的高度
                    float bubbleHeight = 0;
                    if (_m_bubbleOriginMono.txtText != null)
                    {
                        // 虽然是异步的方法，但是其实是同步的
                        GameLanguageMgr.instance.getTextFontAsset(((_ITextMono)_m_bubbleOriginMono.txtText).fontType, _font =>
                                _m_bubbleOriginMono.txtText.font = _font);
                        float singleLineHeight = _m_bubbleOriginMono.txtText.getHeight(string.Empty);
                        float textHeight = _m_bubbleOriginMono.txtText.getHeight(detailInfo.content.getContent());
                        bubbleHeight = originBubbleRectTransform.rect.height - singleLineHeight + textHeight;
                    }

                    _m_fHeight = Mathf.Max(topToBubble + bubbleHeight, _m_fHeight);
                }
            }

            return _m_fHeight;
        }

        protected override void _refreshPlayerInfo(NPGGUIWndPlayerIcon _playerInfoWnd)
        {
            if (_playerInfoWnd == null || detailInfo == null)
                return;

            _playerInfoWnd.setPlayerInfo(detailInfo.sender);
        }

        /// <inheritdoc/>
        protected override void _loadAdditionTemplate(ALStepCounter _stepCounter)
        {
            if (detailInfo == null)
                return;
            
            _stepCounter.chgTotalStepCount(1);
            NPGGUIChatMsgItemFactory.instance.msgItemTextBubbleCacheMgr.getTemplate(detailInfo.sender.getBubbleId(), detailInfo.isMyMsg, (_originBubbleMono) =>
            {
                _m_bubbleOriginMono = _originBubbleMono;
                _stepCounter.addDoneStepCount();
            });
        }

        protected override void _discardAdditionTemplate()
        {
            _m_bubbleOriginMono = null;
        }

        private void _refreshWnd()
        {
            _refreshTextBubble();
        }

        // 刷新文字气泡信息
        private void _refreshTextBubble()
        {
            if (detailInfo == null)
                return;
            
            _m_textBubbleWnd?.setShowData(detailInfo.content.getContent(), detailInfo.isMyMsg);
        }
    }
}