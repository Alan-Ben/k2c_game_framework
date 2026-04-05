using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using Common.NpPlayerInfoObj;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 玩家固定称号页面
    /// </summary>
    public class GGUIWndPlayerTitleFixedPage : _ATALBasicLoadPrefabSubUIWnd<GGUIMonoPlayerTitleFixedPage>
    {
        private string _m_sAssetPath;//窗口对象的资源 加载路径
        private string _m_sObjName;//窗口对象的资源 名字

        //称号列表
        private GGUIWndPlayerTitleFixedGrid _m_wTitleGrid;
        //预览的称号
        private GGUIWndSubPlayerTitle _m_wPlayerTitleShow;
        //是否向其他人展示toggle
        private NPGGUIWndCommonToggleEx _m_wShowOthersToggle;

        public GGUIWndPlayerTitleFixedPage(NPCommonAssetPathInfo _assetPathInfo, Transform _parent)
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
        }
        
        protected override void _onHideWnd()
        {
            _m_wTitleGrid?.hideWnd();
            _m_wShowOthersToggle?.hideWnd();
            _m_wPlayerTitleShow?.hideWnd();
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

            if (wnd.monoGrid != null)
            {
                _m_wTitleGrid = new GGUIWndPlayerTitleFixedGrid(wnd.monoGrid);
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
                    if(_ref != null && _ref.show_type == EPlayerTitleTabType.FIXED)
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

            PlayerTitleInfo titleInfo = NPPlayer.instance.titleComp.getTitleInfo(_m_wTitleGrid.curSelectRef.id);
            if (titleInfo == null || titleInfo.isExpired)
            {
                ALUGUICommon.setLabelTxt(wnd.txtDesc, GCommon.getItemSource(ENPItemType.TITLE, _m_wTitleGrid.curSelectRef.id));
                ALUGUICommon.setLabelTxt(wnd.txtGainTime, "");
            }
            else
            {
                ALUGUICommon.setLabelTxt(wnd.txtDesc, GCommon.getItemDesc(ENPItemType.TITLE, _m_wTitleGrid.curSelectRef.id));
                ALUGUICommon.setLabelTxt(wnd.txtGainTime, TextTranslate.instance.getLanguage(TransKeyConst.playerInfo_gainTime_str,TimeUtil.DateTime2StringMDYHMS(TimeUtil.FromUTCByTimeZone(titleInfo.lastGainTimeS*1000))));
            }
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

        #region 点击事件

        //点击item
        private void _onClickItem(GGUIWndPlayerTitleFixedGridItem _item)
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