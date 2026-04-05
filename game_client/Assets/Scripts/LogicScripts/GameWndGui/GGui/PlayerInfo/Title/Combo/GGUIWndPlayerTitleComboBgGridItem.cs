using System;
using ALPackage;
using Common.NpPlayerInfoObj;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 组合称号底框
    /// </summary>
    public class GGUIWndPlayerTitleComboBgGridItem : _ATALUGUIBasicGridItemWnd<GGUIMonoPlayerTitleComboBgGridItem>
    {
        //信息
        private PlayerTitleComboBgInfo _m_bgInfo;
        //点击选中
        private Action<GGUIWndPlayerTitleComboBgGridItem> _m_onClickSelect;
        //加载的底框
        private GGUIWndPrefabSubDressItem _m_wbgItem;

        /// <summary>
        /// 信息
        /// </summary>
        public PlayerTitleComboBgInfo bgInfo { get { return _m_bgInfo; } }
        /// <summary>
        /// 点击选中
        /// </summary>
        public Action<GGUIWndPlayerTitleComboBgGridItem> onClickSelect
        {
            get { return _m_onClickSelect; }
            set { _m_onClickSelect = value; }
        }

        public GGUIWndPlayerTitleComboBgGridItem(GGUIMonoPlayerTitleComboBgGridItem _wnd) : base(_wnd)
        {
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wbgItem?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wbgItem?.resetWnd();
        }

        protected override void _resetGridItem()
        {
            _m_wbgItem?.resetWnd();
        }

        protected override void _onDiscard()
        {
            if (wnd == null)
                return;

            _m_wbgItem?.discard();
            _m_wbgItem = null;

            ALUGUICommon.uncombineBtnClick(wnd.btnClick, _onClickItem);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            ALUGUICommon.combineBtnClick(wnd.btnClick, _onClickItem);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        public void setInfo(PlayerTitleComboBgInfo _info)
        {
            if (wnd == null || _info == null || _info.titleBgRef == null)
                return;

            _m_bgInfo = _info;

            //设置解锁状态
            bool isUnlock = _m_bgInfo.isUnlock;
            ALUGUICommon.setGameObjEnable(wnd.goLockShowList, !isUnlock);
            ALUGUICommon.setGameObjEnable(wnd.goLockHideList, isUnlock);

            //加载item
            if (null != wnd.transParent)
            {
                GGUIWndPrefabSubDressItem.checkAndLoadPrefab(_m_wbgItem, _m_bgInfo.titleBgRef.asset_path_id, wnd.transParent, (_prefabItem) =>
                {
                    _m_wbgItem = _prefabItem;
                    _m_wbgItem?.showWnd();
                    _m_wbgItem?.setIcon(ENPItemType.TITLE_BG, _m_bgInfo.titleBgRef.id);
                });
            }

            //当前佩戴状态
            PlayerInfo_ComboTitle curComboTitle = NPPlayer.instance.titleComp.getCurWearComboTitleInfo();
            bool isCurrent = curComboTitle != null && _info.id == curComboTitle.getBgId();
            ALUGUICommon.setGameObjEnable(wnd.goCurrentShowList, isCurrent);
            ALUGUICommon.setGameObjEnable(wnd.goCurrentHideList, !isCurrent);

            //设置红点
            ALUGUICommon.setGameObjEnable(wnd.goNewRedTip, _m_bgInfo.isNew);

        }

        /// <summary>
        /// 设置选中
        /// </summary>
        /// <param name="_isSelect"></param>
        public void setSelect(bool _isSelect)
        {
            if (wnd == null)
                return;

            if (_isSelect)
                _setIsView();

            ALUGUICommon.setGameObjEnable(wnd.goSelectShowList, _isSelect);
            ALUGUICommon.setGameObjEnable(wnd.goSelectHideList, !_isSelect);
        }

        //设置已查看
        private void _setIsView()
        {
            if (_m_bgInfo != null && _m_bgInfo.isNew)
            {
                _m_bgInfo.setIsViewed(true);
                NPPlayer.instance.titleComp.reqViewComboTitleBg(_m_bgInfo.id);
            }
        }

        //点击按钮
        private void _onClickItem(GameObject _go)
        {
            _setIsView();
            _m_onClickSelect?.Invoke(this);
        }
    }
}
