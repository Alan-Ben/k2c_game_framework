using System;
using System.Collections.Generic;
using ALPackage;
using Common.DinnerEnum;
using Common.DinnerObj;
using GS2GC.p019_DinnerOp;
using Unity.Mathematics;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 宴会数据类
    /// </summary>
    public class GDinnerInfo
    {
        private long _m_instanceId;// 宴会实例id
        private long _m_dinnerId;// 宴会id
        private long _m_startTimeS;// 开始的时间
        private long _m_endTimeS;// 结束的时间
        private long _m_ownerCid;// 宴会举办者cid
        private long _m_score;// 当前宴会人气
        private bool _m_isJoined;// 是否加入宴会
        private int _m_joinerCount;// 当前赴宴玩家数量
        private int _m_listIdx;// 宴会在列表中的索引
        private Dinner_Idx _m_dinnerIdx;// 宴会索引信息
        private GDinnerTypeRefObj _m_dinnerTypeRef;//宴会的配置信息
        private bool _m_hasPre;// 是否有上一条宴会数据
        private bool _m_hasNext;// 是否有下一条宴会数据
        private EDinnerPermitType _m_permitType;// 凭证类型
        private long _m_permitTypeId;// 凭证类型ID
        private bool _m_isRegDetailInfo = false;
        
        private List<GDinnerJoinerInfo> _m_joinerInfoList;// 参与者列表

        
        public long instanceId { get => _m_instanceId; }
        public long dinnerId { get => _m_dinnerId; }
        public long startTimeS { get => _m_startTimeS; }// 宴会开始时间
        public long endTimeS { get => _m_endTimeS; }// 宴会结束时间
        public long ownerCid { get => _m_ownerCid; }// 宴会举办者cid
        public long score { get => _m_score; }// 宴会人气
        public bool isJoined { get => _m_isJoined; }// 是否加入宴会
        public int joinerCount { get => _m_joinerCount; }// 参与者数量
        public int listIdx { get => _m_listIdx; }// 宴会在列表中的索引
        public bool hasPre { get => _m_hasPre; }// 是否有上一条宴会数据
        public bool hasNext { get => _m_hasNext; }// 是否有下一条宴会数据
        public GDinnerTypeRefObj dinnerTypeRef { get => _m_dinnerTypeRef; }// 宴会配置信息
        public List<GDinnerJoinerInfo> joinerInfoList { get => _m_joinerInfoList; }// 参与者列表

        public GDinnerInfo(long _instanceId)
        {
            _m_instanceId = _instanceId;
            _m_isRegDetailInfo = false;
        }
        
        public GDinnerInfo(Dinner_Info _info, int _listIdx, bool _hasPre, bool _hasNext)
        {
            _m_instanceId = _info.getIdx().getInstanceId();
            updateDetailInfo(_info, _listIdx, _hasPre, _hasNext);
        }
        
        public GDinnerInfo(DinnerIndex _dinnerIdx)
        {
            if (_dinnerIdx == null)
                return;
            _m_instanceId = _dinnerIdx.instanceId;
            _m_dinnerId = _dinnerIdx.dinnerId;
            _m_isRegDetailInfo = false;
        }

        /// <summary>
        /// 更新详细信息
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_listIdx">列表顺序</param>
        public void updateDetailInfo(Dinner_Info _info,int _listIdx, bool _hasPre, bool _hasNext)
        {
            if (null == _info || _m_instanceId != _info.getIdx().getInstanceId())
            {
                return;
            }
            _m_dinnerIdx = _info.getIdx();
            _m_instanceId = _m_dinnerIdx.getInstanceId();
            _m_dinnerId = _m_dinnerIdx.getDinnerId();
            _m_ownerCid = _m_dinnerIdx.getOwnerCid();
            _m_score = _m_dinnerIdx.getScore();
            _m_isJoined = _m_dinnerIdx.getIsJoined();
            _m_joinerCount = _m_dinnerIdx.getJoinerCount();
            _m_startTimeS = _info.getStartTs();
            _m_endTimeS = _m_dinnerIdx.getEndTs();
            
            _m_listIdx = _listIdx;
            _m_hasPre = _hasPre;
            _m_hasNext = _hasNext;
            _m_permitType = _info.getPermitType();
            _m_permitTypeId = _info.getPermitTypeId();

            _m_joinerInfoList = new List<GDinnerJoinerInfo>();
            foreach (var dinnerJoiner in _info.getJoinerList())
            {
                _m_joinerInfoList.Add(new GDinnerJoinerInfo(dinnerJoiner));
            }

            _m_dinnerTypeRef = GRefdataCoreMgr.instance.dinnerTypeRefCore.getRef(_m_dinnerId);
            _m_isRegDetailInfo = true;
        }


        /// <summary>
        /// 请求详细数据
        /// </summary>
        /// <param name="_dealDone">，</param>
        /// <param name="_failDone">，表示宴会找不到或者已经结束</param>
        /// <param name="_isFouce"></param>
        public void regDetailInfo(Action<GDinnerInfo> _dealDone, Action _failDone, bool _isFouce = false)
        {
            if (_m_isRegDetailInfo && !_isFouce)
            {
                _dealDone?.Invoke(this);
                return;
            }

            NPPlayer.instance.dinnerComp.reqDinnerDetailInfo(_m_instanceId, (_info) =>
            {
                updateDetailInfo(_info?.getInfo(), _info.getIdx(), _info.getHasPre(), _info.getHasNext());
                _dealDone?.Invoke(this);
            }, _failDone);
        }

        /// <summary>
        /// 获取宴会剩余时间
        /// </summary>
        /// <returns></returns>
        public long getRemainTimeMs()
        {
            long remainTimeS = _m_endTimeS - FpsAndPingMgr.instance.serverTimeTagS;
            return math.max(remainTimeS, 0) * 1000;
        }

        public EDinnerPlayerStat getPlayerStat()
        {
            if (ownerCid == NPPlayer.instance.playerInfo.CID)
                return EDinnerPlayerStat.OWNER;
            // 如果已经参加或者人数已满
            if(_m_dinnerIdx.getIsJoined() || _m_dinnerIdx.getJoinerCount() >= _m_dinnerTypeRef.default_seat_num)
                return EDinnerPlayerStat.CANNOTJOIN;
            return EDinnerPlayerStat.CANJOIN;
        }

        /// <summary>
        /// 是否是妃子宴会
        /// </summary>
        /// <returns></returns>
        public bool isConsortDinner()
        {
            return _m_permitType == EDinnerPermitType.FAMILY;
        }

        public long getPermitConsortId()
        {
            return _m_permitTypeId;
        }

        /// <summary>
        /// 是否是卷王子嗣庆功宴
        /// </summary>
        public bool isGiftdeChildCeleDinner()
        {
            return _m_permitType == EDinnerPermitType.GIFTDE_CHILD_CELE;
        }

        public long getPermitChildId()
        {
            return _m_permitTypeId;
        }

        /// <summary>
        /// 获取参加宴会的玩家列表
        /// </summary>
        /// <returns></returns>
        public List<long> getJoinPlayerList()
        {
            List<long> joinPlayerList = new List<long>();
            if (_m_joinerInfoList != null)
                foreach (var joiner in _m_joinerInfoList)
                {
                    if (joiner != null && joiner.joinerType == EDinnerJoinerType.PLAYER)
                        joinPlayerList.Add(joiner.joinerId);
                }

            return joinPlayerList;
        }

        public List<DinnerDetailLogInfo> getDetailLogList()
        {
            List<DinnerDetailLogInfo> itemDataList = new List<DinnerDetailLogInfo>();
            
            itemDataList.Add(new DinnerDetailLogInfo(EDinnerDetailLogType.CREATE, _m_dinnerTypeRef, EDinnerJoinerType.PLAYER, _m_ownerCid, 0, 0, _m_startTimeS *  1000));
            foreach (GDinnerJoinerInfo joiner in _m_joinerInfoList)
            {
                if (joiner != null)
                    itemDataList.Add(new DinnerDetailLogInfo(EDinnerDetailLogType.JOIN, _m_dinnerTypeRef, joiner.joinerType, joiner.joinerId,
                        joiner.costId, joiner.score, joiner.joinTimeMs));
            }
            
            itemDataList.Sort((a, b) => b.timeMs.CompareTo(a.timeMs));
            return itemDataList;
        }
        
        public DinnerDetailLogInfo getLastDetailLog()
        {
            List<DinnerDetailLogInfo> logList = getDetailLogList();
            if (logList.Count > 0)
                return logList[logList.Count - 1];
            return null;
        }
        
        public GVideoClipIndex getDinnerVideoInfo(int _peopleCount)
        {
            GVideoClipIndex videoPathId = null;
            if (_m_dinnerTypeRef != null && _m_dinnerTypeRef.video_ids != null && _m_dinnerTypeRef.video_ids.Count > 0)
            {
                _m_dinnerTypeRef.video_ids.Sort(DinnerVideoInfo.sort);
                videoPathId = _m_dinnerTypeRef.video_ids[0]?.videoClipIndex;
                foreach (DinnerVideoInfo videoInfo in _m_dinnerTypeRef.video_ids)
                {
                    // 如果人数匹配，或者超过了则返回上一个
                    if (videoInfo.peopleCount > _peopleCount)
                        break;
                    
                    videoPathId = videoInfo.videoClipIndex;
                }
            }

            return videoPathId;
        }
        
        public GDinnerJoinerInfo getJoinerInfo(long _joinerId)
        {
            if (_m_joinerInfoList != null)
            {
                foreach (var joiner in _m_joinerInfoList)
                {
                    if (joiner.joinerId == _joinerId)
                        return joiner;
                }
            }
            return null;
        }

        public void updateJoinerInfo(Action _onUpdateDone)
        {
            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.regAllDoneDelegate(_onUpdateDone);
            stepCounter.chgTotalStepCount(1);

            if (_m_joinerInfoList != null)
            {
                stepCounter.chgTotalStepCount(_m_joinerInfoList.Count);
                for (var i = 0; i < _m_joinerInfoList.Count; i++)
                {
                    var joiner = _m_joinerInfoList[i];
                    if (joiner == null)
                    {
                        stepCounter.addDoneStepCount();
                        continue;
                    }
                    joiner.regDetailInfo(_info => { stepCounter.addDoneStepCount(); });
                }
            }
            stepCounter.addDoneStepCount();
              
        }
    }
}