using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 竞技场战报界面
    /// </summary>
    public class GGUIWndArenaBattleReport : _ANPGGUIBasicWnd<GGUIMonoArenaBattleReport>
    {
        private static GGUIWndArenaBattleReport _g_instance;
        public static GGUIWndArenaBattleReport instance
        {
            get
            {
                if(_g_instance == null)
                    _g_instance = new GGUIWndArenaBattleReport();
                return _g_instance;
            }
        }

        //战报信息列表
        private List<ArenaBattleReportInfo> _m_lReportList;
        //对手信息列表
        private List<ArenaFightBackInfo> _m_lFightBackList;
        //战报列表
        private GGUIWndArenaBattleReportGrid _m_wReportGrid;
        //对手列表
        private GGUIWndArenaBattleReportOpponentGrid _m_wOpponentGrid;
        //页签列表
        private List<GGUIWndArenaBattleReportTab> _m_lTabWndList;
        //当前选中的页签
        private GGUIWndArenaBattleReportTab _m_wSelectTabWnd;
        //显示序列号
        private long _m_lShowSerialize;
        //是否正在请求对手信息
        private bool _m_bIsReqOpponent;
        //是否正在请求战报信息
        private bool _m_bIsReqReport;

        public GGUIWndArenaBattleReport() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoArenaBattleReport.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoArenaBattleReport.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onShowWnd()
        {
            _m_lShowSerialize = ALSerializeOpMgr.next();
            _m_bIsReqOpponent = false;
            _m_bIsReqReport = false;
        }

        protected override void _onHideWnd()
        {
            _m_lShowSerialize = ALSerializeOpMgr.next();
            _m_lReportList = null;
            _m_lFightBackList = null;
            _m_wSelectTabWnd?.setSelected(false);
            _m_wSelectTabWnd = null;
            _m_bIsReqOpponent = false;
            _m_bIsReqReport = false;
            _hideAllPage();
        }

        protected override void _onReset()
        {
            //页签
            if (_m_lTabWndList != null)
            {
                GGUIWndArenaBattleReportTab tempTabItem = null;
                for (int i = 0; i < _m_lTabWndList.Count; ++i)
                {
                    tempTabItem = _m_lTabWndList[i];
                    if (tempTabItem == null)
                        continue;
                    //重置状态
                    tempTabItem.resetWnd();
                }
            }

            _m_wSelectTabWnd = null;

            _m_wReportGrid?.resetWnd();
            _m_wOpponentGrid?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_wReportGrid?.discard();
            _m_wReportGrid = null;
            _m_wOpponentGrid?.discard();
            _m_wOpponentGrid = null;

            if (_m_lTabWndList != null)
            {
                GGUIWndArenaBattleReportTab tempTabItem = null;
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

            _m_wSelectTabWnd = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            _m_lTabWndList = new List<GGUIWndArenaBattleReportTab>();
            if (null != wnd.monoTabList)
            {
                GGUIArenaBattleReportTabMono tempTabMono = null;
                GGUIWndArenaBattleReportTab tempTabItem = null;
                for (int i = 0; i < wnd.monoTabList.Count; ++i)
                {
                    tempTabMono = wnd.monoTabList[i];
                    if (tempTabMono == null || tempTabMono.monoTab == null)
                        continue;
                    tempTabItem = new GGUIWndArenaBattleReportTab(tempTabMono.monoTab, tempTabMono.tabType);
                    //初始化默认未选中
                    tempTabItem.setSelected(false);
                    //绑定点击事件
                    tempTabItem.onClickTab += _onTabSelect;
                    _m_lTabWndList.Add(tempTabItem);
                }
            }

            if(wnd.monoBattleReportGrid != null)
                _m_wReportGrid = new GGUIWndArenaBattleReportGrid(wnd.monoBattleReportGrid);

            if(wnd.monoBattleReportOpponentGrid != null)
                _m_wOpponentGrid = new GGUIWndArenaBattleReportOpponentGrid(wnd.monoBattleReportOpponentGrid);

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_selectTabType"></param>
        public void setInfo(EArenaBattleReportTabType _selectTabType)
        {
            if (wnd == null)
                return;

            _refreshPageWnd(_selectTabType);
        }

        /// <summary>
        /// 刷新页签窗口
        /// </summary>
        private void _refreshPageWnd(EArenaBattleReportTabType _selectTabType)
        {
            if (null == wnd)
                return;

            //设置选中页签
            if (_m_wSelectTabWnd == null)
            {
                foreach (GGUIWndArenaBattleReportTab itemTab in _m_lTabWndList)
                {
                    if (itemTab.tabType == _selectTabType)
                    {
                        _onTabSelect(itemTab);
                        break;
                    }
                }
            }
            else
            {
                _m_wSelectTabWnd.setSelected(true);
                //根据页签刷新列表内容
                _refreshTabView(_m_wSelectTabWnd.tabType);
            }
        }

        #region 页签页面处理

        //点击切换页签按钮
        private void _onTabSelect(GGUIWndArenaBattleReportTab _tabItemWnd)
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
        private void _refreshTabView(EArenaBattleReportTabType _tabView)
        {
            _hideAllPage();

            switch (_tabView)
            {
                case EArenaBattleReportTabType.OPPONENT://对手
                    _showOpponentPage();
                    break;
                case EArenaBattleReportTabType.REPORT://战报
                    _showReportPage();
                    break;
            }
        }

        //关闭所有页面
        private void _hideAllPage()
        {
            _m_wReportGrid?.hideWnd();
            _m_wOpponentGrid?.hideWnd();
        }

        //显示对手页面
        private void _showOpponentPage()
        {
            //没有数据，先请求数据再展示
            if (_m_lFightBackList == null || _m_lFightBackList.Count == 0)
            {
                if (_m_bIsReqOpponent)
                    return;

                long serialize = _m_lShowSerialize;
                _m_bIsReqOpponent = true;
                NPPlayer.instance.arenaComp.reqArenaFightBackData((_infoList) =>
                {
                    if (serialize != _m_lShowSerialize || wnd == null || !isShow || _infoList == null)
                        return;

                    _m_bIsReqOpponent = false;
                    if (_m_lFightBackList == null)
                        _m_lFightBackList = new List<ArenaFightBackInfo>();
                    else
                        _m_lFightBackList.Clear();

                    for (int i = 0; i < _infoList.Count; i++)
                    {
                        _m_lFightBackList.Add(new ArenaFightBackInfo(_infoList[i]));
                    }

                    //排序 最新的排前面
                    _m_lFightBackList?.Sort((_a, _b) =>
                    {
                        return -(_a.dbId.CompareTo(_b.dbId));
                    });

                    //如果已经不是当前页签，不显示
                    if (_m_wSelectTabWnd != null && _m_wSelectTabWnd.tabType != EArenaBattleReportTabType.OPPONENT)
                        return;

                    _m_wOpponentGrid?.showWnd();
                    _m_wOpponentGrid?.setInfo(_m_lFightBackList);
                });
            }
            else
            {
                _m_wOpponentGrid?.showWnd();
                _m_wOpponentGrid?.setInfo(_m_lFightBackList);
            }
        }

        //显示战报页面
        private void _showReportPage()
        {            
            //没有数据，先请求数据再展示
            if (_m_lReportList == null || _m_lReportList.Count == 0)
            {
                if (_m_bIsReqReport)
                    return;

                long serialize = _m_lShowSerialize;
                _m_bIsReqReport = true;
                NPPlayer.instance.arenaComp.reqArenaBattleReport((_infoList) =>
                {
                    if (serialize != _m_lShowSerialize || wnd == null || !isShow || _infoList == null)
                        return;

                    _m_bIsReqReport = false;
                    if (_m_lReportList == null)
                        _m_lReportList = new List<ArenaBattleReportInfo>();
                    else
                        _m_lReportList.Clear();
                    for (int i = 0; i < _infoList.Count; i++)
                    {
                        _m_lReportList.Add(new ArenaBattleReportInfo(_infoList[i]));
                    }

                    //排序 最新的排前面
                    _m_lReportList?.Sort((_a, _b) =>
                    {
                        return -(_a.dbId.CompareTo(_b.dbId));
                    });

                    //如果已经不是当前页签，不显示
                    if (_m_wSelectTabWnd != null && _m_wSelectTabWnd.tabType != EArenaBattleReportTabType.REPORT)
                        return;

                    _m_wReportGrid?.showWnd();
                    _m_wReportGrid?.setInfo(_m_lReportList);
                });
            }
            else
            {
                _m_wReportGrid?.showWnd();
                _m_wReportGrid?.setInfo(_m_lReportList);
            }
        }

        #endregion

        //点击关闭
        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_ARENA_BATTLE_REPORT);
        }
    }
}