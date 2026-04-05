using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 任务界面
    /// </summary>
    public class GGUIWndQuestMain : _ANPGGUIBasicWnd<GGUIMonoQuestMain>
    {

        private static GGUIWndQuestMain _g_instance;
        public static GGUIWndQuestMain instance
        {
            get
            {
                if(_g_instance == null)
                    _g_instance = new GGUIWndQuestMain();
                return _g_instance;
            }
        }

        //页签列表
        private List<GGUIWndQuestMainTab> _m_lTabWndList;
        //当前选中的页签
        private GGUIWndQuestMainTab _m_wSelectTabWnd;
        //主线任务页面
        private GGUIWndQuestPage _m_wMainQuestPage;
        //功能预告页面
        private GGUIWndFuncPreviewPage _m_FuncPreviewPage;

        protected GGUIWndQuestMain() : base(EALUIWndLayer.ADDITION)
        {
        }

        /********************
       * 获取资源所在资源加载文件名称
       **/
        protected override string _monoAssetPath { get { return GGUIMonoQuestMain.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoQuestMain.objName; } }

        /**************
         * 获取用于加载资源的管理对象
         **/
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onShowWnd()
        {
            _refreshWnd();
            _playSpineAnimation();
        }

        protected override void _onHideWnd()
        {
            _m_wSelectTabWnd?.setSelected(false);

            _hideAllPage();

            //发送一下消息让一些自定义条件处理刷新
            GCommon.reloadCustomLoadPrefab();
        }

        protected override void _onReset()
        {
            if (_m_lTabWndList != null)
            {
                GGUIWndQuestMainTab tempTabItem = null;
                for (int i = 0; i < _m_lTabWndList.Count; ++i)
                {
                    tempTabItem = _m_lTabWndList[i];
                    if (tempTabItem == null)
                        continue;
                    tempTabItem.resetWnd();
                }
            }

            _m_wMainQuestPage?.resetWnd();
            _m_FuncPreviewPage?.resetWnd();
        }

        protected override void _onDiscard()
        {
            if (wnd == null)
                return;

            if (_m_lTabWndList != null)
            {
                GGUIWndQuestMainTab tempTabItem = null;
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

            _m_wSelectTabWnd?.setSelected(false);
            _m_wSelectTabWnd = null;

            _m_wMainQuestPage?.discard();
            _m_wMainQuestPage = null;

            _m_FuncPreviewPage?.discard();
            _m_FuncPreviewPage = null;
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            _m_lTabWndList = new List<GGUIWndQuestMainTab>();
            if (null != wnd.monoTabList)
            {
                GGUIQuestMainTabMono tempTabMono = null;
                GGUIWndQuestMainTab tempTabItem = null;
                for (int i = 0; i < wnd.monoTabList.Count; ++i)
                {
                    tempTabMono = wnd.monoTabList[i];
                    if (tempTabMono == null || tempTabMono.monoTab == null)
                        continue;
                    tempTabItem = new GGUIWndQuestMainTab(tempTabMono.monoTab, tempTabMono.tabType, tempTabMono.tabSubWndAssetId);
                    //初始化默认未选中
                    tempTabItem.setSelected(false);
                    //绑定点击事件
                    tempTabItem.onClickTab += _onTabSelect;
                    _m_lTabWndList.Add(tempTabItem);
                }
            }

            _m_wSelectTabWnd = null;
        }

        /// <summary>
        /// 选中指定页签
        /// </summary>
        /// <param name="_tab"></param>
        public void setSelectTab(EQuestMainTab _tab)
        {
            if (wnd == null || _m_lTabWndList == null)
                return;

            for (int i = 0; i < _m_lTabWndList.Count; i++)
            {
                if (_m_lTabWndList[i] == null)
                    continue;

                if (_m_lTabWndList[i].tabType == _tab)
                {
                    _onTabSelect(_m_lTabWndList[i]);
                    break;
                }
            }

        }

        //刷新窗口
        private void _refreshWnd()
        {
            if (wnd == null || _m_lTabWndList == null)
                return;

            //设置选中的页签
            if (_m_wSelectTabWnd == null)
                setSelectTab(wnd.defaultTab);
            else
            {
                _m_wSelectTabWnd.setSelected(true);
                _refreshTabView(_m_wSelectTabWnd.tabType);
            }

            //当前解锁数据
            FuncUnlockInfo funcUnlockInfo = NPPlayer.instance.funcUnlockComp.getFirstShowInfo();

            //如果功能已经全部解锁，则隐藏页签列表
            if (funcUnlockInfo == null)
            {
                ALUGUICommon.setGameObjEnable(wnd.goAllUnlockHideList, false);
                ALUGUICommon.setGameObjEnable(wnd.goAllUnlockShowList, true);
            }
            else
            {
                ALUGUICommon.setGameObjEnable(wnd.goAllUnlockHideList, true);
                ALUGUICommon.setGameObjEnable(wnd.goAllUnlockShowList, false);
            }

            //刷新红点
            NPPlayer.instance.funcUnlockComp.refreshRedTip();
        }

        //点击切换页签按钮
        private void _onTabSelect(GGUIWndQuestMainTab _tabItemWnd)
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
        private void _refreshTabView(EQuestMainTab _tabView)
        {
            _hideAllPage();

            switch (_tabView)
            {
                case EQuestMainTab.MAIN_QUEST:
                    _showMainQuest();
                    break;
                case EQuestMainTab.FUNC_PREVIEW:
                    _showFuncPreview();
                    break;
            }
        }

        /// <summary>
        /// 打开主线任务页面
        /// </summary>
        private void _showMainQuest()
        {
            if (wnd == null || _m_wSelectTabWnd == null)
                return;

            if (_m_wMainQuestPage == null)
            {
                _m_wMainQuestPage = new GGUIWndQuestPage(NPCommonAssetPathInfo.readFromUiResId(_m_wSelectTabWnd.subWndAssetId), wnd.pageParent);
                _m_wMainQuestPage.load();
            }

            _m_wMainQuestPage.regLoadDoneDelegate(()=>
            {
                if (_m_wSelectTabWnd == null || _m_wSelectTabWnd.tabType != EQuestMainTab.MAIN_QUEST || !isShow)
                    return;

                _m_wMainQuestPage?.showWnd();
            });
        }

        /// <summary>
        /// 打开功能预告页面
        /// </summary>
        private void _showFuncPreview()
        {
            if (wnd == null || _m_wSelectTabWnd == null)
                return;

            if (_m_FuncPreviewPage == null)
            {
                _m_FuncPreviewPage = new GGUIWndFuncPreviewPage(NPCommonAssetPathInfo.readFromUiResId(_m_wSelectTabWnd.subWndAssetId), wnd.pageParent);
                _m_FuncPreviewPage.load();
            }

            _m_FuncPreviewPage.regLoadDoneDelegate(() =>
            {
                if (_m_wSelectTabWnd == null || _m_wSelectTabWnd.tabType != EQuestMainTab.FUNC_PREVIEW || !isShow)
                    return;

                _m_FuncPreviewPage?.showWnd();
            });
        }

        /// <summary>
        /// 关闭所有页面
        /// </summary>
        private void _hideAllPage()
        {
            _m_wMainQuestPage?.hideWnd();
            _m_FuncPreviewPage?.hideWnd();
        }

        #region Spine动画

        /// <summary>
        /// 播放Spine动画
        /// </summary>
        private void _playSpineAnimation()
        {
            if (wnd == null || wnd.spineAnimation == null || wnd.spineAnimation.AnimationState == null || string.IsNullOrEmpty(wnd.spineStartName) || string.IsNullOrEmpty(wnd.spineIdleName))
                return;

            wnd.spineAnimation.AnimationState.SetAnimation(0, wnd.spineStartName, false);
            wnd.spineAnimation.AnimationState.AddAnimation(0, wnd.spineIdleName, true,0f);
        }

        #endregion
    }
}
