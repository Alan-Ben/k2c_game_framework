using ALPackage;
using Common.ChildObj;
using CommonEnum;
using JetBrains.Annotations;
using NPCommon;
using NPEnum;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GOE
{
    public partial class GGUIWndChildMain
    {
        private class GGUIWndChildMainEducatingShow
        {
            [NotNull] private readonly GGUIMonoChildMainEducatingShow _m_wnd;
            private bool _m_bIsShow;

            private NPGGUIWndCommonItem _m_costItem;
            private NPGGUIWndCommonToggleEx _m_oneKeyEducatingToggle;
            private GGUIWndChildVoiceBubble _m_voiceBubbleWnd;

            private ChildViewMgr _m_viewMgr;
            private ALCommonEnableTaskController _m_brainRecoverTimeTask;
            // 当前选中座位信息
            private SeatInfo _m_curSeatInfo;
            // 点击上课按钮计数，用于触发语音和气泡
            private int _m_iClickEducateCount;
            // 长按上课按钮计时器
            private float _m_longPressTimeCounter;
            private ALCommonEnableTaskController _m_longPressTimeTask;
            // 记录当前子嗣上个阶段
            private int _m_iLastStep;



            public GGUIWndChildMainEducatingShow([NotNull] GGUIMonoChildMainEducatingShow _wnd)
            {
                _m_wnd = _wnd;
                initWnd();
            }
            
            
            public void showWnd()
            {
                WinMsg.RegisterMsgAct(WinMsgType.SIMULATE_CLICK_TRAIN_CHILD, _onSimulateClickTrainChild);
                WinMsg.RegisterMsgAct(WinMsgType.SIMULATE_CLICK_CHILD_ONE_KEY_TRAIN_TOGGLE, _onSimulateClickOneKeyTrainToggle);
                WinMsg.RegisterMsgAct(WinMsgType.SIMULATE_CLICK_OPEN_CHILD_ONE_KEY_PLUS_WND, _onSimulateClickOpenOneKeyPlusWnd);
                WinMsg.RegisterMsg(WinMsgType.ON_PLAYER_RES_CHANGE, _onPlayerResChg);
                WinMsg.RegisterMsgAct(WinMsgType.ON_NODE_CHG, _onNodeChg);
                NPPlayer.instance.childComp.onChildLevelChg += _onChildLevelChg;
                _m_bIsShow = true;
                _m_iLastStep = -1;

                _m_costItem?.showWnd();
                _m_oneKeyEducatingToggle?.showWnd();

                refreshWnd();
            }  
            public void hideWnd()
            {
                WinMsg.UnregisterMsgAct(WinMsgType.SIMULATE_CLICK_TRAIN_CHILD, _onSimulateClickTrainChild);
                WinMsg.UnregisterMsgAct(WinMsgType.SIMULATE_CLICK_CHILD_ONE_KEY_TRAIN_TOGGLE, _onSimulateClickOneKeyTrainToggle);
                WinMsg.UnregisterMsgAct(WinMsgType.SIMULATE_CLICK_OPEN_CHILD_ONE_KEY_PLUS_WND, _onSimulateClickOpenOneKeyPlusWnd);
                WinMsg.UnregisterMsg(WinMsgType.ON_PLAYER_RES_CHANGE, _onPlayerResChg);
                WinMsg.UnregisterMsgAct(WinMsgType.ON_NODE_CHG, _onNodeChg);
                NPPlayer.instance.childComp.onChildLevelChg -= _onChildLevelChg;
                _m_costItem?.hideWnd();
                _m_oneKeyEducatingToggle?.hideWnd();
                _m_voiceBubbleWnd?.hideWnd();
                _m_longPressTimeTask.setDisable();

                _m_curSeatInfo = null;
                _m_bIsShow = false;
                _m_iClickEducateCount = 0;
                _m_iLastStep = -1;
            }

            public void resetWnd()
            {
                _m_costItem?.resetWnd();
                _m_oneKeyEducatingToggle?.resetWnd();
                _m_voiceBubbleWnd?.resetWnd();
            }
            public void discard()
            {
                _m_costItem?.discard();
                _m_oneKeyEducatingToggle?.discard();
                _m_voiceBubbleWnd?.discard();
                _m_costItem = null;
                _m_oneKeyEducatingToggle = null;
                _m_voiceBubbleWnd = null;
                _m_brainRecoverTimeTask.setDisable();
                
                ALUGUICommon.uncombineBtnClick(_m_wnd.btnEducating, _onBtnEducatingClick);
                ALUGUICommon.uncombineBtnClick(_m_wnd.btnEducating2, _onBtnEducatingClick);
                ALUGUICommon.uncombineBtnLongPress(_m_wnd.btnEducating, _onBtnEducatingLongPress);
                ALUGUICommon.uncombineBtnLongPress(_m_wnd.btnEducating2, _onBtnEducatingLongPress);
                ALUGUICommon.uncombineBtnPress(_m_wnd.btnEducating, _onBtnEducatingPress);
                ALUGUICommon.uncombineBtnPress(_m_wnd.btnEducating2, _onBtnEducatingPress);
                ALUGUICommon.uncombineBtnClick(_m_wnd.btnBrainAdd, _onBtnBrainAddClick);
                ALUGUICommon.uncombineBtnClick(_m_wnd.btnOneKeyPlusBtn, _onBtnOneKeyPlusBtnClick);
                ALUGUICommon.uncombineBtnClick(_m_wnd.btnStopOneKeyEducating, _onBtnStopOneKeyEducatingClick);
            }
            public void initWnd()
            {
                if (_m_wnd.monoCostItem != null)
                    _m_costItem = new NPGGUIWndCommonItem(_m_wnd.monoCostItem);
                if (_m_wnd.monoOneKeyEducatingToggle != null)
                {
                    _m_oneKeyEducatingToggle = new NPGGUIWndCommonToggleEx(_m_wnd.monoOneKeyEducatingToggle);
                    _m_oneKeyEducatingToggle.clickDelegate = _onOneKeyEducatingToggleClick;
                }
                if (_m_wnd.monoVoiceBubble != null)
                    _m_voiceBubbleWnd = new GGUIWndChildVoiceBubble(_m_wnd.monoVoiceBubble);

                _m_brainRecoverTimeTask = ALCommonEnableDurationActionMonoTask.addMonoTask(_onRecoverTimeChg, 1f);
                
                ALUGUICommon.combineBtnClick(_m_wnd.btnEducating, _onBtnEducatingClick);
                ALUGUICommon.combineBtnClick(_m_wnd.btnEducating2, _onBtnEducatingClick);
                ALUGUICommon.combineBtnLongPress(_m_wnd.btnEducating, _onBtnEducatingLongPress);
                ALUGUICommon.combineBtnLongPress(_m_wnd.btnEducating2, _onBtnEducatingLongPress);
                ALUGUICommon.combineBtnPress(_m_wnd.btnEducating, _onBtnEducatingPress);
                ALUGUICommon.combineBtnPress(_m_wnd.btnEducating2, _onBtnEducatingPress);
                ALUGUICommon.combineBtnClick(_m_wnd.btnBrainAdd, _onBtnBrainAddClick);
                ALUGUICommon.combineBtnClick(_m_wnd.btnOneKeyPlusBtn, _onBtnOneKeyPlusBtnClick);
                ALUGUICommon.combineBtnClick(_m_wnd.btnStopOneKeyEducating, _onBtnStopOneKeyEducatingClick);
            }


            public void refreshWnd(ChildViewMgr _viewMgr)
            {
                _m_viewMgr = _viewMgr;

                // 如果当前座位信息和选中座位信息不一致，说明选中座位发生变化，重置相关数据
                if (_m_curSeatInfo == null || _m_curSeatInfo.childInfo ==  null || _m_curSeatInfo.childInfo.id != _m_viewMgr?.curSelectSeatInfo?.childInfo?.id)
                {
                    _m_curSeatInfo = _m_viewMgr?.curSelectSeatInfo;
                    _m_voiceBubbleWnd?.hideWnd();
                    _m_iLastStep = _m_viewMgr?.curSelectSeatInfo?.childInfo?.step ?? -1;
                    _m_iClickEducateCount = 0;
                }

                refreshWnd();
            }
            public void refreshWnd()
            {
                SeatInfo seatInfo = _m_viewMgr?.curSelectSeatInfo;
                if (seatInfo == null || !_m_bIsShow)
                    return;

                _refreshCost();
                bool isOneKeyUnlock = GCommon.isSimpleUnlock(GRefdataCoreMgr.instance.npGeneral.child_one_key_educate_unlock_id);
                ALUGUICommon.setGameObjEnable(_m_wnd.listOneKeyLockShow, !isOneKeyUnlock);
                _m_oneKeyEducatingToggle?.setSelected(!isOneKeyUnlock
                    ? false
                    : AccountSettingMgr.instance.accountSetting.isChildOneKeyEducating);
                bool isOneKeyPlusUnlock = GCommon.isSimpleUnlock(GRefdataCoreMgr.instance.npGeneral.child_one_key_educate_plus_unlock_id);
                ALUGUICommon.setGameObjEnable(_m_wnd.listOneKeyPlusUnlockShow, isOneKeyPlusUnlock);

                refreshBrainValue();
                _m_wnd.setOneKeyState(_m_viewMgr.curState is ChildViewMgrType.OneKeyTrain or ChildViewMgrType.OneKeyTrainAll);
            }
            public void refreshBrainValue()
            {
                SeatInfo seatInfo = _m_viewMgr?.curSelectSeatInfo;
                if (seatInfo == null)
                    return;
                
                int energy = seatInfo.energy;
                int maxEnergy = seatInfo.maxEnergy;
                ALUGUICommon.setLabelTxt(_m_wnd.txtBrainValue, TextTranslate.instance.getLanguage(TransKeyConst.child_room_EP_num, energy, maxEnergy));
                _m_wnd.setBrainValue(energy, maxEnergy);
            }
            public void showExpCollect(Vector2 _screenPos, long _num)
            {
                if (_num <= 0)
                    return;
                
                NPGTextureIndex expIcon = GCommon.getItemTexIcon(ENPItemType.CURRENCY, (long) ECurrency.HERO_EXP);
                string text = TextTranslate.instance.getLanguage(TransKeyConst.common_add_num, _num.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT));
                NPGUIAddSceneCenterTip.instance.showIconTextTip(expIcon, text, _m_wnd.expCollectTipId, (_tipWnd) => { _tipWnd?.setTipPos(_screenPos); });

                //先将屏幕坐标转换为UI坐标
                GCommon.getParticleUIPosByScreenPos(_screenPos, _uiPos =>
                {
                    //播放粒子特效
                    if (_m_wnd != null)
                        GCommon.showItemParticle(new NPCommon_ItemInfo((int)ENPItemType.CURRENCY, (long)ECurrency.HERO_EXP, _num, null), _uiPos, _m_wnd.particleId);
                });
             }
            //刷新消耗
            private void _refreshCost()
            {
                SeatInfo seatInfo = _m_viewMgr?.curSelectSeatInfo;
                if (seatInfo == null || !_m_bIsShow)
                    return;

                NPCommonCostItem costItems = seatInfo.childInfo?.getEducationCost();
                _m_costItem?.showWnd(costItems);
                _m_wnd.setIsCostFree(costItems == null || costItems.getCount() <= 0);
            }
            //播放语音和气泡
            private void _showVoiceAndBubble()
            {
                if (_m_viewMgr == null)
                    return;

                EChildSexType sex = _m_viewMgr?.curSelectSeatInfo?.childInfo?.initResRef.sex ?? EChildSexType.BOY;
                EChildVoiceType curVoiceType = EChildVoiceType.NONE;
                if (_m_viewMgr.curSelectSeatInfo?.childInfo?.step == 0)
                    curVoiceType = EChildVoiceType.TEACH_STEP_1;
                else if(_m_viewMgr.curSelectSeatInfo?.childInfo?.step == 1)
                    curVoiceType = EChildVoiceType.TEACH_STEP_2;
                else if(_m_viewMgr.curSelectSeatInfo?.childInfo?.step == 2)
                    curVoiceType = EChildVoiceType.TEACH_STEP_3;

                // 没有可播放的语音则不处理
                List<long> voiceIdList = GRefdataCoreMgr.instance.getChildVoiceIdList(sex, curVoiceType);
                if (voiceIdList == null || voiceIdList.Count <= 0)
                    return;

                NPGTextureIndex icon = _m_viewMgr?.curSelectSeatInfo?.childInfo?.resRef?.icon;
                NPGTextureIndex cardIcon = _m_viewMgr?.curSelectSeatInfo?.childInfo?.resRef?.card_icon;
                NPGSpriteIndex iconBg = _m_viewMgr?.curSelectSeatInfo?.childInfo?.qualityRef?.head_bg;
                _m_voiceBubbleWnd?.showWnd();
                _m_voiceBubbleWnd?.setInfo(sex, cardIcon, icon, iconBg, curVoiceType);
            }
            
            private void _onBtnEducatingClick(GameObject _)
            {
                if (_m_viewMgr == null)
                    return;

                if (_m_oneKeyEducatingToggle is { isOn: true })
                {
                    // 重置点击次数及阶段记录
                    _m_iClickEducateCount = 0;
                    _m_iLastStep = _m_viewMgr.curSelectSeatInfo?.childInfo?.step ?? -1;
                    // 根据一键类型请求一键上课
                    if (AccountSettingMgr.instance.accountSetting.isChildOneKeyPlusEducating)
                        _m_viewMgr.oneKeyEducatingPlus();
                    else
                        _m_viewMgr.oneKeyEducating();
                    
                    return;
                }
                
                _m_viewMgr.educate(ALInputControl.instance.getBtnUnityPos(EALGUIOpButtonType.OP_BTN), () =>
                {
                    //开始请求上课，计算点击次数，达到触发条件则播放语音和气泡
                    _m_iClickEducateCount++;
                    if (_m_iClickEducateCount >= _m_wnd.triggerVoiceBubbleClickCount)
                    {
                        _m_iClickEducateCount = 0;
                        _showVoiceAndBubble();
                    }
                });
            }
            private void _onBtnEducatingLongPress()
            {
                _m_longPressTimeCounter = _m_wnd.longPressSpace;
                _m_longPressTimeTask.setDisable();
                _m_longPressTimeTask = ALCommonEnableTickActionMonoTask.addMonoTask(_onLongPressTimeTick);
            }
            private void _onBtnEducatingPress(bool _press, PointerEventData _)
            {
                if (!_press)
                    _m_longPressTimeTask.setDisable();
            }
            private void _onLongPressTimeTick()
            {
                _m_longPressTimeCounter += Time.deltaTime;
                if (_m_longPressTimeCounter >= _m_wnd.longPressSpace)
                {
                    _m_longPressTimeCounter = 0f;
                    _m_viewMgr?.educate(ALInputControl.instance.getBtnUnityPos(EALGUIOpButtonType.OP_BTN), () =>
                    {
                        //开始请求上课，计算点击次数，达到触发条件则播放语音和气泡
                        _m_iClickEducateCount++;
                        if (_m_iClickEducateCount >= _m_wnd.triggerVoiceBubbleClickCount)
                        {
                            _m_iClickEducateCount = 0;
                            _showVoiceAndBubble();
                        }
                    }, false);
                }
            }
            private void _onBtnBrainAddClick(GameObject _)
            {
                GGUIWndChildBrainValueGet.instance.refreshWnd(_m_viewMgr);
                QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndChildBrainValueGet.instance, GGUIWndChildBrainValueGet.instance.showWnd);
            }
            private void _onBtnOneKeyPlusBtnClick(GameObject _)
            {
                GGUIWndChildOneKeyEducationPlus.instance.refreshWnd();
                QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndChildOneKeyEducationPlus.instance, GGUIWndChildOneKeyEducationPlus.instance.showWnd, UINodeTagConst_Child.C_CHILD_ONE_KEY_EDUCATION_PLUS);
            }
            private void _onBtnStopOneKeyEducatingClick(GameObject _)
            {
                _m_viewMgr?.stopOneKeyEducating();
            }
            private void _onOneKeyEducatingToggleClick(NPGGUIWndCommonToggleEx _toggle)
            {
                if (_toggle == null)
                    return;
                
                NPSimpleUnlockRef unlockRef = GRefdataCoreMgr.instance.simpleUnlockMap.getRef(GRefdataCoreMgr.instance.npGeneral.child_one_key_educate_unlock_id);
                bool isOneKeyUnlock = unlockRef == null || unlockRef.isConditionEnable(null);
                if (!isOneKeyUnlock)
                {
                    NPGUIAddSceneCenterTip.instance.showTransTextInfo(unlockRef.getUnlockTip());
                    return;
                }
                
                _toggle.setSelected(!_toggle.isOn);
                AccountSettingMgr.instance.accountSetting.setChildOneKeyEducating(_toggle.isOn);
            }

            private long _m_lastRemainTime;
            private void _onRecoverTimeChg()
            {
                SeatInfo seatInfo = _m_viewMgr?.curSelectSeatInfo;
                if (seatInfo == null)
                    return;
                
                long remainTime = seatInfo.remainMs;
                ALUGUICommon.setLabelTxt(_m_wnd.txtRecoverTime, TextTranslate.instance.getLanguage(TransKeyConst.child_room_EP_countdown_desc, TimeUtil.millisecondsToTime_hms(remainTime)));
                if (remainTime > _m_lastRemainTime)
                    refreshBrainValue();
                
                _m_lastRemainTime = remainTime;
            }

            //模拟点击培养子嗣
            private void _onSimulateClickTrainChild()
            {
                _onBtnEducatingClick(null);
            }

            //模拟点击子嗣一键教学开关
            private void _onSimulateClickOneKeyTrainToggle()
            {
                _onOneKeyEducatingToggleClick(_m_oneKeyEducatingToggle);
            }

            //模拟点击打开子嗣进阶一键教学窗口
            private void _onSimulateClickOpenOneKeyPlusWnd()
            {
                _onBtnOneKeyPlusBtnClick(null);
            }

            // 子嗣等级变化回调
            private void _onChildLevelChg(ChildInfo _childInfo)
            {
                if (_childInfo == null || _childInfo.id != _m_viewMgr?.curSelectSeatInfo?.childInfo?.id)
                    return;

                // 如果阶段变化了，并且一键上课是开启状态，则播放语音和气泡
                if (_m_iLastStep < _childInfo.step && _m_oneKeyEducatingToggle is {isOn: true})
                {
                    _m_iLastStep = _childInfo.step;
                    _showVoiceAndBubble();
                }
            }

            private void _onPlayerResChg(params object[] _objects)
            {
                _refreshCost();
            }
            private void _onNodeChg()
            {
                // 有的时候打开新界面 press up 不会触发，这里监听一下双重保险
                _m_longPressTimeTask.setDisable();
            }
        }
    }
}