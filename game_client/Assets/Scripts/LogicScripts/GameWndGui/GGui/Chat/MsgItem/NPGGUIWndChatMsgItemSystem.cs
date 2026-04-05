
using ALPackage;
using ChatPackage;
using Common.NpChatObj;
using UnityEngine;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 一条系统文字信息的msgItem
    /// </summary>
    public class NPGGUIWndChatMsgItemSystem : _ANPGUISubWndChatMsgListItem<NPGGUIMonoChatMsgItemSystem, NPChatTextMsgSystemInfo>
    {
        // 头像
        private NPGGuiWndTexture _m_wtIconWnd;
        // 头像框
        private NPGGuiWndTexture _m_wIconBgkWnd;
        // 真正加载出来使用的气泡窗口
        private NPGGUIWndChatMsgItemTextBubble _m_textBubbleWnd;

        // 这个msgItem的高度
        private float _m_fHeight;
        // 用来计算高度的气泡模板mono
        private NPGGUIMonoChatMsgItemTextBubble _m_bubbleOriginMono;

        private long _m_bubbleId = 0;//气泡id
        
        public NPGGUIWndChatMsgItemSystem(NPChatTextMsgSystemInfo _detailInfo, Transform _parent, [NotNull] GUICacheMgrChatMsgItem _cacheMgr) : base(_detailInfo, _parent, _cacheMgr)
        {
            _m_fHeight = 0;
            if (null !=  _detailInfo.chatNpcRef)
                _m_bubbleId = _detailInfo.chatNpcRef.bubble_id;
        }

        protected override void _onShowWnd()
        {
            _refreshWnd();
            
            _m_textBubbleWnd?.showWnd();

            // NPPlayer.instance.chatComp.onPlayerInfoChg += _onPlayerInfoChg;
        }

        protected override void _onHideWnd()
        {
            // NPPlayer.instance.chatComp.onPlayerInfoChg -= _onPlayerInfoChg;
            
            _m_textBubbleWnd?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wtIconWnd?.discardTexture();
            _m_wIconBgkWnd?.discardTexture();
            _m_textBubbleWnd?.resetWnd();

            _m_fHeight = 0;
        }

        protected override void _onDiscard()
        {
            _m_wtIconWnd?.discard();
            _m_wIconBgkWnd?.discard();
            if (detailInfo != null)
            {
                // 回收气泡框
                NPGGUIChatMsgItemFactory.instance.msgItemTextBubbleCacheMgr.pushBackItem(_m_bubbleId, detailInfo.isMyMsg, _m_textBubbleWnd);
            }

            _m_textBubbleWnd = null;
        }

        protected override void _onWndInitDone()
        {
            base._onWndInitDone();

            if (wnd != null)
            {
                if (wnd.imgIcon != null)
                    _m_wtIconWnd = new NPGGuiWndTexture(wnd.imgIcon);
                if (wnd.imgIconBgk != null)
                    _m_wIconBgkWnd = new NPGGuiWndTexture(wnd.imgIconBgk);

                if (detailInfo != null)
                {
                    // 加载气泡框
                    NPGGUIChatMsgItemFactory.instance.msgItemTextBubbleCacheMgr.popItem(_m_bubbleId, wnd.bubbleParent, detailInfo.isMyMsg,
                        (_wnd) =>
                        {
                            if (_m_lLoadSerialize == -1)
                            {
                                NPGGUIChatMsgItemFactory.instance.msgItemTextBubbleCacheMgr.pushBackItem(_m_bubbleId, detailInfo.isMyMsg, _wnd);
                                return;
                            }
                    
                            if (_m_textBubbleWnd != null)
                                NPGGUIChatMsgItemFactory.instance.msgItemTextBubbleCacheMgr.pushBackItem(_m_bubbleId, detailInfo.isMyMsg, _m_textBubbleWnd);
                    
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
                        float textHeight = _m_bubbleOriginMono.txtText.getHeight(TextTranslate.instance.getLanguage(detailInfo.content.getContent()));
                        bubbleHeight = originBubbleRectTransform.rect.height - singleLineHeight + textHeight;
                    }

                    _m_fHeight = Mathf.Max(topToBubble + bubbleHeight, _m_fHeight);
                }
            }

            return _m_fHeight;
        }

        /// <inheritdoc/>
        protected override void _loadAdditionTemplate(ALStepCounter _stepCounter)
        {
            if (detailInfo == null)
                return;
            
            _stepCounter.chgTotalStepCount(1);
            NPGGUIChatMsgItemFactory.instance.msgItemTextBubbleCacheMgr.getTemplate(_m_bubbleId, detailInfo.isMyMsg, (_originBubbleMono) =>
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
            _refreshPlayerInfo();
        }

        // 刷新文字气泡信息
        private void _refreshTextBubble()
        {
            if (detailInfo == null)
                return;
            
            _m_textBubbleWnd?.setShowData(TextTranslate.instance.getLanguage(detailInfo.content.getContent()), detailInfo.isMyMsg);
        }
        
        // 刷新玩家信息
        private void _refreshPlayerInfo()
        {
            if (null != _m_wtIconWnd && null != detailInfo.chatNpcRef)
            {
                _m_wtIconWnd.setTexture(detailInfo.chatNpcRef.icon);
            }
            if (null != _m_wIconBgkWnd && null != detailInfo.chatNpcRef)
            {
                _m_wIconBgkWnd.setTexture(detailInfo.chatNpcRef.icon_bkg);
            }

            ALUGUICommon.setLabelTxt(wnd.txtName, detailInfo.getMiniSender());
        }
    }
}