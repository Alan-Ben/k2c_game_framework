using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 火星基地事件详情窗口
    /// </summary>
    public class GGUIWndMarsEventDetail : _ANPGGUIBasicWnd<GGUIMonoMarsEventDetail>
    {
        private static GGUIWndMarsEventDetail _g_instance;
        public static GGUIWndMarsEventDetail instance { get { return _g_instance ??= new GGUIWndMarsEventDetail(); } }
        
        private _IMarsEventInfo _m_iEventInfo;
        private NPPlayerBuffInfo _m_iBuffInfo;//buff数据单独存储，避免频繁获取
        
        private NPGGuiWndTexture _m_wEventBannerTexture;
        private GGUIWndAccessWayContainer _m_wAccessWayContainer;
        
        // 持续事件剩余时间刷新任务（每秒刷新一次）
        private ALCommonEnableTaskController _m_tcLeftTimeRefreshTask;
        
        public GGUIWndMarsEventDetail() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoMarsEventDetail.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoMarsEventDetail.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            // 初始化事件banner纹理
            if (wnd.eventBanner != null)
                _m_wEventBannerTexture = new NPGGuiWndTexture(wnd.eventBanner);

            // 初始化访问途径容器
            if (wnd.accessWayItemContainer != null)
                _m_wAccessWayContainer = new GGUIWndAccessWayContainer(wnd.accessWayItemContainer);
            
            ALUGUICommon.combineBtnClick(wnd.btnClose, _closeBtnClick);
        }

        protected override void _onDiscard()
        {
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnClose, _closeBtnClick);
            }
            
            _m_iEventInfo = null;
            _m_iBuffInfo = null;
            
            // 销毁事件banner纹理
            _m_wEventBannerTexture?.discard();
            _m_wEventBannerTexture = null;
            
            // 销毁访问途径容器
            _m_wAccessWayContainer?.discard();
            _m_wAccessWayContainer = null;

            // 停止倒计时刷新
            _discardLeftTimeRefreshTask();
        }

        protected override void _onShowWnd()
        {
            refreshWnd();
        }

        protected override void _onHideWnd()
        {
            // 隐藏子窗口
            _m_wEventBannerTexture?.hideWnd();
            _m_wAccessWayContainer?.hideWnd();

            // 停止倒计时刷新
            _discardLeftTimeRefreshTask();
        }

        protected override void _onReset()
        {
            // 重置子窗口
            _m_wEventBannerTexture?.discardTexture();
            _m_wAccessWayContainer?.resetWnd();

            // 停止倒计时刷新
            _discardLeftTimeRefreshTask();
        }
        
        /// <summary>
        /// 设置事件数据
        /// </summary>
        /// <param name="_eventInfo">事件信息</param>
        public void setData(_IMarsEventInfo _eventInfo)
        {
            _m_iEventInfo = _eventInfo;
            _m_iBuffInfo = _eventInfo?.buffInfo;
            
            refreshWnd();
        }
        
        /// <summary>
        /// 刷新窗口显示
        /// </summary>
        public void refreshWnd()
        {
            if (wnd == null || !isShow || _m_iEventInfo == null)
                return;

            MarsEventRefObj eventRefObj = _m_iEventInfo.eventRefObj;
            if (eventRefObj == null)
                return;

            // 设置事件banner图片
            if (_m_wEventBannerTexture != null && eventRefObj.banner_img != null)
            {
                _m_wEventBannerTexture.showWnd();
                _m_wEventBannerTexture.setTexture(eventRefObj.banner_img);
            }

            ALUGUICommon.setLabelTxt(wnd.txtName, TextTranslate.instance.getLanguage(eventRefObj.name));
            // 设置事件描述
            ALUGUICommon.setLabelTxt(wnd.txtEventDesc, TextTranslate.instance.getLanguage(eventRefObj.desc));

            // 处理持续事件显示/隐藏逻辑
            bool isDurationEvent = _m_iEventInfo.buffRefObj != null;
            ALUGUICommon.setGameObjEnable(wnd.isDurationEventShowList, isDurationEvent);
            ALUGUICommon.setGameObjEnable(wnd.isDurationEventHideList, !isDurationEvent);

            _initLeftTimeRefreshTask();

            // 设置访问途径列表
            _refreshAccessWayList();
        }

        /// <summary>
        /// 刷新剩余倒计时文本
        /// </summary>
        private void _refreshLeftTimeText()
        {
            if(wnd == null || !isShow)
                return;

            ALUGUICommon.setLabelTxt(wnd.txtEventLeftTime, TimeUtil.millisecondsToTime_hms(_m_iBuffInfo?.curLeftTimeMS ?? 0));
        }
        
        /// <summary>
        /// 刷新访问途径列表
        /// </summary>
        private void _refreshAccessWayList()
        {
            if (_m_wAccessWayContainer == null || _m_iEventInfo?.eventRefObj == null)
                return;

            // 显示访问途径容器并设置数据
            _m_wAccessWayContainer.showWnd();
            _m_wAccessWayContainer.setAccessWayList(_m_iEventInfo.eventRefObj.access_id_list, null);
        }

        #region 倒计时任务

        // 初始化或重置倒计时刷新任务（每秒触发一次）
        private void _initLeftTimeRefreshTask()
        {
            _discardLeftTimeRefreshTask();
            _m_tcLeftTimeRefreshTask = ALCommonTaskController.CommonEnableDurationActionAddMonoTask(() =>
            {
                _refreshLeftTimeText();
                
                if (_m_iBuffInfo == null || _m_iBuffInfo.hasExpired())
                {
                    // 失效则关闭任务
                    _discardLeftTimeRefreshTask();
                }
            }, 1f);
        }

        // 停止倒计时刷新任务
        private void _discardLeftTimeRefreshTask()
        {
            _m_tcLeftTimeRefreshTask.setDisable();
        }

        #endregion
        
        /// <summary>
        /// 关闭窗口
        /// </summary>
        private void _closeBtnClick(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_MARS_EVENT_DETAIL);
        }
    }
}