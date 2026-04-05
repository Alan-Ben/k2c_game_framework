using System;
using System.Collections.Generic;
using ALPackage;
using Common.DinnerObj;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// item
    /// </summary>
    public class GGUIWndEarningGoalGlobalItem : _ANPGGUIBasicGridItemWnd<GGUIMonoEarningGoalGlobalItem>
    {
        private EarningGoalRewardRefObj _m_data;
        private GGUIWndCommonRewardContainer _m_wFirstItemContainer;
        private GGUIWndCommonRewardContainer _m_wItemContainer;
        private NPGGUIWndPlayerIcon _m_firstPlayerInfo; //首达玩家信息

        private EEarningGoalGlobalGetStat _m_curState;
        public GGUIWndEarningGoalGlobalItem(GGUIMonoEarningGoalGlobalItem _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
            
        }

        protected override void _onHideWnd()
        {
            
        }

        protected override void _onReset()
        {
        }

        protected override void _onDiscard()
        {
            _m_firstPlayerInfo?.discard();
            _m_firstPlayerInfo = null;
            _m_wFirstItemContainer?.discard();
            _m_wFirstItemContainer = null;
            _m_wItemContainer?.discard();
            _m_wItemContainer = null;
            if (wnd == null) return;
            ALUGUICommon.uncombineBtnClick(wnd.btnGet, _onBtnGetClick);
        }

        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
            if (wnd.firstPlayerInfo !=null) 
                _m_firstPlayerInfo = new NPGGUIWndPlayerIcon(wnd.firstPlayerInfo);
            if(wnd.itemContainer != null)
                _m_wItemContainer = new GGUIWndCommonRewardContainer(wnd.itemContainer);
            if(wnd.firstItemContainer != null)
                _m_wFirstItemContainer = new GGUIWndCommonRewardContainer(wnd.firstItemContainer);
            
            ALUGUICommon.combineBtnClick(wnd.btnGet, _onBtnGetClick);
        }

        protected override void _resetGridItem()
        {
            _m_firstPlayerInfo?.hideWnd();
        }

        /// <summary>
        /// 设置显示信息
        /// </summary>
        public void setInfo(EarningGoalRewardRefObj _data)
        {
            _m_data = _data;
            _refreshWnd();
        }

        /// <summary>
        /// 刷新界面显示
        /// </summary>
        private void _refreshWnd()
        {
            if (null == wnd || _m_data == null)
                return;
            bool hadDraw = NPPlayer.instance.earningGoalComp.hasDrawReward(_m_data.id);
            long firstPlayerId = NPPlayer.instance.earningGoalComp.firstAchievePlayerCid(_m_data.id);
            bool hasAchieve = firstPlayerId > 0;
            if (hasAchieve)
            {
                _m_firstPlayerInfo?.showWnd();
                _m_firstPlayerInfo?.setPlayer(firstPlayerId);
            }
            else
            {
                _m_firstPlayerInfo?.hideWnd();
            }
            
            _m_curState = hadDraw ? EEarningGoalGlobalGetStat.HadGet : hasAchieve ? EEarningGoalGlobalGetStat.CanGet : EEarningGoalGlobalGetStat.None;
            ECommonRewardType firstRewardType = hasAchieve && firstPlayerId == NPPlayer.instance.playerInfo?.CID ? hadDraw ? ECommonRewardType.HAS_GET_REWARD :ECommonRewardType.CAN_GET_REWARD : ECommonRewardType.NOT_GET_REWARD;
            _m_wFirstItemContainer?.showWnd();
            _m_wFirstItemContainer?.setRewardList(_m_data.first_gain_item_list, firstRewardType);
            
            ECommonRewardType rewardType = hasAchieve ? hadDraw ? ECommonRewardType.HAS_GET_REWARD :ECommonRewardType.CAN_GET_REWARD : ECommonRewardType.NOT_GET_REWARD;
            _m_wItemContainer?.showWnd();
            _m_wItemContainer?.setRewardList(_m_data.all_gain_item_list, rewardType);   
            
            ALUGUICommon.setLabelTxt(wnd.txtTitle,TextTranslate.instance.getLanguage(TransKeyConst.earning_goal_global_item_title, _m_data.earning_goal.ToLargeString(PrimitiveExtension.ELargeStringType.GOLD)));

            ALUGUICommon.setGameObjEnable(wnd.hasAchieveShowGos, hasAchieve);
            ALUGUICommon.setGameObjEnable(wnd.hasAchieveHideGos, !hasAchieve);
            NPCommonEnumStatInfo<EEarningGoalGlobalGetStat>.setStat(wnd.statInfos, _m_curState);    

        }

        private void _onBtnGetClick(GameObject _)
        {
            if (null == wnd || _m_data == null)
                return;
            if (_m_curState == EEarningGoalGlobalGetStat.CanGet)
            {
                NPPlayer.instance.earningGoalComp.reqEarningsGoalDrawReward(_m_data.id, _suc =>
                {
                    _refreshWnd();
                } );
            }
        }
    }
}
