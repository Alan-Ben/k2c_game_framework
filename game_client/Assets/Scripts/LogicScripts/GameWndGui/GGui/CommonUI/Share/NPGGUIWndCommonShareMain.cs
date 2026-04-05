using System;
using ALPackage;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    public enum ENPShareType
    {
        NONE,
        PARTY,
        Machine,
    }
    
    /// <summary>
    /// 分享界面
    /// </summary>
    public class NPGGUIWndCommonShareMain : _ANPGGUIBasicWnd<NPGGUIMonoCommonShareMain>
    {
        private static NPGGUIWndCommonShareMain _g_instance;
        public static NPGGUIWndCommonShareMain instance
        {
            get
            {
                if (_g_instance == null)
                    _g_instance = new NPGGUIWndCommonShareMain();
                return _g_instance;
            }
        }

        private List<NPGGUIWndCommonShareTab> _m_lTabWndList;//页签列表
        private NPGGUIWndCommonShareTab _m_wSelectTabWnd;//当前选中的页签
        private NPGGUIWndCommonShareFriendPage _m_wFriendPage;//好友分享页签
        private Action<long,Action> _m_shareAction;
        private string _m_titleStr;
        private long _m_lastShareTimeS;
        private List<long> _m_hasShareFriend;
        private ENPShareType _m_shareType;

        private NPGGUIWndCommonShareMain() : base(EALUIWndLayer.ADDITION)
        {

        }

        protected override string _monoAssetPath { get { return NPGGUIMonoCommonShareMain.assetPath; } }
        protected override string _monoObjName { get { return NPGGUIMonoCommonShareMain.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onShowWnd()
        {
            _refreshAll();
        }

        protected override void _onHideWnd()
        {
            _m_wSelectTabWnd = null;
            _hideAllPage();
        }

        protected override void _onReset()
        {
            _m_shareAction = null;

            if (_m_lTabWndList != null)
            {
                for (int i = 0; i < _m_lTabWndList.Count; i++)
                {
                    _m_lTabWndList[i]?.resetWnd();
                }
            }
            _resetAllPage();
        }

        protected override void _onDiscard()
        {
            if (wnd == null)
                return;

            _m_shareAction = null;
            _m_lastShareTimeS = 0;
            _m_hasShareFriend.Clear();
            _m_hasShareFriend = null;

            if (_m_lTabWndList != null)
            {
                for (int i = 0; i < _m_lTabWndList.Count; i++)
                {
                    NPGGUIWndCommonShareTab temp = _m_lTabWndList[i];
                    if (temp == null)
                        continue;

                    temp.onClickTab -= _onTabSelect;
                    temp.discard();
                }
                _m_lTabWndList.Clear();
            }
            _m_lTabWndList = null;
            _discardAllPage();

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickBtnClose);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            _m_hasShareFriend = new List<long>();
            //页签列表
            if (wnd.tabList != null)
            {
                _m_lTabWndList = new List<NPGGUIWndCommonShareTab>();
                for (int i = 0; i < wnd.tabList.Count; i++)
                {
                    NPGGUICommonShareTabParam temp = wnd.tabList[i];
                    if (temp.monoTab == null)
                        continue;
                    NPGGUIWndCommonShareTab tabWnd = new NPGGUIWndCommonShareTab(temp.monoTab, temp.shareTabType);
                    tabWnd.onClickTab += _onTabSelect;
                    _m_lTabWndList.Add(tabWnd);
                }
            }

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickBtnClose);
        }


        #region 点击事件

        /// <summary>
        /// 点击关闭按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickBtnClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_ADD_COMMON_SHARE);
        }

        /// <summary>
        /// 选中页签
        /// </summary>
        /// <param name="_tabWnd"></param>
        private void _onTabSelect(NPGGUIWndCommonShareTab _tabWnd)
        {
            if (_tabWnd == null || _m_wSelectTabWnd == _tabWnd)
                return;

            //联盟暂不支持
            if (_tabWnd.tabType == ENPGGUICommonShareTabType.ALLIANCE)
            {
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.common_sysUnOpen_none);
                return;
            }

            //取消原来的选择
            _m_wSelectTabWnd?.setSelected(false);
            //设置新对象
            _m_wSelectTabWnd = _tabWnd;
            _m_wSelectTabWnd.setSelected(true);
            _m_wSelectTabWnd.showWnd();

            //根据页签刷新列表内容
            _refreshTabView(_m_wSelectTabWnd.tabType);
        }

        #endregion


        #region 窗体事件

        /// <summary>
        /// 刷新
        /// </summary>
        private void _refreshAll()
        {
            if (null == wnd)
                return;
            ALUGUICommon.setLabelTxt(wnd.txtTitle, _m_titleStr);
            _refreshPageWnd();
        }

        /// <summary>
        /// 刷新页签相关显示
        /// </summary>
        private void _refreshPageWnd()
        {
            if (wnd == null)
                return;

            if (_m_wSelectTabWnd == null && _m_lTabWndList != null)
            {
                //选中默认页签
                for (int i = 0; i < _m_lTabWndList.Count; i++)
                {
                    NPGGUIWndCommonShareTab temp = _m_lTabWndList[i];
                    if (temp == null)
                        continue;

                    if (temp.tabType == wnd.defaultShareTab)
                    {
                        _onTabSelect(temp);
                        break;
                    }
                }
            }
            else
            {
                _m_wSelectTabWnd.setSelected(true);
                _refreshTabView(_m_wSelectTabWnd.tabType);
            }
        }

        /// <summary>
        /// 隐藏所有页面
        /// </summary>
        private void _hideAllPage()
        {
            _m_wFriendPage?.hideWnd();
        }

        /// <summary>
        /// 重置所有页面
        /// </summary>
        private void _resetAllPage()
        {
            _m_wFriendPage?.resetWnd();
        }

        /// <summary>
        /// 销毁所有页面
        /// </summary>
        private void _discardAllPage()
        {
            _m_wFriendPage?.discard();
            _m_wFriendPage = null;
        }

        /// <summary>
        /// 刷新不同页签显示
        /// </summary>
        /// <param name="_shareTabType"></param>
        private void _refreshTabView(ENPGGUICommonShareTabType _shareTabType)
        {
            _hideAllPage();

            switch (_shareTabType)
            {
                //好友分享
                case ENPGGUICommonShareTabType.FRIEND:
                    _showFriendPage();
                    break;
            }
        }

        /// <summary>
        /// 好友分享
        /// </summary>
        private void _showFriendPage()
        {
            if (wnd == null)
                return;

            if (_m_wFriendPage != null)
            {
                _m_wFriendPage.setInfo(_m_shareType,_shareAction,_isOnCDFunc);
                _m_wFriendPage.showWnd();
            }
            else
            {
                _m_wFriendPage = new NPGGUIWndCommonShareFriendPage(_getPageAssetPathByType(ENPGGUICommonShareTabType.FRIEND), wnd.pageParent);
                _m_wFriendPage.load(() =>
                {
                    if (_m_wFriendPage == null)
                        return;

                    _m_wFriendPage.setInfo(_m_shareType,_shareAction,_isOnCDFunc);
                    _m_wFriendPage.showWnd();
                });
            }
        }

        private void _shareAction(long _cid, Action _action)
        {
            //已经分享过了
            if (null == _m_hasShareFriend || _m_hasShareFriend.Contains(_cid))
            {
                if(null != _action)
                {
                    _action();
                }
                return;
            }

            _m_lastShareTimeS = FpsAndPingMgr.instance.serverTimeTagS;
            _m_hasShareFriend.Add(_cid);
            if (null != _m_shareAction)
            {
                _m_shareAction(_cid, _action);
            }
        }

        private bool _isOnCDFunc(long _cid)
        {
            if (_m_lastShareTimeS + wnd.perShareCD >= FpsAndPingMgr.instance.serverTimeTagS)//共享cd中
                return true;
            if (_m_hasShareFriend.Contains(_cid))//分享过了
                return true;
            return false;
        }

        private NPCommonAssetPathInfo _getPageAssetPathByType(ENPGGUICommonShareTabType _type)
        {
            if (wnd == null || wnd.tabList == null)
                return null;

            for (int i = 0; i < wnd.tabList.Count; i++)
            {
                NPGGUICommonShareTabParam temp = wnd.tabList[i];
                if (temp.shareTabType == _type)
                    return NPCommonAssetPathInfo.readFromUiResId(temp.assetPathId);
            }
            return null;
        }

        #endregion


        #region 外部调用

        /// <summary>
        /// 显示窗口
        /// </summary>
        /// <param name="_type">分享类型</param>
        /// <param name="_titleStr">标题文字</param>
        /// <param name="_shareAction">分享回调</param>
        public void showShareWnd(ENPShareType _type,string _titleStr,Action<long,Action> _shareAction)
        {
            _m_shareType = _type;
            _m_shareAction = _shareAction;
            _m_titleStr = _titleStr;
            showWnd();
        }

        #endregion
    }
}
