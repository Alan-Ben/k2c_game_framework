using System;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// item
    /// </summary>
    public class GGUIWndStepRewardTabItem : _ATALBasicUISubWnd<GGUIMonoStepRewardTabItem>, _IContainerSideRedTipItemInfo
    {
        private ActivityStepRewardInfo _m_stepRewardInfo = null;
        private Action<GGUIWndStepRewardTabItem> _m_onClickItemCallBack = null;
        private NPGGUIWndCommonTab _m_wTab = null;
        
        public ActivityStepRewardInfo stepRewardInfo
        {
            get { return _m_stepRewardInfo; }
        }        
        
        /// <summary>
        /// 是否有红点提示
        /// </summary>
        public bool haveRedTip
        {
            get
            {
                if (_m_stepRewardInfo != null)
                    return _m_stepRewardInfo.getCanGetRewardCount() > 0;
                return false;
            }
        }

        public GGUIWndStepRewardTabItem(GGUIMonoStepRewardTabItem _wnd, Action<GGUIWndStepRewardTabItem> _onClickItemCallBack) : base(_wnd)
        {
            _m_onClickItemCallBack = _onClickItemCallBack;
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
            if (wnd.monoTab != null)
            {
                _m_wTab = new NPGGUIWndCommonTab(wnd.monoTab);
                _m_wTab.clickDelegate += _onTabClick;
            }
        }

        public void setInfo(ActivityStepRewardInfo _data, bool _isFirst)
        {
            _m_stepRewardInfo = _data;
            if (wnd != null)
            {
                ALUGUICommon.setGameObjEnable(wnd.goFirstHideList, !_isFirst);
                ALUGUICommon.setGameObjEnable(wnd.goFirstShowList, _isFirst);
            }

            _refreshWnd();
        }
        
        public void setSelected(bool _isOn)
        {
            if (null == wnd)
                return;
            if (_m_wTab != null)
            {
                _m_wTab.setSelected(_isOn);
            }
        }

        private void _refreshWnd()
        {
            if (null == wnd)
                return;
            ActivityStepRewardSetRefObj stepRef = GRefdataCoreMgr.instance.stepRewardSetRefCore.getRef(_m_stepRewardInfo.stepRewardSetId);
            if (stepRef != null)
            {
                string stepName = TextTranslate.instance.getLanguage(stepRef.step_reward_set_name);
                ALUGUICommon.setLabelTxt(wnd.txtName, stepName);
                ALUGUICommon.setLabelTxt(wnd.txtName2, stepName);
                
            }
            _m_wTab?.showRedTipNum(_m_stepRewardInfo.getCanGetRewardCount());
            
        }
        private void _onTabClick(bool tab)
        {
            if (null == wnd)
                return;
            _m_onClickItemCallBack?.Invoke(this);
        }
    }
}
