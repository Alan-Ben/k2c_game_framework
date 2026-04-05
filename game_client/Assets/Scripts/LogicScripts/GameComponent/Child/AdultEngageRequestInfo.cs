using System;
using Common.ChildObj;
using GS2GC.p014_ChildOp;
using JetBrains.Annotations;

namespace GOE
{
    public class AdultEngageRequestInfo
    {
        // 发起请求的子嗣 id
        private readonly long _m_adultId;
        // 请求的过期时间
        private readonly int _m_expiredTs;

        private AdultInfo _m_adultInfo;
        private bool _m_isStartInit;
        private Action<AdultInfo> _m_initCompleteDelegate;
        
        
        public AdultEngageRequestInfo([NotNull] Adult_ToMeApplyBaseInfo _serverInfo)
        {
            _m_adultId = _serverInfo.getApplyAdultId();
            _m_expiredTs = _serverInfo.getExpiredTs();
        }
        

        public long adultId { get { return _m_adultId; } }
        public bool isEnable { get { return FpsAndPingMgr.instance.serverTimeTagS < _m_expiredTs; } }
        public int expiredTimeS { get { return _m_expiredTs; } }


        public void getAdultInfo(Action<AdultInfo> _complete)
        {
            if (_complete == null)
                return;

            if (_m_adultInfo != null)
            {
                _complete.Invoke(_m_adultInfo);
                return;
            }
            
            _m_initCompleteDelegate += _complete;
            if (_m_isStartInit)
                return;
            
            _m_isStartInit = true;
            NPGSClientListener.sendRequestByLog(GSWriter_014_ChildOp.make_008_ReqGetToMeApply(_m_adultId), 
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_014_008_RetGetToMeApply>((_isSuc, _msg) =>
                {
                    if (_isSuc)
                    {
                        Adult_ToMeApplyInfo serverInfo = _msg.getApply();
                        _m_adultInfo = new AdultInfo(serverInfo.getApplyAdult(), serverInfo.getApplyCid());
                    }
                    
                    Action<AdultInfo> complete = _m_initCompleteDelegate;
                    _m_initCompleteDelegate = null;
                    _m_isStartInit = false;
                    
                    complete?.Invoke(_m_adultInfo);
                }));   
        }
    }
}