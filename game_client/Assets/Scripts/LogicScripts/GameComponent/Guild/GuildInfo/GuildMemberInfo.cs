using System;
using Common.GuildObj;
using NPCommon;

namespace GOE
{
    /// <summary>
    /// 联盟成员信息
    /// </summary>
    public class GuildMemberInfo
    {
        //玩家cid
        private long _m_lCid;
        //职位id
        private long _m_lPositionId;

        //====详细数据====
        private NPCommonSimplePlayerInfo _m_detailInfo;
        //是否正在请求数据
        private bool _m_bIsRequestingDetail;
        //是否初始化了数据
        private bool _m_bIsInitDetailDone;
        //初始化数据完成回调
        private Action<NPCommonSimplePlayerInfo> _m_aOnInitDetailDelegate;

        //====贡献度数据====
        private Guild_MemberContributeInfo _m_contributeInfo;
        //是否正在请求贡献度数据
        private bool _m_bIsRequestingContribute;
        //是否初始化了贡献度数据
        private bool _m_bIsInitContributeDone;
        //初始化贡献度数据完成回调
        private Action<Guild_MemberContributeInfo> _m_aOnInitContributeDelegate;


        /// <summary>
        /// 玩家cid
        /// </summary>
        public long cid { get { return _m_lCid; } }
        /// <summary>
        /// 职位id
        /// </summary>
        public long positionId { get { return _m_lPositionId; } }
        /// <summary>
        /// 玩家详细信息（需要先请求才有数据）
        /// </summary>
        public NPCommonSimplePlayerInfo playerDetailInfo { get { return _m_detailInfo; } }

        public GuildMemberInfo(Guild_MemberBaseInfo _info)
        {
            updateBaseInfo(_info);
        }

        /// <summary>
        /// 更新基础信息
        /// </summary>
        /// <param name="_info"></param>
        public void updateBaseInfo(Guild_MemberBaseInfo _info)
        {
            if (_info == null)
                return;
        
            _m_lCid = _info.getCid();
            _m_lPositionId = _info.getPositionId();
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
            {
                _onComplete.Invoke(_m_detailInfo);
                return;
            }
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

        #region 获取成员贡献度

        /// <summary>
        /// 请求获取成员贡献度
        /// </summary>
        /// <param name="_onComplete">完成回调</param>
        /// <param name="_reqNew">是否直接请求新数据</param>
        public void getContributeInfo(Action<Guild_MemberContributeInfo> _onComplete, bool _reqNew = true)
        {
            if (_onComplete == null)
                return;

            //如果已经有数据并且不请求新数据，直接返回数据
            if (_m_bIsInitContributeDone && _m_contributeInfo != null && !_reqNew)
            {
                _onComplete.Invoke(_m_contributeInfo);
                return;
            }
            else if (_m_aOnInitContributeDelegate == null)
                _m_aOnInitContributeDelegate = _onComplete;
            else
                _m_aOnInitContributeDelegate += _onComplete;

            //是否正在请求玩家详情
            if (!_m_bIsRequestingContribute)
                _reqContributeInfo();
        }

        /// <summary>
        /// 请求贡献度信息
        /// </summary>
        private void _reqContributeInfo()
        {
            _m_bIsRequestingContribute = true;
            //请求玩家简要信息
            NPPlayer.instance.guildComp.reqGuildMemberContribute(_m_lCid, _info =>
            {
                //设置状态信息
                _m_bIsRequestingContribute = false;
                _m_bIsInitContributeDone = true;
                _m_contributeInfo = _info?.getMember();

                //执行回调
                Action<Guild_MemberContributeInfo> onDone = _m_aOnInitContributeDelegate;
                _m_aOnInitContributeDelegate = null;
                onDone?.Invoke(_m_contributeInfo);
            });
        }

        #endregion
    }
}