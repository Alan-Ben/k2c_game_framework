using UnityEngine;
using ALPackage;
using Common.NpPlayerInfoObj;
using NPEnum;
using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 玩家限时称号页面
    /// </summary>
    public class GGUIWndPlayerTitleLimitedPage : _ATALBasicLoadPrefabSubUIWnd<GGUIMonoPlayerTitleLimitedPage>
    {
        private string _m_sAssetPath;//窗口对象的资源 加载路径
        private string _m_sObjName;//窗口对象的资源 名字

        //称号列表
        private GGUIWndPlayerTitleLimitedGrid _m_wTitleGrid;
        //预览的称号
        private GGUIWndSubPlayerTitle _m_wPlayerTitleShow;
        //是否向其他人展示toggle
        private NPGGUIWndCommonToggleEx _m_wShowOthersToggle;
        //任务刷新定时器
        private ALCommonEnableTaskController _m_tcTickTaskController;

        public GGUIWndPlayerTitleLimitedPage(NPCommonAssetPathInfo _assetPathInfo, Transform _parent)
            : base(_parent)
        {
            _m_sAssetPath = _assetPathInfo.asset_path;
            _m_sObjName = _assetPathInfo.obj_name;
        }

        /**************
         * 窗口相关加载配置
         **/
        protected override string _monoAssetPath { get { return _m_sAssetPath; } }
        protected override string _monoObjName { get { return _m_sObjName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onShowWnd()
        {
            //设置开关
            _m_wShowOthersToggle?.showWnd();
            _m_wShowOthersToggle?.setSelected(NPPlayer.instance.titleComp.isShowOthers, true);
            _refreshWnd();

            //开启任务刷新有效期
            _m_tcTickTaskController.setDisable();
            _m_tcTickTaskController = ALCommonTaskController.CommonEnableDurationActionAddMonoTask(_onTick, 1f);
        }
        
        protected override void _onHideWnd()
        {
            _m_wTitleGrid?.hideWnd();
            _m_wShowOthersToggle?.hideWnd();
            _m_wPlayerTitleShow?.hideWnd();
            _m_tcTickTaskController.setDisable();
        }
        
        protected override void _onReset()
        {
            _m_wTitleGrid?.resetWnd();
            _m_wShowOthersToggle?.resetWnd();
            _m_wPlayerTitleShow?.resetWnd();
        }
        
        protected override void _onDiscard()
        {
            _m_wTitleGrid?.discard();
            _m_wTitleGrid = null;
            _m_wShowOthersToggle?.discard();
            _m_wShowOthersToggle = null;
            _m_wPlayerTitleShow?.discard();
            _m_wPlayerTitleShow = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnWear, _onClickWear);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoTitleLimitedGrid != null)
            {
                _m_wTitleGrid = new GGUIWndPlayerTitleLimitedGrid(wnd.monoTitleLimitedGrid);
                _m_wTitleGrid.onClickSelect += _onClickItem;
            }

            if (wnd.toggleShowOthers != null)
            {
                _m_wShowOthersToggle = new NPGGUIWndCommonToggleEx(wnd.toggleShowOthers);
                _m_wShowOthersToggle.clickDelegate += _onClickToggle;
            }

            if (wnd.monoTitleShow != null)
                _m_wPlayerTitleShow = new GGUIWndSubPlayerTitle(wnd.monoTitleShow);

            ALUGUICommon.combineBtnClick(wnd.btnWear, _onClickWear);
        }
        //刷新窗口
        private void _refreshWnd()
        {
            _refreshGrid();
            _refreshCurSelectTitle();
            _refreshBtnState();
        }

        //刷新列表
        private void _refreshGrid()
        {
            if (_m_wTitleGrid != null)
            {
                List<PlayerTitleRefObj> playerTitleRefList = new List<PlayerTitleRefObj>();
                GRefdataCoreMgr.instance.playerTitleRefCore.dealAllRef(_ref =>
                {
                    if (_ref != null && _ref.show_type == EPlayerTitleTabType.LIMITED)
                        playerTitleRefList.Add(_ref);
                });

                _m_wTitleGrid.showWnd();
                _m_wTitleGrid.setShowData(playerTitleRefList);
            }
        }

        //刷新当前选中组合称号
        private void _refreshCurSelectTitle()
        {
            if (wnd == null || _m_wTitleGrid == null || _m_wTitleGrid.curSelectRef == null)
                return;

            if (_m_wPlayerTitleShow != null)
            {
                _m_wPlayerTitleShow.showWnd();
                _m_wPlayerTitleShow.setInfo(_m_wTitleGrid.curSelectRef.id, 1);
            }

            _refreshCurSelectDesc();
        }

        //刷新当前选中描述
        private void _refreshCurSelectDesc()
        {
            if (wnd == null || _m_wTitleGrid == null || _m_wTitleGrid.curSelectRef == null)
                return;

            PlayerTitleInfo titleInfo = NPPlayer.instance.titleComp.getTitleInfo(_m_wTitleGrid.curSelectRef.id);
            ALUGUICommon.setLabelTxt(wnd.txtSource, GCommon.getItemSource(ENPItemType.TITLE, _m_wTitleGrid.curSelectRef.id));
            if (titleInfo == null || titleInfo.isExpired)
                ALUGUICommon.setLabelTxt(wnd.txtCd, _getExpiredTime(_m_wTitleGrid.curSelectRef.expire_time_sec));
            else
            {
                long leftTimeSec = titleInfo.expiredTimeS - FpsAndPingMgr.instance.serverTimeTagS;
                ALUGUICommon.setLabelTxt(wnd.txtCd, _getExpiredTime(leftTimeSec));
            }
        }

        //获取过期时间文本
        private string _getExpiredTime(long _sec)
        {
            string expStr;
            if (_sec > 0)
                expStr = TimeUtil.millisecondsToTime_Two(_sec * 1000);
            else
                expStr = TextTranslate.instance.getLanguage(TransKeyConst.playerDress_alwaysEnable_str);
            return expStr;
        }

        //刷新按钮状态显示
        private void _refreshBtnState()
        {
            EPlayerTitleFixedBtnState btnState = EPlayerTitleFixedBtnState.LOCK;
            if (_m_wTitleGrid != null && _m_wTitleGrid.curSelectRef != null)
            {
                PlayerTitleInfo titleInfo = NPPlayer.instance.titleComp.getTitleInfo(_m_wTitleGrid.curSelectRef.id);
                PlayerInfo_Title curTitle = NPPlayer.instance.titleComp.getCurWearCommonTitleInfo();
                if (titleInfo != null && curTitle != null && titleInfo.refId == curTitle.getId())
                    btnState = EPlayerTitleFixedBtnState.CUR_WEAR;
                else if (titleInfo != null && !titleInfo.isExpired)
                    btnState = EPlayerTitleFixedBtnState.CAN_WEAR;
                else
                    btnState = EPlayerTitleFixedBtnState.LOCK;
            }
            wnd?.setBtnState(btnState);
        }

        //每秒刷新称号到期时间
        private void _onTick()
        {
            //刷新当前选中称号的过期时间
            _refreshCurSelectDesc();
            //刷新按钮状态
            _refreshBtnState();
        }

        #region 点击事件

        //点击item
        private void _onClickItem(GGUIWndPlayerTitleLimitedGridItem _item)
        {
            _refreshCurSelectTitle();
            _refreshBtnState();
        }

        //点击向其他人展示开关
        private void _onClickToggle(NPGGUIWndCommonToggleEx _toggle)
        {
            if (_toggle == null)
                return;

            bool isOn = !_toggle.isOn;
            _toggle.setSelected(isOn);
            NPPlayer.instance.titleComp.reqSetTitleShow(isOn);
        }

        //点击穿戴
        private void _onClickWear(GameObject _go)
        {
            if (wnd == null || _m_wTitleGrid == null || _m_wTitleGrid.curSelectRef == null)
                return;

            PlayerTitleInfo titleInfo = NPPlayer.instance.titleComp.getTitleInfo(_m_wTitleGrid.curSelectRef.id);
            PlayerInfo_Title curTitle = NPPlayer.instance.titleComp.getCurWearCommonTitleInfo();
            if (titleInfo == null || titleInfo.isExpired || (curTitle != null && curTitle.getId() == titleInfo.refId))
                return;

            NPPlayer.instance.titleComp.reqSetCommTitle(titleInfo.refId, _refreshBtnState);
        }

        #endregion
    }
}