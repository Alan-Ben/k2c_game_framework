
using ALPackage;
using ChatPackage;
using Common.NpChatObj;
using UnityEngine;
using JetBrains.Annotations;
using GS2GC.p004_PlayerOp;

namespace GOE
{
    /// <summary>
    /// 一条表情信息的msgItem
    /// </summary>
    public class GGUIWndChatMsgItemEmote : _ATNPGGUIWndPlayerChatMsgItem<GGUIMonoChatMsgItemEmote, ChatEmoteMsgDetailInfo>
    {
        // 真正加载出来使用的气泡窗口
        private NPGGUIWndChatMsgItemTextBubble _m_textBubbleWnd;

        // 这个msgItem的高度
        private float _m_fHeight;
        // 用来计算高度的气泡模板mono
        private NPGGUIMonoChatMsgItemTextBubble _m_bubbleOriginMono;
        private float _m_bubbleHeight;
        private bool _m_isGetHeightEnd = false;

        public GGUIWndChatMsgItemEmote(ChatEmoteMsgDetailInfo _detailInfo, Transform _parent, [NotNull] GUICacheMgrChatMsgItem _cacheMgr) : base(_detailInfo, _parent, _cacheMgr)
        {
            _m_fHeight = 0;
            _getEmoteHeight();
        }

        protected override long senderPlayerCid { get { return detailInfo == null ? 0 : detailInfo.sender.getCid(); } }

        protected override void _onShowWndEx()
        {
            _refreshWnd();
            _m_textBubbleWnd?.showWnd();
        }

        protected override void _onHideWndEx()
        {
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
            if (!_m_isGetHeightEnd)
            {
                _m_fHeight = 0;
                return _m_fHeight;
            }
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
                    float bubbleHeight = _m_bubbleHeight;

                    _m_fHeight = Mathf.Max(topToBubble + bubbleHeight + _originMono.offsetHeight, _m_fHeight);
                }
            }

            return _m_fHeight;
        }

        private void _getEmoteHeight()
        {
            _m_isGetHeightEnd = false;
            _m_bubbleHeight = 0;
            GChatEmoteItemRefObj itemRefObj = GRefdataCoreMgr.instance.chatEmoteItemRefCore.getRef(detailInfo.content.getRefId());
            if (null == itemRefObj)
            {
                _m_isGetHeightEnd = true;
                return;
            }
            _m_textBubbleWnd?.setShowData(itemRefObj.go_index);
            GGoResCore.instance.loadObj(itemRefObj.go_index, (_assetHandle) =>
            {
                if (_assetHandle == null || _assetHandle.loadedInfo == null)
                {
#if UNITY_EDITOR
                    UnityEngine.Debug.LogError("NPGGoIndex加载错误: " + _assetHandle.mainId + " - " + _assetHandle.subId);
#endif
                    _m_isGetHeightEnd = true;
                    return;
                }
                //获取资源对象
                GameObject assetGo = _assetHandle.loadedInfo.obj;
                if (null == assetGo)
                {
#if UNITY_EDITOR
                    UnityEngine.Debug.LogError("NPGGoIndex加载错误: " + _assetHandle.mainId + " - " + _assetHandle.subId);
#endif
                    _m_isGetHeightEnd = true;
                    return;
                }
                    
                _m_bubbleHeight = (assetGo.transform as RectTransform).rect.height;
                _m_isGetHeightEnd = true;
                _refreshHeight();
            });
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
            GChatEmoteItemRefObj itemRefObj = GRefdataCoreMgr.instance.chatEmoteItemRefCore.getRef(detailInfo.content.getRefId());
            if(null == itemRefObj)
                return;
            _m_textBubbleWnd?.setShowData(itemRefObj.go_index);
        }
        
    }
}