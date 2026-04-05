using System;
using System.Collections.Generic;
using ALBasicProtocolPack;
using ALPackage;
using Hotfix.Common.TileMatchObj;
using Hotfix.TileMatchEnum;
using JetBrains.Annotations;

namespace Hotfix
{
    public partial class TileMatchGameLogic
    {
        public abstract class _ATileMatchProcessLogicAgent
        {
            [NotNull] protected TileMatchGameLogic _m_gameLogic;

            [NotNull] protected TileMatch_LogicInfo _m_serverMatchLogicInfo;

            public _ATileMatchProcessLogicAgent([NotNull] TileMatchGameLogic _gameLogic, [NotNull] Common.TileMatchObj.TileMatch_LogicInfo _serverMatchLogicInfo)
            {
                _m_gameLogic = _gameLogic;

                _m_serverMatchLogicInfo = _serverMatchLogicInfo;
                _parseLogicData(_serverMatchLogicInfo.getData());
            }

            /// <summary>
            /// 逻辑处理代理的序列号(同一序列号一起开始表现, 表现完后进行下一序列号表现)
            /// </summary>
            public int serialId { get { return _m_serverMatchLogicInfo.getSerialId(); } }
            
            /// <summary>
            /// 逻辑处理类型
            /// </summary>
            public TileMatchEnum.ETileMatch_LogicType logicType { get { return _m_serverMatchLogicInfo.getLogicType(); } }

            /// <summary>
            /// 增加的分数
            /// </summary>
            public int addScore { get { return _m_serverMatchLogicInfo.getScore(); } }

            /// <summary>
            /// 属于哪个任务的序列号, 用于任务格子处理
            /// </summary>
            public int belongTaskSerialId { get; set; }
            
            public abstract string dataStr { get; }

            protected T _parseLogicData<T>(byte[] _logicDataByte) where T : ALBasicProtocolPack._IALProtocolStructure, new()
            {
                T info = new T();
                if(_logicDataByte != null && _logicDataByte.Length > 0)
                {
                    ALProtocolBuf protocolBuf = new ALProtocolBuf(_logicDataByte);
                    info.readPackage(protocolBuf);
                }

                return info;
            }

            
            /// <summary>
            /// 解析逻辑数据
            /// </summary>
            /// <param name="_logicDataByte"></param>
            protected abstract void _parseLogicData(byte[] _logicDataByte);

            /// <summary>
            /// 获取清除的格子数量
            /// </summary>
            /// <returns></returns>
            public abstract int getClearBlockNum();
            
            /// <summary>
            /// 处理过程
            /// </summary>
            /// <param name="_dealComplete"></param>
            public void dealProcess(Action _dealComplete)
            {
                _dealProcess(() =>
                {
                    _dealComplete?.Invoke();
                    ALMsgSys.SendMsg(HotfixMsgType.ON_A_TILEMATCH_LOGIC_PROCESS_DEAL_DONE, this);
                });
            }

            protected abstract void _dealProcess(Action _dealComplete);
            
            public static _ATileMatchProcessLogicAgent matchProcessLogicAgent([NotNull] TileMatchGameLogic _gameLogic, Common.TileMatchObj.TileMatch_LogicInfo _serverMatchLogicInfo)
            {
                if (_serverMatchLogicInfo == null)
                    return null;
                
                switch (_serverMatchLogicInfo.getLogicType())
                {
                    case ETileMatch_LogicType.NORMAL:
                        return new TileMatchProcessLogicAgent_Composite(_gameLogic, _serverMatchLogicInfo);
                    
                    case ETileMatch_LogicType.BOX:
                    case ETileMatch_LogicType.ROCKET:
                    case ETileMatch_LogicType.RAINBOW:
                        return new TileMatchProcessLogicAgent_Remove(_gameLogic, _serverMatchLogicInfo);
                    
                    case ETileMatch_LogicType.BOX_BOX:
                    case ETileMatch_LogicType.BOX_ROCKET:
                    case ETileMatch_LogicType.ROCKET_ROCKET:
                    case ETileMatch_LogicType.RAINBOW_RAINBOW:
                        return new TileMatchProcessLogicAgent_CombineRemove(_gameLogic, _serverMatchLogicInfo);
                    
                    case ETileMatch_LogicType.BOX_RAINBOW:
                    case ETileMatch_LogicType.ROCKET_RAINBOW:
                        return new TileMatchProcessLogicAgent_RainbowTrans(_gameLogic, _serverMatchLogicInfo);
                 
                    case ETileMatch_LogicType.DROP:
                        return new TileMatchProcessLogicAgent_Drop(_gameLogic, _serverMatchLogicInfo);
                    
                    default:
                        return null;
                }
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="_list"></param>
            /// <param name="_dealAction">处理每个列表中元素的方法, 参数1给出列表元素, 参数2给出处理完成一个列表元素时需要调用的方法</param>
            /// <param name="_dealDone">所有列表元素处理完时调用的方法</param>
            /// <typeparam name="T"></typeparam>
            protected void _dealList<T>(List<T> _list, Action<T, Action> _dealAction, Action _dealDone)
            {
                if (_list == null || _list.Count <= 0 || _dealAction == null)
                {
                    _dealDone?.Invoke();
                    return;
                }
                
                ALStepCounter stepCounter = new ALStepCounter();
                stepCounter.chgTotalStepCount(_list.Count);
                stepCounter.regAllDoneDelegate(_dealDone);

                int listCount = _list.Count;
                for (int i = 0; i < listCount; i++)
                {
                    T item = _list[i];
                    _dealAction(item, stepCounter.addDoneStepCount);
                }
            }
            
            public override string ToString()
            {
                return $"[{GetType()}] serialId:{serialId} logicType:{logicType} data:{dataStr}";
            }
        }
    }
}