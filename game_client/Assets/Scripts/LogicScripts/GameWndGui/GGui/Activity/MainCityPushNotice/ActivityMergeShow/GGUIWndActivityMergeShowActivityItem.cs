using System;
using ALPackage;
using Common.ActivityEnum;

namespace GOE
{
    /// <summary>
    /// 活动合并展示主城推送弹窗活动Item
    /// </summary>
    public class GGUIWndActivityMergeShowActivityItem : _ANPGGUIBasicGridItemWnd<GGUIMonoActivityMergeShowActivityItem>
    {
        private _ABaseActivityInfo _m_activityInfo;
        private PushNoticeActivityMergeRefObj _m_pushNoticeActivityMergeRefObj;

        private NPGGuiWndTexture _m_wBanner;

        /// <summary>
        /// 点击前往按钮事件
        /// </summary>
        public event Action<GGUIWndActivityMergeShowActivityItem> onGotoBtnClick;

        public GGUIWndActivityMergeShowActivityItem(GGUIMonoActivityMergeShowActivityItem _wnd) : base(_wnd)
        {
            initWnd();
        }

        public _ABaseActivityInfo activityInfo { get { return _m_activityInfo; } }
        public PushNoticeActivityMergeRefObj pushNoticeActivityMergeRefObj { get { return _m_pushNoticeActivityMergeRefObj; } }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            // 构建banner图展示
            if (wnd.banner != null)
                _m_wBanner = new NPGGuiWndTexture(wnd.banner);

            // 绑定前往按钮点击事件
            ALUGUICommon.combineBtnClick(wnd.goToBtn, _onGotoBtnClick);
        }

        protected override void _onDiscard()
        {
            onGotoBtnClick = null;

            // 解绑前往按钮点击事件
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.goToBtn, _onGotoBtnClick);
            }

            // 释放banner图展示
            _m_wBanner?.discard();
            _m_wBanner = null;

            _m_activityInfo = null;
            _m_pushNoticeActivityMergeRefObj = null;
        }
        
        protected override void _onShowWnd()
        {
            refreshWnd();
        }

        protected override void _onHideWnd()
        {
            _m_wBanner?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wBanner?.discardTexture();
        }

        protected override void _resetGridItem()
        {
        }

        /// <summary>
        /// 刷新界面(带参数)
        /// </summary>
        /// <param name="_activityInfo">活动信息</param>
        /// <param name="_refObj">活动合并展示配表数据</param>
        public void refreshWnd(_ABaseActivityInfo _activityInfo, PushNoticeActivityMergeRefObj _refObj)
        {
            _m_activityInfo = _activityInfo;
            _m_pushNoticeActivityMergeRefObj = _refObj;
            refreshWnd();
            secTick();
        }

        /// <summary>
        /// 刷新界面
        /// </summary>
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow)
                return;

            if (_m_activityInfo == null || _m_pushNoticeActivityMergeRefObj == null)
                return;

            // 获取活动配表数据
            GActivityMainRefObj activityMainRef = GRefdataCoreMgr.instance.activityMainRefCore.getRef(_m_activityInfo.activityId);

            // 设置活动名称
            string activityName = string.Empty;
            if (activityMainRef != null && !string.IsNullOrEmpty(activityMainRef.name))
                activityName = TextTranslate.instance.getLanguage(activityMainRef.name);
            ALUGUICommon.setLabelTxt(wnd.txtActivityName, activityName);
            
            // 设置活动banner
            if (_m_wBanner != null)
            {
                _m_wBanner.showWnd();
                _m_wBanner.setTexture(_m_pushNoticeActivityMergeRefObj.banner);
            }

            if (wnd.activityStateShowInfoList != null)
            {
                NPCommonEnumStatMutexShowInfo<EActivityState>.setStat(wnd.activityStateShowInfoList, _m_activityInfo.activityState);
            }
            
            // 设置活动时间
            _refreshActivityTime();
        }

        public void secTick()
        {
            if(wnd == null || !isShow || _m_activityInfo == null)
                return;

            long settlingTimeMs = _m_activityInfo.settleTimeMs;//结算时间戳
            //是否需要显示到结算倒计时(活动在运行时, 且距离结算时间小于等于配置的提前显示时间)
            bool needShowToSettlingCountDown = _m_activityInfo.activityState == EActivityState.PLAYING 
                                              && FpsAndPingMgr.instance.serverTimeTag + wnd.showToSettlingCountDownBeforeSettlingHour * 60 * 60 * 1000 >= settlingTimeMs;

            if (needShowToSettlingCountDown)
            {
                ALUGUICommon.setGameObjEnable(wnd.onShowToSettlingCountDownShow, true);
                ALUGUICommon.setGameObjEnable(wnd.onShowToSettlingCountDownHide, false);
                
                string countDownStr = TimeUtil.millisecondsToTime_Two(settlingTimeMs - FpsAndPingMgr.instance.serverTimeTag);
                if (!string.IsNullOrEmpty(wnd.txtToSettlingCountDownKey))
                    countDownStr = TextTranslate.instance.getLanguage(wnd.txtToSettlingCountDownKey, countDownStr);
                
                ALUGUICommon.setLabelTxt(wnd.txtToSettlingCountDown, countDownStr);
            }
            else
            {
                ALUGUICommon.setGameObjEnable(wnd.onShowToSettlingCountDownShow, false);
                ALUGUICommon.setGameObjEnable(wnd.onShowToSettlingCountDownHide, true);
            }
        }
        
        /// <summary>
        /// 刷新活动时间
        /// </summary>
        private void _refreshActivityTime()
        {
            if (wnd == null || wnd.txtActivityTime == null || _m_activityInfo == null)
                return;

            // 转换为本地时间
            System.DateTime startTime = TimeUtil.FromUTCByTimeZone(_m_activityInfo.startTimeMs);
            System.DateTime endTime = TimeUtil.FromUTCByTimeZone(_m_activityInfo.endTimeMs);

            // 格式化时间字符串
            string timeStr = TimeUtil.DateTime2String_DurationLong_YMD(startTime, endTime);
            ALUGUICommon.setLabelTxt(wnd.txtActivityTime, timeStr);
        }
        
        /// <summary>
        /// 前往按钮点击回调
        /// </summary>
        private void _onGotoBtnClick(UnityEngine.GameObject _)
        {
            onGotoBtnClick?.Invoke(this);
        }
    }
}
