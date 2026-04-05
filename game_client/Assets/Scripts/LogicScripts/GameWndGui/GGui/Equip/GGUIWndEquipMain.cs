using System.Collections.Generic;
using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 藏品主页面
    /// </summary>
    public class GGUIWndEquipMain : _ANPGGUIBasicResBarWnd<GGUIMonoEquipMain>
    {
        private static GGUIWndEquipMain _g_instance;
        public static GGUIWndEquipMain instance
        {
            get
            {
                if(_g_instance == null)
                    _g_instance = new GGUIWndEquipMain();
                return _g_instance;
            }
        }

        //已拥有藏品列表
        private GGUIWndEquipMainListGrid _m_wOwnEquipListGrid;
        //藏品图鉴列表
        private GGUIWndEquipMainListGrid _m_wHandbookEquipListGrid;
        //页签列表
        private List<GGUIWndEquipMainListTab> _m_lTabWndList;
        //当前选中的页签
        private GGUIWndEquipMainListTab _m_wSelectTabWnd;
        //展示数据列表
        private List<_IEquipCardShow> _m_lShowInfoList;
        //滚动列表的位置
        private float _m_LastListPos;
        private float _m_LastHandbookListPos;

        public GGUIWndEquipMain() : base(EALUIWndLayer.NORMAL)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoEquipMain.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoEquipMain.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override bool isShowAniPlayOnlyOne { get { return true; } }
        
        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsgAct(WinMsgType.ON_EQUIP_REMOVE, _onEquipRemove);
            WinMsg.RegisterMsg(WinMsgType.SIMULATE_CLICK_EQUIP_ITEM_BY_INDEX, _onSimulateClickEquipItem);
            _refreshWnd();

            //滚到上次的位置
            if (_m_wOwnEquipListGrid != null)
                _m_wOwnEquipListGrid.scrollMoveToTarget(_m_LastListPos);

            //滚到上次的位置
            if (_m_wHandbookEquipListGrid != null)
                _m_wHandbookEquipListGrid.scrollMoveToTarget(_m_LastHandbookListPos);
        }

        protected override void _onHideWnd()
        {
            //记录一下当前位置
            if (_m_wOwnEquipListGrid != null)
                _m_LastListPos = _m_wOwnEquipListGrid.getScrollCurPos();

            //记录一下当前位置
            if (_m_wHandbookEquipListGrid != null)
                _m_LastHandbookListPos = _m_wHandbookEquipListGrid.getScrollCurPos();

            WinMsg.UnregisterMsgAct(WinMsgType.ON_EQUIP_REMOVE, _onEquipRemove);
            WinMsg.UnregisterMsg(WinMsgType.SIMULATE_CLICK_EQUIP_ITEM_BY_INDEX, _onSimulateClickEquipItem);
            _m_wOwnEquipListGrid?.hideWnd();
            _m_wHandbookEquipListGrid?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wOwnEquipListGrid?.resetWnd();
            _m_wHandbookEquipListGrid?.resetWnd();

            //页签
            if (_m_lTabWndList != null)
            {
                GGUIWndEquipMainListTab tempTabItem = null;
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

            _m_LastListPos = 0f;
            _m_LastHandbookListPos = 0f;
        }

        protected override void _onDiscard()
        {
            _m_LastListPos = 0f;
            _m_LastHandbookListPos = 0f;

            _m_wOwnEquipListGrid?.discard();
            _m_wOwnEquipListGrid = null;

            _m_wHandbookEquipListGrid?.discard();
            _m_wHandbookEquipListGrid = null;

            _m_wSelectTabWnd = null;

            if (_m_lTabWndList != null)
            {
                GGUIWndEquipMainListTab tempTabItem = null;
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

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
            ALUGUICommon.uncombineBtnClick(wnd.btnRecycle, _onClickRecycle);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            _m_wSelectTabWnd = null;

            if (wnd.monoListGrid != null)
            {
                _m_wOwnEquipListGrid = new GGUIWndEquipMainListGrid(wnd.monoListGrid);
                _m_wOwnEquipListGrid.clickDelegate += _onClickEquipItem;
            }

            if (wnd.monoHandbookListGrid != null)
            {
                _m_wHandbookEquipListGrid = new GGUIWndEquipMainListGrid(wnd.monoHandbookListGrid);
                _m_wHandbookEquipListGrid.clickDelegate += _onClickEquipItem;
            }

            _m_lTabWndList = new List<GGUIWndEquipMainListTab>();
            if (null != wnd.monoTabList)
            {
                GGUIEquipMainTabMono tempTabMono = null;
                GGUIWndEquipMainListTab tempTabItem = null;
                for (int i = 0; i < wnd.monoTabList.Count; ++i)
                {
                    tempTabMono = wnd.monoTabList[i];
                    if (tempTabMono == null)
                        continue;
                    tempTabItem = new GGUIWndEquipMainListTab(tempTabMono.monoTab, tempTabMono.tabType);
                    //初始化默认未选中
                    tempTabItem.setSelected(false);
                    //绑定点击事件
                    tempTabItem.onClickTab += _onTabSelect;
                    _m_lTabWndList.Add(tempTabItem);
                }
            }

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
            ALUGUICommon.combineBtnClick(wnd.btnRecycle, _onClickRecycle);
        }

        //刷新列表
        private void _refreshWnd()
        {
            //设置选择页签
            if (_m_wSelectTabWnd == null)
            {
                foreach (GGUIWndEquipMainListTab itemTab in _m_lTabWndList)
                {
                    if (itemTab.tabType == EEquipMainTabType.OWN)
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

        //点击切换页签按钮
        private void _onTabSelect(GGUIWndEquipMainListTab _tabItemWnd)
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
        private void _refreshTabView(EEquipMainTabType _tabView)
        {
            if (_m_lShowInfoList == null)
                _m_lShowInfoList = new List<_IEquipCardShow>();
            _m_lShowInfoList.Clear();
            _m_wOwnEquipListGrid?.hideWnd();
            _m_wHandbookEquipListGrid?.hideWnd();

            switch (_tabView)
            {
                case EEquipMainTabType.OWN:
                    NPPlayer.instance.equipComp.getEquipInfoList(_m_lShowInfoList);
                    _m_lShowInfoList.Sort(_sortOwnList);
                    if (_m_wOwnEquipListGrid != null)
                    {
                        _m_wOwnEquipListGrid.showWnd();
                        _m_wOwnEquipListGrid.setInfo(EEquipMainTabType.OWN, _m_lShowInfoList);
                    }
                    break;
                case EEquipMainTabType.ILLUSTRATED_HANDBOOK:
                    GRefdataCoreMgr.instance.equipRefCore.dealAllRef(_ref =>
                    {
                        _m_lShowInfoList.Add(new EquipSimpleShowInfo(null,_ref));
                    });
                    _m_lShowInfoList.Sort(_sortHandbookList);
                    if (_m_wHandbookEquipListGrid != null)
                    {
                        _m_wHandbookEquipListGrid.showWnd();
                        _m_wHandbookEquipListGrid.setInfo(EEquipMainTabType.ILLUSTRATED_HANDBOOK, _m_lShowInfoList);
                    }
                    break;
            }
        }

        //已拥有列表排序：
        //装备中>空闲中
        //品质从高到低排序
        //等级从高到低排序
        //加成百分比从高到低排序
        //觉醒等级从高到低排序
        //额外等级从高到低排序
        //藏品ID排序
        private int _sortOwnList(_IEquipCardShow _a, _IEquipCardShow _b)
        {
            if (_a == null || _b == null)
                return 0;

            EquipInfo equipInfoA = _a.equipInfo;
            EquipInfo equipInfoB = _b.equipInfo;
            if (equipInfoA == null || equipInfoA.equipRef  == null || equipInfoB == null || equipInfoB.equipRef == null)
                return 0;

            //装备中>空闲中
            bool haveHeroA = equipInfoA.wearHeroId > 0;
            bool haveHeroB = equipInfoB.wearHeroId > 0;
            int haveHeroCompare = haveHeroA.CompareTo(haveHeroB);
            if (haveHeroCompare != 0)
                return -haveHeroCompare;

            //品质从高到低排序
            EQuality qualityA = GCommon.getItemQuality(ENPItemType.EQUIP, equipInfoA.equipId);
            EQuality qualityB = GCommon.getItemQuality(ENPItemType.EQUIP, equipInfoB.equipId);
            int qualityCompare = qualityA.CompareTo(qualityB);
            if (qualityCompare != 0)
                return -qualityCompare;

            //等级从高到低排序
            if (equipInfoA.level != equipInfoB.level)
                return -(equipInfoA.level.CompareTo(equipInfoB.level));

            //有伙伴的话加成百分比从高到低排序
            if (haveHeroA && haveHeroB)
            {
                long addValueA = equipInfoA.skillAddValue;
                long addValueB = equipInfoB.skillAddValue;
                if (addValueA != addValueB)
                    return -(addValueA.CompareTo(addValueB));
            }

            //额外等级从高到低排序
            if (equipInfoA.equipRef.addition_quality_level != equipInfoB.equipRef.addition_quality_level)
                return -(equipInfoA.equipRef.addition_quality_level.CompareTo(equipInfoB.equipRef.addition_quality_level));

            //藏品ID排序
            return equipInfoA.equipId.CompareTo(equipInfoB.equipId);
        }

        //图鉴列表排序：
        //品质从高到低排序
        //排序id从小到大
        //藏品额外等级从高到低排序
        //ID排序
        private int _sortHandbookList(_IEquipCardShow _a, _IEquipCardShow _b)
        {
            if (_a == null || _b == null)
                return 0;

            EquipRefObj equipRefA = _a.equipRef;
            EquipRefObj equipRefB = _b.equipRef;
            if (equipRefA == null || equipRefB == null)
                return 0;

            //品质从高到低排序
            EQuality qualityA = GCommon.getItemQuality(ENPItemType.EQUIP, equipRefA.id);
            EQuality qualityB = GCommon.getItemQuality(ENPItemType.EQUIP, equipRefB.id);
            int qualityCompare = qualityA.CompareTo(qualityB);
            if (qualityCompare != 0)
                return -qualityCompare;

            //排序id从小到大
            if (equipRefA.sort_id != equipRefB.sort_id)
                return equipRefA.sort_id.CompareTo(equipRefB.sort_id);

            //藏品额外等级从高到低排序
            if (equipRefA.addition_quality_level != equipRefB.addition_quality_level)
                return -(equipRefA.addition_quality_level.CompareTo(equipRefB.addition_quality_level));

            //ID排序
            return equipRefA.id.CompareTo(equipRefB.id);
        }

        //藏品移除事件
        private void _onEquipRemove()
        {
            if (_m_wSelectTabWnd == null)
                return;

            _refreshTabView(_m_wSelectTabWnd.tabType);
        }

        //模拟点击藏品列表打开详情界面, item下标从0开始
        private void _onSimulateClickEquipItem(params object[] _obj)
        {
            if (_obj == null || _obj.Length == 0 || _m_lShowInfoList == null || _m_wSelectTabWnd == null)
                return;

            long index = (long)_obj[0];
            if (index < 0 || index >= _m_lShowInfoList.Count)
                return;

            _onClickEquipItem(_m_wSelectTabWnd.tabType, _m_lShowInfoList[(int)index]);
        }

        //点击藏品item
        private void _onClickEquipItem(EEquipMainTabType _tabType, _IEquipCardShow _showInfo)
        {
            if (_showInfo == null)
                return;

            if (_tabType == EEquipMainTabType.OWN)
            {
                //打开技能详情
                List<EquipInfo> ownInfoList = new List<EquipInfo>();
                for (int i = 0; i < _m_lShowInfoList.Count; i++)
                {
                    if(_m_lShowInfoList[i] != null)
                        ownInfoList.Add(_m_lShowInfoList[i].equipInfo);
                }
                EquipInfo curSelectEquipInfo = _showInfo.equipInfo;
                QueueMgr.instance.AddNode(new GMainQueueEquipDetailNode(ownInfoList, curSelectEquipInfo));
            }
            else
            {
                //打开图鉴详情
                List<EquipRefObj> refList = new List<EquipRefObj>();
                for (int i = 0; i < _m_lShowInfoList.Count; i++)
                {
                    if(_m_lShowInfoList[i] != null)
                        refList.Add(_m_lShowInfoList[i].equipRef);
                }
                EquipRefObj curSelectRef = _showInfo.equipRef;
                QueueMgr.instance.AddNode(new GMainQueueEquipPreviewNode(refList, curSelectRef));
            }
        }

        //点击关闭按钮
        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_EQUIP_MAIN);
        }

        //点击回收
        private void _onClickRecycle(GameObject _go)
        {
            QueueMgr.instance.AddNode(new GNodeEquipRecycle());
        }
    }

    /// <summary>
    /// 普通藏品信息
    /// </summary>
    public class EquipSimpleShowInfo : _IEquipCardShow
    {
        private EquipInfo _m_equipInfo;
        private EquipRefObj _m_equipRefObj;

        public EquipInfo equipInfo { get { return _m_equipInfo; } }
        public EquipRefObj equipRef { get { return _m_equipRefObj; } }

        public EquipSimpleShowInfo(EquipInfo _equipInfo, EquipRefObj _refObj)
        {
            _m_equipInfo = _equipInfo;
            _m_equipRefObj = _refObj;
        }
    }
}