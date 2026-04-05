using ALPackage;
using System;
using NPEnum;
using UnityEngine;


namespace GOE
{
    public class GGUIWndPlayerIconBgkListItem : _ANPGGUIBasicGridItemWnd<GGUIMonoPlayerIconBgkListItem>, _IPlayerInfoItemPlay
    {
        private PlayerIconBgkShowInfo _m_showInfo;
        //品质
        private GGuiWndSprite _m_qualitySptWnd;
        //点击事件
        private Action<int, _IPlayerInfoItemPlay> _m_clickDelegate;

        private GGUIWndPrefabSubDressItem _m_wIconBgkItem;

        #region override
        public GGUIWndPlayerIconBgkListItem(GGUIMonoPlayerIconBgkListItem _wnd)
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
            _m_wIconBgkItem?.hideWnd();
        }

        protected override void _onReset()
        {
            if (null != _m_qualitySptWnd)
                _m_qualitySptWnd.discardTexture();

            _m_wIconBgkItem?.resetWnd();
        }
        //重置Grid单个对象
        protected override void _resetGridItem()
        {
            if (null != _m_qualitySptWnd)
                _m_qualitySptWnd.discardTexture();

            _m_wIconBgkItem?.resetWnd();
        }

        protected override void _onDiscard()
        {
            if (null != _m_qualitySptWnd)
                _m_qualitySptWnd.discard();
            _m_qualitySptWnd = null;

            _m_wIconBgkItem?.discard();
            _m_wIconBgkItem = null;

            ALUGUICommon.uncombineBtnClick(wnd.clickGo, _didSelectItem);
        }
        #endregion
        // 初始化UI
        public void refreshItem(PlayerIconBgkShowInfo _item, bool _isSelected, bool _needShowRedTip)
        {
            if (null == wnd || null == _item || null == _item.refObj || null == _item.baseData)
                return;

            _m_showInfo = _item;

            if (null != wnd.iconBgkPosPar)
            {
                GGUIWndPrefabSubDressItem.checkAndLoadPrefab(_m_wIconBgkItem, _item.refObj.asset_path_id, wnd.iconBgkPosPar, (_iconBgkItem) =>
                {
                    _m_wIconBgkItem = _iconBgkItem;
                    _m_wIconBgkItem?.showWnd();
                    _m_wIconBgkItem?.setIcon(ENPItemType.ICON_BGK, _item.refObj.id);
                });
            }

            if (null != _m_qualitySptWnd)
                _m_qualitySptWnd.setTexture(GCommon.getItemQualityIcon(NPEnum.ENPItemType.ICON_BGK, _item.baseData.quality));

            //是否使用中
            bool isUse = false;
            if (null != NPPlayer.instance.playerInfo)
            {
                isUse = NPPlayer.instance.playerInfo.getCurrentIconBgkId() == _item.refObj.id;
                ALUGUICommon.setGameObjEnable(wnd.usingFlagImg, isUse);
            }

            // // 显示角标"New" 未拥有 正在使用中 未解锁 则不展示
            // if (null == _item.iconBgkItem || !_item.iconBgkItem.isNew || isUse || _item.isLock || _item.isExpired)
            //     ALUGUICommon.setGameObjEnable(wnd.newFlagImg, false);
            // else
            //     ALUGUICommon.setGameObjEnable(wnd.newFlagImg, true);
            ALUGUICommon.setGameObjEnable(wnd.newFlagImg, _needShowRedTip && !_item.isLock && !_item.isExpired);

            //设置是否选中
            ALUGUICommon.setGameObjEnable(wnd.selectImg, _isSelected);
            // if(_isSelected)
            //     _setIsView();

            //判断上锁 
            if (_item.isLock || _item.isExpired)
                ALUGUICommon.setGameObjEnable(wnd.lockImg, true);
            else
                ALUGUICommon.setGameObjEnable(wnd.lockImg, false);
        }
        public void playSelectAni()
        {
            if (null == wnd || null == wnd.selectAni)
                return;

            wnd.selectAni.Play();
        }

        public void setClickDelegate(Action<int,_IPlayerInfoItemPlay> _action)
        {
            _m_clickDelegate = _action;
        }

        // //设置已查看
        // private void _setIsView()
        // {
        //     if (_m_showInfo == null || _m_showInfo.iconBgkItem == null)
        //         return;
        //
        //     if (_m_showInfo.iconBgkItem.isNew && !_m_showInfo.isLock && !_m_showInfo.isExpired)
        //     {
        //         //设置数据查看过
        //         _m_showInfo.iconBgkItem.setIsViewed(true);
        //         //并告知服务器
        //         NPPlayer.instance.iconBgkComp.reqIsViewed(_m_showInfo.iconBgkItem.iconBgkRef.id);
        //     }
        // }
        protected void _didSelectItem(GameObject _go)
        {
            // _setIsView();
            if (null != _m_clickDelegate)
                _m_clickDelegate(itemIdx,this);
        }
    }
}
