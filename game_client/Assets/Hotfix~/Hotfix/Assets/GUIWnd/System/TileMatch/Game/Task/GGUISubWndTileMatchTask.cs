using System;
using System.Collections.Generic;
using ALPackage;
using GOE;
using JetBrains.Annotations;
using UnityEngine;

namespace Hotfix
{
    /// <summary>
    /// 三消任务子窗口
    /// </summary>
    public class GGUISubWndTileMatchTask : _AHotfixBaseSubWnd<GGUISubMonoTileMatchTask>
    {
        private long _m_lShowSerialize;
        
        private TileMatchGameLogic _m_gameLogic;
        private TileMatchTaskInfo _m_taskInfo; // 当前任务信息
        private List<TileMatchTaskInfo> _m_newTaskInfoList; // 新任务信息列表
        private TileMatchModeRefObj _m_rGameModel;

        private NPGGUIWndProgress _m_wLeftStepSlider; // 剩余步数进度条
        private GGUIWndTileMatchTaskBlockItemContainer _m_wTaskBlockItemContainer;
        private GGUISubWndTileMatchStepReward _m_wStepRewardWnd; // 阶段奖励窗口
        private GGUISubWndTileMatchGuestShow _m_wGuestShowWnd; // 客人展示窗口

        private GGUIWndTileMatchTaskFlyItemCache _m_taskFlyItemCache; // 飞行任务item缓存

        [NotNull] private Dictionary<int, int> _m_dTaskNeedFlyItemCountDic = new Dictionary<int, int>();// 任务需要飞行的item数量统计
        [NotNull] private Dictionary<int, Dictionary<long, int>> _m_dTempTaskBlockAddDoneNumDic = new Dictionary<int, Dictionary<long, int>>();// 临时任务格子增加完成数量统计(任务序列号 -> (格子id -> 增加数量))
        [NotNull] private List<GGUIWndTileMatchTaskBlockFlyItem> _m_lFlyingItemList = new List<GGUIWndTileMatchTaskBlockFlyItem>();//正在飞行中的item列表
        
        public GGUISubWndTileMatchTask(GGUIHotfixCommonMono _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onWndInitDoneHotfix()
        {
            if(hotfixWnd == null)
                return;

            if (hotfixWnd.monoLeftStepSlider)
                _m_wLeftStepSlider = new NPGGUIWndProgress(hotfixWnd.monoLeftStepSlider);
            
            if (hotfixWnd.monoTaskBlockContainer != null)
                _m_wTaskBlockItemContainer = new GGUIWndTileMatchTaskBlockItemContainer(hotfixWnd.monoTaskBlockContainer);
            
            if (hotfixWnd.taskFlyItemCacheRoot != null && hotfixWnd.monoTaskFlyItemPrefab != null)
            {
                _m_taskFlyItemCache = new GGUIWndTileMatchTaskFlyItemCache(hotfixWnd.taskFlyItemCacheRoot, 1, TileMatchUtil.column * TileMatchUtil.row);
                _m_taskFlyItemCache.init(hotfixWnd.monoTaskFlyItemPrefab);
            }

            if (hotfixWnd.monoStepReward != null)
                _m_wStepRewardWnd = new GGUISubWndTileMatchStepReward(hotfixWnd.monoStepReward);

            if (hotfixWnd.monoGuestShow != null)
                _m_wGuestShowWnd = new GGUISubWndTileMatchGuestShow(hotfixWnd.monoGuestShow);
        }
        
        protected override void _onDiscard()
        {
            _m_wLeftStepSlider?.discard();
            _m_wLeftStepSlider = null;
            
            _m_wTaskBlockItemContainer?.discard();
            _m_wTaskBlockItemContainer = null;

            _m_dTaskNeedFlyItemCountDic.Clear();
            _m_dTempTaskBlockAddDoneNumDic.Clear();
            _discardAllFlyingItem();
            _m_taskFlyItemCache?.discard();
            _m_taskFlyItemCache = null;
            
            _m_wStepRewardWnd?.discard();
            _m_wStepRewardWnd = null;

            _m_wGuestShowWnd?.discard();
            _m_wGuestShowWnd = null;
            
            resetGameLogic();
        }
        
        protected override void _onShowWnd()
        {
            _m_lShowSerialize = ALSerializeOpMgr.next();
            
            _m_wStepRewardWnd?.showWnd();
            _m_wGuestShowWnd?.showWnd();
        }

        protected override void _onHideWnd()
        {
            _m_lShowSerialize = ALSerializeOpMgr.next();

            _m_wLeftStepSlider?.hideWnd();
            _m_wTaskBlockItemContainer?.hideWnd();

            _m_dTaskNeedFlyItemCountDic.Clear();
            _m_dTempTaskBlockAddDoneNumDic.Clear();
            _discardAllFlyingItem();
            _m_taskFlyItemCache?.pushBackAllCacheItems();

            _m_wStepRewardWnd?.hideWnd();
            _m_wGuestShowWnd?.hideWnd();
            
            resetGameLogic();
        }

        protected override void _onReset()
        {
            _m_wLeftStepSlider?.resetWnd();
            _m_wTaskBlockItemContainer?.resetWnd();

            _m_dTaskNeedFlyItemCountDic.Clear();
            _m_dTempTaskBlockAddDoneNumDic.Clear();
            _discardAllFlyingItem();
            _m_taskFlyItemCache?.pushBackAllCacheItems();

            _m_wStepRewardWnd?.resetWnd();
            _m_wGuestShowWnd?.resetWnd();

            resetGameLogic();
        }
        
        /// <summary>
        /// 设置游戏逻辑
        /// </summary>
        public void setGameLogic(TileMatchGameLogic _gameLogic)
        {
            resetGameLogic();
            _m_gameLogic = _gameLogic;

            _updateCurTaskInfoFormGameLogic();
            
            if (_m_gameLogic != null)
            {
                _m_gameLogic.onNewTaskInfo += _onNewTaskInfo;
                _m_gameLogic.onTaskInfoChg += _onTaskInfoChg;
                _m_gameLogic.onStartDealServer += _oStartDealServer;
                _m_gameLogic.onBlockClear += _onBlockClear;
            }
        }
        
        private void _updateCurTaskInfoFormGameLogic()
        {
            _m_taskInfo = _m_gameLogic?.taskInfo;
            if (_m_taskInfo != null && _m_taskInfo.isDone())
                _m_taskInfo = null;
            
            _m_newTaskInfoList?.Clear();
            _m_rGameModel = _m_gameLogic?.nowGameModeRefObj;

            _m_dTaskNeedFlyItemCountDic.Clear();
            _m_dTempTaskBlockAddDoneNumDic.Clear();
            _discardAllFlyingItem();

            _m_wStepRewardWnd?.refreshWnd();
            
            _refreshCurTaskShow(true);
        }
        
        public void resetGameLogic()
        {
            if (_m_gameLogic != null)
            {
                _m_gameLogic.onNewTaskInfo -= _onNewTaskInfo;
                _m_gameLogic.onTaskInfoChg -= _onTaskInfoChg;
                _m_gameLogic.onStartDealServer += _oStartDealServer;
                _m_gameLogic.onBlockClear -= _onBlockClear;
            }

            _m_gameLogic = null;
            _m_rGameModel = null;
            _m_taskInfo = null;
            _m_newTaskInfoList?.Clear();
            _m_newTaskInfoList = null;
            
            _m_dTaskNeedFlyItemCountDic.Clear();
            _m_dTempTaskBlockAddDoneNumDic.Clear();
            _discardAllFlyingItem();
        }

        /// <summary>
        /// 刷新当前任务显示
        /// </summary>
        /// <param name="_useTaskInfoDoneNum">是否使用任务自己的完成数量</param>
        private void _refreshCurTaskShow(bool _useTaskInfoDoneNum)
        {
            if(hotfixWnd == null)
                return;
            
            ALUGUICommon.setGameObjEnable(hotfixWnd.hasTaskShow, _m_taskInfo != null);
            ALUGUICommon.setGameObjEnable(hotfixWnd.noTaskShow, _m_taskInfo == null);

            if (!_useTaskInfoDoneNum)
            {
                List<TileMatchTaskBlockInfo> blockInfoList = new List<TileMatchTaskBlockInfo>();
                if (_m_taskInfo?.taskBLockInfoList != null)
                {
                    TileMatchTaskBlockInfo blockInfo = null;
                    TileMatchTaskBlockInfo tempBlockInfo = null;
                    for (int i = 0, count = _m_taskInfo.taskBLockInfoList.Count; i < count; i++)
                    {
                        blockInfo = _m_taskInfo.taskBLockInfoList[i];
                        tempBlockInfo = null;
                        if (blockInfo != null)
                        {
                            if(_m_dTempTaskBlockAddDoneNumDic.TryGetValue(_m_taskInfo.serialId, out Dictionary<long, int> blockAddDoneNumDic) && blockAddDoneNumDic != null)
                            {
                                if(blockAddDoneNumDic.TryGetValue(blockInfo.blockId, out int addDoneNum))
                                    tempBlockInfo = new TileMatchTaskBlockInfo(blockInfo.blockId, addDoneNum, blockInfo.needTotalNum);
                            }

                            if (tempBlockInfo == null)
                                tempBlockInfo = new TileMatchTaskBlockInfo(blockInfo.blockId, 0, blockInfo.needTotalNum);
                            
                            blockInfoList.Add(tempBlockInfo);
                        }
                    }
                }
                
                if (_m_wTaskBlockItemContainer != null)
                {
                    _m_wTaskBlockItemContainer.showWnd();
                    _m_wTaskBlockItemContainer.setData(blockInfoList);
                }
            }
            else
            {
                if (_m_wTaskBlockItemContainer != null)
                {
                    _m_wTaskBlockItemContainer.showWnd();
                    _m_wTaskBlockItemContainer.setData(_m_taskInfo?.taskBLockInfoList);
                }
            }

            // _m_wTaskBlockItemContainer刷新后, 更改飞行中的item的目标位置
            GGUIWndTileMatchTaskBlockFlyItem flyItem = null;
            for (int i = 0, count = _m_lFlyingItemList.Count; i < count; i++)
            {
                flyItem = _m_lFlyingItemList[i];
                if(flyItem == null)
                    continue;
                
                GGUIWndTileMatchTaskBlockItem targetBlockItem = _m_wTaskBlockItemContainer?.getTaskItemWnd(flyItem.blockId);
                if (targetBlockItem != null && targetBlockItem.rectTransform != null)
                {
                    flyItem.chgFlyTarget(targetBlockItem.rectTransform.position);
                }
            }
            
            // 刷新剩余步数进度条
            _refreshCurTaskLeftStep();

            long gainScore = _m_taskInfo == null || _m_taskInfo.taskRefObj == null || _m_rGameModel == null ? 0 : _m_taskInfo.taskRefObj.gain_score * _m_rGameModel.multiple;
            // 刷新可获取分数显示
            ALUGUICommon.setLabelTxt(hotfixWnd.txtAddScore, TextTranslate.instance.getLanguage(HotfixTransKeyConst.tilematch_taskAddScore_num, gainScore.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT)));
        }

        /// <summary>
        /// 刷新剩余步数进度条
        /// </summary>
        private void _refreshCurTaskLeftStep()
        {
            if (_m_taskInfo != null)
            {
                int totalStep = _m_taskInfo.stepLimit;

                if (_m_wLeftStepSlider != null)
                {
                    _m_wLeftStepSlider.showWnd();
                    _m_wLeftStepSlider.setProgress(totalStep - _m_taskInfo.hadGoStep, totalStep, EValueFormatType.NORMAL, HotfixTransKeyConst.tilematch_taskLeftStep_num2);
                }
            }
            else
            {
                _m_wLeftStepSlider?.hideWnd();
            }
        }

        #region 格子飞行收集表现

        /// <summary>
        /// 销毁所有飞行中的item
        /// </summary>
        private void _discardAllFlyingItem()
        {
            for(int i = 0, count = _m_lFlyingItemList.Count; i < count; i++)
            {
                _discardFlyingItem(_m_lFlyingItemList[i]);
            }
            _m_lFlyingItemList.Clear();
        }

        private void _discardFlyingItem(GGUIWndTileMatchTaskBlockFlyItem _flyItem)
        {
            if(_flyItem == null)
                return;

            if (_m_taskFlyItemCache != null)
                _m_taskFlyItemCache.pushBackCacheItem(_flyItem);
            else
                _flyItem.discard();
        }

        private void _discardFlyingItem(Predicate<GGUIWndTileMatchTaskBlockFlyItem> _match)
        {
            if(_match == null)
                return;
            
            GGUIWndTileMatchTaskBlockFlyItem item;
            // 倒序遍历, 避免移除元素时影响遍历
            for(int count = _m_lFlyingItemList.Count, i = count - 1; i >= 0; i--)
            {
                item = _m_lFlyingItemList[i];
                if(item == null)
                    continue;

                if (_match(item))
                {
                    _discardFlyingItem(item);
                    _m_lFlyingItemList.RemoveAt(i);
                }
            }
        }
        
        #endregion
        
        /// <summary>
        /// 当任务信息发生变化时
        /// </summary>
        private void _onTaskInfoChg()
        {
            // 只需要更新步数信息, 具体任务完成的数量要跟随表现进行变更
            _refreshCurTaskLeftStep();
        }

        /// <summary>
        /// 任务数据变成新的时
        /// </summary>
        private void _onNewTaskInfo()
        {
            if (_m_taskInfo == null)
            {// 若没有当前任务信息, 表示是初始化的任务数据
                _updateCurTaskInfoFormGameLogic();
            }
            else
            {
                // 若有当前任务信息, 表示当前任务已完成, 需要更换新任务, 这里先记录新任务, 刷新要等后面格子表现完成
                if (_m_newTaskInfoList == null)
                    _m_newTaskInfoList = new List<TileMatchTaskInfo>();
                _m_newTaskInfoList.Add(_m_gameLogic?.taskInfo);
            }
        }
        
        private void _oStartDealServer(TileMatchGameLogic.TileMatchGameStateMachineState_DealServer _dealServerState)
        {
            if(_dealServerState == null)
                return;

            int clearBlockCount = 0;
            if (_dealServerState.processLogicAgentsDic != null)
            {
                foreach (var processLogicAgentList in _dealServerState.processLogicAgentsDic.Values)
                {
                    if(processLogicAgentList == null)
                        continue;

                    foreach (var agent in processLogicAgentList)
                    {
                        clearBlockCount += (agent?.getClearBlockNum() ?? 0);
                    }
                }
            }
            
            if (_m_dTaskNeedFlyItemCountDic.TryGetValue(_dealServerState.belongTaskSerialId, out int _count))
            {
                _m_dTaskNeedFlyItemCountDic[_dealServerState.belongTaskSerialId] = _count + clearBlockCount;
            }
            else
            {
                _m_dTaskNeedFlyItemCountDic[_dealServerState.belongTaskSerialId] = clearBlockCount;
            }
        }

        /// <summary>
        /// 当格子被消除
        /// </summary>
        private void _onBlockClear(Vector3 _worldPosition, TileMatchBlockShowRefObj _blockShowRefObj, int _belongTaskSeriaId)
        {
            int wndCurTaskHadGoStep = _m_taskInfo?.hadGoStep ?? 0;//格子被清除时, 当前窗口任务已经走的步数(为了防止遇到弱网情况(推送在回包前到达), 叠加上上一次消除的格子未在下一次进行移动消除前飞行完成 的情况)
            if (_blockShowRefObj != null && _m_wTaskBlockItemContainer != null)
            {
                GGUIWndTileMatchTaskBlockItem taskBlockItem = _m_wTaskBlockItemContainer.getTaskItemWnd(_blockShowRefObj.block_id);
                if (taskBlockItem != null && taskBlockItem.rectTransform != null && _m_taskFlyItemCache != null)
                {
                    GGUIWndTileMatchTaskBlockFlyItem blockFlyItem = _m_taskFlyItemCache.popItem();
                    if (blockFlyItem != null)
                    {
                        long showSerialize = _m_lShowSerialize;
                        long gameLogicSerialize = _m_gameLogic?.startSerialize ?? 0;
                        
                        _m_lFlyingItemList.Add(blockFlyItem);
                        blockFlyItem.showWnd();
                        blockFlyItem.setData(_blockShowRefObj, _belongTaskSeriaId, _worldPosition, taskBlockItem.rectTransform.position,
                            () =>
                            {
                                if(showSerialize != _m_lShowSerialize || !isShow || _m_gameLogic == null || _m_gameLogic.startSerialize != gameLogicSerialize)
                                    return;

                                if (_m_taskInfo == null || _m_taskInfo.serialId != _belongTaskSeriaId)
                                {
                                    Debug.LogError($"当格子飞行结束时, 改格子所属的任务:{_belongTaskSeriaId} 与 当前页面显示任务:{_m_taskInfo?.serialId} 不同, 这是不应该出现的情况, 这里先处理直接更新当前页面需要显示任务");
                                    
                                    // 更新任务
                                    _updateCurTaskInfo();
                                }
                                
                                _discardFlyingItem(blockFlyItem);
                                _m_lFlyingItemList.Remove(blockFlyItem);
                                
                                // 若格子任务 是 当前页面显示任务时
                                if (_m_taskInfo != null && _m_taskInfo.serialId == _belongTaskSeriaId)
                                {
                                    taskBlockItem = _m_wTaskBlockItemContainer?.getTaskItemWnd(_blockShowRefObj.block_id);//重新获取一次格子目标窗口
                                    taskBlockItem?.addDoneNum(1);// 格子增加完成数量
                                }

                                _onBlockFlyComplete(_belongTaskSeriaId, _blockShowRefObj, wndCurTaskHadGoStep);
                            });
                        
                        return;
                    }
                }
            }

            _onBlockFlyComplete(_belongTaskSeriaId, _blockShowRefObj, wndCurTaskHadGoStep);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_belongTaskSeriaId">清除的格子所属任务序列号</param>
        /// <param name="_blockShowRefObj">清除的格子配表数据</param>
        /// <param name="_onClearWndCurTaskHadGoStep">格子清除时, 窗口当前任务已走步数</param>
        private void _onBlockFlyComplete(int _belongTaskSeriaId, TileMatchBlockShowRefObj _blockShowRefObj, int _onClearWndCurTaskHadGoStep)
        {
            if (_m_dTaskNeedFlyItemCountDic.TryGetValue(_belongTaskSeriaId, out int needFlyCount))
            {
                _m_dTaskNeedFlyItemCountDic[_belongTaskSeriaId] = needFlyCount - 1;
            }

            if (_blockShowRefObj != null)
            {
                if(!_m_dTempTaskBlockAddDoneNumDic.TryGetValue(_belongTaskSeriaId, out Dictionary<long, int> blockAddDoneNumDic) || blockAddDoneNumDic == null)
                {
                    blockAddDoneNumDic = new Dictionary<long, int>();
                    _m_dTempTaskBlockAddDoneNumDic[_belongTaskSeriaId] = blockAddDoneNumDic;
                }
                
                if(blockAddDoneNumDic.TryGetValue(_blockShowRefObj.block_id, out int addDoneNum))
                {
                    blockAddDoneNumDic[_blockShowRefObj.block_id] = addDoneNum + 1;
                }
                else
                {
                    blockAddDoneNumDic[_blockShowRefObj.block_id] = 1;
                }
            }
            
            // 若是当前任务格子飞行完成
            if(_m_taskInfo == null || _m_taskInfo.serialId == _belongTaskSeriaId)
            {
                _checkCurTaskBlockFlyComplete(_onClearWndCurTaskHadGoStep);
            }
        }
        
        /// <summary>
        /// 当前任务的格子是否飞行完成
        /// <param name="_onClearWndCurTaskHadGoStep">格子清除时, 窗口当前任务已走步数</param>
        /// </summary>
        private void _checkCurTaskBlockFlyComplete(int _onClearWndCurTaskHadGoStep)
        {
            if(hotfixWnd == null || !isShow)
                return;

            // 若当前任务还有未完成飞行的格子, 则不进行后续任务完成检测
            if (_m_taskInfo != null && _m_dTaskNeedFlyItemCountDic.TryGetValue(_m_taskInfo.serialId, out int needFlyCount) && needFlyCount > 0)
            {
                return;
            }

            if (_m_taskInfo != null)
            {
                // 判断任务是否完成应该先用_m_wTaskBlockItemContainer中的完成状态来判断, 因为_m_taskInfo数据在收到推送时会立刻更新, 此时若遇到弱网情况(推送在回包前到达), 叠加上上一次消除的格子未在下一次进行移动消除前飞行完成
                // 就会导致上一次消除格子飞行完成后, 触发该检查, 但是又由于弱网情况, 任务数据先更新为了已完成
                // (上一次消除任务还未完成, 本次消除任务才完成, 本来应该在本次消除格子飞行完后, 任务更新, 但是由于上一次消除格子飞行太慢, 在上一次消除前就进行了本次消除操作, 任务数据就会在上一次消除格子还没飞行完成时进行更新,
                // 结果上次消除完成后, 触发检查任务完成时, 任务数据已经是完成状态了, 导致任务完成表现提前触发了)
                bool taskIsDone = _m_wTaskBlockItemContainer?.isAllItemCollectedDone() ?? _m_taskInfo.isDone();
                // 若当前任务已完成
                if (taskIsDone)
                {
                    // 旧任务完成表现
                    _playAnimation(hotfixWnd.taskCompleteAnimName, null);
                    
                    // 增加阶段奖励积分
                    if (_m_taskInfo != null && _m_taskInfo.taskRefObj != null)
                    {
                        // Debug.LogError($"==============任务完成 增加分数addScore:{_m_taskInfo.taskRefObj.gain_score}");
                        long gainScore = _m_rGameModel == null ? _m_taskInfo.taskRefObj.gain_score : _m_taskInfo.taskRefObj.gain_score * _m_rGameModel.multiple;
                        _m_wStepRewardWnd?.addStageScore(gainScore);
                        _m_wStepRewardWnd?.addActivityTotalScore(gainScore);
                    }
                    // 客人展示窗口进行成功表现
                    _m_wGuestShowWnd?.addTaskResult(true);
                
                    // 更新当前任务
                    _updateCurTaskInfo();
                }
                // 当前任务未完成 且 超出步数限制时
                else if (_m_taskInfo.taskRefObj != null && _m_taskInfo.taskRefObj.step_limit <= _onClearWndCurTaskHadGoStep)
                {
                    _playAnimation(hotfixWnd.taskFailAniAnimName, null);
                    
                    // 客人展示窗口进行失败表现
                    _m_wGuestShowWnd?.addTaskResult(false);
                    
                    // 更新当前任务
                    _updateCurTaskInfo();
                }
            }
            else
            {
                // 更新当前任务
                _updateCurTaskInfo();
            }
        }
        
        /// <summary>
        /// 刷新当前任务
        /// </summary>
        private void _updateCurTaskInfo()
        {
            // 若当前任务还未完成且未超出步数限制, 则不进行任务更新
            if (_m_taskInfo != null && !_m_taskInfo.isDone() && !_m_taskInfo.reachStepLimit)
                return;

            if (_m_taskInfo != null)
            {
                // 将当前任务的飞行item全部销毁
                _m_dTaskNeedFlyItemCountDic.Remove(_m_taskInfo.serialId);
                _m_dTempTaskBlockAddDoneNumDic.Remove(_m_taskInfo.serialId);
                _discardFlyingItem((_itemWnd)=>
                {
                    return _itemWnd != null && _itemWnd.belongTaskSerialId == _m_taskInfo.serialId;
                });

                _m_taskInfo = null;
            }
            
            // 若存在新增的任务
            if (_m_newTaskInfoList != null && _m_newTaskInfoList.Count > 0)
            {
                TileMatchTaskInfo newTaskInfo = null;
                for (int i = 0, count = _m_newTaskInfoList.Count; i < count; i++)
                {
                    newTaskInfo = _m_newTaskInfoList[i];
                    if(newTaskInfo == null)
                        continue;
                        
                    // 若新的任务已经完成, 增加新完成任务积分
                    if (newTaskInfo.taskRefObj != null && newTaskInfo.isDone())
                    {
                        // Debug.LogError($"==============_updateCurTaskInfo newTaskInfo newTaskInfo.serialId:{newTaskInfo.serialId} 增加分数addScore:{newTaskInfo.taskRefObj.gain_score}");
                        long gainScore = _m_rGameModel == null ? newTaskInfo.taskRefObj.gain_score : newTaskInfo.taskRefObj.gain_score * _m_rGameModel.multiple;
                        _m_wStepRewardWnd?.addStageScore(gainScore);
                        _m_wStepRewardWnd?.addActivityTotalScore(gainScore);
                    }

                    // _m_newTaskInfoList列表的最后一个为最新的任务数据
                    if (i == count - 1)
                    {
                        _m_taskInfo = newTaskInfo;
                    }
                    else if(!newTaskInfo.isDone() && !newTaskInfo.reachStepLimit)// 不是最新任务的话, 且任务未完成且未超出步数限制时, 这里不应该存在这种情况
                    {
                        Debug.LogError($"任务:{newTaskInfo} 未完成且未超出步数限制, 却不是最新任务, 这里忽略该任务数据, 但是不应该存在这种情况, 请检查服务器下发数据");
                    }
                }

                if (_m_taskInfo != null && _m_taskInfo.isDone()) //若最新的任务也已经完成, 直接置空
                {
                    _m_taskInfo = null;
                }
                _m_newTaskInfoList?.Clear();

                // 保留最新任务的飞行item数量统计 和 飞行中item, 其他任务的飞行item全部销毁
                if (_m_taskInfo != null)
                {
                    _m_dTaskNeedFlyItemCountDic.TryGetValue(_m_taskInfo.serialId, out int needFlyCount);
                    _m_dTaskNeedFlyItemCountDic.Clear();
                    _m_dTaskNeedFlyItemCountDic.Add(_m_taskInfo.serialId, needFlyCount);
                    
                    _m_dTempTaskBlockAddDoneNumDic.TryGetValue(_m_taskInfo.serialId, out Dictionary<long, int> blockAddDoneNumDic);
                    _m_dTempTaskBlockAddDoneNumDic.Clear();
                    _m_dTempTaskBlockAddDoneNumDic.Add(_m_taskInfo.serialId, blockAddDoneNumDic);
                    
                    _discardFlyingItem((_itemWnd)=>
                    {
                        return _itemWnd != null && _itemWnd.belongTaskSerialId != _m_taskInfo.serialId;
                    });
                }
                else
                {
                    _m_dTaskNeedFlyItemCountDic.Clear();
                    _m_dTempTaskBlockAddDoneNumDic.Clear();
                    _discardAllFlyingItem();
                }
            }

            _refreshCurTaskShow(false);
        }
        
        #region 动画

        /// <summary>
        /// 播放动画
        /// </summary>
        private void _playAnimation(string _aniName, Action _playDone)
        {
            if (hotfixWnd == null || hotfixWnd.ani == null || string.IsNullOrEmpty(_aniName))
            {
                _playDone?.Invoke();
                return;
            }

            hotfixWnd.ani.Play(_aniName, _playDone);
        }

        private void _sample(string _aniName, float _normalizedTime)
        {
            if (hotfixWnd == null || hotfixWnd.ani == null || string.IsNullOrEmpty(_aniName))
            {
                return;
            }
            
            hotfixWnd.ani.Sample(_aniName, _normalizedTime);
        }

        #endregion
    }
}