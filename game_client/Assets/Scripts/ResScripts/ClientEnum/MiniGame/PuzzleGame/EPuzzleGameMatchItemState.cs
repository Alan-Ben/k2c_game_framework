namespace GOE.MiniGame
{
    public enum EPuzzleGameMatchItemState
    {
        NONE,//空状态
        IDLE,//空闲状态
        NO_ITEM_WAITING_MATCH,//没有item在等待匹配状态
        WRONG_ITEM_WAITING_MATCH,//错误item在等待匹配状态
        RIGHT_ITEM_WAITING_MATCH,//正确item在等待匹配状态
        MATCH_SUCCESS,//匹配成功状态
        MATCH_FAIL_RESUME,//匹配失败恢复状态
        NO_MATCH_RESUME,//没有匹配后恢复状态
    }
}