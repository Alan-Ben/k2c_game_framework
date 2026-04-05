using System;
using Common.GuildObj;

namespace GOE
{
    /// <summary>
    /// 联盟入盟请求信息
    /// </summary>
    public class GuildJoinRequestInfo
    {
        //玩家cid
        private long _m_lCid;
        //数据库id
        private long _m_lDBId;
        //请求入盟时间
        private long _m_lRequestTimeMs;

        //====详细数据====
        private NPCommonSimplePlayerInfo _m_detailInfo;
        //是否正在请求数据
        private bool _m_bIsRequestingDetail;
        //是否初始化了数据
        private bool _m_bIsInitDetailDone;
        //初始化数据完成回调
        private Action<NPCommonSimplePlayerInfo> _m_aOnInitDetailDelegate;

        /// <summary>
        /// 玩家cid
        /// </summary>
        public long cid { get { return _m_lCid; } }
        /// <summary>
        /// 数据库id
        /// </summary>
        public long dbId { get { return _m_lDBId; } }
        /// <summary>
        /// 请求入盟时间
        /// </summary>
        public long requestTimeMs { get { return _m_lRequestTimeMs; } }

        public GuildJoinRequestInfo(Guild_JoinRequestInfo _info)
        {
            updateBaseInfo(_info);
        }

        /// <summary>
        /// 更新基础信息
        /// </summary>
        /// <param name="_info"></param>
        public void updateBaseInfo(Guild_JoinRequestInfo _info)
        {
            if (_info == null)
                return;
        
            _m_lCid = _info.getCid();
            _m_lDBId = _info.getDbId();
            _m_lRequestTimeMs = _info.getRequestTimeMs();
        }

        #region 获取成员详情

        /// <summary>
        /// 请求获取成员详情
        /// </summary>
        /// <param name="_onComplete">完成回调</param>
        /// <param name="_reqNew">是否直接请求新数据</param>
        public void getPlayerDetailInfo(Action<NPCommonSimplePlayerInfo> _onComplete, bool _reqNew = true)
        {
            if (_onComplete == null)
                return;

            //如果已经有数据并且不请求新数据，直接返回数据
            if (_m_bIsInitDetailDone && _m_detailInfo != null && !_reqNew)
                _onComplete.Invoke(_m_detailInfo);
            else if (_m_aOnInitDetailDelegate == null)
                _m_aOnInitDetailDelegate = _onComplete;
            else
                _m_aOnInitDetailDelegate += _onComplete;

            //是否正在请求玩家详情
            if (!_m_bIsRequestingDetail)
                _reqDetailInfo();
        }

        /// <summary>
        /// 请求详细信息
        /// </summary>
        private void _reqDetailInfo()
        {
            _m_bIsRequestingDetail = true;
            //请求玩家简要信息
            NPPlayer.instance.rankCommonComp.reqPlayerBriefInfo(_m_lCid, _info =>
            {
                //设置状态信息
                _m_bIsRequestingDetail = false;
                _m_bIsInitDetailDone = true;
                _m_detailInfo = new NPCommonSimplePlayerInfo(_info);

                //执行回调
                Action<NPCommonSimplePlayerInfo> onDone = _m_aOnInitDetailDelegate;
                _m_aOnInitDetailDelegate = null;
                onDone?.Invoke(_m_detailInfo);
            });
        }

        #endregion
    }
}