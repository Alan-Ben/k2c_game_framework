using System;
using ALPackage;
using Common.MarsObj;
using GC2GS.p004_PlayerOp;
using GC2GS.p041_MarsExploreOp;
using GS2GC.p004_PlayerOp;
using GS2GC.p041_MarsExploreOp;
using JetBrains.Annotations;
using NPCommon;
using UnityEngine;

namespace GOE
{
    public class MarsExploreMineInfo : _IMarsExplorePosItem, _IMarsExploreMineItem
    {
        private readonly long _m_instanceId;
        private readonly long _m_posId;
        private readonly long _m_endTimeMs;
        [NotNull] private readonly MarsExploreMineRefObj _m_refObj;
        private readonly long _m_startTimeMs;

        //动态序列号，确保确认消息只发一次
        private int _m_dynamicDataSerialize;

        private int _m_sentDynamicDataSerialize;
        private long _m_lServerMineSerialize;
        private long _m_occupiedCid;
        private long _m_occupiedTeamId;
        private long _m_occupiedMs;
        private long _m_collectSpeed;
        private long _m_occupiedResourceNum;

        public MarsExploreMineInfo([NotNull] Mars_MineIdx _serverData)
        {
            _m_instanceId = _serverData.getId();
            _m_posId = _serverData.getPos();
            _m_endTimeMs = _serverData.getEndShowMs();
            _m_refObj = GRefdataCoreMgr.instance.marsExploreMineRefCore.getRef(_serverData.getRefId());
            _m_startTimeMs = _serverData.getStartShowMs();
            if (_m_refObj == null)
            {
                ALLog.Error($"[MarsExploreMineInfo::MarsExploreMineInfo] 未找到火星探索矿点配置, refId={_serverData.getRefId()}");
                return;
            }

            NPGSClientListener.sendRequestByLog(new GC2GS_041_011_ReqMarsMineInfo(_m_instanceId), 
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_041_011_RetMarsMineInfo>((_isSuc, _msg) =>
                {
                    if (!_isSuc)
                        return;

                    Mars_MineDynamic mineDynamic = _msg?.getInfo();
                    _updateData(mineDynamic);
                }, _errorCode =>
                {
                    if (_errorCode == ErrorCodeConst.MARS_MINE_NOT_FOUND)
                        return;
                    
                    NPGUIAddSceneCenterTip.instance.showErrorInfo(_errorCode);
                }, false));
        }


        public long instanceId { get { return _m_instanceId; } }
        public long posId { get { return _m_posId; } }
        [NotNull] public MarsExploreMineRefObj refObj { get { return _m_refObj; } }
        public bool isMe { get { return _m_occupiedCid == NPPlayer.instance.playerInfo.CID; } }
        public long serverMineSerialize { get { return _m_lServerMineSerialize; } }
        public long occupiedCid { get { return _m_occupiedCid; } }
        public long occupiedTeamId { get { return _m_occupiedTeamId; } }
        public long remainNum
        {
            get
            {
                long result = _m_occupiedResourceNum;
                if (_m_occupiedCid > 0)
                    result = Mathf.FloorToInt(_m_occupiedResourceNum - (FpsAndPingMgr.instance.serverTimeTag - _m_occupiedMs) * _m_collectSpeed / 1000f);

                return Math.Max(0, result);
            }
        }
        public long startTime { get { return _m_startTimeMs; } }
        public long endTime { get { return _m_endTimeMs; } }


        public void reqPlayerInfo(Action<PlayerInfo_IconShow> _complete)
        {
            if (_m_occupiedCid == 0)
            {
                _complete?.Invoke(null);
                return;
            }

            if (isMe)
            {
                PlayerInfo_IconShow selfInfo = NPPlayer.instance.playerInfo.getPlayerBriefInfo();
                _complete?.Invoke(selfInfo);
                return;
            }
            
            NPGSClientListener.sendRequestByLog(NPGSWriter_004_PlayerOp.make_011_ReqSomeOnePlayerBriefInfo(_m_occupiedCid), 
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_004_011_RetSomeOnePlayerBriefInfo>((_isSuc, _msg) =>
                {
                    if (!_isSuc)
                    {
                        _complete?.Invoke(null);
                        return;
                    }

                    PlayerInfo_IconShow briefInfo = _msg?.getPlayerBrief();
                    _complete?.Invoke(briefInfo);
                }));
        }
        public long calculateRemainCollectTimeSec()
        {
            if (_m_collectSpeed <= 0)
                return 0;

            return Mathf.FloorToInt(remainNum * 1000f / _m_collectSpeed);
        }


        internal bool _isValid()
        {
            return _m_refObj != null;
        }
        public void updateTime()
        {
            if (_m_sentDynamicDataSerialize == _m_dynamicDataSerialize)
                return;
            
            long timeNow = FpsAndPingMgr.instance.serverTimeTag;
            bool isOver = timeNow >= _m_endTimeMs;

            //未发送才会发送
            if (remainNum < 0 || (!isMe && isOver))
            {
                _m_sentDynamicDataSerialize = _m_dynamicDataSerialize;
                NPGSClientListener.sendRequestByLog(GSWriter_041_MarsExploreOp.make_013_ReqNoticeMarsMine(_m_instanceId, _m_lServerMineSerialize), 
                    new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_041_013_RetNoticeMarsMine>((_isSuc, _msg) =>
                    {
                        
                    }, _errorCode =>
                    {
                        if (_errorCode == ErrorCodeConst.MARS_MINE_NOT_FOUND)
                            return;

                        NPGUIAddSceneCenterTip.instance.showErrorInfo(_errorCode);
                    }, false, true));
            }
        }
        /// <summary>
        /// 尝试检查信息，通过41-13发送矿消息，附带状态序列号可以检查
        /// </summary>
        public void checkInfo()
        {
            NPGSClientListener.sendRequestByLog(GSWriter_041_MarsExploreOp.make_013_ReqNoticeMarsMine(_m_instanceId, _m_lServerMineSerialize), 
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_041_013_RetNoticeMarsMine>((_isSuc, _msg) =>
                {
                        
                }, _errorCode =>
                {
                    if (_errorCode == ErrorCodeConst.MARS_MINE_NOT_FOUND)
                        return;

                    NPGUIAddSceneCenterTip.instance.showErrorInfo(_errorCode);
                }, false, true));
        }

        internal void _updateData(Mars_MineDynamic _serverData)
        {
            _m_dynamicDataSerialize = ALSerializeOpMgr.next();

            _m_lServerMineSerialize = _serverData.getSerialize();
            _m_occupiedCid = _serverData.getOccupiedCid();
            _m_occupiedTeamId = _serverData.getOccupiedTeamId();
            _m_occupiedMs = _serverData.getOccupiedMs();
            _m_collectSpeed = _serverData.getCollectSpeed();
            _m_occupiedResourceNum = _serverData.getRemainNum();
        }
    }
}
