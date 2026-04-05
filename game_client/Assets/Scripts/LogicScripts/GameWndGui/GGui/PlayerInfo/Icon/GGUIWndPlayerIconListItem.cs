using ALPackage;
using System;
using NPEnum;
using UnityEngine;

namespace GOE
{
    public class GGUIWndPlayerIconListItem : _ANPGGUIBasicGridItemWnd<GGUIMonoPlayerIconListItem>
    {
        //显示信息
        private PlayerIconShowInfo _m_showInfo;
        //品质
        private GGuiWndSprite _m_qualitySptWnd;
        //点击事件
        private Action<GGUIWndPlayerIconListItem> _m_clickDelegate;
        //加载的头像
        private GGUIWndPrefabSubDressItem _m_wIconItem;

        /// <summary>
        /// 展示信息
        /// </summary>
        public PlayerIconShowInfo showInfo { get { return _m_showInfo; } }
        /// <summary>
        /// 点击item事件
        /// </summary>
        public Action<GGUIWndPlayerIconListItem> onClickItem { get { return _m_clickDelegate; } set { _m_clickDelegate = value; } }

        #region override
        public GGUIWndPlayerIconListItem(GGUIMonoPlayerIconListItem _wnd)
           : base(_wnd) 
        {
            initWnd();
        }

        protected override void _onWndInitDone()
        {
            if (null == wnd)
                return;

            ALUGUICommon.combineBtnClick(wnd.clickGo, _didSelectItem);
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wIconItem?.hideWnd();
        }

        protected override void _onReset()
        {
            if (null != _m_qualitySptWnd)
                _m_qualitySptWnd.discardTexture();

            _m_wIconItem?.resetWnd();
        }
        //重置Grid单个对象
        protected override void _resetGridItem()
        {
            if (null != _m_qualitySptWnd)
                _m_qualitySptWnd.discardTexture();

            _m_wIconItem?.resetWnd();
        }

        protected override void _onDiscard()
        {
            if (null != _m_qualitySptWnd)
                _m_qualitySptWnd.discard();
            _m_qualitySptWnd = null;

            _m_wIconItem?.discard();
            _m_wIconItem = null;

            ALUGUICommon.uncombineBtnClick(wnd.clickGo, _didSelectItem);
        }
        #endregion
        // 初始化UI
        public void refreshItem(PlayerIconShowInfo _item, bool _isSelected, bool _needShowRedTip)
        {
            if (null == wnd || null == _item || null == _item.refObj || null == _item.baseData)
                return;

            _m_showInfo = _item;

            if (null != wnd.iconPosPar)
            {
                GGUIWndPrefabSubDressItem.checkAndLoadPrefab(_m_wIconItem, _item.refObj.asset_path_id, wnd.iconPosPar, (_iconPrefabItem) =>
                {
                    _m_wIconItem = _iconPrefabItem;
                    _m_wIconItem?.showWnd();
                    _m_wIconItem?.setIcon(ENPItemType.ICON, _item.refObj.id);
                });
            }

            if (null != _m_qualitySptWnd)
            {
                NPQualityRefObj qualityRefObj = GRefdataCoreMgr.instance.getQuality(NPEnum.ENPItemType.ICON, _item.baseData.quality);
                if(null != qualityRefObj)
                    _m_qualitySptWnd.setTexture(qualityRefObj.sp_icon);
            }

            //是否使用中
            bool isUse = false;
            if (null != NPPlayer.instance.playerInfo)
            {
                isUse = NPPlayer.instance.playerInfo.getCurrentIconId() == _item.refObj.id;
                ALUGUICommon.setGameObjEnable(wnd.usingFlagImg, isUse);
            }

            // // 显示角标"New" 未拥有 正在使用中 未解锁 则不展示
            // if (null == _item.iconItem || !_item.iconItem.isNew || isUse || _item.isLock || _item.isExpired)
            //     ALUGUICommon.setGameObjEnable(wnd.newFlagImg, false);
            // else
            //     ALUGUICommon.setGameObjEnable(wnd.newFlagImg, true);
            ALUGUICommon.setGameObjEnable(wnd.newFlagImg, _needShowRedTip && !_item.isLock && !_item.isExpired);

            //设置是否选中
            ALUGUICommon.setGameObjEnable(wnd.selectImg, _isSelected);
            // if (_isSelected)
            //     _setIsView();

            //判断上锁 和过期
            if (_item.isLock || _item.isExpired)
                ALUGUICommon.setGameObjEnable(wnd.lockImg, true);
            else
                ALUGUICommon.setGameObjEnable(wnd.lockImg, false);
        }

        // //设置已查看
        // private void _setIsView()
        // {
        //     if (_m_showInfo == null)
        //         return;
        //
        //     //点击时设置已查看
        //     if (_m_showInfo.iconItem != null && _m_showInfo.iconItem.isNew && !_m_showInfo.isLock && !_m_showInfo.isExpired)
        //     {
        //         //设置数据查看过
        //         _m_showInfo.iconItem.setIsViewed(true);
        //         //并告知服务器
        //         NPPlayer.instance.iconComp.reqIsViewed(_m_showInfo.iconItem.playerIconRef.id);
        //     }
        // }

        protected void _didSelectItem(GameObject _go)
        {
            // _setIsView();

            if (null != _m_clickDelegate)
                _m_clickDelegate(this);
        }
    }
}
