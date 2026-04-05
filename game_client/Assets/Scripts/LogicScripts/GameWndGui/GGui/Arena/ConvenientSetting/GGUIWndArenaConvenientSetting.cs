using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 竞技场便捷设置弹窗
    /// </summary>
    public class GGUIWndArenaConvenientSetting : _ANPGGUIBasicWnd<GGUIMonoArenaConvenientSetting>
    {
        private static GGUIWndArenaConvenientSetting _g_instance;
        public static GGUIWndArenaConvenientSetting instance
        {
            get
            {
                if (_g_instance == null)
                    _g_instance = new GGUIWndArenaConvenientSetting();
                return _g_instance;
            }
        }

        //自动随机翻开连胜奖励开关
        private NPGGUIWndCommonToggleEx _m_wAutoGetRoundReward;
        //跳过单场战斗动画开关
        private NPGGUIWndCommonToggleEx _m_wSkipBattle;
        //购买水晶临时增益开关
        private NPGGUIWndCommonToggleEx _m_wBuyBuffByCrystal;
        //购买2银币增益开关
        private NPGGUIWndCommonToggleEx _m_wBuyBuffByTwoCoin;
        //购买1银币增益开关
        private NPGGUIWndCommonToggleEx _m_wBuyBuffByOneCoin;
        //不购买增益开关
        private NPGGUIWndCommonToggleEx _m_wNotToBuyBuff;

        public GGUIWndArenaConvenientSetting() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoArenaConvenientSetting.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoArenaConvenientSetting.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onShowWnd()
        {
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            _m_wAutoGetRoundReward?.hideWnd();
            _m_wSkipBattle?.hideWnd();
            _m_wBuyBuffByCrystal?.hideWnd();
            _m_wBuyBuffByTwoCoin?.hideWnd();
            _m_wBuyBuffByOneCoin?.hideWnd();
            _m_wNotToBuyBuff?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wAutoGetRoundReward?.resetWnd();
            _m_wSkipBattle?.resetWnd();
            _m_wBuyBuffByCrystal?.resetWnd();
            _m_wBuyBuffByTwoCoin?.resetWnd();
            _m_wBuyBuffByOneCoin?.resetWnd();
            _m_wNotToBuyBuff?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_wAutoGetRoundReward?.discard();
            _m_wAutoGetRoundReward = null;
            _m_wSkipBattle?.discard();
            _m_wSkipBattle = null;
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
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoAutoGetRoundRewardToggle != null)
            {
                _m_wAutoGetRoundReward = new NPGGUIWndCommonToggleEx(wnd.monoAutoGetRoundRewardToggle);
                _m_wAutoGetRoundReward.clickDelegate += _onClickAutoGetRoundReward;
            }

            if (wnd.monoSkipBattleToggle != null)
            {
                _m_wSkipBattle = new NPGGUIWndCommonToggleEx(wnd.monoSkipBattleToggle);
                _m_wSkipBattle.clickDelegate += _onClickSkipBattle;
            }


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
        }

        //刷新窗口
        private void _refreshWnd()
        {
            _m_wAutoGetRoundReward?.showWnd();
            _m_wAutoGetRoundReward?.setSelected(AccountSettingMgr.instance.accountSetting.arenaSetAutoGetRoundReward, true);

            _m_wSkipBattle?.showWnd();
            _m_wSkipBattle?.setSelected(AccountSettingMgr.instance.accountSetting.arenaSetSkipBattle, true);

            _m_wBuyBuffByCrystal?.showWnd();
            _m_wBuyBuffByCrystal?.setSelected(AccountSettingMgr.instance.accountSetting.arenaSetAutoBuyBuffByCrystal, true);

            _m_wBuyBuffByTwoCoin?.showWnd();
            _m_wBuyBuffByTwoCoin?.setSelected(AccountSettingMgr.instance.accountSetting.arenaSetAutoBuyBuffByTwoCoin, true);

            _m_wBuyBuffByOneCoin?.showWnd();
            _m_wBuyBuffByOneCoin?.setSelected(AccountSettingMgr.instance.accountSetting.arenaSetAutoBuyBuffByOneCoin, true);

            _m_wNotToBuyBuff?.showWnd();
            _m_wNotToBuyBuff?.setSelected(AccountSettingMgr.instance.accountSetting.arenaSetNotToBuyBuff, true);
        }

        //重置选中状态
        private void _resetSelect()
        {
            _m_wBuyBuffByCrystal?.setSelected(false, true);
            _m_wBuyBuffByTwoCoin?.setSelected(false, true);
            _m_wBuyBuffByOneCoin?.setSelected(false, true);
            _m_wNotToBuyBuff?.setSelected(false, true);

            AccountSettingMgr.instance.accountSetting.setArenaSetAutoBuyBuffByCrystal(false);
            AccountSettingMgr.instance.accountSetting.setArenaSetAutoBuyBuffByTwoCoin(false);
            AccountSettingMgr.instance.accountSetting.setArenaSetAutoBuyBuffByOneCoin(false);
            AccountSettingMgr.instance.accountSetting.setArenaSetNotToBuyBuff(false);
        }

        #region 点击事件

        //点击自动随机翻开连胜奖励开关
        private void _onClickAutoGetRoundReward(NPGGUIWndCommonToggleEx _toggle)
        {
            if (_m_wAutoGetRoundReward == null)
                return;

            bool isOn = !_m_wAutoGetRoundReward.isOn;
            _m_wAutoGetRoundReward.setSelected(isOn, true);
            AccountSettingMgr.instance.accountSetting?.setArenaSetAutoGetRoundReward(isOn);
        }

        //点击跳过单场战斗动画开关
        private void _onClickSkipBattle(NPGGUIWndCommonToggleEx _toggle)
        {
            if (_m_wSkipBattle == null)
                return;

            bool isOn = !_m_wSkipBattle.isOn;
            _m_wSkipBattle.setSelected(isOn, true);
            AccountSettingMgr.instance.accountSetting?.setArenaSetSkipBattle(isOn);
        }

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
            AccountSettingMgr.instance.accountSetting?.setArenaSetAutoBuyBuffByCrystal(isOn);
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
            AccountSettingMgr.instance.accountSetting?.setArenaSetAutoBuyBuffByTwoCoin(isOn);
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
            AccountSettingMgr.instance.accountSetting?.setArenaSetAutoBuyBuffByOneCoin(isOn);
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
            AccountSettingMgr.instance.accountSetting?.setArenaSetNotToBuyBuff(isOn);
        }

        //点击关闭
        private void _onClickClose(GameObject obj)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_ARENA_CONVENTENT_SETTING);
        }

        #endregion
    }
}