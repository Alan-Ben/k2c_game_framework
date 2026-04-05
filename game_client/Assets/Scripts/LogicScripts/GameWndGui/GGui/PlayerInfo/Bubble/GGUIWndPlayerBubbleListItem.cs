using UnityEngine;
using System;
using ALPackage;
using NPEnum;


namespace GOE
{
    public class GGUIWndPlayerBubbleListItem : _ANPGGUIBasicGridItemWnd<GGUIMonoPlayerBubbleListItem>, _IPlayerInfoItemPlay
    {
        private NPPlayerBubbleShowInfo _m_showInfo;
        //点击事件
        private Action<int, _IPlayerInfoItemPlay> _m_clickDelegate;
        // 资源
        private GGUIWndPrefabSubDressItem _m_wBubbleItem;

        #region override
        public GGUIWndPlayerBubbleListItem(GGUIMonoPlayerBubbleListItem _wnd)
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
            _m_wBubbleItem?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wBubbleItem?.resetWnd();
        }
        //重置Grid单个对象
        protected override void _resetGridItem()
        {
            _m_wBubbleItem?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_wBubbleItem?.discard();
            _m_wBubbleItem = null;

            ALUGUICommon.uncombineBtnClick(wnd.clickGo, _didSelectItem);
        }
        #endregion
        // 初始化UI
        public void refreshItem(NPPlayerBubbleShowInfo _item, bool _isSelected, bool _needShowRedTip)
        {
            if (null == _item || null == _item.refObj || null == _item.baseData)
                return;

            _m_showInfo = _item;

            bool isUse = false;
            if (null != NPPlayer.instance.playerInfo)
                isUse = NPPlayer.instance.playerInfo.getCurrentBubbleId() == _item.refObj.id;
            ALUGUICommon.setGameObjEnable(wnd.usingFlagImg, isUse);

            // // 显示角标"New" 未拥有 正在使用中 未解锁 则不展示
            // if (null == _item.bubbleItem || !_item.bubbleItem.isNew || isUse || _item.isLock || _item.isExpired)
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

            NPGSpriteIndex sptIcon = _m_showInfo?.refObj?.spt_icon;
            //气泡框预制体
            GGUIWndPrefabSubDressItem.checkAndLoadPrefab(_m_wBubbleItem, _item.refObj.asset_path_id, wnd.parentPos, (_bubbleItem) =>
            {
                _m_wBubbleItem = _bubbleItem;
                _m_wBubbleItem?.showWnd();
                _m_wBubbleItem?.setIcon(ENPItemType.BUBBLE, _item.refObj.id);
                _m_wBubbleItem?.setSptIcon(sptIcon);
            });
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
        //     if (_m_showInfo == null || _m_showInfo.bubbleItem == null)
        //         return;
        //
        //     if (_m_showInfo.bubbleItem.isNew && !_m_showInfo.isLock && !_m_showInfo.isExpired)
        //     {
        //         //设置数据查看过
        //         _m_showInfo.bubbleItem.setIsViewed(true);
        //         //并告知服务器
        //         NPPlayer.instance.bubbleComp.reqIsViewed(_m_showInfo.bubbleItem.refObj.id);
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
