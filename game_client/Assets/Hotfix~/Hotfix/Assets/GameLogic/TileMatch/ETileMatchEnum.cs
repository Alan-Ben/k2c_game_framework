namespace Hotfix
{
    public enum ETileMatchGameState
    {
        None = 0,
        Init = 1, //初始化
        Idle = 2, //待机
        Reorder = 3, //重刷
        Switch = 4, //交换状态
        Tips = 5, //提示状态
        Died = 6, //死局状态
        DealServer = 7, //处理服务器交互状态 
    }
    
    public enum ETileMatchDirection
    {
        None,
        Left,//左
        Right,//右
        Up,//上
        Down//下
    }
}