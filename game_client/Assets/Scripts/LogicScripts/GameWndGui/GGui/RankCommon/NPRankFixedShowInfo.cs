using ALPackage;
using System;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 常驻排行榜显示数据
    /// </summary>
    public class NPRankFixedShowInfo
    {
        //常驻排行榜id
        private long _m_lRankFixedId;
        //个人或公会id
        private long _m_cid;
        //个人或工会分数
        private long _m_score;
        //当前是否请求了排行榜数据
        private bool _m_isReqDetail;
        //是否请求完成
        private bool _m_bIsDetailDone;
        //第一名玩家信息
        private NPCommonSimplePlayerInfo _m_firstPlayerInfo;
        //第一名公会信息
        private GuildOtherInfo _m_firstGuildInfo;
        //请求完成回调
        private Action<NPRankFixedShowInfo> _m_dRequestDelegate;


        /// <summary>
        /// 第一名玩家或公会id 0表示没有第一名
        /// </summary>
        public long cid { get { return _m_cid; } }
        /// <summary>
        /// 第一名分数
        /// </summary>
        public long score { get { return _m_score; } }
        /// <summary>
        /// 第一名玩家信息
        /// </summary>
        public NPCommonSimplePlayerInfo firstPlayerInfo { get { return _m_firstPlayerInfo; } }
        /// <summary>
        /// 第一名公会信息
        /// </summary>
        public GuildOtherInfo firstGuildInfo { get { return _m_firstGuildInfo; } }


        //构造函数
        public NPRankFixedShowInfo()
        {
        }

        /// <summary>
        /// 设置加载完成
        /// </summary>
        protected void _setDetailDone()
        {
            //提前清除了数据 过滤掉
            if (!_m_isReqDetail)
                return;

            //设置数据加载完成
            _m_bIsDetailDone = true;

            if (null != _m_dRequestDelegate)
                _m_dRequestDelegate(this);
            _m_dRequestDelegate = null;
        }

        //请求第一名排行榜信息
        public void reqFirstRankInfo(long _rankFixedId, Action<NPRankFixedShowInfo> _callBack)
        {
            _m_lRankFixedId = _rankFixedId;

            //已经完成直接调用
            if (_m_bIsDetailDone)
            {
                if(null != _callBack)
                    _callBack(this);
                return;
            }

            //注册回调返回
            if (null == _m_dRequestDelegate)
                _m_dRequestDelegate = _callBack;
            else
                _m_dRequestDelegate += _callBack;

            //判断是否开始请求，已经开始请求则直接返回
            if (_m_isReqDetail)
                return;

            _m_isReqDetail = true;

            NPPlayer.instance.rankCommonComp.reqInRankPlayerInfoByRank(_rankFixedId, 1, (_player)=>
            {
                if (null == _player)
                {
                    //输出日志
                    ALLog.Error($"can not find {_rankFixedId}'s 1st info!");

                    _setDetailDone();
                }
                else
                {
                    _m_cid = _player.getKey();
                    if (_m_cid == 0)
                    {
                        _setDetailDone();
                        return;
                    }

                    _m_score = _player.getScore();
                    _reqFirstInfo(_player.getKey());
                }
            });
        }

        //请求第一名玩家信息
        private void _reqFirstInfo(long _cid)
        {
            NPRankFixedRefObj rankFixRef = GRefdataCoreMgr.instance.rankFixedRefCore.getRef(_m_lRankFixedId);
            NPRankRefObj rankRef = GRefdataCoreMgr.instance.rankCommonRefCore.getRef(rankFixRef != null ? rankFixRef.rank_id : 0);
            if (rankRef != null && rankRef.rank_type == ERankType.GUILD)
            {
                //联盟排行榜
                NPPlayer.instance.guildComp.reqOtherGuildInfo(_cid, info =>
                {
                    if (info != null)
                        _m_firstGuildInfo = new GuildOtherInfo(info);
                    _setDetailDone();

                }, () =>
                {
                    _m_firstGuildInfo = null;
                    _setDetailDone();
                });
            }
            else
            {
                //个人排行榜
                NPPlayer.instance.rankCommonComp.reqPlayerBriefInfo(_cid, (_player) =>
                {
                    if (null == _player)
                    {
                        //输出日志
                        ALLog.Error($"can not find {_cid}'s BriefInfo!");

                        _setDetailDone();
                        return;
                    }
                    else
                    {
                        if (null == _m_firstPlayerInfo)
                            _m_firstPlayerInfo = new NPCommonSimplePlayerInfo(_player);
                        else
                            _m_firstPlayerInfo.update(_player);

                        _setDetailDone();
                    }
                });
            }
        }

        public void clearData()
        {
            _m_dRequestDelegate = null;
            _m_isReqDetail = false;
            _m_bIsDetailDone = false;
        }
    }
}
