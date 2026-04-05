namespace ALPackage
{
    /// <summary>
    /// ALPackage内部的消息，约定消息号100以下，是内部使用？
    /// 业务用的消息，自己定义一个新的XXMsgType枚举，只能用100以上的int
    /// </summary>
    public enum ALMsgType
    {
        NONE,       //默认无效字段
        UI_CLICK, //UI点击处理
    }
}