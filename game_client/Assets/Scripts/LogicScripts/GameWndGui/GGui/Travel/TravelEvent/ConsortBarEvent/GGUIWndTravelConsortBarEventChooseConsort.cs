using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 酒馆事件
    /// </summary>
    public class GGUIWndTravelConsortBarEventChooseConsort : _ANPGGUIBasicWnd<GGUIMonoTravelConsortBarEventChooseConsort>
    {
        [NotNull] private TravelConsortBarEventInfo _m_travelConsortBarEventInfo;//酒馆事件信息
        [NotNull] private List<ConsortInfo_UnlockNotAutoRefresh> _m_lCanSelectConsortList = new List<ConsortInfo_UnlockNotAutoRefresh>();//可选的妃子列表
        [NotNull] private Dictionary<long, ETravelConsortUnlockStat> _m_dConsortUnlockStat = new Dictionary<long, ETravelConsortUnlockStat>();//妃子解锁状态

        [NotNull] private List<GGUIWndTarvelConsortItem> _m_lTravelConsortItemList = new List<GGUIWndTarvelConsortItem>();//选择妃子item列表
        private GGUIWndTravelConsortBarEventChooseCost _m_wTravelConsortBarEventChooseCostWnd = null;//消耗选择窗口

        private long _m_lWndShowAnimationSerializeId;
        
        public GGUIWndTravelConsortBarEventChooseConsort([NotNull] TravelConsortBarEventInfo _travelConsortBarEventInfo) : base(EALUIWndLayer.ADDITION)
        {
            _m_travelConsortBarEventInfo = _travelConsortBarEventInfo;
            
            _m_lCanSelectConsortList.Clear();
            _m_dConsortUnlockStat.Clear();
            if (_m_travelConsortBarEventInfo != null && _m_travelConsortBarEventInfo.consortBarEventRefObj != null &&
                _m_travelConsortBarEventInfo.consortBarEventRefObj.trigger_consort_id_list != null)
            {
                foreach (long consortId in _m_travelConsortBarEventInfo.consortBarEventRefObj.trigger_consort_id_list)
                {
                    _m_lCanSelectConsortList.Add(new ConsortInfo_UnlockNotAutoRefresh(consortId));
                    _m_dConsortUnlockStat[consortId] = NPPlayer.instance.travelComp.getTravelConsortUnlockStat(consortId);
                }
            }
        }

        protected override string _monoAssetPath { get { return GGUIMonoTravelConsortBarEventChooseConsort.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoTravelConsortBarEventChooseConsort.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        
        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            if (wnd.travelConsortItemList != null)
            {
                GGUIWndTarvelConsortItem itemWnd = null;
                foreach (var itemMono in wnd.travelConsortItemList)
                {
                    if(itemMono == null)
                        continue;
                    
                    itemWnd = new GGUIWndTarvelConsortItem(itemMono);
                    itemWnd.onItemClick += _onConsortItemClick;
                    
                    _m_lTravelConsortItemList.Add(itemWnd);
                }
            }

            if (wnd.monoChooseCost != null)
            {
                _m_wTravelConsortBarEventChooseCostWnd = new GGUIWndTravelConsortBarEventChooseCost(wnd.monoChooseCost);
                _m_wTravelConsortBarEventChooseCostWnd.onChooseCost += _onChooseCost;
                _m_wTravelConsortBarEventChooseCostWnd.onCloseWndBtnClick += _doCloseChooseCostWnd;
            }
            
            ALUGUICommon.combineBtnClick(wnd.skipShowAniBtn, _onSkipShowAnimationBtnClick);
        }
        
        protected override void _onDiscard()
        {
            if(wnd != null)
                ALUGUICommon.uncombineBtnClick(wnd.skipShowAniBtn, _onSkipShowAnimationBtnClick);
                
            
            dealAllConsortItemWnd((_itemWnd) =>
            {
                if (_itemWnd == null)
                    return;

                _itemWnd.onItemClick -= _onConsortItemClick;
                _itemWnd.discard();
            });
            _m_lTravelConsortItemList.Clear();

            _m_lCanSelectConsortList.Clear();
            _m_dConsortUnlockStat.Clear();

            if (_m_wTravelConsortBarEventChooseCostWnd != null)
            {
                _m_wTravelConsortBarEventChooseCostWnd.onChooseCost -= _onChooseCost;
                _m_wTravelConsortBarEventChooseCostWnd.onCloseWndBtnClick -= _doCloseChooseCostWnd;
                
                _m_wTravelConsortBarEventChooseCostWnd.discard();
                _m_wTravelConsortBarEventChooseCostWnd = null;    
            }
        }
        
        protected override void _onShowWnd()
        {
            _playShowAnimation();
            
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            _m_lWndShowAnimationSerializeId = ALSerializeOpMgr.next();
            
            dealAllConsortItemWnd((_itemWnd) =>
            {
                _itemWnd?.hideWnd();
            });

            _m_wTravelConsortBarEventChooseCostWnd?.hideWnd();
        }

        protected override void _onReset()
        {
            dealAllConsortItemWnd((_itemWnd) =>
            {
                _itemWnd?.resetWnd();
            });
            
            _m_wTravelConsortBarEventChooseCostWnd?.resetWnd();
        }

        private void _refreshWnd()
        {
            GGUIWndTarvelConsortItem itemWnd = null;
            ConsortInfo_UnlockNotAutoRefresh consortShowInfo = null;
            for (int i = 0; i < _m_lTravelConsortItemList.Count; i++)
            {
                itemWnd = _m_lTravelConsortItemList[i];
                if(itemWnd == null)
                    continue;
                
                // 若窗口数量大于可选妃子数量，则隐藏多余的窗口
                if (i >= _m_lCanSelectConsortList.Count)
                {
                    itemWnd.hideWnd();
                    continue;
                }

                consortShowInfo = _m_lCanSelectConsortList[i];
                if (consortShowInfo == null)
                {
                    itemWnd.hideWnd();
                    continue;
                }
                
                itemWnd.showWnd();
                itemWnd.setInfo(consortShowInfo, _getConsortUnlockStat(consortShowInfo.consortId));
            }

            _doCloseChooseCostWnd();
        }
        
        private ETravelConsortUnlockStat _getConsortUnlockStat(long _consortId)
        {
            if (_m_dConsortUnlockStat.TryGetValue(_consortId, out ETravelConsortUnlockStat _stat))
                return _stat;

            return NPPlayer.instance.travelComp.getTravelConsortUnlockStat(_consortId);
        }
        
        private void dealAllConsortItemWnd(Action<GGUIWndTarvelConsortItem> _action)
        {
            if(_action == null)
                return;

            foreach (var itemWnd in _m_lTravelConsortItemList)
            {
                _action(itemWnd);
            }
        }
        
        /// <summary>
        /// item被点击回调
        /// </summary>
        /// <param name="_itemWnd"></param>
        private void _onConsortItemClick(GGUIWndTarvelConsortItem _itemWnd)
        {
            if(_itemWnd == null)
                return;

            _IConsortShowInfo consortShowInfo = _itemWnd.consortShowInfo;
            _m_travelConsortBarEventInfo.selectConsortShowInfo = consortShowInfo;

            _showChooseCostWnd();
        }

        /// <summary>
        /// 显示消耗道具子窗口
        /// </summary>
        private void _showChooseCostWnd()
        {
            if(_m_travelConsortBarEventInfo.selectConsortShowInfo == null)
                return;
            
            if (_m_wTravelConsortBarEventChooseCostWnd != null)
            {
                _m_wTravelConsortBarEventChooseCostWnd.showWnd();
                _m_wTravelConsortBarEventChooseCostWnd.setData(_m_travelConsortBarEventInfo, _m_travelConsortBarEventInfo.selectConsortShowInfo, 
                    _getConsortUnlockStat(_m_travelConsortBarEventInfo.selectConsortShowInfo.consortId));

                if (wnd != null)
                {
                    ALUGUICommon.setGameObjEnable(wnd.onShowChooseCostWndShowGoList, true);
                    ALUGUICommon.setGameObjEnable(wnd.onShowChooseCostWndHideGoList, false);
                }
            }
        }
        
        /// <summary>
        /// 关闭选择消耗窗口
        /// </summary>
        private void _doCloseChooseCostWnd()
        {
            _m_wTravelConsortBarEventChooseCostWnd?.hideWnd();

            if (wnd != null)
            {
                ALUGUICommon.setGameObjEnable(wnd.onShowChooseCostWndShowGoList, false);
                ALUGUICommon.setGameObjEnable(wnd.onShowChooseCostWndHideGoList, true);
            }
        }

        /// <summary>
        /// 当选择了消耗物品时
        /// </summary>
        /// <param name="_costRefObj"></param>
        private void _onChooseCost(TravelEventConsortBarCostRefObj _costRefObj)
        {
            _m_travelConsortBarEventInfo.selectBarCostRefObj = _costRefObj;
            
            // 选择消耗物品后, 退出
            _doClose();
        }

        private void _doClose()
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_TRAVEL_CONSORT_BAR_EVNENT_CHOOSE_CONSORT);
        }

        #region 窗口显示动画

        /// <summary>
        /// 播放窗口显示动画
        /// </summary>
        private void _playShowAnimation()
        {
            if(wnd == null)
                return;
            
            long showSerializeId = _m_lWndShowAnimationSerializeId = ALSerializeOpMgr.next();
            if (wnd.wndAnimation != null && !string.IsNullOrEmpty(wnd.showAnimationName))
            {
                wnd.wndAnimation.Sample(wnd.showAnimationName, 0f);
                wnd.wndAnimation.Play(wnd.showAnimationName, () =>
                {
                    if(showSerializeId != _m_lWndShowAnimationSerializeId)
                        return;

                    _onShowAnimationComplete();
                });
            }
            else
            {
                _onShowAnimationComplete();
            }
        }

        /// <summary>
        /// 当窗口显示动画播放完成
        /// </summary>
        private void _onShowAnimationComplete()
        {
            _m_lWndShowAnimationSerializeId = ALSerializeOpMgr.next();
            
            if(wnd == null || wnd.afterShowAniShieldOpTime <= 0)
                return;
            
            //打开操作遮罩
            int inputMaskSerialize = MainCameraMono.selfInstance.openAllInputMask();
            CommonTaskController.CommonActionAddMonoTask(() =>
            {
                MainCameraMono.selfInstance.closeAllInputMask(inputMaskSerialize);
            }, wnd.afterShowAniShieldOpTime);
        }

        /// <summary>
        /// 跳过显示动画按钮被点击
        /// </summary>
        private void _onSkipShowAnimationBtnClick(GameObject _go)
        {
            if (wnd != null && wnd.wndAnimation != null && !string.IsNullOrEmpty(wnd.showAnimationName))
            {
                wnd.wndAnimation.Sample(wnd.showAnimationName, 1f);
            }
            
            _onShowAnimationComplete();
        }
        
        #endregion
        
        public static void showTravelConsortBarEventWnd([NotNull] TravelConsortBarEventInfo _travelConsortBarEventInfo, Action _onClose)
        {
            if (_travelConsortBarEventInfo.consortBarEventRefObj == null)
            {
                _onClose?.Invoke();
                return;
            }
            
            GGUIWndTravelConsortBarEventChooseConsort wnd = new GGUIWndTravelConsortBarEventChooseConsort(_travelConsortBarEventInfo);
            
            QueueMgr.instance.AddNode(new BaseOnAddContainerSceneUIWndQueueNode(EUIQueueStageType.MAIN, UINodeTagConst.C_TRAVEL_CONSORT_BAR_EVNENT_CHOOSE_CONSORT, false
                , true, false, null, wnd, true, false,
                null, null, null, () =>
                {
                    wnd.discard();
                    wnd = null;
                    
                    _onClose?.Invoke();
                }));
        }
    }
}