using ALPackage;
using CommonEnum;
using NPCommon;
using NPEnum;
using System;

namespace GOE
{
    // 排行榜显示数据
    public class NPRankCommonShowInfo : _ARankCommonShowInfo<NPRankCommonShowInfo>
    {
        //排行榜id 
        private long _m_rankId;
        private NPRankRefObj _m_rankRefObj;

        //排行榜实例id
        private long _m_rankFixedId;

        //基础信息 - 排名名次
        private int _m_rankSortId;

        //基础信息 - 排行玩家的cid
        private long _m_cid;

        //基础信息 - 排行分数
        private long _m_rankScore;

        //基础信息 - 分数来源id
        private long _m_sourceId;

        //工会或个人所在的服
        private int _m_serverId;

        //是否是跨服排行榜
        private bool _m_isCross;

        //玩家信息
        private NPCommonSimplePlayerInfo _m_playerInfo;

        //当前是否请求了详细信息
        private bool _m_isReqDetail;
        private bool _m_bIsDetailDone;

        //请求回调对象
        private Action<NPRankCommonShowInfo> _m_dRequestDelegate;

        //点赞积分
        private long _m_likeScore;

        //当前是否请求了点赞积分
        private bool _m_isReqLike;
        private bool _m_bIsLikeDone;
        //请求回调对象
        private Action<NPRankCommonShowInfo> _m_dRequestLikeDelegate;


        //构造函数
        public NPRankCommonShowInfo(Common.RankObj.Rank_BaseItem item, bool _isCross)
        {
            if (null == item)
                return;

            _m_rankScore = item.getScore();
            _m_cid = item.getKey();
            _m_sourceId = item.getSourceId();
            _m_rankSortId = item.getRank();
            _m_isCross = _isCross;
            _m_isReqDetail = false;
            _m_bIsDetailDone = false;
            _m_isReqLike = false;
            _m_bIsLikeDone = false;
            _m_dRequestDelegate = null;
        }

        //排行榜id 
        public long rankId { get { return _m_rankId; } }

        //排行榜实例id
        public long rankFixedId { get { return _m_rankFixedId; } }

        //排名名次
        public override int rankSortId { get { return _m_rankSortId; } }

        //排行对应的玩家cid
        public override EValueFormatType rankScoreFormat { get { return _m_rankRefObj?.process_num_format ?? EValueFormatType.NORMAL; } }
        public override long cid { get { return _m_cid; } }
        public override ERankDetailShowType showType { get { return _m_rankRefObj?.show_type ?? ERankDetailShowType.NONE; } }

        //排行分数
        public override long rankScore { get { return _m_rankScore; } }

        //来源id
        public long sourceId { get { return _m_sourceId; } }

        //分数来源文本
        public string sourceTxt { get { return GCommon.getItemName(ENPItemType.HERO, _m_sourceId); } }

        //工会或个人所在的服
        public int serverId { get { return _m_serverId; } }

        //点赞积分
        public long likeScore { get { return _m_likeScore; } }

        //玩家信息
        public override NPCommonSimplePlayerInfo playerInfo { get { return _m_playerInfo; } }

        //当前是否请求了详细信息
        public bool isReqDetail { get { return _m_isReqDetail; } }
        public bool isDetailDone { get { return _m_bIsDetailDone; } }

        //当前是否请求了点赞信息
        public bool isReqLike { get { return _m_isReqDetail; } }
        public bool isLikeDone { get { return _m_bIsDetailDone; } }
        public override bool isCross { get { return _m_isCross; } }

        //设置排行榜id
        public void setRankId(long _rankId, long _rankFixedId)
        {
            _m_rankId = _rankId;
            _m_rankRefObj = GRefdataCoreMgr.instance.rankCommonRefCore.getRef(_m_rankId);

            _m_rankFixedId = _rankFixedId;
        }

        //设置简要信息
        public void setBriefInfo(PlayerInfo_IconShow _playerInfo)
        {
            if (null == _playerInfo)
                return;

            _m_playerInfo = new NPCommonSimplePlayerInfo(_playerInfo);
        }

        //设置详细信息
        public void setDetailInfo(Common.NpPlayerInfoObj.PlayerInfo_CommonShow _playerInfo)
        {
            if (null == _playerInfo)
                return;

            _m_playerInfo = new NPCommonSimplePlayerInfo(_playerInfo);
        }

        /// <summary>
        /// 设置加载完成
        /// </summary>
        protected void _setDetailDone()
        {
            //设置数据加载完成
            _m_bIsDetailDone = true;

            if (null != _m_dRequestDelegate)
                _m_dRequestDelegate(this);
            _m_dRequestDelegate = null;
        }

        /// <summary>
        /// 设置加载完成
        /// </summary>
        protected void _setLikeDone()
        {
            //设置数据加载完成
            _m_bIsLikeDone = true;

            if (null != _m_dRequestLikeDelegate)
                _m_dRequestLikeDelegate(this);
            _m_dRequestLikeDelegate = null;
        }

        private void checkIsDone(Action<NPRankCommonShowInfo> _callBack)
        {
            //已经完成直接调用
            if (_m_bIsDetailDone)
            {
                if (null != _callBack)
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
        }

        private void checkIsLikeDone(Action<NPRankCommonShowInfo> _callBack)
        {
            //已经完成直接调用
            if (_m_bIsLikeDone)
            {
                if (null != _callBack)
                    _callBack(this);
                return;
            }

            //注册回调返回
            if (null == _m_dRequestLikeDelegate)
                _m_dRequestLikeDelegate = _callBack;
            else
                _m_dRequestLikeDelegate += _callBack;

            //判断是否开始请求，已经开始请求则直接返回
            if (_m_isReqLike)
                return;

            _m_isReqLike = true;
        }

        //获取数据
        public override void getInfo(bool _isDetail, Action<NPRankCommonShowInfo> _callBack)
        {
            checkIsDone(_callBack);
            if (_m_bIsDetailDone)
                return;

            if (_isDetail)
            {
                GCommon.reqPlayerInfoSer(_m_cid, (_rankPlayer) =>
                {
                    if (null == _rankPlayer)
                    {
                        //输出日志
                        ALLog.Error($"can not find {_m_cid}'s detailInfo!");
                        _setDetailDone();
                    }
                    else
                    {
                        //设置信息
                        setDetailInfo(_rankPlayer.getSomeOneShowInfo());
                        _setDetailDone();
                    }
                });
            }
            else
            {
                NPPlayer.instance.rankCommonComp.reqPlayerBriefInfo(_m_cid, (_rankPlayer) =>
                {
                    if (null == _rankPlayer)
                    {
                        //输出日志
                        ALLog.Error($"can not find {_m_cid}'s briefInfo!");
                        _setDetailDone();
                    }
                    else
                    {
                        //设置信息
                        setBriefInfo(_rankPlayer);
                        _setDetailDone();
                    }
                });
            }
        }

        //获取点赞数据
        public void getLikeScore(Action<NPRankCommonShowInfo> _callBack)
        {
            checkIsLikeDone(_callBack);
            if (_m_bIsLikeDone)
                return;

            //获取点赞积分
            NPPlayer.instance.rankCommonComp.reqRankLikeScore(_m_rankFixedId, _m_cid, (_likeScore) =>
            {
                _m_likeScore = _likeScore;
                _setLikeDone();
            });
        }

        /// <summary>
        /// 获取分数来源文本
        /// </summary>
        public bool isShowSource()
        {
            if (null == _m_rankRefObj)
                return false;

            return _m_rankRefObj.show_type == ERankDetailShowType.HERO;
        }
    }
}
