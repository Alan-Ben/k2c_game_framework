using ALPackage;

/// <summary>
/// 逐个显示item的container抽象基类
/// </summary>
/// <typeparam name="_T_ITEM_MONO"></typeparam>
public abstract class _ATNPGGUIMonoAddItemContainer<_T_ITEM_MONO> : _TALUGUIMonoContainerWnd<_T_ITEM_MONO> where _T_ITEM_MONO : _AALBasicUIWndMono
{
    [ALHeader("逐个显示item的时间间隔（秒）")]
    public float showItemIntervalTime = 0.2f;
}