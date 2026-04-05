using System;
using System.Collections.Generic;
using System.Text;
using ALPackage;
using GOE;
using JetBrains.Annotations;

namespace Hotfix
{
    public partial class TileMatchGameLogic
    {
        public class TileMatchGameStateMachineState_DealServer : _ATileMatchGameStateMachineBaseState
        {
            private Dictionary<int, List<_ATileMatchProcessLogicAgent>> _m_dProcessLogicAgentsDic;//逻辑处理代理列表
            private int _m_iStartProcessLogicSerialize = 0;
            private int _m_iEndProcessLogicSerialize = 0;
            private int _m_iBelongTaskSerialId = 0;//属于哪个任务的序列号

            public TileMatchGameStateMachineState_DealServer([NotNull] TileMatchGameLogic _gameLogic) : base(_gameLogic)
            {
            }

            public override ETileMatchGameState state { get { return ETileMatchGameState.DealServer; } }

            public IReadOnlyDictionary<int, List<_ATileMatchProcessLogicAgent>> processLogicAgentsDic { get { return _m_dProcessLogicAgentsDic; } }
            public int belongTaskSerialId { get { return _m_iBelongTaskSerialId; } }
            
            protected override void _onEnter()
            {
                _m_gameLogic._onStartDealServer(this);
                _dealTileMatchProcessLogicAgent(_m_iStartProcessLogicSerialize, () =>
                {
                    _m_gameLogic._onDealServerDone();
                });
            }

            protected override void _onExit()
            {
                _m_dProcessLogicAgentsDic?.Clear();
                _m_dProcessLogicAgentsDic = null;

                _m_iStartProcessLogicSerialize = int.MaxValue;
                _m_iEndProcessLogicSerialize = int.MinValue;
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

            public void init(Dictionary<int, List<_ATileMatchProcessLogicAgent>> _matchLogicAgentsDic, int _startProcessLogicSerialize, int _endProcessLogicSerialize, int _belongTaskSerialId)
            {
#if UNITY_EDITOR
                if (_AALMonoMain.instance.showDebugOutput)
                {
                    StringBuilder sb = new StringBuilder();
                    if (_matchLogicAgentsDic != null && _matchLogicAgentsDic.Count > 0)
                    {
                        for (int serialize = _startProcessLogicSerialize; serialize <= _endProcessLogicSerialize; serialize++)
                        {
                            if (_matchLogicAgentsDic.TryGetValue(serialize, out List<_ATileMatchProcessLogicAgent> _agentList)
                                && _agentList != null && _agentList.Count > 0)
                            {
                                sb.Append($"\n\n ========== serialize:{serialize} agentCount:{_agentList.Count} agents:");

                                for (int j = 0; j < _agentList.Count; j++)
                                {
                                    _ATileMatchProcessLogicAgent agent = _agentList[j];
                                    if (agent != null)
                                    {
                                        sb.Append($"\n === {agent}");
                                    }
                                }
                            }

                            sb.Append($"\n");
                        }
                    }

                    Debug.Log(
                        $"======[TileMatchGameStateMachineState_DealServer init] _startProcessLogicSerialize:{_startProcessLogicSerialize}, _endProcessLogicSerialize:{_endProcessLogicSerialize} {sb.ToString()}");
                }
#endif
                
                _m_dProcessLogicAgentsDic = _matchLogicAgentsDic;
                _m_iStartProcessLogicSerialize = _startProcessLogicSerialize;
                _m_iEndProcessLogicSerialize = _endProcessLogicSerialize;
                _m_iBelongTaskSerialId = _belongTaskSerialId;

                if (_m_dProcessLogicAgentsDic != null)
                {
                    foreach (var agentList in _m_dProcessLogicAgentsDic.Values)
                    {
                        if(agentList == null)
                            continue;

                        foreach (var agent in agentList)
                        {
                            if(agent != null)
                                agent.belongTaskSerialId = _m_iBelongTaskSerialId;
                        }
                    }
                }
            }

            /// <summary>
            /// 处理服务器消息的具体逻辑
            /// 每处理一个逻辑会重新调用本函数，处理下一个逻辑
            /// 每个逻辑附带多个处理信息
            /// </summary>
            /// <param name="_serverLogicSerialize"></param>
            /// <param name="_complete"></param>
            private void _dealTileMatchProcessLogicAgent(int _serverLogicSerialize, Action _complete)
            {
#if UNITY_EDITOR
                if(_AALMonoMain.instance.showDebugOutput)
                    Debug.Log($"===[TileMatchGameStateMachineState_DealServer _dealTileMatchProcessLogicAgent] _serverLogicSerialize:{_serverLogicSerialize}");
#endif
                // 序列号超出处理范围, 直接完成
                if(_m_dProcessLogicAgentsDic == null || _m_dProcessLogicAgentsDic.Count <= 0 || _serverLogicSerialize < _m_iStartProcessLogicSerialize || _serverLogicSerialize > _m_iEndProcessLogicSerialize)
                {
                    _complete?.Invoke();
                    return;
                }

                // 若序列号_serialize没有需要处理的逻辑, 则直接处理下一个序列号
                if (!_m_dProcessLogicAgentsDic.TryGetValue(_serverLogicSerialize, out List<_ATileMatchProcessLogicAgent> _agentList) || _agentList == null || _agentList.Count <= 0)
                {
                    _dealTileMatchProcessLogicAgent(_serverLogicSerialize + 1, _complete);
                    return;
                }

                int agentCount = _agentList.Count;
                long serialize = enterSerialize;//记录状态机序列号
                
                ALStepCounter stepCounter = new ALStepCounter();
                stepCounter.chgTotalStepCount(agentCount);
                stepCounter.regAllDoneDelegate(() =>
                {
                    // 状态机序列号发生变化时, 不需要再继续处理了
                    if(serialize != enterSerialize)
                        return;
                    
                    ALMsgSys.SendMsg(HotfixMsgType.ON_TILEMATCH_SAME_SERIALIZE_LOGIC_PROCESS_DEAL_DONE, _agentList);
                    
                    // 否则继续处理下一个序列号
                    _dealTileMatchProcessLogicAgent(_serverLogicSerialize + 1, _complete);
                });

                ALMsgSys.SendMsg(HotfixMsgType.ON_TILEMATCH_SAME_SERIALIZE_LOGIC_PROCESS_START_DEAL, _agentList);
                
                _ATileMatchProcessLogicAgent agent = null;
                for (int i = 0; i < agentCount; i++)
                {
                    agent = _agentList[i];
                    if (agent == null)
                    {
                        stepCounter.addDoneStepCount();
                    }
                    else
                    {
                        agent.dealProcess(stepCounter.addDoneStepCount);
                    }
                }
            }
        }
    }
}