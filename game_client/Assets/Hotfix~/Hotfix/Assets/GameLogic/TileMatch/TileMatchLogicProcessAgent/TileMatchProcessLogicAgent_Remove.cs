using System;
using System.Collections.Generic;
using ALPackage;
using Hotfix.Common.TileMatchObj;
using Hotfix.TileMatchEnum;
using IlRuntimeLitJson;
using JetBrains.Annotations;
using UnityEngine;

namespace Hotfix
{
    public partial class TileMatchGameLogic
    {
        /// <summary>
        /// 单独的特殊格消除 逻辑处理代理(包括彩虹的特殊格与普通格交换消除、包括彩虹的特殊格被动触发消除、彩虹格与非彩虹的特殊格交换后将普通格转化成的特殊格的消除)
        /// </summary>
        public class TileMatchProcessLogicAgent_Remove : _ATileMatchProcessLogicAgent
        {
            private TileMatch_Remove _m_info;
            
            public TileMatchProcessLogicAgent_Remove([NotNull] TileMatchGameLogic _gameLogic, [NotNull] TileMatch_LogicInfo _serverMatchLogicInfo) : base(_gameLogic, _serverMatchLogicInfo)
            {
            }

            // public override string dataStr { get { return TileMatchUtil.getTileMatch_RemoveStr(_m_info); } }
            public override string dataStr { get { return HotfixGCommon.GetInfoPropertys(_m_info); } }

            protected override void _parseLogicData(byte[] _logicDataByte)
            {
                _m_info = _parseLogicData<TileMatch_Remove>(_logicDataByte);
            }

            public override int getClearBlockNum()
            {
                if(_m_info == null)
                    return 0;

                List<int> clearBlockIndexList = _m_info.getRemoveBlockIndexList();
                int count = clearBlockIndexList?.Count ?? 0;
                if (clearBlockIndexList != null)
                {
                    // 判断清除列表中是否有触发格子本身, 若没有则加1
                    if (!clearBlockIndexList.Contains(_m_info.getTriggerBlockIndex()))
                        count++;
                }

                return count;
            }
            
            protected override void _dealProcess(Action _dealComplete)
            {
                if(_AALMonoMain.instance.showDebugOutput)
                    Debug.Log_EditorOnly($"TileMatchProcessLogicAgent_Remove dealProcess start : {dataStr}");
                
                if (_m_info == null)
                {
                    _dealComplete?.Invoke();
                    return;
                }

                TileMatchBlockInfo triggerBoxInfo = _m_gameLogic._getBlockInfoFormArray(_m_info.getTriggerBlockIndex());
                
                long gameLogicSerialize = _m_gameLogic.startSerialize;
                
                ALProcess process = ALProcess.CreateProcess();
                process
                    .addDelegateProcess((_complete) =>
                    {
                        // 播放格子触发特效
                        if (gameLogicSerialize != _m_gameLogic.startSerialize)
                            return;
                        
                        if (triggerBoxInfo != null && triggerBoxInfo.blockShow != null)
                        {
                            triggerBoxInfo.blockShow.proactiveUniteNormalTriggerShow(_complete);
                        }
                        else
                        {
                            _complete?.Invoke();
                        }
                    })
                    .addDelegateProcess((_complete) =>
                    {
                        if (gameLogicSerialize != _m_gameLogic.startSerialize)
                            return;

                        switch (logicType)
                        {
                            // 若触发的是彩虹格子, 比较特殊, 需要播放飞行特效
                            case ETileMatch_LogicType.RAINBOW:
                                if (triggerBoxInfo != null && triggerBoxInfo.blockShow != null && _m_info != null)
                                {
                                    _showRainbowFlySfx(triggerBoxInfo, _m_info.getRemoveBlockIndexList(), _complete);
                                }
                                else
                                {
                                    _complete?.Invoke();
                                }
                                break;
                            
                            // 其他类型的格子触发, 展示不做什么特殊表现
                            default:
                                _complete?.Invoke();
                                break;
                        }
                    })
                    .addDelegateProcess((_complete) =>
                    {
                        if (gameLogicSerialize != _m_gameLogic.startSerialize)
                            return;

                        if (triggerBoxInfo == null || triggerBoxInfo.blockShow == null || _m_info == null)
                        {
                            _complete?.Invoke();
                            return;
                        }
                        
                        // 播放格子清除特效
                        ALStepCounter stepCounter = new ALStepCounter();
                        stepCounter.chgTotalStepCount(2);
                        stepCounter.regAllDoneDelegate(_complete);

                        _dealList(_m_info.getRemoveBlockIndexList(), (_posIndex, _itemDealDone) =>
                        {
                            TileMatchBlockInfo targetBlockInfo = _m_gameLogic._getBlockInfoFormArray(_posIndex);
                            // 跳过触发格子本身, 触发格子自身消除在下面独立进行
                            if(targetBlockInfo == null || targetBlockInfo.logicPos == triggerBoxInfo.logicPos)
                                _itemDealDone?.Invoke();
                            else
                                _m_gameLogic._clearBlock(targetBlockInfo, true, belongTaskSerialId, (_targetBlockShow, _beforePushBackDealDone) =>
                                {
                                    if(_targetBlockShow == null)
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
                        
                        // 清除触发格
                        _m_gameLogic._clearBlock(triggerBoxInfo, true, belongTaskSerialId, (_blockShow, _beforePushBackDealDone) =>
                        {
                            if(_blockShow == null)
                                _beforePushBackDealDone?.Invoke();
                            else
                            {
                                _blockShow.playByClearAnimation(_beforePushBackDealDone);
                            }
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

            /// <summary>
            /// 显示彩虹飞行特效
            /// </summary>
            /// <param name="_triggerBlockInfo"></param>
            /// <param name="_clearBlockIndexList"></param>
            /// <param name="_complete"></param>
            private void _showRainbowFlySfx(TileMatchBlockInfo _triggerBlockInfo, List<int> _clearBlockIndexList, Action _complete)
            {
                if (_triggerBlockInfo == null || _triggerBlockInfo.blockShow == null || _clearBlockIndexList == null || _clearBlockIndexList.Count <= 0)
                {
                    _complete?.Invoke();
                    return;
                }

                long gameLogicSerialize = _m_gameLogic.startSerialize;
                GGUIWndTileMatchCheckerSubRainbow rainbowWnd = _triggerBlockInfo.blockShow.getCheckerSubWnd<GGUIWndTileMatchCheckerSubRainbow>();
                if (rainbowWnd == null)
                {
                    _complete?.Invoke();
                    return;
                }
                
                _dealList(_clearBlockIndexList, (_posIndex, _itemDealDone) =>
                {
                    TileMatchBlockInfo targetBlockInfo = _m_gameLogic._getBlockInfoFormArray(_posIndex);
                    if(targetBlockInfo == null || targetBlockInfo.logicPos == _triggerBlockInfo.logicPos)
                        _itemDealDone?.Invoke();
                    else
                        rainbowWnd.showUniteNormalFlySfx(targetBlockInfo.logicPos, _itemDealDone);
                }, () =>
                {
                    if(gameLogicSerialize != _m_gameLogic.startSerialize)
                        return;
                    
                    _complete?.Invoke();
                });
            }
        }
    }
}