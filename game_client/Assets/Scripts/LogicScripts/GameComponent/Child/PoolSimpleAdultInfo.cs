using System;
using Common.ChildObj;
using GS2GC.p004_PlayerOp;
using GS2GC.p014_ChildOp;
using NPCommon;

namespace GOE
{
    public class PoolSimpleAdultInfo
    {
        private readonly long _m_adultId;
        private readonly long _m_playerCid;
        private readonly long _m_bonus;
        private readonly long _m_minBonus;

        private AdultInfo _m_adultInfo;
        private bool _m_isStartAdultInfoInit;
        private Action<AdultInfo> _m_adultInfoInitCompleteDelegate;
        
        private PlayerInfo_IconShow _m_playerInfo;
        private bool _m_isStartPlayerInfoInit;
        private Action<PlayerInfo_IconShow> _m_playerInfoInitCompleteDelegate;
        
        
        public PoolSimpleAdultInfo(long _adultId, long _playerCid, long _bonus, long _minBonus)
        {
            _m_adultId = _adultId;
            _m_playerCid = _playerCid;
            _m_bonus = _bonus;
            _m_minBonus = _minBonus;
        }
        
        
        public long cid { get { return _m_playerCid; } }
        public long adultId { get { return _m_adultId; } }
        public long bonus { get { return _m_bonus; } }
        public long minBonus { get { return _m_minBonus; } }


        public void getAdultInfo(Action<AdultInfo> _complete)
        {
            if (_complete == null)
                return;

            if (_m_adultInfo != null)
            {
                _complete.Invoke(_m_adultInfo);
                return;
            }

            _m_adultInfoInitCompleteDelegate += _complete;
            if (_m_isStartAdultInfoInit)
                return;

            _m_isStartAdultInfoInit = true;
            NPGSClientListener.sendRequestByLog(GSWriter_014_ChildOp.make_018_ReqGetPoolAdult(_m_playerCid, _m_adultId),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_014_018_RetGetPoolAdult>((_isSuc, _msg) =>
                {
                    if (_isSuc)
                    {
                        Adult_PoolInfo serverInfo = _msg.getPoolAdult();
                        _m_adultInfo = new AdultInfo(serverInfo.getAdult(), serverInfo.getCid());
                    }
                    
                    Action<AdultInfo> complete = _m_adultInfoInitCompleteDelegate;
                    _m_adultInfoInitCompleteDelegate = null;
                    _m_isStartAdultInfoInit = false;
                    
                    complete?.Invoke(_m_adultInfo);
                }));
        }

        public void getPlayerInfo(Action<PlayerInfo_IconShow> _complete)
        {
            if (_complete == null)
                return;
            
            if (_m_playerInfo != null)
            {
                _complete.Invoke(_m_playerInfo);
                return;
            }
            
            _m_playerInfoInitCompleteDelegate += _complete;
            if (_m_isStartPlayerInfoInit)
                return;
            
            _m_isStartPlayerInfoInit = true;
            if (_m_playerCid == NPPlayer.instance.playerInfo.CID)
            {
                _m_playerInfo = new PlayerInfo_IconShow();
                _m_playerInfo.setCid(NPPlayer.instance.playerInfo.CID);
                _m_playerInfo.setPlayerName(NPPlayer.instance.playerInfo.PlayerName);
            }
            else
            {
                NPGSClientListener.sendRequestByLog(NPGSWriter_004_PlayerOp.make_011_ReqSomeOnePlayerBriefInfo(_m_playerCid),
                    new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_004_011_RetSomeOnePlayerBriefInfo>(
                        (_isSuc, _msg) =>
                        {
                            if (_isSuc)
                                _m_playerInfo = _msg.getPlayerBrief();

                            Action<PlayerInfo_IconShow> complete = _m_playerInfoInitCompleteDelegate;
                            _m_playerInfoInitCompleteDelegate = null;
                            _m_isStartPlayerInfoInit = false;
                            
                            complete?.Invoke(_m_playerInfo);
                        }));
            }
        }
    }
}