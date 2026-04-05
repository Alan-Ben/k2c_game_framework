using System;
using ALPackage;
using Common.ArenaEnum;
using GS2GC.p023_ArenaOp;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 竞技场一键谈判弹窗
    /// </summary>
    public class GGUIWndArenaBattleOneKey : _ANPGGUIBasicWnd<GGUIMonoArenaBattleOneKey>
    {
        private static GGUIWndArenaBattleOneKey _g_instance;
        public static GGUIWndArenaBattleOneKey instance
        {
            get
            {
                if (_g_instance == null)
                    _g_instance = new GGUIWndArenaBattleOneKey();
                return _g_instance;
            }
        }

        //购买水晶临时增益开关
        private NPGGUIWndCommonToggleEx _m_wBuyBuffByCrystal;
        //购买2银币增益开关
        private NPGGUIWndCommonToggleEx _m_wBuyBuffByTwoCoin;
        //购买1银币增益开关
        private NPGGUIWndCommonToggleEx _m_wBuyBuffByOneCoin;
        //不购买增益开关
        private NPGGUIWndCommonToggleEx _m_wNotToBuyBuff;
        //一键谈判返回回调
        private Action<GS2GC_023_013_RetArenaAKeyAttack> _m_aOnAKeyAttackRet;

        public GGUIWndArenaBattleOneKey() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoArenaBattleOneKey.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoArenaBattleOneKey.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wBuyBuffByCrystal?.hideWnd();
            _m_wBuyBuffByTwoCoin?.hideWnd();
            _m_wBuyBuffByOneCoin?.hideWnd();
            _m_wNotToBuyBuff?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wBuyBuffByCrystal?.resetWnd();
            _m_wBuyBuffByTwoCoin?.resetWnd();
            _m_wBuyBuffByOneCoin?.resetWnd();
            _m_wNotToBuyBuff?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_wBuyBuffByCrystal?.discard();
            _m_wBuyBuffByCrystal = null;
            _m_wBuyBuffByTwoCoin?.discard();
            _m_wBuyBuffByTwoCoin = null;
            _m_wBuyBuffByOneCoin?.discard();
            _m_wBuyBuffByOneCoin = null;
            _m_wNotToBuyBuff?.discard();
            _m_wNotToBuyBuff = null;

            if(wnd == null)
                return; 

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
            ALUGUICommon.uncombineBtnClick(wnd.btnFight, _onClickFight);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoBuyBuffByCrystalToggle != null)
            {
                _m_wBuyBuffByCrystal = new NPGGUIWndCommonToggleEx(wnd.monoBuyBuffByCrystalToggle);
                _m_wBuyBuffByCrystal.clickDelegate += _onClickBuyBuffByCrystal;
            }

            if (wnd.monoBuyBuffByTwoCoinToggle != null)
            {
                _m_wBuyBuffByTwoCoin = new NPGGUIWndCommonToggleEx(wnd.monoBuyBuffByTwoCoinToggle);
                _m_wBuyBuffByTwoCoin.clickDelegate += _onClickBuyBuffByTwoCoin;
            }

            if (wnd.monoBuyBuffByOneCoinToggle != null)
            {
                _m_wBuyBuffByOneCoin = new NPGGUIWndCommonToggleEx(wnd.monoBuyBuffByOneCoinToggle);
                _m_wBuyBuffByOneCoin.clickDelegate += _onClickBuyBuffByOneCoin;
            }

            if (wnd.monoNotToBuyBuffToggle != null)
            {
                _m_wNotToBuyBuff = new NPGGUIWndCommonToggleEx(wnd.monoNotToBuyBuffToggle);
                _m_wNotToBuyBuff.clickDelegate += _onClickNotToBuyBuff;
            }

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
            ALUGUICommon.combineBtnClick(wnd.btnFight, _onClickFight);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        public void setInfo(Action<GS2GC_023_013_RetArenaAKeyAttack> _onAKeyAttackRet)
        {
            _m_aOnAKeyAttackRet = _onAKeyAttackRet;
            _refreshWnd();
        }

        //刷新窗口
        private void _refreshWnd()
        {
            _m_wBuyBuffByCrystal?.showWnd();
            _m_wBuyBuffByCrystal?.setSelected(AccountSettingMgr.instance.accountSetting.arenaOneKeySetAutoBuyBuffByCrystal, true);

            _m_wBuyBuffByTwoCoin?.showWnd();
            _m_wBuyBuffByTwoCoin?.setSelected(AccountSettingMgr.instance.accountSetting.arenaOneKeySetAutoBuyBuffByTwoCoin, true);

            _m_wBuyBuffByOneCoin?.showWnd();
            _m_wBuyBuffByOneCoin?.setSelected(AccountSettingMgr.instance.accountSetting.arenaOneKeySetAutoBuyBuffByOneCoin, true);

            _m_wNotToBuyBuff?.showWnd();
            _m_wNotToBuyBuff?.setSelected(AccountSettingMgr.instance.accountSetting.arenaOneKeySetNotToBuyBuff, true);
        }

        //重置选中状态
        private void _resetSelect()
        {
            _m_wBuyBuffByCrystal?.setSelected(false, true);
            _m_wBuyBuffByTwoCoin?.setSelected(false, true);
            _m_wBuyBuffByOneCoin?.setSelected(false, true);
            _m_wNotToBuyBuff?.setSelected(false, true);

            AccountSettingMgr.instance.accountSetting.setArenaOneKeySetAutoBuyBuffByCrystal(false);
            AccountSettingMgr.instance.accountSetting.setArenaOneKeySetAutoBuyBuffByTwoCoin(false);
            AccountSettingMgr.instance.accountSetting.setArenaOneKeySetAutoBuyBuffByOneCoin(false);
            AccountSettingMgr.instance.accountSetting.setArenaOneKeySetNotToBuyBuff(false);
        }

        #region 点击事件

        //点击购买水晶临时增益开关
        private void _onClickBuyBuffByCrystal(NPGGUIWndCommonToggleEx _toggle)
        {
            if (_m_wBuyBuffByCrystal == null)
                return;

            bool isOn = !_m_wBuyBuffByCrystal.isOn;

            //如果开启了开关，重置其他选项
            if (isOn)
                _resetSelect();

            _m_wBuyBuffByCrystal.setSelected(isOn, true);
            AccountSettingMgr.instance.accountSetting?.setArenaOneKeySetAutoBuyBuffByCrystal(isOn);
        }

        //点击购买2银币增益开关
        private void _onClickBuyBuffByTwoCoin(NPGGUIWndCommonToggleEx _toggle)
        {
            if (_m_wBuyBuffByTwoCoin == null)
                return;

            bool isOn = !_m_wBuyBuffByTwoCoin.isOn;

            //如果开启了开关，重置其他选项
            if (isOn)
                _resetSelect();

            _m_wBuyBuffByTwoCoin.setSelected(isOn, true);
            AccountSettingMgr.instance.accountSetting?.setArenaOneKeySetAutoBuyBuffByTwoCoin(isOn);
        }

        //点击购买1银币增益开关
        private void _onClickBuyBuffByOneCoin(NPGGUIWndCommonToggleEx _toggle)
        {
            if (_m_wBuyBuffByOneCoin == null)
                return;

            bool isOn = !_m_wBuyBuffByOneCoin.isOn;

            //如果开启了开关，重置其他选项
            if (isOn)
                _resetSelect();

            _m_wBuyBuffByOneCoin.setSelected(isOn, true);
            AccountSettingMgr.instance.accountSetting?.setArenaOneKeySetAutoBuyBuffByOneCoin(isOn);
        }

        //点击不购买增益开关
        private void _onClickNotToBuyBuff(NPGGUIWndCommonToggleEx _toggle)
        {
            if (_m_wNotToBuyBuff == null)
                return;

            bool isOn = !_m_wNotToBuyBuff.isOn;

            //如果开启了开关，重置其他选项
            if (isOn)
                _resetSelect();

            _m_wNotToBuyBuff.setSelected(isOn, true);
            AccountSettingMgr.instance.accountSetting?.setArenaOneKeySetNotToBuyBuff(isOn);
        }

        //点击关闭
        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_ARENA_BATTLE_ONE_KEY);
        }

        //点击开始谈判
        private void _onClickFight(GameObject _go)
        {
            EArenaBuffType selectType = EArenaBuffType.NONE;
            if (_m_wBuyBuffByCrystal != null && _m_wBuyBuffByCrystal.isOn)
                selectType = EArenaBuffType.CRYSTAL;
            else if (_m_wBuyBuffByTwoCoin != null && _m_wBuyBuffByTwoCoin.isOn)
                selectType = EArenaBuffType.TWO_COIN;
            else if (_m_wBuyBuffByOneCoin != null && _m_wBuyBuffByOneCoin.isOn)
                selectType = EArenaBuffType.ONE_COIN;
            else if (_m_wNotToBuyBuff != null && _m_wNotToBuyBuff.isOn)
                selectType = EArenaBuffType.NONE;

            //先屏蔽输入
            int maskSerialize = MainCameraMono.selfInstance.openAllInputMask();
            NPPlayer.instance.arenaComp.reqArenaAKeyAttack(selectType,(_isSuc, _msg) =>
            {
                MainCameraMono.selfInstance.closeAllInputMask(maskSerialize);
                if (!_isSuc)
                    return;
                QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_ARENA_BATTLE_ONE_KEY);
                _m_aOnAKeyAttackRet?.Invoke(_msg);
                _m_aOnAKeyAttackRet = null;
            });
        }

        #endregion
    }
}