using System;
using System.Collections.Generic;
using ALPackage;
using Hotfix.Common.TileMatchObj;
using IlRuntimeLitJson;
using JetBrains.Annotations;
using UnityEngine;

namespace Hotfix
{
    public partial class TileMatchGameLogic
    {
        /// <summary>
        /// 格子生成掉落 逻辑处理代理(已存在的格子掉落,以及生成新的格子)
        /// </summary>
        public class TileMatchProcessLogicAgent_Drop : _ATileMatchProcessLogicAgent
        {
            private TileMatch_Drop _m_info;
            
            public TileMatchProcessLogicAgent_Drop([NotNull] TileMatchGameLogic _gameLogic, [NotNull] TileMatch_LogicInfo _serverMatchLogicInfo) : base(_gameLogic, _serverMatchLogicInfo)
            {
            }

            // public override string dataStr { get { return TileMatchUtil.getTileMatch_DropStr(_m_info); } }
            public override string dataStr { get { return HotfixGCommon.GetInfoPropertys(_m_info); } }

            protected override void _parseLogicData(byte[] _logicDataByte)
            {
                _m_info = _parseLogicData<TileMatch_Drop>(_logicDataByte);
            }

            public override int getClearBlockNum()
            {
                // 掉落过程不涉及格子消除, 下面有两个调用_clearBlock的地方, 但都是为了容错处理, 正常情况下不应该有格子被消除
                return 0;
            }

            protected override void _dealProcess(Action _dealComplete)
            {
                if(_AALMonoMain.instance.showDebugOutput)
                    Debug.Log_EditorOnly($"TileMatchProcessLogicAgent_Drop dealProcess start : {dataStr}");

                if (_m_info == null)
                {
                    _dealComplete?.Invoke();
                    return;
                }

                long gameLogicSerialize = _m_gameLogic.startSerialize;
                
                ALProcess alProcess = ALProcess.CreateProcess();

                alProcess
                    .addDelegateProcess((_complete) =>
                    {
                        ALStepCounter stepCounter = new ALStepCounter();
                        stepCounter.chgTotalStepCount(2);
                        stepCounter.regAllDoneDelegate(_complete);

                        _dealBlockPosChgProcess(_m_info.getPosChgList(), stepCounter.addDoneStepCount);
                        _dealGenBlockProcess(_m_info.getGenBlockList(), stepCounter.addDoneStepCount);
                    })
                    .addProcess(() =>
                    {
                        if(gameLogicSerialize != _m_gameLogic.startSerialize)
                            return;
                        
                        _dealComplete?.Invoke();
                    })
                    .deal();
            }
            
            /// <summary>
            /// 处理格子位置变化的过程
            /// </summary>
            /// <returns></returns>
            private ALProcess _dealBlockPosChgProcess(List<Hotfix.Common.TileMatchObj.TileMatch_BlockPosChg> _blockPosChgList)
            {
                ALProcess process = ALProcess.CreateProcess();
                
                process.addDelegateProcess((_complete) =>
                {
                    _dealList(_blockPosChgList, (_posChgInfo, itemDealDone) =>
                    {
                        if (_posChgInfo == null)
                            itemDealDone?.Invoke();
                        else
                        {
                            TileMatchBlockInfo oriBlockInfo = _m_gameLogic._getBlockInfoFormArray(_posChgInfo.getOriBlockIndex());// 起始位置格子信息
                            TileMatchBlockInfo tarBlockInfo = _m_gameLogic._getBlockInfoFormArray(_posChgInfo.getTarBlockIndex());// 目标位置格子信息
                            if(oriBlockInfo == null || tarBlockInfo == null)
                                itemDealDone?.Invoke();
                            else
                            {
                                _m_gameLogic._clearBlock(tarBlockInfo, false, 0, null);//清除目标位置格子数据(正确情况下落的目标位置格子应该已经被清空, 这里主要做个容错)
                                GGUIWndTileMatchChecker oriBlockShow = oriBlockInfo.blockShow;// 获取起始格子表现
                                long oriBlockRefId = oriBlockInfo.blockRefId;// 获取起始格子配表id
                                oriBlockInfo.resetBlockInfo();// 重置起始格子数据
                                oriBlockInfo.resetBlockShow();//重置起始格子表现
                                tarBlockInfo.setBlockRefId(oriBlockRefId);//设置目标位置格子数据
                                tarBlockInfo.setBlockShow(oriBlockShow);//设置目标位置格子表现

                                if (tarBlockInfo.blockShow != null)
                                {
                                    // 格子进行下落
                                    tarBlockInfo.blockShow.fallTo(_m_gameLogic._m_gameShow.getBlockUiLocalPosition(TileMatchUtil.indexToVector2(_posChgInfo.getTarBlockIndex())),
                                        () =>
                                        {
                                            // 下落完成后播放idle动画
                                            tarBlockInfo.blockShow.playIdleAnimation();
                                            itemDealDone?.Invoke();
                                        });
                                }
                                else
                                {
                                    itemDealDone?.Invoke();
                                }
                            }
                        }
                    }, () =>
                    {
                        _complete?.Invoke();
                    });
                })
                .deal();

                return process;
            }

            /// <summary>
            /// 处理格子位置变化的过程
            /// </summary>
            /// <returns></returns>
            private void _dealBlockPosChgProcess(List<Hotfix.Common.TileMatchObj.TileMatch_BlockPosChg> _blockPosChgList, Action _complete)
            {
                ALProcess process = ALProcess.CreateProcess();
                
                process.addDelegateProcess((_complete) =>
                    {
                        _dealList(_blockPosChgList, (_posChgInfo, itemDealDone) =>
                        {
                            if (_posChgInfo == null)
                                itemDealDone?.Invoke();
                            else
                            {
                                TileMatchBlockInfo oriBlockInfo = _m_gameLogic._getBlockInfoFormArray(_posChgInfo.getOriBlockIndex());// 起始位置格子信息
                                TileMatchBlockInfo tarBlockInfo = _m_gameLogic._getBlockInfoFormArray(_posChgInfo.getTarBlockIndex());// 目标位置格子信息
                                if(oriBlockInfo == null || tarBlockInfo == null)
                                    itemDealDone?.Invoke();
                                else
                                {
                                    _m_gameLogic._clearBlock(tarBlockInfo, false, 0, null);//清除目标位置格子数据(正确情况下落的目标位置格子应该已经被清空, 这里主要做个容错)
                                    GGUIWndTileMatchChecker oriBlockShow = oriBlockInfo.blockShow;// 获取起始格子表现
                                    long oriBlockRefId = oriBlockInfo.blockRefId;// 获取起始格子配表id
                                    oriBlockInfo.resetBlockInfo();// 重置起始格子数据
                                    oriBlockInfo.resetBlockShow();//重置起始格子表现
                                    tarBlockInfo.setBlockRefId(oriBlockRefId);//设置目标位置格子数据
                                    tarBlockInfo.setBlockShow(oriBlockShow);//设置目标位置格子表现

                                    if (tarBlockInfo.blockShow != null)
                                    {
                                        // 格子进行下落
                                        tarBlockInfo.blockShow.fallTo(_m_gameLogic._m_gameShow.getBlockUiLocalPosition(TileMatchUtil.indexToVector2(_posChgInfo.getTarBlockIndex())),
                                            () =>
                                            {
                                                // 下落完成后播放idle动画
                                                tarBlockInfo.blockShow.playIdleAnimation();
                                                itemDealDone?.Invoke();
                                            });
                                    }
                                    else
                                    {
                                        itemDealDone?.Invoke();
                                    }
                                }
                            }
                        }, () =>
                        {
                            _complete?.Invoke();
                        });
                    })
                    .addProcess(() =>
                    {
                        _complete?.Invoke();
                    })
                    .deal();
            }
            
            /// <summary>
            /// 生成格子表现过程
            /// </summary>
            /// <returns></returns>
            private ALProcess _dealGenBlockProcess(List<Hotfix.Common.TileMatchObj.TileMatch_BlockInfo> _genBlockList)
            {
                ALProcess process = ALProcess.CreateProcess();

                process.addDelegateProcess((_complete) =>
                {
                    Dictionary<int, List<TileMatchBlockInfo>> verticalFallBoxInfoDic = _getVerticalFallBoxInfoDic(_genBlockList);
                    if (verticalFallBoxInfoDic == null || verticalFallBoxInfoDic.Count <= 0)
                    {
                        _complete?.Invoke();
                        return;
                    }

                    ALStepCounter stepCounter = new ALStepCounter();
                    stepCounter.chgTotalStepCount(verticalFallBoxInfoDic.Count);
                    stepCounter.regAllDoneDelegate(_complete);
                    
                    foreach (var blockList in verticalFallBoxInfoDic.Values)
                    {
                        _dealVerticalFall(blockList, stepCounter.addDoneStepCount);
                    }
                })
                .deal();

                return process;
            }
            
            /// <summary>
            /// 生成格子表现过程
            /// </summary>
            /// <returns></returns>
            private void _dealGenBlockProcess(List<Hotfix.Common.TileMatchObj.TileMatch_BlockInfo> _genBlockList, Action _complete)
            {
                ALProcess process = ALProcess.CreateProcess();

                process.addDelegateProcess((_complete) =>
                    {
                        Dictionary<int, List<TileMatchBlockInfo>> verticalFallBoxInfoDic = _getVerticalFallBoxInfoDic(_genBlockList);
                        if (verticalFallBoxInfoDic == null || verticalFallBoxInfoDic.Count <= 0)
                        {
                            _complete?.Invoke();
                            return;
                        }

                        ALStepCounter stepCounter = new ALStepCounter();
                        stepCounter.chgTotalStepCount(verticalFallBoxInfoDic.Count);
                        stepCounter.regAllDoneDelegate(_complete);
                    
                        foreach (var blockList in verticalFallBoxInfoDic.Values)
                        {
                            _dealVerticalFall(blockList, stepCounter.addDoneStepCount);
                        }
                    })
                    .addProcess(() =>
                    {
                        _complete?.Invoke();
                    })
                    .deal();
            }

            /// <summary>
            /// 纵向掉落格子信息，字典的key是x, 值是该纵向掉落的所有格子
            /// </summary>
            /// <returns></returns>
            private Dictionary<int, List<TileMatchBlockInfo>> _getVerticalFallBoxInfoDic(List<Hotfix.Common.TileMatchObj.TileMatch_BlockInfo> _genBlockList)
            {
                Dictionary<int, List<TileMatchBlockInfo>> verticalFallBoxInfoDic = new Dictionary<int, List<TileMatchBlockInfo>>();
                if (_genBlockList == null || _genBlockList.Count <= 0)
                    return verticalFallBoxInfoDic;
                
                int boxCount = _genBlockList.Count;
                for(int i = 0;i < boxCount; i++)
                {
                    Hotfix.Common.TileMatchObj.TileMatch_BlockInfo blockInfo = _genBlockList[i];
                    if (blockInfo == null || blockInfo.getBaseInfo() == null)
                        continue;

                    TileMatchBlockInfo tileMatchBlockInfo = _m_gameLogic._getBlockInfoFormArray(blockInfo.getIndex());
                    if (tileMatchBlockInfo == null)
                        continue;

                    _m_gameLogic._fillBlock(tileMatchBlockInfo, blockInfo.getBaseInfo().getBlockId());
                    if(tileMatchBlockInfo.blockShow != null)
                        tileMatchBlockInfo.blockShow.playCreateAnimation(null);
                    
                    int x = tileMatchBlockInfo.logicPos.x;
                    if (!verticalFallBoxInfoDic.TryGetValue(x, out List<TileMatchBlockInfo> blockList) || blockList == null)
                    {
                        blockList = new List<TileMatchBlockInfo>();
                        verticalFallBoxInfoDic[x] = blockList;
                    }
                    
                    blockList.Add(tileMatchBlockInfo);
                }

                foreach (var blockList in verticalFallBoxInfoDic.Values)
                {
                    if(blockList == null)
                        continue;
                    
                    // 按照Y从小到大排序
                    blockList.Sort((_blockA, _blockB) =>
                    {
                        if (_blockB == null)
                            return -1;
                        if (_blockA == null)
                            return 1;
                        if (object.ReferenceEquals(_blockA, _blockB))
                            return 0;

                        return _blockA.logicPos.y.CompareTo(_blockB.logicPos.y);
                    });
                }
                
                return verticalFallBoxInfoDic;
            }

            /// <summary>
            /// 执行纵向掉落
            /// </summary>
            /// <param name="_blockList"></param>
            private void _dealVerticalFall(List<TileMatchBlockInfo> _blockList, Action _complete)
            {
                if (_blockList == null || _blockList.Count <= 0)
                {
                    _complete?.Invoke();
                    return;
                }

                int blockCount = _blockList.Count;
                ALStepCounter stepCounter = new ALStepCounter();
                stepCounter.chgTotalStepCount(blockCount);
                stepCounter.regAllDoneDelegate(_complete);
                
                for (int i = 0; i < blockCount; i++)
                {
                    TileMatchBlockInfo blockInfo = _blockList[i];
                    if (blockInfo == null || blockInfo.blockShow == null)
                    {
                        stepCounter.addDoneStepCount();
                        continue;
                    }

                    Vector2 fallStartPos = _m_gameLogic._m_gameShow.getBlockUiLocalPosition(new Vector2Int(blockInfo.logicPos.x, TileMatchUtil.row + i));
                    Vector2 fallEndPos = _m_gameLogic._m_gameShow.getBlockUiLocalPosition(blockInfo.logicPos);
                    blockInfo.blockShow.fallTo(fallStartPos, fallEndPos, ()=>
                    {
                        // 下落完成后播放idle动画
                        blockInfo.blockShow.playIdleAnimation();
                        stepCounter.addDoneStepCount();
                    });
                }
            }
        }
    }
}