using System;
using System.Collections.Generic;
using ALPackage;
using Hotfix.Common.TileMatchObj;
using IlRuntimeLitJson;
using JetBrains.Annotations;

namespace Hotfix
{
    public partial class TileMatchGameLogic
    {
        /// <summary>
        /// 普通消除合成 逻辑处理代理(普通格子的交换消除合成)
        /// </summary>
        public class TileMatchProcessLogicAgent_Composite : _ATileMatchProcessLogicAgent
        {
            private TileMatch_Composite _m_info;
            
            public TileMatchProcessLogicAgent_Composite([NotNull] TileMatchGameLogic _gameLogic, [NotNull] TileMatch_LogicInfo _serverMatchLogicInfo) : base(_gameLogic, _serverMatchLogicInfo)
            {
            }

            // public override string dataStr { get { return TileMatchUtil.getTileMatch_CompositeStr(_m_info); } }
            public override string dataStr { get { return HotfixGCommon.GetInfoPropertys(_m_info); } }

            protected override void _parseLogicData(byte[] _logicDataByte)
            {
                _m_info = _parseLogicData<TileMatch_Composite>(_logicDataByte);
            }

            public override int getClearBlockNum()
            {
                if(_m_info == null)
                    return 0;

                List<int> clearBlockIndexList = _m_info.getRemoveBlockIndexList();
                return clearBlockIndexList?.Count ?? 0;
            }

            protected override void _dealProcess(Action _dealComplete)
            {
                if(_AALMonoMain.instance.showDebugOutput)
                    Debug.Log_EditorOnly($"TileMatchProcessLogicAgent_Composite dealProcess start : {dataStr}");

                if (_m_info == null)
                {
                    _dealComplete?.Invoke();
                    return;
                }

                long gameLogicSerialize = _m_gameLogic.startSerialize;
                
                ALStepCounter stepCounter = new ALStepCounter();

                ALProcess process = ALProcess.CreateProcess();
                process
                    .addDelegateProcess((_complete) =>
                    {
                        if(gameLogicSerialize != _m_gameLogic.startSerialize)
                            return;
                        
                        // 移除的格子索引列表
                        List<int> removeBlockList = _m_info.getRemoveBlockIndexList();
                        int removeBlockCount = removeBlockList?.Count ?? 0;
                        if (removeBlockList == null || removeBlockCount <= 0)
                        {
                            _complete?.Invoke();
                        }
                        else
                        {
                            stepCounter.resetAll();
                            stepCounter.chgTotalStepCount(removeBlockCount);
                            stepCounter.regAllDoneDelegate(_complete);

                            for (int i = 0; i < removeBlockCount; i++)
                            {
                                int removeBlockIndex = removeBlockList[i];
                                TileMatchBlockInfo blockInfo = _m_gameLogic._getBlockInfoFormArray(removeBlockIndex);//获取格子数据
                                if (blockInfo == null)
                                {
                                    stepCounter.addDoneStepCount();
                                }
                                else
                                {
                                    _m_gameLogic._clearBlock(blockInfo, true, belongTaskSerialId, (_blockShow, _beforePushBackDealDone) =>
                                    {
                                        // 播放格子清除动画
                                        if(_blockShow == null)
                                            _beforePushBackDealDone?.Invoke();
                                        else
                                            _blockShow.playByClearAnimation(_beforePushBackDealDone);
                                    }, () =>
                                    {
                                        stepCounter.addDoneStepCount();
                                    });
                                }
                            }
                        }
                    })
                    .addDelegateProcess((_complete) =>
                    {
                        if(gameLogicSerialize != _m_gameLogic.startSerialize)
                            return;
                    
                        List<Hotfix.Common.TileMatchObj.TileMatch_BlockInfo> genBlockInfoList = _m_info.getGenBlockList();
                        int genBlockCount = genBlockInfoList?.Count ?? 0;
                        if (genBlockInfoList == null || genBlockCount <= 0)
                        {
                            _complete?.Invoke();
                            return;
                        }
                        
                        stepCounter.resetAll();
                        stepCounter.chgTotalStepCount(genBlockCount);
                        stepCounter.regAllDoneDelegate(() =>
                        {
                            if(gameLogicSerialize != _m_gameLogic.startSerialize)
                                return;
                            
                            _complete?.Invoke();
                        });

                        TileMatch_BlockInfo genServerBlockInfo = null;
                        for (int i = 0; i < genBlockCount; i++)
                        {
                            genServerBlockInfo = genBlockInfoList[i];
                            if (genServerBlockInfo == null || genServerBlockInfo.getBaseInfo() == null)
                            {
                                stepCounter.addDoneStepCount();
                            }
                            else
                            {
                                TileMatchBlockInfo blockInfo = _m_gameLogic._getBlockInfoFormArray(genServerBlockInfo.getIndex());//获取格子数据
                                if (blockInfo == null)
                                {
                                    stepCounter.addDoneStepCount();
                                }
                                else
                                {
                                    _m_gameLogic._fillBlock(blockInfo, genServerBlockInfo.getBaseInfo().getBlockId());
                                    if (blockInfo.blockShow == null)
                                    {
                                        stepCounter.addDoneStepCount();
                                    }
                                    else
                                    {
                                        // 播放格子创建动画
                                        blockInfo.blockShow.playCreateAnimation(() =>
                                        {
                                            // 生成动画播放完成后播放idle动画
                                            blockInfo.blockShow.playIdleAnimation();
                                        });
                                        stepCounter.addDoneStepCount();
                                    }
                                }
                            }
                        }
                    })
                    .addProcess(() =>
                    {
                        if(gameLogicSerialize != _m_gameLogic.startSerialize)
                            return;
                        
                        _dealComplete?.Invoke();
                    })
                    .deal();
            }
        }
    }
}