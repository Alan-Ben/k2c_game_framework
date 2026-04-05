using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public partial class ChildViewMgr
    {
        [NotNull] private readonly _TALSimpleStateMachine<ChildViewMgrType> _m_stateMachine;
        
        [ItemNotNull, NotNull] private readonly List<SeatInfo> _m_seatList;
        private SeatInfo _m_curSelectSeatInfo;
        // 教室的对象
        private ClassroomView _m_classroomView;
        // 是否初始化和当前的序列号
        private bool _m_isInit;
        private int _m_initSerialize;


        public ChildViewMgr(SeatInfo _defaultSeatInfo)
        {
            _m_stateMachine = new _TALSimpleStateMachine<ChildViewMgrType>();
            _m_stateMachine.changeState(new ChildViewMgrStateNone());
            
            _m_seatList = new List<SeatInfo>();
            _m_curSelectSeatInfo = _defaultSeatInfo;
        }
        
        
        /// <summary>
        /// 当前的座位列表
        /// </summary>
        /// <remarks>
        /// 为方便使用开放出去，外部应该只读
        /// </remarks>
        [ItemNotNull, NotNull] public List<SeatInfo> seatList { get { return _m_seatList; } }
        /// <summary>
        /// 当前选中的座位
        /// </summary>
        public SeatInfo curSelectSeatInfo { get { return _m_curSelectSeatInfo; } }
        /// <summary>
        /// 当前的状态
        /// </summary>
        public ChildViewMgrType curState { get { return _m_stateMachine.curState.state; } }


        internal void refreshClassroomView()
        {
            if (_m_classroomView != null && _m_curSelectSeatInfo != null)
            {
                _m_classroomView.updateSeatInfo(_m_curSelectSeatInfo);
            }
        }
        

        public void init(Action _complete)
        {
            if (_m_isInit)
            {
                _complete?.Invoke();
                return;
            }
            _m_isInit = true;
            
            // 设置当前初始化的序列号
            int serialize = _m_initSerialize;

            // 获取所有座位，如果没有默认选中的座位，就选中第一个有子嗣存在的
            NPPlayer.instance.childComp.getSeatListNonAlloc(_m_seatList);
            _m_curSelectSeatInfo ??= _m_seatList.GetFirst(_info => _info.childInfo == null);
            _m_curSelectSeatInfo ??= _m_seatList.GetFirst();
            
            // 通知 UI 刷新成本管理器的样子
            GGUIWndChildMain.instance.refreshWnd(this);
            // 如果当前存在选中的座位，根据座位情况展示教室
            _m_classroomView = new ClassroomView();
            _m_classroomView.load(setInitDone);

            // 初始化完成后的调用
            void setInitDone()
            {
                // 如果序列号不一致，说明这次初始化已经被丢弃了
                if (serialize != _m_initSerialize)
                {
                    _complete?.Invoke();
                    return;
                }

                refreshClassroomView();
                _m_stateMachine.changeState(new ChildViewMgrStateIdle(this));
                
                _complete?.Invoke();
            }
        }
        public void discard()
        {
            if (!_m_isInit)
                return;
            
            _m_isInit = false;
            _m_initSerialize = ALSerializeOpMgr.next();
            
            _m_stateMachine.changeState(new ChildViewMgrStateNone());

            _m_classroomView?.discard();
            _m_classroomView = null;
        }


        public void selectSeat(SeatInfo _seatInfo)
        {
            if (_m_curSelectSeatInfo == _seatInfo)
                return;
            
            _m_curSelectSeatInfo = _seatInfo;
            refreshClassroomView();
            
            // 切换为 idle 状态，停止其它的操作
            _m_stateMachine.changeState(new ChildViewMgrStateIdle(this));
        }
        public void setName(string _name, Action<bool> _complete)
        {
            ChildInfo childInfo = _m_curSelectSeatInfo?.childInfo;
            if (childInfo == null)
            {
                ALLog.Error("[ChildViewMgr] setName: childInfo is null!]");
                _complete?.Invoke(false);
                return;
            }
            
            int maskSerialize = MainCameraMono.selfInstance.openAllInputMask();
            NPPlayer.instance.childComp.reqSetChildName(childInfo.id, _name, _isSuc =>
            {
                GGUIWndChildMain.instance.refreshWnd();
                MainCameraMono.selfInstance.closeAllInputMask(maskSerialize);
                _complete?.Invoke(_isSuc);
            });
        }
        public void oneKeyNaming()
        {
            if (_m_seatList.Count == 0)
                return;

            int serialize = MainCameraMono.selfInstance.openAllInputMask();
            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(_m_seatList.Count);
            stepCounter.regAllDoneDelegate(() =>
            {
                GGUIWndChildMain.instance.refreshWnd();
                MainCameraMono.selfInstance.closeAllInputMask(serialize);
            });

            foreach (SeatInfo seatInfo in _m_seatList)
            {
                ChildInfo childInfo = seatInfo.childInfo;
                if (childInfo == null)
                {
                    stepCounter.addDoneStepCount();
                    continue;
                }

                if (!string.IsNullOrEmpty(childInfo.name))
                {
                    stepCounter.addDoneStepCount();
                    continue;
                }
                
                NPPlayer.instance.childComp.reqSetChildName(childInfo.id, GRefdataCoreMgr.instance.getChildRandomName(childInfo.sex), _isSuc => stepCounter.addDoneStepCount());
            }
        }
        public void graduate()
        {
            ChildInfo childInfo = _m_curSelectSeatInfo?.childInfo;
            if (childInfo == null)
            {
                ALLog.Error("[ChildViewMgr] graduate: childInfo is null!]");
                return;
            }
            
            int maskSerialize = MainCameraMono.selfInstance.openAllInputMask();
            NPPlayer.instance.childComp.reqSetChildGraduate(childInfo.id, (_isSuc, _msg) =>
            {
                if (_isSuc)
                {
                    refreshClassroomView();
                    GGUIWndChildMain.instance.refreshWnd();

                    UnmarriedInfo unmarriedInfo = new UnmarriedInfo(_msg.getAdult());
                    if (AccountSettingMgr.instance.warningTipSaver.needShowWarningTip(ENPWarningType.CHILD_GRADUATE_SUCCESS))
                        NPUINoticeMgr.instance.addDealer(new NoticeDealer_ChildGraduateSuccess(unmarriedInfo.adultInfo));
                    
                    if (unmarriedInfo.adultInfo.isSuper)
                        NPPlayer.instance.dinnerComp.showGiftdeChildPermitGetNotice(unmarriedInfo.adultInfo.id);
                    
                    NPUINoticeMgr.instance.addDealer(new NoticeDealer_ChildGraduateResult(unmarriedInfo.adultInfo, _msg.getGainItemList()?.GetFirst()?.toCommonItemData()));
                }

                MainCameraMono.selfInstance.closeAllInputMask(maskSerialize);
            });    
        }
        public void oneKeyGraduate()
        {
            if (_m_seatList.Count == 0)
                return;

            bool allFailed = true;
            List<_IChildInfo> childInfoList = new List<_IChildInfo>();
            List<_IItem> presentItemList = new List<_IItem>();
            int serialize = MainCameraMono.selfInstance.openAllInputMask();
            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(_m_seatList.Count + 1);
            stepCounter.regAllDoneDelegate(() =>
            {
                if (!allFailed)
                {
                    refreshClassroomView();

                    GGUIWndChildMain.instance.refreshWnd();
                    foreach (var adultInfo in childInfoList)
                    {
                        if (adultInfo != null && adultInfo.isSuper)
                            NPPlayer.instance.dinnerComp.showGiftdeChildPermitGetNotice(adultInfo.id);
                    }
                    
                    NPUINoticeMgr.instance.addDealer(new NoticeDealer_ChildMultiGraduateResult(childInfoList, presentItemList));
                }

                MainCameraMono.selfInstance.closeAllInputMask(serialize);
            });
            
            foreach (SeatInfo seatInfo in _m_seatList)
            {
                ChildInfo childInfo = seatInfo.childInfo;
                if (childInfo == null)
                {
                    stepCounter.addDoneStepCount();
                    continue;
                }

                if (!childInfo.canGraduate())
                {
                    stepCounter.addDoneStepCount();
                    continue;
                }
                
                NPPlayer.instance.childComp.reqSetChildGraduate(childInfo.id, (_isSuc, _msg) =>
                {
                    if (_isSuc)
                    {
                        allFailed = false;
                        UnmarriedInfo unmarriedInfo = new UnmarriedInfo(_msg.getAdult());
                        childInfoList.Add(unmarriedInfo.adultInfo);
                        presentItemList.Add(_msg?.getGainItemList()?.GetFirst()?.toCommonItemData());
                    }
                    stepCounter.addDoneStepCount();
                });
            }
            
            stepCounter.addDoneStepCount();
        }
        public void educate(Vector2 _clickScreenPos, Action _onStartReqTrain = null, bool _needShowTip = true)
        {
            ChildInfo childInfo = _m_curSelectSeatInfo?.childInfo;
            if (childInfo == null)
                return;

            if (string.IsNullOrEmpty(childInfo.name))
                return;
            
            if (_m_curSelectSeatInfo.energy <= 0)
            {
                if (_needShowTip)
                {
                    GGUIWndChildBrainValueGet.instance.refreshWnd(this);
                    QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndChildBrainValueGet.instance, GGUIWndChildBrainValueGet.instance.showWnd);
                }
                return;
            }

            if (!GCommon.isItemEnough(childInfo.getEducationCost(), _needShowTip))
                return;

            if (childInfo.canGraduate())
                return;

            _onStartReqTrain?.Invoke();
            int maskSerialize = MainCameraMono.selfInstance.openAllInputMask();
            NPPlayer.instance.childComp.reqTrainChild(childInfo.id, (_isSuc, _msg) =>
            {
                if (_isSuc)
                {
                    if (childInfo.canGraduate())
                    {
                        GGUIWndChildMain.instance.refreshWnd();
                    }
                    else
                    {
                        if (childInfo.canStepUp())
                            _m_stateMachine.changeState(new ChildViewMgrStateStepUp(this));
                        else
                        {
                            GGUIWndChildMain.instance.refreshProgress();
                            GGUIWndChildMain.instance.refreshBrainValue();
                        }
                    }

                    GGUIWndChildMain.instance.showExpCollect(_clickScreenPos, _msg.getGainValue());
                    _m_classroomView?.playEducateSfx();
                    GGUIWndChildMain.instance.playEducateSfx(childInfo);
                    if (_msg.getAddBonus() > 0)
                        GGUIWndChildMain.instance.refreshChildInfo();
                    GGUIWndChildMain.instance.refreshContainerItem(childInfo);
                }

                MainCameraMono.selfInstance.closeAllInputMask(maskSerialize);
            });
        }
        public void oneKeyEducating()
        {
            ChildInfo childInfo = _m_curSelectSeatInfo?.childInfo;
            if (childInfo == null)
                return;
            
            if (_m_curSelectSeatInfo.energy <= 0)
            {
                GGUIWndChildBrainValueGet.instance.refreshWnd(this);
                QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndChildBrainValueGet.instance, GGUIWndChildBrainValueGet.instance.showWnd);
                return;
            }
            
            if (!GCommon.isItemEnough(childInfo.getEducationCost(), true))
                return;
            
            _m_stateMachine.changeState(new ChildViewMgrStateOneKeyTrain(this));
        }
        public void oneKeyEducatingPlus()
        {
            bool isAllEnergyEmpty = true;
            bool noChildren = true;
            foreach (SeatInfo seatInfo in _m_seatList)
            {
                if (seatInfo.childInfo != null && !string.IsNullOrEmpty(seatInfo.childInfo.name) && !seatInfo.childInfo.canGraduate())
                {
                    noChildren = false;
                    if (seatInfo.energy > 0)
                    {
                        isAllEnergyEmpty = false;
                        break;
                    }
                }
            }
            
            if (curSelectSeatInfo?.childInfo != null && !GCommon.isItemEnough(curSelectSeatInfo.childInfo.getEducationCost(), true))
                return;

            if (noChildren)
            {
                ALLog.Error("没有子嗣存在，无法开启高级一键上课");
                return;
            }

            if (isAllEnergyEmpty)
            {
                GGUIWndChildBrainValueGet.instance.refreshWnd(this);
                QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndChildBrainValueGet.instance, GGUIWndChildBrainValueGet.instance.showWnd);
                return;
            }
            
            _m_stateMachine.changeState(new ChildViewMgrStateOneKeyTrainAll(this));
        }
        public void stopOneKeyEducating()
        {
            if (curState is not (ChildViewMgrType.OneKeyTrain or ChildViewMgrType.OneKeyTrainAll))
                return;
            
            _m_stateMachine.changeState(new ChildViewMgrStateIdle(this));
        }
        public void recoverEnergy(int _count, Action<bool> _complete)
        {
            if (_m_curSelectSeatInfo == null)
            {
                _complete?.Invoke(false);
                return;
            }
            
            int maskSerialize = MainCameraMono.selfInstance.openAllInputMask();
            long seatId = _m_curSelectSeatInfo.id;
            NPPlayer.instance.childComp.reqAddSeatEnergy(seatId, _count, _isSuc =>
            {
                if (_isSuc)
                {
                    GGUIWndChildMain.instance.refreshWnd();
                    NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.child_energyRecoverSuccess_none);
                    GGUIWndChildMain.instance.showEnergyRecover(seatId, _count);
                }
                MainCameraMono.selfInstance.closeAllInputMask(maskSerialize);
                _complete?.Invoke(_isSuc);
            });
        }
        public void recoverAllEnergy(Action _complete)
        {
            if (_m_seatList.Count == 0)
            {
                _complete?.Invoke();
                return;
            }
            
            List<_RecoverData> recoverDataList = new List<_RecoverData>();
            int maskSerialize = MainCameraMono.selfInstance.openAllInputMask();
            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(_m_seatList.Count + 1);
            stepCounter.regAllDoneDelegate(() =>
            {
                GGUIWndChildMain.instance.refreshWnd();
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.child_energyRecoverSuccess_none);
                foreach (_RecoverData data in recoverDataList)
                    GGUIWndChildMain.instance.showEnergyRecover(data.seatId, data.count);
                MainCameraMono.selfInstance.closeAllInputMask(maskSerialize);
                _complete?.Invoke();
            });
            
            foreach (SeatInfo seatInfo in _m_seatList)
            {
                if (seatInfo.childInfo == null)
                {
                    stepCounter.addDoneStepCount();
                    continue;
                }
                
                int count = seatInfo.childInfo.getGraduatingEnergyNeed();
                if (count <= 0)
                {
                    stepCounter.addDoneStepCount();
                    continue;
                }
                
                recoverDataList.Add(new _RecoverData { seatId = seatInfo.id, count = count });
                NPPlayer.instance.childComp.reqAddSeatEnergy(seatInfo.id, count, _isSuc => stepCounter.addDoneStepCount());
            }
            
            stepCounter.addDoneStepCount();
        }
        
        private struct _RecoverData
        {
            public long seatId;
            public int count;
        }
    }
}