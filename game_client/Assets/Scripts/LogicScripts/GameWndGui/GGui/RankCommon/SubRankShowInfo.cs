using ALPackage;
using CommonEnum;
using NPCommon;
using NPEnum;
using System;
using Common.NpPlayerInfoObj;

namespace GOE
{
    /// <summary>
    /// 子排行榜显示数据
    /// </summary>
    public class SubRankShowInfo
    {
        //排行榜id 
        private long _m_rankId;
        //排行榜配置
        private NPRankRefObj _m_rankRefObj;
        //基础信息 - 排行玩家的cid
        private long _m_cid;
        //基础信息 - 排行分数
        private long _m_rankScore;
        //基础信息 - 分数来源id
        private long _m_sourceId;
        //工会或个人所在的服
        private int _m_serverId;
        //玩家信息
        private NPCommonSimplePlayerInfo _m_playerInfo;
        //当前是否请求了详细信息
        private bool _m_isReqDetail;
        //当前详细信息是否完成
        private bool _m_bIsDetailDone;
        //请求回调对象
        private Action<SubRankShowInfo> _m_dRequestDelegate;
        //请求回调对象
        private Action<SubRankShowInfo> _m_dRequestLikeDelegate;

        /// <summary>
        /// 排行榜id 
        /// </summary>
        public long rankId { get { return _m_rankId; } }
        /// <summary>
        /// 分数格式
        /// </summary>
        public EValueFormatType rankScoreFormat { get { return _m_rankRefObj?.process_num_format ?? EValueFormatType.NORMAL; } }
        /// <summary>
        /// 排行对应的玩家cid
        /// </summary>
        public long cid { get { return _m_cid; } }
        /// <summary>
        /// 显示类型
        /// </summary>
        public ERankDetailShowType showType { get { return _m_rankRefObj?.show_type ?? ERankDetailShowType.NONE; } }
        /// <summary>
        /// 排行分数
        /// </summary>
        public long rankScore { get { return _m_rankScore; } }
        /// <summary>
        /// 来源id
        /// </summary>
        public long sourceId { get { return _m_sourceId; } }
        /// <summary>
        /// 分数来源文本
        /// </summary>
        public string sourceTxt { get { return GCommon.getItemName(ENPItemType.HERO, _m_sourceId); } }
        /// <summary>
        /// 工会或个人所在的服
        /// </summary>
        public int serverId { get { return _m_serverId; } }
        /// <summary>
        /// 玩家信息
        /// </summary>
        public NPCommonSimplePlayerInfo playerInfo { get { return _m_playerInfo; } }
        /// <summary>
        /// 当前是否请求了详细信息
        /// </summary>
        public bool isReqDetail { get { return _m_isReqDetail; } }
        /// <summary>
        /// 当前详细信息是否完成
        /// </summary>
        public bool isDetailDone { get { return _m_bIsDetailDone; } }


        //构造函数
        public SubRankShowInfo(Common.RankObj.Rank_BaseSubItem item, long _rankId)
        {
            if (null == item)
                return;

            _m_rankScore = item.getScore();
            _m_cid = item.getKey();
            _m_sourceId = item.getSourceId();
            _m_rankId = _rankId;
            _m_rankRefObj = GRefdataCoreMgr.instance.rankCommonRefCore.getRef(_m_rankId);
            _m_isReqDetail = false;
            _m_bIsDetailDone = false;
            _m_dRequestDelegate = null;
        }

        //设置简要信息
        public void setBriefInfo(PlayerInfo_IconShow _playerInfo)
        {
            if (null == _playerInfo)
                return;

            _m_playerInfo = new NPCommonSimplePlayerInfo(_playerInfo);
        }

        //设置详细信息
        public void setDetailInfo(PlayerInfo_CommonShow _playerInfo)
        {
            if (null == _playerInfo)
                return;

            _m_playerInfo = new NPCommonSimplePlayerInfo(_playerInfo);
        }

        /// <summary>
        /// 设置加载完成
        /// </summary>
        private void _setDetailDone()
        {
            //设置数据加载完成
            _m_bIsDetailDone = true;

            if (null != _m_dRequestDelegate)
                _m_dRequestDelegate(this);
            _m_dRequestDelegate = null;
        }

        private void checkIsDone(Action<SubRankShowInfo> _callBack)
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
        public void getInfo(bool _isDetail, Action<SubRankShowInfo> _callBack)
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
