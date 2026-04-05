using ALPackage;
using NPEnum;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 联盟冲榜详情奖励页面列表item
    /// </summary>
    public class GGUIWndGuildRankRushDetailRewardContainerItem : _ATALBasicUISubWnd<GGUIMonoGuildRankRushDetailRewardContainerItem>
    {
        //盟主奖励列表
        private NPGGUIWndCommonItemContainer _m_wLeaderItemContainer;
        //成员奖励列表
        private NPGGUIWndCommonItemContainer _m_wMemberItemContainer;
        //奖励信息
        private GActivityRankRewardRefObj _m_rankRewardRef;

        public GGUIWndGuildRankRushDetailRewardContainerItem(GGUIMonoGuildRankRushDetailRewardContainerItem _wnd)
            : base(_wnd)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wLeaderItemContainer?.hideWnd();
            _m_wMemberItemContainer?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wLeaderItemContainer?.resetWnd();
            _m_wMemberItemContainer?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_wLeaderItemContainer?.discard();
            _m_wLeaderItemContainer = null;
            _m_wMemberItemContainer?.discard();
            _m_wMemberItemContainer = null;
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoLeaderItemContainer != null)
                _m_wLeaderItemContainer = new NPGGUIWndCommonItemContainer(wnd.monoLeaderItemContainer);

            if (wnd.monoMemberItemContainer != null)
                _m_wMemberItemContainer = new NPGGUIWndCommonItemContainer(wnd.monoMemberItemContainer);
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
            _m_wLeaderItemContainer?.showWnd();
            _m_wLeaderItemContainer?.showItemList(rewardItemList);

            //盟主奖励列表
            _m_wMemberItemContainer?.showWnd();
            _m_wMemberItemContainer?.showItemList(_refObj.member_reward_item_list);

            ALUGUICommon.setGameObjEnable(wnd.goSelfRankHideList,true);
            ALUGUICommon.setGameObjEnable(wnd.goSelfRankShowList,false);

            //刷新容器布局
            _refreshContentLayout();
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

        /// <summary>
        /// 刷新容器布局
        /// </summary>
        public void _refreshContentLayout()
        {
            ALCommonActionMonoTask.addNextFrameTask(() =>
            {
                if (wnd == null || wnd.rewardContainer == null)
                    return;

                LayoutRebuilder.ForceRebuildLayoutImmediate(wnd.rewardContainer);
            });
        }
    }
}
