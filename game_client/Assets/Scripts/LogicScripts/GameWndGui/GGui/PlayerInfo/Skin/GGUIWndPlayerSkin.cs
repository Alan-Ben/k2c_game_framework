using System.Collections.Generic;
using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 玩家皮肤界面
    /// </summary>
    public class GGUIWndPlayerSkin : _ANPGGUIBasicResBarWnd<GGUIMonoPlayerSkin>
    {

        private static GGUIWndPlayerSkin _g_instance;
        public static GGUIWndPlayerSkin instance
        {
            get
            {
                if (_g_instance == null)
                    _g_instance = new GGUIWndPlayerSkin();
                return _g_instance;
            }
        }

        //皮肤列表
        private GGUIWndPlayerSkinContainer _m_wSkinContainer;
        //解锁道具
        private NPGGUIWndCommonItem _m_wUnlockItem;
        //升级道具
        private NPGGUIWndCommonItem _m_wUpgradeItem;
        //当前选中的皮肤
        private PlayerSkinRefObj _m_curSelectPlayerSkin;
        //皮肤形象
        private NPGGUIWndCommonShowCase _m_wShowCase;
        //是否正在处理请求
        private bool _m_bIsDealingReq;
        //显示序列
        private long _m_lShowSerialize;

        protected GGUIWndPlayerSkin() : base(EALUIWndLayer.NORMAL)
        {
        }

        /********************
       * 获取资源所在资源加载文件名称
       **/
        protected override string _monoAssetPath { get { return GGUIMonoPlayerSkin.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoPlayerSkin.objName; } }

        /**************
         * 获取用于加载资源的管理对象
         **/
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        /// <summary>
        /// 在窗口作为Scene中的主展示窗口的时候，在切换时是否会需要释放
        /// </summary>
        public override bool needDiscardOnSwitch { get { return true; } }


        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsg(WinMsgType.ON_PLAYER_SKIN_LEVEL_CHG, _onSkinLevelChg);
            WinMsg.RegisterMsg(WinMsgType.ON_PLAYER_PARAM_CHANGE, _onPlayerParamChg);
            _m_lShowSerialize = ALSerializeOpMgr.next();
            _m_bIsDealingReq = false;
            _refreshContainer();
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_PLAYER_SKIN_LEVEL_CHG, _onSkinLevelChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_PLAYER_PARAM_CHANGE, _onPlayerParamChg);
            _m_wSkinContainer?.hideWnd();
            _m_wUnlockItem?.hideWnd();
            _m_wUpgradeItem?.hideWnd();
            _m_wShowCase?.hideWnd();
            _m_lShowSerialize = ALSerializeOpMgr.next();
            _m_bIsDealingReq = false;
        }

        protected override void _onReset()
        {
            _m_wSkinContainer?.resetWnd();
            _m_wUnlockItem?.resetWnd();
            _m_wUpgradeItem?.resetWnd();
            _m_wShowCase?.resetWnd();
        }
        protected override void _onDiscard()
        {
            _m_wSkinContainer?.discard();
            _m_wSkinContainer = null;
            _m_wUnlockItem?.discard();
            _m_wUnlockItem = null;
            _m_wUpgradeItem?.discard();
            _m_wUpgradeItem = null;
            _m_wShowCase?.discard();
            _m_wShowCase = null;

            if(wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnWear, _onClickWear);
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
            ALUGUICommon.uncombineBtnClick(wnd.btnUnlock, _onClickUnlock);
            ALUGUICommon.uncombineBtnClick(wnd.btnTakeOff, _onClickTakeOff);
            ALUGUICommon.uncombineBtnClick(wnd.btnUpgrade, _onClickUpgrade);
            ALUGUICommon.uncombineBtnClick(wnd.btnLockNoUnlockItem, _onClickLockNoUnlockItemBtn);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoPlayerSkinContainer != null)
            {
                _m_wSkinContainer = new GGUIWndPlayerSkinContainer(wnd.monoPlayerSkinContainer);
                _m_wSkinContainer.onClickItem += _onClickItem;
            }

            if(wnd.monoUnlockItem != null)
                _m_wUnlockItem = new NPGGUIWndCommonItem(wnd.monoUnlockItem);

            if(wnd.monoUpgradeItem != null)
                _m_wUpgradeItem = new NPGGUIWndCommonItem(wnd.monoUpgradeItem);

            if(wnd.monoShowCase != null)
                _m_wShowCase = new NPGGUIWndCommonShowCase(wnd.monoShowCase);

            ALUGUICommon.combineBtnClick(wnd.btnWear, _onClickWear);
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
            ALUGUICommon.combineBtnClick(wnd.btnUnlock, _onClickUnlock);
            ALUGUICommon.combineBtnClick(wnd.btnTakeOff, _onClickTakeOff);
            ALUGUICommon.combineBtnClick(wnd.btnUpgrade, _onClickUpgrade);
            ALUGUICommon.combineBtnClick(wnd.btnLockNoUnlockItem, _onClickLockNoUnlockItemBtn);
        }

        /// <summary>
        /// 设置选中皮肤
        /// </summary>
        public void setSelectSkin(long _skinRefId)
        {
            _m_curSelectPlayerSkin = GRefdataCoreMgr.instance.playerSkinRefCore.getRef(_skinRefId);
        }

        //刷新列表
        private void _refreshContainer()
        {
            List<PlayerSkinRefObj> playerSkinRefList = new List<PlayerSkinRefObj>();
            GRefdataCoreMgr.instance.playerSkinRefCore.dealAllRef(_ref =>
            {
                if(_ref != null && !_ref.is_hide)
                    playerSkinRefList.Add(_ref);
            });

            playerSkinRefList.Sort((_a,_b)=>_a.sort_id.CompareTo(_b.sort_id));

            if (_m_wSkinContainer != null)
            {
                _m_wSkinContainer.showWnd();
                _m_wSkinContainer.showItemList(playerSkinRefList, _m_curSelectPlayerSkin != null ? _m_curSelectPlayerSkin.id : 0);
            }
        }

        //刷新窗口
        private void _refreshWnd()
        {
            _refreshState();
            _refreshDetail();
            _refreshRedTip();
        }

        //刷新状态显隐
        private void _refreshState()
        {
            if (wnd == null || _m_curSelectPlayerSkin == null)
                return;

            PlayerSkinInfo skinInfo = NPPlayer.instance.skinComp.getSkinInfo(_m_curSelectPlayerSkin.id);
            long curWearId = NPPlayer.instance.playerInfo.getCurrentSkinId();

            //按钮状态显隐
            EPlayerSkinBtnState btnState = EPlayerSkinBtnState.LOCK_NEED_UNLOCK_ITEM;
            if (skinInfo != null)
            {
                if (skinInfo.skinId == curWearId)
                    btnState = EPlayerSkinBtnState.UNLOCK_WEAR;
                else
                    btnState = EPlayerSkinBtnState.UNLOCK_NO_WEAR;
            }
            else if (_m_curSelectPlayerSkin.unlock_item == null || _m_curSelectPlayerSkin.unlock_item.getItemType() == ENPItemType.NONE)
                btnState = EPlayerSkinBtnState.LOCK_NO_UNLOCK_ITEM;
            else
                btnState = EPlayerSkinBtnState.LOCK_NEED_UNLOCK_ITEM;

            wnd.setBtnShowState(btnState);

            //是否满级
            bool isMaxLevel = skinInfo != null && skinInfo.nextSkinLevelRef == null;
            ALUGUICommon.setGameObjEnable(wnd.goMaxLevelShowList, isMaxLevel);
            ALUGUICommon.setGameObjEnable(wnd.goMaxLevelHideList, !isMaxLevel);

            //是否有加成属性
            //当前等级数据，未解锁取1级数据
            PlayerSkinLevelRefObj playerSkinLevelRefObj = GRefdataCoreMgr.instance.getPlayerSkinLevelRef(_m_curSelectPlayerSkin.id, skinInfo != null ? skinInfo.level : 1);
            bool isHaveProperty = playerSkinLevelRefObj != null &&
                                  playerSkinLevelRefObj.player_property != null && 
                                  (playerSkinLevelRefObj.player_property.getPropertyValue(ENPPlayerPropertyType.INTIMACY) > 0 || playerSkinLevelRefObj.player_property.getPropertyValue(ENPPlayerPropertyType.CHARM) > 0);
            ALUGUICommon.setGameObjEnable(wnd.goHavePropertyShowList, isHaveProperty);
            ALUGUICommon.setGameObjEnable(wnd.goHavePropertyHideList, !isHaveProperty);
        }

        //刷新皮肤详情
        private void _refreshDetail()
        {
            if (wnd == null || _m_curSelectPlayerSkin == null)
                return;

            PlayerSkinInfo skinInfo = NPPlayer.instance.skinComp.getSkinInfo(_m_curSelectPlayerSkin.id);

            //皮肤形象
            if (_m_wShowCase != null)
            {
                _AShowCaseUnitInfoObj[] showCaseUnitInfoObjList = new _AShowCaseUnitInfoObj[1];
                showCaseUnitInfoObjList.SetValue(new ShowCaseCommonResUnitInfoObj(_m_curSelectPlayerSkin.td_show), 0);
                _m_wShowCase.showWnd(showCaseUnitInfoObjList);
            }
            //解锁道具
            if (_m_wUnlockItem != null)
            {
                _m_wUnlockItem.showWnd();
                _m_wUnlockItem.setItem(_m_curSelectPlayerSkin.unlock_item);
            }
            //名称
            ALUGUICommon.setLabelTxt(wnd.txtName, GCommon.getItemName(ENPItemType.PLAYER_SKIN, _m_curSelectPlayerSkin.id));
            //描述
            ALUGUICommon.setLabelTxt(wnd.txtDesc, GCommon.getItemDesc(ENPItemType.PLAYER_SKIN, _m_curSelectPlayerSkin.id));

            //当前等级数据，未解锁取1级数据
            PlayerSkinLevelRefObj playerSkinLevelRefObj = GRefdataCoreMgr.instance.getPlayerSkinLevelRef(_m_curSelectPlayerSkin.id, skinInfo != null ? skinInfo.level : 1);
            if (playerSkinLevelRefObj == null)
                return;

            //升级道具
            if (playerSkinLevelRefObj != null && playerSkinLevelRefObj.upgrade_cost != null && playerSkinLevelRefObj.upgrade_cost.IsValid)
            {
                if (_m_wUpgradeItem != null)
                {
                    _m_wUpgradeItem.showWnd();
                    _m_wUpgradeItem.setItem(playerSkinLevelRefObj.upgrade_cost);
                }
            }

            //亲密度加成
            ALUGUICommon.setLabelTxt(wnd.txtAddIntimacyValue,
                playerSkinLevelRefObj.player_property != null &&
                playerSkinLevelRefObj.player_property.getPropertyValue(ENPPlayerPropertyType.INTIMACY) > 0
                    ? playerSkinLevelRefObj.player_property.getPropertyValue(ENPPlayerPropertyType.INTIMACY)
                    : 0);
            //加护力加成
            ALUGUICommon.setLabelTxt(wnd.txtAddCharmValue,
                playerSkinLevelRefObj.player_property != null &&
                playerSkinLevelRefObj.player_property.getPropertyValue(ENPPlayerPropertyType.CHARM) > 0
                    ? playerSkinLevelRefObj.player_property.getPropertyValue(ENPPlayerPropertyType.CHARM)
                    : 0);
            //家人数量
            long consortCount = NPPlayer.instance.consortComp.getConsortCount();
            ALUGUICommon.setLabelTxt(wnd.txtConsortCount, consortCount);
        }

        //刷新红点
        private void _refreshRedTip()
        {
            if (wnd == null || _m_curSelectPlayerSkin == null)
                return;

            PlayerSkinInfo skinInfo = NPPlayer.instance.skinComp.getSkinInfo(_m_curSelectPlayerSkin.id);
            bool canUpgrade = skinInfo != null && skinInfo.curSkinLevelRef != null &&
                              skinInfo.nextSkinLevelRef != null &&
                              GCommon.isItemEnough(skinInfo.curSkinLevelRef.upgrade_cost, false);
            bool canUnlock = skinInfo == null && GCommon.isItemEnough(_m_curSelectPlayerSkin.unlock_item, false);

            ALUGUICommon.setGameObjEnable(wnd.goUpgradeRedTip, canUpgrade);
            ALUGUICommon.setGameObjEnable(wnd.goUnlockRedTip, canUnlock);
        }

        #region 点击事件

        //点击皮肤item
        private void _onClickItem(GGUIWndPlayerSkinContainerItem _item)
        {
            if(_item == null || _item.playerSkinRef == null)
                return;

            _m_curSelectPlayerSkin = _item.playerSkinRef;
            _m_lShowSerialize = ALSerializeOpMgr.next();
            _m_bIsDealingReq = false;
            _refreshWnd();
        }

        //点击关闭按钮
        private void _onClickClose(GameObject _obj)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst_PlayerInfo.C_MAIN_PLAYERINFO_SKIN_NODE);
        }

        //点击解锁按钮
        private void _onClickUnlock(GameObject _obj)
        {
            if (_m_bIsDealingReq)
                return;

            if (_m_curSelectPlayerSkin == null)
                return;

            PlayerSkinInfo skinInfo = NPPlayer.instance.skinComp.getSkinInfo(_m_curSelectPlayerSkin.id);
            if (skinInfo != null)
            {
                Debug.LogError($"【玩家皮肤】皮肤已解锁还在请求解锁，id：{_m_curSelectPlayerSkin.id}");
                return;
            }

            //解锁道具是否足够
            if (!GCommon.isItemEnough(_m_curSelectPlayerSkin.unlock_item, true))
                return;

            //请求解锁皮肤
            long serialize = _m_lShowSerialize;
            _m_bIsDealingReq = true;
            NPPlayer.instance.skinComp.reqUnlockPlayerSkin(_m_curSelectPlayerSkin.id,(_isSuc)=>
            {
                if (serialize != _m_lShowSerialize)
                    return;

                _m_bIsDealingReq = false;
                _refreshWnd();
            });
        }

        //点击穿戴按钮
        private void _onClickWear(GameObject _obj)
        {
            if (_m_bIsDealingReq)
                return;

            if (_m_curSelectPlayerSkin == null)
                return;

            PlayerSkinInfo skinInfo = NPPlayer.instance.skinComp.getSkinInfo(_m_curSelectPlayerSkin.id);
            if (skinInfo == null)
            {
                Debug.LogError($"【玩家皮肤】请求穿戴未获得的皮肤，id：{_m_curSelectPlayerSkin.id}");
                return;
            }

            //请求穿戴皮肤
            long serialize = _m_lShowSerialize;
            _m_bIsDealingReq = true;
            NPPlayer.instance.skinComp.reqSetCurPlayerSkin(_m_curSelectPlayerSkin.id, (_isSuc)=>
            {
                if (serialize != _m_lShowSerialize)
                    return;

                _m_bIsDealingReq = false;
                _refreshState();
            });
        }

        //点击取消穿戴按钮
        private void _onClickTakeOff(GameObject _obj)
        {
            if (_m_bIsDealingReq)
                return;

            if (_m_curSelectPlayerSkin == null)
                return;

            //请求穿戴皮肤
            long serialize = _m_lShowSerialize;
            _m_bIsDealingReq = true;
            NPPlayer.instance.skinComp.reqUnsetCurPlayerSkin((_isSuc)=>
            {
                if (serialize != _m_lShowSerialize)
                    return;

                _m_bIsDealingReq = false;
                _refreshState();
            });
        }

        //点击升级按钮
        private void _onClickUpgrade(GameObject _obj)
        {
            if (_m_bIsDealingReq)
                return;

            if (_m_curSelectPlayerSkin == null)
                return;

            PlayerSkinInfo skinInfo = NPPlayer.instance.skinComp.getSkinInfo(_m_curSelectPlayerSkin.id);
            if (skinInfo == null)
            {
                Debug.LogError($"【玩家皮肤】请求升级未获得的皮肤，id：{_m_curSelectPlayerSkin.id}");
                return;
            }

            //皮肤是否满级
            if (skinInfo.nextSkinLevelRef == null)
            {
                Debug.LogError($"【玩家皮肤】请求升级的皮肤已满级，id：{_m_curSelectPlayerSkin.id}");
                return;
            }

            //升级道具是否足够
            if (skinInfo.curSkinLevelRef != null && !GCommon.isItemEnough(skinInfo.curSkinLevelRef.upgrade_cost, true))
                return;

            //请求升级皮肤
            long serialize = _m_lShowSerialize;
            _m_bIsDealingReq = true;
            NPPlayer.instance.skinComp.reqUpgradePlayerSkin(_m_curSelectPlayerSkin.id, (_isSuc)=>
            {
                if (serialize != _m_lShowSerialize)
                    return;

                _m_bIsDealingReq = false;
                _refreshWnd();
            });
        }

        //点击不需要解锁道具的未解锁按钮
        private void _onClickLockNoUnlockItemBtn(GameObject _go)
        {
            if (_m_curSelectPlayerSkin == null)
                return;

            NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.common_notObtained_str,GCommon.getItemName(ENPItemType.PLAYER_SKIN, _m_curSelectPlayerSkin.id)));
        }

        #endregion

        #region 消息事件

        //皮肤等级变化
        private void _onSkinLevelChg(params object[] _objects)
        {
            if(_objects == null || _objects.Length < 3)
                return;

            long skinId = (long)_objects[0];
            int oriLevel = (int)_objects[1];
            int curLevel = (int)_objects[2];
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndPlayerSkinUpgradeSuc.instance, () =>
            {
                GGUIWndPlayerSkinUpgradeSuc.instance.showWnd();
                GGUIWndPlayerSkinUpgradeSuc.instance.setInfo(skinId, curLevel);
            },UINodeTagConst_PlayerInfo.C_MAIN_PLAYERINFO_SKIN_UPGRADE_NODE);
        }

        //玩家参数变化
        private void _onPlayerParamChg(params object[] _objects)
        {
            if (_objects == null || _objects.Length == 0)
                return;

            int index = (int)_objects[0];
            if (index == (int)ENPPlayerParam.PLAYER_SKIN)
                _refreshContainer();
        }

        #endregion
    }
}
