using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using CommonEnum;

namespace GOE
{
    /// <summary>
    /// 藏品升级页签界面
    /// </summary>
    public class GGUIWndEquipUpgradePage : _ATALBasicLoadPrefabSubUIWnd<GGUIMonoEquipUpgradePage>
    {
        private string _m_sAssetPath;//窗口对象的资源 加载路径
        private string _m_sObjName;//窗口对象的资源 名字

        //藏品信息
        private EquipInfo _m_equipInfo;
        //连升十级勾选
        private NPGGUIWndCommonToggleEx _m_wTenLevelUpToggle;
        //升级消耗道具
        private NPGGUIWndCommonItem _m_wCostItem;
        //特效列表
        private List<CommonUISfxObj> _m_lSfxObjList;

        public GGUIWndEquipUpgradePage(NPCommonAssetPathInfo _assetPathInfo, Transform _parent)
            : base(_parent)
        {
            _m_sAssetPath = _assetPathInfo.asset_path;
            _m_sObjName = _assetPathInfo.obj_name;
        }

        /**************
         * 窗口相关加载配置
         **/
        protected override string _monoAssetPath { get { return _m_sAssetPath; } }
        protected override string _monoObjName { get { return _m_sObjName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsg(WinMsgType.ON_EQUIP_LEVEL_CHG, _onEquipLevelChg);
            WinMsg.RegisterMsg(WinMsgType.ON_EQUIP_BASE_CHG, _onEquipChg);
            WinMsg.RegisterMsg(WinMsgType.ON_BAG_ITEM_ADD, _onBagItemChg);
            WinMsg.RegisterMsg(WinMsgType.ON_BAG_ITEM_UPDATE, _onBagItemChg);
            WinMsg.RegisterMsg(WinMsgType.ON_BAG_ITEM_REMOVE, _onBagItemChg);
            WinMsg.RegisterMsgAct(WinMsgType.SIMULATE_CLICK_EQUIP_UPGRADE, _onSimulateClickEquipUpgrade);
            _m_wTenLevelUpToggle?.showWnd();
            _m_wTenLevelUpToggle?.setSelected(AccountSettingMgr.instance.accountSetting.isEquipTenUpgrade, true, false);
        }
        
        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_EQUIP_LEVEL_CHG, _onEquipLevelChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_EQUIP_BASE_CHG, _onEquipChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_BAG_ITEM_ADD, _onBagItemChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_BAG_ITEM_UPDATE, _onBagItemChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_BAG_ITEM_REMOVE, _onBagItemChg);
            WinMsg.UnregisterMsgAct(WinMsgType.SIMULATE_CLICK_EQUIP_UPGRADE, _onSimulateClickEquipUpgrade);
            _m_wCostItem?.hideWnd();
            _discardSfx();
        }
        
        protected override void _onReset()
        {
            _m_wCostItem?.resetWnd();
        }
        
        protected override void _onDiscard()
        {
            if (wnd == null)
                return;

            _m_wTenLevelUpToggle?.discard();
            _m_wTenLevelUpToggle = null;

            _m_wCostItem?.discard();
            _m_wCostItem = null;

            _discardSfx();

            ALUGUICommon.uncombineBtnClick(wnd.btnUpgrade, _onClickUpgrade);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.toggleTenLevelUp != null)
            {
                _m_wTenLevelUpToggle = new NPGGUIWndCommonToggleEx(wnd.toggleTenLevelUp);
                _m_wTenLevelUpToggle.clickDelegate += _onClickToggle;
            }

            if (wnd.monoCostItem != null)
                _m_wCostItem = new NPGGUIWndCommonItem(wnd.monoCostItem);

            ALUGUICommon.combineBtnClick(wnd.btnUpgrade, _onClickUpgrade);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        public void setInfo(EquipInfo _info)
        {
            _m_equipInfo = _info;
            _refreshWnd();
        }

        //刷新窗口
        private void _refreshWnd()
        {
            _refreshBaseInfo();
            _refreshButtonState();
        }

        //刷新基础信息
        private void _refreshBaseInfo()
        {
            if (wnd == null || _m_equipInfo == null)
                return;

            //设置等级
            ALUGUICommon.setLabelTxt(wnd.txtLevel, TextTranslate.instance.getLanguage(TransKeyConst.equip_equipLevel_num, _m_equipInfo.level));
            //设置资质
            ALUGUICommon.setLabelTxt(wnd.txtTalent, TextTranslate.instance.getLanguage(TransKeyConst.equip_talentValue_num, _m_equipInfo.talentValue));
        }

        //刷新按钮状态
        private void _refreshButtonState()
        {
            if (wnd == null || _m_equipInfo == null || _m_equipInfo.equipRef == null)
                return;

            //是否可以十连升级
            bool canTenLevelUp = GCommon.isSimpleUnlock(GRefdataCoreMgr.instance.npGeneral.equip_upgrade_ten_times_simple_unlock_id);
            ALUGUICommon.setGameObjEnable(wnd.goTenLevelUpShowList, canTenLevelUp);
            ALUGUICommon.setGameObjEnable(wnd.goTenLevelUpHideList, !canTenLevelUp);

            //设置消耗的道具
            bool isTen = _m_wTenLevelUpToggle != null && _m_wTenLevelUpToggle.isOn;
            NPCommonCostItem costItem;
            if (isTen)
            {
                long targetLevel = _m_equipInfo.level + 10;
                if (targetLevel > _m_equipInfo.levelLimit)
                    targetLevel = _m_equipInfo.levelLimit;
                costItem = new NPCommonCostItem(_m_equipInfo.equipRef.cost_item);
                costItem.setCount(costItem.count*(targetLevel  - _m_equipInfo.level));
            }
            else
                costItem = new NPCommonCostItem(_m_equipInfo.equipRef.cost_item);

            if (_m_wCostItem != null)
            {
                _m_wCostItem.showWnd();
                _m_wCostItem.setItem(costItem);
            }

            //是否满级
            bool isMaxLevel = _m_equipInfo.level == _m_equipInfo.levelLimit;
            ALUGUICommon.setGameObjEnable(wnd.goMaxLevelHideList, !isMaxLevel);
            ALUGUICommon.setGameObjEnable(wnd.goMaxLevelShowList, isMaxLevel);
        }

        //销毁特效
        private void _discardSfx()
        {
            if (_m_lSfxObjList != null)
            {
                for (int i = 0; i < _m_lSfxObjList.Count; i++)
                {
                    _m_lSfxObjList[i]?.forceDiscard();
                }
                _m_lSfxObjList.Clear();
                _m_lSfxObjList = null;
            }
        }

        #region 消息事件

        //藏品信息变更
        private void _onEquipChg(params object[] _objects)
        {
            if (_objects == null || _objects.Length == 0)
                return;

            long dbId = (long) _objects[0];

            if (_m_equipInfo != null && _m_equipInfo.dbId == dbId)
            {
                _refreshBaseInfo();
                _refreshButtonState();
            }
        }

        //道具变更
        private void _onBagItemChg(params object[] _objects)
        {
            _refreshButtonState();
        }

        //模拟点击升级藏品
        private void _onSimulateClickEquipUpgrade()
        {
            _onClickUpgrade(null);
        }

        //等级变更
        private void _onEquipLevelChg(params object[] _objects)
        {
            if (_objects == null || _objects.Length < 3 || _m_equipInfo == null)
                return;

            EquipInfo equipInfo = _objects[0] as EquipInfo;
            long oriLevel = (long)_objects[1];
            long newLevel = (long)_objects[2];
            if (equipInfo != null && equipInfo.dbId == _m_equipInfo.dbId)
            {
                //播放特效
                if (_m_lSfxObjList == null)
                    _m_lSfxObjList = new List<CommonUISfxObj>();

                if (wnd != null && wnd.upgradeSfxParent != null)
                {
                    CommonUISfxObj sfxObj = null;
                    if (newLevel - oriLevel == 1 && wnd.singleUpgradeSfxId > 0)//单次升级成功特效
                        sfxObj = PlaySfxMgr.instance.playUISfx(wnd.singleUpgradeSfxId, wnd.upgradeSfxParent);
                    else if (newLevel - oriLevel > 1 && wnd.tenUpgradeSfxId > 0)//十连升级成功特效
                        sfxObj = PlaySfxMgr.instance.playUISfx(wnd.tenUpgradeSfxId, wnd.upgradeSfxParent);

                    if (sfxObj != null)
                        _m_lSfxObjList.Add(sfxObj);
                }
            }
        }

        #endregion

        #region 点击事件

        //点击连升十级开关
        private void _onClickToggle(NPGGUIWndCommonToggleEx _commonToggleEx)
        {
            if (_m_wTenLevelUpToggle == null)
                return;

            if (!GCommon.isSimpleUnlock(GRefdataCoreMgr.instance.npGeneral.equip_upgrade_ten_times_simple_unlock_id, true))
                return;

            bool isOn = !_m_wTenLevelUpToggle.isOn;
            _m_wTenLevelUpToggle.setSelected(isOn, true);
            AccountSettingMgr.instance.accountSetting.setIsEquipTenUpgrade(isOn);
            _refreshButtonState();
        }

        //点击升级按钮
        private void _onClickUpgrade(GameObject _go)
        {
            if (_m_equipInfo == null || _m_equipInfo.equipRef == null)
                return;

            //是否满级
            bool isMaxLevel = _m_equipInfo.level == _m_equipInfo.levelLimit;
            if (isMaxLevel)
                return;

            bool isTen = _m_wTenLevelUpToggle != null && _m_wTenLevelUpToggle.isOn;
            //判断消耗，只要能升一级就可以升级
            if (GCommon.isItemEnough(_m_equipInfo.equipRef.cost_item, true))
            {
                //请求升级
                NPPlayer.instance.equipComp.reqEquipUpgrade(_m_equipInfo.dbId, isTen);
            }
        }

        #endregion
    }
}