using System;
using ALPackage;
using CommonEnum;
using UnityEngine;

namespace GOE
{
    public class GGUIWndConsortBusinessSkillUnlock : _ANPGGUIBasicWnd<GGUIMonoConsortBusinessSkillUnlock>
    {
        private static GGUIWndConsortBusinessSkillUnlock _g_instance;
        public static GGUIWndConsortBusinessSkillUnlock instance { get { return _g_instance ??= new GGUIWndConsortBusinessSkillUnlock(); } }
        
        private ConsortBusinessSkillRefObj _m_skillRef;//技能配表数据
        private GGottenConsortInfo _m_iConsortInfo;
        private Action _m_aExecuteClose;//执行关闭方法
        
        private NPGGuiWndTexture _m_skillIcon;//技能图标
        private NPGGUIWndCommonToggleEx _m_notTodayToggle;//今日不再提示toggle
        
        public GGUIWndConsortBusinessSkillUnlock() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoConsortBusinessSkillUnlock.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoConsortBusinessSkillUnlock.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        
        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            if (wnd.imgSkillIcon != null)
                _m_skillIcon = new NPGGuiWndTexture(wnd.imgSkillIcon);

            if (wnd.monoDontShowTodayToggle != null)
            {
                _m_notTodayToggle = new NPGGUIWndCommonToggleEx(wnd.monoDontShowTodayToggle);
                _m_notTodayToggle.clickDelegate += _onNotTodayToggleClick;
            }
            
            ALUGUICommon.combineBtnClick(wnd.btnSure, _onBtnSureClick);
            ALUGUICommon.combineBtnClick(wnd.btnGoto, _onBtnGotoClick);
        }
        
        protected override void _onDiscard()
        {
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnSure, _onBtnSureClick);
                ALUGUICommon.uncombineBtnClick(wnd.btnGoto, _onBtnGotoClick);
            }
            
            _m_skillIcon?.discard();
            _m_skillIcon = null;

            if (_m_notTodayToggle != null)
            {
                _m_notTodayToggle.clickDelegate -= _onNotTodayToggleClick;
                _m_notTodayToggle.discard();
                _m_notTodayToggle = null;    
            }
            
            _m_aExecuteClose = null;
        }
        
        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_skillIcon?.hideWnd();

            if (_m_notTodayToggle != null)
            {
                // 若勾选了今日不再提示，则保存设置
                if(_m_notTodayToggle.isOn)
                    AccountSettingMgr.instance.warningTipSaver.setTodayIgnoreWarningTip(ENPWarningType.CONSORT_BUSINESS_SKILL_UNLOCK);
                
                _m_notTodayToggle.hideWnd();
            }
        }

        protected override void _onReset()
        {
            _m_skillIcon?.discardTexture();
            _m_notTodayToggle?.resetWnd();
        }

        public void setDate(ConsortBusinessSkillRefObj _skillRefObj, GGottenConsortInfo _consortInfo, Action _executeClose)
        {
            _m_skillRef = _skillRefObj;
            _m_iConsortInfo = _consortInfo;
            _m_aExecuteClose = _executeClose;

            _refreshWnd();
        }
        
        private void _refreshWnd()
        {
            if(wnd == null || _m_skillRef == null || _m_iConsortInfo == null)
                return;

            ALUGUICommon.setLabelTxt(wnd.txtUnlockTip, TextTranslate.instance.getLanguage(TransKeyConst.consort_business_skillUnlockTip_num, _m_skillRef.unlock_need_intimacy));
            
            BasicAttrRefObj selectSKillAttrRefObj = GRefdataCoreMgr.instance.basicAttrRefCore.getRef((long) _m_skillRef.property);
            if (selectSKillAttrRefObj != null)
            {
                if (_m_skillIcon != null)
                {
                    _m_skillIcon.showWnd();
                    _m_skillIcon.setTexture(selectSKillAttrRefObj.icon);
                }

                _m_iConsortInfo.getBusinessSkillInfo(_m_skillRef.id, (_businessSkillInfo) =>
                {
                    long proAdd = (_businessSkillInfo?.proAdd ?? 100) / 100;
                    if (selectSKillAttrRefObj.type != ESpecAttrType.NONE)
                    {
                        // 设置当前效果描述
                        ALUGUICommon.setLabelTxt(wnd.txtSkillEffectDesc,
                            TextTranslate.instance.getLanguage(TransKeyConst.consort_business_skillEffectDesc_str_num, selectSKillAttrRefObj.name, proAdd));
                    }
                    else
                    {
                        // 设置当前效果描述
                        ALUGUICommon.setLabelTxt(wnd.txtSkillEffectDesc,
                            TextTranslate.instance.getLanguage(TransKeyConst.consort_business_skillEffectDescAllAttr_num, proAdd));
                    }
                });
            }

            if (_m_notTodayToggle != null)
            {
                _m_notTodayToggle.showWnd();
                _m_notTodayToggle.setSelected(!AccountSettingMgr.instance.warningTipSaver.needShowWarningTip(ENPWarningType.CONSORT_BUSINESS_SKILL_UNLOCK));
            }
        }

        /// <summary>
        /// 不再提示toggle被点击
        /// </summary>
        /// <param name="_toggle"></param>
        private void _onNotTodayToggleClick(NPGGUIWndCommonToggleEx _toggle)
        {
            if(_m_notTodayToggle == null)
                return;
            
            _m_notTodayToggle.setSelected(!_m_notTodayToggle.isOn);
        }
        
        /// <summary>
        /// 确认按钮点击
        /// </summary>
        private void _onBtnSureClick(GameObject _go)
        {
            _m_aExecuteClose?.Invoke();
        }
        
        private void _onBtnGotoClick(GameObject _go)
        {
            _m_aExecuteClose?.Invoke();
            WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_CONSORT_DETAIL_TAB, EUnLockConsortDetailWndTabType.BUSINESS);
        }
    }
}