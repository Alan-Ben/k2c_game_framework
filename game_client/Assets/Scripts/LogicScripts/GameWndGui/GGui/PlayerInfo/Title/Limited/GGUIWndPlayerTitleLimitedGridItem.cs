using System;
using ALPackage;
using Common.NpPlayerInfoObj;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 固定称号列表item
    /// </summary>
    public class GGUIWndPlayerTitleLimitedGridItem : _ATALUGUIBasicGridItemWnd<GGUIMonoPlayerTitleLimitedGridItem>
    {
        //称号信息
        private PlayerTitleRefObj _m_titleRef;
        //点击选中
        private Action<GGUIWndPlayerTitleLimitedGridItem> _m_onClickSelect;
        //加载的底框
        private GGUIWndPrefabSubDressItem _m_wbgItem;

        /// <summary>
        /// 称号信息
        /// </summary>
        public PlayerTitleRefObj titleRef { get { return _m_titleRef; } }
        /// <summary>
        /// 点击选中
        /// </summary>
        public Action<GGUIWndPlayerTitleLimitedGridItem> onClickSelect
        {
            get { return _m_onClickSelect; }
            set { _m_onClickSelect = value; }
        }

        public GGUIWndPlayerTitleLimitedGridItem(GGUIMonoPlayerTitleLimitedGridItem _wnd) : base(_wnd)
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
        public void setInfo(PlayerTitleRefObj _info)
        {
            if (wnd == null || _info == null)
                return;

            _m_titleRef = _info;

            //设置解锁状态
            PlayerTitleInfo titleInfo = NPPlayer.instance.titleComp.getTitleInfo(_m_titleRef.id);
            bool isUnlock = titleInfo != null && !titleInfo.isExpired;
            ALUGUICommon.setGameObjEnable(wnd.goLockShowList, !isUnlock);
            ALUGUICommon.setGameObjEnable(wnd.goLockHideList, isUnlock);

            //设置佩戴状态
            PlayerInfo_Title curTitle = NPPlayer.instance.titleComp.getCurWearCommonTitleInfo();
            bool isCurrent = curTitle != null && _m_titleRef.id == curTitle.getId();
            ALUGUICommon.setGameObjEnable(wnd.goCurrentShowList, isCurrent);
            ALUGUICommon.setGameObjEnable(wnd.goCurrentHideList, !isCurrent);

            long uiResId = 0;
            if (titleInfo == null)
                uiResId = GCommon.getPlayerTitleUIResId(_m_titleRef.id, 1);
            else
                uiResId = titleInfo.getUiResId();

            //加载item
            if (null != wnd.transParent)
            {
                GGUIWndPrefabSubDressItem.checkAndLoadPrefab(_m_wbgItem, uiResId, wnd.transParent, (_prefabItem) =>
                {
                    _m_wbgItem = _prefabItem;
                    _m_wbgItem?.showWnd();
                    _m_wbgItem?.setIcon(ENPItemType.TITLE, _m_titleRef.id);
                    _m_wbgItem?.setName(GCommon.getItemName(ENPItemType.TITLE, _m_titleRef.id));
                });
            }

            //设置红点
            if (titleInfo != null && titleInfo.isNew)
                ALUGUICommon.setGameObjEnable(wnd.goNewRedTip, true);
            else
                ALUGUICommon.setGameObjEnable(wnd.goNewRedTip, false);
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
            {
                ALUGUICommon.setGameObjEnable(wnd.goNewRedTip, false);
                _setIsView();
            }

            ALUGUICommon.setGameObjEnable(wnd.goSelectShowList, _isSelect);
            ALUGUICommon.setGameObjEnable(wnd.goSelectHideList, !_isSelect);
        }

        //设置已查看
        private void _setIsView()
        {
            if (_m_titleRef == null)
                return;

            PlayerTitleInfo titleInfo = NPPlayer.instance.titleComp.getTitleInfo(_m_titleRef.id);
            if (titleInfo != null && titleInfo.isNew)
            {
                titleInfo.setIsViewed(true);
                NPPlayer.instance.titleComp.reqviewTitle(titleInfo.refId);
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
