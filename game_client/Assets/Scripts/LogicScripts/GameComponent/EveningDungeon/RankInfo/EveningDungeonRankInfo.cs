using System;
using ALPackage;
using CommonEnum;
using NPCommon;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 晚间副本排行信息
    /// </summary>
    public class EveningDungeonRankInfo : _ARankCommonShowInfo<EveningDungeonRankInfo>
    {
        //基础信息 - 排名名次
        private int _m_rankSortId;

        //基础信息 - 排行玩家的cid
        private long _m_cid;

        //基础信息 - 排行分数
        private long _m_rankScore;

        //基础信息 - 分数来源id
        private long _m_sourceId;

        //玩家信息
        private NPCommonSimplePlayerInfo _m_playerInfo;

        //当前是否请求了详细信息
        private bool _m_isReqDetail;
        private bool _m_bIsDetailDone;

        //请求回调对象
        private Action<EveningDungeonRankInfo> _m_dRequestDelegate;

        //请求回调对象
        private Action<EveningDungeonRankInfo> _m_dRequestLikeDelegate;

        //构造函数
        public EveningDungeonRankInfo(Common.RankObj.Rank_BaseItem item)
        {
            update(item);
        }

        //排名名次
        public override int rankSortId { get { return _m_rankSortId; } }

        public override EValueFormatType rankScoreFormat { get { return EValueFormatType.NORMAL; } }
        
        //排行对应的玩家cid
        public override long cid { get { return _m_cid; } }
        public override ERankDetailShowType showType { get; }
        public override bool isCross { get { return false; } }

        //排行分数
        public override long rankScore { get { return _m_rankScore; } }

        //来源id
        public long sourceId { get { return _m_sourceId; } }

        //分数来源文本
        public string sourceTxt { get { return GCommon.getItemName(ENPItemType.HERO, _m_sourceId); } }

        //玩家信息
        public override NPCommonSimplePlayerInfo playerInfo { get { return _m_playerInfo; } }

        //当前是否请求了详细信息
        public bool isReqDetail { get { return _m_isReqDetail; } }
        public bool isDetailDone { get { return _m_bIsDetailDone; } }

        public void update(Common.RankObj.Rank_BaseItem item)
        {
            if (null == item)
                return;

            _m_rankScore = item.getScore();
            _m_cid = item.getKey();
            _m_sourceId = item.getSourceId();
            _m_rankSortId = item.getRank();
            _m_isReqDetail = false;
            _m_bIsDetailDone = false;
            _m_dRequestDelegate = null;
        }   
        
        /// <summary>
        /// 更新排行数据, 玩家数据不变
        /// </summary>
        public void updateRankInfo(int _rank, long _rankScore)
        {
            _m_rankSortId = _rank;
            _m_rankScore = _rankScore;
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

        private void checkIsDone(Action<EveningDungeonRankInfo> _callBack)
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

        //获取数据
        public override void getInfo(bool _isDetail, Action<EveningDungeonRankInfo> _callBack)
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
    }
}