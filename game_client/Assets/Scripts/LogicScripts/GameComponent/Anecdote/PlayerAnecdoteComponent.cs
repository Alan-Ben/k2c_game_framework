using ALPackage;
using System;
using System.Collections.Generic;
using Common.AnecdoteObj;
using GS2GC.p002_InitOp;
using GS2GC.p021_PlayerInfo;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 经营事件组件
    /// </summary>
    public class PlayerAnecdoteComponent : _ANPBasicPlayerComponent
    {
        // 所有事件的总字典
        [NotNull] private readonly Dictionary<long, _AAnecdoteEventInfo> _m_allEventInfoDic;
        // 显示事件的所有位置列表
        [ItemNotNull, NotNull] private readonly List<AnecdotePosInfo> _m_posList;
        // 是否在事件处理中
        private bool _m_inEventProcess;


        public PlayerAnecdoteComponent(NPPlayerComponentMgr _compMgr) 
            : base(_compMgr)
        {
            _m_allEventInfoDic = new Dictionary<long, _AAnecdoteEventInfo>();
            _m_posList = new List<AnecdotePosInfo>();
        }


        public event Action<AnecdotePosInfo> onPosInfoAdd;
        public event Action<AnecdotePosInfo> onPosInfoRemove;
        

        public override bool isMustInit { get { return true; } }
        public override ENPPlayerCompType compType { get { return ENPPlayerCompType.ANECDOTE; } }
        public override ENPPlayerCompType[] dependCompList { get { return null; } }
        /// <summary>
        /// 是否允许提前初始化。提前初始化的意思是在依赖项没有完成初始化之前就进行初始化操作（一般是提前发送消息）
        /// 在初始化结果消息返回的时候，通过特殊的初始化函数dealPreInitFunc进行处理函数注册，再依赖项完成之后才进行初始化处理
        /// </summary>
        public override bool canPreInit { get { return true; } }
        public bool inEventProcess { get { return _m_inEventProcess; } }


        /// <summary>
        /// 发送初始化协议提前申请内容
        /// </summary>
        public override void presendInitProtocol()
        {
            NPGSClientListener.sendRequestByLog(GSWriter_002_InitOp.make_009_ReqAnecdoteInit(),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_002_009_RetAnecdoteInit>((_isSuc, _msg) =>
                {
                    if (!_isSuc)
                    {
                        setInitFail();
                        return;
                    }

                    _initData(_msg);
                }));
        }
        protected override void _dealInit()
        {
        }
        //组件加载完成时的调用
        protected override void _onInitDone()
        {
        }
        protected override void _onInitFail()
        {
            ALLog.Error("PlayerAnecdoteComponent init Fail!!!");
        }
        protected override void _discard()
        {
            _m_allEventInfoDic.Clear();
            _m_posList.Clear();
        }
        
        
        /// <summary>
        /// 根据位置 id 获得对应的位置数据
        /// </summary>
        public AnecdotePosInfo getPosInfo(long _posId)
        {
            foreach (AnecdotePosInfo posInfo in _m_posList)
            {
                if (posInfo.posId == _posId)
                    return posInfo;
            }

            return null;
        }
        public AnecdotePosInfo getPosInfo(EAnecdoteType _type)
        {
            switch (_type)
            {
                case EAnecdoteType.HERO_GAIN:
                    foreach (AnecdotePosInfo posInfo in _m_posList)
                    {
                        _AAnecdoteEventInfo eventInfo = posInfo.getNextEnableEvent();
                        if (eventInfo != null && eventInfo.hasHeroReward())
                            return posInfo;
                    }
                    
                    NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.anecdote_cantFindHeroRewardEvent_none);
                    break;
            }

            return null;
        }
        public int getAnecdoteEventCount(long _eventId)
        {
            int result = 0;
            foreach (_AAnecdoteEventInfo eventInfo in _m_allEventInfoDic.Values)
            {
                if (eventInfo != null && eventInfo.eventRef.id == _eventId)
                    result++;
            }

            return result;
        }
        /// <summary>
        /// 获得所有位置数据
        /// </summary>
        public List<AnecdotePosInfo> getPosList()
        {
            return new List<AnecdotePosInfo>(_m_posList);
        }
        /// <summary>
        /// 获得所有位置数据
        /// </summary>
        public void getPosListNonAlloc(List<AnecdotePosInfo> _list)
        {
            if (_list == null)
                return;
            
            _list.Clear();
            _list.AddRange(_m_posList);
        }
        public void setStartEventProcess()
        {
            _m_inEventProcess = true;    
        }
        public void setEndEventProcess()
        {
            _m_inEventProcess = false;
            WinMsg.SendMsg(WinMsgType.CONTROL_TUROTIAL_SETP, ENPTutorialTriggerType.ANECDOTE_EVENT_PROCESS_END);
        }
        


        private void _initData(GS2GC_002_009_RetAnecdoteInit _msg)
        {
            if (_msg == null)
            {
                setInitFail();
                return;
            }

            List<Anecdote_EventInfo> serverList = _msg.getEventList();
            if (serverList != null)
            {
                foreach (Anecdote_EventInfo serverInfo in serverList)
                {
                    _addEvent(serverInfo);
                }
            }
            
            setInitDone();
        }
        private void _addEvent(Anecdote_EventInfo _serverInfo)
        {
            if (_serverInfo == null)
                return;

            if (_m_allEventInfoDic.ContainsKey(_serverInfo.getInstanceId()))
            {
                ALLog.Error("[PlayerAnecdoteComponent] Error: add event instanceId is exist!!!");
                return;
            }
            
            _AAnecdoteEventInfo eventInfo = _AAnecdoteEventInfo.createInstance(_serverInfo);
            if (eventInfo == null)
            {
                ALLog.Error("[PlayerAnecdoteComponent] Error: add event createInstance fail!!!");
                return;
            }
            
            _m_allEventInfoDic.Add(_serverInfo.getInstanceId(), eventInfo);
            AnecdotePosInfo posInfo = getPosInfo(eventInfo.posId);
            if (posInfo == null)
            {
                posInfo = new AnecdotePosInfo(eventInfo.posId);
                posInfo._addEvent(eventInfo);
                _m_posList.Add(posInfo);
                onPosInfoAdd?.Invoke(posInfo);
            }
            else
                posInfo._addEvent(eventInfo);
        }
        private void _removeEvent(long _instanceId)
        {
            if (!_m_allEventInfoDic.TryGetValue(_instanceId, out _AAnecdoteEventInfo eventInfo))
                return;

            _m_allEventInfoDic.Remove(_instanceId);
            if (eventInfo == null)
                return;
            
            AnecdotePosInfo posInfo = getPosInfo(eventInfo.posId);
            if (posInfo == null)
                return;
            
            posInfo._removeEvent(eventInfo);
            if (posInfo.isEmpty)
            {
                _m_posList.Remove(posInfo);
                onPosInfoRemove?.Invoke(posInfo);
            }
        }

        internal void _onAnecdoteChg(GS2GC_021_070_OnAnecdoteChg _msg)
        {
            byte[] serverInfo = _msg?.getExtraData();
            if (serverInfo == null)
                return;

            if (!_m_allEventInfoDic.TryGetValue(_msg.getInstanceId(), out _AAnecdoteEventInfo eventInfo))
                return;

            eventInfo._updateData(serverInfo);
        }
        internal void _onAnecdoteEventAdd(GS2GC_021_071_OnAnecdoteEventAdd _msg)
        {
            List<Anecdote_EventInfo> serverList = _msg?.getEventList();
            if (serverList == null)
                return;
            
            foreach (Anecdote_EventInfo eventInfo in serverList)
            {
                _addEvent(eventInfo);    
            }
        }
        internal void _onAnecdoteEventRemove(GS2GC_021_072_OnAnecdoteEventRemove _msg)
        {
            if (_msg == null)
                return;
            
            _removeEvent(_msg.getInstanceId());
        }
    }
}
