using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 通用活动礼包界面
    /// </summary>
    public class GGUIWndActivityGiftPack : _ANPGGUIBasicWnd<GGUIMonoActivityGiftPack>
    {
        private static GGUIWndActivityGiftPack _g_instance;
        public static GGUIWndActivityGiftPack instance
        {
            get
            {
                if(_g_instance == null)
                    _g_instance = new GGUIWndActivityGiftPack();
                return _g_instance;
            }
        }

        //活动信息
        private _ABaseActivityInfo _m_activityInfo;
        //页签列表
        private GGUIWndActivityGiftPackTabContainer _m_wTabContainer;
        //加载的页面字典，<资源id，页面类>
        [NotNull] private Dictionary<long, _AALBasicLoadUIWndBasicClass> _m_dPageDic = new Dictionary<long, _AALBasicLoadUIWndBasicClass>();
        //钻石礼包页面资源ID
        private const long CRYSTAL_GIFT_PACK_PAGE_RES_ID = 6003;
        //现金礼包页面资源ID
        private const long CASH_GIFT_PACK_PAGE_RES_ID = 6004;

        public GGUIWndActivityGiftPack() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoActivityGiftPack.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoActivityGiftPack.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        protected override bool isShowAniPlayOnlyOne { get { return true; } }

        protected override void _onShowWnd()
        {
            if(_m_wTabContainer != null && _m_wTabContainer.curSelectItem != null)
                _refreshTabView(_m_wTabContainer.curSelectItem.giftPackTabType, _m_wTabContainer.curSelectItem.targetId);
        }

        protected override void _onHideWnd()
        {
            _m_wTabContainer?.hideWnd();
            _hideAllPage();
        }

        protected override void _onReset()
        {
            _m_wTabContainer?.resetWnd();

            foreach (_AALBasicLoadUIWndBasicClass pageWnd in _m_dPageDic.Values)
            {
                pageWnd?.resetWnd();
            }
        }

        protected override void _onDiscard()
        {
            _m_wTabContainer?.discard();
            _m_wTabContainer = null;

            foreach (_AALBasicLoadUIWndBasicClass pageWnd in _m_dPageDic.Values)
            {
                pageWnd?.discard();
            }
            _m_dPageDic.Clear();

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoTabContainer != null)
            {
                _m_wTabContainer = new GGUIWndActivityGiftPackTabContainer(wnd.monoTabContainer);
                _m_wTabContainer.onClickItem += _onClickTabItem;
            }


            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_activityId"></param>
        public void setInfo(long _activityId)
        {
            if (wnd == null)
                return;

            //取最后一个活动
            _m_activityInfo = NPPlayer.instance.commonActivityComp.getValidActivityInfoByActivityId(_activityId);

            _m_wTabContainer?.showWnd();
            _m_wTabContainer?.showItemList(_m_activityInfo);
        }

        //点击页签
        private void _onClickTabItem(GGUIWndActivityGiftPackTabContainerItem _item)
        {
            if (_item == null)
                return;

            _refreshTabView(_item.giftPackTabType, _item.targetId);
        }

        //根据页签刷新列表内容
        private void _refreshTabView(EActivityGiftPackTabType _tabView, long _id)
        {
            _hideAllPage();

            switch (_tabView)
            {
                case EActivityGiftPackTabType.CRYSTAL://钻石礼包
                    _showCrystalGiftPackPage();
                    break;
                case EActivityGiftPackTabType.CASH://现金礼包
                    _showCashGiftPackPage(_id);
                    break;
            }
        }

        //显示钻石礼包页面
        private void _showCrystalGiftPackPage()
        {
            if (wnd == null || _m_activityInfo == null)
                return;

            //先尝试获取已加载的页面
            _m_dPageDic.TryGetValue(CRYSTAL_GIFT_PACK_PAGE_RES_ID, out _AALBasicLoadUIWndBasicClass pageWnd);

            //展示钻石礼包页面
            if (pageWnd is GGUIWndActivityCrystalGiftPackPage crystalGiftPackPage)
            {
                crystalGiftPackPage.showWnd();
                crystalGiftPackPage.setInfo(_m_activityInfo);
            }
            else
            {
                crystalGiftPackPage = new GGUIWndActivityCrystalGiftPackPage(UIResPathAssistant.getAssetInfo(CRYSTAL_GIFT_PACK_PAGE_RES_ID), wnd.pageParent);
                crystalGiftPackPage.load(() =>
                {
                    if (crystalGiftPackPage == null)
                        return;
                    crystalGiftPackPage.showWnd();
                    crystalGiftPackPage.setInfo(_m_activityInfo);
                });
                _m_dPageDic[CRYSTAL_GIFT_PACK_PAGE_RES_ID] = crystalGiftPackPage;
            }
        }

        //显示现金礼包页面
        private void _showCashGiftPackPage(long _cashGiftPackId)
        {
            if (wnd == null || _m_activityInfo == null)
                return;

            //先尝试获取已加载的页面
            _m_dPageDic.TryGetValue(CASH_GIFT_PACK_PAGE_RES_ID, out _AALBasicLoadUIWndBasicClass pageWnd);

            //展示现金礼包页面
            if (pageWnd is GGUIWndActivityCashGiftPackPage cashGiftPackPage)
            {
                cashGiftPackPage.showWnd();
                cashGiftPackPage.setInfo(_m_activityInfo, _cashGiftPackId);
            }
            else
            {
                cashGiftPackPage = new GGUIWndActivityCashGiftPackPage(UIResPathAssistant.getAssetInfo(CASH_GIFT_PACK_PAGE_RES_ID), wnd.pageParent);
                cashGiftPackPage.load(() =>
                {
                    if (cashGiftPackPage == null)
                        return;
                    cashGiftPackPage.showWnd();
                    cashGiftPackPage.setInfo(_m_activityInfo, _cashGiftPackId);
                });
                _m_dPageDic[CASH_GIFT_PACK_PAGE_RES_ID] = cashGiftPackPage;
            }
        }

        //关闭所有页面
        private void _hideAllPage()
        {
            foreach (_AALBasicLoadUIWndBasicClass pageWnd in _m_dPageDic.Values)
            {
                pageWnd?.hideWnd();
            }
        }

        //点击关闭
        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_ACTIVITY_GIFT_PACK);
        }
    }
}