using System;
using System.Collections.Generic;
using ALPackage;
using Common.EventObj;
using CommonEnum;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 关卡派遣事件选择大臣页面
    /// </summary>
    public class GGUIWndCommonSimpleDispatchEventSelectHero : _AGGUIWndCommonDispatchEvent<GGUIMonoCommonSimpleDispatchEventSelectHero>
    {
        private static GGUIWndCommonSimpleDispatchEventSelectHero _g_instance;
        public static GGUIWndCommonSimpleDispatchEventSelectHero instance
        {
            get
            {
                if (_g_instance == null)
                    _g_instance = new GGUIWndCommonSimpleDispatchEventSelectHero();
                return _g_instance;
            }
        }
        
        private GGUIWndHeroCommonSimpleDispatchSelectHeroGrid _m_wHeroCardGrid;//大臣卡片列表
        // private GGUIWndHeroAttrMutexFilter _m_wAttrFilter;//属性过滤器

        private bool _m_bStartSelectHero;//是否开始选择大臣

        [NotNull] private List<HeroSatisfyConditionCount> _m_lFilteredHeroShowList = new List<HeroSatisfyConditionCount>();//过滤后要显示的大臣数据列表
        
        protected override string _monoAssetPath { get { return GGUIMonoCommonSimpleDispatchEventSelectHero.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoCommonSimpleDispatchEventSelectHero.objName; } }

        public GGUIWndCommonSimpleDispatchEventSelectHero() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override void _onInitDoneDispatchEventWnd()
        {
            if(wnd == null)
                return;

            if (wnd.monoHeroCardGrid != null)
            {
                _m_wHeroCardGrid = new GGUIWndHeroCommonSimpleDispatchSelectHeroGrid(wnd.monoHeroCardGrid);
                _m_wHeroCardGrid.onItemClick += _onHeroCardClick;
            }
        }

        protected override void _onDiscardDispatchEventWnd()
        {
            if (_m_wHeroCardGrid != null)
            {
                _m_wHeroCardGrid.onItemClick -= _onHeroCardClick;
                _m_wHeroCardGrid.discard();
                _m_wHeroCardGrid = null;
            }

            // if (_m_wAttrFilter != null)
            // {
            //     _m_wAttrFilter.onTabSelect -= _onFilterTabClick;
            //     _m_wAttrFilter.discard();
            //     _m_wAttrFilter = null;    
            // }
            
            _m_lFilteredHeroShowList.Clear();
        }

        protected override void _onShowWndEventWnd()
        {
            // _m_wAttrFilter?.showWnd();
        }

        protected override void _onHideWndEventWnd()
        {
            _m_wHeroCardGrid?.hideWnd();
            // _m_wAttrFilter?.hideWnd();
            
            _m_lFilteredHeroShowList.Clear();
        }

        protected override void _onResetEventWnd()
        {
            _m_wHeroCardGrid?.resetWnd();
            // _m_wAttrFilter?.resetWnd();
            
            _m_lFilteredHeroShowList.Clear();
        }

        protected override void _onSetEventDataSubWnd()
        {
            _initHeroInfoList();//初始化显示数据

            _m_bStartSelectHero = false;//设置是否开始选择大臣为false
        }

        protected override void _refreshWndEventWnd()
        {
            if (wnd == null || _m_iDispatchEventShowAgent == null)
                return;
            
            // _refreshHeroCardGrid();在刷新_m_wAttrFilter时会调用_refreshHeroCardGrid所以这里不用刷新了
            // if(_m_wAttrFilter != null)
            //     _m_wAttrFilter.refreshWnd(true);

            _refreshStartSelectHero();
        }

        private void _refreshStartSelectHero()
        {
            if (wnd == null)
                return;
            
            ALUGUICommon.setGameObjEnable(wnd.startSelectHeroShowList, _m_bStartSelectHero);
            ALUGUICommon.setGameObjEnable(wnd.startSelectHeroHideList, !_m_bStartSelectHero);
        }

        #region 数据设置与初始化

        /// <summary>
        /// 初始化所有大臣信息列表
        /// </summary>
        private void _initHeroInfoList()
        {
        }

        /// <summary>
        /// 更新被过滤过的大臣数据列表
        /// </summary>
        /// <param name="_attrType">需要过滤的类型</param>
        private void _updateFilteredHeroInfoList(EBasicAttrType _attrType)
        {
            _m_lFilteredHeroShowList.Clear();
            
            _m_lHeroSatisfyConditionCountList.ForEach((_heroSatisfyConditionCount) =>
            {
                if(_heroSatisfyConditionCount.heroShowInfo == null || _heroSatisfyConditionCount.heroShowInfo.heroRefObj == null)
                    return;
                
                // if(_attrType == EBasicAttrType.NONE || GRefdataCoreMgr.instance.heroHasSpecialAttr(_heroSatisfyConditionCount.heroShowInfo.heroRefObj, _attrType))
                //     _m_lFilteredHeroShowList.Add(_heroSatisfyConditionCount);
            });
        }

        #endregion

        #region 窗口刷新

        /// <summary>
        /// 刷新大臣选择Grid
        /// </summary>
        private void _refreshHeroCardGrid()
        {
            if (_m_wHeroCardGrid != null)
            {
                _m_wHeroCardGrid.showWnd();
                _m_wHeroCardGrid.setShowInfoList(_m_iDispatchEventShowAgent, _m_lFilteredHeroShowList, selectedHeroShowList);
            }
        }
        
        #endregion

        #region 窗口事件

        /// <summary>
        /// 当点击大臣卡片时
        /// </summary>
        private void _onHeroCardClick(GGUIWndHeroCommonSimpleDispatchSelectHeroItem _item)
        {
            if(_item == null || _item.heroCardShowInfo == null || _item.heroCardShowInfo.heroRefObj == null || _m_wHeroCardGrid == null || _m_iDispatchEventShowAgent == null)
                return;

            long heroId = _item.heroCardShowInfo.heroRefObj.id;

            bool findHeroShowInfo = _findHeroShowInfo(_m_lFilteredHeroShowList, heroId, out HeroSatisfyConditionCount _clickHeroSatisfyConditionCount);
            // 若在过滤后的大臣数据列表中没有该大臣数据
            if (!findHeroShowInfo || _clickHeroSatisfyConditionCount.heroShowInfo == null)
            {
                Debug.LogError($"[GGUIWndCommonDispatchEventSelectHero _m_wHeroCardGrid:_onHeroCardClick] 在过滤后的大臣显示数据列表_m_lFilteredHeroShowList中找不到大臣:{heroId}, " +
                               $"这本是不应该出现的情况因为_m_wHeroCardGrid的显示数据来源就是_m_lFilteredHeroShowList, 请检查哪里出了错误");
                return;
            }

            if (_item.curState == ECommonSelectState.SELECTED)//若item当前状态是选中状态
            {
                _m_wHeroCardGrid?.setHeroSelect(_item.heroCardShowInfo, false);//将item取消选中
                
                // 若在选中大臣列表数据中没有找到该大臣
                if (_findHeroShowInfo(selectedHeroShowList, heroId) == null)
                {
                    Debug.LogError($"[GGUIWndCommonDispatchEventSelectHero _m_wHeroCardGrid:_onHeroCardClick] 在选中的大臣显示数据列表selectedHeroShowList中找不到大臣:{heroId}, " +
                                   $"这本是不应该出现的情况因为_m_wHeroCardGrid的显示数据来源是_m_lFilteredHeroShowList,_m_lFilteredHeroShowList数据来源是_m_lHeroSatisfyConditionCountList, " +
                                   $"而选中的大臣数据列表selectedHeroShowList数据来源也是_m_lHeroSatisfyConditionCountList, 不应该出现_m_lFilteredHeroShowList中被选中的数据不包含在selectedHeroShowList中, 请检查哪里出了错误");
                }
                else
                {
                    // 移除该选中大臣
                    selectedHeroShowList.Remove((_heroShowInfo) =>
                    {
                        if (_heroShowInfo != null && _heroShowInfo.heroRefObj != null &&
                            _heroShowInfo.heroRefObj.id == heroId)
                            return true;

                        return false;
                    });
                }
            }
            else//若item当前是未被选中状态
            {
                if (selectedHeroShowList.Count >= _m_iDispatchEventShowAgent.canDispatchMaxHeroNum)//若当前已选中大臣数量已达最大数量
                {
                    NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.commonEvent_dispatchEventSelectHeroMax_none);
                    return;
                }
                
                _m_wHeroCardGrid?.setHeroSelect(_item.heroCardShowInfo, true);//将item选中
                
                // 若在选中大臣列表数据中没有找到该大臣
                if (_findHeroShowInfo(selectedHeroShowList, heroId) == null)
                {
                    selectedHeroShowList.Add(_clickHeroSatisfyConditionCount.heroShowInfo);//加入选中大臣列表
                }
                else
                {
                    Debug.LogError($"[GGUIWndCommonDispatchEventSelectHero _m_wHeroCardGrid:_onHeroCardClick] 在选中的大臣显示数据列表selectedHeroShowList中找到了大臣:{heroId}, " +
                                   $"这本是不应该出现的情况因为点击item时该大臣是未被选中状态, 但是却在选中大臣列表selectedHeroShowList中找到了该大臣, 可能哪里出现的问题, 请检测");
                }
            }
            
            _refreshCondContainer(false);//更新条件列表显示
            _refreshHeroHeadContainer();//刷新选中大臣头像列表
            _refreshDispatchBtn();//刷新派遣按钮
        }

        /// <summary>
        /// 当点击大臣头像icon时
        /// </summary>
        // protected override void _onClickHeroHeadIconSub(GGUIWndHeroIconNullableItem _item)
        // {
        //     // 若点击item为空 或 点击item有大臣数据但是对应的配表数据为空 直接返回
        //     if(_item == null || (_item.heroShowData != null && _item.heroShowData.heroRefObj == null))
        //         return;
        //
        //     if (_item.heroShowData == null)
        //     {
        //         if (!_m_bStartSelectHero)//若还没有进入选择大臣状态, 进入选择大臣状态
        //         {
        //             _m_bStartSelectHero = true;
        //             _refreshStartSelectHero();
        //         }
        //         else//若已经进入选择大臣状态, 则弹出tip提示玩家选择大臣
        //         {
        //             NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.commonEvent_dispatchEventSelectHeroTip_none);
        //         }
        //         return;
        //     }
        //     
        //     long heroId = _item.heroShowData.heroRefObj.id;
        //     _IHeroCardShow heroShowInfo = _findHeroShowInfo(selectedHeroShowList, heroId);
        //     // 若没有在选中的大臣数据列表中找到对应大臣数据, 直接返回
        //     if (heroShowInfo == null)
        //     {
        //         Debug.LogError($"[GGUIWndCommonDispatchEventSelectHero _m_wHeroHeadContainer:_onClickHeroHeadIcon] 在选中的大臣列表selectedHeroShowList中没有找到大臣:{heroId}" +
        //                        $"这本是不该出现的情况, 因为选中大臣的头像_m_wHeroHeadContainer数量来源是selectedHeroShowList");
        //         
        //         return;
        //     }
        //
        //     selectedHeroShowList.Remove(heroShowInfo);//从选中大臣列表中移除
        //
        //     if (_m_wHeroCardGrid != null)
        //     {
        //         _m_wHeroCardGrid.setHeroSelect(heroShowInfo, false);//在大臣卡片列表中尝试将其设置为未选中
        //     }
        //
        //     _refreshHeroHeadContainer();//刷新选中大臣头像列表
        //     _refreshCondContainer(false);//更新条件列表显示
        //     _refreshDispatchBtn();//刷新派遣按钮
        // }

        /// <summary>
        /// 当过滤tab被点击时
        /// </summary>
        /// <param name="_attrType"></param>
        private void _onFilterTabClick(EBasicAttrType _attrType)
        {
            _updateFilteredHeroInfoList(_attrType);//更新过滤大臣数据列表
            _refreshHeroCardGrid();//刷新卡片列表
        }
        
        /// <summary>
        /// 一键派遣按钮被点击
        /// </summary>
        protected override void _onAKeyDispatchBtnClickSub()
        {
            _refreshHeroCardGrid();//刷新大臣卡片列表
        }
        
        #endregion
    }
}