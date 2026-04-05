namespace GOE.MiniGame
{
    public enum EPuzzleGameState
    {
        NONE,//空状态， 不在游戏中状态
        IDLE,//空闲状态
        WAITING_MATCH,//等待匹配状态
        MATCH_SUCCESS,//匹配成功状态
        MATCH_FAIL_RESUME,//匹配失败恢复状态
        GAME_SUCCESS,//游戏成功状态
    }
}