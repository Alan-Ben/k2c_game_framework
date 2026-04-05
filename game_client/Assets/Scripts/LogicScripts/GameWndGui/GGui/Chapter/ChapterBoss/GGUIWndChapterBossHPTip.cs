using System;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 爬塔战斗飘血提示
    /// </summary>
    public class GGUIWndChapterBossHPTip : _ATALBasicUISubWnd<GGUIMonoChapterBossHPTip>
    {
        public GGUIWndChapterBossHPTip(GGUIMonoChapterBossHPTip _wnd) : base(_wnd)
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
        }

        public void setInfoPlay(Transform _parent, string _info, string _aniName, Action _delayDoneAction)
        {
            if (null == wnd)
            {
                _delayDoneAction?.Invoke();
                return;
            }
            wnd.transform.parent = _parent;
            wnd.transform.localPosition = Vector3.zero;
            wnd.transform.localScale = Vector3.one;

            ALUGUICommon.setLabelTxt(wnd.txtHp, _info);
            if (wnd.showAni != null) 
                wnd.showAni.Play(_aniName, _delayDoneAction);
        }
    }
}
