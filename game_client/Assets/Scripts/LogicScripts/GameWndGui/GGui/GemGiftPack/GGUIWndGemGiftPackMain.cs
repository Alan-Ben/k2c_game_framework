using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 钻石礼包主界面
    /// </summary>
    public class GGUIWndGemGiftPackMain : _ANPGGUIBasicResBarWnd<GGUIMonoGemGiftPackMain>
    {
        private static GGUIWndGemGiftPackMain _g_instance;
        public static GGUIWndGemGiftPackMain instance
        {
            get
            {
                if(_g_instance == null)
                    _g_instance = new GGUIWndGemGiftPackMain();
                return _g_instance;
            }
        }

        //页签列表
        private List<GGUIWndGemGiftPackMainTab> _m_lTabWndList;
        //当前选中的页签
        private GGUIWndGemGiftPackMainTab _m_wSelectTabWnd;
        //钻石商店页面
        private GGUIWndGemGiftPackActivityPage _m_wActivityGemPage;

        public GGUIWndGemGiftPackMain() : base(EALUIWndLayer.NORMAL)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoGemGiftPackMain.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoGemGiftPackMain.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override bool isShowAniPlayOnlyOne { get { return true; } }
        public override bool needDiscardOnSwitch { get { return true; } }

        protected override void _onShowWnd()
        {
            if(wnd == null)
                return;

            setInfo(wnd.defaultTab);
        }

        protected override void _onHideWnd()
        {
            _hideAllPage();
        }

        protected override void _onReset()
        {
            _m_wActivityGemPage?.resetWnd();
        }

        protected override void _onDiscard()
        {
            if (_m_lTabWndList != null)
            {
                GGUIWndGemGiftPackMainTab tempTabItem = null;
                for (int i = 0; i < _m_lTabWndList.Count; ++i)
                {
                    tempTabItem = _m_lTabWndList[i];
                    if (tempTabItem == null)
                        continue;
                    tempTabItem.discard();
                }
                _m_lTabWndList.Clear();
                _m_lTabWndList = null;
            }

            _m_wActivityGemPage?.discard();
            _m_wActivityGemPage = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            _m_lTabWndList = new List<GGUIWndGemGiftPackMainTab>();
            if (null != wnd.monoTabList)
            {
                GGUIGemGiftPackMainTabMono tempTabMono = null;
                GGUIWndGemGiftPackMainTab tempTabItem = null;
                for (int i = 0; i < wnd.monoTabList.Count; ++i)
                {
                    tempTabMono = wnd.monoTabList[i];
                    if (tempTabMono == null)
                        continue;
                    tempTabItem = new GGUIWndGemGiftPackMainTab(tempTabMono.monoTab, tempTabMono.tabType);
                    //初始化默认未选中
                    tempTabItem.setSelected(false);
                    //绑定点击事件
                    tempTabItem.onClickTab += _onTabSelect;
                    _m_lTabWndList.Add(tempTabItem);
                }
            }

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
        }

        /// <summary>
        /// 设置选中页签
        /// </summary>
        /// <param name="_type"></param>
        public void setInfo(EGemGiftPackMainTabType _type)
        {
            foreach (GGUIWndGemGiftPackMainTab itemTab in _m_lTabWndList)
            {
                if (itemTab.tabType == _type)
                {
                    _onTabSelect(itemTab);
                    break;
                }
            }
        }

        //点击切换页签按钮
        private void _onTabSelect(GGUIWndGemGiftPackMainTab _tabItemWnd)
        {
            if (null == _tabItemWnd || _m_wSelectTabWnd == _tabItemWnd)
                return;

            //取消原来的选择
            if (null != _m_wSelectTabWnd)
                _m_wSelectTabWnd.setSelected(false);

            //设置新对象
            _m_wSelectTabWnd = _tabItemWnd;

            if (null != _m_wSelectTabWnd)
            {
                _m_wSelectTabWnd.setSelected(true);

                //根据页签刷新列表内容
                _refreshTabView(_m_wSelectTabWnd.tabType);
            }
        }

        //根据页签刷新列表内容
        private void _refreshTabView(EGemGiftPackMainTabType _tabView)
        {
            _hideAllPage();
            switch (_tabView)
            {
                case EGemGiftPackMainTabType.ACTIVITY_GEM:
                    _showActivityGemPage();
                    break;
            }
        }

        //显示钻石商店页面
        private void _showActivityGemPage()
        {
            if (wnd == null)
                return;

            if (_m_wActivityGemPage != null)
                _m_wActivityGemPage.showWnd();
            else
            {
                _m_wActivityGemPage = new GGUIWndGemGiftPackActivityPage(_getPageAssetPathIdByType(EGemGiftPackMainTabType.ACTIVITY_GEM), wnd.pageParent);
                _m_wActivityGemPage.load(() =>
                {
                    if (_m_wActivityGemPage == null)
                        return;
                    _m_wActivityGemPage.showWnd();
                });
            }
        }

        //隐藏所有页面
        private void _hideAllPage()
        {
            _m_wActivityGemPage?.hideWnd();
        }

        //根据页签类型获取对应的子页面的加载路径
        private long _getPageAssetPathIdByType(EGemGiftPackMainTabType _type)
        {
            if (wnd == null || wnd.monoTabList == null)
                return 0;

            foreach (GGUIGemGiftPackMainTabMono mono in wnd.monoTabList)
            {
                if (mono.tabType == _type)
                    return mono.resId;
            }

            return 0;
        }

        //点击关闭按钮
        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_GEM_GIFT_PACK_MAIN);
        }
    }
}