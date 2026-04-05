using ALPackage;
using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 火星居民补充Hub
    /// </summary>
    public class GGUIWndMarsResidentReplenishFollowerController : _ATALGGUICommonFollowItemController<GGUIMonoMarsResidentReplenishFollower, GGUIWndMarsResidentReplenishFollower>
    {
        private readonly GResPathIndex _m_resIndex;
        private readonly MarsResidentReplenishView _m_ResidentReplenishView;

        public GGUIWndMarsResidentReplenishFollowerController(int _uiAssetPathId, MarsResidentReplenishView _view)
        {
            _m_resIndex = new GResPathIndex(_uiAssetPathId);
            _m_ResidentReplenishView = _view;
        }
        
        public override _AALBasicLoadResIndexInfo followItemIndex { get { return _m_resIndex; } }
        
        protected override GGUIWndMarsResidentReplenishFollower _createItemWnd(GGUIMonoMarsResidentReplenishFollower _wndMono)
        {
            if (_wndMono == null)
                return null;

            GGUIWndMarsResidentReplenishFollower wnd = new GGUIWndMarsResidentReplenishFollower(_wndMono, _m_ResidentReplenishView);
            wnd.showWnd();
            return wnd;
        }

        public override void onFollowRootShow()
        {
            if(wnd != null)
                wnd.showWnd();
        }

        public override void onFollowRootHide()
        {
            if(wnd != null)
                wnd.hideWnd();
        }

        public void refreshWnd()
        {
            if(wnd == null)
                return;
            
            wnd.refreshWnd();
        }
    }
    
    /// <summary>
    /// 火星居民补充Follower窗口
    /// </summary>
    public class GGUIWndMarsResidentReplenishFollower : _ATALGGUIWndCommonFollowItem<GGUIMonoMarsResidentReplenishFollower>
    {
        private readonly MarsResidentReplenishView _m_ResidentReplenishView;
        
        private Common.MarsObj.Mars_PeopleImmigrant _m_immigrantInfo;//居民移民信息
        
        private ALCommonEnableTaskController _m_countDownTaskController;//倒计时任务控制器
        
        public GGUIWndMarsResidentReplenishFollower(GGUIMonoMarsResidentReplenishFollower _wnd, MarsResidentReplenishView _view) : base(_wnd)
        {
            _m_ResidentReplenishView = _view;
            
            initWnd();
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            // 绑定按钮点击事件
            ALUGUICommon.combineBtnClick(wnd.btnClick, _onClickReplenish);
        }
        
        protected override void _onDiscard()
        {
            if (wnd != null)
            {
                // 解绑按钮点击事件
                ALUGUICommon.uncombineBtnClick(wnd.btnClick, _onClickReplenish);
            }
        }
        
        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsgAct(WinMsgType.SIMULATE_CLICK_MARS_RESIDENT_REPLENISH_FOLLOWER, _onSimulateClickReplenish);
            refreshWnd();
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsgAct(WinMsgType.SIMULATE_CLICK_MARS_RESIDENT_REPLENISH_FOLLOWER, _onSimulateClickReplenish);
            _discardCountDownTask();
        }

        protected override void _onReset()
        {
        }

        /// <summary>
        /// 刷新窗口显示
        /// </summary>
        public void refreshWnd()
        {
            if (wnd == null || !isShow)
                return;

            _m_immigrantInfo = NPPlayer.instance.marsComp.peopleSubComponent.getImmigrantInfo();
            if(_m_immigrantInfo != null && _m_immigrantInfo.getEndMs() > FpsAndPingMgr.instance.serverTimeTag)
            {
                // 有移民信息且未到结束时间，初始化倒计时任务
                _initCountDownTask();
            }
            else
            {
                // 无移民信息或已到结束时间，销毁倒计时任务
                _discardCountDownTask();
            }
            
            EMarsResidentReplenishState residentReplenishState = MarsUtil.getMarsResidentReplenishState();
            NPCommonEnumStatMutexShowInfo<EMarsResidentReplenishState>.setStat(wnd.replenishStateShowList, residentReplenishState);
        }

        private void _refreshCountDown()
        {
            if (wnd == null)
                return;

            long leftCountDownMs = (_m_immigrantInfo?.getEndMs() ?? 0) - FpsAndPingMgr.instance.serverTimeTag;
            if (leftCountDownMs <= 0)
                leftCountDownMs = 0;
            
            string countDownStr = TimeUtil.millisecondsToTime_hms(leftCountDownMs);
            ALUGUICommon.setLabelTxt(wnd.txtCountDown, countDownStr);
            ALUGUICommon.setLabelTxt(wnd.tmpCountDown, countDownStr);
        }
        
        /// <summary>
        /// 点击补充按钮
        /// </summary>
        private void _onClickReplenish(GameObject _go)
        {
            _m_ResidentReplenishView?._onClickReplenish();
        }

        /// <summary>
        /// 模拟点击补充按钮
        /// </summary>
        private void _onSimulateClickReplenish()
        {
            if (wnd == null) return;
            _onClickReplenish(wnd.btnClick);
        }

        #region 倒计时任务

        /// <summary>
        /// 初始化倒计时任务
        /// </summary>
        private void _initCountDownTask()
        {
            _discardCountDownTask();
            
            ALCommonTaskController.CommonEnableDurationActionAddMonoTask(() =>
            {
                if(!isShow)
                    return;
                
                _refreshCountDown();//刷新倒计时显示
                
                if(_m_immigrantInfo == null || _m_immigrantInfo.getEndMs() <= FpsAndPingMgr.instance.serverTimeTag)
                {
                    // 移民信息为空或已到结束时间，销毁任务
                    _discardCountDownTask();

                    // 倒计时结束后, 刷新view显示
                    _m_ResidentReplenishView?.refreshShow();
                    
                    // 刷新下红点
                    NPPlayer.instance.marsComp.redTipDealer.refreshPeopleReplenishRedTip();
                }
            }, 1f);
        }
        
        /// <summary>
        /// 销毁任务
        /// </summary>
        private void _discardCountDownTask()
        {
            _m_countDownTaskController.setDisable();
        }

        #endregion
    }
}
