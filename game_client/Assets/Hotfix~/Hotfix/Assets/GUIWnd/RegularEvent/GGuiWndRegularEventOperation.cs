using System;
using ALPackage;
using Common.ActivityEnum;
using GOE;
using NPEnum;
using System.Collections.Generic;
using UnityEngine;

namespace Hotfix
{
    /// <summary>
    /// 万能活动操作界面，支持换皮
    /// </summary>
    public class GGuiWndRegularEventOperation : _AHotfixBaseWnd<GGUIMonoRegularEventOperation>
    {
        //活动id
        private long _m_lActivityId;
        //活动信息
        private _ABaseActivityInfo _m_activityInfo;
        //当前选中的商店道具
        private RegularEventShopItemRefObj _m_curSelectItemRef;
        //使用十次勾选
        private NPGGUIWndCommonToggleEx _m_wUseTenToggle;
        //消耗道具
        private NPGGUIWndCommonItem _m_wCostItem;
        //附加排行榜界面
        private GGUIWndRegularEventSubRank _m_wSubRank;
        //是否正在处理活动关闭
        private bool _m_bIsDealingClose;
        //显示序列
        private long _m_lShowSerialize;
        //动画参数名哈希值
        private int _m_animatorStateTypeHash;
        //展示奖励弹窗事件
        private Action _m_aDealShowReward;
        //使用道具特效对象列表
        private List<CommonUISfxObj> _m_useItemSfxList;

        protected override string _monoAssetPath { get { return GCommon.getActivityPrefabSkinAssetPath(_m_lActivityId,HotfixUINodeTagConst.REGULAR_EVENT_OPERATION); } }
        protected override string _monoObjName { get { return GCommon.getActivityPrefabSkinObjName(_m_lActivityId, HotfixUINodeTagConst.REGULAR_EVENT_OPERATION); } }
        public override bool needDiscardOnSwitch { get { return true; } }

        public GGuiWndRegularEventOperation(long _activityId) : base(EALUIWndLayer.NORMAL)
        {
            _m_lActivityId = _activityId;
        }

        protected override void _onShowWnd()
        {
            _m_lShowSerialize = ALSerializeOpMgr.next();
            _m_activityInfo = NPPlayer.instance.commonActivityComp.getValidActivityInfoByActivityId(_m_lActivityId);
            WinMsg.RegisterMsg(WinMsgType.ON_COMMON_ACTIVITY_STATE_CHG, _onActivityStateChg);
            ALMsgSys.RegisterMsg(HotfixMsgType.ON_REGULAR_EVENT_SHOP_ITEM_CHG, _onShopChg);

            _checkAutoSetItem();
            _refreshWnd();

            if(hotfixWnd != null && hotfixWnd.useItemAnimator != null)
                hotfixWnd.useItemAnimator.SetInteger(_m_animatorStateTypeHash, hotfixWnd.idleAniValue);
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_COMMON_ACTIVITY_STATE_CHG, _onActivityStateChg);
            ALMsgSys.UnregisterMsg(HotfixMsgType.ON_REGULAR_EVENT_SHOP_ITEM_CHG, _onShopChg);
            _m_bIsDealingClose = false;
            _m_lShowSerialize = ALSerializeOpMgr.next();

            _m_wUseTenToggle?.hideWnd();
            _m_wCostItem?.hideWnd();
            _m_wSubRank?.hideWnd();
            _m_aDealShowReward?.Invoke();
            _m_aDealShowReward = null;

            if (_m_useItemSfxList != null)
            {
                for (int i = 0; i < _m_useItemSfxList.Count; i++)
                {
                    _m_useItemSfxList[i]?.forceDiscard();
                }
                _m_useItemSfxList.Clear();
            }
        }

        protected override void _onReset()
        {
            _m_wUseTenToggle?.resetWnd();
            _m_wCostItem?.resetWnd();
            _m_wSubRank?.resetWnd();
        }

        protected override void _onDiscard()
        {
            if (hotfixWnd == null)
                return;

            _m_wUseTenToggle?.discard();
            _m_wUseTenToggle = null;
            _m_wCostItem?.discard();
            _m_wCostItem = null;
            _m_wSubRank?.discard();
            _m_wSubRank = null;

            ALUGUICommon.uncombineBtnClick(hotfixWnd.btnAddItem, _onClickAddItem);
            ALUGUICommon.uncombineBtnClick(hotfixWnd.btnClose, _onClickClose);
            ALUGUICommon.uncombineBtnClick(hotfixWnd.btnUse, _onClickUse);
        }

        protected override void _onWndInitDoneHotfix()
        {
            if (hotfixWnd == null)
                return;

            if (hotfixWnd.toggleUseTen != null)
            {
                _m_wUseTenToggle = new NPGGUIWndCommonToggleEx(hotfixWnd.toggleUseTen);
                _m_wUseTenToggle.clickDelegate += _onClickToggle;
            }

            if (hotfixWnd.monoCostItem != null)
                _m_wCostItem = new NPGGUIWndCommonItem(hotfixWnd.monoCostItem);

            if(hotfixWnd.monoSubRank != null)
                _m_wSubRank = new GGUIWndRegularEventSubRank(hotfixWnd.monoSubRank);

            if(!string.IsNullOrEmpty(hotfixWnd.aniParamName))
                _m_animatorStateTypeHash = Animator.StringToHash(hotfixWnd.aniParamName);

            ALUGUICommon.combineBtnClick(hotfixWnd.btnAddItem, _onClickAddItem);
            ALUGUICommon.combineBtnClick(hotfixWnd.btnClose, _onClickClose);
            ALUGUICommon.combineBtnClick(hotfixWnd.btnUse, _onClickUse);
        }

        //设置选中的商店道具
        public void setShopItem(RegularEventShopItemRefObj _shopItemRef)
        {
            _m_curSelectItemRef = _shopItemRef;
            _refreshWnd();
        }

        //刷新窗口
        private void _refreshWnd()
        {
            if (hotfixWnd == null)
                return;

            //使用十次开关
            _m_wUseTenToggle?.showWnd();
            _m_wUseTenToggle?.setSelected(HotfixNPPlayer.instance.regularEventComponent.useTen, true, false);

            //消耗道具
            if(_m_curSelectItemRef == null)
                _m_wCostItem?.hideWnd();
            else
            {
                NPCommonCostItem costItem = new NPCommonCostItem(_m_curSelectItemRef.item);
                costItem.setCount(GCommon.getItemCount(_m_curSelectItemRef.item.item));
                _m_wCostItem?.showWnd();
                _m_wCostItem?.setItem(costItem);
            }

            //显示状态
            GGameCommonInfo.grayImage(hotfixWnd.goCanNotUseGrayList, _m_curSelectItemRef == null);
            ALUGUICommon.setGameObjEnable(hotfixWnd.goHaveItemHideList, _m_curSelectItemRef == null);
            ALUGUICommon.setGameObjEnable(hotfixWnd.goHaveItemShowList, _m_curSelectItemRef != null);

            //设置排行榜
            _m_wSubRank?.showWnd();
            _m_wSubRank?.setInfo(_m_lActivityId);
        }

        //检查是否需要自动设置道具
        private void _checkAutoSetItem()
        {
            //已经有了不处理
            if (_m_curSelectItemRef != null)
                return;

            //获取道具列表
            List<RegularEventShopItemRefObj> regularEventShopItemRefList = new List<RegularEventShopItemRefObj>();
            HotfixRefdataCoreMgr.instance.regularEventShopItemRefCore.dealAllRef(_ref =>
            {
                if (_ref != null && _ref.activity_id == _m_lActivityId && _ref.item != null &&
                    GCommon.isItemEnough(_ref.item.getItemType(), _ref.item.subId, 1, false))
                    regularEventShopItemRefList.Add(_ref);
            });
            regularEventShopItemRefList.Sort(_sortList);

            //设置选中第一个
            if (regularEventShopItemRefList.Count > 0)
                _m_curSelectItemRef = regularEventShopItemRefList[0];
        }
        private int _sortList(RegularEventShopItemRefObj _a, RegularEventShopItemRefObj _b)
        {
            if (_a == null || _b == null || _a.item == null || _b.item == null)
                return 0;

            EQuality qualityA = GCommon.getItemQuality(_a.item.getItemType(), _a.item.subId);
            EQuality qualityB = GCommon.getItemQuality(_b.item.getItemType(), _b.item.subId);

            if (qualityA != qualityB)
                return -(qualityA.CompareTo(qualityB));

            return _a.id.CompareTo(_b.id);
        }

        #region 点击事件

        //点击使用十次开关
        private void _onClickToggle(NPGGUIWndCommonToggleEx _commonToggleEx)
        {
            if (_m_wUseTenToggle == null)
                return;

            bool isOn = !_m_wUseTenToggle.isOn;
            _m_wUseTenToggle.setSelected(isOn, true);
            HotfixNPPlayer.instance.regularEventComponent.useTen = isOn;
        }

        //点击关闭按钮
        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(HotfixUINodeTagConst.REGULAR_EVENT_OPERATION);
        }

        //点击添加道具按钮
        private void _onClickAddItem(GameObject _go)
        {
            GGuiWndRegularEventShop shopWnd = new GGuiWndRegularEventShop(_m_lActivityId);
            shopWnd.onUseItem += setShopItem;
            QueueMgr.instance.addNode_InGame_SingleWnd(shopWnd, shopWnd.showWnd, HotfixUINodeTagConst.REGULAR_EVENT_SHOP);
        }

        //点击使用按钮
        private void _onClickUse(GameObject _go)
        {
            if (_m_curSelectItemRef == null || _m_activityInfo == null)
                return;

            bool isTen = _m_wUseTenToggle != null && _m_wUseTenToggle.isOn;

            long serialize = _m_lShowSerialize;
            HotfixNPPlayer.instance.regularEventComponent.reqRegularActivityUseItem(_m_activityInfo.instanceId, _m_curSelectItemRef.id, isTen,
                (isSuc, _itemList) =>
                {
                    if (serialize != _m_lShowSerialize)
                        return;

                    if (!isSuc)
                        return;

                    //如果用完了重置选择
                    if(_m_curSelectItemRef != null && _m_curSelectItemRef.item != null&& GCommon.getItemCount(_m_curSelectItemRef.item.item) <= 0)
                        _m_curSelectItemRef = null;

                    //播放动画，奖励弹窗
                    if (hotfixWnd != null && hotfixWnd.useItemAnimator != null)
                    {
                        //播放使用动画
                        hotfixWnd.useItemAnimator.SetInteger(_m_animatorStateTypeHash, hotfixWnd.useItemAniValue);
                        CommonTaskController.CommonActionAddNextFrameTask(() =>
                        {
                            //重置动画id
                            hotfixWnd.useItemAnimator.SetInteger(_m_animatorStateTypeHash, -1);
                        });
                    }

                    //播放特效
                    if (hotfixWnd != null && hotfixWnd.sfxParent != null && hotfixWnd.useSfxId > 0)
                    {
                        if (_m_useItemSfxList == null)
                            _m_useItemSfxList = new List<CommonUISfxObj>();
                        CommonUISfxObj sfxObj = PlaySfxMgr.instance.playUISfx(hotfixWnd.useSfxId, hotfixWnd.sfxParent);
                        _m_useItemSfxList.Add(sfxObj);
                    }

                    //设置展示奖励事件
                    _m_aDealShowReward = () => 
                    {
                        //展示奖励
                        GCommon.showGainRewardTip(_itemList);
                    };

                    CommonTaskController.CommonActionAddMonoTask(() =>
                    {
                        if (serialize != _m_lShowSerialize)
                            return;

                        //展示奖励
                        _m_aDealShowReward?.Invoke();
                        _m_aDealShowReward = null;

                    }, hotfixWnd.delayShowRewardTimeSec);

                    //刷新窗口
                    _checkAutoSetItem();
                    _refreshWnd();
                });
        }

        #endregion

        #region 消息事件

        //活动状态变更
        private void _onActivityStateChg(params object[] _objects)
        {
            if (_objects == null || _objects.Length < 4)
                return;

            if (_m_bIsDealingClose)
                return;
            _m_bIsDealingClose = true;

            long activityId = (long)_objects[0];
            long activityInstanceId = (long)_objects[1];
            EActivityState oriState = (EActivityState)_objects[2];
            EActivityState curState = (EActivityState)_objects[3];
            if (activityId != _m_lActivityId)
                return;

            //是否在活动期，否则弹窗结束活动
            if (curState != EActivityState.PLAYING)
            {
                NPMesMgr.instance.showOneBtnMes(
                    TextTranslate.instance.getLanguage(TransKeyConst.common_activity_alreadyEnd_none),
                    TextTranslate.instance.getLanguage(TransKeyConst.confirm),
                    () =>
                    {
                        _m_bIsDealingClose = false;
                        QueueMgr.instance.QuitUntilCanStop(_node =>
                        {
                            return _node != null && _node.nodeTag == UINodeTagConst.C_ACTIVITY_CENTER_WND;
                        });
                    });
            }
        }

        //商店变更
        private void _onShopChg(params object[] _args)
        {
            if (_args == null || _args.Length == 0)
                return;

            long instanceId = (long)_args[0];
            if (_m_activityInfo == null || _m_activityInfo.instanceId != instanceId)
                return;

            _checkAutoSetItem();
            _refreshWnd();
        }

        #endregion
    }
}