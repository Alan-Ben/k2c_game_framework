
using Common.ActivityObj;
using System;
using Common.ActivityEnum;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 本服冲榜信息数据
    /// </summary>
    public class ActivityRankRushInfo
    {
        //活动信息
        private _ABaseActivityInfo _m_activityInfo;
        //冲榜配置
        private ActivityRankRushRefObj _m_activityRankRushRefObj;
        //排行榜配置
        private NPRankRefObj _m_rankRefObj;

        //====领奖数据====
        //领奖期自己的领取信息
        private Activity_RankSettleInfo _m_selfSettleInfo;
        //是否正在请求领取信息
        private bool _m_bIsRequestingSettleInfo;
        //是否初始化了领取信息
        private bool _m_bIsInitSettleInfoDone;
        //初始化领取信息完成回调
        private Action<Activity_RankSettleInfo> _m_aOnInitSettleInfoDelegate;

        /// <summary>
        /// 活动信息
        /// </summary>
        public _ABaseActivityInfo activityInfo { get { return _m_activityInfo; } }
        /// <summary>
        /// 冲榜配置
        /// </summary>
        public ActivityRankRushRefObj activityRankRushRefObj { get { return _m_activityRankRushRefObj; } }
        /// <summary>
        /// 排行榜配置
        /// </summary>
        public NPRankRefObj rankRefObj { get { return _m_rankRefObj; } }
        /// <summary>
        /// 是否是联盟冲榜
        /// </summary>
        public bool isGuildRankRush { get { return _m_rankRefObj != null && _m_rankRefObj.rank_type == ERankType.GUILD; } }
        /// <summary>
        /// 是否是新的冲榜
        /// </summary>
        public bool isNew
        {
            get
            {
                if (_m_activityRankRushRefObj == null || _m_activityInfo == null)
                    return false;

                return AccountSettingMgr.instance.accountSetting.getIsNewRankRush(_m_activityRankRushRefObj.id, _m_activityInfo.instanceId);
            }
        }
        /// <summary>
        /// 是否展示在冲榜活动界面
        /// </summary>
        public bool isShowInRankRushWnd { get { return _m_activityRankRushRefObj != null && _m_activityRankRushRefObj.is_show_in_rank_rush_wnd; } }

        public ActivityRankRushInfo(_ABaseActivityInfo _activityInfo, ActivityRankRushRefObj _rankRushRef)
        {
            _m_activityInfo = _activityInfo;
            _m_activityRankRushRefObj = _rankRushRef;
            if(_m_activityRankRushRefObj != null)
                _m_rankRefObj = GRefdataCoreMgr.instance.rankCommonRefCore.getRef(_m_activityRankRushRefObj.rank_id);
        }

        /// <summary>
        /// 设置冲榜已看
        /// </summary>
        public void setIsReadNew()
        {
            if (_m_activityRankRushRefObj == null || _m_activityInfo == null)
                return;

            AccountSettingMgr.instance.accountSetting.setRecordRankRush(_m_activityRankRushRefObj.id, _m_activityInfo.instanceId);
            //刷新红点
            NPPlayer.instance.commonActivityComp.refreshRankRushRewardRedTip();
        }

        /// <summary>
        /// 设置已经领取奖励
        /// </summary>
        /// <param name="_isGet"></param>
        public void setIsGetReward(bool _isGet)
        {
            _m_selfSettleInfo?.setHadDraw(_isGet);
            //刷新红点
            NPPlayer.instance.commonActivityComp.refreshRankRushRewardRedTip();
        }


        #region 获取领奖期自己的领取信息

        /// <summary>
        /// 请求获取自己领取信息
        /// </summary>
        /// <param name="_onComplete">完成回调</param>
        /// <param name="_reqNewInfo">是否请求最新数据，如果是联盟冲榜可能会有联盟变动需要获取最新数据</param>
        public void getSettleInfo(Action<Activity_RankSettleInfo> _onComplete, bool _reqNewInfo = false)
        {
            if (_onComplete == null)
                return;

            //如果活动信息为空或者活动状态不是领奖期，直接返回空
            if (_m_activityInfo == null || _m_activityInfo.activityState != EActivityState.REWARDING)
            {
                _onComplete.Invoke(null);
                return;
            }

            //如果已经有数据并且不请求新数据，直接返回数据
            if (_m_bIsInitSettleInfoDone && _m_selfSettleInfo != null && !_reqNewInfo)
            {
                _onComplete.Invoke(_m_selfSettleInfo);
                return;
            }
            else if (_m_aOnInitSettleInfoDelegate == null)
                _m_aOnInitSettleInfoDelegate = _onComplete;
            else
                _m_aOnInitSettleInfoDelegate += _onComplete;

            //是否正在请求玩家详情
            if (!_m_bIsRequestingSettleInfo)
                _reqSettleInfo();
        }

        /// <summary>
        /// 请求自己领取信息
        /// </summary>
        private void _reqSettleInfo()
        {
            if (_m_activityInfo == null || _m_activityRankRushRefObj == null)
            {
                //执行回调
                Action<Activity_RankSettleInfo> onDone = _m_aOnInitSettleInfoDelegate;
                _m_aOnInitSettleInfoDelegate = null;
                onDone?.Invoke(null);
                return;
            }

            _m_bIsRequestingSettleInfo = true;

            //请求自己的结算信息
            NPPlayer.instance.commonActivityComp.reqActivityRankSettleInfo(_m_activityInfo.instanceId, _m_activityRankRushRefObj.rank_id,
            (_isSuc, _msg) =>
            {
                //设置状态信息
                _m_bIsRequestingSettleInfo = false;
                _m_bIsInitSettleInfoDone = true;
                _m_selfSettleInfo = null;
                if (_isSuc)
                    _m_selfSettleInfo = _msg.getSettleInfo();
                //执行回调
                Action<Activity_RankSettleInfo> onDone = _m_aOnInitSettleInfoDelegate;
                _m_aOnInitSettleInfoDelegate = null;
                onDone?.Invoke(_m_selfSettleInfo);
            },false);
        }

        #endregion
    }
}