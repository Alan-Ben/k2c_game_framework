using System;
using System.Collections.Generic;
using System.Linq;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class GGUIWndConsortCGTabWnd
    {
        public EConsortCGType cgType;
        public string tabNameKey;
        public GGUIWndConsortCGTab tabWnd;
        public GGUIWndConsortCGGrid cgGrid;

        public GGUIWndConsortCGTabWnd([NotNull] ConsortCgTabSetting _cgTabSetting, Action<GGUIWndConsortCGTab> _onTabClick)
        {
            cgType = _cgTabSetting.cgType;
            tabNameKey = _cgTabSetting.tabNameKey;

            if (_cgTabSetting.tabMono != null)
            {
                tabWnd = new GGUIWndConsortCGTab(_cgTabSetting.tabMono, cgType);
                tabWnd.onClickTab += _onTabClick;
            }
            
            if(_cgTabSetting.cgGrid != null)
                cgGrid = new GGUIWndConsortCGGrid(_cgTabSetting.cgGrid);
        }
    }
    
    public class GGUIWndConsortCGMain : _ANPGGUIBasicResBarWnd<GGUIMonoConsortCGMain>
    {
        private static GGUIWndConsortCGMain _g_instance = null;
        public static GGUIWndConsortCGMain instance { get { return _g_instance ??= new GGUIWndConsortCGMain(); } }

        [NotNull] private Dictionary<EConsortCGType, GGUIWndConsortCGTabWnd> _m_dCgTypeTabWndDic = new Dictionary<EConsortCGType, GGUIWndConsortCGTabWnd>();
        
        private EConsortCGType _m_eCurSelectCgType = EConsortCGType.INVITE;//当前选中的cg类型
        [NotNull] private Dictionary<EConsortCGType, List<ConsortCGRefObj>> _m_dCgRefObjDic = new Dictionary<EConsortCGType, List<ConsortCGRefObj>>();//cg数据列表
        [NotNull] private Dictionary<ConsortCGRefObj, ConsortCgInfo> _m_dGetCGInfoDic = new Dictionary<ConsortCGRefObj, ConsortCgInfo>();//已获取的cg信息字典
        
        public GGUIWndConsortCGMain() : base(EALUIWndLayer.NORMAL)
        {
        }
        
        protected override string _monoAssetPath { get { return GGUIMonoConsortCGMain.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoConsortCGMain.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            _initTabWnd();
            
            foreach (var consortCgInfo in NPPlayer.instance.consortComp.consortCgInfoList)
            {
                if (consortCgInfo != null && consortCgInfo.consortCgRefObj != null)
                    _m_dGetCGInfoDic[consortCgInfo.consortCgRefObj] = consortCgInfo;
            }
            
            ALUGUICommon.combineBtnClick(wnd.btnReturn, _onReturnBtnClick);
        }
        
        protected override void _onDiscard()
        {
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnReturn, _onReturnBtnClick);
            }

            _discardTabWnd();
            
            _m_dCgRefObjDic.Clear();
            _m_dGetCGInfoDic.Clear();
        }
        
        protected override void _onShowWnd()
        {
            _dealAllTabWnd((_tabWnd) =>
            {
                if(_tabWnd == null)
                    return;
                
                _tabWnd.tabWnd?.showWnd();
            });
            
            // 刷新选中的tab页面
            _refreshSelectTab();
        }

        protected override void _onHideWnd()
        {
            _dealAllTabWnd((_tabWnd) =>
            {
                if(_tabWnd == null)
                    return;
                
                _tabWnd.tabWnd?.hideWnd();
                _tabWnd.cgGrid?.hideWnd();
            });
        }

        protected override void _onReset()
        {
            _dealAllTabWnd((_tabWnd) =>
            {
                if(_tabWnd == null)
                    return;
                
                _tabWnd.tabWnd?.resetWnd();
                _tabWnd.cgGrid?.resetWnd();
            });
        }

        private void _initTabWnd()
        {
            _discardTabWnd();

            if(wnd == null || wnd.cgTypeSetting == null)
                return;

            foreach (var tabSetting in wnd.cgTypeSetting)
            {
                if(tabSetting == null)
                    continue;
                
                GGUIWndConsortCGTabWnd tabWnd = new GGUIWndConsortCGTabWnd(tabSetting, _onTabClick);
                _m_dCgTypeTabWndDic[tabWnd.cgType] = tabWnd;

                if (tabWnd.cgGrid != null)
                {
                    tabWnd.cgGrid.onClickDrawReward += _onClickDrawCGReward;
                }
            }
        }

        private void _discardTabWnd()
        {
            foreach (GGUIWndConsortCGTabWnd item in _m_dCgTypeTabWndDic.Values)
            {
                if(item == null)
                    continue;

                if (item.tabWnd != null)
                {
                    item.tabWnd.discard();
                }
                item.tabWnd = null;

                if (item.cgGrid != null)
                {
                    item.cgGrid.onClickDrawReward -= _onClickDrawCGReward;
                    item.cgGrid.discard();
                }
                item.cgGrid = null;
            }
            
            _m_dCgTypeTabWndDic.Clear();
        }

        private void _dealAllTabWnd(Action<GGUIWndConsortCGTabWnd> _action)
        {
            if(_action == null)
                return;

            _m_dCgTypeTabWndDic.Values.ForEach(_action);
        }

        private void _onTabClick(GGUIWndConsortCGTab _tab)
        {
            if(_tab == null)
                return;
            
            setSelectCgTabType(_tab.tabType, false);
        }
        
        /// <summary>
        /// 设置选中tab
        /// </summary>
        public void setSelectCgTabType(EConsortCGType _type, bool _forceRefresh)
        {
            if(_m_eCurSelectCgType == _type && !_forceRefresh)
                return;
            
            _m_eCurSelectCgType = _type;
            if(wnd == null || !isShow)
                return;

            _refreshSelectTab();
        }

        public void _refreshSelectTab()
        {
            if(_m_dCgTypeTabWndDic.Count <= 0)
                return;
            
            if(!_m_dCgTypeTabWndDic.TryGetValue(_m_eCurSelectCgType, out GGUIWndConsortCGTabWnd tabWnd))
            {
                // 若找不到对应的tab类型配置
                _m_eCurSelectCgType = wnd == null || wnd.defaultCgType == EConsortCGType.NONE ? EConsortCGType.INVITE : wnd.defaultCgType;
                if (!_m_dCgTypeTabWndDic.TryGetValue(_m_eCurSelectCgType, out tabWnd))
                {
                    tabWnd = _m_dCgTypeTabWndDic.Values.FirstOrDefault();
                    if(tabWnd != null)
                        _m_eCurSelectCgType = tabWnd.cgType;
                }
            }

            if (tabWnd == null)
                return;

            if (!_m_dCgRefObjDic.TryGetValue(_m_eCurSelectCgType, out List<ConsortCGRefObj> cgRefList) || cgRefList == null)
            {
                cgRefList = new List<ConsortCGRefObj>();
                GRefdataCoreMgr.instance.getConsortCGRefList(_m_eCurSelectCgType, cgRefList);
                _sortCG(cgRefList);
                
                _m_dCgRefObjDic[_m_eCurSelectCgType] = cgRefList;
            }
            
            _dealAllTabWnd((item) =>
            {
                if(item == null || item.cgType == _m_eCurSelectCgType)
                    return;
               
                item.tabWnd?.setSelected(item.cgType == _m_eCurSelectCgType);
                item.cgGrid?.hideWnd();
            });
            
            tabWnd.tabWnd?.setSelected(true);

            if (tabWnd.cgGrid != null)
            {
                tabWnd.cgGrid.showWnd();
                tabWnd.cgGrid.setData(cgRefList);
            }

            if (wnd != null)
            {
                ALUGUICommon.setLabelTxt(wnd.txtSelectTabName, TextTranslate.instance.getLanguage(tabWnd.tabNameKey));
                
                int unlockCount = 0;
                foreach (var cgRefObj in cgRefList)
                {
                    if (cgRefObj != null && NPPlayer.instance.consortComp.getConsortCgInfo(cgRefObj.cg_id) != null)
                        unlockCount++;
                }

                if (string.IsNullOrEmpty(wnd.collectionProgressKey))
                {
                    ALUGUICommon.setLabelTxt(wnd.txtCollectionProgress, TextTranslate.instance.getLanguage(TransKeyConst.common_currentTotalNum_num_num, unlockCount, cgRefList.Count));
                }
                else
                {
                    ALUGUICommon.setLabelTxt(wnd.txtCollectionProgress, TextTranslate.instance.getLanguage(wnd.collectionProgressKey, unlockCount, cgRefList.Count));
                }
            }
        }

        // 对cgRefList进行排序, 按照 已获取>未获取 -> 未领取奖励>已领取奖励 -> cg_id 升序 排序
        private void _sortCG(List<ConsortCGRefObj> _cgRefList)
        {
            if(_cgRefList == null)
                return;
            
            _cgRefList.Sort((_a, _b) =>
            {
                if (_b == null) return -1;
                if (_a == null) return 1;
                if(ReferenceEquals(_a, _b)) return 0;
                    
                _m_dGetCGInfoDic.TryGetValue(_a, out ConsortCgInfo consortCgInfo1);
                _m_dGetCGInfoDic.TryGetValue(_b, out ConsortCgInfo consortCgInfo2);
                bool getCG1 = consortCgInfo1 != null;
                bool getCG2 = consortCgInfo2 != null;

                if (getCG1 != getCG2)
                    return -getCG1.CompareTo(getCG2);

                if(consortCgInfo1 != null && consortCgInfo2 != null && consortCgInfo1.rewarded != consortCgInfo2.rewarded)
                {
                    return consortCgInfo1.rewarded.CompareTo(consortCgInfo2.rewarded);
                }
                    
                return _a.cg_id.CompareTo(_b.cg_id);
            });
        }
        
        /// <summary>
        /// 当返回按钮被点击时
        /// </summary>
        /// <param name="_go"></param>
        private void _onReturnBtnClick(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_CONSORT_CG_MAIN);
        }

        /// <summary>
        /// 点击CG奖励
        /// </summary>
        private void _onClickDrawCGReward(GGUIWndConsortCGGridItem _item)
        {
            if(_item == null || _item.consortCGRefObj == null)
                return;
            
            NPPlayer.instance.consortComp.reqGetCgUnlockReward(_item.consortCGRefObj.cg_id, (_msg) =>
            {
                if(_m_dCgRefObjDic.TryGetValue(_m_eCurSelectCgType, out List<ConsortCGRefObj> cgRefList) && cgRefList != null)
                    _sortCG(cgRefList);
                
                _refreshSelectTab();
            }, null);
        }
    }
}