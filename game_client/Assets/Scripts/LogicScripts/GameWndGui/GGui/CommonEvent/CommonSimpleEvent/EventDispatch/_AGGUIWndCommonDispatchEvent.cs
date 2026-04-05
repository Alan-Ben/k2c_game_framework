using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class CommonDispatchEventConditionShowInfo : _IConditionDescShow
    {
        private CommonEventDispatchCondRefObj _m_rDispatchCondRefObj;//派遣事件的配表数据
        private List<_IHeroCardShow> _m_lSelectHeroShowInfoList;//选中的大臣id列表
        
        public NPGTextureIndex conditionIcon { get { return _m_rDispatchCondRefObj?.icon; } }
        public string conditionDesc
        {
            get
            {
                return _m_rDispatchCondRefObj == null
                    ? string.Empty
                    : TextTranslate.instance.getLanguage(_m_rDispatchCondRefObj.desc, _m_rDispatchCondRefObj.desc_args);
            }
        }

        public bool conditionIsEnable
        {
            get
            {
                return GRefdataCoreMgr.checkDispatchEventConditionIsEnable(_m_rDispatchCondRefObj, _m_lSelectHeroShowInfoList);
            }
        }
        public void jumpFunc()
        {
        }

        public CommonDispatchEventConditionShowInfo(CommonEventDispatchCondRefObj _conditionRefObj, List<_IHeroCardShow> _heroShowInfoList)
        {
            _m_rDispatchCondRefObj = _conditionRefObj;
            _m_lSelectHeroShowInfoList = _heroShowInfoList;
        }
    }
    
    public abstract class _AGGUIWndCommonDispatchEvent<T_MONO> : _AGGUIWndCommonSimpleEvent<T_MONO> where T_MONO : _AGGUIMonoCommonDispatchEvent
    {
        protected CommonSimpleDispatchEventAgent _m_iDispatchEventShowAgent;//派遣事件信息

        private NPGGuiWndTexture _m_wEventIcon;//事件icon
        private GGUIWndConditionDescContainer _m_wCondContainer;//条件Container
        // private GGUIWndHeroIconNullableItemContainer _m_wHeroHeadContainer;//选中大臣头像列表
        
        private List<_IConditionDescShow> _m_lConditionShowInfo;//达成条件显示信息
        [NotNull] protected List<HeroSatisfyConditionCount> _m_lHeroSatisfyConditionCountList = new List<HeroSatisfyConditionCount>();//所拥有的大臣满足的条件数量列表

        public _AGGUIWndCommonDispatchEvent(EALUIWndLayer _layer) : base(_layer)
        {
        }

        [NotNull] protected List<_IHeroCardShow> selectedHeroShowList { get { return GUIAddSceneCommonDispatchEvent.instance.selectedHeroShowList; } } //选中的大臣id列表
        
        protected override void _onInitDoneSub()
        {
            if (wnd != null)
            {
                if (wnd.monoCondContainer != null)
                    _m_wCondContainer = new GGUIWndConditionDescContainer(wnd.monoCondContainer);

                if (wnd.eventIcon != null)
                    _m_wEventIcon = new NPGGuiWndTexture(wnd.eventIcon);
                
                ALUGUICommon.combineBtnClick(wnd.btnDispatch, _onDispatchBtnClick);
                ALUGUICommon.combineBtnClick(wnd.btnClose, _onCloseBtnClick);
                ALUGUICommon.combineBtnClick(wnd.btnAkeyDispatch, _onAKeyDispatchBtnClick);
            }
            
            _onInitDoneDispatchEventWnd();
        }

        protected override void _onDiscardSub()
        {
            _m_wCondContainer?.discard();
            _m_wCondContainer = null;
            
            // if (_m_wHeroHeadContainer != null)
            // {
            //     _m_wHeroHeadContainer.onItemClick -= _onClickHeroHeadIcon;
            //     _m_wHeroHeadContainer.discard();
            //     _m_wHeroHeadContainer = null;
            // }
            
            _m_wEventIcon?.discard();
            _m_wEventIcon = null;
            
            _m_lConditionShowInfo?.Clear();
            _m_lHeroSatisfyConditionCountList.Clear();

            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnDispatch, _onDispatchBtnClick);
                ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onCloseBtnClick);
                ALUGUICommon.uncombineBtnClick(wnd.btnAkeyDispatch, _onAKeyDispatchBtnClick);
            }
            
            _onDiscardDispatchEventWnd();
        }

        protected override void _onShowWndSub()
        {
            _onShowWndEventWnd();
        }

        protected override void _onHideWndSub()
        {
            _m_wCondContainer?.hideWnd();
            // _m_wHeroHeadContainer?.hideWnd();
            _m_wEventIcon?.hideWnd();

            _m_lConditionShowInfo?.Clear();
            _m_lHeroSatisfyConditionCountList.Clear();

            _onHideWndEventWnd();
        }

        protected override void _onResetSub()
        {
            _m_wCondContainer?.resetWnd();
            // _m_wHeroHeadContainer?.resetWnd();
            _m_wEventIcon?.discardTexture();

            _m_lConditionShowInfo?.Clear();
            _m_lHeroSatisfyConditionCountList.Clear();

            _onResetEventWnd();
        }

        protected override void _onSetEventData(_ACommonSimpleEventAgent _commonEventAgent)
        {
            if (!(_commonEventAgent is CommonSimpleDispatchEventAgent))
            {
                Debug.LogError("[GGUIWndCommonSimpleDispatchEventSelectHero _onSetEventData] 传入参数_commonEventAgent 不是 CommonSimpleDispatchEventAgent 类型");
            }
            
            _m_iDispatchEventShowAgent = _commonEventAgent as CommonSimpleDispatchEventAgent;

            _initConditionShowInfo();
            _initHeroSatisfyConditionCountList();
            
            _onSetEventDataSubWnd();
        }

        protected override void _refreshWndSub()
        {
            if (wnd != null && _m_iDispatchEventShowAgent != null)
            {
                if (_m_wEventIcon != null)
                {
                    _m_wEventIcon.showWnd();
                    _m_wEventIcon.setTexture(_m_iDispatchEventShowAgent.CommonEventDispatchShowRefObj?.detail_icon);
                }
            }

            _refreshCondContainer(true);
            _refreshHeroHeadContainer();
            _refreshDispatchBtn();
            
            _refreshWndEventWnd();
        }

        protected override void _doCloseNode()
        {
            GNodeCommonDispatchEvent.doCloseNode(true);
        }

        /// <summary>
        /// 初始化条件显示信息
        /// </summary>
        private void _initConditionShowInfo()
        {
            if (_m_lConditionShowInfo == null)
                _m_lConditionShowInfo = new List<_IConditionDescShow>();
            _m_lConditionShowInfo.Clear();

            if (_m_iDispatchEventShowAgent == null || _m_iDispatchEventShowAgent.dispatchConditionRefObjList == null)
                return;

            foreach (CommonEventDispatchCondRefObj conditionRefObj in _m_iDispatchEventShowAgent.dispatchConditionRefObjList)
            {
                if(conditionRefObj == null)
                    continue;
                
                _m_lConditionShowInfo.Add(new CommonDispatchEventConditionShowInfo(conditionRefObj, selectedHeroShowList));
            }
        }
        
        /// <summary>
        /// 初始化大臣满足条件数量列表
        /// </summary>
        private void _initHeroSatisfyConditionCountList()
        {
            _m_lHeroSatisfyConditionCountList.Clear();
            if (_m_iDispatchEventShowAgent == null)
                return;

            List<_IHeroCardShow> allHeroShowInfoList = new List<_IHeroCardShow>();
            NPPlayer.instance.heroComponent.dealAllHero((_heroInfo) =>
            {
                if(_heroInfo != null)
                    allHeroShowInfoList.Add(_heroInfo);
            });
            
            _m_iDispatchEventShowAgent.getHeroSatisfyConditionCountList(allHeroShowInfoList, _m_lHeroSatisfyConditionCountList);
        }
        
        protected _IHeroCardShow _findHeroShowInfo(List<_IHeroCardShow> _heroShowInfoList, long _heroId)
        {
            if (_heroShowInfoList == null)
                return null;
            
            return _heroShowInfoList.Find((_heroShowInfo) =>
            {
                if (_heroShowInfo != null && _heroShowInfo.heroRefObj != null && _heroShowInfo.heroRefObj.id == _heroId)
                    return true;

                return false;
            });
        }

        protected bool _findHeroShowInfo(List<HeroSatisfyConditionCount> _heroShowInfoList, long _heroId, out HeroSatisfyConditionCount _heroSatisfyConditionCount)
        {
            _heroSatisfyConditionCount = new HeroSatisfyConditionCount();
            if (_heroShowInfoList == null)
                return false;

            foreach (HeroSatisfyConditionCount item in _heroShowInfoList)
            {
                if (item.heroShowInfo != null && item.heroShowInfo.heroRefObj != null &&
                    item.heroShowInfo.heroRefObj.id == _heroId)
                {
                    _heroSatisfyConditionCount = item;
                    return true;
                }
            }

            return false;
        }
        
        #region 刷新窗口方法

        /// <summary>
        /// 刷新条件列表
        /// </summary>
        protected void _refreshCondContainer(bool _resetData)
        {
            if (_m_wCondContainer != null)
            {
                _m_wCondContainer.showWnd();
                
                if(_resetData)
                    _m_wCondContainer.setShowData(_m_lConditionShowInfo);
                else//
                    _m_wCondContainer.iterateShowItemWnd((_item) =>
                    {
                        if(_item != null)
                            _item.refreshWnd(false);

                        return true;
                    });
            }
        }
        
        /// <summary>
        /// 刷新选中大臣icon列表
        /// </summary>
        protected void _refreshHeroHeadContainer()
        {
            // if (_m_wHeroHeadContainer != null)
            // {
            //     _m_wHeroHeadContainer.showWnd();
            //     _m_wHeroHeadContainer.setShowList(selectedHeroShowList, _m_iDispatchEventShowAgent?.canDispatchMaxHeroNum ?? 0);
            // }

            if (wnd != null)
            {
                bool hasSelectHero = selectedHeroShowList.Count > 0;
                ALUGUICommon.setGameObjEnable(wnd.hasSelectHeroShowGoList, hasSelectHero);
                ALUGUICommon.setGameObjEnable(wnd.hasSelectHeroHideGoList, !hasSelectHero);
            }
        }
        
        /// <summary>
        /// 刷新派遣按钮
        /// </summary>
        protected void _refreshDispatchBtn()
        {
            if (_m_iDispatchEventShowAgent == null || wnd == null)
                return;
            
            bool akeyDealIsUnlock = GCommon.isSimpleUnlock(_m_iDispatchEventShowAgent.akeyDispatchSimpleUnlockId);
            
            ALUGUICommon.setGameObjEnable(wnd.akeyDealLockShowGoList, !akeyDealIsUnlock);
            ALUGUICommon.setGameObjEnable(wnd.akeyDealUnlockShowGoList, !akeyDealIsUnlock);
            if (akeyDealIsUnlock)
            {
                GGameCommonInfo.disgrayImage(wnd.akeyDealLockGrayList);
            }
            else
            {
                GGameCommonInfo.grayImage(wnd.akeyDealLockGrayList);
            }

            if (akeyDealIsUnlock && selectedHeroShowList.Count <= 0)//若一键派遣已经解锁且未选中任何大臣时
            {
                ALUGUICommon.setGameObjEnable(wnd.btnDispatch, false);
                ALUGUICommon.setGameObjEnable(wnd.btnAkeyDispatch, true);
            }
            else
            {
                ALUGUICommon.setGameObjEnable(wnd.btnDispatch, true);
                ALUGUICommon.setGameObjEnable(wnd.btnAkeyDispatch, false);
            }
        }

        #endregion

        #region 窗口事件

        /// <summary>
        /// 当点击大臣头像icon时
        /// </summary>
        // protected void _onClickHeroHeadIcon(GGUIWndHeroIconNullableItem _item)
        // {
        //     _onClickHeroHeadIconSub(_item);
        // }

        /// <summary>
        /// 当派遣按钮被点击时
        /// </summary>
        /// <param name="_go"></param>
        protected virtual void _onDispatchBtnClick(GameObject _go)
        {
            if(_m_iDispatchEventShowAgent == null)
                return;
            
            List<long> selectHeroIdList = new List<long>();
            foreach (var heroShowInfo in selectedHeroShowList)
            {
                if(heroShowInfo == null || heroShowInfo.heroRefObj == null)
                    continue;
                
                selectHeroIdList.Add(heroShowInfo.heroRefObj.id);
            }

            Action _afterShowTip = () =>
            {
                _m_iDispatchEventShowAgent.reqDealEvent(selectHeroIdList,
                    (_isSucc, _eventDoneInfo) =>
                    {
                        if(!_isSucc || _eventDoneInfo == null)
                            return;

                        // _doCloseNode();//直接关闭窗口, 结果在Node的onCloseNode中展示
                        _triggerEventDealDone();
                    });
            };

            if (AccountSettingMgr.instance.warningTipSaver.needShowWarningTip(_m_iDispatchEventShowAgent.dispatchEvent_NotAllConditionEnable_WaringTip))
            {
                if (!_m_iDispatchEventShowAgent.allConditionEnable(selectHeroIdList))
                {
                    NPMesMgr.instance.showWarningTipMes(_afterShowTip, null, _m_iDispatchEventShowAgent.dispatchEvent_NotAllConditionEnable_WaringTip, null, TransKeyConst.commonEvent_dispatchEventNotAllCondEnableTip_none);
                }
                else
                {
                    _afterShowTip();
                }
            }
            else
            {
                _afterShowTip();
            }
        }

        /// <summary>
        /// 关闭按钮被点击
        /// </summary>
        /// <param name="_go"></param>
        protected void _onCloseBtnClick(GameObject _go)
        {
            GNodeCommonDispatchEvent.doCloseNode(true);
        }
        
        /// <summary>
        /// 一键派遣按钮被点击
        /// </summary>
        protected virtual void _onAKeyDispatchBtnClick(GameObject _go)
        {
            if (_m_iDispatchEventShowAgent == null)
                return;
            
            // bool akeyDealIsUnlock = _m_iDispatchEventShowAgent.akeyDispatchCondition == null || _m_iDispatchEventShowAgent.akeyDispatchCondition.isEmpty || _m_iDispatchEventShowAgent.akeyDispatchCondition.IsEnable(null);
            // if (!akeyDealIsUnlock)//未解锁的提示
            // {
            //     NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.commonEvent_dispatchEventAkeyFunctionLock_none);
            //     return;
            // }

            if (!GCommon.isSimpleUnlock(_m_iDispatchEventShowAgent.akeyDispatchSimpleUnlockId, true))
            {
                return;
            }
            
            selectedHeroShowList.Clear();
            int bestSchemeSatisfyCondCount = 0;

            _m_iDispatchEventShowAgent.findBestDispatchScheme(_m_lHeroSatisfyConditionCountList, false, selectedHeroShowList, true, ref bestSchemeSatisfyCondCount);

            _refreshCondContainer(false);//刷新条件Container
            _refreshHeroHeadContainer();//刷新选中大臣Head列表
            _refreshDispatchBtn();//刷新派遣按钮

            _onAKeyDispatchBtnClickSub();
        }
        
        #endregion
        
        #region 子类实现方法

        protected abstract void _onInitDoneDispatchEventWnd();
        
        protected abstract void _onDiscardDispatchEventWnd();

        protected abstract void _onShowWndEventWnd();

        protected abstract void _onHideWndEventWnd();

        protected abstract void _onResetEventWnd();

        protected abstract void _onSetEventDataSubWnd();

        protected abstract void _refreshWndEventWnd();
        
        // protected abstract void _onClickHeroHeadIconSub(GGUIWndHeroIconNullableItem _item);

        protected abstract void _onAKeyDispatchBtnClickSub();
        
        #endregion
    }
}