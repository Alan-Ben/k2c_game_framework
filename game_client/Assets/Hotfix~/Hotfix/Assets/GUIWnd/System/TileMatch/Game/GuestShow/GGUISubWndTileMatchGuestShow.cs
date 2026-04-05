using System;
using System.Collections.Generic;
using ALPackage;
using DG.Tweening;
using GOE;
using JetBrains.Annotations;
using UnityEngine;

namespace Hotfix
{
    /// <summary>
    /// 客人和服务员的表现总控制窗口
    /// 负责加载客人和服务员，根据任务结果列表依次播放表现动画
    /// </summary>
    public class GGUISubWndTileMatchGuestShow : _AHotfixBaseSubWnd<GGUISubMonoTileMatchGuestShow>
    {
        /// <summary>
        /// 任务处理结果列表（true=成功，false=失败）
        /// </summary>
        [NotNull] private List<bool> _m_lTaskResultList = new List<bool>();

        /// <summary>
        /// 当前已加载的客人Wnd列表（按位置顺序排列）
        /// </summary>
        [NotNull] private List<GGUIWndTileMatchGuest> _m_lGuestWndList = new List<GGUIWndTileMatchGuest>();

        /// <summary>
        /// 客人缓存池管理器
        /// </summary>
        private GGUIWndTileMatchGuestCacheMgr _m_guestCacheMgr;

        /// <summary>
        /// 服务员Wnd
        /// </summary>
        private GGUIWndTileMatchWaiter _m_wWaiterWnd;

        /// <summary>
        /// 是否正在播放表现动画
        /// </summary>
        private bool _m_bIsPlaying;

        /// <summary>
        /// 窗口显示序列号，用于防止异步加载回调时已被销毁
        /// </summary>
        private int _m_iWndShowSerialize;
        /// <summary>
        /// 所有客人加载序列号，用于防止异步加载回调时已被销毁
        /// </summary>
        private int _m_iAllGuestLoadSerialize;

        public GGUISubWndTileMatchGuestShow(GGUIHotfixCommonMono _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onWndInitDoneHotfix()
        {
            if(hotfixWnd == null)
                return;
            
            // 创建客人缓存池管理器
            _m_guestCacheMgr = new GGUIWndTileMatchGuestCacheMgr("TileMatchGuestCache");

            if (hotfixWnd.waiterPrefabGoIndexList != null && hotfixWnd.waiterPrefabGoIndexList.Count > 0)
            {
                NPGGoIndex waiterGoIndex = hotfixWnd.waiterPrefabGoIndexList.GetRandomItem();
                if (waiterGoIndex != null && waiterGoIndex.isValid())
                {
                    _m_wWaiterWnd = new GGUIWndTileMatchWaiter(hotfixWnd.waiterTrans);
                    _m_wWaiterWnd.setAssetLoadPath(waiterGoIndex.assetPath, waiterGoIndex.objName);
                    _m_wWaiterWnd.load();
                }
            }
        }
        
        protected override void _onDiscard()
        {
            // 回收所有正在使用的客人
            _pushBackAllGuests();
            // 销毁服务员窗口
            _m_wWaiterWnd?.discard();
            _m_wWaiterWnd = null;

            // 销毁客人缓存池（释放所有缓存资源）
            _m_guestCacheMgr?.discardAll();
            _m_guestCacheMgr = null;

            _m_lTaskResultList.Clear();
        }
        
        protected override void _onShowWnd()
        {
            _m_bIsPlaying = true;//加载过程中先设置为正在播放，等加载完成后再根据结果列表决定是否继续播放表现动画

            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(2);
            stepCounter.regAllDoneDelegate(() =>
            {
                _m_bIsPlaying = false;
                
                // 进行入场表现
                _showEnter();
            });
            
            // 加载客人
            _loadAllGuests(stepCounter.addDoneStepCount);
            
            // 显示服务员
            if (_m_wWaiterWnd != null)
            {
                _m_wWaiterWnd.regLoadDoneDelegate(() =>
                {
                    _m_wWaiterWnd.showWnd();
                    _m_wWaiterWnd.setState(ETileMatchWaiterState.IDLE);
                    stepCounter.addDoneStepCount();
                });
            }
            else
            {
                stepCounter.addDoneStepCount();
            }
        }

        protected override void _onHideWnd()
        {
            _m_bIsPlaying = false;
            _m_iWndShowSerialize = ALSerializeOpMgr.next();
            _m_iAllGuestLoadSerialize = ALSerializeOpMgr.next();

            _m_lTaskResultList.Clear();

            // 回收客人到缓存池（不销毁，以便复用）
            _pushBackAllGuests();
            // 隐藏服务员窗口
            _m_wWaiterWnd?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_bIsPlaying = false;
            _m_iWndShowSerialize = ALSerializeOpMgr.next();
            _m_iAllGuestLoadSerialize = ALSerializeOpMgr.next();

            _m_lTaskResultList.Clear();

            // 回收客人到缓存池（不销毁，以便复用）
            _pushBackAllGuests();
            // 隐藏服务员窗口
            _m_wWaiterWnd?.resetWnd();
        }

        /// <summary>
        /// 添加任务处理结果（true=成功，false=失败）
        /// </summary>
        /// <param name="_isSuccess">是否成功</param>
        public void addTaskResult(bool _isSuccess)
        {
            if(!isShow)
                return;
            
            _m_lTaskResultList.Add(_isSuccess);
            
            _tryShowTaskResult();
        }

        #region 客人窗口的加载与销毁

        /// <summary>
        /// 加载一个客人
        /// </summary>
        private void _loadAGuest(Action<GGUIWndTileMatchGuest> _onLoadComplete)
        {
            if (hotfixWnd == null || hotfixWnd.guestPrefabGoIndexList == null || _m_guestCacheMgr == null)
            {
                _onLoadComplete?.Invoke(null);
                return;
            }

            NPGGoIndex goIndex = hotfixWnd.guestPrefabGoIndexList.GetRandomItem();
            if (goIndex == null || !goIndex.isValid())
            {
                _onLoadComplete?.Invoke(null);
                return;
            }
            
            int wndShowSerialize = _m_iWndShowSerialize;
            // 通过缓存池管理器加载客人
            _m_guestCacheMgr.popItem(goIndex, (_index, _guestWnd) =>
            {
                if (_guestWnd == null)
                {
                    Debug.LogError($"加载客人失败，goIndex={goIndex}");
                    _onLoadComplete?.Invoke(null);
                    return;
                }
                
                // 检查是否已被销毁或序列号变化
                if (wndShowSerialize != _m_iWndShowSerialize)
                {
                    // 回收到缓存池
                    _m_guestCacheMgr?.pushBackItem(goIndex, _guestWnd);
                    return;
                }

                if (hotfixWnd == null || _guestWnd.wnd == null)
                {
                    _m_guestCacheMgr?.pushBackItem(goIndex, _guestWnd);
                    _onLoadComplete?.Invoke(null);
                    return;
                }

                // 设置初始位置为guestOriTrans
                _guestWnd.wnd.transform.SetParent(hotfixWnd.guestLoadParent);
                _guestWnd.wnd.transform.position = hotfixWnd.guestOriTrans?.position ?? Vector3.zero;
                _guestWnd.wnd.transform.localScale = Vector3.one;
                
                _guestWnd.showWnd();
                _onLoadComplete?.Invoke(_guestWnd);
            });
        }
        
        /// <summary>
        /// 加载所有客人
        /// 从guestPrefabGoIndexList中随机选取，数量为guestPosTransList的元素数量
        /// 初始位置统一在guestOriTrans，然后依次移动到guestPosTransList对应位置
        /// </summary>
        private void _loadAllGuests(Action _onLoadDone)
        {
            _pushBackAllGuests();//先回收所有客人到缓存池
            if (hotfixWnd == null || _m_guestCacheMgr == null)
            {
                _onLoadDone?.Invoke();
                return;
            }

            if (hotfixWnd.guestPosTransList == null || hotfixWnd.guestPosTransList.Count == 0)
            {
                _onLoadDone?.Invoke();
                return;
            }

            int allGuestLoadSerialize = _m_iAllGuestLoadSerialize = ALSerializeOpMgr.next();
            
            int guestCount = hotfixWnd.guestPosTransList.Count;
            _m_lGuestWndList.SetCount(guestCount);//预设列表容量，方便进行赋值

            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(guestCount);
            stepCounter.regAllDoneDelegate(_onLoadDone);
            
            for (int i = 0; i < guestCount; i++)
            {
                int guestIndex = i;
                _loadAGuest((_guestWnd) =>
                {
                    if (_guestWnd == null)
                    {
                        stepCounter.addDoneStepCount();
                        return;
                    }
                    
                    if (allGuestLoadSerialize != _m_iAllGuestLoadSerialize)
                    {
                        _pushBackGuests(_guestWnd);
                    }
                    else
                    {
                        _m_lGuestWndList[guestIndex] = _guestWnd;
                        stepCounter.addDoneStepCount();
                    }
                });
            }
        }

        /// <summary>
        /// 回收所有客人到缓存池
        /// </summary>
        private void _pushBackAllGuests()
        {
            foreach (var guestWnd in _m_lGuestWndList)
            {
                _pushBackGuests(guestWnd);
            }
            _m_lGuestWndList.Clear();
        }

        private void _pushBackGuests(GGUIWndTileMatchGuest _guestWnd)
        {
            if(_guestWnd == null)
                return;

            if (_m_guestCacheMgr == null)
            {
                GameObject guestGo = _guestWnd.go;
                _guestWnd.discard();
                ALUnityCommon.releaseGameObj(guestGo);
            }
            else
            {
                _m_guestCacheMgr.pushBackItem(_guestWnd.resGoIndex, _guestWnd);
            }
        }
        
        #endregion

        /// <summary>
        /// 入场表现
        /// </summary>
        private void _showEnter()
        {
            if (hotfixWnd == null || !isShow)
                return;

            _m_bIsPlaying = true;
            
            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(1);
            stepCounter.regAllDoneDelegate(() =>
            {
                _m_bIsPlaying = false;
                
                // 入场表现完成后尝试播放任务结果表现
                _tryShowTaskResult();
            });

            int showSerializeId = _m_iWndShowSerialize = ALSerializeOpMgr.next();
            if (hotfixWnd.guestPosTransList != null)
            {
                int showGuestCount = 0;
                // 客人依次入场，从guestOriTrans移动到对应位置
                for (int i = 0; i < _m_lGuestWndList.Count; i++)
                {
                    GGUIWndTileMatchGuest guestWnd = _m_lGuestWndList[i];
                    if (guestWnd == null)
                        continue;

                    // 显示客人
                    guestWnd.showWnd();
                    if (i < hotfixWnd.guestPosTransList.Count && hotfixWnd.guestPosTransList[i] != null)
                    {
                        Vector3 targetPos = hotfixWnd.guestPosTransList[i].position;

                        stepCounter.chgTotalStepCount(1);
                        
                        CommonTaskController.CommonActionAddMonoTask(() =>
                        {
                            if(_m_iWndShowSerialize != showSerializeId)
                                return;
                            
                            guestWnd.moveTo(targetPos, hotfixWnd.guestEnterMoveTime, () =>
                            {
                                if(_m_iWndShowSerialize != showSerializeId)
                                    return;
                            
                                stepCounter.addDoneStepCount();
                            });
                        }, hotfixWnd.firstEnterPerGuestStartMoveDelayTime * showGuestCount);

                        showGuestCount++;
                    }
                }
            }

            if (_m_wWaiterWnd != null)
            {
                _m_wWaiterWnd.showWnd();
                _m_wWaiterWnd.setState(ETileMatchWaiterState.IDLE);
            }
            
            // 启动
            stepCounter.addDoneStepCount();
        }

        #region 任务结果表现

        /// <summary>
        /// 处理下一个任务结果
        /// </summary>
        private void _tryShowTaskResult()
        {
            // 若果正在播放表现动画，则等待当前动画结束后再处理下一个结果
            if(_m_bIsPlaying || !isShow)
                return;
            
            // 没有待处理的结果
            if (_m_lTaskResultList.Count == 0)
            {
                _m_bIsPlaying = false;
                // 所有结果处理完毕，客人和服务员进入IDLE
                _setAllToIdle();
                return;
            }

            _m_bIsPlaying = true;
            // 取出第一个结果
            bool isSuccess = _m_lTaskResultList[0];
            _m_lTaskResultList.RemoveAt(0);
            
            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(2);
            stepCounter.regAllDoneDelegate(() =>
            {
                _m_bIsPlaying = false;// 表现完成，重置播放状态，再驱动下一个
                _tryShowTaskResult();
            });
            
            _showTaskResult_Guest(isSuccess, stepCounter.addDoneStepCount);
            _showTaskResult_Waiter(isSuccess, stepCounter.addDoneStepCount);
        }

        /// <summary>
        /// 任务结果表现 - 顾客
        /// </summary>
        private void _showTaskResult_Guest(bool _isSucc, Action _showComplete)
        {
            long showSerializeId = _m_iWndShowSerialize;
            ALProcess process = ALProcess.CreateProcess();
            process
                .addDelegateProcess((_processComplete) =>
                {
                    if(_m_iWndShowSerialize != showSerializeId)
                        return;
                    
                    // 第一个客人进行表现动画，其他客人保持不动
                    GGUIWndTileMatchGuest guestWnd = _m_lGuestWndList.SafeGet(0);
                    if (guestWnd != null)
                    {
                        if (_isSucc)
                        {
                            guestWnd.showSucc(_processComplete);
                        }
                        else
                        {
                            guestWnd.showFail(_processComplete);
                        }
                    }
                    else
                    {
                        _processComplete?.Invoke();
                    }
                })
                .addDelegateProcess((_processComplete) =>
                {
                    if(_m_iWndShowSerialize != showSerializeId)
                        return;
                    
                    GGUIWndTileMatchGuest firstGuestWnd = _m_lGuestWndList.GetFirstAndRemove();//取出第一个客人并从列表移除
                    
                    ALStepCounter stepCounter = new ALStepCounter();
                    stepCounter.chgTotalStepCount(1);
                    stepCounter.regAllDoneDelegate(() =>
                    {
                        _processComplete?.Invoke();
                    });

                    // 第一个客人翻转离场，移动到guestOriTrans位置
                    if (firstGuestWnd != null && hotfixWnd != null)
                    {
                        stepCounter.chgTotalStepCount(1);
                        firstGuestWnd.flip();// 客人翻转
                        firstGuestWnd.moveTo(hotfixWnd.guestOriTrans?.position ?? Vector3.zero, hotfixWnd.guestExitMoveTime,
                            () =>
                            {
                                _pushBackGuests(firstGuestWnd);// 离场后, 回收离场的客人到缓存池
                                stepCounter.addDoneStepCount();
                            });
                    }
                    
                    //其他客人依次前进, 移动到前一个客人的位置
                    int guestIndex = 0;//因为已经取出第一个客人并且从列表中移除了，所以从索引0开始就是原来的第二个客人
                    if (hotfixWnd?.guestPosTransList != null)
                    {
                        for (; guestIndex < _m_lGuestWndList.Count; guestIndex++)
                        {
                            GGUIWndTileMatchGuest advanceGuestWnd = _m_lGuestWndList[guestIndex];
                            if (advanceGuestWnd == null)
                                continue;

                            // 目标位置为当前索引对应的位置（即前一个客人的位置）
                            if (guestIndex >= hotfixWnd.guestPosTransList.Count)
                                continue;

                            Transform posTrans = hotfixWnd.guestPosTransList[guestIndex];
                            if (posTrans == null)
                                continue;

                            stepCounter.chgTotalStepCount(1);
                            advanceGuestWnd.moveTo(posTrans.position, hotfixWnd.guestAdvanceMoveTime, () =>
                            {
                                stepCounter.addDoneStepCount();
                            });
                        }
                    }

                    // 生成一个新顾客, 从guestOriTrans位置入场，移动到最后一个位置
                    if (hotfixWnd?.guestPosTransList != null && hotfixWnd.guestPosTransList.Count > 0 && guestIndex < hotfixWnd.guestPosTransList.Count)
                    {
                        Transform newGuestPosTrans = hotfixWnd.guestPosTransList[guestIndex];

                        stepCounter.chgTotalStepCount(1);
                        _loadAGuest((_newGuestWnd) =>
                        {
                            if (_newGuestWnd == null || newGuestPosTrans == null || hotfixWnd == null || _m_iWndShowSerialize != showSerializeId)
                            {
                                _pushBackGuests(_newGuestWnd);
                                stepCounter.addDoneStepCount();
                                return;
                            }

                            _m_lGuestWndList.Add(_newGuestWnd);
                            _newGuestWnd.showWnd();
                            _newGuestWnd.moveTo(newGuestPosTrans.position, hotfixWnd.guestEnterMoveTime, () =>
                            {
                                stepCounter.addDoneStepCount();
                            });
                        });
                    }
                    
                    // 启动
                    stepCounter.addDoneStepCount();
                })
                .addProcess(()=>
                {
                    if(_m_iWndShowSerialize != showSerializeId)
                        return;
                    
                    _showComplete?.Invoke();
                })
                .deal();
        }
        
        /// <summary>
        /// 任务结果表现 - 服务员
        /// </summary>
        /// <param name="_showComplete"></param>
        private void _showTaskResult_Waiter(bool _isSucc, Action _showComplete)
        {
            if (_m_wWaiterWnd == null)
            {
                _showComplete?.Invoke();
                return;
            }

            long showSerializeId = _m_iWndShowSerialize;
            if (_isSucc)
            {
                _m_wWaiterWnd.setState(ETileMatchWaiterState.SUCCESS, ()=>
                {
                    if(_m_iWndShowSerialize != showSerializeId)
                        return;
                    
                    _m_wWaiterWnd?.setState(ETileMatchWaiterState.IDLE);//表现动画结束后进入IDLE状态
                    _showComplete?.Invoke();
                });
            }
            else
            {
                _m_wWaiterWnd.setState(ETileMatchWaiterState.FAIL, ()=>
                {
                    if(_m_iWndShowSerialize != showSerializeId)
                        return;
                    
                    _m_wWaiterWnd?.setState(ETileMatchWaiterState.IDLE);//表现动画结束后进入IDLE状态
                    _showComplete?.Invoke();
                });
            }
        }
        
        #endregion

        private void _setAllToIdle()
        {
            // 所有客人进入IDLE状态
            foreach (var guestWnd in _m_lGuestWndList)
            {
                guestWnd?.setState(ETileMatchGuestState.IDLE);
            }

            // 服务员进入IDLE状态
            _m_wWaiterWnd?.setState(ETileMatchWaiterState.IDLE);
        }
    }
}