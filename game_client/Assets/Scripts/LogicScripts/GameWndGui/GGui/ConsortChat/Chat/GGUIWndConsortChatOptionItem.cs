using System;
using ALPackage;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// item
    /// </summary>
    public class GGUIWndConsortChatOptionItem : _ATALBasicUISubWnd<GGUIMonoConsortChatOptionItem>
    {
        private long _m_consortId;
        private long _m_dialogueId;
        private long _m_senteceId;
        private long _m_optionId;
        private Action<long, long,long> _m_onChooseOption;
        public GGUIWndConsortChatOptionItem(GGUIMonoConsortChatOptionItem _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
            
        }

        protected override void _onHideWnd()
        {
            
        }

        protected override void _onReset()
        {
        }

        protected override void _onDiscard()
        {
        }

        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
            
            ALUGUICommon.combineBtnClick(wnd.btnClick, _onBtnClick);
        }

        public void setInfo(long _consortId, long _dialogueId, long _sentenceId, long _optionId, Action<long, long,long> _onChooseOption)
        {
            _m_consortId = _consortId;
            _m_dialogueId = _dialogueId;
            _m_senteceId = _sentenceId;
            _m_optionId = _optionId;
            _m_onChooseOption = _onChooseOption;
            _refreshWnd();
        }

        private void _refreshWnd()
        {
            if (null == wnd)
                return;
            ConsortChatDialogueSentenceRefObj refOption = GRefdataCoreMgr.instance.consortChatDialogueSentenceRefCore.getRef(_m_optionId);
            if (refOption != null) ALUGUICommon.setLabelTxt(wnd.txtContent, refOption.getContent());
        }

        private void _onBtnClick(GameObject _)
        {
            _m_onChooseOption?.Invoke(_m_dialogueId, _m_senteceId, _m_optionId);
        }
    }
}
