using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public partial class GGUIWndChildMain
    {
        public class GGUIWndChildMainNamingShow
        {
            [NotNull] private readonly GGUIMonoChildMainNamingShow _m_wnd;
            private bool _m_bIsShow;
            
            private NPGGUIWndCommonToggleEx _m_oneKeyNamingToggle;
            private ChildViewMgr _m_viewMgr;


            public GGUIWndChildMainNamingShow([NotNull] GGUIMonoChildMainNamingShow _wnd)
            {
                _m_wnd = _wnd;
                initWnd();
            }


            public void showWnd()
            {
                WinMsg.RegisterMsgAct(WinMsgType.SIMULATE_CLICK_OPEN_CHILD_SET_NAME, _onSimulateClickOpenChildSetName);
                _m_bIsShow = true;
                
                _m_oneKeyNamingToggle?.showWnd();

                refreshWnd();
            }  
            public void hideWnd()
            {
                WinMsg.UnregisterMsgAct(WinMsgType.SIMULATE_CLICK_OPEN_CHILD_SET_NAME, _onSimulateClickOpenChildSetName);
                _m_oneKeyNamingToggle?.hideWnd();

                _m_bIsShow = false;
            }

            public void resetWnd()
            {
                _m_oneKeyNamingToggle?.resetWnd();
            }
            public void discard()
            {
                _m_oneKeyNamingToggle?.discard();
                _m_oneKeyNamingToggle = null;
                
                ALUGUICommon.uncombineBtnClick(_m_wnd.btnNaming, _onBtnNamingClick);
                ALUGUICommon.uncombineBtnClick(_m_wnd.btnRule, _onBtnRuleClick);
            }
            public void initWnd()
            {
                if (_m_wnd.monoOneKeyNamingToggle != null)
                {
                    _m_oneKeyNamingToggle = new NPGGUIWndCommonToggleEx(_m_wnd.monoOneKeyNamingToggle);
                    _m_oneKeyNamingToggle.clickDelegate = _onOneKeyNamingToggleClick;
                }
                
                ALUGUICommon.combineBtnClick(_m_wnd.btnNaming, _onBtnNamingClick);
                ALUGUICommon.combineBtnClick(_m_wnd.btnRule, _onBtnRuleClick);
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

                bool isOneKeyUnlock = GCommon.isSimpleUnlock(GRefdataCoreMgr.instance.npGeneral.child_one_key_naming_unlock_id);
                ALUGUICommon.setGameObjEnable(_m_wnd.listOneKeyLockShow, !isOneKeyUnlock);
                if (!isOneKeyUnlock)
                    _m_oneKeyNamingToggle?.setSelected(false);
                else
                    _m_oneKeyNamingToggle?.setSelected(AccountSettingMgr.instance.accountSetting.isChildOneKeyNaming);
            }
            
            
            private void _onBtnNamingClick(GameObject _)
            {
                if (_m_viewMgr == null)
                    return;

                if (_m_oneKeyNamingToggle is { isOn: true })
                {
                    _m_viewMgr.oneKeyNaming();
                    return;
                }
                
                GGUIWndChildNaming.instance.refreshWnd(_m_viewMgr);
                QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndChildNaming.instance, GGUIWndChildNaming.instance.showWnd, UINodeTagConst_Child.C_CHILD_NAMING);
            }
            private void _onBtnRuleClick(GameObject _btn)
            {
                if (_btn == null)
                    return;

                QueueMgr.instance.AddNode(new NPGNodeCommonToolTip_Text(
                    UIResPathAssistant.getAssetPath(UIResPathConst.WIN_TOOL_TIP_TEXT_FOLLOW),
                    UIResPathAssistant.getObjName(UIResPathConst.WIN_TOOL_TIP_TEXT_FOLLOW),
                    TextTranslate.instance.getLanguage(TransKeyConst.child_oneKeyNamingTip_none),
                    (RectTransform)_btn.transform, _m_wnd.tooltipOffset.x, _m_wnd.tooltipOffset.y));
            }
            private void _onOneKeyNamingToggleClick(NPGGUIWndCommonToggleEx _toggle)
            {
                if (_toggle == null)
                    return;
                
                NPSimpleUnlockRef unlockRef = GRefdataCoreMgr.instance.simpleUnlockMap.getRef(GRefdataCoreMgr.instance.npGeneral.child_one_key_naming_unlock_id);
                bool isOneKeyUnlock = unlockRef == null || unlockRef.isConditionEnable(null);
                if (!isOneKeyUnlock)
                {
                    NPGUIAddSceneCenterTip.instance.showTransTextInfo(unlockRef.getUnlockTip());
                    return;
                }
                
                _toggle.setSelected(!_toggle.isOn);
                AccountSettingMgr.instance.accountSetting.setChildOneKeyNaming(_toggle.isOn);
            }

            private void _onSimulateClickOpenChildSetName()
            {
                _onBtnNamingClick(null);
            }
        }
    }
}