using UnityEngine;
using ALPackage;
using System;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 玩家装扮-气泡框
    /// </summary>
    public class GGUIWndPlayerInfoDressBubblePage : _ANPGGUIWndPlayerInfoDressBasePage<GGUIMonoPlayerInfoDressBubblePage>
    {
        //容器
        private GGUIWndPlayerBubbleListItemGrid _m_wndListGrid;
        //气泡框资源
        private GGUIWndPrefabSubDressItem _m_bubblePrefabSubDressItem;
        //定时刷新任务
        private ALCommonEnableTaskController _m_tRefreshTask;

        /// <summary>
        /// 是否正在使用中
        /// </summary>
        protected override bool isUsing { get { return null == showInfo ? false : showInfo.id == NPPlayer.instance.playerInfo.getCurrentBubbleId(); } }

        public GGUIWndPlayerInfoDressBubblePage(long _assetPathInfoId, Transform _parent)
             : base(_assetPathInfoId,_parent)
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
            _m_bubblePrefabSubDressItem?.hideWnd();
            _m_tRefreshTask.setDisable();
        }

        protected override void _onReset()
        {
            _m_wndListGrid?.resetWnd();
            _m_bubblePrefabSubDressItem?.resetWnd();
        }

        protected override void _onDiscard()
        {
            base._onDiscard();

            _m_tRefreshTask.setDisable();

            _m_wndListGrid?.discard();
            _m_wndListGrid = null;

            _m_bubblePrefabSubDressItem?.discard();
            _m_bubblePrefabSubDressItem = null;
        }

        protected override void _onWndInitDone()
        {
            base._onWndInitDone();
            if (null == wnd)
                return;

            if (null != wnd.monoListGrid)
            {
                _m_wndListGrid = new GGUIWndPlayerBubbleListItemGrid(wnd.monoListGrid);
                _m_wndListGrid.onSelectChg += setItem;
            }
        }

        /// <summary>
        /// 设置完数据刷新窗口
        /// </summary>
        /// <param name="_info"></param>
        protected override void _onRefreshWnd(_APlayerBaseShowInfo _info)
        {
            if (wnd == null || _info == null)
                return;

            //设置名称
            ALUGUICommon.setLabelTxt(wnd.txtName, GCommon.getItemName(ENPItemType.BUBBLE, _info.id));

            //加载气泡框
            NPPlayerBubbleShowInfo bubbleShowInfo = (NPPlayerBubbleShowInfo)_info;
            if (null != wnd.bubblePosPar && bubbleShowInfo.refObj != null)
            {
                GGUIWndPrefabSubDressItem.checkAndLoadPrefab(_m_bubblePrefabSubDressItem, bubbleShowInfo.refObj.asset_path_id, wnd.bubblePosPar, (_item) =>
                {
                    _m_bubblePrefabSubDressItem = _item;
                    _m_bubblePrefabSubDressItem?.showWnd();
                    _m_bubblePrefabSubDressItem?.setIcon(ENPItemType.BUBBLE, bubbleShowInfo.refObj.id);
                    _m_bubblePrefabSubDressItem?.setSptIcon(bubbleShowInfo.refObj.spt_icon);
                });
            }

            //设置获取途径、有效期
            if (bubbleShowInfo.bubbleItem != null && !bubbleShowInfo.bubbleItem.isExpired)
            {
                //已拥有
                if (bubbleShowInfo.bubbleItem.expiredTimeS > 0)
                {
                    //有过期时间
                    //开启定时器
                    _m_tRefreshTask.setDisable();
                    _m_tRefreshTask = ALCommonTaskController.CommonEnableDurationActionAddMonoTask(_refreshLeftTime, 1);
                }
                else
                {
                    //没有过期时间，显示永久获得
                    ALUGUICommon.setLabelTxt(wnd.txtSource, TextTranslate.instance.getLanguage(TransKeyConst.getItem_expiredTime_time, TransKeyConst.playerDress_alwaysEnable_str));
                }
            }
            else
            {
                //未拥有，显示来源
                _m_tRefreshTask.setDisable();
                ALUGUICommon.setLabelTxt(wnd.txtSource, GCommon.getItemSource(ENPItemType.BUBBLE, _info.id));
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

        //设置气泡框
        protected override void _reqSet(Action _doneAction)
        {
            //设置气泡框
            NPPlayer.instance.bubbleComp.reqSetBubble(showInfo.id, _doneAction);
        }
    }
}
