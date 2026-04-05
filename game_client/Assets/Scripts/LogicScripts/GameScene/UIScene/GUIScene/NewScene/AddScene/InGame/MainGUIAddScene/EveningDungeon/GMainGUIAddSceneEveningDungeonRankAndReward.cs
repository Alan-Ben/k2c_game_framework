namespace GOE
{
    public class GMainGUIAddSceneEveningDungeonRankAndReward : _ANPGMainGUIAddSceneResBar<GGUIWndEveningDungeonRankAndRewardDetail>
    {
        private static GMainGUIAddSceneEveningDungeonRankAndReward _g_instance;
        public static GMainGUIAddSceneEveningDungeonRankAndReward instance { get { return _g_instance ??= new GMainGUIAddSceneEveningDungeonRankAndReward(); } }
        
        protected override GGUIWndEveningDungeonRankAndRewardDetail _m_wnd { get { return GGUIWndEveningDungeonRankAndRewardDetail.instance; } }

        public void setInfo(EEveningDungeonRankAndRewardDetailTabType _selectTabType)
        {
            GGUIWndEveningDungeonRankAndRewardDetail.instance.setInfo(_selectTabType);
        }
    }
}