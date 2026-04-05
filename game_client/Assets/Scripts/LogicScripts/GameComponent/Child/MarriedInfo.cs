using System;
using Common.ChildObj;
using GS2GC.p014_ChildOp;

namespace GOE
{
    public class MarriedInfo
    {
        private readonly long _m_adultId;

        private AdultInfo _m_myAdultInfo;
        private AdultInfo _m_otherAdultInfo;
        private int _m_marriedTime;
        
        private bool _m_isInit;
        private bool _m_isStartInit;
        private Action _m_initCompleteDelegate;
        
        
        public MarriedInfo(long _adultId)
        {
            _m_adultId = _adultId;
        }
        public MarriedInfo(Adult_MarriedInfo _marriedInfo)
        {
            _m_adultId = _marriedInfo.getAdult().getId();
            _m_myAdultInfo = new AdultInfo(_marriedInfo.getAdult(), NPPlayer.instance.playerInfo.CID);
            _m_otherAdultInfo = new AdultInfo(_marriedInfo.getMarriedAdult(), _marriedInfo.getMarriedCid());
            _m_marriedTime = _marriedInfo.getMarriedTs();
            _m_isInit = true;
        }

        public long adultId { get { return _m_adultId; } }


        public void getMyAdultInfo(Action<AdultInfo> _complete)
        {
            if (_complete == null)
                return;

            _init(() => { _complete.Invoke(_m_myAdultInfo); });
        }
        public void getOtherAdultInfo(Action<AdultInfo> _complete)
        {
            if (_complete == null)
                return;

            _init(() => { _complete.Invoke(_m_otherAdultInfo); });
        }
        public void getMarriedTime(Action<int> _complete)
        {
            if (_complete == null)
                return;
            
            _init(() => { _complete.Invoke(_m_marriedTime); });
        }
        
        
        private void _init(Action _complete)
        {
            if (_m_isInit)
            {
                _complete?.Invoke();
                return;
            }

            _m_initCompleteDelegate += _complete;
            if (_m_isStartInit)
                return;

            _m_isStartInit = true;
            NPGSClientListener.sendRequestByLog(GSWriter_014_ChildOp.make_007_ReqGetMarriedAdult(_m_adultId), 
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_014_007_RetGetMarriedAdult>((_isSuc, _msg) =>
                {
                    if (_isSuc)
                    {
                        Adult_MarriedInfo serverInfo = _msg.getAdult();
                        _m_myAdultInfo = new AdultInfo(serverInfo.getAdult(), NPPlayer.instance.playerInfo.CID);
                        _m_otherAdultInfo = new AdultInfo(serverInfo.getMarriedAdult(), serverInfo.getMarriedCid());
                        _m_marriedTime = serverInfo.getMarriedTs();
                    }

                    _m_isInit = true;
                    Action complete = _m_initCompleteDelegate;
                    _m_initCompleteDelegate = null;
                    _m_isStartInit = false;
                    
                    complete?.Invoke();
                }));
        }
    }
}