namespace GOE
{
    /// <summary>
    /// 按序取东西游戏物品状态
    /// </summary>
    public enum ETakeThingsSequentiallyGameThingState
    {
        NONE,//空状态, 不在游戏中状态
        NOT_TAKEABLE,//不可取状态
        TAKEABLE,//可取状态
        SUCCESSFULLY_TAKEN,//成功拿走状态
    }
}