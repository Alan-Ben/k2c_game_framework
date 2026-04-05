using ALPackage;
using UnityEngine;

/// <summary>
/// 通用物品带缓存的容器container
/// </summary>
public class NPGGUIMonoCommonItemCachedContainer : _TALUGUIMonoContainerWnd<NPGGUIMonoCommonItem>
{
    [ALHeader("每个item都将延迟展示，这里配置延迟的间隔时间")]
    public float delayShowDuration;
}

