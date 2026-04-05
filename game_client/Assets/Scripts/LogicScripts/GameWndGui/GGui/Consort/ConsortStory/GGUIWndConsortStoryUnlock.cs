using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 妃子故事解锁弹窗
    /// </summary>
    public class GGUIWndConsortStoryUnlock : _ANPGGUIBasicWnd<GGUIMonoConsortStoryUnlock>
    {
        private static GGUIWndConsortStoryUnlock _g_instance;
        public static GGUIWndConsortStoryUnlock instance { get { return _g_instance ??= new GGUIWndConsortStoryUnlock(); } }
        
        private ConsortStoryRefObj _m_storyRefObj;//故事配表数据
        private Action _m_aExecuteClose;//执行关闭方法
        
        public GGUIWndConsortStoryUnlock() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoConsortStoryUnlock.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoConsortStoryUnlock.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        
        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;
         
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

            _m_aExecuteClose = null;
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

        public void setData(ConsortStoryRefObj _storyRefObj, Action _executeClose)
        {
            _m_storyRefObj = _storyRefObj;
            _m_aExecuteClose = _executeClose;

            _refreshWnd();
        }

        private void _refreshWnd()
        {
            if(wnd == null || _m_storyRefObj == null)
                return;

            string unlockTip = string.Empty;
            string unlockConditionDesc = TextTranslate.instance.getLanguage(_m_storyRefObj.unlock_condition_desc, _m_storyRefObj.unlock_condition_desc_args_list);
            if (_m_storyRefObj.default_trigger)//若是默认触发
            {
                unlockTip = string.IsNullOrEmpty(wnd.storyTriggerNeedlessUnlockTipKey) ? "{0}" : wnd.storyTriggerNeedlessUnlockTipKey;
                ALUGUICommon.setLabelTxt(wnd.txtUnlockTip, TextTranslate.instance.getLanguage(unlockTip, TextTranslate.instance.getLanguage(unlockConditionDesc)));
            }
            else
            {
                unlockTip = string.IsNullOrEmpty(wnd.storyTriggerNeedfulUnlockTipKey) ? "{0}, {1}" : wnd.storyTriggerNeedfulUnlockTipKey;
                string triggerWayDesc = TextTranslate.instance.getLanguage(_m_storyRefObj.trigger_way_desc, _m_storyRefObj.trigger_way_desc_args_list);
                ALUGUICommon.setLabelTxt(wnd.txtUnlockTip, TextTranslate.instance.getLanguage(unlockTip, unlockConditionDesc, triggerWayDesc));
            }

            ALUGUICommon.setLabelTxt(wnd.txtStoryName, TextTranslate.instance.getLanguage(_m_storyRefObj.story_name));
        }
        
        private void _onBtnSureClick(GameObject _go)
        {
            _m_aExecuteClose?.Invoke();
        }
        
        private void _onBtnGotoClick(GameObject _go)
        {
            _m_aExecuteClose?.Invoke();
            WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_CONSORT_DETAIL_TAB, EUnLockConsortDetailWndTabType.INTERACTION, EUnlockConsortDetailWndInteractionPageTabType.STORY);
        }
    }
}