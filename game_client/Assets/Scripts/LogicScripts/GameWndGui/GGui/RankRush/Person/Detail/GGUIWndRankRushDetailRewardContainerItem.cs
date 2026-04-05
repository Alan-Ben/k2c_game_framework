using ALPackage;
using System.Collections.Generic;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 冲榜详情奖励页面列表item
    /// </summary>
    public class GGUIWndRankRushDetailRewardContainerItem : _ATALBasicUISubWnd<GGUIMonoRankRushDetailRewardContainerItem>
    {
        //奖励列表
        private NPGGUIWndCommonItemContainer _m_wItemContainer;
        //奖励信息
        private GActivityRankRewardRefObj _m_rankRewardRef;

        public GGUIWndRankRushDetailRewardContainerItem(GGUIMonoRankRushDetailRewardContainerItem _wnd)
            : base(_wnd)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wItemContainer?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wItemContainer?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_wItemContainer?.discard();
            _m_wItemContainer = null;
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoItemContainer != null)
                _m_wItemContainer = new NPGGUIWndCommonItemContainer(wnd.monoItemContainer);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_refObj"></param>
        public void setInfo(GActivityRankRewardRefObj _refObj)
        {
            if (wnd == null || _refObj == null)
                return;

            _m_rankRewardRef = _refObj;

            //设置初始显隐
            ALUGUICommon.setGameObjEnable(wnd.goParticipationAwardHideList, true);
            ALUGUICommon.setGameObjEnable(wnd.goParticipationAwardShowList, false);

            if (_refObj.rank_begin == _refObj.rank_end)
            {
                ALUGUICommon.setLabelTxt(wnd.txtRankingDesc, TextTranslate.instance.getLanguage(TransKeyConst.rankRush_rewardRanking_num, _refObj.rank_begin));
                //设置排名的显隐
                wnd.setRankShow(_refObj.rank_begin);
            }
            else if (_refObj.rank_end != -1)
            {
                ALUGUICommon.setLabelTxt(wnd.txtRankingDesc, TextTranslate.instance.getLanguage(TransKeyConst.rankRush_rewardRankingInterval_num_num, _refObj.rank_begin, _refObj.rank_end));
                //设置排名的显隐
                wnd.setRankShow(-1);
            }
            else
            {
                //是参与奖
                ALUGUICommon.setLabelTxt(wnd.txtRankingDesc, TextTranslate.instance.getLanguage(TransKeyConst.rankRush_participationAward_none));
                //设置参与奖的显隐
                ALUGUICommon.setGameObjEnable(wnd.goParticipationAwardHideList, false);
                ALUGUICommon.setGameObjEnable(wnd.goParticipationAwardShowList, true);
                //设置排名的显隐
                wnd.setRankShow(-1);
            }

            //设置奖励列表
            List<NPCommonCostItem> rewardItemList = new List<NPCommonCostItem>();
            if(_refObj.reward_item_list != null)
                rewardItemList.AddRange(_refObj.reward_item_list);
            //称号奖励放到最后
            if (_refObj.title_reward != null && _refObj.title_reward.title_reward_id > 0)
                rewardItemList.Add(new NPCommonCostItem(ENPItemType.TITLE, _refObj.title_reward.title_reward_id, _refObj.title_reward.liftTs));
            _m_wItemContainer?.showWnd();
            _m_wItemContainer?.showItemList(rewardItemList);

            ALUGUICommon.setGameObjEnable(wnd.goSelfRankHideList,true);
            ALUGUICommon.setGameObjEnable(wnd.goSelfRankShowList,false);
        }

        /// <summary>
        /// 根据自己的排名刷新显隐
        /// </summary>
        /// <param name="_selfRank"></param>
        public void refreshBySelfRank(long _selfRank)
        {
            if (wnd == null || _m_rankRewardRef == null)
                return;

            bool isInCurRankReward = _selfRank >= _m_rankRewardRef.rank_begin && (_selfRank <= _m_rankRewardRef.rank_end || _m_rankRewardRef.rank_end == -1);

            ALUGUICommon.setGameObjEnable(wnd.goSelfRankHideList, !isInCurRankReward);
            ALUGUICommon.setGameObjEnable(wnd.goSelfRankShowList, isInCurRankReward);
        }
    }
}
