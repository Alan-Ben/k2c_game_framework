using UnityEngine;
using ALPackage;
using System;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 玩家装扮-头像框
    /// </summary>
    public class GGUIWndPlayerInfoDressIconBgkPage : _ANPGGUIWndPlayerInfoDressBasePage<GGUIMonoPlayerInfoDressIconBgkPage>
    {
        //头像框容器
        private GGUIWndPlayerIconBgkListItemGrid _m_wndListGrid;
        //头像资源
        private GGUIWndPrefabSubDressItem _m_iconPrefabSubDressItem;
        //头像框资源
        private GGUIWndPrefabSubDressItem _m_iconBgkPrefabSubDressItem;
        //定时刷新任务
        private ALCommonEnableTaskController _m_tRefreshTask;

        /// <summary>
        /// 是否正在使用中
        /// </summary>
        protected override bool isUsing { get { return null == showInfo ? false : showInfo.id == NPPlayer.instance.playerInfo.getCurrentIconBgkId(); } }

        public GGUIWndPlayerInfoDressIconBgkPage(long _assetPathInfoId, Transform _parent) : base(_assetPathInfoId,_parent)
        {
        }

        #region override

        protected override void _onShowWnd()
        {
            refreshGrid();
        }

        protected override void _onHideWnd()
        {
            _m_wndListGrid?.hideWnd();
            _m_iconPrefabSubDressItem?.hideWnd();
            _m_iconBgkPrefabSubDressItem?.hideWnd();
            _m_tRefreshTask.setDisable();
        }

        protected override void _onReset()
        {
            _m_wndListGrid?.resetWnd();
            _m_iconPrefabSubDressItem?.resetWnd();
            _m_iconBgkPrefabSubDressItem?.resetWnd();
        }

        protected override void _onDiscard()
        {
            base._onDiscard();

            _m_tRefreshTask.setDisable();

            _m_wndListGrid?.discard();
            _m_wndListGrid = null;

            _m_iconPrefabSubDressItem?.discard();
            _m_iconPrefabSubDressItem = null;

            _m_iconBgkPrefabSubDressItem?.discard();
            _m_iconBgkPrefabSubDressItem = null;
        }

        protected override void _onWndInitDone()
        {
            if (null == wnd)
                return;

            base._onWndInitDone();

            if (null != wnd.monoListGrid)
            {
                _m_wndListGrid = new GGUIWndPlayerIconBgkListItemGrid(wnd.monoListGrid);
                _m_wndListGrid.onSelectChg += setItem;
            }
        }

        /// <summary>
        /// 设置完数据刷新窗口
        /// </summary>
        /// <param name="_info"></param>
        protected override void _onRefreshWnd(_APlayerBaseShowInfo _info)
        {
            if(wnd == null || _info == null)
                return;

            //设置名称
            ALUGUICommon.setLabelTxt(wnd.txtName, GCommon.getItemName(ENPItemType.ICON_BGK, _info.id));

            //加载头像
            long curIconId = NPPlayer.instance.playerInfo.getCurrentIconId();
            PlayerIconRefObj iconRef = GRefdataCoreMgr.instance.playerIconCore.getRef(curIconId);
            if (iconRef != null && iconRef.asset_path_id > 0)
            {
                GGUIWndPrefabSubDressItem.checkAndLoadPrefab(_m_iconPrefabSubDressItem, iconRef.asset_path_id, wnd.iconPosPar, (_item) =>
                {
                    _m_iconPrefabSubDressItem = _item;
                    _m_iconPrefabSubDressItem?.showWnd();
                    _m_iconPrefabSubDressItem?.setIcon(ENPItemType.ICON, iconRef.id);
                });
            }

            //加载当前选中头像框
            PlayerIconBgkShowInfo iconBgkInfo = _info as PlayerIconBgkShowInfo;
            if (iconBgkInfo == null)
                return;
            if (iconBgkInfo.refObj != null && iconBgkInfo.refObj.asset_path_id > 0)
            {
                GGUIWndPrefabSubDressItem.checkAndLoadPrefab(_m_iconBgkPrefabSubDressItem, iconBgkInfo.refObj.asset_path_id, wnd.iconBgkPosPar, (_item) =>
                {
                    _m_iconBgkPrefabSubDressItem = _item;
                    _m_iconBgkPrefabSubDressItem?.showWnd();
                    _m_iconBgkPrefabSubDressItem?.setIcon(ENPItemType.ICON_BGK, iconBgkInfo.refObj.id);
                });
            }

            //设置获取途径、有效期
            if (iconBgkInfo.iconBgkItem != null && !iconBgkInfo.iconBgkItem.isExpired)
            {
                //已拥有
                if (iconBgkInfo.iconBgkItem.expiredTimeS > 0)
                {
                    //有过期时间
                    //开启定时器
                    _m_tRefreshTask.setDisable();
                    _m_tRefreshTask = ALCommonTaskController.CommonEnableDurationActionAddMonoTask(_refreshLeftTime, 1);
                }
                else
                {
                    //没有过期时间，显示永久获得
                    ALUGUICommon.setLabelTxt(wnd.txtSource, TextTranslate.instance.getLanguage(TransKeyConst.getItem_expiredTime_time,TransKeyConst.playerDress_alwaysEnable_str));
                }
            }
            else
            {
                //未拥有，显示来源
                _m_tRefreshTask.setDisable();
                ALUGUICommon.setLabelTxt(wnd.txtSource, GCommon.getItemSource(ENPItemType.ICON_BGK, _info.id));
            }
        }

        #endregion

        //刷新列表
        public void refreshGrid()
        {
            if (null != _m_wndListGrid)
            {
                _m_wndListGrid.showWnd();
                _m_wndListGrid.refresh();
            }
        }

        //刷新倒计时
        private void _refreshLeftTime()
        {
            if (null == wnd || null == _m_showInfo)
                return;

            long leftTime = _m_showInfo.expiredTimeS - FpsAndPingMgr.instance.serverTimeTagS;
            ALUGUICommon.setLabelTxt(wnd.txtSource, TextTranslate.instance.getLanguage(TransKeyConst.getItem_expiredTime_time, TimeUtil.millisecondsToTime_Two(leftTime * 1000)));//倒计时
            if (leftTime <= 0)
            {
                refreshGrid();
                _m_tRefreshTask.setDisable();
            }
        }

        //请求设置头像框
        protected override void _reqSet(Action _doneAction)
        {
            if (showInfo == null || showInfo.isExpired || showInfo.isLock)
                return;

            //设置头像框
            NPPlayer.instance.iconBgkComp.reqSetIconBgk(showInfo.id, _doneAction);
        }
    }
}
