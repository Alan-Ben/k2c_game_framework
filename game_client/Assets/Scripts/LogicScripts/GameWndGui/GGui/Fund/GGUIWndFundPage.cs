using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 基金页面
    /// </summary>
    public class GGUIWndFundPage : _ATALBasicLoadPrefabSubUIWnd<GGUIMonoFundPage>
    {
        //资源id
        private long _m_lUIResId;
        //基金快照列表（UI专用，页面打开期间不更新等级）
        [NotNull] private readonly List<FundInfoSnapshot> _m_lFundSnapshotList;
        //页签容器
        private GGUIWndFundPageTabContainer _m_wTabContainer;
        //加载的页面字典，<资源id，页面类>
        [NotNull] private readonly Dictionary<long, GGUIWndFundBattlePass> _m_dPageDic;
        //当前显示的页面
        private GGUIWndFundBattlePass _m_wCurrentPage;
        //指定的 tab id （只生效一次）
        private long _m_lFundTabId;


        public GGUIWndFundPage(long _uiResId, Transform _parent) : base(_parent)
        {
            _m_lUIResId = _uiResId;
            _m_lFundSnapshotList = new List<FundInfoSnapshot>();
            _m_dPageDic = new Dictionary<long, GGUIWndFundBattlePass>();
            _m_lFundTabId = -1;
        }
        

        protected override string _monoAssetPath { get { return UIResPathAssistant.getAssetPath(_m_lUIResId); } }
        protected override string _monoObjName { get { return UIResPathAssistant.getObjName(_m_lUIResId); } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        

        protected override void _onShowWnd()
        {
            NPPlayer.instance.fundComp.onFundInfoChg += _onFundInfoChg;

            _refreshWnd();
        }
        protected override void _onHideWnd()
        {
            NPPlayer.instance.fundComp.onFundInfoChg -= _onFundInfoChg;

            _m_wTabContainer?.hideWnd();
            _hideAll();
            _m_wCurrentPage = null;
        }
        protected override void _onReset()
        {
            _m_wTabContainer?.resetWnd();

            foreach (GGUIWndFundBattlePass pageWnd in _m_dPageDic.Values)
            {
                pageWnd?.resetWnd();
            }
        }
        protected override void _onDiscard()
        {
            _m_wTabContainer?.discard();
            _m_wTabContainer = null;

            foreach (GGUIWndFundBattlePass pageWnd in _m_dPageDic.Values)
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
                _m_wTabContainer = new GGUIWndFundPageTabContainer(wnd.monoTabContainer);
                _m_wTabContainer.onClickItem += _onClickTabItem;
            }
        }
        
        
        public void setFundPageId(long _fundTabId)
        {
            _m_lFundTabId = _fundTabId;
            _refreshWnd();
        }
        
        
        //刷新窗口
        private void _refreshWnd()
        {
            if (wnd == null || !_m_bIsShow)
                return;

            //创建快照列表
            _m_lFundSnapshotList.Clear();
            ReadOnlyList<FundInfo> sourceList = NPPlayer.instance.fundComp.fundInfoList;
            for (int i = 0; i < sourceList.Count; i++)
            {
                FundInfo fundInfo = sourceList[i];
                if (fundInfo is { isUnlock: true })
                    _m_lFundSnapshotList.Add(new FundInfoSnapshot(fundInfo));
            }

            //根据排序值排序
            _m_lFundSnapshotList.Sort((_a, _b) =>
            {
                if (_a == null && _b == null)
                    return 0;
                if (_a == null)
                    return 1;
                if (_b == null)
                    return -1;

                int sortA = _a.fundRef?.sort_order ?? 0;
                int sortB = _b.fundRef?.sort_order ?? 0;
                return sortA.CompareTo(sortB);
            });

            //展示页签列表
            _m_wTabContainer?.showWnd();
            _m_wTabContainer?.showItemList(_m_lFundSnapshotList, _m_lFundTabId);
            _m_lFundTabId = -1;

            //如果没有页签，隐藏所有页面
            if (_m_lFundSnapshotList.Count <= 0)
                _hideAll();
        }
        //隐藏所有页面
        private void _hideAll()
        {
            foreach (GGUIWndFundBattlePass pageWnd in _m_dPageDic.Values)
            {
                pageWnd?.hideWnd();
            }
        }
        //点击页签
        private void _onClickTabItem(GGUIWndFundPageTabContainerItem _item, bool _moveToTop)
        {
            if (wnd == null || wnd.pageParent == null || _item == null)
                return;

            FundInfoSnapshot snapshot = _item.fundSnapshot;
            if (snapshot?.fundRef == null)
                return;

            long pageUIResId = snapshot.fundRef.page_ui_res_id;

            //先尝试获取已加载的页面
            _m_dPageDic.TryGetValue(pageUIResId, out GGUIWndFundBattlePass pageWnd);

            //先隐藏所有页面
            _hideAll();

            //如果页面不存在，创建新的
            if (pageWnd == null)
            {
                pageWnd = new GGUIWndFundBattlePass(pageUIResId, wnd.pageParent);
                pageWnd.load(() =>
                {
                    if (pageWnd == null)
                        return;

                    pageWnd.showWnd();
                    pageWnd.setInfo(snapshot);
                });
                _m_dPageDic[pageUIResId] = pageWnd;
            }
            else
            {
                pageWnd.showWnd();
                pageWnd.setInfo(snapshot);
            }

            _m_wCurrentPage = pageWnd;
        }
        //数据层基金信息变化
        private void _onFundInfoChg(FundInfo _fundInfo)
        {
            if (_fundInfo == null)
                return;

            //找到对应的快照并更新
            for (int i = 0; i < _m_lFundSnapshotList.Count; i++)
            {
                FundInfoSnapshot snapshot = _m_lFundSnapshotList[i];
                if (snapshot.fundId == _fundInfo.fundId)
                {
                    snapshot.updateFromSource(_fundInfo);
                    //通知当前显示的页面刷新
                    _m_wCurrentPage?.onFundInfoChg(snapshot);
                    break;
                }
            }
        }
    }
}