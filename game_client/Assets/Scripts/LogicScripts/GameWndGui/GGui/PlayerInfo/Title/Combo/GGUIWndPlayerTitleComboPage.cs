using UnityEngine;
using ALPackage;
using System.Collections.Generic;
using Common.NpPlayerInfoObj;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 玩家组合称号页面
    /// </summary>
    public class GGUIWndPlayerTitleComboPage : _ATALBasicLoadPrefabSubUIWnd<GGUIMonoPlayerTitleComboPage>
    {
        private string _m_sAssetPath;//窗口对象的资源 加载路径
        private string _m_sObjName;//窗口对象的资源 名字

        //页签列表
        private List<GGUIWndPlayerTitleComboPageTab> _m_lTabWndList;
        //当前选中的页签
        private GGUIWndPlayerTitleComboPageTab _m_wSelectTabWnd;
        //前缀列表
        private GGUIWndPlayerTitleComboTextGrid _m_wPrefixGrid;
        //后缀页面
        private GGUIWndPlayerTitleComboTextGrid _m_wSuffixGrid;
        //底框页面
        private GGUIWndPlayerTitleComboBgGrid _m_wBgGrid;
        //预览的组合称号
        private GGUIWndSubPlayerTitle _m_wPlayerTitleShow;
        //当前选中的组合称号
        private PlayerInfo_ComboTitle _m_curSelectComboTitle;
        //是否向其他人展示toggle
        private NPGGUIWndCommonToggleEx _m_wShowOthersToggle;
        //底框信息列表
        private List<PlayerTitleComboBgInfo> _m_lBgInfoList;
        //前缀列表
        private List<_IPlayerTitleCombo> _m_lPrefixList;
        //后缀列表
        private List<_IPlayerTitleCombo> _m_lSuffixList;

        public GGUIWndPlayerTitleComboPage(NPCommonAssetPathInfo _assetPathInfo, Transform _parent)
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
            //获取当前穿戴的组合称号
            PlayerInfo_ComboTitle curComboTitle = NPPlayer.instance.titleComp.getCurWearComboTitleInfo();
            if (curComboTitle != null)
                _m_curSelectComboTitle = new PlayerInfo_ComboTitle(curComboTitle.getPreId(), curComboTitle.getSfxId(), curComboTitle.getBgId());
            //设置开关
            _m_wShowOthersToggle?.showWnd();
            _m_wShowOthersToggle?.setSelected(NPPlayer.instance.titleComp.isShowOthers, true);
            //默认选中前缀页面
            _refreshPageWnd(EPlayerTitleComboTabType.PREFIX);
            //默认展示选中预览
            _refreshCurSelectTitle();
            //刷新按钮状态
            _refreshBtnState();
        }
        
        protected override void _onHideWnd()
        {
            _hideAllPage();
            _m_wPlayerTitleShow?.hideWnd();
            _m_wShowOthersToggle?.hideWnd();
        }
        
        protected override void _onReset()
        {
            //页签
            if (_m_lTabWndList != null)
            {
                GGUIWndPlayerTitleComboPageTab tempTabItem = null;
                for (int i = 0; i < _m_lTabWndList.Count; ++i)
                {
                    tempTabItem = _m_lTabWndList[i];
                    if (tempTabItem == null)
                        continue;
                    //重置状态
                    tempTabItem.resetWnd();
                }
            }

            _m_wSelectTabWnd = null;

            _m_wPrefixGrid?.resetWnd();
            _m_wSuffixGrid?.resetWnd();
            _m_wBgGrid?.resetWnd();
            _m_wPlayerTitleShow?.resetWnd();
            _m_wShowOthersToggle?.resetWnd();
        }
        
        protected override void _onDiscard()
        {
            if (_m_lTabWndList != null)
            {
                GGUIWndPlayerTitleComboPageTab tempTabItem = null;
                for (int i = 0; i < _m_lTabWndList.Count; ++i)
                {
                    tempTabItem = _m_lTabWndList[i];
                    if (tempTabItem == null)
                        continue;
                    tempTabItem.discard();
                }
                _m_lTabWndList.Clear();
                _m_lTabWndList = null;
            }

            _m_wSelectTabWnd = null;

            _m_wPrefixGrid?.discard();
            _m_wPrefixGrid = null;

            _m_wSuffixGrid?.discard();
            _m_wSuffixGrid = null;

            _m_wBgGrid?.discard();
            _m_wBgGrid = null;

            _m_wPlayerTitleShow?.discard();
            _m_wPlayerTitleShow = null;

            _m_wShowOthersToggle?.discard();
            _m_wShowOthersToggle = null;

            _m_lBgInfoList?.Clear();
            _m_lBgInfoList = null;

            _m_lPrefixList?.Clear();
            _m_lPrefixList = null;

            _m_lSuffixList?.Clear();
            _m_lSuffixList = null;

            ALUGUICommon.uncombineBtnClick(wnd.btnWear, _onClickWear);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            _m_lTabWndList = new List<GGUIWndPlayerTitleComboPageTab>();
            if (null != wnd.monoTabList)
            {
                GGUIPlayerTitleComboTabMono tempTabMono = null;
                GGUIWndPlayerTitleComboPageTab tempTabItem = null;
                for (int i = 0; i < wnd.monoTabList.Count; ++i)
                {
                    tempTabMono = wnd.monoTabList[i];
                    if (tempTabMono == null || tempTabMono.monoTab == null)
                        continue;
                    tempTabItem = new GGUIWndPlayerTitleComboPageTab(tempTabMono.monoTab, tempTabMono.tabType);
                    //初始化默认未选中
                    tempTabItem.setSelected(false);
                    //绑定点击事件
                    tempTabItem.onClickTab += _onTabSelect;
                    _m_lTabWndList.Add(tempTabItem);
                }
            }

            if (wnd.monoPrefixGrid != null)
            {
                _m_wPrefixGrid = new GGUIWndPlayerTitleComboTextGrid(wnd.monoPrefixGrid);
                _m_wPrefixGrid.onClickSelect += _onClickPrefixItem;
            }

            if (wnd.monoSuffixGrid != null)
            {
                _m_wSuffixGrid = new GGUIWndPlayerTitleComboTextGrid(wnd.monoSuffixGrid);
                _m_wSuffixGrid.onClickSelect += _onClickSuffixItem;
            }

            if (wnd.monoBgGrid != null)
            {
                _m_wBgGrid = new GGUIWndPlayerTitleComboBgGrid(wnd.monoBgGrid);
                _m_wBgGrid.onClickSelect += _onClickBgItem;
            }

            if (wnd.monoTitleShow != null)
                _m_wPlayerTitleShow = new GGUIWndSubPlayerTitle(wnd.monoTitleShow);

            if (wnd.toggleShowOthers != null)
            {
                _m_wShowOthersToggle = new NPGGUIWndCommonToggleEx(wnd.toggleShowOthers);
                _m_wShowOthersToggle.clickDelegate += _onClickToggle;
            }

            _m_lBgInfoList = new List<PlayerTitleComboBgInfo>();
            NPPlayer.instance.titleComp.getComboBgInfoList(_m_lBgInfoList);

            _m_lPrefixList = new List<_IPlayerTitleCombo>();
            NPPlayer.instance.titleComp.getComboPreInfoList(_m_lPrefixList);

            _m_lSuffixList = new List<_IPlayerTitleCombo>();
            NPPlayer.instance.titleComp.getComboSfxInfoList(_m_lSuffixList);

            ALUGUICommon.combineBtnClick(wnd.btnWear, _onClickWear);
        }

        /// <summary>
        /// 刷新页签窗口
        /// </summary>
        private void _refreshPageWnd(EPlayerTitleComboTabType _selectTabType)
        {
            if (null == wnd)
                return;

            //设置选中页签
            if (_m_wSelectTabWnd == null)
            {
                foreach (GGUIWndPlayerTitleComboPageTab itemTab in _m_lTabWndList)
                {
                    if (itemTab.tabType == _selectTabType)
                    {
                        _onTabSelect(itemTab);
                        break;
                    }
                }
            }
            else
            {
                _m_wSelectTabWnd.setSelected(true);
                //根据页签刷新列表内容
                _refreshTabView(_m_wSelectTabWnd.tabType);
            }
        }

        //刷新当前选中组合称号
        private void _refreshCurSelectTitle()
        {
            if (wnd == null)
                return;

            if (_m_wPlayerTitleShow != null)
            {
                _m_wPlayerTitleShow.showWnd();
                _m_wPlayerTitleShow.setInfo(_m_curSelectComboTitle);
            }

            if (_m_curSelectComboTitle == null)
            {
                ALUGUICommon.setLabelTxt(wnd.txtPreSource, "");
                ALUGUICommon.setLabelTxt(wnd.txtSfxSource, "");
                ALUGUICommon.setLabelTxt(wnd.txtBgSource, "");
                return;
            }

            if(_m_curSelectComboTitle.getPreId() > 0)
                ALUGUICommon.setLabelTxt(wnd.txtPreSource, TextTranslate.instance.getLanguage(TransKeyConst.common_str_colon_str, 
                    GCommon.getItemName(ENPItemType.TITLE_PRE, _m_curSelectComboTitle.getPreId()),
                    GCommon.getItemSource(ENPItemType.TITLE_PRE, _m_curSelectComboTitle.getPreId())));
            else
                ALUGUICommon.setLabelTxt(wnd.txtPreSource, "");

            if (_m_curSelectComboTitle.getSfxId() > 0)
                ALUGUICommon.setLabelTxt(wnd.txtSfxSource, TextTranslate.instance.getLanguage(TransKeyConst.common_str_colon_str,
                    GCommon.getItemName(ENPItemType.TITLE_SFX, _m_curSelectComboTitle.getSfxId()),
                    GCommon.getItemSource(ENPItemType.TITLE_SFX, _m_curSelectComboTitle.getSfxId())));
            else
                ALUGUICommon.setLabelTxt(wnd.txtSfxSource, "");

            if (_m_curSelectComboTitle.getBgId() > 0)
                ALUGUICommon.setLabelTxt(wnd.txtBgSource, TextTranslate.instance.getLanguage(TransKeyConst.common_str_colon_str,
                    GCommon.getItemName(ENPItemType.TITLE_BG, _m_curSelectComboTitle.getBgId()),
                    GCommon.getItemSource(ENPItemType.TITLE_BG, _m_curSelectComboTitle.getBgId())));
            else
                ALUGUICommon.setLabelTxt(wnd.txtBgSource, "");
        }

        //刷新按钮状态显示
        private void _refreshBtnState()
        {
            EPlayerTitleComboBtnState btnState = EPlayerTitleComboBtnState.NO_SELECT;
            if (_m_curSelectComboTitle != null)
            {
                PlayerInfo_ComboTitle curComboTitle = NPPlayer.instance.titleComp.getCurWearComboTitleInfo();
                if (curComboTitle != null &&
                    _m_curSelectComboTitle.getPreId() == curComboTitle.getPreId() &&
                    _m_curSelectComboTitle.getSfxId() == curComboTitle.getSfxId() &&
                    _m_curSelectComboTitle.getBgId() == curComboTitle.getBgId())
                    btnState = EPlayerTitleComboBtnState.CUR_WEAR;
                else if(_m_curSelectComboTitle.getPreId() > 0 || _m_curSelectComboTitle.getSfxId() > 0)
                    btnState = EPlayerTitleComboBtnState.CAN_WEAR;
            }
            wnd?.setBtnState(btnState);
        }

        //刷新当前页面的组合称号列表
        private void _refreshCurGridList()
        {
            if (_m_wSelectTabWnd == null)
                return;

            switch (_m_wSelectTabWnd.tabType)
            {
                case EPlayerTitleComboTabType.PREFIX:
                    _m_wPrefixGrid?.forceRefreshAllItem();
                    break;
                case EPlayerTitleComboTabType.SUFFIX:
                    _m_wSuffixGrid?.forceRefreshAllItem();
                    break;
                case EPlayerTitleComboTabType.BG:
                    _m_wBgGrid?.forceRefreshAllItem();
                    break;
            }
        }

        #region 页签页面处理

        //点击切换页签按钮
        private void _onTabSelect(GGUIWndPlayerTitleComboPageTab _tabItemWnd)
        {
            if (null == _tabItemWnd || _m_wSelectTabWnd == _tabItemWnd)
                return;

            //取消原来的选择
            if (null != _m_wSelectTabWnd)
                _m_wSelectTabWnd.setSelected(false);

            //设置新对象
            _m_wSelectTabWnd = _tabItemWnd;

            if (null != _m_wSelectTabWnd)
            {
                _m_wSelectTabWnd.setSelected(true);

                //根据页签刷新列表内容
                _refreshTabView(_m_wSelectTabWnd.tabType);
            }
        }

        //根据页签刷新列表内容
        private void _refreshTabView(EPlayerTitleComboTabType _tabView)
        {
            _hideAllPage();

            switch (_tabView)
            {
                case EPlayerTitleComboTabType.PREFIX://前缀
                    _showPrefixPage();
                    break;
                case EPlayerTitleComboTabType.SUFFIX://后缀
                    _showSuffixPage();
                    break;
                case EPlayerTitleComboTabType.BG://底框
                    _showBgPage();
                    break;
            }
        }

        //关闭所有页面
        private void _hideAllPage()
        {
            _m_wPrefixGrid?.hideWnd();
            _m_wSuffixGrid?.hideWnd();
            _m_wBgGrid?.hideWnd();
        }

        //显示前缀页面
        private void _showPrefixPage()
        {
            if (wnd == null)
                return;

            if (_m_wPrefixGrid != null)
            {
                _m_wPrefixGrid.showWnd();
                _m_wPrefixGrid.setShowData(ENPItemType.TITLE_PRE, _m_lPrefixList);
                _m_wPrefixGrid.setSelect(_m_curSelectComboTitle != null ? _m_curSelectComboTitle.getPreId():0);
            }
        }

        //显示后缀页面
        private void _showSuffixPage()
        {
            if (wnd == null)
                return;

            if (_m_wSuffixGrid != null)
            {
                _m_wSuffixGrid.showWnd();
                _m_wSuffixGrid.setShowData(ENPItemType.TITLE_SFX, _m_lSuffixList);
                _m_wSuffixGrid.setSelect(_m_curSelectComboTitle != null ? _m_curSelectComboTitle.getSfxId() : 0);
            }
        }

        //显示底框页面
        private void _showBgPage()
        {
            if (wnd == null)
                return;

            if (_m_wBgGrid != null)
            {
                _m_wBgGrid.showWnd();
                _m_wBgGrid.setShowData(_m_lBgInfoList);
                _m_wBgGrid.setSelect(_m_curSelectComboTitle != null ? _m_curSelectComboTitle.getBgId() : 0);
            }
        }

        #endregion

        #region 点击事件

        //点击前缀item事件
        private void _onClickPrefixItem(GGUIWndPlayerTitleComboTextGridItem _item)
        {
            if(wnd == null || _item == null)
                return;

            //点击同一个不处理
            if (_m_curSelectComboTitle != null && _m_curSelectComboTitle.getPreId() == _item.id)
                return;

            PlayerTitleComboPreInfo preInfo = NPPlayer.instance.titleComp.getTitleComboPreInfo(_item.id);
            if (preInfo == null)
                return;

            //未解锁弹出详情
            if (!preInfo.isUnlock)
            {
                QueueMgr.instance.AddNode(new GNodePlayerTitleComboItemDetailToolTip(
                    ENPItemType.TITLE_PRE,
                    _item.id,
                    _item.rectTransform,
                    wnd.unlockDetailToolTipInterval.x,
                    wnd.unlockDetailToolTipInterval.y));
                return;
            }

            //已解锁，设置当前选中组合称号
            if (_m_curSelectComboTitle == null)
                _m_curSelectComboTitle = new PlayerInfo_ComboTitle();
            _m_curSelectComboTitle.setPreId(_item.id);

            //刷新当前组合称号预览
            _refreshCurSelectTitle();
            //刷新按钮状态
            _refreshBtnState();
        }

        //点击后缀item事件
        private void _onClickSuffixItem(GGUIWndPlayerTitleComboTextGridItem _item)
        {
            if (wnd == null || _item == null)
                return;

            //点击同一个不处理
            if (_m_curSelectComboTitle != null && _m_curSelectComboTitle.getSfxId() == _item.id)
                return;

            PlayerTitleComboSfxInfo sfxInfo = NPPlayer.instance.titleComp.getTitleComboSfxInfo(_item.id);
            if (sfxInfo == null)
                return;

            //未解锁弹出详情
            if (!sfxInfo.isUnlock)
            {
                QueueMgr.instance.AddNode(new GNodePlayerTitleComboItemDetailToolTip(
                    ENPItemType.TITLE_SFX,
                    _item.id,
                    _item.rectTransform,
                    wnd.unlockDetailToolTipInterval.x,
                    wnd.unlockDetailToolTipInterval.y));
                return;
            }

            //已解锁，设置当前选中组合称号
            if (_m_curSelectComboTitle == null)
                _m_curSelectComboTitle = new PlayerInfo_ComboTitle();
            _m_curSelectComboTitle.setSfxId(_item.id);

            //刷新当前组合称号预览
            _refreshCurSelectTitle();
            //刷新按钮状态
            _refreshBtnState();
        }

        //点击底框item事件
        private void _onClickBgItem(GGUIWndPlayerTitleComboBgGridItem _item)
        {
            if (wnd == null || _item == null || _item.bgInfo == null)
                return;

            //点击同一个不处理
            if (_m_curSelectComboTitle != null && _m_curSelectComboTitle.getBgId() == _item.bgInfo.id)
                return;

            PlayerTitleComboBgInfo bgInfo = _item.bgInfo;

            //未解锁弹出详情
            if (!bgInfo.isUnlock)
            {
                QueueMgr.instance.AddNode(new GNodePlayerTitleComboItemDetailToolTip(
                    ENPItemType.TITLE_BG,
                    bgInfo.id,
                    _item.rectTransform,
                    wnd.unlockDetailToolTipInterval.x,
                    wnd.unlockDetailToolTipInterval.y));
                return;
            }

            //已解锁，设置当前选中组合称号
            if (_m_curSelectComboTitle == null)
                _m_curSelectComboTitle = new PlayerInfo_ComboTitle();
            _m_curSelectComboTitle.setBgId(bgInfo.id);

            //刷新当前组合称号预览
            _refreshCurSelectTitle();
            //刷新按钮状态
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

        //点击穿戴按钮
        private void _onClickWear(GameObject _go)
        {
            if (_m_curSelectComboTitle == null || (_m_curSelectComboTitle.getPreId() <= 0 && _m_curSelectComboTitle.getSfxId() <= 0 && _m_curSelectComboTitle.getBgId() <= 0))
                return;

            NPPlayer.instance.titleComp.reqSetComboTitle(_m_curSelectComboTitle.getPreId(), _m_curSelectComboTitle.getSfxId(), _m_curSelectComboTitle.getBgId(),
                () =>
                {
                    _refreshBtnState();
                    _refreshCurGridList();
                });
        }

        #endregion
    }
}