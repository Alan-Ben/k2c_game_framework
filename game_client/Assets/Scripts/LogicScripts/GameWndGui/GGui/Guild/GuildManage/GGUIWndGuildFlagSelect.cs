using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 联盟创建界面
    /// </summary>
    public class GGUIWndGuildFlagSelect : _ANPGGUIBasicWnd<GGUIMonoGuildFlagSelect>
    {
        private static GGUIWndGuildFlagSelect _g_instance = new GGUIWndGuildFlagSelect();

        public static GGUIWndGuildFlagSelect instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndGuildFlagSelect();
                return _g_instance;
            }
        }

        //原本选中的旗帜数据
        private GuildFlagRefObj _m_oriSelectGuildFlagRef;
        //当前选中的旗帜数据
        private GuildFlagRefObj _m_curSelectGuildFlagRef;
        //选中旗帜确认回调
        private Action<GuildFlagRefObj> _m_aSelectConfirm;
        //是否需要展示消耗
        private bool _m_bNeedShowCost;
        //旗帜列表
        private GGUIWndGuildFlagSelectGrid _m_wFlagGrid;
        //当前选中的旗帜图标
        private NPGGuiWndTexture _m_wFlagIcon;
        //更换旗帜消耗的道具
        private NPGGUIWndCommonItem _m_wCostItem;

        public GGUIWndGuildFlagSelect() : base(EALUIWndLayer.ADDITION)
        {

        }

        protected override string _monoAssetPath { get { return GGUIMonoGuildFlagSelect.assetPath; } }

        protected override string _monoObjName { get { return GGUIMonoGuildFlagSelect.objName; } }

        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wFlagGrid?.hideWnd();
            _m_wFlagIcon?.hideWnd();
            _m_wCostItem?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wFlagGrid?.resetWnd();
            _m_wFlagIcon?.discardTexture();
            _m_wCostItem?.resetWnd();
        }

        protected override void _onDiscard()
        {
            if (wnd == null)
                return;

            _m_wFlagGrid?.discard();
            _m_wFlagGrid = null;

            _m_wFlagIcon?.discard();
            _m_wFlagIcon = null;

            _m_wCostItem?.discard();
            _m_wCostItem = null;

            ALUGUICommon.uncombineBtnClick(wnd.btnChg, _onClickChg);
            ALUGUICommon.uncombineBtnClick(wnd.btnConfirm, _onClickConfirm);
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onCloseBtnClick);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoFlagGrid != null)
            {
                _m_wFlagGrid = new GGUIWndGuildFlagSelectGrid(wnd.monoFlagGrid);
                _m_wFlagGrid.onFlagSelect += _onFlagSelectChg;
            }

            if (wnd.imgIcon != null)
                _m_wFlagIcon = new NPGGuiWndTexture(wnd.imgIcon);

            if (wnd.monoChgCost != null)
                _m_wCostItem = new NPGGUIWndCommonItem(wnd.monoChgCost);

            ALUGUICommon.combineBtnClick(wnd.btnChg, _onClickChg);
            ALUGUICommon.combineBtnClick(wnd.btnConfirm, _onClickConfirm);
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onCloseBtnClick);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_curSelectFlagRef"></param>
        /// <param name="_needShowCost"></param>
        /// <param name="_onSelectConfirm"></param>
        public void setInfo(GuildFlagRefObj _curSelectFlagRef, bool _needShowCost, Action<GuildFlagRefObj> _onSelectConfirm)
        {
            _m_oriSelectGuildFlagRef = _curSelectFlagRef;
            _m_curSelectGuildFlagRef = _curSelectFlagRef;
            _m_bNeedShowCost = _needShowCost;
            _m_aSelectConfirm = _onSelectConfirm;

            _refreshWnd();
        }

        //刷新窗口
        private void _refreshWnd()
        {
            _refreshFlagIcon();
            _refreshFlagList();
            _refreshBtnState();
        }

        //刷新旗帜图标
        private void _refreshFlagIcon()
        {
            if (wnd == null || _m_curSelectGuildFlagRef == null)
                return;

            if (_m_wFlagIcon != null)
            {
                _m_wFlagIcon.showWnd();
                _m_wFlagIcon.setTexture(_m_curSelectGuildFlagRef.icon);
            }
        }

        //刷新旗帜列表
        private void _refreshFlagList()
        {
            if (_m_wFlagGrid != null)
            {
                _m_wFlagGrid.showWnd();
                _m_wFlagGrid.setShowData();

                ALCommonActionMonoTask.addNextFrameTask(() =>
                {
                    _m_wFlagGrid.setDefaultSelect(_m_oriSelectGuildFlagRef);
                });
            }
        }

        //刷新按钮状态
        private void _refreshBtnState()
        {
            if (wnd == null)
                return;

            //如果不需要展示消耗 或者 是当前选中的旗帜，则都是显示确认按钮
            if (!_m_bNeedShowCost || (_m_oriSelectGuildFlagRef != null && _m_curSelectGuildFlagRef != null && _m_oriSelectGuildFlagRef.id == _m_curSelectGuildFlagRef.id))
            {
                ALUGUICommon.setGameObjEnable(wnd.goSelectCurShowList, true);
                ALUGUICommon.setGameObjEnable(wnd.goSelectCurHideList, false);
                _m_wCostItem?.hideWnd();
            }
            else
            {
                //显示消耗
                if (_m_wCostItem != null)
                {
                    _m_wCostItem.showWnd();
                    _m_wCostItem.setItem(GRefdataCoreMgr.instance.npGeneral.guild_change_flag_cost);
                }

                //是否是之前的选择
                bool isCurrent = _m_oriSelectGuildFlagRef != null && 
                                 _m_curSelectGuildFlagRef != null &&
                                 _m_oriSelectGuildFlagRef.id == _m_curSelectGuildFlagRef.id;

                ALUGUICommon.setGameObjEnable(wnd.goSelectCurShowList, isCurrent);
                ALUGUICommon.setGameObjEnable(wnd.goSelectCurHideList, !isCurrent);
            }
        }

        //旗帜选择变更
        private void _onFlagSelectChg(GuildFlagRefObj _refObj)
        {
            _m_curSelectGuildFlagRef = _refObj;
            _refreshFlagIcon();
            _refreshBtnState();
        }

        //点击确认
        private void _onClickConfirm(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst_Guild.C_GUILD_SELECT_FLAG);
            _m_aSelectConfirm?.Invoke(_m_curSelectGuildFlagRef);
        }

        //点击修改
        private void _onClickChg(GameObject _go)
        {
            if (!GCommon.isItemEnough(GRefdataCoreMgr.instance.npGeneral.guild_change_flag_cost, true))
                return;

            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst_Guild.C_GUILD_SELECT_FLAG);
            _m_aSelectConfirm?.Invoke(_m_curSelectGuildFlagRef);
        }

        /// <summary>
        /// 点击关闭按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onCloseBtnClick(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst_Guild.C_GUILD_SELECT_FLAG);
        }
    }
}