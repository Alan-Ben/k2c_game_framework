using System.Collections.Generic;
using UnityEngine;
using ALPackage;

namespace GOE
{
    // 玩家装扮
    public class GGUIWndPlayerInfoDress : _ANPGGUIBasicWnd<GGUIMonoPlayerInfoDress>
    {

        private static GGUIWndPlayerInfoDress _g_instance;
        public static GGUIWndPlayerInfoDress instance
        {
            get
            {
                if (_g_instance == null)
                    _g_instance = new GGUIWndPlayerInfoDress();
                return _g_instance;
            }
        }

        //当前选中的type
        private ENPDressTabType _m_eCurrentType;
        //tab 列表
        private List<GGUIWndDressTab> _m_tabList;
        //用于管理子窗口的容器
        private ALEmptyContainerScene _m_cSubTabWndContainer;
        //视图列表
        private GGUIWndPlayerInfoDressIconPage _m_dressIconPage;
        private GGUIWndPlayerInfoDressIconBgkPage _m_dressIconBgkPage;
        private GGUIWndPlayerInfoDressBubblePage _m_dressBubblePage;

        protected GGUIWndPlayerInfoDress() : base(EALUIWndLayer.ADDITION)
        {
        }

        /********************
       * 获取资源所在资源加载文件名称
       **/
        protected override string _monoAssetPath { get { return GGUIMonoPlayerInfoDress.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoPlayerInfoDress.objName; } }

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
            _m_eCurrentType = wnd.defaultShow;
            _refreshAll();
        }

        protected override void _onHideWnd()
        {
            if (null != _m_cSubTabWndContainer)
                _m_cSubTabWndContainer.hideScene(null);
        }

        protected override void _onReset()
        {
            if (null != _m_dressIconPage)
                _m_dressIconPage.resetWnd();
            if (null != _m_dressIconBgkPage)
                _m_dressIconBgkPage.resetWnd();
            if (null != _m_dressBubblePage)
                _m_dressBubblePage.resetWnd();
        }

        protected override void _onDiscard()
        {
            if (null != _m_cSubTabWndContainer)
                _m_cSubTabWndContainer.quitScene();
            _m_cSubTabWndContainer = null;

            //由于窗口是添加到容器_m_cSubTabWndContainer管理，因此这里窗口直接重置即可
            _m_dressIconPage = null;
            _m_dressIconBgkPage = null;
            _m_dressBubblePage = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.closeBtn, _closeBtnDidClick);
        }

        protected override void _onWndInitDone()
        {
            if (null == wnd)
                return;

            //创建容器Scene
            _m_cSubTabWndContainer = new ALEmptyContainerScene(0);
            _m_cSubTabWndContainer.enterScene();

            //创建tab
            _m_tabList = new List<GGUIWndDressTab>();
            if (null != wnd.monoTabList)
            {
                for (int i = 0; i < wnd.monoTabList.Count; i++)
                {
                    GGUIDressTabMono tabMono = wnd.monoTabList[i];
                    if (null == tabMono)
                        continue;

                    GGUIWndDressTab tabWnd = new GGUIWndDressTab(tabMono.monoTab, tabMono.tabType);
                    tabWnd.setSelected(false);
                    tabWnd.onClickTab += _tabDidClick;

                    _m_tabList.Add(tabWnd);
                }
            }

            ALUGUICommon.combineBtnClick(wnd.closeBtn, _closeBtnDidClick);

            //设置默认type
            _m_eCurrentType = wnd.defaultShow;
        }

        /// <summary>
        /// 设置显示的页签
        /// </summary>
        /// <param name="_type"></param>
        public void setShowType(ENPDressTabType _type)
        {
            //样式一致则不处理
            if (_m_eCurrentType == _type)
                return;

            _m_eCurrentType = _type;

            //刷新
            _refreshAll();
        }

        /// <summary>
        /// 刷新显示处理
        /// </summary>
        /// <param name="_type"></param>
        private void _refreshAll()
        {
            //显示当前数据
            _refreshTab();
            _refreshPage();
        }

        private void _refreshPage()
        {
            if (wnd == null)
                return;

            switch (_m_eCurrentType)
            {
                case ENPDressTabType.ICON:
                    _showDressIconPage();
                    ALUGUICommon.setLabelTxt(wnd.txtTitle, TextTranslate.instance.getLanguage(TransKeyConst.playerInfo_icon_none));
                    break;
                case ENPDressTabType.ICON_BGK:
                    _showDressIconBgkPage();
                    ALUGUICommon.setLabelTxt(wnd.txtTitle, TextTranslate.instance.getLanguage(TransKeyConst.playerInfo_iconBgk_none));
                    break;
                case ENPDressTabType.BUBBLE:
                    _showDressBubblePage();
                    ALUGUICommon.setLabelTxt(wnd.txtTitle, TextTranslate.instance.getLanguage(TransKeyConst.playerInfo_bubble_none));
                    break;
            }
        }
        private void _refreshTab()
        {
            for (int i = 0; i < _m_tabList.Count; i++)
            {
                GGUIWndDressTab tab = _m_tabList[i];
                tab?.setSelected(tab.tabType == _m_eCurrentType);
            }
        }

        /// <summary>
        /// 切换头像视图处理
        /// </summary>
        /// 
        private void _showDressIconPage()
        {
            //如果无对象则创建对象
            if (_m_dressIconPage == null)
            {
                long assestPathInfoId = _getAssestPathInfo(ENPDressTabType.ICON);
                if (wnd != null && assestPathInfoId != 0)
                {
                    if (null == wnd.pageParentPos)
                    {
                        Debug.LogError("NPGGUIWndPlayerInfoDress上 父节点没挂载");
                        return;
                    }
                    _m_dressIconPage = new GGUIWndPlayerInfoDressIconPage(assestPathInfoId, wnd.pageParentPos);
                }
            }
            //通过视图切换主视图
            _m_cSubTabWndContainer?.showMainWnd(_m_dressIconPage);
        }

        /// <summary>
        /// 切换头像框视图处理
        /// </summary>
        private void _showDressIconBgkPage()
        {
            //如果无对象则创建对象
            if (_m_dressIconBgkPage == null)
            {
                long assestPathInfoId = _getAssestPathInfo(ENPDressTabType.ICON_BGK);
                if (wnd != null && assestPathInfoId != 0)
                {
                    if (null == wnd.pageParentPos)
                    {
                        Debug.LogError("NPGGUIWndPlayerInfoDress上 父节点没挂载");
                        return;
                    }
                    _m_dressIconBgkPage = new GGUIWndPlayerInfoDressIconBgkPage(assestPathInfoId, wnd.pageParentPos);
                }
            }
            //通过视图切换主视图
            _m_cSubTabWndContainer?.showMainWnd(_m_dressIconBgkPage);
        }

        /// <summary>
        /// 切换气泡框视图处理
        /// </summary>
        private void _showDressBubblePage()
        {
            if (_m_dressBubblePage == null)
            {
                long assestPathInfoId = _getAssestPathInfo(ENPDressTabType.BUBBLE);
                if (wnd != null && assestPathInfoId != 0)
                {
                    if (null == wnd.pageParentPos)
                    {
                        Debug.LogError("NPGGUIWndPlayerInfoDress上 父节点没挂载");
                        return;
                    }
                    _m_dressBubblePage = new GGUIWndPlayerInfoDressBubblePage(assestPathInfoId, wnd.pageParentPos);
                }
            }

            //通过视图切换主视图
            _m_cSubTabWndContainer?.showMainWnd(_m_dressBubblePage);
        }

        /// <summary>
        /// 获取窗口中某一类型的子页面加载路径
        /// </summary>
        /// <param name="_type"></param>
        /// <returns></returns>
        private long _getAssestPathInfo(ENPDressTabType _type)
        {
            if (null != wnd.monoTabList)
            {
                for (int i = 0; i < wnd.monoTabList.Count; i++)
                {
                    GGUIDressTabMono tabMono = wnd.monoTabList[i];
                    if (tabMono != null && tabMono.tabType == _type)
                        return tabMono.assestPathInfoId;
                }
            }
            return 0;
        }

        #region 点击事件

        //点击tab
        private void _tabDidClick(GGUIWndDressTab _tab)
        {
            //进入一个操作节点
            QueueMgr.instance.addNode_OnlyOp(() => { setShowType(_tab.tabType); }, UINodeTagConst.C_OP_PlayerInfo_Tab);
        }

        //点击关闭按钮
        private void _closeBtnDidClick(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst_PlayerInfo.C_MAIN_PLAYERINFO_DRESS_NODE);
        }
        #endregion
    }
}
