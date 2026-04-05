using UnityEngine;
using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine.UI;


namespace GOE
{
    public class NPGGUIWndCheatInputItem : _ATALBasicUISubWnd <NPGGUIMonoCheatInputItem>
    {
        public NPGGUIWndCheatInputItem(NPGGUIMonoCheatInputItem _wnd)
            : base(_wnd)
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

            ALUGUICommon.combineBtnClick(wnd.btnSend, _onClickSend);
        }
        //
        // public void FocusOn()
        // {
        //     if(wnd != null && wnd.inputCommond != null)
        //         wnd.inputCommond.ActivateInputField();
        // }
        //
        // public void CheckFocus()
        // {
        //     if(wnd != null && wnd.inputCommond != null)
        //     {
        //         if(wnd.inputCommond.isFocused)
        //             _onClickSend(null);
        //     }
        // }

        private void _onClickSend(GameObject _go)
        {
            if(wnd == null || wnd.inputCommond == null)
                return;

            //处理作弊命令
            CheatMgr.instance.dealCheat(wnd.inputCommond.text);
        }
    }
}
