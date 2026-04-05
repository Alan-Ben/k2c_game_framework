
using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public class NPGGUIWndChatMsgItemTextBubble : _ANPGGUIBasicSubWnd<NPGGUIMonoChatMsgItemTextBubble>
    {
        private string _m_text;
        private bool _m_isMyMsg;
        
        private NPGGoIndex _m_goIndex;
        private GameObject _m_emoteGo;
        private int _m_loadSerialize;

        public NPGGUIWndChatMsgItemTextBubble(NPGGUIMonoChatMsgItemTextBubble _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
        }

        protected override void _onReset()
        {
        }

        protected override void _onDiscard()
        {
            _m_loadSerialize = ALSerializeOpMgr.next();
            _discardGo();
            _m_goIndex = null;
        }

        protected override void _onWndInitDone()
        {
        }

        /// <summary>
        /// 文本的显示
        /// </summary>
        /// <param name="_text"></param>
        /// <param name="_isMyMsg"></param>
        public void setShowData(string _text, bool _isMyMsg)
        {
            _m_text = _text;
            _m_isMyMsg = _isMyMsg;
            _m_goIndex = null;

            if (_m_bIsShow)
                _refreshWnd();
        }
        
        /// <summary>
        /// 加载预制体的显示
        /// </summary>
        /// <param name="_goIndex"></param>
        public void setShowData(NPGGoIndex _goIndex)
        {
            _m_text = null;
            _m_goIndex = _goIndex;
            _refreshWnd();
        }

        private void _refreshWnd()
        {
            if (wnd == null)
                return;
            NPGGoIndex curGoIndex = _m_goIndex;
            _discardGo();
            _m_loadSerialize = ALSerializeOpMgr.next();
            if (null == curGoIndex)
            {
                if (wnd.monoSizeFix != null && wnd.txtText != null)
                {
                    ALUGUICommon.setLabelTxt(wnd.txtText, _m_text);
                
                    // 如果是自己的消息，换行后的对齐方式不一样
                    if (_m_isMyMsg)
                    {
                        if (wnd.txtText.preferredWidth > wnd.monoSizeFix.maxPreferredWidth)
                            wnd.txtText.alignment = TextAnchor.UpperLeft;
                        else
                            wnd.txtText.alignment = TextAnchor.UpperRight;
                    }
                }
                ALUGUICommon.setGameObjEnable(wnd.textMsgShow,true);
                ALUGUICommon.setGameObjEnable(wnd.prefabMsgShow,false);
            }
            else
            {
                int loadSerialize = _m_loadSerialize;
                GGoResCore.instance.loadObj(curGoIndex, (_assetHandle) =>
                {
                    if (loadSerialize != _m_loadSerialize)
                        return;
                    if (_assetHandle == null || _assetHandle.loadedInfo == null)
                    {
#if UNITY_EDITOR
                        UnityEngine.Debug.LogError("NPGGoIndex加载错误: " + _assetHandle.mainId + " - " + _assetHandle.subId);
#endif
                        return;
                    }
                    //获取资源对象
                    GameObject assetGo = _assetHandle.loadedInfo.obj;
                    if (null == assetGo)
                    {
#if UNITY_EDITOR
                        UnityEngine.Debug.LogError("NPGGoIndex加载错误: " + _assetHandle.mainId + " - " + _assetHandle.subId);
#endif
                        return;
                    }
                    _m_emoteGo = GameObject.Instantiate(assetGo);
                    _m_emoteGo.transform.SetParent(wnd.loadGoParent);
                    _m_emoteGo.transform.localPosition = Vector3.zero;
                    _m_emoteGo.transform.localScale = Vector3.one;
                });
                
                ALUGUICommon.setGameObjEnable(wnd.textMsgShow,false);
                ALUGUICommon.setGameObjEnable(wnd.prefabMsgShow,true);
            }
            
        }
        
        /// <summary>
        /// 销毁已经加载的go
        /// </summary>
        private void _discardGo()
        {
            if(null != _m_emoteGo)
                ALUnityCommon.releaseGameObj(_m_emoteGo);
            _m_emoteGo = null;
        }
    }
}