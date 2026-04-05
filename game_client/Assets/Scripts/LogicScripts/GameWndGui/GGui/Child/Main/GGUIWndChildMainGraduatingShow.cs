using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public partial class GGUIWndChildMain
    {
        public class GGUIWndChildMainGraduatingShow
        {
            [NotNull] private readonly GGUIMonoChildMainGraduatingShow _m_wnd;
            private bool _m_bIsShow;
            
            private NPGGUIWndCommonToggleEx _m_oneKeyGraduatingToggle;
            private ChildViewMgr _m_viewMgr;
            
            
            public GGUIWndChildMainGraduatingShow([NotNull] GGUIMonoChildMainGraduatingShow _wnd)
            {
                _m_wnd = _wnd;
                initWnd();
            }
            
            
            public void showWnd()
            {
                _m_bIsShow = true;
                
                _m_oneKeyGraduatingToggle?.showWnd();
                
                refreshWnd();
            }  
            public void hideWnd()
            {
                _m_oneKeyGraduatingToggle?.hideWnd();

                _m_bIsShow = false;
            }
            public void resetWnd()
            {
                _m_oneKeyGraduatingToggle?.resetWnd();
            }
            public void discard()
            {
                _m_oneKeyGraduatingToggle?.discard();
                _m_oneKeyGraduatingToggle = null;
                
                ALUGUICommon.uncombineBtnClick(_m_wnd.btnGraduating, _onBtnGraduatingClick);
            }
            public void initWnd()
            {
                if (_m_wnd.monoOneKeyGraduatingToggle != null)
                {
                    _m_oneKeyGraduatingToggle = new NPGGUIWndCommonToggleEx(_m_wnd.monoOneKeyGraduatingToggle);
                    _m_oneKeyGraduatingToggle.clickDelegate = _onOneKeyGraduatingToggleClick;
                }
                
                ALUGUICommon.combineBtnClick(_m_wnd.btnGraduating, _onBtnGraduatingClick);
            }
            
            
            public void refreshWnd(ChildViewMgr _viewMgr)
            {
                _m_viewMgr = _viewMgr;
                refreshWnd();
            }
            public void refreshWnd()
            {
                if (_m_viewMgr == null || !_m_bIsShow)
                    return;

                bool isOneKeyUnlock = GCommon.isSimpleUnlock(GRefdataCoreMgr.instance.npGeneral.child_one_key_graduate_unlock_id);
                ALUGUICommon.setGameObjEnable(_m_wnd.listOneKeyLockShow, !isOneKeyUnlock);
                _m_oneKeyGraduatingToggle?.setSelected(isOneKeyUnlock &&
                                                       AccountSettingMgr.instance.accountSetting.isChildOneKeyGraduating);
            }
            
            
            private void _onBtnGraduatingClick(GameObject _)
            {
                if (_m_viewMgr == null)
                    return;

                if (_m_oneKeyGraduatingToggle is { isOn: true })
                    _m_viewMgr.oneKeyGraduate();
                else
                    _m_viewMgr.graduate();
            }
            private void _onOneKeyGraduatingToggleClick(NPGGUIWndCommonToggleEx _toggle)
            {
                if (_toggle == null)
                    return;
                    
                NPSimpleUnlockRef unlockRef = GRefdataCoreMgr.instance.simpleUnlockMap.getRef(GRefdataCoreMgr.instance.npGeneral.child_one_key_graduate_unlock_id);
                bool isOneKeyUnlock = unlockRef == null || unlockRef.isConditionEnable(null);
                if (!isOneKeyUnlock)
                {
                    NPGUIAddSceneCenterTip.instance.showTransTextInfo(unlockRef.getUnlockTip());
                    return;
                }
                
                _toggle.setSelected(!_toggle.isOn);
                AccountSettingMgr.instance.accountSetting.setChildOneKeyGraduating(_toggle.isOn);
            }
        }
    }
}