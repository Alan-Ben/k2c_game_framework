using ALPackage;
using NPEnum;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// VIP角色特殊奖励列表item
    /// </summary>
    public class GGUIWndVIPActorItemContainerItem : _ATALBasicUISubWnd<GGUIMonoVIPActorItemContainerItem>
    {
        //道具
        private NPCommonCostItem _m_item;
        //领奖状态
        private ECommonRewardType _m_eRewardType;
        //图片
        private NPGGuiWndTexture _m_wIconBodyWnd;
        //背景
        private NPGGuiWndTexture _m_wIconBgWnd;
        //相性图标
        private NPGGuiWndTexture _m_wIconSpecAttrWnd;

        public GGUIWndVIPActorItemContainerItem(GGUIMonoVIPActorItemContainerItem  _mono) : base(_mono)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wIconBodyWnd?.hideWnd();
            _m_wIconBgWnd?.hideWnd();
            _m_wIconSpecAttrWnd?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wIconBodyWnd?.discardTexture();
            _m_wIconBgWnd?.discardTexture();
            _m_wIconSpecAttrWnd?.discardTexture();
        }

        protected override void _onDiscard()
        {
            _m_wIconBodyWnd?.discard();
            _m_wIconBodyWnd = null;
            _m_wIconBgWnd?.discard();
            _m_wIconBgWnd = null;
            _m_wIconSpecAttrWnd?.discard();
            _m_wIconSpecAttrWnd = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClick, _onClickItem);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if(wnd.imgBody != null)
                _m_wIconBodyWnd = new NPGGuiWndTexture(wnd.imgBody);

            if(wnd.imgBg != null)
                _m_wIconBgWnd = new NPGGuiWndTexture(wnd.imgBg);

            if (wnd.imgHeroAttr != null)
                _m_wIconSpecAttrWnd = new NPGGuiWndTexture(wnd.imgHeroAttr);

            ALUGUICommon.combineBtnClick(wnd.btnClick, _onClickItem);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_item"></param>
        /// <param name="_rewardType"></param>
        public void setInfo(NPCommonCostItem _item, ECommonRewardType _rewardType)
        {
            _m_item = _item;
            _m_eRewardType = _rewardType;
            _refreshWnd();
        }

        //刷新窗口
        private void _refreshWnd()
        {
            if (wnd == null || _m_item == null)
                return;

            _m_wIconSpecAttrWnd?.hideWnd();

            NPGTextureIndex bodyIndex = null;
            NPGTextureIndex bgIndex = null;
            switch (_m_item.getItemType())
            {
                case ENPItemType.HERO:
                    HeroRefObj heroRef = GRefdataCoreMgr.instance.heroRefCore.getRef(_m_item.subId);
                    bodyIndex = heroRef?.card_image;

                    NPQualityExtRefObj heroQualityExtRef = GCommon.getQualityExtRefObj(ENPItemType.HERO, _m_item.subId);
                    bgIndex = heroQualityExtRef?.hero_card_bg;

                    //相性图标
                    if (null != _m_wIconSpecAttrWnd && heroRef != null)
                    {
                        BasicAttrRefObj basicAttrRef = GRefdataCoreMgr.instance.basicAttrRefCore.getRef((long)heroRef.spec_attr_type);
                        _m_wIconSpecAttrWnd.showWnd();
                        _m_wIconSpecAttrWnd.setTexture(basicAttrRef?.icon);
                    }

                    break;
                case ENPItemType.CONSORT:
                    GConsortRefObj consortRef = GRefdataCoreMgr.instance.consortRefCore.getRef(_m_item.subId);
                    bodyIndex = consortRef?.card_image;

                    NPQualityExtRefObj consortQualityExtRef = GCommon.getQualityExtRefObj(ENPItemType.CONSORT, _m_item.subId);
                    bgIndex = consortQualityExtRef?.consort_card_bg;
                    break;
            }

            //形象
            _m_wIconBodyWnd?.showWnd();
            _m_wIconBodyWnd?.setTexture(bodyIndex);

            //背景
            _m_wIconBgWnd?.showWnd();
            _m_wIconBgWnd?.setTexture(bgIndex);

            //显隐设置
            ALUGUICommon.setGameObjEnable(wnd.goHeroShowList, _m_item.getItemType() == ENPItemType.HERO);
            ALUGUICommon.setGameObjEnable(wnd.goConsortShowList, _m_item.getItemType() == ENPItemType.CONSORT);

            //领奖状态
            if (_m_eRewardType != ECommonRewardType.NONE)
                NPCommonEnumStatInfo<ECommonRewardType>.setStat(wnd.rewardStatList, _m_eRewardType);
        }

        //点击item
        private void _onClickItem(GameObject _go)
        {
            if (_m_item == null)
                return;

            switch (_m_item.getItemType())
            {
                case ENPItemType.HERO:
                    HeroInfo heroInfo = NPPlayer.instance.heroComponent.getHeroInfo(_m_item.subId);
                    HeroCardShowInfo showInfo = new HeroCardShowInfo(heroInfo, GRefdataCoreMgr.instance.heroRefCore.getRef(_m_item.subId));
                    if (heroInfo == null)
                        QueueMgr.instance.AddNode(new GMainQueueHeroLockInfoNode(showInfo, null));
                    else
                        QueueMgr.instance.AddNode(new GMainQueueHeroInfoNode(showInfo, null));
                    break;
                case ENPItemType.CONSORT:
                    GGottenConsortInfo consortInfo = NPPlayer.instance.consortComp.getConsortInfo(_m_item.subId);
                    if (consortInfo == null)
                        QueueMgr.instance.AddNode(new GNodeLockConsortDetail(new ConsortRefShowInfo(_m_item.subId)));
                    else
                        GNodeUnLockConsortDetail.addConsortNode(new List<GGottenConsortInfo>() { consortInfo }, consortInfo.consortId);
                    break;
            }
        }
    }
}