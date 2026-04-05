using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIWndConsortStoryGridItem : _ANPGGUIBasicGridItemWnd<GGUIMonoConsortStoryGridItem>
    {
        private ConsortStoryRefObj _m_consortStoryRefObj;//故事配表数据
        private GGottenConsortInfo _m_iGottenConsortInfo;//已获得妃子信息
        private Action _m_aDealCloseWnd;//关闭窗口方法
        
        public GGUIWndConsortStoryGridItem(GGUIMonoConsortStoryGridItem _wnd) : base(_wnd)
        {
        }

        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;
         
            ALUGUICommon.combineBtnClick(wnd.lookBtn, _onLookBtnClick);
        }
        
        protected override void _onDiscard()
        {
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.lookBtn, _onLookBtnClick);
            }
            
            _m_aDealCloseWnd = null;
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

        protected override void _resetGridItem()
        {
        }
        
        public void setData(ConsortStoryRefObj _consortStoryRefObj, GGottenConsortInfo _consortInfo, Action _onDealCloseWnd = null)
        {
            _m_consortStoryRefObj = _consortStoryRefObj;
            _m_iGottenConsortInfo = _consortInfo;
            _m_aDealCloseWnd = _onDealCloseWnd;
            
            _refreshWnd();
        }

        /// <summary>
        /// 刷新窗口
        /// </summary>
        private void _refreshWnd()
        {
            if(_m_consortStoryRefObj == null || wnd == null)
                return;

            ALUGUICommon.setLabelTxt(wnd.storyName, TextTranslate.instance.getLanguage(_m_consortStoryRefObj.story_name));
         
            ALUGUICommon.setGameObjEnable(wnd.hasCGShow, _m_consortStoryRefObj.unlock_cg > 0);
            ALUGUICommon.setGameObjEnable(wnd.hasCGHide, _m_consortStoryRefObj.unlock_cg <= 0);

            if (!string.IsNullOrEmpty(_m_consortStoryRefObj.unlock_condition_desc))
            {
                ALUGUICommon.setGameObjEnable(wnd.txtUnlockConditionDesc, true);

                ALUGUICommon.setLabelTxt(wnd.txtUnlockConditionDesc,
                    TextTranslate.instance.getLanguage(_m_consortStoryRefObj.unlock_condition_desc,
                        _m_consortStoryRefObj.unlock_condition_desc_args_list));
            }
            else
            {
                ALUGUICommon.setGameObjEnable(wnd.txtUnlockConditionDesc, false);
            }

            if (!string.IsNullOrEmpty(_m_consortStoryRefObj.unlock_progress_desc))
            {
                ALUGUICommon.setGameObjEnable(wnd.txtUnlockProgress, true);
                ALUGUICommon.setLabelTxt(wnd.txtUnlockProgress, TextTranslate.instance.getLanguage(_m_consortStoryRefObj.unlock_progress_desc, _m_consortStoryRefObj.unlock_progress_desc_args_list));
            }
            else
            {
                ALUGUICommon.setGameObjEnable(wnd.txtUnlockProgress, false);
            }

            if (!string.IsNullOrEmpty(_m_consortStoryRefObj.trigger_way_desc))
            {
                ALUGUICommon.setGameObjEnable(wnd.txtTriggerWayDesc, true);
                ALUGUICommon.setLabelTxt(wnd.txtTriggerWayDesc, TextTranslate.instance.getLanguage(_m_consortStoryRefObj.trigger_way_desc, _m_consortStoryRefObj.trigger_way_desc_args_list));
            }
            else
            {
                ALUGUICommon.setGameObjEnable(wnd.txtTriggerWayDesc, false);
            }
            
            wnd.setStoryState(_m_consortStoryRefObj.getStoryState(_m_iGottenConsortInfo));
        }

        private void _onLookBtnClick(GameObject _go)
        {
            if(_m_consortStoryRefObj == null)
                return;

            EConsortStoryState storyState = _m_consortStoryRefObj.getStoryState(_m_iGottenConsortInfo);

            switch (storyState)
            {
                case EConsortStoryState.LOCK://未解锁
                    if (_m_consortStoryRefObj.unlock_hint_show_type == EHintShowWay.TIP)
                    {
                        if (!string.IsNullOrEmpty(_m_consortStoryRefObj.unlock_condition_desc))
                        {
                            NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(_m_consortStoryRefObj.unlock_condition_desc, _m_consortStoryRefObj.unlock_condition_desc_args_list));
                        }
                        else
                        {
                            _m_consortStoryRefObj.unlock_way_goto_effect?.dealEffect(null);
                            _m_aDealCloseWnd?.Invoke();
                        }
                    }
                    else if (_m_consortStoryRefObj.unlock_hint_show_type == EHintShowWay.POP_WND)
                    {
                        NPMesMgr.instance.showTwoBtnMes(TextTranslate.instance.getLanguage(_m_consortStoryRefObj.unlock_condition_desc, _m_consortStoryRefObj.unlock_condition_desc_args_list)
                            , TextTranslate.instance.getLanguage(TransKeyConst.cancel), ()=>{}, 
                            TextTranslate.instance.getLanguage(_m_consortStoryRefObj.unlock_way_goto_btn_desc), () =>
                            {
                                _m_consortStoryRefObj.unlock_way_goto_effect?.dealEffect(null);
                                _m_aDealCloseWnd?.Invoke();
                            });
                    }
                    else
                    {
                        _m_consortStoryRefObj.unlock_way_goto_effect?.dealEffect(null);
                        _m_aDealCloseWnd?.Invoke();
                    }
                    break;
                
                case EConsortStoryState.UNLOCK_TRIGGERED:
                    // 已解锁并且已触发 直接查看对话
                    GCommon.enterDialogueNode(_m_consortStoryRefObj.dialogue_id, () => { }, true);
                    break;
                
                case EConsortStoryState.UNLOCK_NOT_TRIGGERED_YET:
                    // 已解锁未触发
                    if (_m_consortStoryRefObj.trigger_way_hint_show_type == EHintShowWay.TIP)
                    {
                        if (!string.IsNullOrEmpty(_m_consortStoryRefObj.trigger_way_desc))
                        {
                            NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(_m_consortStoryRefObj.trigger_way_desc, _m_consortStoryRefObj.trigger_way_desc_args_list));
                        }
                        else
                        {
                            _m_consortStoryRefObj.trigger_way_goto_effect?.dealEffect(null);
                            _m_aDealCloseWnd?.Invoke();
                        }
                    }
                    else if (_m_consortStoryRefObj.trigger_way_hint_show_type == EHintShowWay.POP_WND)
                    {
                        NPMesMgr.instance.showTwoBtnMes(TextTranslate.instance.getLanguage(_m_consortStoryRefObj.trigger_way_desc, _m_consortStoryRefObj.trigger_way_desc_args_list)
                            , TextTranslate.instance.getLanguage(TransKeyConst.cancel), ()=>{}, 
                            TextTranslate.instance.getLanguage(_m_consortStoryRefObj.trigger_way_goto_btn_desc), () =>
                            {
                                _m_consortStoryRefObj.trigger_way_goto_effect?.dealEffect(null);
                                _m_aDealCloseWnd?.Invoke();
                            });
                    }
                    else
                    {
                        _m_consortStoryRefObj.trigger_way_goto_effect?.dealEffect(null);
                        _m_aDealCloseWnd?.Invoke();
                    }
                    break;
            }
        }
    }
}