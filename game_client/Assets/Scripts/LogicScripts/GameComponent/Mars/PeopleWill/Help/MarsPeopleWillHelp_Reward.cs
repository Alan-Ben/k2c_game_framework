using Common.MarsObj;

namespace GOE
{
    /// <summary>
    /// 奖励求助
    /// </summary>
    public class MarsPeopleWillHelp_Reward : _ABaseMarsPeopleWillHelp
    {
        private MarsPeopleRewardHelpRefObj _m_RewardHelpRefObj;//配表数据
        
        public MarsPeopleWillHelp_Reward(Mars_Help _serverHelpInfo) : base(_serverHelpInfo)
        {
        }

        public MarsPeopleRewardHelpRefObj rewardHelpRefObj
        {
            get
            {
                if(_m_RewardHelpRefObj == null || _m_RewardHelpRefObj.id != _m_refId)
                    _m_RewardHelpRefObj = GRefdataCoreMgr.instance.marsPeopleRewardHelpRefCore.getRef(_m_refId);
                return _m_RewardHelpRefObj;
            }
        }

        public override void _onUpdate(Mars_Help _serverHelpInfo)
        {
            if(_serverHelpInfo == null)
                return;
        }

        public override void deal()
        {
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndMarsPopularWillRewardHelpDeal.instance, () =>
            {
                GGUIWndMarsPopularWillRewardHelpDeal.instance.setData(this);
                GGUIWndMarsPopularWillRewardHelpDeal.instance.showWnd();
            }, UINodeTagConst.C_MARS_POPULAR_WILL_REWARD_HELP_DEAL);
        }
    }
}