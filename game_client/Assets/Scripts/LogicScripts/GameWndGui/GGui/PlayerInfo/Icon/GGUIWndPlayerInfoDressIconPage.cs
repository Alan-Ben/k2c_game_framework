using UnityEngine;
using System;
using ALPackage;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 玩家装扮-头像
    /// </summary>
    public class GGUIWndPlayerInfoDressIconPage : _ANPGGUIWndPlayerInfoDressBasePage<GGUIMonoPlayerInfoDressIconPage>
    {
        //头像列表
        private GGUIWndPlayerIconListItemGrid _m_wndListGrid;

        /// <summary>
        /// 是否使用中
        /// </summary>
        protected override bool isUsing { get { return null == showInfo ? false : showInfo.id == NPPlayer.instance.playerInfo.getCurrentIconId(); } }

        public GGUIWndPlayerInfoDressIconPage(long _assetPathInfoId, Transform _parent) : base(_assetPathInfoId,_parent)
        {
        }

        #region override

        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsgAct(WinMsgType.PLAYER_INFO_ICON_CHG, _onIconInfoChg);
            WinMsg.RegisterMsg(WinMsgType.ON_PLAYER_PARAM_CHANGE, _onPlayerParamChanged);
            refreshGrid();
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsgAct(WinMsgType.PLAYER_INFO_ICON_CHG, _onIconInfoChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_PLAYER_PARAM_CHANGE, _onPlayerParamChanged);
            if (null != _m_wndListGrid)
                _m_wndListGrid.hideWnd();
        }

        protected override void _onReset()
        {
            if (null != _m_wndListGrid)
                _m_wndListGrid.resetWnd();
        }

        protected override void _onDiscard()
        {
            base._onDiscard();

            if (null != _m_wndListGrid)
                _m_wndListGrid.discard();
            _m_wndListGrid = null;
        }

        protected override void _onWndInitDone()
        {
            base._onWndInitDone();

            if (null == wnd)
                return;

            if (null != wnd.monoListGrid)
            {
                _m_wndListGrid = new GGUIWndPlayerIconListItemGrid(wnd.monoListGrid);
                _m_wndListGrid.onClickSelect += setItem;
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

            ALUGUICommon.setLabelTxt(wnd.txtSource, TextTranslate.instance.getLanguage(TransKeyConst.common_resource_production_tip, GCommon.getItemSource(ENPItemType.ICON, _info.id)));
        }

        #endregion

        //刷新头像列表
        public void refreshGrid()
        {
            if (null != _m_wndListGrid)
            {
                _m_wndListGrid.showWnd();
                _m_wndListGrid.setShowData();
            }
        }

        //请求设置头像
        protected override void _reqSet(Action _doneAction)
        {
            if (showInfo == null || showInfo.isExpired || showInfo.isLock)
                return;

            //设置头像
            NPPlayer.instance.iconComp.reqSetIcon(showInfo.id, _doneAction);
        }

        #region 消息事件

        //头像信息变更
        private void _onIconInfoChg()
        {
            refreshGrid();
        }

        //玩家参数变化
        private void _onPlayerParamChanged(params object[] _objects)
        {
            if (null == _objects || _objects.Length == 0)
                return;
            
            ENPPlayerParam paramType = (ENPPlayerParam)_objects[0];
            if (paramType == ENPPlayerParam.ICON)
            {
                refreshGrid();
            }
        }

        #endregion
    }
}
