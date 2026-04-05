using System;
using System.Collections.Generic;
using ALPackage;
using Common.HeroObj;
using CommonEnum;
using GOE;
using JetBrains.Annotations;
using NPEnum;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GOE
{
    
    public class ChapterEventDispatchHeroInfo : HeroCardShowInfo
    {
        private int count;
        public ChapterEventDispatchHeroInfo(HeroInfo _info, int _count) : base(_info, _info?.heroRefObj)
        {
            count = _count;
        }
        public static int sort(_IHeroCardShow _a, _IHeroCardShow _b)
        {
            ChapterEventDispatchHeroInfo a = _a as ChapterEventDispatchHeroInfo;
            ChapterEventDispatchHeroInfo b = _b as ChapterEventDispatchHeroInfo;
            if (b == null)
                return -1;
            if (a == null)
                return 1;
            int compareRes = -a.count.CompareTo(b.count);
            if (compareRes != 0)
                return compareRes;
        
            if (Object.ReferenceEquals(a.heroInfo, b.heroInfo))
                return 0;
            if (b.heroInfo == null)
                return -1;
            if (a.heroInfo == null)
                return 1;
        
            compareRes = a.heroInfo.level.CompareTo(b.heroInfo.level);
            if (compareRes != 0)
                return -compareRes;
                
            if (Object.ReferenceEquals(a.heroInfo.heroRefObj, b.heroInfo.heroRefObj))
                return 0;
            if (b.heroInfo.heroRefObj == null)
                return -1;
            if (a.heroInfo.heroRefObj == null)
                return 1;
        
            return b.heroInfo.heroRefObj.id.CompareTo(a.heroInfo.heroRefObj.id);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class GGUIWndChapterEventDispatch : _ATALBasicUIWnd<GGUIMonoChapterEventDispatch>
    {
        private static GGUIWndChapterEventDispatch _g_instance = new GGUIWndChapterEventDispatch();

        public static GGUIWndChapterEventDispatch instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndChapterEventDispatch();
                return _g_instance;
            }
        }
        private ChapterEventDispatchRefObj _m_refObj;
        private ChapterEventDispatchShowRefObj _m_showRefObj;
        private Action<List<long>> _m_onDispatch; // 派遣回调
        private GGUIWndChapterEventDispatchCondItemContainer _m_wCondContainer;//条件Container
        private GGUIWndChapterEventDispatchCondStateItemContainer _m_wCondStateContainer;//条件状态Container
        private GGUIWndHeroIconNullableItemContainer _m_wSelectedHeroContainer;//大臣头像Container
        private GGUIWndHeroCommonSelect _m_wHeroSelectWnd;//大臣选择窗口

        private List<_IHeroCardShow> _m_showHeroList; //显示的大臣列表
        [NotNull]
        private List<ChapterEventDispatchConditionInfo> _m_lDispatchCondRefObjList = new List<ChapterEventDispatchConditionInfo>();
        [NotNull]
        private List<_IHeroCardShow> _m_curSelectHeroList = new List<_IHeroCardShow>(); // 当前选中的大臣列表
        private List<bool> _m_curConditionStageList; // 当前条件状态
        private ESpecAttrType _m_filterType = ESpecAttrType.NONE;

        public GGUIWndChapterEventDispatch() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get => GGUIMonoChapterEventDispatch.assetPath; }
        protected override string _monoObjName { get => GGUIMonoChapterEventDispatch.objName; }
        protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }

        protected override void _onShowWnd()
        {
            _m_wCondContainer?.showWnd();
            _m_wCondStateContainer?.showWnd();
            _m_wSelectedHeroContainer?.showWnd();
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            _m_wCondContainer?.hideWnd();
            _m_wCondStateContainer?.hideWnd();
            _m_wSelectedHeroContainer?.hideWnd();
            _m_wHeroSelectWnd?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wCondContainer?.resetWnd();
            _m_wCondStateContainer?.resetWnd();
            _m_wSelectedHeroContainer?.resetWnd();
            _m_wHeroSelectWnd?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_wCondContainer?.discard();
            _m_wCondContainer = null;
            
            _m_wCondStateContainer?.discard();
            _m_wCondStateContainer = null;

            if (_m_wSelectedHeroContainer != null)
            {
                _m_wSelectedHeroContainer.discard();
                _m_wSelectedHeroContainer.onItemClick -= _onSelectHeroContainerItemClick;
            }
            _m_wSelectedHeroContainer = null;
            
            _m_wHeroSelectWnd?.discard();
            _m_wHeroSelectWnd = null;
        }

        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
            
            if (wnd.monoCondContainer != null)
                _m_wCondContainer = new GGUIWndChapterEventDispatchCondItemContainer(wnd.monoCondContainer);
            
            if (wnd.monoCondStateContainer != null)
                _m_wCondStateContainer = new GGUIWndChapterEventDispatchCondStateItemContainer(wnd.monoCondStateContainer);


            if (wnd.monoSelectedHeroContainer != null)
            {
                _m_wSelectedHeroContainer = new GGUIWndHeroIconNullableItemContainer(wnd.monoSelectedHeroContainer);
                _m_wSelectedHeroContainer.onItemClick += _onSelectHeroContainerItemClick;
            }

            if (wnd.monoHeroSelectWnd != null)
            {
                _m_wHeroSelectWnd = new GGUIWndHeroCommonSelect(wnd.monoHeroSelectWnd, _onHeroSelect);
            }
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onBtnCloseClick);
            ALUGUICommon.combineBtnClick(wnd.btnDispatch, _onBtnDispatchClick);
            ALUGUICommon.combineBtnClick(wnd.btnAutoDispatch, _onBtnAutoDispatchClick);

        }
        
        public void refreshWnd(ChapterEventDispatchRefObj _refObj, Action<List<long>> _onDispatch)
        {
            _m_refObj = _refObj;
            _m_onDispatch = _onDispatch;
            if(null == _m_refObj)
                return;
            _m_showRefObj = GRefdataCoreMgr.instance.chapterEventDispatchShowRefCore?.getRef(_m_refObj.dispatch_show_id);
            if (_m_refObj.condition_id_list != null)
            {
                ChapterEventDispatchCondRefObj condRefObj = null;
                _m_lDispatchCondRefObjList.Clear();
                foreach (long conditionId in _m_refObj.condition_id_list)
                {
                    condRefObj = GRefdataCoreMgr.instance.chapterEventDispatchCondRefCore?.getRef(conditionId);
                    if(condRefObj != null)
                        _m_lDispatchCondRefObjList.Add(new ChapterEventDispatchConditionInfo(condRefObj));
                }
            }

            _m_filterType = ESpecAttrType.NONE;
            _m_showHeroList = new List<_IHeroCardShow>();
            // 遍历所有大臣，并按照满足条件进行排序
            NPPlayer.instance.heroComponent.dealAllHero(_info =>
            {
                if(null == _info)
                    return;
                int satisfyCondCount = 0;//满足条件数量
                if (_m_lDispatchCondRefObjList != null)
                {
                    foreach (var conditionInfo in _m_lDispatchCondRefObjList)
                    {
                        // 没有配置条件的情况, 算满足
                        if (conditionInfo == null || conditionInfo.refObj.condition == null ||
                            conditionInfo.refObj.condition.isEmpty || conditionInfo.refObj.condition.IsEnable(_info.heroRefObj, null))
                        {
                            satisfyCondCount++;
                        }
                    }
                }

                _m_showHeroList.Add(new ChapterEventDispatchHeroInfo(_info, satisfyCondCount));
            });
            _m_showHeroList.Sort(ChapterEventDispatchHeroInfo.sort);
            _m_curSelectHeroList.Clear();
            _refreshWnd();
        }

        /// <summary>
        /// 刷新界面
        /// </summary>
        private void _refreshWnd()
        {
            if(null == wnd || null == _m_refObj)
                return;

            if (_m_showRefObj != null)
            {
                ALUGUICommon.setLabelTxt(wnd.txtTitle, TextTranslate.instance.getLanguage(_m_showRefObj.event_title));
                ALUGUICommon.setLabelTxt(wnd.txtDesc, TextTranslate.instance.getLanguage(_m_showRefObj.event_desc));
            }

            
            _m_wCondContainer?.showItemList(_m_lDispatchCondRefObjList);
            if (_m_wHeroSelectWnd != null)
            {
                _m_wHeroSelectWnd.showWnd();
                _m_wHeroSelectWnd.refreshWnd(_m_showHeroList, _m_curSelectHeroList, _m_filterType);
            }

            _m_wSelectedHeroContainer?.setShowList(_m_curSelectHeroList, _m_refObj.hero_num);
            _refreshConditionState();
        }
        
        /// <summary>
        /// 刷新条件达成情况
        /// </summary>
        private void _refreshConditionState()
        {
            if (null == _m_refObj )
                return;
            int matchConditionCount = 0;
            foreach (_IHeroCardShow heroInfo in _m_curSelectHeroList)
            {
                bool conditionMatch = true;
                foreach (ChapterEventDispatchConditionInfo condRef in _m_lDispatchCondRefObjList)
                {
                    if(condRef == null || condRef.refObj.condition == null)
                        continue;
                    bool isMath = heroInfo != null && condRef.refObj.condition.IsEnable(heroInfo.heroRefObj, null);
                    condRef.isMatch = isMath;
                    conditionMatch &= isMath;
                }
                if(conditionMatch)
                    matchConditionCount++;
            }

            if (_m_curConditionStageList == null || _m_curConditionStageList.Count != _m_refObj.hero_num)
            {
                _m_curConditionStageList = new List<bool>();
                for (int i = 0; i < _m_refObj.hero_num; i++)
                {
                    _m_curConditionStageList.Add(false);
                }
            }
            for (int i = 0; i < _m_curConditionStageList.Count; i++)
            {
                if (i < matchConditionCount)
                    _m_curConditionStageList[i] = true;
                else
                    _m_curConditionStageList[i] = false;
            }
            _m_wCondStateContainer?.showItemList(_m_curConditionStageList);
            _m_wCondContainer?.showItemList(_m_lDispatchCondRefObjList);
        }
        
        /// <summary>
        /// 点击选择大臣
        /// </summary>
        /// <param name="_heroInfo"></param>
        private void _onHeroSelect(_IHeroCardShow _heroInfo)
        {
            if (null == _m_refObj || _m_curSelectHeroList == null)
                return;

            if (_m_curSelectHeroList.Remove(_heroInfo))
            {
                _m_wHeroSelectWnd?.refreshWnd(_m_showHeroList, _m_curSelectHeroList, _m_filterType);
                _m_wSelectedHeroContainer?.setShowList(_m_curSelectHeroList, _m_refObj.hero_num);
                _refreshConditionState();
                return;
            }

            if (_m_curSelectHeroList.Count >= _m_refObj.hero_num)
            {
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.chapter_event_dispatch_hero_already_max_tip);
                return;
            }
            _m_curSelectHeroList.Add(_heroInfo);
            _m_wHeroSelectWnd?.refreshWnd(_m_showHeroList, _m_curSelectHeroList, _m_filterType);
            _m_wSelectedHeroContainer?.setShowList(_m_curSelectHeroList, _m_refObj.hero_num);
            _refreshConditionState();
        }

        private void _onSelectHeroContainerItemClick(GGUIWndHeroIconNullableItem _item)
        {
            if (_item != null) _onHeroSelect(_item.heroShowData);
        }

        private void _onBtnCloseClick(GameObject _)
        {
        }
        
        /// <summary>
        /// 派遣按钮点击
        /// </summary>
        /// <param name="_"></param>
        private void _onBtnDispatchClick(GameObject _)
        {
            List<long> selectHeroIdList = new List<long>();
            foreach (_IHeroCardShow hero in _m_curSelectHeroList)
            {
                if (hero != null) selectHeroIdList.Add(hero.id);
            }
            _m_onDispatch?.Invoke(selectHeroIdList);
        }

        /// <summary>
        /// 自动派遣按钮点击
        /// </summary>
        /// <param name="_"></param>
        private void _onBtnAutoDispatchClick(GameObject _)
        {
            if (_m_refObj == null || _m_showHeroList == null || _m_showHeroList.Count == 0 || _m_refObj.hero_num <= 0)
                return;
            List<long> selectHeroIdList = new List<long>();

            // 遍历所有大臣，按排序顺序选择大臣
            for (int i = 0; i < _m_showHeroList.Count; i++)
            {
                if(_m_showHeroList[i] == null)
                    continue;
                selectHeroIdList.Add(_m_showHeroList[i].id);
                if(selectHeroIdList.Count >= _m_refObj.hero_num)
                    break;
            }

            _m_onDispatch?.Invoke(selectHeroIdList);
        }

    }
}