
using System;
using System.Collections.Generic;
using ALPackage;
using Common.TravelObj;
using GS2GC.p008_TravelOp;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public interface _ITravelController
    {
        /// <summary>
        /// 尝试处理已经存在的事件
        /// </summary>
        bool tryDealAlreadyExistedEvent(Action _onDealDone = null, Action _onBreak = null);
        
        /// <summary>
        /// 随机游历表现过程
        /// </summary>
        /// <param name="_onProcessComplete"></param>
        void randomTravelShowProcess(GS2GC.p008_TravelOp.GS2GC_008_001_RetStartTravel _info, Action _onProcessComplete);
        
        /// <summary>
        /// 一键游历表现过程
        /// </summary>
        /// <param name="_onProcessComplete"></param>
        void akeyTravelShowProcess(GS2GC.p008_TravelOp.GS2GC_008_002_RetAkeyTravel _info, long _addExp, Action _onProcessComplete);
    }
    
    /// <summary>
    /// 游历节点
    /// </summary>
    public class GNodeTravelMain : BaseQueueNode, _ITravelController
    {
        [NotNull] private readonly GTravelMainSceneViewMgr _m_sceneViewMgr;
        private int _m_enterSerialize;
        private bool _m_bIsFirstEnterNode;//是否首次进入Node
        private Action<GNodeTravelMain> _m_enterComplete;
        
        private int _m_inputMaskSerialize = -1;
        
        public GNodeTravelMain(Action<GNodeTravelMain> _complete = null): base(EUIQueueStageType.MAIN, UINodeTagConst.C_MAIN_TRAVEL)
        {
            _m_bIsFirstEnterNode = true;
            _m_enterComplete = _complete;
            _m_sceneViewMgr = new GTravelMainSceneViewMgr();
        }

        /// <summary>
        /// 是否所有UI的根节点，如是则在进入本节点的时候会清空所有节点队列
        /// </summary>
        public override bool isRootNode { get { return false; } }
        /// <summary>
        /// 在进行回退处理的时候，队列系统是否能直接退出本节点
        /// </summary>
        public override bool IsCanRollBackQuit { get { return true; } }
        /// <summary>
        /// 本节点在进行前进跳转的时候是否需要自动删除
        /// </summary>
        public override bool NeedAutoRemove { get { return false; } }
        /// <summary>
        /// 在回退操作的时候，是否需要执行上一个节点的进入操作
        /// </summary>
        public override bool IsMainViewNode { get { return true; } }
        /** 当前节点是否还有效 */
        public override bool isEnable { get { return true; } }
        
        /// <summary>
        /// 在节点进入队列的时候执行的事件函数
        /// </summary>
        public override void onEnterQueue()
        {
            GGUIAddSceneTravelMain.instance.setTravelController(this);
        }
        /// <summary>
        /// 不论在任何时候，当节点被退出总队列的时候，都会调用onClose函数
        /// </summary>
        public override void onClose()
        {
            _m_bIsShowingEnterNodeNotice = false;
            _m_aOnAllEnterNodeNoticeDone?.Invoke();
            _m_aOnAllEnterNodeNoticeDone = null;
            
            NPPlayer.instance.travelComp.isTraveling = false;
            GGUIAddSceneTravelMain.instance.setTravelController(null);
            _m_sceneViewMgr.discard();
        }
        
        public override void doEnterNode(Action _triggerEnterDone)
        {
            int serialize = _m_enterSerialize;
            
            base.doEnterNode(null);
            
            //如果需要reload先进一个空场景
            if (GTDSceneMain.instance.curShowScene == MainAddtionTravelMainTDScene.instance && MainAddtionTravelMainTDScene.instance.checkNeedReload())
                GTDSceneMain.instance.showMainScene(MainAdditionEmptyTDScene.instance);

            //顺序加载：先加载TD场景
            GTDSceneMain.instance.showMainScene(MainAddtionTravelMainTDScene.instance, () =>
            {
                if (serialize != _m_enterSerialize)
                    return;
                    
                //再加载UI场景
                GUISceneMain.instance.showMainScene(GGUIAddSceneTravelMain.instance, () =>
                {
                    if (serialize != _m_enterSerialize)
                        return;
                    
                    //最后加载SceneMgr(若加载过了就不用再次加载, 直接显示)
                    if(!_m_sceneViewMgr.isLoaded)
                        _m_sceneViewMgr.load();
                    _m_sceneViewMgr.regLoadDoneDelegate(() =>
                    {
                        _m_sceneViewMgr.showAll();
                        
                        _triggerEnterDone?.Invoke();
                        Action<GNodeTravelMain> complete = _m_enterComplete;
                        _m_enterComplete = null;
                        complete?.Invoke(this);
                    });
                });
            });
        }

        public override void EnterNode()
        {
        }

        public override void QuitNode()
        {
            _m_sceneViewMgr.hideAll();
            GGUIWndTravelProcessSkip.instance.hideWnd();
            
            _m_enterSerialize = ALSerializeOpMgr.next();
        }
        
        /**********************
         * 在本节点不允许esc回退时执行的逻辑。
         **/
        public override void onCannotEscBack()
        {
        }

        #region 在Notice展示后, 引导展示前执行的逻辑

        private bool _m_bIsShowingEnterNodeNotice;//是否正在显示推送弹窗
        private Action _m_aOnAllEnterNodeNoticeDone;//所有推送弹窗处理完成回调
        
        protected internal override void _doNoticeCheckDone(Action _allNodePreDone)
        {
            bool isFirstEnterNode = _m_bIsFirstEnterNode;
            _m_bIsFirstEnterNode = false;
            
            if(_allNodePreDone != null)
                _m_aOnAllEnterNodeNoticeDone += _allNodePreDone;
            
            if(_m_bIsShowingEnterNodeNotice)
                return;

            _m_bIsShowingEnterNodeNotice = true;
            
            _showEnterNodeNotice(isFirstEnterNode, () =>
            {
                _m_bIsShowingEnterNodeNotice = false;
                Action action = _m_aOnAllEnterNodeNoticeDone;
                _m_aOnAllEnterNodeNoticeDone = null;
                action?.Invoke();
            });
        }

        private void _showEnterNodeNotice(bool _isFirstEnterNode, Action _showDone)
        {
            ALProcess process = ALProcess.CreateProcess();

            process
                .addDelegateProcess((_processComplete) =>
                {
                    // 首次进入表现
                    showFirstEnter(_isFirstEnterNode, _processComplete);
                })
                .addDelegateProcess((_processComplete) =>
                {
                    // 游历地点解锁表现
                    showPosUnlock(_processComplete);
                })
                .addDelegateProcess((_processComplete) =>
                {
                    if (_isFirstEnterNode)
                    {
                        // 首次进入节点才尝试处理事件, 因为当前有些事件结果弹窗（如奖励事件）是会回到游历界面才展示的，若处理事件时前往了一个MainNode, 这样回来时就会再次调用_doNoticeCheckDone, 从而调用_showEnterNodeNotice,
                        // 这种情况若不是首次进入节点也尝试进行事件处理的话就可能导致在游历界面时又进行一次事件处理（因为关闭结果弹窗后事件才算处理完成）
                        tryDealAlreadyExistedEvent(_processComplete, _processComplete);      
                    }
                    else
                    {
                        _processComplete?.Invoke();
                    }
                })
                .addProcess(_showDone)
                .deal();
        }
        
        /// <summary>
        /// 首次进入表现
        /// </summary>
        public void showFirstEnter(bool _isFirstEnterNode, Action _showDone)
        {
            // 判断是否首次进入游历系统，若是则展示首次进入表现(首次进入游历系统表现直接放在引导里, 这里就不用了)
            // if (NPPlayer.instance.travelComp.getIsFirstEnterTravel())
            // {
            //     NPPlayer.instance.travelComp.setHadEnteredTravel();
            //     
            //     MainAddtionTravelMainTDScene.instance.focusToMainCity(0);
            //     _m_sceneViewMgr.setParkingMainCity(false);
            //     
            //     ALProcess process = ALProcess.CreateProcess("travel_first_enter_process");
            //     process
            //         .addDelegateProcess((_processComplete) =>
            //         {
            //             GGUIWndTravelMain.instance.playFirstEnterVideo(_processComplete);
            //         })
            //         .addDelegateProcess((_processComplete) =>
            //         {
            //             GGUIWndTravelMain.instance.showFirstEnterDialog(_processComplete);
            //         })
            //         .addDelegateProcess((_processComplete) =>
            //         {
            //             if (_m_sceneViewMgr != null)
            //             {
            //                 _m_sceneViewMgr.showFirstEnter(_processComplete);
            //             }
            //             else
            //             {
            //                 _processComplete?.Invoke();
            //             }
            //         })
            //         .addProcess(_showDone)
            //         .deal();
            // }
            // else
            // {
                if (_isFirstEnterNode)
                {
                    MainAddtionTravelMainTDScene.instance.focusToMainCity(0);
                    _m_sceneViewMgr.setParkingMainCity(false);
                }
                
                _showDone?.Invoke();
            // }
        }

        /// <summary>
        /// 游历地点解锁表现
        /// </summary>
        public void showPosUnlock(Action _showDone)
        {
            ALProcess process = ALProcess.CreateProcess("travel_pos_unlock_process");

            GRefdataCoreMgr.instance.travelPosCore.dealAllRef((posRefObj) =>
            {
                if (posRefObj == null)
                    return;

                bool hasClickGotoBtn = false;//是否点击过前往按钮了
                if (NPPlayer.instance.travelComp.getNeedShowPosUnlock(posRefObj))
                {
                    process
                        .addDelegateProcess((_processComplete) =>
                        {
                            NPPlayer.instance.travelComp.setPosUnlockShowed(posRefObj.id ,null);
                            MainAddtionTravelMainTDScene.instance.focusToPos(posRefObj.id, _processComplete);
                        })
                        .addDelegateProcess((_processComplete) =>
                        {
                            GGUIWndTravelPosUnlock.instance.refreshWnd(posRefObj, () =>
                            {
                                hasClickGotoBtn = true;
                            });
                            QueueMgr.instance.AddNode(new GNodeCommonWndWithCloseFunc(GGUIWndTravelPosUnlock.instance, _processComplete, UINodeTagConst.C_TRAVEL_POS_UNLOCK));
                        })
                        .addDelegateProcess((_processComplete) =>
                        {
                            _m_sceneViewMgr.showPosUnlock(posRefObj.id, _processComplete);
                        })
                        .addDelegateProcess((_processComplete) =>
                        {
                            if (hasClickGotoBtn)
                            {
                                // 若点击了前往按钮, 打开地点详情窗口
                                GGUIWndTravelPosInfo.instance.refreshWnd(posRefObj);
                                QueueMgr.instance.AddNode(new GNodeCommonWndWithCloseFunc_OnlyCloseDiscar(GGUIWndTravelPosInfo.instance, _processComplete, UINodeTagConst.C_TRAVEL_POS_INFO));
                            }
                            else
                            {
                                _processComplete?.Invoke();
                            }
                        });
                }
            });

            process.addProcess(_showDone);
            process.deal();
        }
        
        /// <summary>
        /// 尝试处理所有未处理的事件
        /// </summary>
        public bool tryDealAlreadyExistedEvent(Action _onDealDone = null, Action _onBreak = null)
        {
            // 若还有事件需要处理
            if (NPPlayer.instance.travelComp.eventList != null && NPPlayer.instance.travelComp.eventList.Count > 0)
            {
                NPPlayer.instance.travelComp.isTraveling = true;
                
                // 尝试处理完所有事件
                TravelEventUtil.tryDealAllEvent(NPPlayer.instance.travelComp.eventList.Count > 1, () =>
                {
                    NPPlayer.instance.travelComp.isTraveling = false;
                    
                    _onDealDone?.Invoke();
                }, () =>
                {
                    NPPlayer.instance.travelComp.isTraveling = false;
                    
                    _onBreak?.Invoke();
                });
                
                return true;
            }
            else
            {
                _onDealDone?.Invoke();
                return false;
            }
        }

        #endregion

        #region 表现控制

        public void randomTravelShowProcess(GS2GC_008_001_RetStartTravel _info, Action _onProcessComplete)
        {
            NPPlayer.instance.travelComp.isTraveling = true;
            
            ALProcess process = ALProcess.CreateProcess("random_travel");
            // RenderTexture screenshotRenderTexture = null;
            _ATravelEventInfo travelEventInfo = NPPlayer.instance.travelComp.eventList?.GetFirst();//获取第一个随机游历事件
            TravelPosRefObj travelPosRefObj = travelEventInfo?.travelPosRefObj;
            process
                .addDelegateProcess((_processComplete) =>
                {
                    if (travelPosRefObj != null)
                    {
                        GGUIWndTravelMain.instance.playStartTravelVideo(travelPosRefObj.start_travel_video_index, _processComplete);
                    }
                    else
                    {
                        _processComplete?.Invoke();
                    }
                })
                .addDelegateProcess((_processComplete) =>
                {
                    ALStepCounter stepCounter = new ALStepCounter();
                    stepCounter.chgTotalStepCount(2);
                    stepCounter.regAllDoneDelegate(() =>
                    {  
                        _processComplete?.Invoke();
                    });
                    
                    if (travelEventInfo != null)
                    {
                        MainAddtionTravelMainTDScene.instance.focusToPos(travelEventInfo.posId, stepCounter.addDoneStepCount);
                    }
                    else
                    {
                        stepCounter.addDoneStepCount();
                    }
                    
                    if (_m_sceneViewMgr != null && travelPosRefObj != null)
                    {
                        _m_sceneViewMgr.aircraftFlyToPos(travelPosRefObj.id, stepCounter.addDoneStepCount);
                    }
                    else
                    {
                        stepCounter.addDoneStepCount();
                    }
                })
                .addDelegateProcess((_processComplete) =>
                {
                    if (travelPosRefObj != null)
                    {
                        GGUIWndTravelMain.instance.playLandingVideo(travelPosRefObj.landing_video_index, _processComplete);
                    }
                    else
                    {
                        _processComplete?.Invoke();
                    }
                })
                .addDelegateProcess((_processComplete) =>
                {
                    GGUIWndTravelMain.instance.showEventCenterTip(travelEventInfo, _processComplete);
                })
                // .addDelegateProcess((_processComplete) =>
                // {
                //     ScreenShotUtil.getCaptureScreenshotIntoRenderTextureByDealer((_renderTexture) =>
                //     {
                //         screenshotRenderTexture = _renderTexture;
                //         _processComplete?.Invoke();
                //     });
                // })
                // .addDelegateProcess((_processComplete) =>
                // {
                //     QueueMgr.instance.AddNode(new BaseOnAddContainerSceneUIWndQueueNode(EUIQueueStageType.MAIN, UINodeTagConst.C_TRAVEL_RANDOM_TRAVEL_PROCESS, false
                //         , true, false, null, GGUIWndRandomTravelProcess.instance, true, false,
                //         () =>
                //         {
                //             GGUIWndRandomTravelProcess.instance.setData(screenshotRenderTexture, travelEventInfo);
                //         }, null, null, () =>
                //         {
                //             ScreenShotUtil.releaseCaptureScreenshotIntoRenderTextureByDealer(screenshotRenderTexture);
                //             screenshotRenderTexture = null;
                //             
                //             _processComplete?.Invoke();
                //         }));
                //     
                //     // 展示跳过按钮窗口
                //     GGUIWndTravelProcessSkip.instance.setInfo(() =>
                //     {
                //         QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_TRAVEL_RANDOM_TRAVEL_PROCESS);
                //         GGUIWndTravelProcessSkip.instance.hideWnd();
                //     });
                //     GUISceneMain.instance.showAddWnd(GGUIWndTravelProcessSkip.instance, GGUIWndTravelProcessSkip.instance.showWnd);
                // })
                .addDelegateProcess((_processComplete) =>
                {
                    // 尝试进行事件处理
                    TravelEventUtil.tryDealAllEvent(false, () =>
                    {
                        _processComplete?.Invoke();
                    }, () =>
                    {
                        _processComplete?.Invoke();
                    });
                })
                // .addDelegateProcess((_processComplete) =>
                // {
                //     ScreenShotUtil.releaseCaptureScreenshotIntoRenderTextureByDealer(screenshotRenderTexture);
                //     screenshotRenderTexture = null;
                // })
                .addProcess(() =>
                {
                    NPPlayer.instance.travelComp.isTraveling = false;
                    GGUIWndTravelProcessSkip.instance.hideWnd();
                })
                .addProcess(_onProcessComplete)
                .deal();
        }

        public void akeyTravelShowProcess(GS2GC_008_002_RetAkeyTravel _info, long _addExp, Action _onProcessComplete)
        {
            NPPlayer.instance.travelComp.isTraveling = true;
            
            ALProcess process = ALProcess.CreateProcess("akey_travel");
            process
                .addDelegateProcess((_processComplete) =>
                {
                    if (_info == null)
                    {
                        _processComplete?.Invoke();
                        return;
                    }
                    
                    List<_ITravelEventResultInfo> resultInfoList = new List<_ITravelEventResultInfo>();
                    _ITravelEventResultInfo _resultInfo = null;
                    foreach (Travel_EventResult serverResultInfo in _info.getResultList())
                    {
                        if(serverResultInfo == null)
                            continue;
                        _resultInfo = TravelEventUtil.makeTravelResultEventInfo(serverResultInfo, 0);
                        resultInfoList.Add(_resultInfo);
                    }

                    if (resultInfoList.Count <= 0)
                    {
                        _processComplete?.Invoke();
                    }
                    else
                    {
                        // 显示一键游历结果窗口
                        QueueMgr.instance.AddNode(new BaseOnAddContainerSceneUIWndQueueNode(EUIQueueStageType.MAIN, UINodeTagConst.C_TRAVEL_AKEY_RESULT, true
                            , false, false, null, GGUIWndAkeyTravelResult.instance, true, false,
                            () =>
                            {
                                GGUIWndAkeyTravelResult.instance.showWnd();
                                GGUIWndAkeyTravelResult.instance.setData(resultInfoList, _addExp);
                            }, null, null, () =>
                            {
                                _processComplete?.Invoke();
                            }));
                    }
                })
                .addDelegateProcess((_processComplete) =>
                {
                    // 展示妃子好感度阶段变化窗口
                    NPPlayer.instance.travelComp.showAllConsortLikeStageChgDialog(true, _processComplete);
                })
                .addDelegateProcess((_processComplete) =>
                {
                    // 处理所有事件
                    TravelEventUtil.tryDealAllEvent(NPPlayer.instance.travelComp.eventList.Count > 1, ()=>
                    {
                        _processComplete?.Invoke();
                    }, () =>
                    {
                        _processComplete?.Invoke();
                    });
                })
                .addProcess(() =>
                {
                    NPPlayer.instance.travelComp.isTraveling = false;
                    GGUIWndTravelProcessSkip.instance.hideWnd();
                })
                .addProcess(_onProcessComplete)
                .deal();
        }

        #endregion
    }
}
