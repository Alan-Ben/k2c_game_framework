using System;
using ALPackage;
using Common.EventObj;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 关卡选择事件页面
    /// </summary>
    public class GGUIWndCommonSimpleChoiceEvent : _AGGUIWndCommonSimpleEvent<GGUIMonoCommonSimpleChoiceEvent>
    {
        private static GGUIWndCommonSimpleChoiceEvent _g_instance;
        public static GGUIWndCommonSimpleChoiceEvent instance
        {
            get
            {
                if (_g_instance == null)
                    _g_instance = new GGUIWndCommonSimpleChoiceEvent();
                return _g_instance;
            }
        }
        
        private CommonSimpleChoiceEventAgent _m_iChoiceEventAgent;//

        private NPGGuiWndTexture _m_wEventImg;//事件插图
        private GGUIWndCommonChoiceEventOptionContainer _m_wOptionContainer;//选项Container
        
        public GGUIWndCommonSimpleChoiceEvent() : base(EALUIWndLayer.ADDITION)
        {
        }
        
        public long uiResId { get; set; }
        
        protected override string _monoAssetPath { get { return UIResPathAssistant.getAssetPath(uiResId); } }
        protected override string _monoObjName { get { return UIResPathAssistant.getObjName(uiResId); } }

        protected override void _onInitDoneSub()
        {
            if(wnd == null)
                return;

            if (wnd.monoEventImg != null)
                _m_wEventImg = new NPGGuiWndTexture(wnd.monoEventImg);

            if (wnd.monoOptionContainer != null)
            {
                _m_wOptionContainer = new GGUIWndCommonChoiceEventOptionContainer(wnd.monoOptionContainer);
                _m_wOptionContainer.onOptionClick += _onOptionItemClick;
            }
            
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onCloseBtnClick);
        }

        protected override void _onDiscardSub()
        {
            if(_m_wEventImg != null)
                _m_wEventImg.discard();
            _m_wEventImg = null;

            if (_m_wOptionContainer != null)
            {
                _m_wOptionContainer.onOptionClick -= _onOptionItemClick;
                _m_wOptionContainer.discard();
                _m_wOptionContainer = null;
            }

            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onCloseBtnClick);       
            }
        }

        protected override void _onShowWndSub()
        {
        }

        protected override void _onHideWndSub()
        {
            _m_wEventImg?.hideWnd();
            _m_wOptionContainer?.hideWnd();
        }

        protected override void _onResetSub()
        {
            _m_wEventImg?.discardTexture();
            _m_wOptionContainer?.resetWnd();
        }

        protected override void _onSetEventData(_ACommonSimpleEventAgent _commonEventAgent)
        {
            if (!(_commonEventAgent is CommonSimpleChoiceEventAgent))
            {
                Debug.LogError("[GGUIWndCommonSimpleChoiceEvent _onSetEventData] 传入参数_commonEventAgent 不是 CommonSimpleChoiceEventAgent 类型");
            }
            
            _m_iChoiceEventAgent = _commonEventAgent as CommonSimpleChoiceEventAgent;
        }

        protected override void _refreshWndSub()
        {
            if (wnd == null || _m_iChoiceEventAgent == null)
                return;
            
            if (_m_wEventImg != null)
            {
                _m_wEventImg.showWnd();
                _m_wEventImg.setTexture(_m_iChoiceEventAgent.eventDetailIcon);
            }

            if (_m_wOptionContainer != null)
            {
                _m_wOptionContainer.showWnd();
                _m_wOptionContainer.setChoiceOption(_m_iChoiceEventAgent.optionList);
            }
        }

        /// <summary>
        /// 当选项被点击时
        /// </summary>
        private void _onOptionItemClick(GGUIWndCommonChoiceEventOptionItem _item)
        {
            if (_item == null || _item.choiceOptionRefObj == null || _m_iChoiceEventAgent == null)
                return;

            _m_iChoiceEventAgent.reqDealEvent(_item.choiceOptionRefObj, (_isSucc, _eventDoneInfo) =>
            {
                if(!_isSucc || _eventDoneInfo == null)
                    return;

                // _doCloseNode();//直接关闭窗口, 结果在Node的onCloseNode中展示
                _triggerEventDealDone();
            });
        }

        /// <summary>
        /// 关闭按钮被点击
        /// </summary>
        /// <param name="_go"></param>
        private void _onCloseBtnClick(GameObject _go)
        {
            _doCloseNode();
        }

        protected override void _doCloseNode()
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst_CommonEvent.C_COMMON_CHOICE_EVENT_NODE);
        }
    }
}