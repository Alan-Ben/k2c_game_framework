using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 火星-民意-求助 列表页面（PrefabSubWnd）
    /// 负责加载 GGUIMonoMarsPopularWillHelpPage，并驱动内部 Container 刷新。
    /// </summary>
    public class GGUIWndMarsPopularWillHelpPage : _ANPGGUIBasicLoadPrefabSubWnd<GGUIMonoMarsPopularWillHelpPage>
    {
        private NPCommonAssetPathInfo _m_pagePathInfo;
        
        private List<_IMarsPeopleWillHelp> _m_lTmpHelpList; // 求助列表临时变量

        // 子容器：求助列表
        private GGUIWndMarsPopularWillHelpContainer _m_helpContainer;

        public GGUIWndMarsPopularWillHelpPage(NPCommonAssetPathInfo _pagePathInfo, Transform _parent) : base(_parent)
        {
            _m_pagePathInfo = _pagePathInfo;
        }

        protected override string _monoAssetPath { get { return _m_pagePathInfo?.asset_path; } }
        protected override string _monoObjName { get { return _m_pagePathInfo?.obj_name; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoHelpContainer != null)
                _m_helpContainer = new GGUIWndMarsPopularWillHelpContainer(wnd.monoHelpContainer);
        }

        protected override void _onShowWnd()
        {
            // 首次显示时刷新页面并注册求助相关消息
            refreshWnd();

            WinMsg.RegisterMsg(WinMsgType.ON_MARS_HELP_ADD, _onAddHelp);
            WinMsg.RegisterMsg(WinMsgType.ON_MARS_HELP_DEL, _onDelHelp);
        }

        protected override void _onHideWnd()
        {
            // 对称注销消息监听
            WinMsg.UnregisterMsg(WinMsgType.ON_MARS_HELP_ADD, _onAddHelp);
            WinMsg.UnregisterMsg(WinMsgType.ON_MARS_HELP_DEL, _onDelHelp);

            _m_helpContainer?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_helpContainer?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_helpContainer?.discard();
            _m_helpContainer = null;
            
            _m_lTmpHelpList?.Clear();
            _m_lTmpHelpList = null;
        }
        
        /// <summary>
        /// 页面自身简单刷新（若无数据可不操作）
        /// </summary>
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow)
                return;

            _refreshHelpList(true);
        }
        
        /// <summary>
        /// 刷新求助列表
        /// </summary>
        private void _refreshHelpList(bool _needUpdateInfo)
        {
            if (wnd == null || !_m_bIsShow || _m_helpContainer == null)
                return;

            if (_needUpdateInfo)
            {
                if (_m_lTmpHelpList == null)
                    _m_lTmpHelpList = new List<_IMarsPeopleWillHelp>();
                _m_lTmpHelpList.Clear();
                if (NPPlayer.instance.marsComp.peopleSubComponent.helpList != null)
                    _m_lTmpHelpList.AddRange(NPPlayer.instance.marsComp.peopleSubComponent.helpList);
                _m_lTmpHelpList.Sort(MarsUtil.sortMarsHelp_DealPostpone_IdS2L);
                int helpCount = _m_lTmpHelpList.Count;
                int maxShowHelpCount = GRefdataCoreMgr.instance.npGeneral.mars_daily_help_limit;
                if(helpCount > maxShowHelpCount)
                    _m_lTmpHelpList.RemoveRange(maxShowHelpCount, helpCount - maxShowHelpCount);
            }

            _m_helpContainer.showWnd();
            _m_helpContainer.setData(_m_lTmpHelpList);
        }

        #region 窗口消息处理

        /// <summary>
        /// 新增求助
        /// </summary>
        /// <param name="_"></param>
        private void _onAddHelp(params object[] _objs)
        {
            if(_objs == null || _objs.Length < 1 || !(_objs[0] is _IMarsPeopleWillHelp helpInfo))
                return;
            
            if(_m_lTmpHelpList == null)
                _m_lTmpHelpList = new List<_IMarsPeopleWillHelp>();
            _m_lTmpHelpList.Insert(0, helpInfo);
            
            _refreshHelpList(false);
        }

        /// <summary>
        /// 删除求助
        /// </summary>
        /// <param name="_"></param>
        private void _onDelHelp(params object[] _objs)
        {
            if(_objs == null || _objs.Length < 1 || !(_objs[0] is long _helpInstanceId) || _m_lTmpHelpList == null)
                return;
            
            _m_lTmpHelpList.RemoveAll(helpInfo => helpInfo != null && helpInfo.instanceId == _helpInstanceId);
            _refreshHelpList(false);
        }

        #endregion
    }
}
