using System;
using System.Collections.Generic;
using ALPackage;
using Hotfix.Common.TileMatchObj;
using Hotfix.TileMatchEnum;
using IlRuntimeLitJson;
using JetBrains.Annotations;

namespace Hotfix
{
    public partial class TileMatchGameLogic
    {
        /// <summary>
        /// 组合道具消除 逻辑处理代理(特殊道具间交换消除, 不包括彩虹和非彩虹的特殊格直接的交换消除, 但是包括彩虹与彩虹间的交换消除)
        /// </summary>
        public class TileMatchProcessLogicAgent_CombineRemove : _ATileMatchProcessLogicAgent
        {
            private TileMatch_CombineRemove _m_info;
            
            public TileMatchProcessLogicAgent_CombineRemove([NotNull] TileMatchGameLogic _gameLogic, [NotNull] TileMatch_LogicInfo _serverMatchLogicInfo) : base(_gameLogic, _serverMatchLogicInfo)
            {
            }

            // public override string dataStr { get { return TileMatchUtil.getTileMatch_CombineRemoveStr(_m_info); } }
            public override string dataStr { get { return HotfixGCommon.GetInfoPropertys(_m_info); } }

            protected override void _parseLogicData(byte[] _logicDataByte)
            {
                _m_info = _parseLogicData<TileMatch_CombineRemove>(_logicDataByte);
            }

            public override int getClearBlockNum()
            {
                if(_m_info == null)
                    return 0;

                List<int> clearBlockIndexList = _m_info.getRemoveBlockIndexList();
                int count = clearBlockIndexList?.Count ?? 0;
                if (clearBlockIndexList != null)
                {
                    // 判断清除列表中是否包含主触发格子和子触发格子, 若不包含则加上
                    if (!clearBlockIndexList.Contains(_m_info.getMainTriggerBlockIndex()))
                        count++;
                    if (!clearBlockIndexList.Contains(_m_info.getSubTriggerBlockIndex()))
                        count++;
                }

                return count;
            }
            
            
            protected override void _dealProcess(Action _dealComplete)
            {
                if(_AALMonoMain.instance.showDebugOutput)
                    Debug.Log_EditorOnly($"TileMatchProcessLogicAgent_CombineRemove dealProcess start : {dataStr}");

                if (_m_info == null)
                {
                    _dealComplete?.Invoke();
                    return;
                }
                
                // 主触发方块信息
                TileMatchBlockInfo mainTriggerBoxInfo = _m_gameLogic._getBlockInfoFormArray(_m_info.getMainTriggerBlockIndex());
                // 子触发方块信息
                TileMatchBlockInfo subTriggerBoxInfo = _m_gameLogic._getBlockInfoFormArray(_m_info.getSubTriggerBlockIndex());
                
                long gameLogicSerialize = _m_gameLogic.startSerialize;
                
                ALProcess process = ALProcess.CreateProcess();
                process
                    .addDelegateProcess((_complete) =>
                    {
                        // 播放触发格子的消除特效
                        if (gameLogicSerialize != _m_gameLogic.startSerialize)
                            return;

                        if (_m_info == null || mainTriggerBoxInfo == null || mainTriggerBoxInfo.blockShow == null || subTriggerBoxInfo == null)
                        {
                            _complete?.Invoke();
                        }
                        else
                        {
                            switch (subTriggerBoxInfo.tileMatchBlockRefObj?.type ?? ETileMatch_BlockType.NONE)
                            {
                                case ETileMatch_BlockType.BOOM:
                                    mainTriggerBoxInfo.blockShow.proactiveUniteBoomTriggerShow(subTriggerBoxInfo.logicPos, null);
                                    _complete?.Invoke();
                                    break;
                                case ETileMatch_BlockType.ROCKET:
                                    mainTriggerBoxInfo.blockShow.proactiveUniteRocketTriggerShow(subTriggerBoxInfo.logicPos, null);
                                    _complete?.Invoke();
                                    break;
                                case ETileMatch_BlockType.RAINBOW:
                                    // 若被触发格子为彩虹格子, 那么主动触发的应该也要为彩虹格子(这就是规则, 彩虹格子与包括彩虹的特殊格子交换消除时, 主动触发的格子一定彩虹格子)
                                    // 且 若彩虹格与非彩虹的特殊格交换时, 会通过TileMatchProcessLogicAgent_RainbowTrans进行表现, 不应该在这里, 彩虹格与普通格交换时, 会通过TileMatchProcessLogicAgent_Remove进行表现, 不应该在这里
                                    // 若有若被动触发的格子为彩虹格时, 主动触发的格子也应该为彩虹格子
                                    if (mainTriggerBoxInfo.tileMatchBlockRefObj != null && mainTriggerBoxInfo.tileMatchBlockRefObj.type == ETileMatch_BlockType.RAINBOW)
                                    {
                                        GGUIWndTileMatchCheckerSubRainbow rainbowSub = mainTriggerBoxInfo.blockShow.getCheckerSubWnd<GGUIWndTileMatchCheckerSubRainbow>();
                                        if (rainbowSub != null)
                                        {
                                            rainbowSub.uniteRainbowTriggerShow(subTriggerBoxInfo.logicPos, () =>
                                            {
                                                rainbowSub.showUniteRainbowCheckerboardClear();
                                                _complete?.Invoke();
                                            });
                                        }
                                        else
                                        {
                                            _complete?.Invoke();
                                        }
                                    }
                                    else
                                    {//若主动触发的格子不是彩虹格
                                        Debug.LogError("TileMatchProcessLogicAgent_CombineRemove dealProcess error: mainTriggerBoxInfo is not rainbow block, but subTriggerBoxInfo is rainbow block.");
                                        _complete?.Invoke();
                                    }
                                    break;
                                default:
                                    _complete?.Invoke();
                                    break;
                            }
                        }
                    })
                    .addDelegateProcess((_complete) =>
                    {
                        if (gameLogicSerialize != _m_gameLogic.startSerialize)
                            return;

                        if (_m_info == null)
                        {
                            _complete?.Invoke();
                            return;
                        }
                        
                        // 播放格子清除特效
                        ALStepCounter stepCounter = new ALStepCounter();
                        stepCounter.chgTotalStepCount(3);
                        stepCounter.regAllDoneDelegate(_complete);

                        _dealList(_m_info.getRemoveBlockIndexList(), (_posIndex, _itemDealDone) =>
                        {
                            TileMatchBlockInfo targetBlockInfo = _m_gameLogic._getBlockInfoFormArray(_posIndex);
                            // 将消除方块中的主触发格子和子触发格子排除在外
                            if(targetBlockInfo == null || mainTriggerBoxInfo == null || targetBlockInfo.logicPos == mainTriggerBoxInfo.logicPos
                               || subTriggerBoxInfo == null || targetBlockInfo.logicPos == subTriggerBoxInfo.logicPos)
                                _itemDealDone?.Invoke();
                            else
                                _m_gameLogic._clearBlock(targetBlockInfo, true, belongTaskSerialId, (_targetBlockShow, _beforePushBackDealDone) =>
                                {
                                    if (_targetBlockShow == null)
                                        _beforePushBackDealDone?.Invoke();
                                    else
                                        _targetBlockShow.playByClearAnimation(_beforePushBackDealDone);
                                }, () =>
                                {
                                    _itemDealDone?.Invoke();
                                });
                        }, () =>
                        {
                            stepCounter.addDoneStepCount();
                        });
                        
                        // 主触发格消除
                        _m_gameLogic._clearBlock(mainTriggerBoxInfo, true, belongTaskSerialId, (_blockShow, _beforePushBackDealDone) =>
                        {
                            if(_blockShow == null)
                                _beforePushBackDealDone?.Invoke();
                            else
                                _blockShow.playByClearAnimation(_beforePushBackDealDone);
                        }, () =>
                        {
                            stepCounter.addDoneStepCount();
                        });
                        
                        // 子触发格消除
                        _m_gameLogic._clearBlock(subTriggerBoxInfo, true, belongTaskSerialId, (_blockShow, _beforePushBackDealDone) =>
                        {
                            if(_blockShow == null)
                                _beforePushBackDealDone?.Invoke();
                            else
                                _blockShow.playByClearAnimation(_beforePushBackDealDone);
                        }, () =>
                        {
                            stepCounter.addDoneStepCount();
                        });
                    })
                    .addProcess(() =>
                    {
                        if (gameLogicSerialize != _m_gameLogic.startSerialize)
                            return;
                        
                        _dealComplete?.Invoke();
                    })
                    .deal();
            }
        }
    }
}