using System;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 爬塔战斗飘血提示
    /// </summary>
    public class GGUIWndTowerBattleHPTip : _ATALBasicUISubWnd<GGUIMonoTowerBattleHPTip>
    {
        public GGUIWndTowerBattleHPTip(GGUIMonoTowerBattleHPTip _wnd) : base(_wnd)
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

        public void showWnd(string _info, Action _delayDoneAction)
        {
            if (null == wnd)
            {
                _delayDoneAction?.Invoke();
                return;
            }

            showWnd();
            ALUGUICommon.setLabelTxt(wnd.txtHp, _info);
            if( AccountSettingMgr.instance.accountSetting.towerBattleSpeedTenTimes)
                wnd.wndAnimation.Play(wnd.tenTimesAniName, _delayDoneAction);
            else
                wnd.wndAnimation.Play(wnd.oneTimesAniName, _delayDoneAction);
        }
    }
}
