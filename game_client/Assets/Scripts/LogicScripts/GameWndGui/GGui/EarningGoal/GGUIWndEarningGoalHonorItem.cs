using System;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// item
    /// </summary>
    public class GGUIWndEarningGoalHonorItem : _ATALBasicUISubWnd<GGUIMonoEarningGoalHonorItem>
    {
        private EarningGoalHonorRewardRefObj _m_data;
        private GGUIWndCommonRewardContainer _m_wItemContainer;
        private NPGGUIWndPlayerIcon _m_firstPlayerInfo; //首达玩家信息
        private EEarningGoalHonorGetStat _m_curState;
        public GGUIWndEarningGoalHonorItem(GGUIMonoEarningGoalHonorItem _wnd) : base(_wnd)
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
            ALUGUICommon.combineBtnClick(wnd.btnGet, _onBtnGetClick);
        }

        public void setInfo(EarningGoalHonorRewardRefObj _data)
        {
            _m_data = _data;
            _refreshWnd();
        }

        private void _refreshWnd()
        {
            if (null == wnd || null == _m_data)
                return;
            long firstPlayerId = NPPlayer.instance.earningGoalComp.honorAchievePlayerCid(_m_data.id, out long timeStamp);
            bool hadAchieve = firstPlayerId > 0;
            bool canDraw = firstPlayerId == NPPlayer.instance.playerInfo.CID;

            bool hadDraw = NPPlayer.instance.earningGoalComp.hasDrawHonorReward(_m_data.id);

            if (hadAchieve)
                if (canDraw)
                    if(hadDraw)
                        _m_curState = EEarningGoalHonorGetStat.HadGet;
                    else
                        _m_curState = EEarningGoalHonorGetStat.CanGet;
                else
                    _m_curState = EEarningGoalHonorGetStat.None;
            else
                _m_curState = EEarningGoalHonorGetStat.Wait;

            if (hadAchieve)
            {
                _m_firstPlayerInfo?.showWnd();
                _m_firstPlayerInfo?.setPlayer(firstPlayerId);
            }
            else
            {
                _m_firstPlayerInfo?.hideWnd();
            }      
            _m_wItemContainer?.showWnd();
            ECommonRewardType rewardType = canDraw ? hadDraw ? ECommonRewardType.HAS_GET_REWARD :ECommonRewardType.CAN_GET_REWARD : ECommonRewardType.NOT_GET_REWARD;
            _m_wItemContainer?.setRewardList(_m_data.first_gain_item_list, rewardType);

            if (wnd.specialShows != null)
                foreach (var specialShow in wnd.specialShows)
                {
                    if (specialShow != null)
                    {
                        ALUGUICommon.setGameObjEnable(specialShow.specialShowGos, specialShow.earningGoalHonorId == _m_data.id);
                    }
                }

            ALUGUICommon.setLabelTxt(wnd.txtTitle,TextTranslate.instance.getLanguage(TransKeyConst.earning_goal_honor_item_title, _m_data.earning_goal.ToLargeString(PrimitiveExtension.ELargeStringType.GOLD)));
            ALUGUICommon.setLabelTxt(wnd.txtProcess,
                TextTranslate.instance.getLanguage(TransKeyConst.earning_goal_honor_process_num_num,
                    NPPlayer.instance.specialItemComp.goldData.earnings.ToLargeString(PrimitiveExtension.ELargeStringType.GOLD),
                    _m_data.earning_goal.ToLargeString(PrimitiveExtension.ELargeStringType.GOLD)));
            NPCommonEnumStatInfo<EEarningGoalHonorGetStat>.setStat(wnd.statInfos, _m_curState);
        }
        

        private void _onBtnGetClick(GameObject _)
        {
            if (null == wnd || _m_data == null)
                return;
            if (_m_curState == EEarningGoalHonorGetStat.CanGet)
            {
                NPPlayer.instance.earningGoalComp.reqEarningsGoalDrawHonorReward(_m_data.id, _suc =>
                {
                    _refreshWnd();
                } );
            }
        }
    }
}
