using System;
using ALPackage;
using Common.EventObj;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 奖励事件
    /// </summary>
    public class GGUIWndCommonSimpleAwardEvent : _AGGUIWndCommonSimpleEvent<GGUIMonoCommonSimpleAwardEvent>
    {
        private static GGUIWndCommonSimpleAwardEvent _g_instance;
        public static GGUIWndCommonSimpleAwardEvent instance
        {
            get
            {
                if (_g_instance == null)
                    _g_instance = new GGUIWndCommonSimpleAwardEvent();
                return _g_instance;
            }
        }
        
        private CommonSimpleAwardEventAgent _m_iAwardEventShowAgent;//奖励事件信息

        private NPGGuiWndTexture _m_wEventImg;//事件图片
        private NPGGUIWndCommonRewardContainer _m_wRewardContanier;//奖励列表
        private long _m_lUIResId;//资源id

        /// <summary>
        /// 窗口资源id
        /// </summary>
        public long uiResId
        {
            get { return _m_lUIResId; }
            set { _m_lUIResId = value; }
        }

        protected override string _monoAssetPath { get { return UIResPathAssistant.getAssetPath(_m_lUIResId); } }
        protected override string _monoObjName { get { return UIResPathAssistant.getObjName(_m_lUIResId); } }

        public GGUIWndCommonSimpleAwardEvent() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override void _onInitDoneSub()
        {
            if (wnd == null)
                return;

            if (wnd.monoReward != null)
            {
                _m_wRewardContanier = new NPGGUIWndCommonRewardContainer(wnd.monoReward);
            }

            if (wnd.monoEventImg != null)
                _m_wEventImg = new NPGGuiWndTexture(wnd.monoEventImg);
            
            if(wnd.btnSure != null)
                ALUGUICommon.combineBtnClick(wnd.btnSure, _onSureBtnClick);
        }

        protected override void _onDiscardSub()
        {
            if(_m_wRewardContanier != null)
                _m_wRewardContanier.discard();
            _m_wRewardContanier = null;
            
            if(_m_wEventImg != null)
                _m_wEventImg.discard();
            _m_wEventImg = null;

            if (wnd != null)
            {
                if(wnd.btnSure != null)
                    ALUGUICommon.uncombineBtnClick(wnd.btnSure, _onSureBtnClick);
            }
        }

        protected override void _onShowWndSub()
        {
        }

        protected override void _onHideWndSub()
        {
            if(_m_wRewardContanier != null)
                _m_wRewardContanier.hideWnd();
            
            if(_m_wEventImg != null)
                _m_wEventImg.hideWnd();
        }

        protected override void _onResetSub()
        {
            if(_m_wRewardContanier != null)
                _m_wRewardContanier.resetWnd();
            
            if(_m_wEventImg != null)
                _m_wEventImg.discardTexture();
        }

        protected override void _onSetEventData(_ACommonSimpleEventAgent _commonEventAgent)
        {
            if (!(_commonEventAgent is CommonSimpleAwardEventAgent))
            {
                Debug.LogError("[GGUIWndCommonSimpleAwardEvent _onSetEventData] 传入参数_commonEventAgent 不是 CommonSimpleAwardEventAgent 类型");
            }
            
            _m_iAwardEventShowAgent = _commonEventAgent as CommonSimpleAwardEventAgent;
        }

        protected override void _refreshWndSub()
        {
            if (wnd == null || _m_iAwardEventShowAgent == null)
                return;
            
            if (_m_wRewardContanier != null)
            {
                _m_wRewardContanier.showWnd();
                _m_wRewardContanier.setRewardList(_m_iAwardEventShowAgent.eventShowReward);
            }
            
            if (_m_wEventImg != null)
            {
                _m_wEventImg.showWnd();
                _m_wEventImg.setTexture(_m_iAwardEventShowAgent.eventDetailIcon);
            }
        }

        /// <summary>
        /// 确定按钮被点击时
        /// </summary>
        /// <param name="_go"></param>
        private void _onSureBtnClick(GameObject _go)
        {
            if (_m_iAwardEventShowAgent == null)
                return;
            
            _m_iAwardEventShowAgent.reqDealEvent((_isSucc, _eventDoneInfo) =>
            {
                if(!_isSucc || _eventDoneInfo == null)
                    return;

                // _doCloseNode();//直接关闭窗口, 结果在Node的onCloseNode中展示
                _triggerEventDealDone();
            });
        }

        protected override void _doCloseNode()
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst_CommonEvent.C_COMMON_AWARD_EVENT_NODE);
        }
    }
}