using System;
using System.Collections.Generic;
using ALPackage;
using NPEnum;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 限时兑换
    /// </summary>
    public class GGUIWndRushExchangeMain : _ATALBasicUIWnd<GGUIMonoRushExchangeMain>
    {
        private static GGUIWndRushExchangeMain _g_instance = new GGUIWndRushExchangeMain();
    
        public static GGUIWndRushExchangeMain instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndRushExchangeMain();
                return _g_instance;
            }
        }
        
        // <AutoGen:WndDeclaration>
        private NPGGUIWndCommonItemContainer _m_costItemsWnd;  // 消耗物品
        private NPGGUIWndCommonItemContainer _m_rewardItemsWnd;  // 获得物品
        private NPGGUIWndCommonToggleEx _m_toggleUseGemWnd;  // 钻石补充开关
        // </AutoGen:WndDeclaration>
        private int _m_timeDownSer;
        private ERushExchangeState _m_curState;
        private long _m_timeCdMs;
        private string _m_timeCdKey;
        private bool _m_useGemSupplement = false;

        public GGUIWndRushExchangeMain() : base(EALUIWndLayer.ADDITION)
        {
        }
    
        protected override string _monoAssetPath { get => GGUIMonoRushExchangeMain.assetPath; }
        protected override string _monoObjName { get => GGUIMonoRushExchangeMain.objName; }
        protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }
    
        protected override void _onShowWnd()
        {
            _refreshWnd();
            WinMsg.RegisterMsgAct(WinMsgType.ON_RUSH_EXCHANGE_CHG, _refreshWnd);
            RushExchangeRefObj rushExchangeRef = GRefdataCoreMgr.instance.rushExchangeRefCore.getRef(NPPlayer.instance.rushExchangeComp.rushExchangeRefId);
            if (rushExchangeRef == null)
                NPPlayer.instance.rushExchangeComp.checkRefresh();
        }
    
        protected override void _onHideWnd()
        {
            _m_costItemsWnd?.hideWnd();
            _m_rewardItemsWnd?.hideWnd();
            WinMsg.UnregisterMsgAct(WinMsgType.ON_RUSH_EXCHANGE_CHG, _refreshWnd);
        }
    
        protected override void _onReset()
        {
            _m_costItemsWnd?.resetWnd();
            _m_rewardItemsWnd?.resetWnd();
        }
    
        protected override void _onDiscard()
        {
            _m_costItemsWnd?.discard();
            _m_costItemsWnd = null;
            _m_rewardItemsWnd?.discard();
            _m_rewardItemsWnd = null;
            _m_toggleUseGemWnd?.discard();
            _m_toggleUseGemWnd = null;
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickbtnClose);
                ALUGUICommon.uncombineBtnClick(wnd.btnExchange, _onClickbtnExchange);
                ALUGUICommon.uncombineBtnClick(wnd.btnReward, _onClickbtnReward);
            }
        }
    
        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
                
            if (wnd.costItems != null)
                _m_costItemsWnd = new NPGGUIWndCommonItemContainer(wnd.costItems);
            if (wnd.rewardItems != null)
                _m_rewardItemsWnd = new NPGGUIWndCommonItemContainer(wnd.rewardItems);
            if (wnd.toggleUseGem != null)
            {
                _m_toggleUseGemWnd = new NPGGUIWndCommonToggleEx(wnd.toggleUseGem);
                _m_toggleUseGemWnd.clickDelegate += _onToggleClicktoggleUseGem;
            }
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickbtnClose);
            ALUGUICommon.combineBtnClick(wnd.btnExchange, _onClickbtnExchange);
            ALUGUICommon.combineBtnClick(wnd.btnReward, _onClickbtnReward);
        }
        
        
    
        /// <summary>
        /// 刷新界面
        /// </summary>
        
        private void _refreshWnd()
        {
            if(null == wnd)
                return;
            RushExchangeRefObj rushExchangeRef = GRefdataCoreMgr.instance.rushExchangeRefCore.getRef(NPPlayer.instance.rushExchangeComp.rushExchangeRefId);
            if (rushExchangeRef == null)
                return;
            
            if(_m_costItemsWnd != null)
            {
                _m_costItemsWnd.showWnd();
                _m_costItemsWnd.showItemList(rushExchangeRef.cost_list);
            }
            if(_m_rewardItemsWnd != null)
            {
                _m_rewardItemsWnd.showWnd();
                _m_rewardItemsWnd.showItemList(rushExchangeRef.reward_list);
            }
            
            _m_timeCdMs = NPPlayer.instance.rushExchangeComp.rushExchangeShowTime;
            _m_curState = NPPlayer.instance.rushExchangeComp.rushExchangeState;
            
            switch (_m_curState)
            {
                case ERushExchangeState.Ready:
                    _m_timeCdKey = TransKeyConst.rush_exchange_refresh_time_cd_desc;
                    break;
                case ERushExchangeState.Exchange:
                    _m_timeCdKey = TransKeyConst.rush_exchange_exchanging_time_cd_desc;
                    break;
                case ERushExchangeState.Reward:
                    _m_timeCdKey = TransKeyConst.rush_exchange_wait_time_cd_desc;
                    break;
                case ERushExchangeState.Wait:
                    _m_timeCdKey = TransKeyConst.rush_exchange_wait_time_cd_desc;
                    break;
            }
            RushExchangeStateInfo.setState(wnd.stateInfoList, _m_curState);

            long needGemCount = needGemSupplementCount(rushExchangeRef, out NPCommonItem firstNoEnoughItem);

            if (_m_useGemSupplement && needGemCount <= 0)
                _m_useGemSupplement = false;
                
            if(_m_toggleUseGemWnd != null)
            {
                _m_toggleUseGemWnd.setSelected(_m_useGemSupplement);
            }
            NPCommonCostItem costItem = new NPCommonCostItem(ENPItemType.CURRENCY, (long)CommonEnum.ECurrency.GEM, needGemCount);

            string needGemCountStr = GCommon.addColorForRichText(needGemCount.ToString(), GCommon.isItemEnough(costItem, false) ? wnd.colGemEnough : wnd.colGemNotEnough);
            
            ALUGUICommon.setLabelTxt(wnd.txtUseGemCount, TextTranslate.instance.getLanguage(TransKeyConst.rush_exchange_gem_count_desc, needGemCountStr));
            
            ALUGUICommon.setGameObjEnable(wnd.noEnoughGemShowList, needGemCount >0);
            ALUGUICommon.setGameObjEnable(wnd.noEnoughGemHideList, needGemCount <=0);
            _m_timeDownSer = ALSerializeOpMgr.next();
            _refreshTimeDown(_m_timeDownSer);
            
            ScreenBlurMgr.instance.refreshBlurRT();
        }
        /// <summary>
        /// 刷新倒计时
        /// </summary>
        private void _refreshTimeDown(int _timeDownSer)
        {
            if (null == wnd)
                return;

            if (_timeDownSer != _m_timeDownSer)
                return;
            NPPlayer.instance.rushExchangeComp.checkRefresh();

            string timeDesc = TextTranslate.instance.getLanguage(_m_timeCdKey, TimeUtil.millisecondsToTime_hms(Math.Max(0, _m_timeCdMs - FpsAndPingMgr.instance.serverTimeTag)));
            foreach (Text txtTime in wnd.txtRefreshTimeList)
            {
                ALUGUICommon.setLabelTxt(txtTime,  timeDesc);
            }

            ALCommonTaskController.CommonActionAddMonoTask(() =>
            {
                _refreshTimeDown(_timeDownSer);
            },1f);
        }

        private long needGemSupplementCount(RushExchangeRefObj _rushExchangeRef, out NPCommonItem _firstNoEnoughItem)
        {
            long needGemCount = 0;
            _firstNoEnoughItem = null;
            if (_rushExchangeRef != null && _rushExchangeRef.cost_list != null)
                foreach (NPCommonCostItem costItem in _rushExchangeRef.cost_list)
                {
                    if (costItem == null || costItem.item == null) continue;
                    long reserveCount = NPPlayer.instance.bagComp.getItemCount(costItem.item.itemId);
                    if (costItem.count > reserveCount)
                    {
                        if (_firstNoEnoughItem == null)
                            _firstNoEnoughItem = costItem.item;
                        long needCount = costItem.count - reserveCount;
                        foreach (RushExchangeItemRefObj itemValueRef in GRefdataCoreMgr.instance.rushExchangeItemRefCore.refList)
                        {
                            if (itemValueRef != null && itemValueRef.item == costItem.item)
                            {
                                needGemCount += itemValueRef.gem_count * needCount;
                            }
                        }
                    }
                }

            return needGemCount;
        }
        
        // <AutoGen:Method>
        // 关闭按钮点击事件
        private void _onClickbtnClose(GameObject go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_RUSH_EXCHANGE_MAIN);
        }
        // 确认按钮点击事件
        private void _onClickbtnExchange(GameObject go)
        {
       
            if (NPPlayer.instance.rushExchangeComp.rushExchangeState == ERushExchangeState.Ready)
            {
                RushExchangeRefObj rushExchangeRef = GRefdataCoreMgr.instance.rushExchangeRefCore.getRef(NPPlayer.instance.rushExchangeComp.rushExchangeRefId);
                long needGemCount = needGemSupplementCount(rushExchangeRef, out NPCommonItem firstNoEnoughItem);

                if (_m_useGemSupplement && needGemCount > 0)
                {
                    NPCommonCostItem costItem = new NPCommonCostItem(ENPItemType.CURRENCY, (long)CommonEnum.ECurrency.GEM, needGemCount);
                    // 检查钻石数量是否满足
                    if(!GCommon.isItemEnough(costItem, true))
                        return;

                    NPMesMgr.instance.showTwoBtnMes(TextTranslate.instance.getLanguage(TransKeyConst.rush_exchange_use_gem_confim_tip, needGemCount),
                        TextTranslate.instance.getLanguage(TransKeyConst.cancel),
                        null,
                        TextTranslate.instance.getLanguage(TransKeyConst.confirm),
                        () =>
                        {
                            NPPlayer.instance.rushExchangeComp.reqRushExchange(_m_useGemSupplement);
                        });
                    return;
                }
                else
                {
                    if (!_m_useGemSupplement && needGemCount > 0)
                    {
                        GCommon.dealItemNotEnough(firstNoEnoughItem);
                        return;
                    }

                    NPPlayer.instance.rushExchangeComp.reqRushExchange(_m_useGemSupplement);
                }
              
            }
        }

        // 确认按钮点击事件
        private void _onClickbtnReward(GameObject go)
        {
            if (NPPlayer.instance.rushExchangeComp.rushExchangeState == ERushExchangeState.Reward)
            {
                NPPlayer.instance.rushExchangeComp.reqRushExchangeReward( );
            }
        }

        private void _onToggleClicktoggleUseGem(NPGGUIWndCommonToggleEx _toggleWnd)
        {
            if (!_m_useGemSupplement)
            {
                RushExchangeRefObj rushExchangeRef = GRefdataCoreMgr.instance.rushExchangeRefCore.getRef(NPPlayer.instance.rushExchangeComp.rushExchangeRefId);
                long needGemCount = needGemSupplementCount(rushExchangeRef, out NPCommonItem firstNoEnoughItem);

                NPCommonCostItem costItem = new NPCommonCostItem(ENPItemType.CURRENCY, (long)CommonEnum.ECurrency.GEM, needGemCount);
                // 检查钻石数量是否满足
                if(!GCommon.isItemEnough(costItem, true))
                    return;
                
         
                _m_useGemSupplement =true;
                _m_toggleUseGemWnd?.setSelected(_m_useGemSupplement);
        
            }
            else
            {
                _m_useGemSupplement =false;
                _m_toggleUseGemWnd?.setSelected(_m_useGemSupplement);
            }
        

        }
        // </AutoGen:Method>
    }
}