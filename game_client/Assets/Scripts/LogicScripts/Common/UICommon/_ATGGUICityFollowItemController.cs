using ALPackage;

namespace GOE
{
    /// <summary>
    /// 主城 followitem 的通用Controller，有监听进入op的时候隐藏
    /// </summary>
    public abstract class _ATGGUICityFollowItemController<T, W> : _ATALGGUICommonFollowItemController<T,W>
        where T : ALGGUIMonoCommonFollowItem
        where W : _ATALGGUIWndCommonFollowItem<T>
    {
    }
}