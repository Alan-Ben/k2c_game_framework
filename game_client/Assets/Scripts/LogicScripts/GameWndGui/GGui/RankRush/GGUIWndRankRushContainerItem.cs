using ALPackage;
using System;
using Common.ActivityEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 限时冲榜列表item
    /// </summary>
    public class GGUIWndRankRushContainerItem : _ATALBasicUISubWnd<GGUIMonoRankRushContainerItem>
    {
        //banner图
        private NPGGuiWndTexture _m_wBannerTex;
        //冲榜信息
        private ActivityRankRushInfo _m_rankRushInfo;
        //任务刷新定时器
        private ALCommonEnableTaskController _m_tcTickTaskController;

        public GGUIWndRankRushContainerItem(GGUIMonoRankRushContainerItem _wnd)
            : base(_wnd)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wBannerTex?.hideWnd();
            _m_tcTickTaskController.setDisable();
        }

        protected override void _onReset()
        {
            _m_wBannerTex?.discardTexture();
        }

        protected override void _onDiscard()
        {
            _m_wBannerTex?.discard();
            _m_wBannerTex = null;

            _m_tcTickTaskController.setDisable();

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClick, _onClickItem);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.imgBanner != null)
                _m_wBannerTex = new NPGGuiWndTexture(wnd.imgBanner);

            ALUGUICommon.combineBtnClick(wnd.btnClick, _onClickItem);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_rankRushInfo"></param>
        public void setInfo(ActivityRankRushInfo _rankRushInfo)
        {
            _m_rankRushInfo = _rankRushInfo;
            _refreshWnd();

            //先重置任务
            _m_tcTickTaskController.setDisable();
            //开启任务刷新倒计时显示
            _m_tcTickTaskController = ALCommonTaskController.CommonEnableDurationActionAddMonoTask(_onCDRefresh, 1f);
        }

        //刷新窗口
        private void _refreshWnd()
        {
            if (wnd == null || _m_rankRushInfo == null || _m_rankRushInfo.activityRankRushRefObj == null || _m_rankRushInfo.activityInfo == null)
                return;

            //设置名称
            ALUGUICommon.setLabelTxt(wnd.txtName, TextTranslate.instance.getLanguage(_m_rankRushInfo.rankRefObj?.nameStr));

            //设置图片
            if (_m_wBannerTex != null)
            {
                _m_wBannerTex.showWnd();
                _m_wBannerTex.setTexture(_m_rankRushInfo.activityRankRushRefObj.banner_tex);
            }

            //设置日期
            DateTime startTime = TimeUtil.FromUTCByTimeZone(_m_rankRushInfo.activityInfo.startTimeMs);
            DateTime closeTime = TimeUtil.FromUTCByTimeZone(_m_rankRushInfo.activityInfo.closeTimeMs);
            ALUGUICommon.setLabelTxt(wnd.txtTime, TextTranslate.instance.getLanguage(TransKeyConst.common_time_str, TimeUtil.DateTime2String_DurationLong_MD_HM(startTime, closeTime)));

            //设置显示状态
            wnd.setState(_m_rankRushInfo.activityInfo.activityState);

            //设置跨服显隐
            ALUGUICommon.setGameObjEnable(wnd.goCrossShowList, _m_rankRushInfo.activityInfo.isCross);
            ALUGUICommon.setGameObjEnable(wnd.goLocalShowList, !_m_rankRushInfo.activityInfo.isCross);

            //刷新一次倒计时
            _onCDRefresh();
            //刷新结算信息
            _refreshSettleInfo();
        }

        //倒计时刷新
        private void _onCDRefresh()
        {
            if (wnd == null || _m_rankRushInfo == null || _m_rankRushInfo.activityRankRushRefObj == null || _m_rankRushInfo.activityInfo == null)
                return;

            //当前状态结束时间
            long curStateFinishTimeMs = 0;
            //当前状态文本颜色
            Color curTextColor = wnd.getCDTextColor(_m_rankRushInfo.activityInfo.activityState);
            //翻译key
            string transKey = null;

            switch (_m_rankRushInfo.activityInfo.activityState)
            {
                case EActivityState.PLAYING:
                    curStateFinishTimeMs = _m_rankRushInfo.activityInfo.endTimeMs;
                    transKey = TransKeyConst.rankRush_playing_str;
                    break;
                case EActivityState.SETTLING:
                    curStateFinishTimeMs = _m_rankRushInfo.activityInfo.settleTimeMs;
                    transKey = TransKeyConst.rankRush_settling_str;
                    break;
                case EActivityState.REWARDING:
                    curStateFinishTimeMs = _m_rankRushInfo.activityInfo.closeTimeMs;
                    transKey = TransKeyConst.rankRush_gettingReward_str;
                    break;
            }

            long leftTimeMs = curStateFinishTimeMs - FpsAndPingMgr.instance.serverTimeTag;
            if (leftTimeMs < 0)
                leftTimeMs = 0;

            if(!string.IsNullOrEmpty(transKey))
                ALUGUICommon.setLabelTxt(wnd.txtCD, GCommon.addColorForRichText(TextTranslate.instance.getLanguage(transKey, TimeUtil.millisecondsToTime_dhms(leftTimeMs)), curTextColor));
        }

        //刷新结算信息
        private void _refreshSettleInfo()
        {
            if(wnd == null || _m_rankRushInfo == null)
                return;

            //设置领奖默认显示状态
            ALUGUICommon.setGameObjEnable(wnd.goCanGetRewardHideList, true);
            ALUGUICommon.setGameObjEnable(wnd.goCanGetRewardShowList, false);

            //设置红点默认显示状态
            ALUGUICommon.setGameObjEnable(wnd.goRedTip, _m_rankRushInfo.isNew);

            _m_rankRushInfo.getSettleInfo(_settleInfo =>
            {
                if(wnd == null || !isShow)
                    return;

                //是新的或者可领取奖励，显示红点
                ALUGUICommon.setGameObjEnable(wnd.goRedTip, _m_rankRushInfo.isNew || (_settleInfo != null && !_settleInfo.getHadDraw() && _settleInfo.getRank() > 0));
                //设置领奖显示状态
                ALUGUICommon.setGameObjEnable(wnd.goCanGetRewardHideList, _settleInfo != null && (_settleInfo.getHadDraw() || _settleInfo.getRank() <= 0));
                ALUGUICommon.setGameObjEnable(wnd.goCanGetRewardShowList, _settleInfo != null && ((!_settleInfo.getHadDraw() && _settleInfo.getRank() > 0)));
            }, _m_rankRushInfo.isGuildRankRush);
        }

        //点击事件
        private void _onClickItem(GameObject _go)
        {
            if (_m_rankRushInfo == null)
                return;

            if(_m_rankRushInfo.isGuildRankRush)
                QueueMgr.instance.AddNode(new GNodeGuildRankRushDetail(_m_rankRushInfo, ERankRushDetailTabType.REWARD, true));
            else
                QueueMgr.instance.AddNode(new GNodeRankRushDetail(_m_rankRushInfo, ERankRushDetailTabType.REWARD, true));
        }
    }
}
