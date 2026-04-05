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
        /// 彩虹转化 逻辑处理代理(彩虹特殊格和其他特殊格组合的转化过程, 将普通格转化成特殊格子后的消除会通过TileMatchProcessLogicAgent_Remove处理)
        /// </summary>
        public class TileMatchProcessLogicAgent_RainbowTrans : _ATileMatchProcessLogicAgent
        {
            private TileMatch_RainbowTrans _m_info;
            
            // public override string dataStr { get { return TileMatchUtil.getTileMatch_RainbowTransStr(_m_info); } }
            public override string dataStr { get { return HotfixGCommon.GetInfoPropertys(_m_info); } }

            public TileMatchProcessLogicAgent_RainbowTrans([NotNull] TileMatchGameLogic _gameLogic, [NotNull] TileMatch_LogicInfo _serverMatchLogicInfo) : base(_gameLogic, _serverMatchLogicInfo)
            {
            }

            protected override void _parseLogicData(byte[] _logicDataByte)
            {
                _m_info = _parseLogicData<TileMatch_RainbowTrans>(_logicDataByte);
            }

            public override int getClearBlockNum()
            {
                if(_m_info == null)
                    return 0;

                List<int> clearBlockIndexList = _m_info.getBeTransIndexList();
                return clearBlockIndexList?.Count ?? 0;
            }

            protected override void _dealProcess(Action _dealComplete)
            {
                if(_AALMonoMain.instance.showDebugOutput)
                    Debug.Log_EditorOnly($"TileMatchProcessLogicAgent_RainbowTrans dealProcess start : {dataStr}");

                if (_m_info == null)
                {
                    _dealComplete?.Invoke();
                    return;
                }
                
                TileMatchBlockInfo triggerBoxInfo = _m_gameLogic._getBlockInfoFormArray(_m_info.getTriggerBlockIndex());
                // 彩虹转化, 触发格子应该要为彩虹格
                GGUIWndTileMatchCheckerSubRainbow rainbowSubWnd = triggerBoxInfo?.blockShow?.getCheckerSubWnd<GGUIWndTileMatchCheckerSubRainbow>();
                
                long gameLogicSerialize = _m_gameLogic.startSerialize;
                
                ALProcess alProcess = ALProcess.CreateProcess();
                alProcess
                    .addDelegateProcess((_complete) =>
                    {
                        if(gameLogicSerialize != _m_gameLogic.startSerialize)
                            return;
                        
                        if(triggerBoxInfo == null || triggerBoxInfo.blockShow == null)
                            _complete?.Invoke();
                        else
                        {
                            switch (logicType)
                            {
                                // 彩虹格与炸弹格交换消除
                                case ETileMatch_LogicType.BOX_RAINBOW:
                                    triggerBoxInfo.blockShow.proactiveUniteBoomTriggerShow(Vector2Int.zero, _complete);
                                    break;
                                
                                case ETileMatch_LogicType.ROCKET_RAINBOW:
                                    triggerBoxInfo.blockShow.proactiveUniteRocketTriggerShow(Vector2Int.zero, _complete);
                                    break;
                                
                                // 暂时只有上面的类型会使用彩虹转化展示逻辑, 其他的就直接完成表现
                                default:
                                    _complete?.Invoke();
                                    break;
                            }
                        }
                    })
                    .addDelegateProcess((_complete) =>
                    {
                        if(gameLogicSerialize != _m_gameLogic.startSerialize)
                            return;

                        if (_m_info == null || rainbowSubWnd == null)
                        {
                            _complete?.Invoke();
                            return;
                        }
                        
                        _dealList(_m_info.getBeTransIndexList(), (_beTransIndex, _itemDealDone) =>
                        {
                            switch (logicType)
                            {
                                // 展示联合炸弹格子消除时的飞行特效
                                case ETileMatch_LogicType.BOX_RAINBOW:
                                    rainbowSubWnd.showUniteBoomFlySfx(TileMatchUtil.indexToVector2(_beTransIndex), _itemDealDone);
                                    break;
                            
                                // 展示联合火箭格子消除时的飞行特效
                                case ETileMatch_LogicType.ROCKET_RAINBOW:
                                    rainbowSubWnd.showUniteRocketFlySfx(TileMatchUtil.indexToVector2(_beTransIndex), _itemDealDone);
                                    break;
                            
                                // 暂时只有上面的类型会使用彩虹转化展示逻辑, 其他的就直接完成表现
                                default:
                                    _itemDealDone?.Invoke();
                                    break;
                            }
                        }, () =>
                        {
                            _complete?.Invoke();
                        });
                    })
                    .addDelegateProcess((_complete) =>
                    {
                        if(gameLogicSerialize != _m_gameLogic.startSerialize)
                            return;

                        if (_m_info == null)
                        {
                            _complete?.Invoke();
                            return;
                        }

                        long nowBlockRefId = _m_info.getNewBlockId();
                        _dealList(_m_info.getBeTransIndexList(), (_beTransIndex, _itemDealDone) =>
                        {
                            // 获取要转化的格子信息
                            TileMatchBlockInfo transBlockInfo = _m_gameLogic._getBlockInfoFormArray(_beTransIndex);
                            _m_gameLogic._clearBlock(transBlockInfo, true, belongTaskSerialId, null);// 清除原来格子
                            _m_gameLogic._fillBlock(transBlockInfo, nowBlockRefId);//设置新格子数据和表现
                            if (transBlockInfo == null || transBlockInfo.blockShow == null)
                            {
                                _itemDealDone?.Invoke();
                            }
                            else
                            {
                                // 进行新格子生成表现
                                switch (logicType)
                                {
                                    case ETileMatch_LogicType.BOX_RAINBOW:
                                        GGUIWndTileMatchCheckerSubBoom boomSubWnd = transBlockInfo.blockShow.getCheckerSubWnd<GGUIWndTileMatchCheckerSubBoom>();
                                        if (boomSubWnd == null)
                                        {
                                            _itemDealDone?.Invoke();
                                        }
                                        else
                                        {
                                            boomSubWnd.playCreateByRainbowAnimation(_itemDealDone);
                                        }
                                        break;
                            
                                    case ETileMatch_LogicType.ROCKET_RAINBOW:
                                        GGUIWndTileMatchCheckerSubRocket rocketSubWnd = transBlockInfo.blockShow.getCheckerSubWnd<GGUIWndTileMatchCheckerSubRocket>();
                                        if (rocketSubWnd == null)
                                        {
                                            _itemDealDone?.Invoke();
                                        }
                                        else
                                        {
                                            rocketSubWnd.playCreateByRainbowAnimation(_itemDealDone);
                                        }
                                        break;
                            
                                    // 暂时只有上面的类型会使用彩虹转化展示逻辑, 其他的就直接完成表现
                                    default:
                                        _itemDealDone?.Invoke();
                                        break;
                                }
                            }
                        }, () =>
                        {
                            _complete?.Invoke();
                        });
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