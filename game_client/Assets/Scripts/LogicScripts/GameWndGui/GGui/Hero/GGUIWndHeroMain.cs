using System;
using System.Collections.Generic;
using ALPackage;
using CommonEnum;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 伙伴主页面
    /// </summary>
    public class GGUIWndHeroMain : _ANPGGUIBasicResBarWnd<GGUIMonoHeroMain>
    {
        private static GGUIWndHeroMain _g_instance;
        public static GGUIWndHeroMain instance
        {
            get
            {
                if(_g_instance == null)
                    _g_instance = new GGUIWndHeroMain();
                return _g_instance;
            }
        }

        //伙伴数据列表
        private List<HeroCardShowInfo> _m_heroShowList;
        //伙伴筛选后数据列表
        private List<HeroCardShowInfo> _m_heroFilterShowList;
        //排序子窗口
        private GGUIWndHeroMainSort _m_wHeroListSort;
        //筛选子窗口
        private GGUIWndHeroMainFilter _m_wHeroListFilter;
        //伙伴列表
        private GGUIWndHeroListGrid _m_listPageGrid;
        //需要特殊展示红点的伙伴id列表
        private HashSet<long> _m_specialShowRedTipHeroId;
        //滚动列表的位置
        private float _m_LastListPos;

        public GGUIWndHeroMain() : base(EALUIWndLayer.NORMAL)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoHeroMain.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoHeroMain.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override bool isShowAniPlayOnlyOne { get { return true; } }
        
        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsg(WinMsgType.SIMULATE_OPEN_HERO_INFO_BY_INDEX, _simulateOpenHeroInfoByIndex);
            WinMsg.RegisterMsg(WinMsgType.HERO_MAIN_SCROLL_MOVE_TO_HERO, _onScrollMoveToHero);

            //初始化伙伴列表
            _initHeroList();

            //每次打开窗口都重置为上次选中状态
            if (_m_wHeroListSort != null)
            {
                _m_wHeroListSort.showWnd();
                _m_wHeroListSort.initState(AccountSettingMgr.instance.accountSetting.heroListSortType);
            }
            if (_m_wHeroListFilter != null)
            {
                _m_wHeroListFilter.showWnd();
                _m_wHeroListFilter.initState(NPPlayer.instance.heroComponent.heroListFilterType);
            }
            _refreshWnd();

            //滚到上次的位置
            if (_m_listPageGrid != null)
                _m_listPageGrid.scrollMoveToTarget(_m_LastListPos);
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.SIMULATE_OPEN_HERO_INFO_BY_INDEX, _simulateOpenHeroInfoByIndex);
            WinMsg.UnregisterMsg(WinMsgType.HERO_MAIN_SCROLL_MOVE_TO_HERO, _onScrollMoveToHero);

            //记录一下当前位置
            if (_m_listPageGrid != null)
                _m_LastListPos = _m_listPageGrid.getScrollCurPos();

            if (_m_wHeroListSort != null)
                _m_wHeroListSort.hideWnd();

            if (_m_wHeroListFilter != null)
                _m_wHeroListFilter.hideWnd();
        }

        protected override void _onReset()
        {
            if (_m_listPageGrid != null)
                _m_listPageGrid.resetWnd();

            if (_m_wHeroListSort != null)
                _m_wHeroListSort.resetWnd();

            if (_m_wHeroListFilter != null)
                _m_wHeroListFilter.resetWnd();
        }

        protected override void _onDiscard()
        {
            if (_m_listPageGrid != null)
                _m_listPageGrid.discard();
            _m_listPageGrid = null;

            if (_m_wHeroListSort != null)
                _m_wHeroListSort.discard();
            _m_wHeroListSort = null;

            if (_m_wHeroListFilter != null)
                _m_wHeroListFilter.discard();
            _m_wHeroListFilter = null;

            if (_m_heroShowList != null)
                _m_heroShowList.Clear();
            _m_heroShowList = null;

            if (_m_heroFilterShowList != null)
                _m_heroFilterShowList.Clear();
            _m_heroFilterShowList = null;
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            _m_heroShowList = new List<HeroCardShowInfo>();
            _m_heroFilterShowList = new List<HeroCardShowInfo>();
            _m_LastListPos = 0f;

            if (wnd.monoHeroGrid != null)
            {
                _m_listPageGrid = new GGUIWndHeroListGrid(wnd.monoHeroGrid);
                _m_listPageGrid.clickDelegate += _onClickCardItem;
            }

            if (wnd.monoSort != null)
            {
                _m_wHeroListSort = new GGUIWndHeroMainSort(wnd.monoSort);
                _m_wHeroListSort.initState(AccountSettingMgr.instance.accountSetting.heroListSortType);
                _m_wHeroListSort.onSortChanged += _onSortChg;
            }

            if (wnd.monoFilter != null)
            {
                _m_wHeroListFilter = new GGUIWndHeroMainFilter(wnd.monoFilter);
                _m_wHeroListFilter.initState(NPPlayer.instance.heroComponent.heroListFilterType);
                _m_wHeroListFilter.onFilterChanged += _onFilterChg;
            }
        }

        //重置列表位置
        public void resetListPos()
        {
            _m_LastListPos = 0;
        }
        
        //初始化骑士列表
        private void _initHeroList()
        {
            if (null == _m_heroShowList)
                _m_heroShowList = new List<HeroCardShowInfo>();
            _m_heroShowList.Clear();

            if (null == _m_specialShowRedTipHeroId)
                _m_specialShowRedTipHeroId = new HashSet<long>();
            _m_specialShowRedTipHeroId.Clear();

            GRefdataCoreMgr.instance.heroRefCore.dealAllRef(_refObj =>
            {
                if (_refObj != null)
                    _m_heroShowList.Add(new HeroCardShowInfo(NPPlayer.instance.heroComponent.getHeroInfo(_refObj.id), _refObj));
            });

            //按照实力排序，取前几个需要特殊展示红点的伙伴
            _sortByPower();
            for (int i = 0; i < wnd.specShowRedTipCount; i++)
            {
                if (_m_heroShowList[i] != null && _m_heroShowList[i].heroInfo != null)
                    _m_specialShowRedTipHeroId.Add(_m_heroShowList[i].id);
            }
        }

        //刷新列表
        private void _refreshWnd()
        {
            _sortHeroList();
            _dealFilterHeroList();
            _refreshGrid();
        }

        //刷新列表
        private void _refreshGrid()
        {
            if (wnd == null)
                return;

            if (_m_listPageGrid != null)
            {
                _m_listPageGrid.showWnd();
                _m_listPageGrid.showHeroList(_m_heroFilterShowList, checkIsSpecalRedTipHero);
            }

            long totalCount = 0;
            long ownCount = 0;
            if (_m_heroFilterShowList != null)
            {
                totalCount = _m_heroFilterShowList.Count;
                for (int i = 0; i < _m_heroFilterShowList.Count; i++)
                {
                    if (_m_heroFilterShowList[i] != null && _m_heroFilterShowList[i].isUnlock)
                        ownCount++;
                }
            }
            //设置拥有的数量
            ALUGUICommon.setLabelTxt(wnd.txtNum, TextTranslate.instance.getLanguage(TransKeyConst.hero_ownNum_num_num, ownCount, totalCount));
        }

        /// <summary>
        /// 检查是否是需要特殊展示红点的伙伴
        /// </summary>
        /// <param name="_heroId"></param>
        /// <returns></returns>
        public bool checkIsSpecalRedTipHero(long _heroId)
        {
            //如果配置-1则都要显示
            if (wnd != null && wnd.specShowRedTipCount == -1)
                return true;

            return _m_specialShowRedTipHeroId != null && _m_specialShowRedTipHeroId.Contains(_heroId);
        }

        #region 排序

        /// <summary>
        /// 处理筛选列表
        /// </summary>
        private void _dealFilterHeroList()
        {
            if (_m_heroShowList == null)
                return;

            ESpecAttrType targetFilterType = NPPlayer.instance.heroComponent.heroListFilterType;
            if (_m_heroFilterShowList == null)
                _m_heroFilterShowList = new List<HeroCardShowInfo>();
            _m_heroFilterShowList.Clear();

            //全部
            if(targetFilterType == ESpecAttrType.NONE)
                _m_heroFilterShowList.AddRange(_m_heroShowList);
            else
            {
                //指定相性
                for (int i = 0; i < _m_heroShowList.Count; i++)
                {
                    if (_m_heroShowList[i] != null && 
                        _m_heroShowList[i].heroRefObj != null &&
                        _m_heroShowList[i].heroRefObj.spec_attr_type == targetFilterType)
                        _m_heroFilterShowList.Add(_m_heroShowList[i]);
                }
            }
        }

        /// <summary>
        /// 排序
        /// </summary>
        private void _sortHeroList()
        {
            switch (AccountSettingMgr.instance.accountSetting.heroListSortType)
            {
                case EHeroListSortTabType.DEFAULT://默认
                    _sortDefault();
                    break;
                case EHeroListSortTabType.POWER://实力
                    _sortByPower();
                    break;
                case EHeroListSortTabType.TALENT://资质
                    _sortByTalent();
                    break;
                case EHeroListSortTabType.STAR://觉醒
                    _sortByStar();
                    break;
            }
        }

        //默认排序，品质从高到低 实力从高到低 角色id
        private void _sortDefault()
        {
            _m_heroShowList?.Sort((_a, _b) =>
            {
                if(_a.isUnlock.CompareTo(_b.isUnlock) != 0)
                    return -_a.isUnlock.CompareTo(_b.isUnlock);

                EQuality qualityA = GCommon.getItemQuality(ENPItemType.HERO, _a.id);
                EQuality qualityB = GCommon.getItemQuality(ENPItemType.HERO, _b.id);
                if (qualityA.CompareTo(qualityB) != 0)
                    return -qualityA.CompareTo(qualityB);

                long powerA = _a.power;
                long powerB = _b.power;
                if (powerA.CompareTo(powerB) != 0)
                    return -powerA.CompareTo(powerB);

                return _a.id.CompareTo(_b.id);
            });
        }

        //实力，实力从高到低 品质从高到低 角色id
        private void _sortByPower()
        {

            _m_heroShowList?.Sort((_a, _b) =>
            {
                if (_a.isUnlock.CompareTo(_b.isUnlock) != 0)
                    return -_a.isUnlock.CompareTo(_b.isUnlock);

                long powerA = _a.power;
                long powerB = _b.power;
                if (powerA.CompareTo(powerB) != 0)
                    return -powerA.CompareTo(powerB);

                EQuality qualityA = GCommon.getItemQuality(ENPItemType.HERO, _a.id);
                EQuality qualityB = GCommon.getItemQuality(ENPItemType.HERO, _b.id);
                if (qualityA.CompareTo(qualityB) != 0)
                    return -qualityA.CompareTo(qualityB);

                return _a.id.CompareTo(_b.id);
            });
        }

        //资质，资质从高到低 品质从高到低 实力从高到低 角色id
        private void _sortByTalent()
        {
            _m_heroShowList?.Sort((_a, _b) =>
            {
                if (_a.isUnlock.CompareTo(_b.isUnlock) != 0)
                    return -_a.isUnlock.CompareTo(_b.isUnlock);

                long talentA = _a.getTotalTalent();
                long talentB = _b.getTotalTalent();
                if (talentA.CompareTo(talentB) != 0)
                    return -talentA.CompareTo(talentB);

                EQuality qualityA = GCommon.getItemQuality(ENPItemType.HERO, _a.id);
                EQuality qualityB = GCommon.getItemQuality(ENPItemType.HERO, _b.id);
                if (qualityA.CompareTo(qualityB) != 0)
                    return -qualityA.CompareTo(qualityB);

                long powerA = _a.power;
                long powerB = _b.power;
                if (powerA.CompareTo(powerB) != 0)
                    return -powerA.CompareTo(powerB);

                return _a.id.CompareTo(_b.id);
            });
        }

        //觉醒，觉醒星级从高到低 品质从到到低 实力从高到低 角色id
        private void _sortByStar()
        {
            _m_heroShowList?.Sort((_a, _b) =>
            {
                if (_a.isUnlock.CompareTo(_b.isUnlock) != 0)
                    return -_a.isUnlock.CompareTo(_b.isUnlock);

                long starA = _a.star;
                long starB = _b.star;
                if (starA.CompareTo(starB) != 0)
                    return -starA.CompareTo(starB);

                EQuality qualityA = GCommon.getItemQuality(ENPItemType.HERO, _a.id);
                EQuality qualityB = GCommon.getItemQuality(ENPItemType.HERO, _b.id);
                if (qualityA.CompareTo(qualityB) != 0)
                    return -qualityA.CompareTo(qualityB);

                long powerA = _a.power;
                long powerB = _b.power;
                if (powerA.CompareTo(powerB) != 0)
                    return -powerA.CompareTo(powerB);

                return _a.id.CompareTo(_b.id);
            });
        }

        #endregion

        #region 点击事件

        //排序变更
        private void _onSortChg(EHeroListSortTabType _type)
        {
            //保存选择
            AccountSettingMgr.instance.accountSetting.setHeroListSortType(_type);

            _refreshWnd();
        }

        //筛选变更
        private void _onFilterChg(ESpecAttrType _type)
        {
            //保存选择
            NPPlayer.instance.heroComponent.heroListFilterType = _type;

            _refreshWnd();
        }

        //点击伙伴卡牌
        private void _onClickCardItem(GGUIWndHeroListGridItem _item)
        {
            if (_item == null || _item.heroShowInfo == null)
                return;

            _openHeroWnd(_item.heroShowInfo);
        }

        //打开伙伴界面
        private void _openHeroWnd(HeroCardShowInfo _cardShowInfo)
        {
            if (null == _cardShowInfo)
                return;

            if (_cardShowInfo.heroInfo == null)
            {
                //打开未解锁伙伴详情弹窗
                QueueMgr.instance.AddNode(new GMainQueueHeroLockInfoNode(_cardShowInfo, _m_heroShowList));
            }
            else
            {
                //打开伙伴详情弹窗
                QueueMgr.instance.AddNode(new GMainQueueHeroInfoNode(_cardShowInfo, _m_heroShowList));
            }
        }

        #endregion

        #region 消息事件

        //根据下标，模拟点击打开大臣详情弹窗
        private void _simulateOpenHeroInfoByIndex(object[] _objs)
        {
            if (null == _objs || _objs.Length == 0)
                return;

            long index = (long)_objs[0];

            if (null == _m_listPageGrid || _m_heroShowList == null || index < 0 || _m_heroShowList.Count <= index)
                return;

            _openHeroWnd(_m_heroShowList[(int)index]);
        }

        /// <summary>
        /// 滚动到指定类型的伙伴
        /// 参数：_objs[0] 为 EHeroMainTargetHeroType 枚举类型
        /// </summary>
        private void _onScrollMoveToHero(params object[] _objs)
        {
            if (_objs == null || _objs.Length < 1 || _objs[0] == null || _m_listPageGrid == null)
                return;

            EHeroMainTargetHeroType _moveType;
            if(_objs[0] is EHeroMainTargetHeroType)
                _moveType = (EHeroMainTargetHeroType)_objs[0];
            else if(_objs[0] is string)
                ALCommon.TryEnumParse(typeof(EHeroMainTargetHeroType), (string)_objs[0], out _moveType);
            else
                return;
            
            float moveTime = 0f;
            if (_objs.Length >= 2 && _objs[1] != null)
            {
                if (_objs[1] is float)
                    moveTime = (float)_objs[1];
                else if (_objs[1] is string)
                    moveTime = ALCommon.ParseFloat((string)_objs[1]);
            }
            
            switch (_moveType)
            {
                case EHeroMainTargetHeroType.LEVEL_MIN_HERO:
                    _m_listPageGrid.scrollMoveToMinLevelHero(moveTime);
                    break;
                case EHeroMainTargetHeroType.BUSINESS_SKILL_LEVEL_MIN_HERO:
                    _m_listPageGrid.scrollMoveToMinBusinessSkillLevelHero(moveTime);
                    break;
                case EHeroMainTargetHeroType.NO_WEAR_EQUIP_HERO:
                    _m_listPageGrid.scrollMoveToNoWearEquipHero(moveTime);
                    break;
                default:
                    Debug.LogError_EditorOnly($"[GGUIWndHeroMain _onScrollMoveToHero] 未实现滚动类型：{_moveType}");
                    break;
            }
        }
        
        /// <summary>
        /// 获取目标伙伴的RectTransform
        /// </summary>
        /// <param name="_type"></param>
        /// <returns></returns>
        public RectTransform getTargetHeroRectTransform(EHeroMainTargetHeroType _type)
        {
            if (_m_listPageGrid == null)
                return null;
            
            switch (_type)
            {
                case EHeroMainTargetHeroType.LEVEL_MIN_HERO:
                    return _m_listPageGrid.getMinLevelHeroRectTransform();
                case EHeroMainTargetHeroType.BUSINESS_SKILL_LEVEL_MIN_HERO:
                    return _m_listPageGrid.getMinBusinessSkillLevelHeroRectTransform();
                case EHeroMainTargetHeroType.NO_WEAR_EQUIP_HERO:
                    return _m_listPageGrid.getNoWearEquipHeroRectTransform();
                default:
                    return null;
            }
        }
        
        #endregion
    }
}