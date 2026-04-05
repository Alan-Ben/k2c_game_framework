using ALPackage;
using Common;
using System;
using System.Collections;
using System.Collections.Generic;
using NPEnum;
using UnityEngine;

namespace GOE
{
    // 通用奖励物品item
    public class GGUIWndCreatePlayerPrefabItem : _ATALBasicUISubWnd<GGUIMonoCreatePlayerPrefabItem>
    {
        private NPGGuiWndTexture _m_playerIcon;
        //半身像
        private NPGGuiWndTexture _m_playerCardImage;
        private Action<GGUIWndCreatePlayerPrefabItem> _m_clickAction;//点击回调
        private bool _m_isSelected = false;
        private PlayerCreatPlayerPrefabRefObj _m_refObj;
        
        public GGUIWndCreatePlayerPrefabItem(GGUIMonoCreatePlayerPrefabItem _wnd, Action<GGUIWndCreatePlayerPrefabItem> _clickAction) : base(_wnd)
        {
            _m_clickAction = _clickAction;
            initWnd();
        }

        public PlayerCreatPlayerPrefabRefObj refObj { get { return _m_refObj; } }
        
        // 初始化
        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
            
            if (null != wnd.monoIcon)
                _m_playerIcon = new NPGGuiWndTexture(wnd.monoIcon);

            if(null != wnd.imgCard)
                _m_playerCardImage = new NPGGuiWndTexture(wnd.imgCard);

            ALUGUICommon.combineBtnClick(wnd.btnSelect, _clickSelected);

        }
        protected override void _onShowWnd()
        {
            if (null != _m_playerIcon)
                _m_playerIcon.showWnd();
            if (null != _m_playerCardImage)
                _m_playerCardImage.showWnd();
        }
        protected override void _onHideWnd()
        {
            if (null != _m_playerIcon)
                _m_playerIcon.hideWnd();
            if (null != _m_playerCardImage)
                _m_playerCardImage.hideWnd();
        }

        protected override void _onReset()
        {
            if (null != _m_playerIcon)
                _m_playerIcon.discardTexture();
            if (null != _m_playerCardImage)
                _m_playerCardImage.discardTexture();
        }

        protected override void _onDiscard()
        {
            if (null != _m_playerIcon)
                _m_playerIcon.discard();
            _m_playerIcon = null;
            if (null != _m_playerCardImage)
                _m_playerCardImage.discard();
            _m_playerCardImage = null;

            _m_refObj = null;
        }

        public void setInfo(PlayerCreatPlayerPrefabRefObj _refObj)
        {
            if(null == _refObj)
                return;
            
            _m_refObj = _refObj;
            NPGTextureIndex iconIndex = GCommon.getItemTexIcon(ENPItemType.ICON, _refObj.icon_id);
            if (null != _m_playerIcon)
                _m_playerIcon.setTexture(iconIndex);

            PlayerSkinRefObj skinRef = GRefdataCoreMgr.instance.playerSkinRefCore.getRef(_refObj.skin_id);
            if(null != _m_playerCardImage)
                _m_playerCardImage.setTexture(skinRef?.card_image);

            _m_isSelected = false;
            _refreshSelected();
        }
        
        /// <summary>
        /// 设置选中状态
        /// </summary>
        /// <param name="_isSelected"></param>
        public void setSelected(bool _isSelected)
        {
            _m_isSelected = _isSelected;
            _refreshSelected();
        }
        
        private void _refreshSelected()
        {
            if (null == wnd)
                return;

            ALUGUICommon.setGameObjEnable(wnd.goListShowOnSelect, _m_isSelected);
            ALUGUICommon.setGameObjEnable(wnd.goListHideOnSelect, !_m_isSelected);
        }
        
        private void _clickSelected(GameObject obj)
        {
            _m_clickAction?.Invoke(this);
        }
    }
}
