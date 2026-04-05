using System;
using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 玩家皮肤item
    /// </summary>
    public class GGUIWndPlayerSkinContainerItem : _ATALBasicUISubWnd<GGUIMonoPlayerSkinContainerItem>
    {
        //玩家皮肤配置
        private PlayerSkinRefObj _m_playerSkinRef;
        //头像
        private NPGGuiWndTexture _m_wIconWnd;
        //是否选中
        private bool _m_bIsSelect;
        //选中item事件
        private Action<GGUIWndPlayerSkinContainerItem> _m_bOnSelectItem;

        /// <summary>
        /// 选中item事件
        /// </summary>
        public Action<GGUIWndPlayerSkinContainerItem> onSelectItem { get { return _m_bOnSelectItem; } set { _m_bOnSelectItem = value; } }
        /// <summary>
        /// 玩家皮肤配置
        /// </summary>
        public PlayerSkinRefObj playerSkinRef { get { return _m_playerSkinRef; } }

        public GGUIWndPlayerSkinContainerItem(GGUIMonoPlayerSkinContainerItem _mono) : base(_mono)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsg(WinMsgType.ON_PLAYER_SKIN_CHG, _onSkinChg);
            WinMsg.RegisterMsg(WinMsgType.ON_PLAYER_SKIN_ADD, _onSkinChg);
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_PLAYER_SKIN_CHG, _onSkinChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_PLAYER_SKIN_ADD, _onSkinChg);
            _m_wIconWnd?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wIconWnd?.discardTexture();
        }

        protected override void _onDiscard()
        {
            _m_bOnSelectItem = null;

            _m_wIconWnd?.discard();
            _m_wIconWnd = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClick, _onClickItem);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.imgIcon != null)
                _m_wIconWnd = new NPGGuiWndTexture(wnd.imgIcon);

            ALUGUICommon.combineBtnClick(wnd.btnClick, _onClickItem);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_id"></param>
        public void setInfo(PlayerSkinRefObj _playerSkinRef)
        {
            if (wnd == null)
                return;

            _m_playerSkinRef = _playerSkinRef;
            _refreshWnd();
        }

        //刷新窗口
        private void _refreshWnd()
        {
            if (wnd == null || _m_playerSkinRef == null)
                return;

            //是否解锁
            PlayerSkinInfo skinInfo = NPPlayer.instance.skinComp.getSkinInfo(_m_playerSkinRef.id);
            bool isUnlock = skinInfo != null;

            //设置头像
            _m_wIconWnd?.showWnd();
            _m_wIconWnd?.setTexture(GCommon.getItemTexIcon(ENPItemType.PLAYER_SKIN, _m_playerSkinRef.id));

            //设置等级
            ALUGUICommon.setLabelTxt(wnd.txtLevel, TextTranslate.instance.getLanguage(TransKeyConst.common_level_num, skinInfo != null ? skinInfo.level : 0));
            ALUGUICommon.setGameObjEnable(wnd.txtLevel, skinInfo != null);

            //设置未解锁显隐
            ALUGUICommon.setGameObjEnable(wnd.goLockShowList, !isUnlock);
            ALUGUICommon.setGameObjEnable(wnd.goLockHideList, isUnlock);

            //设置当前佩戴
            bool isCurrent = NPPlayer.instance.playerInfo.getCurrentSkinId() == _m_playerSkinRef.id;
            ALUGUICommon.setGameObjEnable(wnd.goCurrentShowList, isCurrent);
            ALUGUICommon.setGameObjEnable(wnd.goCurrentHideList, !isCurrent);

            //刷新红点
            refreshRedTip();
        }

        //刷新红点
        public void refreshRedTip()
        {
            if (wnd == null || _m_playerSkinRef == null)
                return;

            PlayerSkinInfo skinInfo = NPPlayer.instance.skinComp.getSkinInfo(_m_playerSkinRef.id);
            //是否已解锁可升级
            bool canUpgrade = skinInfo != null &&
                              skinInfo.curSkinLevelRef != null && 
                              skinInfo.nextSkinLevelRef != null &&
                              skinInfo.curSkinLevelRef.upgrade_cost != null &&
                              skinInfo.curSkinLevelRef.upgrade_cost.getItemType() != ENPItemType.NONE &&
                              GCommon.isItemEnough(skinInfo.curSkinLevelRef.upgrade_cost, false);
            //是否可解锁
            bool canUnlock = skinInfo == null && 
                             _m_playerSkinRef.unlock_item != null &&
                             _m_playerSkinRef.unlock_item.getItemType() != ENPItemType.NONE &&
                             GCommon.isItemEnough(_m_playerSkinRef.unlock_item, false);
            //是否新获得未读
            bool isNewGain = NPPlayer.instance.skinComp.needShowGainRedTip(_m_playerSkinRef.id);
            bool needShowRedTip = canUpgrade || canUnlock || isNewGain;

            ALUGUICommon.setGameObjEnable(wnd.goRedTip, needShowRedTip);
        }

        //设置选中状态
        public void setSelect(bool _isSelect)
        {
            if (wnd == null)
                return;

            _m_bIsSelect = _isSelect;
            ALUGUICommon.setGameObjEnable(wnd.goSelectShowList, _m_bIsSelect);
            ALUGUICommon.setGameObjEnable(wnd.goSelectHideList, !_m_bIsSelect);

            if (_isSelect)
            {
                //已选中设置新获得红点已读，刷新红点
                NPPlayer.instance.skinComp.setReadGainRedTip(_m_playerSkinRef.id);
                refreshRedTip();
            }
        }

        //点击详情按钮
        private void _onClickItem(GameObject _go)
        {
            if (_m_playerSkinRef == null)
                return;

            _m_bOnSelectItem?.Invoke(this);
        }

        //皮肤变更
        private void _onSkinChg(params object[] _objs)
        {
            if (_objs == null || _objs.Length == 0 || _objs[0] == null || _m_playerSkinRef == null)
                return;

            long skinId = (long)_objs[0];
            if (skinId == _m_playerSkinRef.id)
            {
                //如果已选中，设置新获得红点已读
                if (_m_bIsSelect)
                    NPPlayer.instance.skinComp.setReadGainRedTip(_m_playerSkinRef.id);

                _refreshWnd();
            }
        }
    }
}