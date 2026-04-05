using System.Collections.Generic;
using ALPackage;
using CommonEnum;
using JetBrains.Annotations;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// VIP奖励主界面
    /// </summary>
    public class GGUIWndVIPMain : _ANPGGUIBasicResBarWnd<GGUIMonoVIPMain>
    {
        private static GGUIWndVIPMain _g_instance;
        public static GGUIWndVIPMain instance
        {
            get
            {
                if(_g_instance == null)
                    _g_instance = new GGUIWndVIPMain();
                return _g_instance;
            }
        }

        //页签列表
        private GGUIWndVIPTabContainer _m_wTabContainer;
        //奖励容器
        private GGUIWndCommonRewardContainer _m_wRewardContainer;
        //角色特殊奖励列表
        private GGUIWndVIPActorItemContainer _m_wActorItemContainer;
        //页面字典,<资源id，页面>
        [NotNull] private Dictionary<long, GGUIWndVIPDetailPage> _m_dpageDic = new Dictionary<long, GGUIWndVIPDetailPage>();


        public GGUIWndVIPMain() : base(EALUIWndLayer.NORMAL)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoVIPMain.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoVIPMain.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        protected override bool isShowAniPlayOnlyOne { get { return true; } }
        public override bool needDiscardOnSwitch { get { return true; } }

        protected override void _onShowWnd()
        {
            _refreshContainer();
        }

        protected override void _onHideWnd()
        {
            _m_wTabContainer?.hideWnd();
            _m_wRewardContainer?.hideWnd();
            _m_wActorItemContainer?.hideWnd();
            foreach (GGUIWndVIPDetailPage bgPage in _m_dpageDic.Values)
            {
                bgPage?.hideWnd();
            }
        }

        protected override void _onReset()
        {
            _m_wTabContainer?.resetWnd();
            _m_wRewardContainer?.resetWnd();
            _m_wActorItemContainer?.resetWnd();
            foreach (GGUIWndVIPDetailPage bgPage in _m_dpageDic.Values)
            {
                bgPage?.resetWnd();
            }
        }

        protected override void _onDiscard()
        {
            _m_wTabContainer?.discard();
            _m_wTabContainer = null;
            _m_wRewardContainer?.discard();
            _m_wRewardContainer = null;
            _m_wActorItemContainer?.discard();
            _m_wActorItemContainer = null;
            foreach (GGUIWndVIPDetailPage bgPage in _m_dpageDic.Values)
            {
                bgPage?.discard();
            }
            _m_dpageDic.Clear();

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
            ALUGUICommon.uncombineBtnClick(wnd.btnGetReward, _onClickGetReward);
            ALUGUICommon.uncombineBtnClick(wnd.btnAccess, _onClickAccess);
            ALUGUICommon.uncombineBtnClick(wnd.btnPrevious, _onClickPrevious);
            ALUGUICommon.uncombineBtnClick(wnd.btnNext, _onClickNext);
            ALUGUICommon.uncombineBtnClick(wnd.btnVIPPreview, _onClickVipPreview);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoTabContainer != null)
            {
                _m_wTabContainer = new GGUIWndVIPTabContainer(wnd.monoTabContainer);
                _m_wTabContainer.onClickItem += _onClickTabItem;
            }

            if (wnd.monoRewardContainer != null)
                _m_wRewardContainer = new GGUIWndCommonRewardContainer(wnd.monoRewardContainer);

            if (wnd.monoActorItemContainer != null)
                _m_wActorItemContainer = new GGUIWndVIPActorItemContainer(wnd.monoActorItemContainer);

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
            ALUGUICommon.combineBtnClick(wnd.btnGetReward, _onClickGetReward);
            ALUGUICommon.combineBtnClick(wnd.btnAccess, _onClickAccess);
            ALUGUICommon.combineBtnClick(wnd.btnPrevious, _onClickPrevious);
            ALUGUICommon.combineBtnClick(wnd.btnNext, _onClickNext);
            ALUGUICommon.combineBtnClick(wnd.btnVIPPreview, _onClickVipPreview);
        }

        /// <summary>
        /// 刷新界面
        /// </summary>
        private void _refreshWnd(bool _rewardMoveToLeft = false)
        {
            _refreshRewardState(_rewardMoveToLeft);
            _refreshPage();
        }

        /// <summary>
        /// 刷新VIP列表
        /// </summary>
        private void _refreshContainer()
        {
            if (_m_wTabContainer == null)
                return;

            List<VipRefObj> vipRefList = new List<VipRefObj>();
            vipRefList.AddRange(GRefdataCoreMgr.instance.vipRefCore.refList);
            for (int i = 0; i < vipRefList.Count; i++)
            {
                if (vipRefList[i].vip_lvl == 0)
                {
                    vipRefList.RemoveAt(i);
                    break;
                }
            }
            _m_wTabContainer.showWnd();
            _m_wTabContainer.showItemList(vipRefList);

            //如果已经有选择则跳过
            if (_m_wTabContainer.curSelectItem != null && _m_wTabContainer.curSelectItem.vipRef != null)
            {
                _refreshWnd();
                return;
            }

            //设置默认选中
            long vipLevel = NPPlayer.instance.playerInfo.getValue(ENPPlayerParam.VIP_LVL);
            for (int i = 0; i < vipRefList.Count; i++)
            {
                if(vipRefList[i] == null)
                    continue;

                //选中可领取的 或者 下一个vip等级 或者 是最后一个
                if ((vipRefList[i].vip_lvl != 0 && !NPPlayer.instance.playerInfo.isGetVipReward((int) vipRefList[i].vip_lvl)) ||
                    (vipRefList[i].vip_lvl == vipLevel + 1) || 
                    (i == vipRefList.Count - 1))
                {
                    _m_wTabContainer.setSelectItem(vipRefList[i].vip_lvl, false);
                    break;
                }
            }
        }

        /// <summary>
        /// 刷新奖励状态
        /// </summary>
        private void _refreshRewardState(bool _rewardMoveToLeft = false)
        {
            if (wnd == null)
                return;

            ECommonRewardType rewardType = _getRewardType();
            VipRefObj curVipRef = _m_wTabContainer?.curSelectItem?.vipRef;
            if (curVipRef == null)
                return;

            //奖励列表
            List<NPCommonCostItem> itemList = new List<NPCommonCostItem>();
            List<NPCommonCostItem> actorItemList = new List<NPCommonCostItem>();
            if (curVipRef.gain_item_list != null)
            {
                itemList.AddRange(curVipRef.gain_item_list);
                itemList.Sort(_sortItemList);

                //如果有配置角色特殊奖励列表，则分离出来
                if (_m_wActorItemContainer != null)
                {
                    for (int i = itemList.Count - 1; i >= 0; i--)
                    {
                        if (itemList[i] != null && (itemList[i].getItemType() == ENPItemType.HERO || itemList[i].getItemType() == ENPItemType.CONSORT))
                        {
                            actorItemList.Add(itemList[i]);
                            itemList.RemoveAt(i);
                        }
                    }
                }
            }
            _m_wRewardContainer?.showWnd();
            _m_wRewardContainer?.setRewardList(itemList, rewardType);
            if(_rewardMoveToLeft)
                _m_wRewardContainer?.moveToLeft();

            //角色特殊奖励列表
            if (_m_wActorItemContainer != null)
            {
                actorItemList.Sort(_sortItemList);
                _m_wActorItemContainer.showWnd();
                _m_wActorItemContainer.showItemList(actorItemList, rewardType);
            }

            //奖励列表标题
            ALUGUICommon.setLabelTxt(wnd.txtRewardTitle, TextTranslate.instance.getLanguage(TransKeyConst.vip_rewardTitle_num, curVipRef.vip_lvl));

            //按钮状态
            NPCommonEnumStatInfo<ECommonRewardType>.setStat(wnd?.goRewardStatList, rewardType);

            //前后按钮显隐
            ALUGUICommon.setGameObjEnable(wnd.btnPrevious, GRefdataCoreMgr.instance.vipRefCore.getRef(curVipRef.vip_lvl - 1) != null && curVipRef.vip_lvl != 1);
            ALUGUICommon.setGameObjEnable(wnd.btnNext, GRefdataCoreMgr.instance.vipRefCore.getRef(curVipRef.vip_lvl + 1) != null);
        }

        /// <summary>
        /// 刷新页面加载
        /// </summary>
        private void _refreshPage()
        {
            if(wnd == null || wnd.pageParent == null)
                return;

            VipRefObj curVipRef = _m_wTabContainer?.curSelectItem?.vipRef;
            if (curVipRef == null)
                return;

            _hideAllPage();
            if (!_m_dpageDic.TryGetValue(curVipRef.page_ui_res_id, out GGUIWndVIPDetailPage detailPage))
            {
                detailPage = new GGUIWndVIPDetailPage(curVipRef.page_ui_res_id, wnd.pageParent);
                detailPage.load(() =>
                {
                    detailPage.showWnd();
                    detailPage.setInfo(curVipRef);
                });
                _m_dpageDic[curVipRef.page_ui_res_id] = detailPage;
            }
            else
            {
                detailPage?.showWnd();
                detailPage?.setInfo(curVipRef);
            }
        }

        /// <summary>
        /// 隐藏所有页面
        /// </summary>
        private void _hideAllPage()
        {
            foreach (GGUIWndVIPDetailPage bgPage in _m_dpageDic.Values)
            {
                bgPage?.hideWnd();
            }
        }

        /// <summary>
        /// 获取奖励类型枚举
        /// </summary>
        /// <returns></returns>
        private ECommonRewardType _getRewardType()
        {
            ECommonRewardType rewardType = ECommonRewardType.NONE;

            VipRefObj curSelectVipRef = _m_wTabContainer?.curSelectItem?.vipRef;
            if (curSelectVipRef != null)
            {
                if (NPPlayer.instance.playerInfo.isGetVipReward((int) curSelectVipRef.vip_lvl))
                    rewardType = ECommonRewardType.HAS_GET_REWARD;
                else if(GCommon.getItemCount(ENPItemType.CURRENCY,(long)ECurrency.VIP_EXP) >= curSelectVipRef.vip_exp)
                    rewardType = ECommonRewardType.CAN_GET_REWARD;
                else
                    rewardType = ECommonRewardType.NOT_GET_REWARD;
            }
            return rewardType;
        }

        /// <summary>
        /// 奖励列表排序，情人>顾问>其他，品质,id
        /// </summary>
        /// <param name="_a"></param>
        /// <param name="_b"></param>
        /// <returns></returns>
        private int _sortItemList(NPCommonCostItem _a, NPCommonCostItem _b)
        {
            if (_a == null || _b == null)
                return 0;

            bool isHeroA = _a.getItemType() == ENPItemType.HERO;
            bool isHeroB = _b.getItemType() == ENPItemType.HERO;
            bool isConsortA = _a.getItemType() == ENPItemType.CONSORT;
            bool isConsortB = _b.getItemType() == ENPItemType.CONSORT;
            EQuality qualityA = _a.getQuality();
            EQuality qualityB = _b.getQuality();

            if (isConsortA != isConsortB)
                return -(isConsortA.CompareTo(isConsortB));

            if (isHeroA != isHeroB)
                return -(isHeroA.CompareTo(isHeroB));

            if(qualityA != qualityB)
                return -(qualityA.CompareTo(qualityB));

            return _a.subId.CompareTo(_b.subId);
        }

        #region 点击事件

        /// <summary>
        /// 点击关闭按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_VIP_MAIN);
        }

        /// <summary>
        /// 点击领取奖励按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickGetReward(GameObject _go)
        {
            if (_getRewardType() != ECommonRewardType.CAN_GET_REWARD)
                return;

            VipRefObj curSelectVipRef = _m_wTabContainer?.curSelectItem?.vipRef;
            if (curSelectVipRef == null)
                return;

            //请求领奖
            NPPlayer.instance.playerInfoComp.reqDrawVipLevelReward(curSelectVipRef.vip_lvl, _isSuc =>
            {
                if (_isSuc)
                {
                    _refreshWnd();
                }
            });
        }

        //点击充值途径按钮
        private void _onClickAccess(GameObject _go)
        {
            GCommon.popItemAccessWays(ENPItemType.CURRENCY, (int)ECurrency.VIP_EXP);
        }

        //点击上一个
        private void _onClickPrevious(GameObject _go)
        {
            VipRefObj curSelectVipRef = _m_wTabContainer?.curSelectItem?.vipRef;
            if (curSelectVipRef == null)
                return;

            VipRefObj prVipRef = GRefdataCoreMgr.instance.vipRefCore.getRef(curSelectVipRef.vip_lvl - 1);
            if (prVipRef == null)
                return;

            _m_wTabContainer?.setSelectItem(curSelectVipRef.vip_lvl - 1, true);
        }

        //点击下一个
        private void _onClickNext(GameObject _go)
        {
            VipRefObj curSelectVipRef = _m_wTabContainer?.curSelectItem?.vipRef;
            if (curSelectVipRef == null)
                return;

            VipRefObj prVipRef = GRefdataCoreMgr.instance.vipRefCore.getRef(curSelectVipRef.vip_lvl + 1);
            if (prVipRef == null)
                return;

            _m_wTabContainer?.setSelectItem(curSelectVipRef.vip_lvl + 1, true);
        }

        //点击页签item
        private void _onClickTabItem(GGUIWndVIPTabContainerItem _item)
        {
            _refreshWnd(true);
        }
        //点击vip特权预览
        private void _onClickVipPreview(GameObject _go)
        {
            if (_m_wTabContainer != null && _m_wTabContainer.curSelectItem != null && _m_wTabContainer.curSelectItem.vipRef != null)
                GGUIWndVIPLevelPreview.instance.setStartSelectVIPLevel(_m_wTabContainer.curSelectItem.vipRef.vip_lvl);
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndVIPLevelPreview.instance, GGUIWndVIPLevelPreview.instance.showWnd, UINodeTagConst.C_VIP_LEVEL_PREVIEW);
        }

        #endregion
    }
}
