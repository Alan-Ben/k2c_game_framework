namespace GOE
{
    public interface _IMarsExploreTeamSelectDealer
    {
        long targetPower { get; }
        
        /// <summary>
        /// 选择完成队伍之后的处理
        /// </summary>
        void dealSelectTeam(long _teamId);

        /// <summary>
        /// 关闭当前窗口的处理
        /// </summary>
        void closeWnd();

        /// <summary>
        /// 在选择队伍界面显隐的时候的事件函数
        /// </summary>
        void onTeamSelectWndShow();
        void onTeamSelectWndHide();
    }
}
