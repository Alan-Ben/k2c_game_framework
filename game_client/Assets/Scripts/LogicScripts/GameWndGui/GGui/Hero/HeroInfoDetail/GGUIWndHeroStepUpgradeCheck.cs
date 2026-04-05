using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 伙伴升阶确认界面
    /// </summary>
    public class GGUIWndHeroStepUpgradeCheck : _ANPGGUIBasicWnd<GGUIMonoHeroStepUpgradeCheck>
    {
        private static GGUIWndHeroStepUpgradeCheck _g_instance;
        public static GGUIWndHeroStepUpgradeCheck instance
        {
            get
            {
                if(_g_instance == null)
                    _g_instance = new GGUIWndHeroStepUpgradeCheck();
                return _g_instance;
            }
        }

        //伙伴信息
        private HeroInfo _m_heroInfo;
        //伙伴卡牌展示
        public GGUIWndHeroCommonCardItem _m_wHeroCard;
        //消耗列表
        private NPGGUIWndCommonItemContainer _m_wCostItemContainer;
        //是否正在处理升阶
        private bool _m_bIsDealingStepUp;

        public GGUIWndHeroStepUpgradeCheck() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoHeroStepUpgradeCheck.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoHeroStepUpgradeCheck.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsg(WinMsgType.ON_BAG_ITEM_ADD, _onBagItemChg);
            WinMsg.RegisterMsg(WinMsgType.ON_BAG_ITEM_UPDATE, _onBagItemChg);
            _m_bIsDealingStepUp = false;
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_BAG_ITEM_ADD, _onBagItemChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_BAG_ITEM_UPDATE, _onBagItemChg);

            if (_m_wCostItemContainer != null)
              _m_wCostItemContainer.hideWnd();

            if(_m_wHeroCard != null)
                _m_wHeroCard.hideWnd();

            _m_bIsDealingStepUp = false;
        }

        protected override void _onReset()
        {
            if(_m_wCostItemContainer != null)
                _m_wCostItemContainer.resetWnd();

            if (_m_wHeroCard != null)
                _m_wHeroCard.resetWnd();
        }

        protected override void _onDiscard()
        {
            if(null != _m_wCostItemContainer)
                _m_wCostItemContainer.discard();
            _m_wCostItemContainer = null;

            if (_m_wHeroCard != null)
                _m_wHeroCard.discard();
            _m_wHeroCard = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
            ALUGUICommon.uncombineBtnClick(wnd.btnStepUpgrade, _onClickStepUpgrade);
        }

        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
            
            if(wnd.monoStepCostUpgrade != null)
                _m_wCostItemContainer = new NPGGUIWndCommonItemContainer(wnd.monoStepCostUpgrade);

            if (wnd.monoHeroCard != null)
                _m_wHeroCard = new GGUIWndHeroCommonCardItem(wnd.monoHeroCard);

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
            ALUGUICommon.combineBtnClick(wnd.btnStepUpgrade, _onClickStepUpgrade);
        }
        
        //设置信息
        public void setInfo(HeroInfo _heroInfo)
        {
            if(null == _heroInfo)
                return;

            _m_heroInfo = _heroInfo;

            _refreshWnd();
        }

        //刷新窗口
        private void _refreshWnd()
        {
            _refreshHeroCard();
            _refreshNextValue();
            _refreshCost();
        }

        //刷新伙伴卡牌展示
        private void _refreshHeroCard()
        {
            if (null == wnd || null == _m_heroInfo)
                return;

            //伙伴卡牌展示
            if (_m_wHeroCard != null)
            {
                _m_wHeroCard.showWnd();
                _m_wHeroCard.setInfo(_m_heroInfo);
            }
        }

        //刷新升阶值变化
        private void _refreshNextValue()
        {
            if (null == wnd || null == _m_heroInfo || _m_heroInfo.nextHeroStepRef == null || _m_heroInfo.curHeroStepRef == null || _m_heroInfo.heroRefObj == null)
                return;

            //等级上限
            ALUGUICommon.setLabelTxt(wnd.txtCurLevelLimit, TextTranslate.instance.getLanguage(TransKeyConst.common_level_num, _m_heroInfo.curHeroStepRef.level_limit));
            ALUGUICommon.setLabelTxt(wnd.txtNextLevelLimit, _m_heroInfo.nextHeroStepRef != null ? TextTranslate.instance.getLanguage(TransKeyConst.common_level_num, _m_heroInfo.nextHeroStepRef.level_limit) : "");
            //资质值
            long curTalent = _m_heroInfo.getTotalTalent();
            ALUGUICommon.setLabelTxt(wnd.txtCurTalent, curTalent);

            //计算升阶带来的资质技能变更
            curTalent += HeroCommon.calStepTalentSkillDifferentValueWithLevelDiffer(_m_heroInfo, 1);
            ALUGUICommon.setLabelTxt(wnd.txtNextTalent, curTalent);
        }

        //刷新消耗
        private void _refreshCost()
        {
            List<NPCommonCostItem> commonCostItems = _m_heroInfo.curHeroStepRef.cost_item_list;
            if (null == commonCostItems)
                return;

            // 消耗列表
            if (null != _m_wCostItemContainer)
            {
                _m_wCostItemContainer.showWnd();
                _m_wCostItemContainer.showItemList(commonCostItems.toCommonItemDataList());
            }
        }
        
        //点击关闭
        private void _onClickClose(GameObject _gameObject)
        {
            if(null == _m_heroInfo)
                return;
            
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_HERO_STEP_UPGRADE);
        }
        
        //点击升阶
        private void _onClickStepUpgrade(GameObject _gameObject)
        {
            if(null == _m_heroInfo || null == _m_heroInfo.curHeroStepRef || _m_bIsDealingStepUp)
                return;

            //判断物品是否足够
            if (GCommon.isItemEnough(_m_heroInfo.curHeroStepRef.cost_item_list, true))
            {
                _m_bIsDealingStepUp = true;
                NPPlayer.instance.heroComponent.reqHeroStepUpgrade(_m_heroInfo.id, () =>
                {
                    _m_bIsDealingStepUp = false;
                    QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_HERO_STEP_UPGRADE);
                    //打开结算弹窗
                    QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndHeroStepUpgradeSuc.instance, () =>
                    {
                        GGUIWndHeroStepUpgradeSuc.instance.showWnd();
                        GGUIWndHeroStepUpgradeSuc.instance.setInfo(_m_heroInfo);
                    },UINodeTagConst.C_HERO_STEP_UPGRADE_SUC);
                });
            }
        }

        //背包物品变更
        private void _onBagItemChg(params object[] _objects)
        {
            _refreshCost();
        }
    }
}