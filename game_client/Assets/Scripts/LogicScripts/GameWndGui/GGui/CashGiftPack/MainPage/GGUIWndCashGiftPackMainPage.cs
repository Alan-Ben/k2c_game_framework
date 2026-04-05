using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 现金礼包主页面
    /// </summary>
    public class GGUIWndCashGiftPackMainPage : _ATALBasicLoadPrefabSubUIWnd<GGUIMonoCashGiftPackMainPage>
    {
        //资源id
        private long _m_lUIResId;
        //礼包组列表
        private List<GiftPackGroupRefObj> _m_lGiftPackGroupList;
        //可显示的礼包组列表
        private List<GiftPackGroupRefObj> _m_lCanShowGroupList;
        //页签列表
        private GGUIWndCashGiftPackMainPageTabContainer _m_wTabContainer;
        //加载的页面字典，<资源id，页面类>
        [NotNull] private Dictionary<long, _AALBasicLoadUIWndBasicClass> _m_dPageDic = new Dictionary<long, _AALBasicLoadUIWndBasicClass>();

        public GGUIWndCashGiftPackMainPage(long _uiResId, Transform _parent) : base(_parent)
        {
            _m_lUIResId = _uiResId;
        }

        protected override string _monoAssetPath { get { return UIResPathAssistant.getAssetPath(_m_lUIResId); } }
        protected override string _monoObjName { get { return UIResPathAssistant.getObjName(_m_lUIResId); } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsg(WinMsgType.ON_COMMON_ACTIVITY_ADD, _onActivityStateChg);
            WinMsg.RegisterMsg(WinMsgType.ON_COMMON_ACTIVITY_CLOSE, _onActivityStateChg);
            WinMsg.RegisterMsg(WinMsgType.ON_COMMON_ACTIVITY_STATE_CHG, _onActivityStateChg);
            _refreshWnd();
        }
        
        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_COMMON_ACTIVITY_ADD, _onActivityStateChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_COMMON_ACTIVITY_CLOSE, _onActivityStateChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_COMMON_ACTIVITY_STATE_CHG, _onActivityStateChg);
            _m_wTabContainer?.hideWnd();
            _hideAll();
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
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoTabContainer != null)
            {
                _m_wTabContainer = new GGUIWndCashGiftPackMainPageTabContainer(wnd.monoTabContainer);
                _m_wTabContainer.onClickItem += _onClickTabItem;
            }
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_giftPackGroupRefLsit"></param>
        public void setInfo(List<GiftPackGroupRefObj> _giftPackGroupRefLsit)
        {
            _m_lGiftPackGroupList = _giftPackGroupRefLsit;
            _refreshWnd();
        }

        //刷新窗口
        private void _refreshWnd()
        {
            if (wnd == null || _m_lGiftPackGroupList == null)
                return;

            if (_m_lCanShowGroupList == null)
                _m_lCanShowGroupList = new List<GiftPackGroupRefObj>();
            _m_lCanShowGroupList.Clear();

            //获取可显示的礼包组列表
            for (int i = 0; i < _m_lGiftPackGroupList.Count; i++)
            {
                if (_m_lGiftPackGroupList[i] != null && 
                    (_m_lGiftPackGroupList[i].show_condition == null ||
                     _m_lGiftPackGroupList[i].show_condition.isEmpty ||
                     _m_lGiftPackGroupList[i].show_condition.IsEnable(null)))
                    _m_lCanShowGroupList.Add(_m_lGiftPackGroupList[i]);
            }

            //按id排序
            _m_lCanShowGroupList.Sort((_a,_b)=>_a.id.CompareTo(_b.id));
            //展示页签列表
            _m_wTabContainer?.showWnd();
            _m_wTabContainer?.showItemList(_m_lCanShowGroupList);

            //如果没有页签，隐藏所有页面
            if(_m_lCanShowGroupList.Count <= 0)
                _hideAll();
        }

        //隐藏所有页面
        private void _hideAll()
        {
            foreach (_AALBasicLoadUIWndBasicClass pageWnd in _m_dPageDic.Values)
            {
                pageWnd?.hideWnd();
            }
        }

        //点击页签
        private void _onClickTabItem(GGUIWndCashGiftPackMainPageTabContainerItem _item, bool _moveToTop)
        {
            if (wnd == null || wnd.pageParent == null)
                return;

            GiftPackGroupRefObj giftPackGroupRef = _item?.giftPackGroupRef;
            if (giftPackGroupRef == null)
                return;

            //先尝试获取已加载的页面
            _m_dPageDic.TryGetValue(giftPackGroupRef.page_ui_res_id, out _AALBasicLoadUIWndBasicClass pageWnd);

            //先隐藏所有页面
            _hideAll();

            //根据类型展示不同的页面
            switch (giftPackGroupRef.show_type)
            {
                case EGiftPackGroupShowType.ACTIVITY:
                case EGiftPackGroupShowType.WEEKLY:
                case EGiftPackGroupShowType.MARS_WEEKLY:
                case EGiftPackGroupShowType.RANK_RUSH:
                    GGUIWndCashGiftPackGroupDetailPage_Normal normalPage = pageWnd as GGUIWndCashGiftPackGroupDetailPage_Normal;
                    if (normalPage == null)
                    {
                        normalPage = new GGUIWndCashGiftPackGroupDetailPage_Normal(giftPackGroupRef.page_ui_res_id, wnd.pageParent);
                        normalPage.load(() =>
                        {
                            if (normalPage == null)
                                return;

                            normalPage.showWnd();
                            normalPage.setInfo(giftPackGroupRef, _moveToTop);
                        });
                        _m_dPageDic[giftPackGroupRef.page_ui_res_id] = normalPage;
                    }
                    else
                    {
                        normalPage.showWnd();
                        normalPage.setInfo(giftPackGroupRef, _moveToTop);
                    }
                    break;
                case EGiftPackGroupShowType.DAILY:
                case EGiftPackGroupShowType.MARS_DAILY:
                    GGUIWndCashGiftPackGroupDetailPage_Daily dailyPage = pageWnd as GGUIWndCashGiftPackGroupDetailPage_Daily;
                    if (dailyPage == null)
                    {
                        dailyPage = new GGUIWndCashGiftPackGroupDetailPage_Daily(giftPackGroupRef.page_ui_res_id, wnd.pageParent);
                        dailyPage.load(() =>
                        {
                            if (dailyPage == null)
                                return;

                            dailyPage.showWnd();
                            dailyPage.setInfo(giftPackGroupRef, _moveToTop);
                        });
                        _m_dPageDic[giftPackGroupRef.page_ui_res_id] = dailyPage;
                    }
                    else
                    {
                        dailyPage.showWnd();
                        dailyPage.setInfo(giftPackGroupRef, _moveToTop);
                    }
                    break;
                case EGiftPackGroupShowType.GUILD:
                    break;
            }
        }

        //活动状态变化
        private void _onActivityStateChg(params object[] _objects)
        {
            _refreshWnd();
        }
    }
}
