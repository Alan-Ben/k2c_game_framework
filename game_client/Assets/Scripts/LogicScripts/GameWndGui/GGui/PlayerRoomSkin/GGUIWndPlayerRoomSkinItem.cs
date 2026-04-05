using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 玩家卧室皮肤列表项 Wnd
    /// </summary>
    public class GGUIWndPlayerRoomSkinItem : _ATNPGGUIWndCommonSelectItem<GGUIMonoPlayerRoomSkinItem>
    {
        // 皮肤配表数据
        private PlayerRoomSkinRefObj _m_roomSkinRefObj;
        // 皮肤拥有数据（可能为null，表示未拥有）
        private PlayerRoomSkinInfo _m_roomSkinInfo;
        // 当前状态
        private EPlayerRoomSkinState _m_eState;

        private NPGGuiWndTexture _m_wCardIcon;

        public GGUIWndPlayerRoomSkinItem(_INPGGUIWndCommonSelectItemOpReceive _receiver, GGUIMonoPlayerRoomSkinItem _wnd) : base(_receiver, _wnd)
        {
            initWnd();
        }

        /// <summary>
        /// 皮肤配表数据
        /// </summary>
        public PlayerRoomSkinRefObj roomSkinRefObj { get { return _m_roomSkinRefObj; } }
        
        /// <summary>
        /// 皮肤拥有数据
        /// </summary>
        public PlayerRoomSkinInfo roomSkinInfo { get { return _m_roomSkinInfo; } }
        
        /// <summary>
        /// 当前状态
        /// </summary>
        public EPlayerRoomSkinState state { get { return _m_eState; } }


        protected override void _onShowWnd()
        {
            refreshWnd();
        }
        
        protected override void _onHideWnd()
        {
            _m_wCardIcon?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wCardIcon?.discardTexture();
        }

        protected override void _resetGridItem()
        {
        }
        
        protected override void _onWndInitDone()
        {
            base._onWndInitDone();
            
            if(wnd == null)
                return;
            
            if(wnd.texIcon != null)
                _m_wCardIcon = new NPGGuiWndTexture(wnd.texIcon);
            
            ALUGUICommon.combineBtnClick(wnd.btnUse, _onUseBtnClick);
        }
        
        protected override void _dealSelectBtnDiscard()
        {
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnUse, _onUseBtnClick);
            }
            
            _m_wCardIcon?.discard();
            _m_wCardIcon = null;
            
            _m_roomSkinRefObj = null;
            _m_roomSkinInfo = null;
        }


        /// <summary>
        /// 刷新界面（带参数）
        /// </summary>
        /// <param name="_refObj">皮肤配表数据</param>
        public void refreshWnd(PlayerRoomSkinRefObj _refObj, PlayerRoomSkinInfo _playerRoomSkinInfo, EPlayerRoomSkinState _playerRoomSkinState)
        {
            _m_roomSkinRefObj = _refObj;
            _m_roomSkinInfo = _playerRoomSkinInfo;
            _m_eState = _playerRoomSkinState;
            
            refreshWnd();
        }

        /// <summary>
        /// 刷新界面
        /// </summary>
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow)
                return;

            if (_m_wCardIcon != null && _m_roomSkinRefObj != null)
            {
                _m_wCardIcon.showWnd();
                _m_wCardIcon.setTexture(_m_roomSkinRefObj.card_img);
            }
            
            _refreshStateShow();
        }

        /// <summary>
        /// 刷新状态显示
        /// </summary>
        private void _refreshStateShow()
        {
            if (wnd == null)
                return;

            NPCommonEnumStatMutexShowInfo<EPlayerRoomSkinState>.setStat(wnd.stateConfigList, _m_eState);
        }
        
        private void _onUseBtnClick(GameObject _go)
        {
            if(_m_roomSkinRefObj == null || _m_eState != EPlayerRoomSkinState.POSSESS_UNOCCUPIED)
                return;
            
            NPPlayer.instance.roomSkinComp.reqSetRoomSkin(_m_roomSkinRefObj.id);
        }
    }
}
