using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIWndGuildStepTaskRushRankActivity : _AGGUIWndStepTaskRushRankActivity<GGUIMonoGuildStepTaskRushRankActivity>
    {
        public GGUIWndGuildStepTaskRushRankActivity(long _activityId, NPCommonAssetPathInfo _assetPathInfo, Transform _parent) : base(_activityId, _assetPathInfo, _parent)
        {
        }

        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsg(WinMsgType.ON_COMMON_ACTIVITY_STEP_REWARD_SCORE_CHG, _onStepRewardScoreChg);
            
            base._onShowWnd();
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_COMMON_ACTIVITY_STEP_REWARD_SCORE_CHG, _onStepRewardScoreChg);

            base._onHideWnd();
        }

        protected override void _getRankInfo(Action<long, long> _onGetRankInfo)
        {
            if(_onGetRankInfo == null)
                return;
            
            long guildId = NPPlayer.instance.guildComp.guildInfo?.guildId ?? 0;
            if (guildId <= 0 || _m_rankRushInfo == null || _m_rankRushInfo.activityInfo == null || _m_rankRushInfo.activityRankRushRefObj == null)
            {
                _onGetRankInfo(0, 0);
                return;
            }
            
            NPPlayer.instance.commonActivityComp.reqActivityRankBaseInfoByKey(
                _m_rankRushInfo.activityInfo.instanceId,
                _m_rankRushInfo.activityRankRushRefObj.rank_id,
                guildId, _m_rankRushInfo.activityInfo.isCross,
                _msg =>
                {
                    // 回调校验：窗口已关闭或序列号不匹配则忽略
                    if (_msg == null || _msg.getBaseItem() == null || wnd == null || !isShow)
                        return;

                    _onGetRankInfo(_msg.getBaseItem().getRank(), _msg.getBaseItem().getScore());
                });
        }

        protected override void _refreshWnd(bool _resetInfo)
        {
            base._refreshWnd(_resetInfo);

            _refreshHasGuildShow();
            _refreshPersonalScore();
        }

        private void _refreshHasGuildShow()
        {
            if(wnd == null)
                return;
            
            bool hasGuild = NPPlayer.instance.guildComp.isJoinGuild();
            ALUGUICommon.setGameObjEnable(wnd.hasGuildShow, hasGuild);
            ALUGUICommon.setGameObjEnable(wnd.noGuildShow, !hasGuild);
        }

        /// <summary>
        /// 刷新个人积分
        /// </summary>
        private void _refreshPersonalScore()
        {
            if(wnd == null)
                return;
            
            ActivityStepRewardInfo stepRewardInfo = _m_lStepRewardInfoList?.SafeGet(0);//默认取第一个阶段奖励信息

            string scoreStr = string.Empty;
            if (stepRewardInfo != null)
            {
                scoreStr = GCommon.getValueFormatStr(stepRewardInfo.stepRewardSetRef?.process_num_format ?? EValueFormatType.NORMAL, stepRewardInfo.totalScore);
                if (wnd.txtPersonalScoreKey != null)
                {
                    scoreStr = TextTranslate.instance.getLanguage(wnd.txtPersonalScoreKey, scoreStr);
                }
            }
            ALUGUICommon.setLabelTxt(wnd.txtPersonalScore, scoreStr);
        }
        
        /// <summary>
        /// 阶段奖励积分变化回调
        /// 参数：无
        /// </summary>
        private void _onStepRewardScoreChg(params object[] _objs)
        {
            _refreshPersonalScore();//阶段积分变化可能会导致个人积分变化
        }
    }
}