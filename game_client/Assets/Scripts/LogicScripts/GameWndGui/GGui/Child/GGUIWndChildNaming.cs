using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class GGUIWndChildNaming : _ANPGGUIBasicWnd<GGUIMonoChildNaming>
    {
        [NotNull] public static GGUIWndChildNaming instance { get { return _g_instance ??= new GGUIWndChildNaming(); } }
        private static GGUIWndChildNaming _g_instance;

        private GGUISubWndChildInfo _m_childInfoWnd;
        
        private ChildViewMgr _m_viewMgr;
        private string _m_lastSuitName;
        

        public GGUIWndChildNaming() 
            : base(EALUIWndLayer.ADDITION)
        {
        }
        
        
        protected override string _monoAssetPath { get { return GGUIMonoChildNaming.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoChildNaming.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        

        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsgAct(WinMsgType.SIMULATE_CLICK_CONFIRM_CHILD_SET_NAME, _onSimulateClickConfirmChildSetName);
            _m_childInfoWnd?.showWnd();
            
            refreshWnd();
        }
        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsgAct(WinMsgType.SIMULATE_CLICK_CONFIRM_CHILD_SET_NAME, _onSimulateClickConfirmChildSetName);
            _m_childInfoWnd?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_childInfoWnd?.resetWnd();
        }
        protected override void _onDiscard()
        {
            _m_childInfoWnd?.discard();
            _m_childInfoWnd = null;
            
            if (wnd == null)
                return;
            
            if (wnd.iptName != null)
                wnd.iptName.onValueChanged.RemoveListener(_onInputValueChanged);
            
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onBtnCloseClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnConfirm, _onBtnConfirmClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnRandom, _onBtnRandomClick);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoChildInfo != null)
                _m_childInfoWnd = new GGUISubWndChildInfo(wnd.monoChildInfo);
            if (wnd.iptName != null)
                wnd.iptName.onValueChanged.AddListener(_onInputValueChanged);
            
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onBtnCloseClick);
            ALUGUICommon.combineBtnClick(wnd.btnConfirm, _onBtnConfirmClick);
            ALUGUICommon.combineBtnClick(wnd.btnRandom, _onBtnRandomClick);
        }


        public void refreshWnd(ChildViewMgr _viewMgr)
        {
            _m_viewMgr = _viewMgr;
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow)
                return;

            _m_lastSuitName = string.Empty;
            if (wnd.iptName != null)
                _onInputValueChanged(wnd.iptName.text);
            
            _m_childInfoWnd?.refreshWnd(_m_viewMgr?.curSelectSeatInfo?.childInfo);
            _onBtnRandomClick(null);
        }
        
        
        private void _onBtnCloseClick(GameObject _)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst_Child.C_CHILD_NAMING);
        }
        private void _onBtnConfirmClick(GameObject _)
        {
            if (wnd == null || wnd.iptName == null || _m_viewMgr == null)
                return;

            _m_viewMgr.setName(wnd.iptName.text, _isSuc =>
            {
                if (_isSuc)
                {
                    //子嗣首次赐名成功
                    WinMsg.SendMsg(WinMsgType.CONTROL_TUROTIAL_SETP, ENPTutorialTriggerType.CHILD_FIRST_SET_NAME_SUC);
                    //上浮提示
                    NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.playerInfo_setnameSuc_str);
                    //播放音效
                    if(wnd != null && wnd.setNameSucAudioId > 0)
                        PlayAudioMgr.instance.playClip(wnd.setNameSucAudioId);
                    //关闭窗口
                    _onBtnCloseClick(null);
                }
            });
        }
        private void _onBtnRandomClick(GameObject _)
        {
            if (wnd == null || wnd.iptName == null)
                return;

            ChildInfo childInfo = _m_viewMgr?.curSelectSeatInfo?.childInfo;
            if (childInfo == null)
                return;
            
            wnd.iptName.text = GRefdataCoreMgr.instance.getChildRandomName(childInfo.sex);
        }
        private void _onInputValueChanged(string _value)
        {
            int length = CharacterDetermineMgr.instance.getUnicodeStringLength(_value);
            int maxLength = GRefdataCoreMgr.instance.npGeneral.child_name_length_limit;
            if (maxLength <= 0)
                return;
            
            if (length > maxLength)
            {
                wnd.iptName.text = _m_lastSuitName;
                return;
            }
            
            ALUGUICommon.setLabelTxt(wnd.txtLimitDesc, TextTranslate.instance.getLanguage(TransKeyConst.common_useNum2_num_num, length, maxLength));
            _m_lastSuitName = _value;
        }

        private void _onSimulateClickConfirmChildSetName()
        {
            _onBtnConfirmClick(null);
        }
    }
}