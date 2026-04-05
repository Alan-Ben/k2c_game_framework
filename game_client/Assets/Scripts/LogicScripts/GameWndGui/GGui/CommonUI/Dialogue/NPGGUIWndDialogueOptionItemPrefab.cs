using ALPackage;
using System;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 对话回应选项
    /// </summary>
    public class NPGGUIWndDialogueOptionItemPrefab : _ATALBasicLoadPrefabSubUIWnd<NPGGUIMonoDialogueOptionItemPrefab>
    {
        private int _m_iOptionIndex;//该选项的索引
        private Action<NPGGUIWndDialogueOptionItemPrefab> _m_aOnSelect;//选中回调
        private NPDialogueResponseOptionRefObj _m_refObj;


        public NPGGUIWndDialogueOptionItemPrefab(NPDialogueResponseOptionRefObj _refObj, Transform _parent) : base(_parent)
        {
            _m_refObj = _refObj;
        }


        protected override string _monoAssetPath { get => UIResPathAssistant.getAssetPath(_m_refObj.ui_path_id); }
        protected override string _monoObjName { get => UIResPathAssistant.getObjName(_m_refObj.ui_path_id); }
        protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }
        
        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsg(WinMsgType.SIMULATE_CLICK_DIALOG_OPTION, _simulateClickOption);
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.SIMULATE_CLICK_DIALOG_OPTION, _simulateClickOption);
        }

        protected override void _onReset()
        {
            _m_aOnSelect = null;
        }

        protected override void _onDiscard()
        {
            if (wnd == null)
                return;

            _m_aOnSelect = null;

            ALUGUICommon.uncombineBtnClick(wnd.btnSelect, _onClickBtnSelect);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            ALUGUICommon.combineBtnClick(wnd.btnSelect, _onClickBtnSelect);
        }

        /// <summary>
        /// 点击选择按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickBtnSelect(GameObject _go)
        {
            _m_aOnSelect?.Invoke(this);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_onSelect"></param>
        public void setInfo(Action<NPGGUIWndDialogueOptionItemPrefab> _onSelect)
        {
            this.regLoadDoneDelegate(() =>
            {
                showWnd();
                if (wnd == null || null == _m_refObj)
                    return;
                _m_aOnSelect = _onSelect;
                ALUGUICommon.setLabelTxt(wnd.txtResponse, TextTranslate.instance.getLanguage(_m_refObj.option_desc));
            });
            this.load();
        }

        /// <summary>
        /// 模拟点击选项
        /// </summary>
        /// <param name="_objs"></param>
        private void _simulateClickOption(params object[] _objs)
        {
            if(_m_refObj == null || _objs == null || _objs.Length < 1 || !(_objs[0] is long _optionId))
                return;
            
            if(_optionId != _m_refObj.id)
                return;
            
            _onClickBtnSelect(null);
        }
    }
}
