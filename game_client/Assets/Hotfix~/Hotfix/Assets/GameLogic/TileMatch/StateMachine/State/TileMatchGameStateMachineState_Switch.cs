using System;
using System.Collections.Generic;
using ALPackage;
using GOE;
using Hotfix.Common.TileMatchObj;
using Hotfix.TileMatchEnum;
using JetBrains.Annotations;

namespace Hotfix
{
    public partial class TileMatchGameLogic
    {
        public class TileMatchGameStateMachineState_Switch : _ATileMatchGameStateMachineBaseState
        {
            private TileMatchBlockInfo _m_iStartBlockInfo;//起始方块信息
            private TileMatchBlockInfo _m_iEndBlockInfo;//终点方块信息
            
            private Dictionary<int, List<_ATileMatchProcessLogicAgent>> _m_dProcessLogicAgentsDic;//逻辑处理代理列表
            private int _m_iStartProcessLogicSerialize = 0;
            private int _m_iEndProcessLogicSerialize = 0;
            
            public TileMatchGameStateMachineState_Switch([NotNull] TileMatchGameLogic _gameLogic) : base(_gameLogic)
            {
            }

            public override ETileMatchGameState state { get { return ETileMatchGameState.Switch; } }

            protected override void _onEnter()
            {
                ALMsgSys.RegisterMsg(HotfixMsgType.GET_TILEMATCH_LOGIC_PROCESS, _onGetTileMatchLogicProcess);
                
                if (_m_iStartBlockInfo == null || _m_iEndBlockInfo == null)
                {
                    _m_gameLogic._m_stateMachine.setState<TileMatchGameStateMachineState_Idle>();
                    return;
                }

                // 体力不足
                if (_m_gameLogic.nowGameModeRefObj == null || !GCommon.lazycdEnough(HotfixRefdataCoreMgr.instance.tileMatchOtherRefObj.tilematch_lazy_cd_id, _m_gameLogic.nowGameModeRefObj.multiple, true))
                {
                    _m_gameLogic._m_stateMachine.setState<TileMatchGameStateMachineState_Idle>();
                    return;
                }
                
                long logicSerialize = _m_gameLogic.startSerialize;
                _m_gameLogic._playExchangeBlockAudio();//播放交换方块音效
                _m_gameLogic._exchangeBlock(_m_iStartBlockInfo, _m_iEndBlockInfo, () =>
                {
                    if(!_m_gameLogic.isRunning || logicSerialize != _m_gameLogic.startSerialize)
                        return;
                    
                    // 若其中有一个方块是特殊方块, 则一定是可以交换的
                    if(_m_gameLogic._checkIsSpecialBlock(_m_iStartBlockInfo) || _m_gameLogic._checkIsSpecialBlock(_m_iEndBlockInfo))
                    {
                        // 请求交换格子
                        _reqSwitchBlock();
                    }
                    else//若两个都不是特殊方块, 则客户端需要判断是否可以交换
                    {
                        // 若交换后可消除
                        if (_m_gameLogic._checkEliminate(_m_iStartBlockInfo.logicPos.x, _m_iStartBlockInfo.logicPos.y) ||
                            _m_gameLogic._checkEliminate(_m_iEndBlockInfo.logicPos.x, _m_iEndBlockInfo.logicPos.y))
                        {
                            // 请求交换格子
                            _reqSwitchBlock();
                        }
                        else
                        {
                            // 交换后不可消除
                            _m_gameLogic._m_errorSwitchCount++;//错误交换次数增加
                            CommonTaskController.CommonActionAddMonoTask(() =>
                            {
                                if(!_m_gameLogic.isRunning || logicSerialize != _m_gameLogic.startSerialize)
                                    return;

                                long stateEnterSerialize = enterSerialize;
                                //重新换回两个格子
                                _m_gameLogic._exchangeBlock(_m_iStartBlockInfo, _m_iEndBlockInfo, () =>
                                {
                                    if(stateEnterSerialize != enterSerialize)
                                        return;
                                    
                                    _m_gameLogic._m_stateMachine.setState<TileMatchGameStateMachineState_Idle>();
                                });
                                
                            }, _m_gameLogic._m_gameConfig.errorSwitchResetDelayTime);
                        }
                    }
                });
            }

            protected override void _onExit()
            {
                ALMsgSys.UnregisterMsg(HotfixMsgType.GET_TILEMATCH_LOGIC_PROCESS, _onGetTileMatchLogicProcess);

                _m_iStartBlockInfo = null;
                _m_iEndBlockInfo = null;
                
                // _m_dProcessLogicAgentsDic?.Clear();//这里清除, 因为会传到 DealServer状态中, 需要在DealServer状态中处理完后再清除
                _m_dProcessLogicAgentsDic = null;
            }

            protected override void _onTick(float _deltaTime)
            {
            }

            public override bool canEnterState(_ATHotfixStateBase<ETileMatchGameState> _newState)
            {
                return false;
            }

            public override void resetData()
            {
            }
            
            public override void doPress(TileMatchBlockInfo _cubeInfo)
            {
            }

            
            public void init(TileMatchBlockInfo _startBlockInfo, TileMatchBlockInfo _endBlockInfo)
            {
                _m_iStartBlockInfo = _startBlockInfo;
                _m_iEndBlockInfo = _endBlockInfo;
            }

            /// <summary>
            /// 请求交换方块
            /// </summary>
            private void _reqSwitchBlock()
            {
                if(_m_iStartBlockInfo == null || _m_iEndBlockInfo == null)
                    return;
                
                _m_dProcessLogicAgentsDic?.Clear();
                _m_iStartProcessLogicSerialize = int.MaxValue;
                _m_iEndProcessLogicSerialize = int.MinValue;

                long serialize = enterSerialize;
                int opMaskSerialize = _m_gameLogic._m_gameShow.openOpMask();//请求协议时, 打开操作屏蔽遮罩
                
                int belongTaskSerialId = _m_gameLogic.taskInfo?.serialId ?? 0;//所属的任务序列号
                HotfixNPPlayer.instance.tileMatchComponent.reqTileMatchSwitchItem(HotfixAccountSettingMgr.instance.hotfixAccountSetting.getTileMatchModelType(), 
                    _m_iStartBlockInfo.logicPosIndex, _m_iEndBlockInfo.logicPosIndex, (_isSucc, _msg) =>
                    {
                        // 请求完成后关闭操作屏蔽遮罩
                        _m_gameLogic._m_gameShow.closeOpMask(opMaskSerialize);
                        if (!_m_gameLogic.isRunning || serialize != enterSerialize)
                            return;
                        
                        if (_isSucc)
                        {
                            // 若请求交换成功, 进入处理协议状态
                            _m_gameLogic._m_stateMachine.setState((TileMatchGameStateMachineState_DealServer _state) =>
                            {
                                _state?.init(_m_dProcessLogicAgentsDic, _m_iStartProcessLogicSerialize, _m_iEndProcessLogicSerialize, belongTaskSerialId);
                            });
                        }
                        else
                        {
                            // 请求交换失败, 重新换回两个格子
                            _m_gameLogic._exchangeBlock(_m_iStartBlockInfo, _m_iEndBlockInfo, () =>
                            {
                                if (!_m_gameLogic.isRunning || serialize != enterSerialize)
                                    return;
                
                                // 返回idle状态
                                _m_gameLogic._m_stateMachine.setState<TileMatchGameStateMachineState_Idle>();
                            });
                        }
                    });

                // List<Common.TileMatchObj.TileMatch_LogicInfo> logicInfoList = new List<TileMatch_LogicInfo>();
                // Common.TileMatchObj.TileMatch_LogicInfo logicInfo = null;
                //
                // TileMatch_Composite compositeInfo = new TileMatch_Composite();
                // compositeInfo.addRemoveBlockIndexList(27);
                // compositeInfo.addRemoveBlockIndexList(28);
                // compositeInfo.addRemoveBlockIndexList(29);
                // logicInfo = new TileMatch_LogicInfo();
                // logicInfo.setSerialId(0);
                // logicInfo.setLogicType(ETileMatch_LogicType.NORMAL);
                // logicInfo.setData(compositeInfo.makePackage());
                // logicInfoList.Add(logicInfo);
                //
                // TileMatch_Drop dropInfo = new TileMatch_Drop();
                // dropInfo.addPosChgList(new TileMatch_BlockPosChg(33, 27));
                // dropInfo.addPosChgList(new TileMatch_BlockPosChg(39, 33));
                // dropInfo.addPosChgList(new TileMatch_BlockPosChg(34, 28));
                // dropInfo.addPosChgList(new TileMatch_BlockPosChg(40, 34));
                // dropInfo.addPosChgList(new TileMatch_BlockPosChg(35, 29));
                // dropInfo.addPosChgList(new TileMatch_BlockPosChg(41, 35));
                // dropInfo.addGenBlockList(new TileMatch_BlockInfo(39, new TileMatch_BlockBaseInfo(1, 0)));
                // dropInfo.addGenBlockList(new TileMatch_BlockInfo(40, new TileMatch_BlockBaseInfo(2, 0)));
                // dropInfo.addGenBlockList(new TileMatch_BlockInfo(41, new TileMatch_BlockBaseInfo(4, 0)));
                // logicInfo = new TileMatch_LogicInfo();
                // logicInfo.setSerialId(1);
                // logicInfo.setLogicType(ETileMatch_LogicType.DROP);
                // logicInfo.setData(dropInfo.makePackage());
                // logicInfoList.Add(logicInfo);
                //
                // _onGetTileMatchLogicProcess(logicInfoList);
                //
                // _m_gameLogic._m_stateMachine.setState((TileMatchGameStateMachineState_DealServer _state) =>
                // {
                //     _state?.init(_m_dProcessLogicAgentsDic, _m_iStartProcessLogicSerialize, _m_iEndProcessLogicSerialize);
                // });
            }

            private void _onGetTileMatchLogicProcess(params object[] _objs)
            {
                if(_objs == null || _objs.Length < 1 || !(_objs[0] is List<Common.TileMatchObj.TileMatch_LogicInfo> _serverLogicInfoList) || _serverLogicInfoList.Count <= 0)
                    return;
                
                // 记录开始和结束的序列号
                int startSerialize = _serverLogicInfoList.GetFirst()?.getSerialId() ?? int.MaxValue;
                _m_iStartProcessLogicSerialize = Math.Min(_m_iStartProcessLogicSerialize, startSerialize);
                
                int endSerialize = _serverLogicInfoList.GetLast()?.getSerialId() ?? int.MinValue;
                _m_iEndProcessLogicSerialize = Math.Max(_m_iEndProcessLogicSerialize, endSerialize);

                if (_m_dProcessLogicAgentsDic == null)
                    _m_dProcessLogicAgentsDic = new Dictionary<int, List<_ATileMatchProcessLogicAgent>>();
                
                int listCount = _serverLogicInfoList.Count;
                Common.TileMatchObj.TileMatch_LogicInfo serverMatchLogicInfo;
                _ATileMatchProcessLogicAgent matchProcessLogicAgent;
                for (int i = 0; i < listCount; i++)
                {
                    serverMatchLogicInfo = _serverLogicInfoList[i];
                    if(serverMatchLogicInfo == null || serverMatchLogicInfo.getData() == null || serverMatchLogicInfo.getData().Length < 0)
                        continue;
                    
                    matchProcessLogicAgent = _ATileMatchProcessLogicAgent.matchProcessLogicAgent(_m_gameLogic, serverMatchLogicInfo);
                    if(matchProcessLogicAgent == null)
                        continue;

                    if (!_m_dProcessLogicAgentsDic.TryGetValue(serverMatchLogicInfo.getSerialId(), out List<_ATileMatchProcessLogicAgent> _agentList) || _agentList == null)
                    {
                        _agentList = new List<_ATileMatchProcessLogicAgent>();
                        _m_dProcessLogicAgentsDic[serverMatchLogicInfo.getSerialId()] = _agentList;
                    }
                    
                    _agentList.Add(matchProcessLogicAgent);
                }
            }
        }
    }
}